using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Diagnostics;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.UI.Events;
using Jump.ViewModels;

namespace Jump.Models
{
    public class DetalleAutomaticoModel : ModelBase
    {
        // Variable necesarias
        string transaccionGeneral = "Transacción general";
        int posicionImagenPreview = 0;
        Transaction traGeneral;

        // Detalle de armadura
        Type claseDetalleBarra = typeof(RebarBendingDetailType);
        BuiltInCategory categoriaDetalleBarra = BuiltInCategory.OST_RebarBendingDetails;

        // Parámetros para las vistas
        ViewFamily seccion = ViewFamily.Section;
        ViewFamily planoEstructural = ViewFamily.StructuralPlan;
        DisplayStyle estiloVista = DisplayStyle.FlatColors;
        ViewDetailLevel nivelDetalle = ViewDetailLevel.Fine;

        // Parámetros para las etiquetas y cotas
        BuiltInCategory categoriaEtiquetaArmadura = BuiltInCategory.OST_RebarTags;
        DimensionStyleType cotaEstiloLineal = DimensionStyleType.Linear;

        // Lista de objetos
        List<ViewFamilyType> tipoVistasSeccion = new List<ViewFamilyType>();
        List<ViewFamilyType> tipoVistasPlanoEstructural = new List<ViewFamilyType>();
        List<View> plantillas = new List<View>();
        List<FamilySymbol> etiquetasElemento = new List<FamilySymbol>();
        List<FamilySymbol> etiquetasArmaduras = new List<FamilySymbol>();
        List<Element> detalleArmadura = new List<Element>();
        List<DimensionType> cotasLineales = new List<DimensionType>();
        List<SpotDimensionType> cotasElevacion = new List<SpotDimensionType>();

        // Objetos creados
        List<Element> listaEtiquetasCreadas = new List<Element>();
        //List<Familia> familiasEstructurales = new List<Familia>();
        //List<Element> listaElementosEstructurales = new List<Element>();
        //List<Element> elementos = new List<Element>();
        //List<Element> elementosVistaPreview = new List<Element>();
        //List<Element> etiquetasLongitud = new List<Element>();

        public DetalleAutomaticoModel(UIApplication UIApp)
        {
            this.UIApp = UIApp;
            this.UIDoc = UIApp.ActiveUIDocument;
            this.Appli = UIApp.Application;
            this.Doc = UIDoc.Document;

            traGeneral = new Transaction(this.Doc, transaccionGeneral);
            traGeneral.Start();

            tipoVistasSeccion = new FilteredElementCollector(Doc)
                                .OfClass(typeof(ViewFamilyType))
                                .Cast<ViewFamilyType>()
                                .Where(v => v.ViewFamily == seccion)
                                .OrderBy(x => x.Name).ToList();

            tipoVistasPlanoEstructural = new FilteredElementCollector(Doc)
                                        .OfClass(typeof(ViewFamilyType))
                                        .Cast<ViewFamilyType>()
                                        .Where(v => v.ViewFamily == planoEstructural)
                                        .OrderBy(x => x.Name).ToList();

            plantillas = new FilteredElementCollector(Doc)
                                .OfClass(typeof(Autodesk.Revit.DB.View))
                                .Cast<Autodesk.Revit.DB.View>()
                                .Where(v => v.IsTemplate)
                                .OrderBy(x => x.Name).ToList();

            etiquetasArmaduras = Tools.ObtenerEtiquetasIndependientes(this.Doc, categoriaEtiquetaArmadura);
            
            detalleArmadura = Tools.ObtenerTodosTiposSegunClaseYCategoria(this.Doc, claseDetalleBarra, categoriaDetalleBarra);

            cotasLineales = Tools.ObtenerCotas(this.Doc, cotaEstiloLineal);

            cotasElevacion = Tools.ObtenerCotasElevacion(this.Doc);
        }

        /// <summary> Prefijo para obtener el lenguaje </summary>
        public string Clave { get; set; }

        /// <summary> Clase de los elementos estructurales en Revit </summary>
        public Type Clase { get; set; }

