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
using System.Windows.Media.TextFormatting;

namespace Jump
{
    public partial class frmDetalleAutomatico : System.Windows.Forms.Form
    {
        // Variable necesarias
        string IdiomaDelPrograma;
        Document doc;
        string transaccionGrupoImagenPreview = "grupo vista previa";
        int posicionImagenPreview = 0;
        TransactionGroup traGeneral;
        public bool bandera = false;
        Type claseDetalleBarra = typeof(RebarBendingDetailType); 
        BuiltInCategory categoriaDetalleBarra = BuiltInCategory.OST_RebarBendingDetails;

        // Parámetros para los elementos estructurales
        public Type clase;
        public BuiltInCategory categoria;
        public BuiltInCategory categoriaEtiqueta;
        public int indiceComboboxEscalaVista;
        public int posicionEtiquetaIndependienteElemento;
        public int posicionEtiquetaCotaProfundidad;
        public int posicionEtiquetaIndependienteArmadura;
        public bool cotaVerticalIzquierda;
        public bool cotaVerticalDerecha;
        public bool cotaHorizontalArriba;
        public bool cotaHorizontalAbajo;
        public string clave;
        public List<ElementId> listaSeleccionados;

        // Parámetros generales
        List<Element> listaElementosEstructurales = new List<Element>();
        List<Element> elementos = new List<Element>();
        List<Element> elementosVistaPreview = new List<Element>();
        List<FamilySymbol> etiquetasElemento = new List<FamilySymbol>();
        List<FamilySymbol> etiquetasArmaduras = new List<FamilySymbol>();
        List<Element> etiquetasLongitud = new List<Element>();
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
        DisplayStyle estiloVista = DisplayStyle.FlatColors;
        ViewFamily seccion = ViewFamily.Section;
        ViewFamily planoEstructural = ViewFamily.StructuralPlan;

        // Constructor del formulario
        public frmDetalleAutomatico(Document doc)
        {
            InitializeComponent();
            
            Tools.AddinManager();

            // Variable necesarias
            this.IdiomaDelPrograma = Tools.ObtenerIdiomaDelPrograma();
            this.doc = doc;

            // Crea el grupo de transacciones
            traGeneral = new TransactionGroup(this.doc, transaccionGrupoImagenPreview);
            traGeneral.Start();
        }

        /// <summary> Carga el formulario </summary>
        private void frmDetalleAutomatico_Load(object sender, EventArgs e)
        {
            // Llama a las funciones
            AgregarElementos();
            AsignarPreviewDeImagen();

            // Asignación de textos según el idioma
            this.Text = Language.ObtenerTexto(IdiomaDelPrograma, clave + "4");
            btnAceptar.Text = Language.ObtenerTexto(IdiomaDelPrograma, clave + "5");
            btnCancelar.Text = Language.ObtenerTexto(IdiomaDelPrograma, clave + "6");
            gbxSeleccion.Text = Language.ObtenerTexto(IdiomaDelPrograma, clave + "1-2");
            rbtnTodos.Text = Language.ObtenerTexto(IdiomaDelPrograma, clave + "1-3");
            rbtnElementosSeleccionados.Text = Language.ObtenerTexto(IdiomaDelPrograma, clave + "1-4");
            rbtnConjuntoDeLaLista.Text = Language.ObtenerTexto(IdiomaDelPrograma, clave + "1-5");
            gbxEtiquetas.Text = Language.ObtenerTexto(IdiomaDelPrograma, clave + "2-1");
            lblEscala.Text = Language.ObtenerTexto(IdiomaDelPrograma, clave + "2-2");
            chbEtiquetaElemento.Text = Language.ObtenerTexto(IdiomaDelPrograma, clave + "2-3");
            chbEtiquetaArmadura.Text = Language.ObtenerTexto(IdiomaDelPrograma, clave + "2-4");
            chbEtiquetaLongitud.Text = Language.ObtenerTexto(IdiomaDelPrograma, clave + "2-5");
            chbCotaLineal.Text = Language.ObtenerTexto(IdiomaDelPrograma, clave + "2-6");
            chbCotaElevacion.Text = Language.ObtenerTexto(IdiomaDelPrograma, clave + "2-7");
            gbxEtiquetaVistaPrevia.Text = Language.ObtenerTexto(IdiomaDelPrograma, clave + "3-1");
            gbxVistas.Text = Language.ObtenerTexto(IdiomaDelPrograma, clave + "4-1");
            chbVistaXX.Text = Language.ObtenerTexto(IdiomaDelPrograma, clave + "4-2");
            chbPlantillaVistaX.Text = Language.ObtenerTexto(IdiomaDelPrograma, clave + "4-2-1");
            chbVistaYY.Text = Language.ObtenerTexto(IdiomaDelPrograma, clave + "4-3");
            chbPlantillaVistaY.Text = Language.ObtenerTexto(IdiomaDelPrograma, clave + "4-3-1");
            chbPlanoEstructural.Text = Language.ObtenerTexto(IdiomaDelPrograma, clave + "4-4");
            chbPlantillaPlanoEstructural.Text = Language.ObtenerTexto(IdiomaDelPrograma, clave + "4-4-1");
        }

