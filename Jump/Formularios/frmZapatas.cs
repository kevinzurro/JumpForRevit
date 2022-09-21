using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Resources;
using System.Diagnostics;
using System.Windows.Forms;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.Structure;

namespace Jump
{
    public partial class frmZapatas : System.Windows.Forms.Form
    {
        // Variable necesarias
        string IdiomaDelPrograma;
        Document doc;
        UIDocument uiDoc;
        string transaccionGrupoImagenPreview = "grupo vista previa";
        string transaccionImagenPreview = "vista previa";
        int posicionImagenPreview = 0;
        TransactionGroup tg;

        // Parámetros generales
        List<Element> listaZapatas = new List<Element>();
        List<Element> elementos = new List<Element>();
        List<Parameter> parametrosEjemplar = new List<Parameter>();
        List<FamilySymbol> etiquetasBase = new List<FamilySymbol>();
        List<FamilySymbol> etiquetasArmaduras = new List<FamilySymbol>();
        List<TextNoteType> etiquetasLongitud = new List<TextNoteType>();
        List<DimensionType> cotasLineales = new List<DimensionType>();
        List<SpotDimensionType> cotasElevacion = new List<SpotDimensionType>();

        // Lista de etiquetas creadas en la vista
        List<Element> listaEtiquetasCreadas = new List<Element>();

        // DataGridView de los díametros y estilos de líneas
        DataGridView dgvEstiloLinea = new DataGridView();

        // Parámetros para las cimentaciones
        Type clase = typeof(FamilyInstance);
        BuiltInCategory categoria = BuiltInCategory.OST_StructuralFoundation;
        BuiltInCategory categoriaEtiquetaBase = BuiltInCategory.OST_StructuralFoundationTags;

        // Parámetros para las etiquetas y vistas
        BuiltInCategory categoriaEtiquetaArmadura = BuiltInCategory.OST_RebarTags;
        DimensionStyleType cotaEstiloLineal = DimensionStyleType.Linear;
        ViewDetailLevel nivelDetalle = ViewDetailLevel.Fine;

        // Constructor del formulario
        public frmZapatas(Document doc, UIDocument uiDoc)
        {
            InitializeComponent();
            
            Tools.AddinManager();
            Tools.CrearRegistroArctualizadorArmaduras(doc.Application.ActiveAddInId);

            // Variable necesarias
            this.IdiomaDelPrograma = Tools.ObtenerIdiomaDelPrograma();
            this.doc = doc;
            this.uiDoc = uiDoc;

            // Crea el grupo de transacciones
            tg = new TransactionGroup(this.doc, transaccionGrupoImagenPreview);

            // Llama a las funciones
            AgregarZapatas();
            AgregarDiametrosYEstilos();
            CargarComboboxEtiquetas(this.doc);
            AsignarPreviewDeImagen();
        }

        /// <summary> Agrega las zapatas a la lista </summary>
        private void AgregarZapatas()
        {
            // Limpia la lista
            this.lstElementos.Items.Clear();

            // Asigna las zapatas del proyecto a la lista
            this.elementos = Tools.ObtenerTodosEjemplaresSegunClaseYCategoria(doc, clase, categoria);

            // Elimina los subelementos
            this.elementos = Tools.EliminarSubelementos(this.elementos);

            // Agrega los elementos a la listbox
            Tools.RellenarListBoxDeElementos(this.lstElementos, doc, this.elementos);

            // Agrega los elementos a la lista desplegable para el preview
            Tools.RellenarComboboxElementos(this.cmbElementosPreview, this.elementos);
        }

        /// <summary> Agrega los diámetros y estilos de lineas al DataGrid </summary>
        private void AgregarDiametrosYEstilos()
        {
            // Crea el DataGridView de los diámetros y estilos
            this.dgvEstiloLinea = Tools.CrearDataGridViewDeDiametrosYEstilos(IdiomaDelPrograma);

            // Agregas las filas al DataGridView
            this.dgvEstiloLinea.Rows.Add(Tools.CrearDataGridViewDeDiametrosYEstilos(IdiomaDelPrograma).Rows);

            // Obtiene el DataGridView con los diámetros y estilos de líneas
            Tools.AgregarDiametrosYEstilos(this.dgvEstiloLinea, this.dgvEstiloLinea.Columns[Tools.nombreColumnaEstilosLineas] as DataGridViewComboBoxColumn, doc);
        }
        