        /// <summary> Categoría del elemento estructural </summary>
        public BuiltInCategory Categoria { get; set; }

        /// <summary> Categoría de la etiqueta para el elemento estructural </summary>
        public BuiltInCategory CategoriaEtiqueta { get; set; }

        /// <summary> Indice del Combobox para la escala de la vista </summary>
        public int IndiceComboboxEscalaVista { get; set; }

        /// <summary> Posición de la etiqueta independiente del elemento estructural </summary>
        public int PosicionEtiquetaIndependienteElemento { get; set; }

        /// <summary> Posición de la etiqueta independiente de la armadura </summary>
        public int PosicionEtiquetaIndependienteArmadura { get; set; }

        /// <summary> Posición de la cota de profundidad del elemento estructural </summary>
        public int PosicionEtiquetaCotaProfundidad { get; set; }

        /// <summary> Cota lineal vertical izquierda del elemento estructural </summary>
        public bool CotaVerticalIzquierda { get; set; }

        /// <summary> Cota lineal vertical derecha del elemento estructural </summary>
        public bool CotaVerticalDerecha { get; set; }

        /// <summary> Cota lineal horizontal arriba del elemento estructural </summary>
        public bool CotaHorizontalArriba { get; set; }

        /// <summary> Cota lineal horizontal abajo del elemento estructural </summary>
        public bool CotaHorizontalAbajo { get; set; }

        /// <summary> Lista de elementos seleccionados en Revit </summary>
        public List<ElementId> ListaSeleccionados { get; set; }

        /// <summary> Obtiene todos los tipos de vistas para la sección </summary>
        public ObservableCollection<Familia> ObtenerTiposDeSecciones()
        {
            ObservableCollection<Familia> tiposDeVistas = Familia.ObtenerFamilia(this.tipoVistasSeccion.Cast<Element>().ToList());

            return tiposDeVistas;
        }

        /// <summary> Obtiene todos los tipos de vistas para la sección </summary>
        public ObservableCollection<Familia> ObtenerTiposDePlanosEstructurales()
        {
            ObservableCollection<Familia> tiposPlanoEstructura = Familia.ObtenerFamilia(this.tipoVistasPlanoEstructural.Cast<Element>().ToList());

            return tiposPlanoEstructura;
        }

        /// <summary> Obtiene todas las plantillas de vistas </summary>
        public ObservableCollection<Familia> ObtenerTiposDePlantillas()
        {
            ObservableCollection<Familia> plantillas = Familia.ObtenerFamilia(this.plantillas.Cast<Element>().ToList());

            return plantillas;
        }

        /// <summary> Obtiene todas las etiquetas para el elemento estructural </summary>
        public ObservableCollection<Familia> ObtenerEtiquetasElemento()
        { 
            etiquetasElemento = Tools.ObtenerEtiquetasIndependientes(this.Doc, CategoriaEtiqueta);

            ObservableCollection<Familia> etiquetas = Familia.ObtenerFamilia(etiquetasElemento);

            return etiquetas;
        }

        /// <summary> Obtiene todas las etiquetas para las armaduras </summary>
        public ObservableCollection<Familia> ObtenerEtiquetasArmadura()
        {
            ObservableCollection<Familia> etiquetas = Familia.ObtenerFamilia(this.etiquetasArmaduras);

            return etiquetas;
        }

        /// <summary> Obtiene todos los tipos de detalles de armaduras </summary>
        public ObservableCollection<Familia> ObtenerTiposDeDetalleDeArmadura()
        {
            ObservableCollection<Familia> detalles = Familia.ObtenerFamilia(this.detalleArmadura);

            return detalles;
        }

        /// <summary> Obtiene todos los tipos de cotas lineales </summary>
        public ObservableCollection<Familia> ObtenerTiposDeCotaLineales()
        {
            ObservableCollection<Familia> tipos = Familia.ObtenerFamilia(this.cotasLineales.Cast<Element>().ToList());

            return tipos;
        }