        /// <summary> Agrega los elementos estructurales a la lista </summary>
        private void AgregarElementos()
        {
            // Limpia la lista
            this.lstElementos.Items.Clear();

            // Asigna las zapatas del proyecto a la lista
            this.elementos = Tools.ObtenerTodosEjemplaresSegunClaseYCategoria(doc, clase, categoria);
            this.elementosVistaPreview = this.elementos;

            // Elimina los subelementos
            try { this.elementos = Tools.EliminarSubelementos(this.elementos); } catch (Exception) { }

            // Completa la lista
            this.etiquetasElemento.AddRange(Tools.ObtenerEtiquetasIndependientes(doc, categoriaEtiqueta));
            this.etiquetasArmaduras.AddRange(Tools.ObtenerEtiquetasIndependientes(doc, categoriaEtiquetaArmadura));
            this.etiquetasLongitud.AddRange(Tools.ObtenerTodosTiposSegunClaseYCategoria(doc, claseDetalleBarra, categoriaDetalleBarra));
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

            // Determina la vista que se usa
            List<ViewFamilyType> tipoSeccion = new FilteredElementCollector(doc).OfClass(typeof(ViewFamilyType))
                                               .Cast<ViewFamilyType>()
                                               .Where(v => v.ViewFamily == seccion)
                                               .OrderBy(x => x.Name).ToList();

            List<ViewFamilyType> tipoPlanoEstructural = new FilteredElementCollector(doc).OfClass(typeof(ViewFamilyType))
                                                        .Cast<ViewFamilyType>()
                                                        .Where(v => v.ViewFamily == planoEstructural)
                                                        .OrderBy(x => x.Name).ToList();

            List<Autodesk.Revit.DB.View> tipoPlantilla = new FilteredElementCollector(doc)
                                                          .OfClass(typeof(Autodesk.Revit.DB.View))
                                                          .Cast<Autodesk.Revit.DB.View>()
                                                          .Where(v => v.IsTemplate)
                                                          .OrderBy(x => x.Name).ToList();

            // Agrega los elementos a la listbox
            Tools.RellenarListBoxDeElementos(this.lstElementos, doc, this.elementos);
            Tools.RellenarComboBoxDeElementosPreview(this.cmbElementosPreview, this.doc, this.elementosVistaPreview);

            Tools.RellenarCombobox(this.cmbTipoSeccionX, new List<ViewFamilyType>(tipoSeccion));
            Tools.RellenarCombobox(this.cmbPlantillaSeccionX, new List<Autodesk.Revit.DB.View>(tipoPlantilla));

            Tools.RellenarCombobox(this.cmbTipoSeccionY, new List<ViewFamilyType>(tipoSeccion));
            Tools.RellenarCombobox(this.cmbPlantillaSeccionY, new List<Autodesk.Revit.DB.View>(tipoPlantilla));

            Tools.RellenarCombobox(this.cmbTipoPlanoEstructural, new List<ViewFamilyType>(tipoPlanoEstructural));
            Tools.RellenarCombobox(this.cmbPlantillaPlanoEstructural, new List<Autodesk.Revit.DB.View>(tipoPlantilla));
        }
        