        /// <summary> Carga los combobox de las etiquetas </summary>
        private void CargarComboboxEtiquetas(Document doc)
        {
            // Completa la lista
            this.etiquetasBase.AddRange(Tools.ObtenerEtiquetasIndependientes(doc, categoriaEtiquetaBase));
            this.etiquetasArmaduras.AddRange(Tools.ObtenerEtiquetasIndependientes(doc, categoriaEtiquetaArmadura));
            this.etiquetasLongitud.AddRange(Tools.ObtenerEstilosTexto(doc));
            this.cotasLineales.AddRange(Tools.ObtenerCotas(doc, cotaEstiloLineal));
            this.cotasElevacion.AddRange(Tools.ObtenerCotasElevacion(doc));

            // Limpia y rellena el combobox
            Tools.RellenarComboboxEtiquetas(this.cmbEtiquetaCimentacion, etiquetasBase);
            Tools.RellenarComboboxEtiquetas(this.cmbEtiquetaArmadura, etiquetasArmaduras);
            Tools.RellenarComboboxEtiquetaTexto(this.cmbEtiquetaLongitud, etiquetasLongitud);
            Tools.RellenarComboboxEstilosCotas(this.cmbEstiloCota, cotasLineales);
            Tools.RellenarComboboxEstilosCotasProfundidad(this.cmbEstiloCotaProfundidad, cotasElevacion);
            Tools.RellenarComboboxEscalas(this.cmbEscalaVista);
        }

        /// <summary> Asigna una imagen de prueba para las etiquetas, cotas y despieces de barras </summary>
        private void AsignarPreviewDeImagen()
        {
            // Verifica que no haya empezado
            if (!tg.HasStarted())
            {
                // Activa el grupos de transacción
                tg.Start();
            }

            // Crea una transacción
            using (Transaction t = new Transaction(this.doc, transaccionImagenPreview))
            {
                t.Start();

                // Verifica que existan elementos
                if (this.elementos.Count > 0)
                {
                    // Crea la vista para la sección
                    Autodesk.Revit.DB.View vista = Tools.VistaXX(this.doc, this.elementos[posicionImagenPreview]);

                    //Verifica si la vista es nula
                    if (vista == null)
                    {
                        // Crea otra vista para la sección
                        vista = Tools.VistaYY(this.doc, this.elementos[posicionImagenPreview]);
                    }

                    // Configura la vista y crea las etiquetas
                    CrearEtiquetasYConfigurarVista(vista, this.elementos[posicionImagenPreview]);

                    // Crea la vista previa
                    PreviewControl vistaPrevia = new PreviewControl(this.doc, vista.Id);

                    // Asigna la vista previa para visualizar
                    this.PreviewEtiquetas.Child = vistaPrevia;
                }

                t.Commit();
            }
        }

        /// <summary> Carga el preview control </summary>
        private void ActivarODesactivarImagenes_CheckedChanged(object sender, EventArgs e)
        {            
            // Verifica que la vista previa no sea nula
            if (this.PreviewEtiquetas.Child != null)
            {
                // Obtiene una ventana con el preview de la vista
                PreviewControl pc = this.PreviewEtiquetas.Child as PreviewControl;

                // Obtiene la vista
                Autodesk.Revit.DB.View vista = this.doc.GetElement(pc.ViewId) as Autodesk.Revit.DB.View;

                // Crea una transacción
                using (Transaction t = new Transaction(this.doc, transaccionGrupoImagenPreview))
                {
                    t.Start();

                    // Oculta todo menos el elementos y sus barras
                    Tools.MostrarSolamenteElementoYBarrasEnVista(this.doc, vista, this.elementos[posicionImagenPreview]);

                    // Crea de nuevo las etiquetas
                    CrearEtiquetas(vista, this.elementos[posicionImagenPreview]);

                    t.Commit();
                }
            }
        }

        /// <summary> Elimina las vistas previas del formulario </summary>
        private void EliminarVistaPrevia()
        {
            // Obtiene a ventana con el preview de la vista
            PreviewControl pc = this.PreviewEtiquetas.Child as PreviewControl;

            // Verifica que no sea nula
            if (pc != null)
            {
                // Elimina la ventana
                pc.Dispose();
            }
        }

