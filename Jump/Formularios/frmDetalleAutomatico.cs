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
    public partial class frmDetalleAutomatico : System.Windows.Forms.Form
    {
        // Variable necesarias
        string IdiomaDelPrograma;
        Document doc;
        UIDocument uiDoc;
        string transaccionGrupoImagenPreview = "grupo vista previa";
        string transaccionImagenPreview = "vista previa";
        int posicionImagenPreview = 0;
        TransactionGroup tg;

        // Parámetros para los elementos estructurales
        public Type clase;
        public BuiltInCategory categoria;
        public BuiltInCategory categoriaEtiqueta;
        public int indiceComboboxTextoBarra;
        public int indiceComboboxEscalaVista;
        public int posicionEtiquetaIndependienteElemento;
        public int posicionEtiquetaCotaProfundidad;
        public int posicionEtiquetaIndependienteArmadura;
        public bool cotaVerticalIzquierda;
        public bool cotaVerticalDerecha;
        public bool cotaHorizontalArriba;
        public bool cotaHorizontalAbajo;
        public string clave;

        // Parámetros generales
        List<Element> listaElementosEstructurales = new List<Element>();
        List<Element> elementos = new List<Element>();
        List<FamilySymbol> etiquetasElemento = new List<FamilySymbol>();
        List<FamilySymbol> etiquetasArmaduras = new List<FamilySymbol>();
        List<TextNoteType> etiquetasLongitud = new List<TextNoteType>();
        List<DimensionType> cotasLineales = new List<DimensionType>();
        List<SpotDimensionType> cotasElevacion = new List<SpotDimensionType>();

        // Lista de etiquetas creadas en la vista
        List<Element> listaEtiquetasCreadas = new List<Element>();

        // DataGridView de los díametros y estilos de líneas
        DataGridView dgvEstiloLinea = new DataGridView();

        // Parámetros para las etiquetas y vistas
        BuiltInCategory categoriaEtiquetaArmadura = BuiltInCategory.OST_RebarTags;
        DimensionStyleType cotaEstiloLineal = DimensionStyleType.Linear;
        ViewDetailLevel nivelDetalle = ViewDetailLevel.Fine;

        // Constructor del formulario
        public frmDetalleAutomatico(Document doc, UIDocument uiDoc)
        {
            InitializeComponent();
            
            Tools.AddinManager();
            Tools.CrearRegistroActualizadorArmaduras(doc.Application.ActiveAddInId);

            // Variable necesarias
            this.IdiomaDelPrograma = Tools.ObtenerIdiomaDelPrograma();
            this.doc = doc;
            this.uiDoc = uiDoc;

            // Crea el grupo de transacciones
            tg = new TransactionGroup(this.doc, transaccionGrupoImagenPreview);

            // Crea el DataGridView de los diámetros y estilos
            this.dgvEstiloLinea = Tools.ObtenerDataGridViewDeDiametrosYEstilos(this.dgvEstiloLinea, doc, this.IdiomaDelPrograma);
        }

        /// <summary> Carga el formulario </summary>
        private void frmDetalleAutomatico_Load(object sender, EventArgs e)
        {
            // Llama a las funciones
            AgregarElementos();
            CargarComboboxEtiquetas(this.doc);
            AsignarPreviewDeImagen();

            // Asignación de textos según el idioma
            this.Name = Language.ObtenerTexto(IdiomaDelPrograma, clave + "4");
            gbxSeleccion.Text = Language.ObtenerTexto(IdiomaDelPrograma, clave + "1-2");
            rbtnTodos.Text = Language.ObtenerTexto(IdiomaDelPrograma, clave + "1-3");
            rbtnElementosSeleccionados.Text = Language.ObtenerTexto(IdiomaDelPrograma, clave + "1-4");
            rbtnConjuntoDeLaLista.Text = Language.ObtenerTexto(IdiomaDelPrograma, clave + "1-5");
            gbxEtiquetas.Text = Language.ObtenerTexto(IdiomaDelPrograma, clave + "2-1");
            lblEscala.Text = Language.ObtenerTexto(IdiomaDelPrograma, clave + "2-2");
            chbEtiquetaBase.Text = Language.ObtenerTexto(IdiomaDelPrograma, clave + "2-3");
            chbEtiquetaArmadura.Text = Language.ObtenerTexto(IdiomaDelPrograma, clave + "2-4");
            chbEtiquetaLongitud.Text = Language.ObtenerTexto(IdiomaDelPrograma, clave + "2-5");
            chbCotaLineal.Text = Language.ObtenerTexto(IdiomaDelPrograma, clave + "2-6");
            chbCotaElevacion.Text = Language.ObtenerTexto(IdiomaDelPrograma, clave + "2-7");
            gbxEtiquetaVistaPrevia.Text = Language.ObtenerTexto(IdiomaDelPrograma, clave + "3-1");
            gbxEjecutar.Text = Language.ObtenerTexto(IdiomaDelPrograma, clave + "4-1");
            chbVistaXX.Text = Language.ObtenerTexto(IdiomaDelPrograma, clave + "4-2");
            chbVistaYY.Text = Language.ObtenerTexto(IdiomaDelPrograma, clave + "4-3");
            btnEjecutar.Text = Language.ObtenerTexto(IdiomaDelPrograma, clave + "5-1");
        }

        /// <summary> Agrega los elementos estructurales a la lista </summary>
        private void AgregarElementos()
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
            Tools.RellenarCombobox(this.cmbElementosPreview, this.elementos);
        }

        /// <summary> Carga los combobox de las etiquetas </summary>
        private void CargarComboboxEtiquetas(Document doc)
        {
            // Completa la lista
            this.etiquetasElemento.AddRange(Tools.ObtenerEtiquetasIndependientes(doc, categoriaEtiqueta));
            this.etiquetasArmaduras.AddRange(Tools.ObtenerEtiquetasIndependientes(doc, categoriaEtiquetaArmadura));
            this.etiquetasLongitud.AddRange(Tools.ObtenerEstilosTexto(doc));
            this.cotasLineales.AddRange(Tools.ObtenerCotas(doc, cotaEstiloLineal));
            this.cotasElevacion.AddRange(Tools.ObtenerCotasElevacion(doc));

            // Limpia y rellena el combobox
            Tools.RellenarCombobox(this.cmbEtiquetaElementoEstructural, etiquetasElemento);
            Tools.RellenarCombobox(this.cmbEtiquetaArmadura, etiquetasArmaduras);
            Tools.RellenarCombobox(this.cmbEtiquetaLongitud, etiquetasLongitud);
            Tools.RellenarCombobox(this.cmbEstiloCota, cotasLineales);
            Tools.RellenarCombobox(this.cmbEstiloCotaProfundidad, cotasElevacion);
            Tools.RellenarComboboxEscalas(this.cmbEscalaVista);

            if (this.cmbEscalaVista.Items.Count > 0 && this.indiceComboboxEscalaVista < this.cmbEscalaVista.Items.Count)
            {
                // Asigna el primer elemento a la lista desplegable
                this.cmbEscalaVista.SelectedIndex = this.indiceComboboxEscalaVista;
            }
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

        /// <summary> Elimina la vista previa del formulario </summary>
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

        /// <summary> Cambia la escala de la vista y guarda en las configuraciones </summary>
        private void cmbEscala_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.indiceComboboxEscalaVista = this.cmbEscalaVista.SelectedIndex;

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
            // Obtiene el indice seleccionado del combo
            this.indiceComboboxTextoBarra = this.cmbEtiquetaLongitud.SelectedIndex;
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
        private void frmDetalleAutomatico_FormClosed(object sender, FormClosedEventArgs e)
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
                
                // Limpia la lista
                this.listaElementosEstructurales.Clear();

                // Agrega todos los elementos del proyecto a la lista
                if (this.rbtnTodos.Checked)
                {
                    // Asigna todas los elementos
                    this.listaElementosEstructurales = this.elementos;
                }

                // Agrega los elementos seleccionados en el proyecto a la lista
                if (this.rbtnElementosSeleccionados.Checked)
                {
                    // Obtiene los elementos seleccionados que coinciden con la lista de zapatas
                    this.listaElementosEstructurales = Tools.ObtenerElementosCoincidentesConLista(this.elementos, listaSeleccionados);
                }

                // Agrega los elementos seleccionadas de la listabox
                if (this.rbtnConjuntoDeLaLista.Checked)
                {
                    // Obtiene los elementos seleccionados de la listbox y agrega a la lista
                    this.listaElementosEstructurales = Tools.ObtenerElementosDeUnListbox(this.lstElementos, this.doc, this.elementos);
                }
                
                // Verifica que la lista de elementos estructurales contenga elementos para poder continuar
                if (this.listaElementosEstructurales.Count > 0)
                {
                    // Llama al formulario barra de progreso
                    frmBarraProgreso barraProgreso = new frmBarraProgreso(this.listaElementosEstructurales.Count);

                    // Muestra el formulario
                    barraProgreso.Show();

                    // Recorre todos los elementos de la lista
                    foreach (Element elem in this.listaElementosEstructurales)
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

        /// <summary> Crea la vista </summary>
        private Autodesk.Revit.DB.View CrearEtiquetasYConfigurarVista(Autodesk.Revit.DB.View vista, Element elem)
        {
            // Cambia las configuraciones de visualización de la vista
            vista = Tools.CambiarConfiguracionVista(this.cmbEscalaVista, this.doc, vista, nivelDetalle);

            // Muestra solamente el elemento y sus armaduras
            Tools.MostrarSolamenteElementoYBarrasEnVista(this.doc, vista, elem);

            // Muestra todos los elementos de la lista
            Tools.MostrarElementosVista(doc, vista, listaEtiquetasCreadas);

            // Crea las etiquetas para la vista
            CrearEtiquetas(vista, elem);

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
                DimensionType tipoCota = (DimensionType)this.cmbEstiloCota.SelectedItem;

                //(tipoCota == null) ? (DimensionType)this.cmbEstiloCota.Items[0] , null ;
                
                try
                {
                    // Verifica que esté activo la cota vertical izquierda
                    if (this.cotaVerticalIzquierda)
                    {
                        try
                        {
                            // Crea la cota vertical izquierda
                            listaCotas.Add(Tools.CrearCotaVerticalIzquierdaParaElemento(doc, vista, elem, tipoCota));
                        }
                        catch (Exception) { }
                    }

                    // Verifica que esté activo la cota vertical derecha
                    if (this.cotaVerticalDerecha)
                    {
                        try
                        {
                            // Crea la cota vertical derecha
                            listaCotas.Add(Tools.CrearCotaVerticalDerechaParaElemento(doc, vista, elem, tipoCota));
                        }
                        catch (Exception) { }
                    }

                    // Verifica que esté activo la cota horizontal arriba
                    if (this.cotaHorizontalArriba)
                    {
                        try
                        {
                            // Crea la cota horizontal arriba
                            listaCotas.Add(Tools.CrearCotaHorizontalArribaParaElemento(doc, vista, elem, tipoCota));
                        }
                        catch (Exception) { }
                    }

                    // Verifica que esté activo la cota horizontal abajo
                    if (this.cotaHorizontalAbajo)
                    {
                        try
                        {
                            // Crea la cota horizontal abajo
                            listaCotas.Add(Tools.CrearCotaHorizontalAbajoParaElemento(doc, vista, elem, tipoCota));
                        }
                        catch (Exception) { }
                    }

                    // Agrega las cotas a la lista
                    listaEtiquetasCreadas.AddRange(listaCotas);
                }
                catch (Exception) { }
            }

            // Etiqueta del elemento estructural
            if (this.chbEtiquetaBase.Checked)
            {
                try
                {
                    // Obtiene el FamilySymbol de la etiqueta seleccionada
                    FamilySymbol tipoEtiqueta = (FamilySymbol)this.cmbEtiquetaElementoEstructural.SelectedItem;

                    // Crea una etiqueta independiente del elemento
                    IndependentTag etiqueta = Tools.CrearEtiquetaSegunConfiguracion(this.doc, vista, elem, tipoEtiqueta, this.posicionEtiquetaIndependienteElemento);

                    // Obtiene la dirección según las configuraciones
                    XYZ direccion = Tools.DireccionSegunPosicionDeEtiqueta(vista, this.posicionEtiquetaIndependienteElemento);

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
                    SpotDimensionType tipoCotaProfundidad = (SpotDimensionType)this.cmbEstiloCotaProfundidad.SelectedItem;
                   
                    // Crea la cota de profundidad
                    SpotDimension cotaProfundidad = Tools.CrearCotaProfundidad(this.doc, vista, elem, tipoCotaProfundidad, this.posicionEtiquetaCotaProfundidad);

                    // Obtiene la dirección según las configuraciones
                    XYZ direccion = Tools.DireccionSegunPosicionDeEtiqueta(vista, this.posicionEtiquetaCotaProfundidad);

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
                        FamilySymbol tipoEtiqueta = (FamilySymbol)this.cmbEtiquetaArmadura.SelectedItem;

                        // Crea la etiqueta independiente de la barra
                        IndependentTag etiqueta = Tools.CrearEtiquetaSegunConfiguracion(this.doc, vista, barra, tipoEtiqueta, this.posicionEtiquetaIndependienteArmadura);

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

                        // Asigna el tipo de texto a la representación de la barra
                        armadura.TipoDeTexto = (TextNoteType)this.cmbEtiquetaLongitud.SelectedItem;

                        // Verifica que la opción de etiqueta esté activo
                        if (this.chbEtiquetaArmadura.Checked)
                        {
                            // Asigna el tipo de etiqueta
                            armadura.TipoEtiquetaArmadura = (FamilySymbol)this.cmbEtiquetaArmadura.SelectedItem;

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
            OrdenarYMoverRepresentacionArmaduraSegunDireccion(this.doc, vista, elem, listaArmaduraRepresentacion);
            
            // Ajusta el recuadro de la vista
            Tools.AjustarRecuadroDeVista(this.doc, vista, listaEtiquetasCreadas);

            // Verifica que la transacción grupal finalizó
            if (this.tg.HasEnded())
            {
                // Agrega la representación de armadura la barra
                Tools.GuardarRepresentacionArmaduraDeBarra(barras, listaArmaduraRepresentacion);
            }
            
            // Ajusta el zoom de la vista
            AjustarVistaDePreviewControl();
        }

        ///<summary> Ordena y mueve las Represetaciones de Armaduras según las opciones </summary>
        public void OrdenarYMoverRepresentacionArmaduraSegunDireccion(Document doc, Autodesk.Revit.DB.View vista, Element elem, List<ArmaduraRepresentacion> armaduras)
        {
            // Crea las listas
            List<ArmaduraRepresentacion> listaArmadurasArriba = new List<ArmaduraRepresentacion>();
            List<ArmaduraRepresentacion> listaArmadurasAbajo = new List<ArmaduraRepresentacion>();
            List<ArmaduraRepresentacion> listaArmadurasIzquierda = new List<ArmaduraRepresentacion>();
            List<ArmaduraRepresentacion> listaArmadurasDerecha = new List<ArmaduraRepresentacion>();

            // Crea una transformada de la vista
            Transform tra = vista.CropBox.Transform;

            // Obtiene el recuadro del elemento
            BoundingBoxXYZ bbElem = Tools.ObtenerRecuadroElementoParaleloAVista(doc, vista, elem);

            // Obtiene el baricentro del recuadro del elemento
            XYZ puntoMedioElem = Tools.ObtenerBaricentroElemento(bbElem);

            // Recorre la lista de Representación de Armaduras
            foreach (ArmaduraRepresentacion bar in armaduras)
            {
                try
                {
                    // Obtiene el recuadro de la barra
                    BoundingBoxXYZ bbArmadura = Tools.ObtenerRecuadroElementoParaleloAVista(doc, vista, bar.Barra);

                    // Obtiene el baricentro del recuadro de la barra
                    XYZ puntoMedioArmadura = Tools.ObtenerBaricentroElemento(bbArmadura);

                    // Obtiene la distancia en coordenadas de la vista
                    XYZ distanciaRelativa = tra.Inverse.OfVector(puntoMedioArmadura - puntoMedioElem);

                    OrganizarListaSegunDireccionDeBarra(vista, distanciaRelativa, bar,
                                                        ref listaArmadurasArriba, ref listaArmadurasAbajo,
                                                        ref listaArmadurasIzquierda, ref listaArmadurasDerecha);
                }
                catch (Exception) { }
            }

            OrdenarYMoverListaConArmadurasRepresentacion(doc, vista, tra, elem,
                                                         ref listaArmadurasArriba, ref listaArmadurasAbajo,
                                                         ref listaArmadurasIzquierda, ref listaArmadurasDerecha);
        }

        ///<summary> Organiza una Representación de Armadura según una dirección </summary>
        public void OrganizarListaSegunDireccionDeBarra(Autodesk.Revit.DB.View vista, XYZ distanciaRelativa, ArmaduraRepresentacion bar,
                                                               ref List<ArmaduraRepresentacion> listaArmadurasArriba,
                                                               ref List<ArmaduraRepresentacion> listaArmadurasAbajo,
                                                               ref List<ArmaduraRepresentacion> listaArmadurasIzquierda,
                                                               ref List<ArmaduraRepresentacion> listaArmadurasDerecha)
        {
            // Obtiene la transformada inversa de la vista
            Transform traInv = vista.CropBox.Transform.Inverse;

            // Verifica si la distancia es cero
            if (distanciaRelativa.IsZeroLength())
            {
                // Proyecta y asigna la posición de la armadura en coordenadas relativas
                bar.Posicion = traInv.Inverse.OfVector(Tools.ProyectarVectorSobreDireccion(distanciaRelativa, traInv.OfVector(vista.RightDirection)));

                // Agrega la armadura a la lista
                listaArmadurasDerecha.Add(bar);
            }

            // Verifica si X es mayor a Y
            else if (Math.Abs(distanciaRelativa.X) >= Math.Abs(distanciaRelativa.Y))
            {
                // Verifica si X es positivo
                if (Tools.ObtenerSignoComponenteDeVector(distanciaRelativa.X) == 1)
                {
                    // Proyecta y asigna la posición de la armadura
                    bar.Posicion = traInv.Inverse.OfVector(Tools.ProyectarVectorSobreDireccion(distanciaRelativa, traInv.OfVector(vista.RightDirection)));

                    // Agrega la armadura a la lista
                    listaArmadurasDerecha.Add(bar);
                }

                else
                {
                    // Proyecta y asigna la posición de la armadura
                    bar.Posicion = traInv.Inverse.OfVector(Tools.ProyectarVectorSobreDireccion(distanciaRelativa, traInv.OfVector(vista.RightDirection.Negate())));

                    // Agrega la armadura a la lista
                    listaArmadurasIzquierda.Add(bar);
                }
            }

            // Y es mayor a X
            else
            {
                // Verifica si Y es positivo
                if (Tools.ObtenerSignoComponenteDeVector(distanciaRelativa.Y) == 1)
                {
                    // Proyecta y asigna la posición de la armadura
                    bar.Posicion = traInv.Inverse.OfVector(Tools.ProyectarVectorSobreDireccion(distanciaRelativa, traInv.OfVector(vista.UpDirection)));

                    // Agrega la armadura a la lista
                    listaArmadurasArriba.Add(bar);
                }

                else
                {
                    // Proyecta y asigna la posición de la armadura
                    bar.Posicion = traInv.Inverse.OfVector(Tools.ProyectarVectorSobreDireccion(distanciaRelativa, traInv.OfVector(vista.UpDirection.Negate())));

                    // Agrega la armadura a la lista
                    listaArmadurasAbajo.Add(bar);
                }
            }
        }

        ///<summary> Ordena y mueve las listas de Representación de Armadura </summary>
        public void OrdenarYMoverListaConArmadurasRepresentacion(Document doc, Autodesk.Revit.DB.View vista, Transform tra, Element elem,
                                                                        ref List<ArmaduraRepresentacion> listaArmadurasArriba,
                                                                        ref List<ArmaduraRepresentacion> listaArmadurasAbajo,
                                                                        ref List<ArmaduraRepresentacion> listaArmadurasIzquierda,
                                                                        ref List<ArmaduraRepresentacion> listaArmadurasDerecha)
        {
            // Verifica que existan elementos
            if (listaArmadurasArriba.Count > 0)
            {
                // Ordena la lista
                listaArmadurasArriba = listaArmadurasArriba.OrderBy(x => tra.Inverse.OfVector(x.Posicion).Y).ToList();

                // Mueve los elementos de la lista
                MoverListaConArmaduras(doc, vista, elem, vista.UpDirection, listaArmadurasArriba);
            }

            // Verifica que existan elementos
            if (listaArmadurasAbajo.Count > 0)
            {
                // Ordena la lista
                listaArmadurasAbajo = listaArmadurasAbajo.OrderByDescending(x => tra.Inverse.OfPoint(x.Posicion).Y).ToList();

                // Mueve los elementos de la lista
                MoverListaConArmaduras(doc, vista, elem, vista.UpDirection.Negate(), listaArmadurasAbajo);
            }

            // Verifica que existan elementos
            if (listaArmadurasDerecha.Count > 0)
            {
                // Ordena la lista
                listaArmadurasDerecha = listaArmadurasDerecha.OrderBy(x => tra.Inverse.OfVector(x.Posicion).X).ToList();

                // Mueve los elementos de la lista
                MoverListaConArmaduras(doc, vista, elem, vista.RightDirection, listaArmadurasDerecha);
            }

            // Verifica que existan elementos
            if (listaArmadurasIzquierda.Count > 0)
            {
                // Ordena la lista
                listaArmadurasIzquierda = listaArmadurasIzquierda.OrderByDescending(x => tra.Inverse.OfVector(x.Posicion).X).ToList();

                // Mueve los elementos de la lista
                MoverListaConArmaduras(doc, vista, elem, vista.RightDirection.Negate(), listaArmadurasIzquierda);
            }
        }

        ///<summary> Mueve la lista de Representacion de Armaduras según una dirección </summary>
        public void MoverListaConArmaduras(Document doc, Autodesk.Revit.DB.View vista, Element elem, XYZ direccion, List<ArmaduraRepresentacion> armaduras)
        {
            // Crea las banderas de las direcciones
            bool banderaArriba = true;
            bool banderaAbajo = true;
            bool banderaDerecha = true;
            bool banderaIzquierda = true;

            // Crea una transformada de la vista
            Transform tra = vista.CropBox.Transform;

            // Recuadro del elemento
            BoundingBoxXYZ bbElem = Tools.ObtenerRecuadroElementoParaleloAVista(doc, vista, elem);

            // Distancia a mover
            XYZ distancia = new XYZ();

            // Dimensiones del elemento en coordenadas relativas
            XYZ elementoDimensiones = tra.Inverse.OfVector((bbElem.Max - bbElem.Min) / 2);
            XYZ elementoAncho = new XYZ(Math.Abs(elementoDimensiones.X), 0, 0);
            XYZ elementoAlto = new XYZ(0, Math.Abs(elementoDimensiones.Y), 0);

            foreach (ArmaduraRepresentacion bar in armaduras)
            {
                try
                {
                    bar.DibujarArmaduraSegunDatagridview(this.dgvEstiloLinea);

                    // Recuadro de la barra
                    BoundingBoxXYZ bbBar = bar.ObtenerBoundingBoxDeArmadura();

                    // Asigna la transformada de la vista al recuadro
                    //bbBar.Transform = tra;

                    // Dimensiones de la Representación de Armadura en coordenadas relativas
                    XYZ barDimensiones = tra.Inverse.OfVector(bbBar.Max - bbBar.Min);
                    XYZ barAncho = new XYZ(Math.Abs(barDimensiones.X), 0, 0);
                    XYZ barAlto = new XYZ(0, Math.Abs(barDimensiones.Y), 0);

                    // Verifica si la dirección es arriba
                    if (direccion.IsAlmostEqualTo(vista.UpDirection))
                    {
                        // Verifica si el la primera pasada
                        if (banderaArriba)
                        {
                            //distancia = elementoAlto - bar.Posicion + barAlto;
                            distancia = tra.Inverse.OfVector(Tools.ProyectarVectorSobreDireccion((bbElem.Max - bbBar.Min), vista.UpDirection)) + barAlto / 2;

                            // Cambia el estado de la bandera
                            banderaArriba = false;
                        }
                        else
                        {
                            distancia += barAlto ;
                        }
                    }

                    // Verifica si la dirección es abajo
                    else if (direccion.IsAlmostEqualTo(vista.UpDirection.Negate()))
                    {
                        // Verifica si el la primera pasada
                        if (banderaAbajo)
                        {
                            // Obtiene la distancia a mover
                            //distancia = elementoAlto - bar.Posicion - barAlto;
                            distancia = tra.Inverse.OfVector(Tools.ProyectarVectorSobreDireccion((bbElem.Min - bbBar.Max), vista.UpDirection.Negate())) - barAlto / 2;

                            // Cambia el estado de la bandera
                            banderaAbajo = false;
                        }
                        else
                        {
                            distancia -= barAlto;
                        }
                    }

                    // Verifica si la dirección es derecha
                    else if (direccion.IsAlmostEqualTo(vista.RightDirection))
                    {
                        // Verifica si el la primera pasada
                        if (banderaDerecha)
                        {
                            //distancia = elementoAncho - bar.Posicion + barAncho;
                            distancia = tra.Inverse.OfVector(Tools.ProyectarVectorSobreDireccion((bbElem.Max - bbBar.Min), vista.RightDirection)) + barAncho;

                            // Cambia el estado de la bandera
                            banderaDerecha = false;
                        }
                        else
                        {
                            distancia += barAncho / 2;
                        }
                    }

                    // Verifica si la dirección es izquierda
                    else if (direccion.IsAlmostEqualTo(vista.RightDirection.Negate()))
                    {
                        // Verifica si el la primera pasada
                        if (banderaIzquierda)
                        {
                            //distancia = elementoAncho.Negate() + bar.Posicion - barAncho;
                            distancia = tra.Inverse.OfVector(Tools.ProyectarVectorSobreDireccion((bbElem.Min - bbBar.Max), vista.RightDirection.Negate())) - barAncho;

                            // Cambia el estado de la bandera
                            banderaIzquierda = false;
                        }
                        else
                        {
                            distancia -= barAncho / 2;
                        }
                    }

                    else
                    {
                        // Verifica si el la primera pasada
                        if (banderaDerecha)
                        {
                            //distancia = elementoAncho - bar.Posicion + barAncho;
                            distancia = tra.Inverse.OfVector(Tools.ProyectarVectorSobreDireccion((bbElem.Max - bbBar.Min), vista.RightDirection)) + barAncho;

                            // Cambia el estado de la bandera
                            banderaDerecha = false;
                        }
                        else
                        {
                            distancia += barAncho / 2;
                        }
                    }

                    // Lo lleva a coordenadas globales
                    bar.Posicion = tra.OfVector(distancia);
                    
                    bar.MoverArmaduraRepresentacionConEtiqueta(bar.Posicion);
                }
                catch (Exception) { }
            }
        }

    }
}