        /// <summary> Asigna una imagen de prueba para las etiquetas, cotas y despieces de barras </summary>
        private void AsignarPreviewDeImagen()
        {
            // Verifica que existan elementos
            if (this.elementosVistaPreview.Count > 0)
            {
                using (Transaction tr = new Transaction(this.doc, this.transaccionGrupoImagenPreview))
                {
                    tr.Start();

                    // Crea la vista para la sección
                    Autodesk.Revit.DB.View vista = null;

                    if (this.chbVistaXX.Checked)
                    {
                        vista = Tools.VistaXX(this.doc, this.elementosVistaPreview[posicionImagenPreview]);

                        AsignarTipoYPlantillaAVista(vista, (ViewFamilyType)this.cmbTipoSeccionX.SelectedItem,
                            (Autodesk.Revit.DB.View)this.cmbPlantillaSeccionX.SelectedItem, this.chbPlantillaVistaX);
                    }

                    else if (this.chbVistaYY.Checked)
                    {
                        vista = Tools.VistaYY(this.doc, this.elementosVistaPreview[posicionImagenPreview]);

                        AsignarTipoYPlantillaAVista(vista, (ViewFamilyType)this.cmbTipoSeccionY.SelectedItem,
                            (Autodesk.Revit.DB.View)this.cmbPlantillaSeccionY.SelectedItem, this.chbPlantillaVistaY);
                    }

                    else if (this.chbPlanoEstructural.Checked)
                    {
                        vista = Tools.VistaEnPlanta(this.doc, this.elementosVistaPreview[posicionImagenPreview]);

                        AsignarTipoYPlantillaAVista(vista, (ViewFamilyType)this.cmbTipoPlanoEstructural.SelectedItem,
                            (Autodesk.Revit.DB.View)this.cmbPlantillaPlanoEstructural.SelectedItem, this.chbPlantillaPlanoEstructural);
                    }

                    else
                    {
                        // Crea otra vista para la sección
                        vista = null;
                    }

                    if (vista != null)
                    {
                        // Configura la vista y crea las etiquetas
                        vista = CrearEtiquetasYConfigurarVista(vista, this.elementosVistaPreview[posicionImagenPreview]);

                        // Crea la vista previa
                        PreviewControl vistaPrevia = new PreviewControl(this.doc, vista.Id);

                        // Asigna la vista previa para visualizar
                        this.PreviewEtiquetas.Child = vistaPrevia;
                    }

                    tr.Commit();
                }
            }
        }

        // <summary> Asigna el tipo de seccion y la plantilla a la vista </summary>
        private void AsignarTipoYPlantillaAVista(Autodesk.Revit.DB.View vista, ViewFamilyType vft, Autodesk.Revit.DB.View plantilla, CheckBox chb)
        {
            if (vista != null)
            {
                if (vft != null && vft.Id != ElementId.InvalidElementId)
                {
                    try { vista.ChangeTypeId(vft.Id); } catch (Exception) { }
                }

                if (plantilla != null && plantilla.Id != ElementId.InvalidElementId && chb.Checked)
                {
                    try 
                    { 
                        vista.ViewTemplateId = plantilla.Id;

                        vista.CropBoxActive = true;
                    } 
                    catch (Exception) { }
                }
            }

            this.doc.Regenerate();
        }