        /// <summary> Ajusta la vista para que quede centrado y con zoom </summary>
        private void AjustarVistaDePreviewControl(object sender, EventArgs e)
        {
            // Obtiene a ventana con el preview de la vista
            PreviewControl pc = this.PreviewEtiquetas.Child as PreviewControl;

            // Verifica que no sea nula
            if (pc != null)
            {
                try
                {
                    // Hace zoom y coloca centrado la vista
                    pc.UIView.ZoomToFit();
                }
                catch (Exception) { }
            }
        }

        /// <summary> Ajusta la vista para que quede centrado y con zoom </summary>
        private void AjustarVistaDePreviewControl()
        {
            // Obtiene a ventana con el preview de la vista
            PreviewControl pc = this.PreviewEtiquetas.Child as PreviewControl;

            // Verifica que no sea nula
            if (pc != null)
            {                
                try
                {
                    // Hace zoom y coloca centrado la vista
                    pc.UIView.ZoomToFit();
                }
                catch (Exception) { }
            }
        }

        /// <summary> Carga el formulario </summary>
        private void frmZapatas_Load(object sender, EventArgs e)
        {
            // Asignación de textos según el idioma
            this.Name = Language.ObtenerTexto(IdiomaDelPrograma, "Zap4");
            gbxSeleccion.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Zap1-2");
            rbtnTodos.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Zap1-3");
            rbtnElementosSeleccionados.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Zap1-4");
            rbtnConjuntoDeLaLista.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Zap1-5");
            gbxEtiquetas.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Zap2-1");
            lblEscala.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Zap2-2");
            chbEtiquetaBase.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Zap2-3");
            chbEtiquetaArmadura.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Zap2-4");
            chbEtiquetaLongitud.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Zap2-5");
            chbCotaLineal.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Zap2-6");
            chbCotaElevacion.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Zap2-7");
            gbxEtiquetaVistaPrevia.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Zap3-1");
            gbxEjecutar.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Zap4-1");
            chbVistaXX.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Zap4-2");
            chbVistaYY.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Zap4-3");
            btnEjecutar.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Zap5-1");
        }

        /// <summary> Cambia la escala de la vista y guarda en las configuraciones </summary>
        private void cmbEscala_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Guarda el indice seleccionado del combobox escala de la vista
            Tools.GuardarEscalaDeVistaEnConfiguraciones(this.cmbEscalaVista);

            // Verifica que exista algo en el PreviewControl
            if (this.PreviewEtiquetas.Child != null)
            {
                // Elimina la vista previa
                EliminarVistaPrevia();

                // Actualiza la vista previa
                AsignarPreviewDeImagen();
            }
        }