        /// <summary> Obtiene todos los tipos de cotas de elevación </summary>
        public ObservableCollection<Familia> ObtenerTiposDeCotaDeElevacion()
        {
            ObservableCollection<Familia> tipos = Familia.ObtenerFamilia(this.cotasElevacion.Cast<Element>().ToList());

            return tipos;
        }

        /// <summary> Obtiene una colección observable con todas las instancias de familias </summary>
        public ObservableCollection<Familia> ObtenerTodasLasFamilias()
        {
            List<Element> lista = Tools.ObtenerTodosEjemplaresSegunClaseYCategoria(this.Doc, Clase, Categoria);

            // Elimina los subelementos
            try { lista = Tools.EliminarSubelementos(lista); } catch (Exception) { }

            return Familia.ObtenerFamilia(lista);
        }

        /// <summary> Obtiene una colección observable con todas las instancias de familias seleccionadas en Revit </summary>
        public ObservableCollection<Familia> ObtenerFamiliasSeleccionadasEnRevit()
        {
            List<Element> lista = Tools.ObtenerElementosSeleccionadosEnProyecto(this.UIDoc, this.Doc);

            lista = lista.Where(x => x.GetType() == this.Clase && x.Category.BuiltInCategory == Categoria).ToList();

            return Familia.ObtenerFamilia(lista);
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

        /// <summary> Crea las etiquetas de un elemento en la vista preview </summary>
        public void CrearEtiquetasDeElementoParaPreview(DetAutUserControlViewModel ucvm, View vista, Element elem)
        {
            if (vista != null && elem != null)
            {
                Debug.WriteLine("Cambia de etiquetas");
            }
        }

        /// <summary> Muestra el elemento estructural, sus barras y ajusta el recuadro de la vista </summary>
        private View MostrarElementoBarrasYAjustarRecuadroDeVista(View vista, Element elem)
        {
            Tools.MostrarSolamenteElementoYBarrasEnVista(this.Doc, vista, elem);

            vista = Tools.AjustarRecuadroDeVista(vista, elem, Tools.ObtenerArmadurasDeElemento(elem, vista));

            return vista;
        }

        /// <summary> Asigna el tipo de seccion y la plantilla a la vista </summary>
        private void AsignarTipoYPlantillaAVista(View vista, DetAutUserControlViewModel ucvm)
        {
            ViewFamilyType vft = Familia.ObtenerElemento(ucvm.TipoDeVista) as ViewFamilyType;

            View plantilla = Familia.ObtenerElemento(ucvm.PlantillaDeVista) as View;

            if (vista != null)
            {
                if (vft != null && vft.Id != ElementId.InvalidElementId)
                {
                    try { vista.ChangeTypeId(vft.Id); } catch (Exception) { }
                }

                if (plantilla != null && plantilla.Id != ElementId.InvalidElementId && ucvm.PlantillaDeVistaBool)
                {
                    try
                    {
                        vista.ViewTemplateId = plantilla.Id;

                        vista.CropBoxActive = true;
                    }
                    catch (Exception) { }
                }

                if (vista.ViewTemplateId == ElementId.InvalidElementId)
                {
                    vista = Tools.CambiarConfiguracionVista(ucvm.Escala, this.Doc, vista, nivelDetalle, estiloVista);
                }
            }

            this.Doc.Regenerate();
        }

        /// <summary> Crea la vista XX </summary>
        public View CrearVistaXX(Element elem, DetAutUserControlViewModel ucvm)
        {
            // Crea la vista XX
            View vista = Tools.VistaXX(this.Doc, elem);

            this.Doc.Regenerate();

            AsignarTipoYPlantillaAVista(vista, ucvm);

            vista = MostrarElementoBarrasYAjustarRecuadroDeVista(vista, elem);

            return vista;
        }

        /// <summary> Crea la vista YY </summary>
        public View CrearVistaYY(Element elem, DetAutUserControlViewModel ucvm)
        {
            // Crea la vista YY
            View vista = Tools.VistaYY(this.Doc, elem);

            this.Doc.Regenerate();

            AsignarTipoYPlantillaAVista(vista, ucvm);

            vista = MostrarElementoBarrasYAjustarRecuadroDeVista(vista, elem);

            return vista;
        }

        /// <summary> Crea la vista en planta del elemento </summary>
        public View CrearVistaEnPlanta(Element elem, DetAutUserControlViewModel ucvm)
        {
            // Crea la vista en planta
            View vista = Tools.VistaEnPlanta(this.Doc, elem);

            this.Doc.Regenerate();

            AsignarTipoYPlantillaAVista(vista, ucvm);

            vista = MostrarElementoBarrasYAjustarRecuadroDeVista(vista, elem);

            return vista;
        }

        /// <summary> Crea las vistas y las etiquetas de las vistas </summary>
        public void CrearVistasYEtiquetas(DetalleAutomaticoViewModel detAutVM)
        {
            DetAutUserControlViewModel ucvmXX = detAutVM.DetAutoSeccXX;
            DetAutUserControlViewModel ucvmYY = detAutVM.DetAutoSeccYY;
            DetAutUserControlViewModel ucvmPlanta = detAutVM.DetAutoPlanta;

            CerrarTransacciónGeneral();

            int contX = (detAutVM.VistaXX) ? ucvmXX.FamiliasSeleccionadas.Count : 0;
            int contY = (detAutVM.VistaYY) ? ucvmYY.FamiliasSeleccionadas.Count : 0;
            int contZ = (detAutVM.VistaPlanta) ? ucvmPlanta.FamiliasSeleccionadas.Count : 0;

            // Llama al formulario barra de progreso
            frmBarraProgreso barraProgreso = new frmBarraProgreso(contX + contY + contZ);

            // Muestra el formulario
            barraProgreso.Show();

            using (Transaction tra = new Transaction(this.Doc, Language.ObtenerTexto(AboutJump.IdiomaAddin, Clave + "5-1")))
            {
                tra.Start();

                if (detAutVM.VistaXX)
                {
                    foreach (Familia fa in ucvmXX.FamiliasSeleccionadas)
                    {
                        if (barraProgreso.Cancelado())
                        {
                            break;
                        }

                        Element elem = Familia.ObtenerElemento(fa);

                        View vista = CrearVistaXX(elem, ucvmXX);

                        CrearEtiquetasDeElemento(ucvmXX, vista, elem);

                        barraProgreso.Incrementar();
                    }
                }

                if (detAutVM.VistaYY)
                {
                    foreach (Familia fa in ucvmYY.FamiliasSeleccionadas)
                    {
                        if (barraProgreso.Cancelado())
                        {
                            break;
                        }

                        Element elem = Familia.ObtenerElemento(fa);

                        View vista = CrearVistaYY(elem, ucvmYY);

                        CrearEtiquetasDeElemento(ucvmYY, vista, elem);
                        
                        barraProgreso.Incrementar();
                    }
                }

                if (detAutVM.VistaPlanta)
                {
                    foreach (Familia fa in ucvmPlanta.FamiliasSeleccionadas)
                    {
                        if (barraProgreso.Cancelado())
                        {
                            break;
                        }

                        Element elem = Familia.ObtenerElemento(fa);

                        View vista = CrearVistaXX(elem, ucvmPlanta);

                        CrearEtiquetasDeElemento(ucvmPlanta, vista, elem);
                        
                        barraProgreso.Incrementar();
                    }
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

                barraProgreso.Close();
            }
        }

        /// <summary> Crea las etiquetas de un elemento en una vista particular </summary>
        public void CrearEtiquetasDeElemento(DetAutUserControlViewModel ucvm, View vista, Element elem)
        {
            if (vista != null && elem != null)
            {
                vista = MostrarElementoBarrasYAjustarRecuadroDeVista(vista, elem);

                listaEtiquetasCreadas.Clear();

                // Crea la lista de cotas en la vista
                List<Dimension> listaCotas = new List<Dimension>();

                // Cota lineal
                if (ucvm.CotaLinealBool)
                {
                    // Obtiene el DimensionType de la cota seleccionada
                    DimensionType tipoCota = (DimensionType)Familia.ObtenerElemento(ucvm.CotaLineal);

                    // Verifica que esté activo la cota vertical izquierda
                    if (this.CotaVerticalIzquierda)
                    {
                        try
                        {
                            // Crea la cota vertical izquierda
                            listaCotas.Add(Tools.CrearCotaVerticalIzquierdaParaElemento(this.Doc, vista, elem, tipoCota));
                        }
                        catch (Exception) { }
                    }

                    // Verifica que esté activo la cota vertical derecha
                    if (this.CotaVerticalDerecha)
                    {
                        try
                        {
                            // Crea la cota vertical derecha
                            listaCotas.Add(Tools.CrearCotaVerticalDerechaParaElemento(this.Doc, vista, elem, tipoCota));
                        }
                        catch (Exception) { }
                    }

                    // Verifica que esté activo la cota horizontal arriba
                    if (this.CotaHorizontalArriba)
                    {
                        try
                        {
                            // Crea la cota horizontal arriba
                            listaCotas.Add(Tools.CrearCotaHorizontalArribaParaElemento(this.Doc, vista, elem, tipoCota));
                        }
                        catch (Exception) { }
                    }

                    // Verifica que esté activo la cota horizontal abajo
                    if (this.CotaHorizontalAbajo)
                    {
                        try
                        {
                            // Crea la cota horizontal abajo
                            listaCotas.Add(Tools.CrearCotaHorizontalAbajoParaElemento(this.Doc, vista, elem, tipoCota));
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
                if (ucvm.EtiquetaElementoBool)
                {
                    try
                    {
                        // Obtiene el FamilySymbol de la etiqueta seleccionada
                        FamilySymbol tipoEtiqueta = (FamilySymbol)Familia.ObtenerElemento(ucvm.EtiquetaElemento);

                        // Crea una etiqueta independiente del elemento
                        IndependentTag etiqueta = Tools.CrearEtiquetaSegunConfiguracion(this.Doc, vista, elem, tipoEtiqueta, this.PosicionEtiquetaIndependienteElemento);

                        if (listaCotas.Count > 0)
                        {
                            // Obtiene la dirección según las configuraciones
                            XYZ direccion = Tools.DireccionSegunPosicionDeEtiqueta(vista, elem, etiqueta, this.PosicionEtiquetaIndependienteElemento);

                            // Obtiene el vector para mover la etiqueta
                            XYZ vector = Tools.ObtenerVectorParaMoverEtiqueta(vista, direccion, etiqueta, listaCotas);

                            // Mueve la etiqueta
                            ElementTransformUtils.MoveElement(this.Doc, etiqueta.Id, vector);
                        }

                        // Agrega la etiqueta a la lista
                        listaEtiquetasCreadas.Add(etiqueta);
                    }
                    catch (Exception) { }
                }

                // Cota de elevación
                if (ucvm.CotaProfundidadBool)
                {
                    try
                    {
                        // Obtiene el SpotDimensionType de la cota de profundidad seleccionada
                        SpotDimensionType tipoCotaProfundidad = (SpotDimensionType)Familia.ObtenerElemento(ucvm.CotaProfundidad);

                        // Crea la cota de profundidad
                        SpotDimension cotaProfundidad = Tools.CrearCotaProfundidad(this.Doc, vista, elem, tipoCotaProfundidad, this.PosicionEtiquetaCotaProfundidad);

                        // Agrega la cota de profundidad a la lista
                        listaEtiquetasCreadas.Add(cotaProfundidad);
                    }
                    catch (Exception) { }
                }

                // Obtiene todas las armaduras del elemento
                List<Rebar> todasBarras = Tools.ObtenerArmadurasDeElemento(elem, vista);

                // Obtiene las armaduras que su plano sea paralelo al de la vista
                List<Rebar> barras = Tools.ObtenerArmaduraPerpendicularVista(vista, todasBarras);

                // Crea la lista de Representacion de Armaduras
                List<ArmaduraRepresentacion> listaArmaduraRepresentacion = new List<ArmaduraRepresentacion>();

                // Recorre todas las armaduras que posee el elemento
                foreach (Rebar barra in barras)
                {
                    IndependentTag etiquetaArmadura = null;

                    // Etiqueta de armadura
                    if (ucvm.EtiquetaArmaduraBool)
                    {
                        try
                        {
                            // Obtiene el FamilySymbol de la etiqueta seleccionada
                            FamilySymbol tipoEtiqueta = (FamilySymbol)Familia.ObtenerElemento(ucvm.EtiquetaArmadura);

                            // Crea la etiqueta independiente de la barra
                            etiquetaArmadura = Tools.CrearEtiquetaArmaduraSegunConfiguracion(this.Doc, vista, barra, tipoEtiqueta, this.PosicionEtiquetaIndependienteArmadura);

                            // Agrega la etiqueta de armadura a la lista
                            listaEtiquetasCreadas.Add(etiquetaArmadura);
                        }
                        catch (Exception) { }
                    }

                    // Longitud parcial de barra
                    if (ucvm.DetalleArmaduraBool)
                    {
                        try
                        {
                            RebarBendingDetailType tipoBarra = (RebarBendingDetailType)Familia.ObtenerElemento(ucvm.DetalleArmadura);

                            XYZ baricentro = Tools.ObtenerBaricentroDeRecuadro(barra.get_BoundingBox(vista));

                            IndependentTag representacionArmadura = RebarBendingDetail.Create(Doc, vista.Id, barra.Id, Jump.Properties.Settings.Default.PosicionBarraADibujar, tipoBarra, baricentro, 0) as IndependentTag;

                            this.Doc.Regenerate();

                            listaEtiquetasCreadas.Add(representacionArmadura);

                            ArmaduraRepresentacion armadura = new ArmaduraRepresentacion(representacionArmadura, etiquetaArmadura, this.PosicionEtiquetaIndependienteArmadura);

                            listaArmaduraRepresentacion.Add(armadura);
                        }
                        catch (Exception) { }
                    }

                    // Regenera el documento
                    this.Doc.Regenerate();
                }

                // Mueve los despieces de Armaduras
                OrdenarYMoverRepresentacionArmaduraSegunDireccion(vista, elem, listaArmaduraRepresentacion);
            }
        }

        ///<summary> Ordena y mueve las Represetaciones de Armaduras según las opciones </summary>
        private void OrdenarYMoverRepresentacionArmaduraSegunDireccion(View vista, Element elem, List<ArmaduraRepresentacion> armaduras)
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

                    XYZ direccionPrincipal = tra.Inverse.OfVector(Tools.ObtenerNormalADireccionPrincipalArmadura(this.Doc, vista, representacion.Representacion.GetTaggedLocalElements().FirstOrDefault() as Rebar));

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
        private void OrganizarListaSegunDireccionDeBarra(View vista, XYZ distanciaRelativa, ArmaduraRepresentacion representacion,
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
        private void OrdenarYMoverListaConArmadurasRepresentacion(View vista, Transform tra, Element elem,
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
        private void MoverListaConArmaduras(View vista, Element elem, XYZ direccion, List<ArmaduraRepresentacion> armaduras)
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
                    ElementTransformUtils.MoveElement(this.Doc, representacion.Representacion.Id, tra.OfVector(distancia));

                    if (representacion.Etiqueta != null)
                    {
                        try
                        {
                            ElementTransformUtils.MoveElement(this.Doc, representacion.Etiqueta.Id, tra.OfVector(distancia));

                            XYZ distanciaEtiqueta = Tools.ProyectarVectorSobreDireccionYSentido(tra.OfVector(etiquetaDimension), representacion.PosicionEtiqueta);

                            ElementTransformUtils.MoveElement(this.Doc, representacion.Etiqueta.Id, distanciaEtiqueta);
                        }
                        catch (Exception) { }
                    }
                }
                catch (Exception) { }

                this.Doc.Regenerate();
            }
        }
    }
}