        /// <summary> Carga el preview control </summary>
        private void ActivarODesactivarImagenes_CheckedChanged(object sender, EventArgs e)
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
            // Verifica que la vista previa no sea nula
            if (this.PreviewEtiquetas.Child != null)
            {
                AjustarVistaDePreviewControl();
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

        /// <summary> Cambia la lista de elemento preview según la selección </summary>
        private void rbtnTodos_CheckedChanged(object sender, EventArgs e)
        {
            this.posicionImagenPreview = 0;

            this.elementosVistaPreview = ObtenerElementosSeleccionados();

            Tools.RellenarComboBoxDeElementosPreview(this.cmbElementosPreview, this.doc, this.elementosVistaPreview);

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
        private void CerrarTransacciónGeneral()
        {
            // Verifica que la transacción comenzó
            if (traGeneral.HasStarted())
            {
                // Deshace los cambios
                traGeneral.RollBack();
            }

            // Verifica que la transacción no finalizó
            if (!traGeneral.HasEnded())
            {
                // Verifica que la transacción comenzó
                if (traGeneral.HasStarted())
                {
                    // Deshace los cambios
                    traGeneral.RollBack();
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
                CerrarTransacciónGeneral();

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
            CerrarTransacciónGeneral();

            // Elimina las vistas previas
            EliminarVistaPrevia();
        }

        /// <summary> Botón cancelar del formulario </summary>
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            // Elimina las transacción grupal
            CerrarTransacciónGeneral();

            // Elimina las vistas previas
            EliminarVistaPrevia();

            // Cierra el formulario
            this.Close();
        }

        /// <summary> Ejecuta todas las acciones </summary>
        private void btnEjecutar_Click(object sender, EventArgs e)
        {
            // Cierra las transacciones grupales
            CerrarTransacciónGeneral();

            // Limpia la lista
            this.listaElementosEstructurales.Clear();

            this.listaElementosEstructurales = ObtenerElementosSeleccionados();

            // Verifica que la lista de elementos estructurales contenga elementos para poder continuar
            if (this.listaElementosEstructurales.Count > 0)
            {
                // Llama al formulario barra de progreso
                frmBarraProgreso barraProgreso = new frmBarraProgreso(this.listaElementosEstructurales.Count);

                // Muestra el formulario
                barraProgreso.Show();

                using (Transaction tra = new Transaction(this.doc, Language.ObtenerTexto(IdiomaDelPrograma, clave + "5-1"))) 
                {
                    tra.Start();

                    // Recorre todos los elementos de la lista
                    foreach (Element elem in this.listaElementosEstructurales)
                    {
                        if (barraProgreso.Cancelado())
                        {
                            break;
                        }

                        // Verifica que la vista X-X esté activado
                        if (this.chbVistaXX.Checked)
                        {
                            // Crea la vista XX
                            Autodesk.Revit.DB.View vista = Tools.VistaXX(this.doc, elem);

                            this.doc.Regenerate();

                            AsignarTipoYPlantillaAVista(vista, (ViewFamilyType)this.cmbTipoSeccionX.SelectedItem,
                                (Autodesk.Revit.DB.View)this.cmbPlantillaSeccionX.SelectedItem, this.chbPlantillaVistaX);

                            // Configura la vista y crea las etiquetas
                            CrearEtiquetasYConfigurarVista(vista, elem);
                        }

                        // Verifica que la vista Y-Y esté activado
                        if (this.chbVistaYY.Checked)
                        {
                            // Crea la vista YY
                            Autodesk.Revit.DB.View vista = Tools.VistaYY(this.doc, elem);

                            this.doc.Regenerate();

                            AsignarTipoYPlantillaAVista(vista, (ViewFamilyType)this.cmbTipoSeccionY.SelectedItem,
                                (Autodesk.Revit.DB.View)this.cmbPlantillaSeccionY.SelectedItem, this.chbPlantillaVistaY);

                            // Configura la vista y crea las etiquetas
                            CrearEtiquetasYConfigurarVista(vista, elem);
                        }

                        // Verifica que la vista Y-Y esté activado
                        if (this.chbPlanoEstructural.Checked)
                        {
                            // Crea la vista YY
                            Autodesk.Revit.DB.View vista = Tools.VistaEnPlanta(this.doc, elem);

                            this.doc.Regenerate();

                            AsignarTipoYPlantillaAVista(vista, (ViewFamilyType)this.cmbTipoPlanoEstructural.SelectedItem,
                                (Autodesk.Revit.DB.View)this.cmbPlantillaPlanoEstructural.SelectedItem, this.chbPlantillaPlanoEstructural);

                            // Configura la vista y crea las etiquetas
                            CrearEtiquetasYConfigurarVista(vista, elem);
                        }

                        // Incrementa la barra de progreso
                        barraProgreso.Incrementar();
                    }

                    // Verifica que la operación no se haya cancelado
                    if (barraProgreso.Cancelado())
                    {
                        tra.RollBack();
                    }
                    else
                    {
                        tra.Commit();
                    }
                }

                // Cierra el formulario barra de progreso
                barraProgreso.Close();
            }

            this.bandera = true;

            // Cierra el formulario
            this.Close();
        }

        /// <summary> Obtiene los elementos según el radiobutton </summary>
        private List<Element> ObtenerElementosSeleccionados()
        {
            List<Element> lista = new List<Element>();

            // Agrega todos los elementos del proyecto a la lista
            if (this.rbtnTodos.Checked)
            {
                // Asigna todas los elementos
                lista = this.elementos;
            }

            // Agrega los elementos seleccionados en el proyecto a la lista
            if (this.rbtnElementosSeleccionados.Checked)
            {
                // Obtiene los elementos seleccionados que coinciden con la lista de zapatas
                lista = Tools.ObtenerElementosCoincidentesConLista(this.elementos, Tools.ObtenerElementoSegunID(this.doc, this.listaSeleccionados));
            }

            // Agrega los elementos seleccionadas de la listabox
            if (this.rbtnConjuntoDeLaLista.Checked)
            {
                // Obtiene los elementos seleccionados de la listbox y agrega a la lista
                lista = Tools.ObtenerElementosDeUnListbox(this.lstElementos, this.doc, this.elementos);
            }

            return lista;
        }

        /// <summary> Configura la vista y crea las etiquetas seleccionadas  </summary>
        private Autodesk.Revit.DB.View CrearEtiquetasYConfigurarVista(Autodesk.Revit.DB.View vista, Element elem)
        {
            if (vista != null)
            {
                if (vista.ViewTemplateId == ElementId.InvalidElementId)
                {
                    vista = Tools.CambiarConfiguracionVista(this.cmbEscalaVista, this.doc, vista, nivelDetalle, estiloVista);
                }

                Tools.MostrarSolamenteElementoYBarrasEnVista(this.doc, vista, elem);

                vista = Tools.AjustarRecuadroDeVista(vista, elem, Tools.ObtenerArmadurasDeElemento(elem, vista));

                CrearEtiquetas(vista, elem);

                this.doc.Regenerate();
            }

            return vista;
        }
        
        /// <summary> Crea las etiquetas, cotas y despiece de armaduras en una vista dada </summary>
        private void CrearEtiquetas(Autodesk.Revit.DB.View vista, Element elem)
        {
            // Limpia la lista
            listaEtiquetasCreadas.Clear();

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

                // Verifica que esté activo la cota vertical izquierda
                if (this.cotaVerticalIzquierda)
                {
                    try
                    {
                        // Crea la cota vertical izquierda
                        listaCotas.Add(Tools.CrearCotaVerticalIzquierdaParaElemento(this.doc, vista, elem, tipoCota));
                    }
                    catch (Exception) { }
                }

                // Verifica que esté activo la cota vertical derecha
                if (this.cotaVerticalDerecha)
                {
                    try
                    {
                        // Crea la cota vertical derecha
                        listaCotas.Add(Tools.CrearCotaVerticalDerechaParaElemento(this.doc, vista, elem, tipoCota));
                    }
                    catch (Exception) { }
                }

                // Verifica que esté activo la cota horizontal arriba
                if (this.cotaHorizontalArriba)
                {
                    try
                    {
                        // Crea la cota horizontal arriba
                        listaCotas.Add(Tools.CrearCotaHorizontalArribaParaElemento(this.doc, vista, elem, tipoCota));
                    }
                    catch (Exception) { }
                }

                // Verifica que esté activo la cota horizontal abajo
                if (this.cotaHorizontalAbajo)
                {
                    try
                    {
                        // Crea la cota horizontal abajo
                        listaCotas.Add(Tools.CrearCotaHorizontalAbajoParaElemento(this.doc, vista, elem, tipoCota));
                    }
                    catch (Exception) { }
                }

                if (listaCotas.Count > 0)
                {
                    // Agrega las cotas a la lista
                    listaEtiquetasCreadas.AddRange(listaCotas);
                }
            }

            // Etiqueta del elemento estructural
            if (this.chbEtiquetaElemento.Checked)
            {
                try
                {
                    // Obtiene el FamilySymbol de la etiqueta seleccionada
                    FamilySymbol tipoEtiqueta = (FamilySymbol)this.cmbEtiquetaElementoEstructural.SelectedItem;

                    // Crea una etiqueta independiente del elemento
                    IndependentTag etiqueta = Tools.CrearEtiquetaSegunConfiguracion(this.doc, vista, elem, tipoEtiqueta, this.posicionEtiquetaIndependienteElemento);

                    if (listaCotas.Count > 0)
                    {
                        // Obtiene la dirección según las configuraciones
                        XYZ direccion = Tools.DireccionSegunPosicionDeEtiqueta(vista, elem, etiqueta, this.posicionEtiquetaIndependienteElemento);
                        
                        // Obtiene el vector para mover la etiqueta
                        XYZ vector = Tools.ObtenerVectorParaMoverEtiqueta(vista, direccion, etiqueta, listaCotas);
                        
                        // Mueve la etiqueta
                        ElementTransformUtils.MoveElement(this.doc, etiqueta.Id, vector);
                    }

                    // Agrega la etiqueta a la lista
                    listaEtiquetasCreadas.Add(etiqueta);
                }
                catch (Exception) { }
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

                    // Agrega la cota de profundidad a la lista
                    listaEtiquetasCreadas.Add(cotaProfundidad);
                }
                catch (Exception) { }
            }

            // Recorre todas las armaduras que posee el elemento
            foreach (Rebar barra in barras)
            {
                IndependentTag etiquetaArmadura = null;

                // Etiqueta de armadura
                if (this.chbEtiquetaArmadura.Checked)
                {
                    try
                    {
                        // Obtiene el FamilySymbol de la etiqueta seleccionada
                        FamilySymbol tipoEtiqueta = (FamilySymbol)this.cmbEtiquetaArmadura.SelectedItem;

                        // Crea la etiqueta independiente de la barra
                        etiquetaArmadura = Tools.CrearEtiquetaArmaduraSegunConfiguracion(this.doc, vista, barra, tipoEtiqueta, this.posicionEtiquetaIndependienteArmadura);

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
                        RebarBendingDetailType tipoBarra = (RebarBendingDetailType)this.cmbEtiquetaLongitud.SelectedItem;

                        XYZ baricentro = Tools.ObtenerBaricentroDeRecuadro(barra.get_BoundingBox(vista));

                        IndependentTag representacionArmadura = RebarBendingDetail.Create(doc, vista.Id, barra.Id, Jump.Properties.Settings.Default.PosicionBarraADibujar, tipoBarra, baricentro, 0) as IndependentTag;

                        this.doc.Regenerate();

                        listaEtiquetasCreadas.Add(representacionArmadura);

                        ArmaduraRepresentacion armadura = new ArmaduraRepresentacion(representacionArmadura, etiquetaArmadura, this.posicionEtiquetaIndependienteArmadura);

                        listaArmaduraRepresentacion.Add(armadura);
                    }
                    catch (Exception) { }
                }

                // Regenera el documento
                this.doc.Regenerate();
            }

            // Mueve los despieces de Armaduras
            OrdenarYMoverRepresentacionArmaduraSegunDireccion(vista, elem, listaArmaduraRepresentacion);

            // Ajusta el zoom de la vista
            AjustarVistaDePreviewControl();
        }

        ///<summary> Crea las cotas lineales </summary>
        public List<Dimension> CrearCotasLineales(Autodesk.Revit.DB.View vista, Element elem, DimensionType tipoCota)
        {
            // Crea la lista de cotas en la vista
            List<Dimension> listaCotas = new List<Dimension>();

            

            return listaCotas;
        }

        ///<summary> Ordena y mueve las Represetaciones de Armaduras según las opciones </summary>
        public void OrdenarYMoverRepresentacionArmaduraSegunDireccion(Autodesk.Revit.DB.View vista, Element elem, List<ArmaduraRepresentacion> armaduras)
        {
            // Crea las listas
            List<ArmaduraRepresentacion> listaArmadurasArriba = new List<ArmaduraRepresentacion>();
            List<ArmaduraRepresentacion> listaArmadurasAbajo = new List<ArmaduraRepresentacion>();
            List<ArmaduraRepresentacion> listaArmadurasIzquierda = new List<ArmaduraRepresentacion>();
            List<ArmaduraRepresentacion> listaArmadurasDerecha = new List<ArmaduraRepresentacion>();

            // Crea una transformada de la vista
            Transform tra = vista.CropBox.Transform;

            // Obtiene el recuadro del elemento
            BoundingBoxXYZ bbElem = elem.get_BoundingBox(null);

            // Obtiene el baricentro del recuadro del elemento
            XYZ puntoMedioElem = Tools.ObtenerBaricentroDeRecuadro(bbElem);

            // Recorre la lista de Representación de Armaduras
            foreach (ArmaduraRepresentacion representacion in armaduras)
            {
                try
                {
                    XYZ direccion = tra.Inverse.OfVector(representacion.Representacion.TagHeadPosition - puntoMedioElem);

                    XYZ direccionPrincipal = tra.Inverse.OfVector(Tools.ObtenerNormalADireccionPrincipalArmadura(this.doc, vista, representacion.Representacion.GetTaggedLocalElements().FirstOrDefault() as Rebar));

                    XYZ distanciaRelativa = Tools.ProyectarVectorSobreDireccion(direccion, direccionPrincipal);
                    
                    OrganizarListaSegunDireccionDeBarra(vista, distanciaRelativa, representacion,
                                                        ref listaArmadurasArriba, ref listaArmadurasAbajo,
                                                        ref listaArmadurasIzquierda, ref listaArmadurasDerecha);
                }
                catch (Exception) { }
            }

            OrdenarYMoverListaConArmadurasRepresentacion(vista, tra, elem,
                                                         ref listaArmadurasArriba, ref listaArmadurasAbajo,
                                                         ref listaArmadurasIzquierda, ref listaArmadurasDerecha);
        }

        ///<summary> Organiza una Representación de Armadura según una dirección </summary>
        public void OrganizarListaSegunDireccionDeBarra(Autodesk.Revit.DB.View vista, XYZ distanciaRelativa, ArmaduraRepresentacion representacion,
                                                        ref List<ArmaduraRepresentacion> listaArmadurasArriba,
                                                        ref List<ArmaduraRepresentacion> listaArmadurasAbajo,
                                                        ref List<ArmaduraRepresentacion> listaArmadurasIzquierda,
                                                        ref List<ArmaduraRepresentacion> listaArmadurasDerecha)
        {
            // Verifica si la distancia es cero
            if (distanciaRelativa.IsZeroLength())
            {
                // Agrega la armadura a la lista
                listaArmadurasDerecha.Add(representacion);
            }

            // Verifica si X es mayor a Y
            else if (Math.Abs(distanciaRelativa.X) >= Math.Abs(distanciaRelativa.Y))
            {
                // Verifica si X es positivo
                if (Tools.ObtenerSignoComponenteDeVector(distanciaRelativa.X) == 1)
                {
                    // Agrega la armadura a la lista
                    listaArmadurasDerecha.Add(representacion);
                }

                else
                {
                    // Agrega la armadura a la lista
                    listaArmadurasIzquierda.Add(representacion);
                }
            }

            // Y es mayor a X
            else
            {
                // Verifica si Y es positivo
                if (Tools.ObtenerSignoComponenteDeVector(distanciaRelativa.Y) == 1)
                {
                    // Agrega la armadura a la lista
                    listaArmadurasArriba.Add(representacion);
                }

                else
                {
                    // Agrega la armadura a la lista
                    listaArmadurasAbajo.Add(representacion);
                }
            }
        }

        ///<summary> Ordena y mueve las listas de Representación de Armadura </summary>
        public void OrdenarYMoverListaConArmadurasRepresentacion(Autodesk.Revit.DB.View vista, Transform tra, Element elem,
                                                                 ref List<ArmaduraRepresentacion> listaArmadurasArriba,
                                                                 ref List<ArmaduraRepresentacion> listaArmadurasAbajo,
                                                                 ref List<ArmaduraRepresentacion> listaArmadurasIzquierda,
                                                                 ref List<ArmaduraRepresentacion> listaArmadurasDerecha)
        {
            // Verifica que existan elementos
            if (listaArmadurasArriba.Count > 0)
            {
                // Ordena la lista
                listaArmadurasArriba = listaArmadurasArriba.OrderBy(x => tra.Inverse.OfVector(x.Representacion.TagHeadPosition).Y).ToList();

                // Mueve los elementos de la lista
                MoverListaConArmaduras(vista, elem, vista.UpDirection, listaArmadurasArriba);
            }

            // Verifica que existan elementos
            if (listaArmadurasAbajo.Count > 0)
            {
                // Ordena la lista
                listaArmadurasAbajo = listaArmadurasAbajo.OrderByDescending(x => tra.Inverse.OfPoint(x.Representacion.TagHeadPosition).Y).ToList();

                // Mueve los elementos de la lista
                MoverListaConArmaduras(vista, elem, vista.UpDirection.Negate(), listaArmadurasAbajo);
            }

            // Verifica que existan elementos
            if (listaArmadurasDerecha.Count > 0)
            {
                // Ordena la lista
                listaArmadurasDerecha = listaArmadurasDerecha.OrderBy(x => tra.Inverse.OfVector(x.Representacion.TagHeadPosition).X).ToList();

                // Mueve los elementos de la lista
                MoverListaConArmaduras(vista, elem, vista.RightDirection, listaArmadurasDerecha);
            }

            // Verifica que existan elementos
            if (listaArmadurasIzquierda.Count > 0)
            {
                // Ordena la lista
                listaArmadurasIzquierda = listaArmadurasIzquierda.OrderByDescending(x => tra.Inverse.OfVector(x.Representacion.TagHeadPosition).X).ToList();

                // Mueve los elementos de la lista
                MoverListaConArmaduras(vista, elem, vista.RightDirection.Negate(), listaArmadurasIzquierda);
            }
        }

        ///<summary> Mueve la lista de Representacion de Armaduras según una dirección </summary>
        public void MoverListaConArmaduras(Autodesk.Revit.DB.View vista, Element elem, XYZ direccion, List<ArmaduraRepresentacion> armaduras)
        {
            // Crea las banderas de las direcciones
            bool banderaArriba = true;
            bool banderaAbajo = true;
            bool banderaDerecha = true;
            bool banderaIzquierda = true;

            // Crea una transformada de la vista
            Transform tra = vista.CropBox.Transform;

            // Recuadro del elemento
            BoundingBoxXYZ bbElem = elem.get_BoundingBox(vista);

            // Distancia a mover
            XYZ distancia = new XYZ();

            foreach (ArmaduraRepresentacion representacion in armaduras)
            {
                try
                {
                    // Recuadro de la barra
                    BoundingBoxXYZ bbBar = representacion.Representacion.get_BoundingBox(vista);

                    // Dimensiones de la Representación de Armadura en coordenadas relativas
                    XYZ barDimensiones = tra.Inverse.OfVector(bbBar.Max - bbBar.Min);
                    XYZ barAncho = new XYZ(Math.Abs(barDimensiones.X), 0, 0);
                    XYZ barAlto = new XYZ(0, Math.Abs(barDimensiones.Y), 0);

                    XYZ etiquetaDimension = new XYZ();
                    XYZ etiquetaAncho = new XYZ();
                    XYZ etiquetaAlto = new XYZ();

                    if (representacion.Etiqueta != null)
                    {
                        etiquetaDimension = tra.Inverse.OfVector(representacion.Etiqueta.get_BoundingBox(vista).Max - representacion.Etiqueta.get_BoundingBox(vista).Min);
                        etiquetaAncho = new XYZ(Math.Abs(etiquetaDimension.X), 0, 0);
                        etiquetaAlto = new XYZ(0, Math.Abs(etiquetaDimension.Y), 0);
                    }

                    // Verifica si la dirección es arriba
                    if (direccion.IsAlmostEqualTo(vista.UpDirection))
                    {
                        // Verifica si el la primera pasada
                        if (banderaArriba)
                        {
                            //distancia = tra.Inverse.OfVector(Tools.ProyectarVectorSobreDireccionYSentido((bbElem.Max - bbBar.Min), vista.UpDirection)) / 2 + barAlto;
                            XYZ proyectado = Tools.ProyectarVectorSobreDireccionYSentido(bbElem.Max - representacion.Representacion.get_BoundingBox(vista).Min, vista.UpDirection);
                            distancia = tra.Inverse.OfVector(proyectado) + barAlto + etiquetaAlto;
                            
                            // Cambia el estado de la bandera
                            banderaArriba = false;
                        }
                        else
                        {
                            distancia += barAlto + etiquetaAlto;
                        }                     
                    }

                    // Verifica si la dirección es abajo
                    else if (direccion.IsAlmostEqualTo(vista.UpDirection.Negate()))
                    {
                        // Verifica si el la primera pasada
                        if (banderaAbajo)
                        {
                            // Obtiene la distancia a mover
                            //distancia = tra.Inverse.OfVector(Tools.ProyectarVectorSobreDireccionYSentido((bbElem.Min - bbBar.Max), vista.UpDirection.Negate())) / 2 - barAlto;
                            XYZ proyectado = Tools.ProyectarVectorSobreDireccionYSentido(bbElem.Min - representacion.Representacion.get_BoundingBox(vista).Max, vista.UpDirection.Negate());
                            distancia = tra.Inverse.OfVector(proyectado) - barAlto - etiquetaAlto;

                            // Cambia el estado de la bandera
                            banderaAbajo = false;
                        }
                        else
                        {
                            distancia -= (barAlto + etiquetaAlto);
                        }
                    }

                    // Verifica si la dirección es derecha
                    else if (direccion.IsAlmostEqualTo(vista.RightDirection))
                    {
                        // Verifica si el la primera pasada
                        if (banderaDerecha)
                        {
                            //distancia = tra.Inverse.OfVector(Tools.ProyectarVectorSobreDireccionYSentido((bbElem.Max - bbBar.Min), vista.RightDirection))/2 + barAncho;
                            XYZ proyectado = Tools.ProyectarVectorSobreDireccionYSentido(bbElem.Max - representacion.Representacion.get_BoundingBox(vista).Min, vista.RightDirection);
                            distancia = tra.Inverse.OfVector(proyectado) + barAncho + etiquetaAncho;

                            // Cambia el estado de la bandera
                            banderaDerecha = false;
                        }
                        else
                        {
                            distancia += barAncho + etiquetaAncho;
                        }
                    }

                    // Verifica si la dirección es izquierda
                    else
                    {
                        // Verifica si el la primera pasada
                        if (banderaIzquierda)
                        {
                            //distancia = tra.Inverse.OfVector(Tools.ProyectarVectorSobreDireccionYSentido((bbElem.Min - bbBar.Max), vista.RightDirection.Negate()))/2 - barAncho;
                            XYZ proyectado = Tools.ProyectarVectorSobreDireccionYSentido(bbElem.Min - representacion.Representacion.get_BoundingBox(vista).Max, vista.RightDirection.Negate());
                            distancia = tra.Inverse.OfVector(proyectado) - barAncho - etiquetaAncho;

                            // Cambia el estado de la bandera
                            banderaIzquierda = false;
                        }
                        else
                        {
                            distancia -= (barAlto + etiquetaAlto);
                        }
                    }

                    // Lo lleva a coordenadas globales
                    ElementTransformUtils.MoveElement(this.doc, representacion.Representacion.Id, tra.OfVector(distancia));

                    if (representacion.Etiqueta != null)
                    {
                        try
                        {
                            ElementTransformUtils.MoveElement(this.doc, representacion.Etiqueta.Id, tra.OfVector(distancia));

                            XYZ distanciaEtiqueta = Tools.ProyectarVectorSobreDireccionYSentido(tra.OfVector(etiquetaDimension), representacion.PosicionEtiqueta);

                            ElementTransformUtils.MoveElement(this.doc, representacion.Etiqueta.Id, distanciaEtiqueta);
                        }
                        catch (Exception) { }
                    }
                }
                catch (Exception) { }

                this.doc.Regenerate();
            }
        }
    }
}