        /// <summary> Cambia el tipo de texto y guarda en las configuraciones </summary>
        private void cmbEtiquetaLongitud_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Guarda el tipo de texto de las longitudes parciales
            Tools.GuardarZapataIndiceTextoDeBarra(this.cmbEtiquetaLongitud);
        }

        /// <summary> Cambia la imagen del preview según el elemento seleccionado </summary>
        private void cmbElementosPreview_SelectionChangeCommitted(object sender, EventArgs e)
        {
            // Obtiene el indice del elemento seleccionado
            this.posicionImagenPreview = this.cmbElementosPreview.SelectedIndex;

            // Verifica que exista algo en el PreviewControl
            if (this.PreviewEtiquetas.Child != null)
            {
                // Elimina la vista previa
                EliminarVistaPrevia();

                // Actualiza la vista previa
                AsignarPreviewDeImagen();
            }
        }

        /// <summary> Cierra la transacción grupal y deshace los cambios </summary>
        private void CerrarTransacciónGrupo()
        {
            // Verifica que la transacción comenzó
            if (tg.HasStarted())
            {
                // Deshace los cambios
                tg.RollBack();
            }

            // Verifica que la transacción no finalizó
            if (!tg.HasEnded())
            {
                // Verifica que la transacción comenzó
                if (tg.HasStarted())
                {
                    // Deshace los cambios
                    tg.RollBack();
                }
            }
        }

        /// <summary> Cierra el formulario cuando se presiona la tecla Esc </summary>
        private void frmCerrar_KeyDown(object sender, KeyEventArgs e)
        {
            // Verifica que sea la tecla Esc
            if (e.KeyCode == Keys.Escape)
            {
                // Elimina las transacción grupal
                CerrarTransacciónGrupo();

                // Elimina las vistas previas
                EliminarVistaPrevia();

                // Cierra el formulario
                this.Close();
            }
        }

        /// <summary> Cuando se cierra el formulario </summary>
        private void frmZapatas_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Elimina las transacción grupal
            CerrarTransacciónGrupo();

            // Elimina las vistas previas
            EliminarVistaPrevia();
        }

        /// <summary> Ejecuta todas las acciones </summary>
        private void btnEjecutar_Click(object sender, EventArgs e)
        {
            // Obtiene los elementos seleccionados en el proyecto
            List<Element> listaSeleccionados = Tools.ObtenerElementosSeleccionadosEnProyecto(this.uiDoc, this.doc, this.elementos);

            // Cierra las transacciones grupales
            CerrarTransacciónGrupo();

            // Abre una transacción
            using (Transaction t = new Transaction(this.doc, Language.ObtenerTexto(IdiomaDelPrograma, "Zap5-2")))
            {
                t.Start();
                
                // Limpia la lista de zapatas
                this.listaZapatas.Clear();

                // Agrega todas las zapatas del proyecto a la lista
                if (this.rbtnTodos.Checked)
                {
                    // Obtiene todas las zapatas
                    this.elementos = Tools.ObtenerTodosEjemplaresSegunClaseYCategoria(doc, clase, categoria);

                    // Elimina los subelementos
                    this.elementos = Tools.EliminarSubelementos(this.elementos);

                    // Asigna todas las zapatas
                    this.listaZapatas = this.elementos;
                }

                // Agrega las zapatas seleccionadas en el proyecto a la lista
                if (this.rbtnElementosSeleccionados.Checked)
                {
                    // Obtiene los elementos seleccionados que coinciden con la lista de zapatas
                    this.listaZapatas = Tools.ObtenerElementosCoincidentesConLista(this.elementos, listaSeleccionados);
                }

                // Agrega las zapatas seleccionadas de la listabox
                if (this.rbtnConjuntoDeLaLista.Checked)
                {
                    // Obtiene los elementos seleccionados de la listbox y agrega a la lista
                    this.listaZapatas = Tools.ObtenerElementosDeUnListbox(this.lstElementos, this.doc, this.elementos);
                }
                
                // Verifica que la lista de zapatas contenga elementos para poder continuar
                if (this.listaZapatas.Count > 0)
                {
                    // Llama al formulario barra de progreso
                    frmBarraProgreso barraProgreso = new frmBarraProgreso(this.listaZapatas.Count);

                    // Muestra el formulario
                    barraProgreso.Show();

                    // Recorre todos los elementos de la lista
                    foreach (Element elem in this.listaZapatas)
                    {
                        // Verifica que la vista X-X esté activado
                        if (this.chbVistaXX.Checked)
                        {
                            // Crea la vista XX
                            Autodesk.Revit.DB.View vista = Tools.VistaXX(this.doc, elem);

                            // Configura la vista y crea las etiquetas
                            CrearEtiquetasYConfigurarVista(vista, elem);
                        }

                        // Verifica que la vista Y-Y esté activado
                        if (this.chbVistaYY.Checked)
                        {
                            // Crea la vista YY
                            Autodesk.Revit.DB.View vista = Tools.VistaYY(this.doc, elem);

                            // Configura la vista y crea las etiquetas
                            CrearEtiquetasYConfigurarVista(vista, elem);
                        }

                        // Incrementa la barra de progreso
                        barraProgreso.Incrementar();
                    }

                    // Cierra el formulario barra de progreso
                    barraProgreso.Close();

                    t.Commit();
                }
            }

            // Cierra el formulario
            this.Close();
        }

        /// <summary> Crea la vista XX </summary>
        private Autodesk.Revit.DB.View CrearEtiquetasYConfigurarVista(Autodesk.Revit.DB.View vista, Element elem)
        {
            // Cambia las configuraciones de visualización de la vista
            vista = Tools.CambiarConfiguracionVista(this.cmbEscalaVista, this.doc, vista, nivelDetalle);

            // Crea las etiquetas para la vista
            CrearEtiquetas(vista, elem);

            // Muestra solamente el elemento y sus armaduras
            Tools.MostrarSolamenteElementoYBarrasEnVista(this.doc, vista, elem);

            // Muestra todos los elementos de la lista
            Tools.MostrarElementosVista(doc, vista, listaEtiquetasCreadas);

            return vista;
        }

        /// <summary> Crea las etiquetas, cotas y despiece de armaduras en una vista dada </summary>
        private void CrearEtiquetas(Autodesk.Revit.DB.View vista, Element elem)
        {
            // Limpia la lista
            listaEtiquetasCreadas.Clear();

            // Muestra solamente el elemento y sus armaduras
            Tools.MostrarSolamenteElementoYBarrasEnVista(this.doc, vista, elem);

            // Crea la lista de Representacion de Armaduras
            List<ArmaduraRepresentacion> listaArmaduraRepresentacion = new List<ArmaduraRepresentacion>();

            // Crea la lista de cotas en la vista
            List<Dimension> listaCotas = new List<Dimension>();

            // Obtiene todas las armaduras del elemento
            List<Rebar> todasBarras = Tools.ObtenerArmadurasDeElemento(elem, vista);

            // Obtiene las armaduras que su plano sea paralelo al de la vista
            List<Rebar> barras = Tools.ObtenerArmaduraPerpendicularVista(vista, todasBarras);

            // Cota lineal
            if (this.chbCotaLineal.Checked)
            {
                // Obtiene el DimensionType de la cota seleccionada
                DimensionType tipoCota = this.cotasLineales.FirstOrDefault(eti => eti.Name == this.cmbEstiloCota.SelectedItem.ToString());

                try
                {
                    // Crea la cota de altura a la izquierda del elemento
                    listaCotas = Tools.CrearCotaParaElemento(this.doc, vista, elem, tipoCota);

                    // Agrega las cotas a la lista
                    listaEtiquetasCreadas.AddRange(listaCotas);
                }
                catch (Exception) { }
            }

            // Etiqueta de cimentación
            if (this.chbEtiquetaBase.Checked)
            {
                try
                {
                    // Obtiene el FamilySymbol de la etiqueta seleccionada
                    FamilySymbol tipoEtiqueta = this.etiquetasBase.FirstOrDefault(eti => eti.Name == this.cmbEtiquetaCimentacion.SelectedItem.ToString());

                    // Posición de la etiqueta
                    int posicion = Jump.Properties.Settings.Default.EtiquetaIndependienteZapatas;

                    // Crea una etiqueta independiente del elemento
                    IndependentTag etiqueta = Tools.CrearEtiquetaSegunConfiguracion(this.doc, vista, elem, tipoEtiqueta, posicion);

                    // Obtiene la dirección según las configuraciones
                    XYZ direccion = Tools.DireccionSegunPosicionDeEtiqueta(vista, posicion);

                    // Obtiene el vector para mover la etiqueta
                    XYZ vector = Tools.ObtenerVectorParaMoverEtiqueta(vista, direccion, etiqueta, listaCotas);

                    // Mueve la etiqueta
                    ElementTransformUtils.MoveElement(this.doc, etiqueta.Id, vector);

                    // Agrega la etiqueta a la lista
                    listaEtiquetasCreadas.Add(etiqueta);
                }
                catch (Exception ) { }
            }

            // Cota de elevación
            if (this.chbCotaElevacion.Checked)
            {
                try
                {
                    // Obtiene el SpotDimensionType de la cota de profundidad seleccionada
                    SpotDimensionType tipoCotaProfundidad = this.cotasElevacion.FirstOrDefault(eti => eti.Name == this.cmbEstiloCotaProfundidad.SelectedItem.ToString());

                    // Posición de la etiqueta
                    int posicion = Jump.Properties.Settings.Default.EtiquetaCotaProfundidad;

                    // Crea la cota de profundidad
                    SpotDimension cotaProfundidad = Tools.CrearCotaProfundidad(this.doc, vista, elem, tipoCotaProfundidad, posicion);

                    // Obtiene la dirección según las configuraciones
                    XYZ direccion = Tools.DireccionSegunPosicionDeEtiqueta(vista, posicion);

                    // Obtiene el vector a mover
                    XYZ vector = Tools.ObtenerVectorParaMoverEtiqueta(vista, direccion, cotaProfundidad, listaCotas);

                    // Verifica que el vector sea menor al ancho del elemento
                    vector = Tools.VerificarAnchoDeElementoParaMoverCotaProfundidad(vista, vector);

                    // Mueve la cota
                    ElementTransformUtils.MoveElement(doc, cotaProfundidad.Id, vector);

                    // Agrega la cota de profundidad a la lista
                    listaEtiquetasCreadas.Add(cotaProfundidad);
                }
                catch (Exception) { }
            }

            // Recorre todas las armaduras que posee el elemento
            foreach (Rebar barra in barras)
            {
                // Crea una etiqueta independiente
                IndependentTag etiquetaArmadura = null;

                // Etiqueta de armadura
                if (this.chbEtiquetaArmadura.Checked)
                {
                    try
                    {
                        // Obtiene el FamilySymbol de la etiqueta seleccionada
                        FamilySymbol tipoEtiqueta = this.etiquetasArmaduras.FirstOrDefault(eti => eti.Name == this.cmbEtiquetaArmadura.SelectedItem.ToString());

                        // Posición de la etiqueta
                        int posicion = Jump.Properties.Settings.Default.EtiquetaIndependienteArmadura;

                        // Crea la etiqueta independiente de la barra
                        IndependentTag etiqueta = Tools.CrearEtiquetaSegunConfiguracion(this.doc, vista, barra, tipoEtiqueta, posicion);

                        // Asigna la etiqueta
                        etiquetaArmadura = etiqueta;

                        // Agrega la etiqueta de armadura a la lista
                        listaEtiquetasCreadas.Add(etiquetaArmadura);
                    }
                    catch (Exception) { }
                }

                // Longitud parcial de barra
                if (this.chbEtiquetaLongitud.Checked)
                {
                    try
                    {
                        // Crea una representación de barra
                        ArmaduraRepresentacion armadura = new ArmaduraRepresentacion(this.doc, vista, barra);

                        // Agrega la Armadura a la lista
                        listaArmaduraRepresentacion.Add(armadura);

                        // Dibuja las armaduras y asigna los estilos de líneas en función de cada diámetro
                        armadura.CurvasDeArmadura = Tools.DibujarArmaduraSegunDatagridview(this.dgvEstiloLinea, this.doc, vista, barra);

                        // Obtiene el tipo de texto
                        TextNoteType tipoTexto = this.etiquetasLongitud.FirstOrDefault(x => x.Name == this.cmbEtiquetaLongitud.SelectedItem.ToString());

                        // Asigna el tipo de texto a la representación de la barra
                        armadura.TipoDeTexto = tipoTexto;

                        // Crea notas de texto con la longitud parcial de la barra
                        armadura.TextosDeLongitudesParciales = Tools.CrearTextNoteDeArmadura(this.doc, vista, barra, tipoTexto);

                        // Agrega la armadura a la lista de despieces
                        Inicio.listaArmaduraRepresentacion.Add(armadura);

                        // Verifica que la opción de etiqueta esté activo
                        if (this.chbEtiquetaArmadura.Checked)
                        {
                            // Asigna el tipo de etiqueta
                            armadura.TipoEtiquetaArmadura = this.etiquetasArmaduras.FirstOrDefault(eti => eti.Name == this.cmbEtiquetaArmadura.SelectedItem.ToString());

                            // Asigna la etiqueta
                            armadura.EtiquetaArmadura = etiquetaArmadura;
                        }

                        // Agrega la representación de la armadura a la lista
                        listaEtiquetasCreadas.AddRange(armadura.CurvasDeArmadura);
                        listaEtiquetasCreadas.AddRange(armadura.TextosDeLongitudesParciales);
                    }
                    catch (Exception) { }
                }

                // Regenera el documento
                this.doc.Regenerate();
            }

            // Mueve los despieces de Armaduras
            Tools.OrdenarYMoverRepresentacionArmaduraSegunDireccion(this.doc, vista, elem, listaArmaduraRepresentacion);
            
            // Ajusta el recuadro de la vista
            Tools.AjustarRecuadroDeVista(this.doc, vista, listaEtiquetasCreadas);

            // Ajusta el zoom de la vista
            AjustarVistaDePreviewControl();
        }
    }
}