using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using SpreadsheetLight;
using DocumentFormat.OpenXml;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.DB.ExtensibleStorage;
using Revit.ES.Extension.ElementExtensions;

namespace Jump
{
    public static class Tools
    {
        #region Variables necesarias para las funciones

        #region Precisión para ordenar los elementos

        // Precisión para el orden
        private static int precisionOrdenarX = Properties.Settings.Default.precisionOrdenarX;
        private static int precisionOrdenarY = Properties.Settings.Default.precisionOrdenarY;

        #endregion

        #region Nombre para el DataGridView de diámetros y estilos de líneas

        #endregion

        #region Creación de archivo Excel

        // Ruta del archivo de excel y Json
        public static string formatoArchivoDiametroYEstilo = ".xlsx";
        public static string formatoArchivoArmaduras = "Rebar.xlsx";
        public static string formatoArchivoRevitDocumento = ".rvt";
        public static string formatoArchivoRevitFamilia = ".rfa";
        public static string formatoArchivoRevitPlantilla = ".rte";
        public static string formatoArchivoJson = ".json";

        // Nombre de la pestaña para los diámetros
        private static string pestanaExcelDiametros = "Sheet1";

        // Celda donde se inserta la tabla para el excel
        private static string celdaExcelInsertarDiametros = "A1";
        private static bool celdaExcelEncabezadoDiametros = false;

        // Nombre de la pestaña para las armaduras
        private static string pestanaExcelArmaduras = "Sheet2";

        // Celda donde se inserta la tabla de las armaduras para el excel
        private static string celdaExcelInsertarArmaduras = "A1";
        private static bool celdaExcelEncabezadoArmaduras = false;

        #endregion

        #region Parámetros para el dibujo de las armaduras

        // Tipo de unidad para el texto de las barras
        private static UnitType tipoUnidadTexto = UnitType.UT_Reinforcement_Length;

        // Ajustar las curvas si se superponen
        private static bool superposicion = false;

        // Omitir ganchos
        private static bool ganchos = false;

        // Omitir los diámetros de doblez
        private static bool radioDeGiro = false;

        // Define si es Multi-Planar o no
        static MultiplanarOption multiplePlano = MultiplanarOption.IncludeOnlyPlanarCurves;

        // Selecciona cual barra del conjunto dibuja
        private static int posicionBarra = 0;

        // Texto para la escala
        private static string escalaInicio = "1 : ";

        // Valor de tolerancia para vectores
        private const double toleranciaComponenteVector = 1.0e-9;

        // Punto donde se coloca el texto para las longitudes parciales de armaduras
        private static double puntoDeCurvaParaTexto = 0.5;

        #endregion

        #region Numeración de las posiciones para las etiquetas

        // Posiciones de las etiquetas independientes
        public const int ArribaIzquierda = 1;
        public const int ArribaCentro = 2;
        public const int ArribaDerecha = 3;
        public const int CentroIzquierda = 4;
        public const int CentroMedio = 5;
        public const int CentroDerecha = 6;
        public const int AbajoIzquierda = 7;
        public const int AbajoCentro = 8;
        public const int AbajoDerecha = 9;

        // Lista con las posiciones de las etiquetas
        public static List<int> listaPosicionEtiquetas = new List<int>()
        {
            ArribaIzquierda, ArribaCentro, ArribaDerecha,
            CentroIzquierda, CentroMedio, CentroDerecha,
            AbajoIzquierda, AbajoCentro, AbajoDerecha,
        };

        #endregion

        // Lista de escalas para la vista
        private static List<int> listaEscalas = CompletarEscalas();

        // Multiplica la distancia a mover para las cotas lineales
        private static double multiplicadorDistanciaCotasLinealesDerecha = 2;

        // Multiplica la distancia a mover para las cotas lineales
        private static double multiplicadorDistanciaCotasLinealesAbajo = 1.25;

        // Punto para realizar el corte transversal para lineas
        private static double corteTransversalBasadoLinea = 0.5;

        #region Funciones para las variables

        ///<summary> Completa una lista con las escalas del proyecto </summary>
        private static List<int> CompletarEscalas()
        {
            // Crea la lista a devolver
            List<int> lista = new List<int>();

            // Completa la lista con las escalas
            lista.Add(1);
            lista.Add(2);
            lista.Add(5);
            lista.Add(10);
            lista.Add(20);
            lista.Add(25);
            lista.Add(50);
            lista.Add(100);
            lista.Add(200);
            lista.Add(500);
            lista.Add(1000);
            lista.Add(2000);
            lista.Add(5000);

            return lista;
        }

        // Lista de elementos filtrados por clases
        private static List<ElementFilter> filtroElementosPorClase = new List<ElementFilter>()
        {
            new ElementClassFilter(typeof(Floor)),
            new ElementClassFilter(typeof(HostedSweep)),
            new ElementClassFilter(typeof(Wall)),
            new ElementClassFilter(typeof(WallFoundation)),
        };

        // Crea un arreglo con todas las categorías estructurales
        private static BuiltInCategory[] categoriasEstructurales = new BuiltInCategory[]
        {
            BuiltInCategory.OST_EdgeSlab,
            BuiltInCategory.OST_FloorsStructure,
            BuiltInCategory.OST_StructConnectionAnchors,
            BuiltInCategory.OST_StructConnectionPlates,
            BuiltInCategory.OST_StructConnectionWelds,
            BuiltInCategory.OST_StructuralColumns,
            BuiltInCategory.OST_StructuralFraming,
            BuiltInCategory.OST_StructuralFoundation,
            BuiltInCategory.OST_WallsStructure
        };

        #endregion

        #endregion

        #region Idiomas

        ///<summary> Verifica el idioma para cargar la aplicación </summary>
        public static string VerificarIdioma(string I)
        {
            bool bandera = false;

            // Recorre todos los idiomas disponibles
            foreach (string idioma in Language.IdiomasDisponibles)
            {
                // Verifica que el idioma que recibió esté en el vector de idiomas disponibles
                if (I == idioma)
                {
                    bandera = true;
                }
            }

            // Asigna español como el idioma del programa
            if (!bandera)
            {
                I = "Español";
            }
            return I;
        }

        ///<summary> Verifica que el vector de idiomas disponibles tenga contenido </summary>
        public static void CargarIdiomas()
        {
            if (Language.IdiomasDisponibles.Count == 0)
            {
                // Carga los idiomas disponibles al vector principal
                Language.CargarIdiomasDisponibles();
            }
        }

        ///<summary> Devuelve el idioma guardado en las propiedades </summary>
        public static string ObtenerIdiomaDelPrograma()
        {
            string I;

            // Busca el idioma en las preferencias
            try
            {
                I = Properties.Settings.Default["IdiomaDelPrograma"].ToString();
            }

            catch (Exception)
            {
                I = "Español";
            }

            return I;
        }

        #endregion
        
        #region Addin Manager

        /// <summary> Carga los idiomas y paises disponibles que el Addin Manager no tire error </summary>
        public static void AddinManager()
        {
            // Carga los idiomas
            CargarIdiomas();

            // Asigna el texto del idioma
            Language.CargaTextosDeCadaIdioma();

            // Carga los paises disponibles
            Pais.CargarPaisesDisponibles();
        }

        #endregion

        #region Visibilidad de Armaduras y Elementos

        ///<summary> Cambia la visibilidad a sin tapar o tapado de la armadura </summary>
        public static void ArmaduraVisible(Document doc, View vistaActual, ElementId vistaActualID, bool visibilidad)
        {
            // Crea una colección de barras de acero de la vista actual
            ICollection<Element> barras1 = new FilteredElementCollector(doc, vistaActualID).OfClass(typeof(Rebar)).ToElements();
            ICollection<Element> barras2 = new FilteredElementCollector(doc, vistaActualID).OfClass(typeof(RebarInSystem)).ToElements();

            // Recorre todas las barras y activa o desactiva
            foreach (Rebar r in barras1)
            {
                try
                {
                    // Activa o desactiva la visibilidad de la armadura
                    r.SetUnobscuredInView(vistaActual, visibilidad);
                }
                catch (Exception)
                {
                    continue;
                }
            }

            // Recorre todas las barras y activa o desactiva
            foreach (RebarInSystem r in barras2)
            {
                try
                {
                    // Activa o desactiva la visibilidad de la armadura
                    r.SetUnobscuredInView(vistaActual, visibilidad);
                }
                catch (Exception)
                {
                    continue;
                }
            }
        }

        ///<summary> Cambia la visibilidad a solido o filamento de la armadura </summary>
        public static void ArmaduraSolida(Document doc, View3D vista3D, ElementId vistaActualID, bool visibilidad)
        {
            // Crea una colección de barras de acero de la vista actual
            ICollection<Element> barras1 = new FilteredElementCollector(doc, vistaActualID).OfClass(typeof(Rebar)).ToElements();
            ICollection<Element> barras2 = new FilteredElementCollector(doc, vistaActualID).OfClass(typeof(RebarInSystem)).ToElements();

            // Recorre todas las barras y activa o desactiva
            foreach (Rebar r in barras1)
            {
                try
                {
                    // Activa o desactiva el solido de la armadura
                    r.SetSolidInView(vista3D, visibilidad);
                }
                catch (Exception)
                {
                    continue;
                }
            }

            // Recorre todas las barras y activa o desactiva
            foreach (RebarInSystem r in barras2)
            {
                try
                {
                    // Activa o desactiva el solido de la armadura
                    r.SetSolidInView(vista3D, visibilidad);
                }
                catch (Exception)
                {
                    continue;
                }
            }
        }
        
        ///<summary> Cambia la visibilidad a visible de los elementos en la vista </summary>
        public static void MostrarElementosVista(Document doc, View vista, List<Element> lista)
        {
            // Crea una lista de ElementID
            List<ElementId> listaID = new List<ElementId>();

            // Verifica que la visibilidad sea en algunas de estas vistas
            if (vista.ViewType == ViewType.CeilingPlan ||
                vista.ViewType == ViewType.Elevation ||
                vista.ViewType == ViewType.EngineeringPlan ||
                vista.ViewType == ViewType.FloorPlan ||
                vista.ViewType == ViewType.Section ||
                vista.ViewType == ViewType.SystemBrowser ||
                vista.ViewType == ViewType.ThreeD)
            {
                // Recorre todos los elementos de la lista y obtiene su ID
                foreach (Element elem in lista)
                {
                    try
                    {
                        if (elem.CanBeHidden(vista))
                        {
                            listaID.Add(elem.Id);
                        }
                    }
                    catch (Exception) { }
                }

                // Verifica que haya elementos en la lista
                if (listaID.Count > 0)
                {
                    // Muestra los elementos en la vista actual
                    vista.UnhideElements(listaID);
                }
            }
        }

        ///<summary> Cambia la visibilidad a oculto de los elementos en la vista </summary>
        public static void OcultarElementosVista(Document doc, View vista, List<Element> lista)
        {
            // Crea una lista de ElementID
            List<ElementId> listaID = new List<ElementId>();
            
            // Verifica que la visibilidad sea en algunas de estas vistas
            if (vista.ViewType == ViewType.CeilingPlan ||
                vista.ViewType == ViewType.Elevation ||
                vista.ViewType == ViewType.EngineeringPlan ||
                vista.ViewType == ViewType.FloorPlan ||
                vista.ViewType == ViewType.Section ||
                vista.ViewType == ViewType.SystemBrowser ||
                vista.ViewType == ViewType.ThreeD)
            {
                // Recorre todos los elementos de la lista y obtiene su ID
                foreach (Element elem in lista)
                {                    
                    // Verifica si el elemento se puede ocultar
                    if (elem.CanBeHidden(vista))
                    {
                        // Oculta el elemento
                        listaID.Add(elem.Id);
                    }
                }
                // Verifica que haya elementos en la lista
                if (listaID.Count > 0)
                {
                    // Oculta los elementos en la vista actual
                    vista.HideElements(listaID);
                }
            }
        }

        ///<summary> Activa la visibilidad de las armaduras </summary>
        public static void ActivarVisibilidadArmaduras(Document doc, View vistaActual)
        {
            // Obtiene todas las barras del modelo
            List<Element> barras = new List<Element>();

            // Obtiene todos los ejemplares de armaduras
            barras = ObtenerTodosEjemplaresSegunCategoria(doc, BuiltInCategory.OST_Rebar);

            // Obtiene el Id de las barras del modelo
            ICollection<ElementId> barrasId = new List<ElementId>();

            // Obtiene todos ElementId de las armaduras
            barrasId = ObtenerIdElemento(barras);

            // Verifica que la lista no esté vacia
            if (barrasId.Count > 0)
            {
                // Muestra los elementos en la vista actual
                vistaActual.UnhideElements(barrasId);
            }
        }

        #endregion

        #region Lista de Categorias, Elementos, Parámetros de Tipo o Ejemplar

        /// <summary> Obtiene una lista ordenada alfabéticamente de categorias existentes en el Modelo </summary>
        public static List<Category> ObtenerCategorias(Document doc)
        {
            // Crea el colector
            FilteredElementCollector colector = new FilteredElementCollector(doc);

            // Filtra que los elementos sean de ejemplar
            List<Element> elementos = colector.WhereElementIsNotElementType().ToList();

            // Filtra según sea la categoría asignada
            List<Element> filtrados = (from elem in elementos
                                       where elem.Category != null
                                       && elem.Category.CategoryType == CategoryType.Model
                                       select elem).ToList();

            // Crea una lista vacia de categorias
            List<Category> categorias = new List<Category>();

            // Recorre los elementos filtrados
            foreach (Element elem in filtrados)
            {
                // Valida que la categoria NO exista en la lista
                if (!categorias.Exists(x => x.Id == elem.Category.Id))
                {
                    categorias.Add(elem.Category);
                }
            }

            // Ordena la lista alfabéticamente
            categorias = categorias.OrderBy(x => x.Name).ToList();

            return categorias;
        }

        /// <summary> Obtiene una lista ordenada alfabéticamente de todos los tipos según una categoría </summary>
        public static List<Element> ObtenerTodosTiposSegunCategoria(Document doc, BuiltInCategory categoria)
        {
            // Crea el colector
            FilteredElementCollector colector = new FilteredElementCollector(doc);

            // Filtra que los elementos sean de ejemplar
            List<Element> tipos = colector.WhereElementIsElementType().ToList();

            // Filtra según sean la categoría asignada
            List<Element> lista = (from elem in tipos
                                   where elem.Category != null
                                   && elem.Category.Id == new ElementId(categoria)
                                   select elem).ToList();

            // Ordena la lista alfabéticamente
            lista = lista.OrderBy(x => x.Name).ToList();

            return lista;
        }

        /// <summary> Obtiene una lista ordenada alfabéticamente de todos los tipos según una clase </summary>
        public static List<Element> ObtenerTodosTiposSegunClase(Document doc, Type clase)
        {
            // Crea el colector
            FilteredElementCollector colector = new FilteredElementCollector(doc);

            // Filtra que los elementos sean de ejemplar
            List<Element> tipos = colector.WhereElementIsElementType().OfClass(clase).ToList();

            // Filtra según sean la categoría asignada
            List<Element> lista = (from elem in tipos
                                   where elem.Category != null
                                   select elem).ToList();

            // Ordena la lista alfabéticamente
            lista = lista.OrderBy(x => x.Name).ToList();

            return lista;
        }

        /// <summary> Obtiene una lista de todos los ejemplares en el proyecto </summary>
        public static List<Element> ObtenerTodosEjemplares(Document doc)
        {
            // Crea el colector
            FilteredElementCollector colector = new FilteredElementCollector(doc);

            // Filtra que los elementos sean de ejemplar
            List<Element> elementos = colector.WhereElementIsNotElementType().ToList();

            // Filtra según sean la categoría asignada
            List<Element> lista = (from elem in elementos
                                   where elem.Category != null
                                   && elem.Category.Id != new ElementId(BuiltInCategory.OST_Views)
                                   && elem.Category.Id != new ElementId(BuiltInCategory.OST_Cameras)
                                   select elem).ToList();
            return lista;
        }

        /// <summary> Obtiene una lista ordenada alfabéticamente de todos los ejemplares según una categoría </summary>
        public static List<Element> ObtenerTodosEjemplaresSegunCategoria(Document doc, BuiltInCategory categoria)
        {
            // Crea el colector
            FilteredElementCollector colector = new FilteredElementCollector(doc);

            // Filtra que los elementos sean de ejemplar
            List<Element> elementos = colector.WhereElementIsNotElementType().ToList();

            // Filtra según sean la categoría asignada
            List<Element> lista = (from elem in elementos
                                   where elem.Category != null
                                   && elem.Category.Id == new ElementId(categoria)
                                   select elem).ToList();

            // Ordena la lista alfabéticamente
            lista = lista.OrderBy(x => x.Name).ToList();

            return lista;
        }

        /// <summary> Obtiene una lista ordenada alfabéticamente de todos los ejemplares según una clase </summary>
        public static List<Element> ObtenerTodosEjemplaresSegunClase(Document doc, Type clase)
        {
            // Crea el colector
            FilteredElementCollector colector = new FilteredElementCollector(doc);

            // Filtra que los elementos sean de ejemplar
            List<Element> elementos = colector.WhereElementIsNotElementType().OfClass(clase).ToList();

            // Filtra según sean la categoría asignada
            List<Element> lista = (from elem in elementos
                                   where elem.Category != null
                                   select elem).ToList();

            // Ordena la lista alfabéticamente
            lista = lista.OrderBy(x => x.Name).ToList();

            return lista;
        }

        /// <summary> Obtiene una lista ordenada alfabéticamente de todos los ejemplares según una clase y categoría </summary>
        public static List<Element> ObtenerTodosEjemplaresSegunClaseYCategoria(Document doc, Type clase, BuiltInCategory categoria)
        {
            // Crea el colector
            FilteredElementCollector colector = new FilteredElementCollector(doc);

            // Filtra que los elementos sean de ejemplar
            List<Element> elementos = colector.WhereElementIsNotElementType().OfClass(clase).ToList();

            // Filtra según sean la categoría asignada
            List<Element> lista = (from elem in elementos
                                   where elem.Category != null
                                   && elem.Category.Id == new ElementId(categoria)
                                   select elem).ToList();

            // Ordena la lista alfabéticamente
            lista = lista.OrderBy(x => x.Name).ToList();

            return lista;
        }

        /// <summary> Obtiene una lista de todos los ejemplares estructurales </summary>
        public static List<Element> ObtenerTodosEjemplaresEstructurales(Document doc)
        {
            // Crea un arreglo con todas las categorías estructurales
            BuiltInCategory[] categorias = categoriasEstructurales;

            // Crea una lista de filtros
            IList<ElementFilter> a = new List<ElementFilter>(categorias.Count());

            // Recorre cada categoria y agrega al al filtro 
            foreach (BuiltInCategory bic in categorias)
            {
                a.Add(new ElementCategoryFilter(bic));
            }

            // Crea un filtro que contiene un conjunto de filtros
            LogicalOrFilter filtroCategorias = new LogicalOrFilter(a);

            // Crea un filtro que contiene un conjunto de filtros
            LogicalAndFilter filtroFamilyInstance = new LogicalAndFilter
                                                    (filtroCategorias, new ElementClassFilter(typeof(FamilyInstance)));

            // Crea una lista de filtros por clase
            IList<ElementFilter> b = new List<ElementFilter>();

            // Recorre todas las clases de la lista
            foreach (ElementFilter elemFil in filtroElementosPorClase)
            {
                b.Add(elemFil);
            }

            // Agrega que tipo de clase tiene que filtrar
            b.Add(filtroFamilyInstance);
            
            // Crea el filtro final
            LogicalOrFilter filtroClases = new LogicalOrFilter(b);

            // Crea el colector
            FilteredElementCollector colector = new FilteredElementCollector(doc);

            // Toma el colector y lo pasa por el filtro
            colector.WherePasses(filtroClases);

            // Crea la lista y asigna los elementos que sean de ejemplar
            List<Element> elementos = colector.WhereElementIsNotElementType().ToList();

            // Ordena la lista alfabéticamente
            elementos = elementos.OrderBy(x => x.Name).ToList();

            return elementos;
        }

        /// <summary> Obtiene una lista de todos los ejemplares estructurales </summary>
        public static List<Element> ObtenerTodosEjemplaresEstructuralesActivos(Document doc)
        {
            // Crea la lista a devolver
            List<Element> estructura = new List<Element>();

            // Crea un arreglo con todas las categorías estructurales
            BuiltInCategory[] categorias = categoriasEstructurales;

            // Crea una lista de filtros
            IList<ElementFilter> a = new List<ElementFilter>(categorias.Count());

            // Recorre cada categoria y agrega al al filtro 
            foreach (BuiltInCategory bic in categorias)
            {
                a.Add(new ElementCategoryFilter(bic));
            }

            // Crea un filtro que contiene un conjunto de filtros
            LogicalOrFilter filtroCategorias = new LogicalOrFilter(a);

            // Crea un filtro que contiene un conjunto de filtros
            LogicalAndFilter filtroFamilyInstance = new LogicalAndFilter
                                                    (filtroCategorias, new ElementClassFilter(typeof(FamilyInstance)));

            // Crea una lista de filtros por clase
            IList<ElementFilter> b = new List<ElementFilter>();

            // Recorre todas las clases de la lista
            foreach (ElementFilter elemFil in filtroElementosPorClase)
            {
                b.Add(elemFil);
            }

            // Agrega que tipo de clase tiene que filtrar
            b.Add(filtroFamilyInstance);

            // Crea el filtro final
            LogicalOrFilter filtroClases = new LogicalOrFilter(b);

            // Crea el colector
            FilteredElementCollector colector = new FilteredElementCollector(doc);

            // Toma el colector y lo pasa por el filtro
            colector.WherePasses(filtroClases);

            // Crea la lista y asigna los elementos que sean de ejemplar
            List<Element> elementos = colector.WhereElementIsNotElementType().ToList();

            // Recorre la lista de elementos
            foreach (Element elem in elementos)
            {
                try
                {
                    // Verifica que el modelo analítico esté activado
                    if (Convert.ToBoolean(elem.get_Parameter(BuiltInParameter.STRUCTURAL_ANALYTICAL_MODEL).AsInteger()) == true)
                    {
                        estructura.Add(elem);
                    }
                }
                catch (Exception) { }
            }

            //Filtra que los elementos sean de la clase FamilyInstance
            List<Element> pilotes = colector.OfClass(typeof(FamilyInstance)).ToList();

            // Recorre la lista de pilotes
            foreach (Element elem in pilotes)
            {
                // Verifica que el elementos sea de la categoría
                if (elem.Category.Id == new ElementId(BuiltInCategory.OST_StructuralFoundation))
                {
                    // Agrega los pilotes (pilares de fundación)
                    estructura.Add(elem);
                }
            }
            
            // Ordena la lista alfabéticamente
            estructura = estructura.OrderBy(x => x.Name).ToList();

            return estructura;
        }

        /// <summary> Obtiene una lista de todos los ejemplares analíticos </summary>
        public static List<Element> ObtenerTodosEjemplaresAnaliticos(Document doc)
        {
            // Crea el colector
            FilteredElementCollector colector = new FilteredElementCollector(doc);

            // Filtra que los elementos sean de ejemplar
            List<Element> elementos = colector.WhereElementIsNotElementType().ToList();

            // Filtra según sean la categoría asignada
            List<Element> lista = (from elem in elementos
                                   where elem.Category != null
                                   && elem.Category.CategoryType == CategoryType.AnalyticalModel
                                   && elem.Category.Id != new ElementId(BuiltInCategory.OST_LoadCases)
                                   select elem).ToList();
            return lista;
        }

        /// <summary> Obtiene una lista de todos los subelementos que posee un elemento </summary>
        public static List<Element> ObtenerTodosSubElementos(Document doc, Element elem)
        {
            // Crea la lista de los subelementos
            List<Element> subElementos = new List<Element>();

            // Castea el elemento
            FamilyInstance fi = elem as FamilyInstance;

            // Verifica que el elemento tenga subelementos
            if (fi.GetSubComponentIds().Count > 0)
            {
                // Agrega a la lista
                subElementos = ObtenerElementoSegunID(doc, fi.GetSubComponentIds().ToList());
            }

            return subElementos;
        }

        ///<summary> Obtiene una lista ordenada alfabéticamente de todos los parametros de ejemplar </summary>
        public static List<Parameter> ObtenerParametrosEjemplar(Document doc, Element elem)
        {
            // Crea una lista vacia de parametros
            List<Parameter> lista = new List<Parameter>();

            // Recorre los parametros y agrega a la lista
            foreach (Parameter param in elem.Parameters)
            {
                lista.Add(param);
            }

            // Ordena la lista alfabéticamente
            lista = lista.OrderBy(x => x.Definition.Name).ToList();

            return lista;
        }

        ///<summary> Obtiene una lista ordenada alfabéticamente de todos los parametros de ejemplar de una categoría</summary>
        public static List<Parameter> ObtenerParametrosEjemplar(Document doc, BuiltInCategory categoria)
        {
            // Crea una lista vacia
            List<Parameter> lista = new List<Parameter>();
            List<Element> elementos = new List<Element>();

            // Llena la lista todos los elementos según la categoría
            elementos = ObtenerTodosEjemplaresSegunCategoria(doc, categoria);

            // Verifica que exista elementos para buscar sus parámetros
            if (elementos.Count > 0)
            {
                // Recorre los parametros y agrega a la lista
                foreach (Parameter param in elementos[0].Parameters)
                {
                    lista.Add(param);
                }

                // Ordena la lista alfabéticamente
                lista = lista.OrderBy(x => x.Definition.Name).ToList();
            }
            return lista;
        }

        ///<summary> Devuelve una lista de parametros que se pueden modificar y sean tipo texto </summary>
        public static List<Parameter> ObtenerParametrosEjemplarModificables(Document doc, List<Parameter> lista)
        {
            // Crea una lista vacia de parámetros
            List<Parameter> parametros = new List<Parameter>();

            // Filtra los parámetros de ejemplar que no cumplan con los requisitos
            parametros = (from param in lista
                          where param.IsReadOnly == false
                          && param.StorageType == StorageType.String
                          select param).ToList();

            return parametros;
        }

        ///<summary> Obtiene una lista ordenada alfabéticamente de los niveles del proyecto </summary>
        public static List<Level> ObtenerNiveles(Document doc)
        {
            // Crea una lista vacia de niveles
            List<Level> niveles = new List<Level>();

            // Crea un collector de Niveles
            FilteredElementCollector collector = new FilteredElementCollector(doc);

            //Filtra que los elementos sean de nivel
            List<Element> elementos = collector.OfClass(typeof(Level)).ToList();

            // Llena la lista de niveles
            foreach (var item in elementos)
            {
                // Convierte el elemento en un Nivel
                Level nivel = item as Level;

                // Agrega el nivel a la lista
                niveles.Add(nivel);
            }

            // Ordena la lista alfabéticamente
            niveles = niveles.OrderBy(x => x.Name).ToList(); ;

            return niveles;
        }

        ///<summary> Obtiene una lista de los elementos seleccionados en el modelo </summary>
        public static List<Element> ObtenerConjuntoSeleccionado(Document doc, UIDocument uiDoc)
        {
            // Crea una lista de elementos a devolver
            List<Element> elementos = new List<Element>();

            // Crea una lista de referencias de objetos
            IList<Reference> Objetos = uiDoc.Selection.PickObjects(Autodesk.Revit.UI.Selection.ObjectType.Element);

            // Obtiene el ID de los objetos seleccionados
            List<ElementId> lista = (from Reference r in Objetos select r.ElementId).ToList();

            // Recorre la lista de ElementID y agrega a la lista de elementos
            foreach (ElementId eid in lista)
            {
                Element e = doc.GetElement(eid);
                elementos.Add(e);
            }

            return elementos;
        }

        ///<summary> Obtiene el ID de una lista de elementos del modelo </summary>
        public static List<ElementId> ObtenerIdElemento(List<Element> lista)
        {
            // Crea la lista de los Id de los elementos
            List<ElementId> elemId = new List<ElementId>();

            // Recorre la lista y agrega a la lista de ID
            foreach (Element elem in lista)
            {
                elemId.Add(elem.Id);
            }

            return elemId;
        }

        ///<summary> Obtiene los elementos del modelo según su ID </summary>
        public static List<Element> ObtenerElementoSegunID(Document doc, List<ElementId> listaId)
        {
            // Crea la lista a devolver
            List<Element> lista = new List<Element>();

            // Recorra la lista
            foreach (ElementId elemId in listaId)
            {
                if (elemId != ElementId.InvalidElementId)
                {
                    // Agrega el elemento
                    lista.Add(doc.GetElement(elemId));
                }
            }

            return lista;
        }

        ///<summary> Obtiene una lista ordenada alfabéticamente con todos los estilos de linea </summary>
        public static List<Category> ObtenerEstilosDeLinea(Document doc)
        {

            // Crea la lista de los estilos de lineas del proyecto
            List<Category> estilos = new List<Category>();

            // Crea las variables
            Category categoria = doc.Settings.Categories.get_Item(BuiltInCategory.OST_Lines);
            CategoryNameMap subcategoria = categoria.SubCategories;

            // Obtiene todos los estilos de lineas del proyecto
            foreach (Category estilo in subcategoria)
            {
                estilos.Add(estilo);
            }

            // Ordena la lista alfabéticamente
            estilos = estilos.OrderBy(x => x.Name).ToList();

            return estilos;
        }

        ///<summary> Obtiene una lista ordenada alfabéticamente con todas las etiquetas de armadura </summary>
        public static List<FamilySymbol> ObtenerEtiquetasIndependientes(Document doc, BuiltInCategory categoria)
        {
            // Crea la lista a devolver
            List<FamilySymbol> etiquetas = new List<FamilySymbol>();

            // Busca las etiquetas
            IEnumerable<FamilySymbol> tags = new FilteredElementCollector(doc).OfClass(typeof(FamilySymbol)).Cast<FamilySymbol>();

            // Filtra según sean la categoría asignada
            etiquetas = (from elem in tags
                         where elem.Category.Id == new ElementId(categoria)
                         select elem).ToList();

            return etiquetas;
        }

        ///<summary> Obtiene una lista ordenada alfabéticamente con todos los estilos textos </summary>
        public static List<TextNoteType> ObtenerEstilosTexto(Document doc)
        {
            // Crea el colector
            FilteredElementCollector colector = new FilteredElementCollector(doc);

            // Filtra que los elementos sean de ejemplar
            List<TextNoteType> lista = colector.WhereElementIsElementType()
                                               .OfClass(typeof(TextNoteType))
                                               .Cast<TextNoteType>().ToList();

            // Ordena la lista alfabéticamente
            lista = lista.OrderBy(x => x.Name).ToList();

            return lista;
        }

        ///<summary> Obtiene una lista ordenada alfabéticamente con todos los estilos de cotas lineales </summary>
        public static List<DimensionType> ObtenerCotas(Document doc, DimensionStyleType estilo)
        {
            // Crea una lista
            List<DimensionType> lista = new List<DimensionType>();

            // Crea un filtro de la categoría dimensiones
            IEnumerable<DimensionType> colector = new FilteredElementCollector(doc).WhereElementIsElementType()
                                                                                   .OfClass(typeof(DimensionType))
                                                                                   .Cast<DimensionType>();

            // Filtra según sean la categoría asignada
            lista = (from elem in colector
                     where elem.StyleType == estilo
                     && elem.Parameters.Size > 5
                     select elem).ToList();

            // Ordena la lista alfabéticamente
            lista = lista.OrderBy(x => x.Name).ToList();
            
            return lista;
        }

        ///<summary> Obtiene una lista ordenada alfabéticamente con todos los estilos de cotas de elevación </summary>
        public static List<SpotDimensionType> ObtenerCotasElevacion(Document doc)
        {
            // Crea un filtro de la categoría dimensiones
            IEnumerable<SpotDimensionType> colector = new FilteredElementCollector(doc).WhereElementIsElementType()
                                                                                       .OfClass(typeof(SpotDimensionType))
                                                                                       .Cast<SpotDimensionType>();

            // Filtra que sea de la categoría adecuada
            List<SpotDimensionType> lista = (from elem in colector
                                             where elem.StyleType == DimensionStyleType.SpotElevation
                                             select elem).ToList();

            // Ordena la lista alfabéticamente
            lista = lista.OrderBy(x => x.Name).ToList();

            return lista;
        }

        ///<summary> Obtiene una lista de elementos según una selección en un ListBox </summary>
        public static List<Element> ObtenerElementosSeleccionadosEnProyecto(UIDocument uiDoc, Document doc, List<Element> todosLosElementos)
        {
            // Crea la lista a devolver
            List<Element> lista = new List<Element>();

            // Obtiene el Id de los elementos seleccionados
            List<ElementId> listaId = uiDoc.Selection.GetElementIds().ToList();

            // Verifica que los elementos seleccionados en el modelo estén en la lista elementos totales
            listaId = Tools.ObtenerIdElementosDeUnaLista(todosLosElementos, listaId);

            // Obtiene los elementos seleccionados segú el Id y agrega a la lista
            lista.AddRange(Tools.ObtenerElementoSegunID(doc, listaId));

            return lista;
        }

        ///<summary> Obtiene una lista de elementos que coincida con los elementos de otra lista </summary>
        public static List<Element> ObtenerElementosCoincidentesConLista(List<Element> listaBase, List<Element> listaSeleccionados)
        {
            // Crea la lista a devolver
            List<Element> lista = new List<Element>();

            // Compara el ID de los elementos y devuelve los coincidentes
            lista = listaBase.Where(x => listaSeleccionados.Any(y => y.Id == x.Id)).ToList();

            return lista;
        }

        ///<summary> Obtiene una lista de elementos ID que coincida con los elementos ID de otra lista </summary>
        public static List<ElementId> ObtenerElementosIDCoincidentesConLista(List<ElementId> listaBase, List<ElementId> listaSeleccionados)
        {
            // Crea la lista a devolver
            List<ElementId> lista = new List<ElementId>();

            // Compara el ID de los elementos y devuelve los coincidentes
            lista = listaBase.Where(x => listaSeleccionados.Any(y => y == x)).ToList();

            return lista;
        }

        ///<summary> Obtiene una lista de elementos según una selección en un ListBox </summary>
        public static List<Element> ObtenerElementosDeUnListbox(System.Windows.Forms.ListBox listbox, 
                                                                Document doc, 
                                                                List<Element> todosLosElementos)
        {
            // Crea la lista a devolver
            List<Element> elementos = new List<Element>();
            List<string> elementosSeleccionados = new List<string>();

            elementosSeleccionados = listbox.SelectedItems.Cast<string>().ToList();

            // Recorre la lista de elementos seleccionados
            for (int i = 0; i < elementosSeleccionados.Count; i++)
            {
                for (int j = 0; j < todosLosElementos.Count; j++)
                {
                    // Obtiene el FamilySymbol del elemento
                    FamilySymbol sym = doc.GetElement(todosLosElementos[j].GetTypeId()) as FamilySymbol;

                    // Crea el nombre a mostrar y luego el ID del elemento
                    string nombre = sym.Family.Name + ": " + todosLosElementos[j].Name + " <" + todosLosElementos[j].Id.ToString() + ">";

                    // Verifica que el nombre del listbox sea igual al del elemento de la lista
                    if (nombre == elementosSeleccionados[i])
                    {
                        // Agrega el elemento a la lista
                        elementos.Add(todosLosElementos[j]);
                    }
                }
            }

            return elementos;
        }

        ///<summary> Obtiene una lista de ID de los elementos seleccionados en el modelo </summary>
        public static List<ElementId> ObtenerIdElementosDeUnaLista(List<Element> todosLosElementos, List<ElementId> elementosId)
        {
            // Crea la lista a devolver
            List<ElementId> Id = new List<ElementId>();

            // Recorre la lista de todos los elementos
            foreach (Element elem in todosLosElementos)
            {
                // Verifica que los elementos de la lista Id estén en todos los elementos
                if (elementosId.Contains(elem.Id))
                {
                    // Agrega el elemento como un ElementId
                    Id.Add(elem.Id);
                }
            }

            return Id;
        }

        ///<summary> Obtiene una lista con los parámetros A, B, C, D... de una barra </summary>
        public static List<Parameter> ObtenerParametrosLongitudDeBarra(Document doc, Rebar barra)
        {
            // Crea la lista a devolver
            List<Parameter> listaParametros = new List<Parameter>();

            // Obtiene la forma de la armadura
            RebarShape barraForma = doc.GetElement(barra.GetShapeId()) as RebarShape;

            // Obtiene la definición de la forma
            RebarShapeDefinitionBySegments barraFormaDefinicion = barraForma.GetRebarShapeDefinition() as RebarShapeDefinitionBySegments;

            // Obtiene una lista con los parámetros (A, B, C, D...)
            List<ElementId> listaParametrosId = barraFormaDefinicion.GetParameters().ToList();
            
            // Recorre la lista de Id de los parámetros A, B, C, D...
            foreach (ElementId elemid in listaParametrosId)
            {
                // Recorre los parámetros de la barra
                foreach (Parameter param in barra.Parameters)
                {
                    // Verifica que el parámetro sea igual a A, B, C, D...
                    if (param.Id == elemid)
                    {
                        // Agrega a la lista
                        listaParametros.Add(param);
                    }
                }
            }

            return listaParametros;
        }

        #endregion
        
        #region Grupos y elementos anidados

        /// <summary> Crea un grupo a partir de una lista de elementos </summary>
        public static Group CrearGrupo(Document doc, List<Element> lista)
        {
            // Crea el grupo a devolver
            Group grupo = null;
            
            // Obtiene el ID de los elementos de la lista
            ICollection<ElementId> listaId = Tools.ObtenerIdElemento(lista);

            // Verifica que la lista de elementos tenga contenido
            if (listaId.Count > 0)
            {
                doc.Regenerate();

                // Asigna los elementos al grupo
                grupo = doc.Create.NewGroup(listaId);
            }

            return grupo;
        }

        /// <summary> Verifica que un grupo tenga elementos de la clase DetailArc o DetailLine y TextNote </summary>
        public static void VerificarYEliminarGrupo(Document doc, View vista, Group grupo)
        {
            // Crea una bandera
            bool bandera1 = false;
            bool bandera2 = false;
            bool bandera3 = false;

            // Obtiene todos los miembros del grupo
            IList<ElementId> listaId = grupo.GetMemberIds();

            // Verifica la cantidad de elementos del grupo
            if (listaId.Count <= 1)
            {
                // Desagrupa
                grupo.UngroupMembers();

                // Elimina los elementos del grupo
                doc.Delete(listaId);

                // Cambia el estado de la bandera
                bandera3 = true;
            }
                        
            // La lista tiene más de un elemento
            else
            {
                //Recorre todos los elementos en la lista
                foreach (ElementId elemId in listaId)
                {
                    // Obtiene el elemento
                    Element elem = doc.GetElement(elemId);

                    try
                    {
                        // Castea el elemento a línea
                        DetailLine linea = elem as DetailLine;

                        bandera1 = true;
                    }

                    catch (Exception) { }

                    try
                    {
                        // Castea el elemento a TextNote
                        TextNote texto = elem as TextNote;

                        bandera2 = true;
                    }

                    catch (Exception) { }
                }
            }

            // Verifica que no haya pasado por el 1er if
            if (!bandera3)
            {
                // Verifica las banderas
                if (!bandera1 && !bandera2)
                {
                    // Desagrupa
                    grupo.UngroupMembers();

                    // Elimina los elementos del grupo
                    doc.Delete(listaId);
                }
            }
        }
        
        ///<summary> Devuelve verdadero si el elemento posee subcomponentes, es decir familias anidadas (ejemplo los cabezales con pilotes) </summary>
        public static bool VerificarSubElementosAnidados(Element elem)
        {
            // Crea el booleano
            bool respuesta = false;

            // Obtiene una lista con los Id de todos los subcomponentes
            List<ElementId> listaId = (elem as FamilyInstance).GetSubComponentIds().ToList();

            // Verifica que la lista tenga por lo menos un subcomponente
            if (listaId.Count > 0)
            {
                // Cambia el estado de la bandera
                respuesta = true;
            }

            return respuesta;
        }

        #endregion

        #region Unidades

        ///<summary> Obtiene las unidades del proyecto </summary>
        public static Units UnidadesObtenerUnits(Document doc)
        {
            // Obtiene las unidades del proyecto
            Units uni = doc.GetUnits();

            return uni;
        }

        ///<summary> Obtiene el FormatOptions de un parámetro </summary>
        public static FormatOptions UnidadesObtenerFormatOptions(Document doc, UnitType UT)
        {
            // Obtiene las unidades del proyecto
            Units unidades = doc.GetUnits();

            // Obtiene las opciones de formato
            FormatOptions FO = unidades.GetFormatOptions(UT);
            
            return FO;
        }

        ///<summary> Obtiene la unidad específica del proyecto </summary>
        public static DisplayUnitType ObtenerUnidadDelProyecto(Document doc, UnitType tipoUnidad)
        {
            // Obtiene las unidades del proyecto
            Units Unidades = doc.GetUnits();
            
            // Obtiene las opciones de números con unidades
            FormatOptions formato = Unidades.GetFormatOptions(tipoUnidad);

            // Obtiene el simbolo de la unidad
            return formato.DisplayUnits;
        }

        ///<summary> Obtiene el separador decimal punto "." o coma "," </summary>
        public static DecimalSymbol UnidadesObtenerSeparadorDecimal(Document doc)
        {
            // Crea el string
            DecimalSymbol separador;

            // Obtiene las unidades del proyecto
            Units uni = doc.GetUnits();

            separador = uni.DecimalSymbol;

            return separador;
        }

        ///<summary> Obtiene el DisplayUnitType de un parámetro </summary>
        public static DisplayUnitType UnidadesObtenerDisplayUnitTypeParametro(Parameter param)
        {
            // Obtiene el DisplayUnitType
            return param.DisplayUnitType;
        }

        ///<summary> Obtiene el UnitSymbolType de un parámetro </summary>
        public static UnitSymbolType UnidadesObtenerUnitSymbolTypeParametro(Document doc, Parameter param)
        {
            // Obtiene las unidades del proyecto
            Units unidades = doc.GetUnits();

            // Obtiene las opciones de formato
            FormatOptions formato = unidades.GetFormatOptions(param.Definition.UnitType);

            // Obtiene el UnitSymbolType
            return formato.UnitSymbol;
        }

        ///<summary> Obtiene el UnitType de un parámetro </summary>
        public static UnitType UnidadesObtenerUnitTypeParametro(Parameter param)
        {
            // Obtiene el UnitType
            return param.Definition.UnitType;
        }

        #endregion

        #region Dibujo de Armaduras

        ///<summary> Obtiene una lista de todas las barras en un elemento </summary>
        public static List<Rebar> ObtenerArmadurasDeElemento(Element elem, View vista)
        {
            // Crea la lista a devolver
            List<Rebar> listaArmadura = new List<Rebar>();

            // Obtiene todas las barras del elemento
            IList<Rebar> lista = RebarHostData.GetRebarHostData(elem).GetRebarsInHost();

            listaArmadura.AddRange(lista);

            return listaArmadura;
        }

        ///<summary> Obtiene una lista de barras que su plano sea perpendicular al de una vista </summary>
        public static List<Rebar> ObtenerArmaduraPerpendicularVista(View vista, List<Rebar> lista)
        {
            // Crea la lista a devolver
            List<Rebar> listaBarras = new List<Rebar>();

            // Recorre cada armadura de la lista
            foreach (Rebar barra in lista)
            {
                // Verifica el plano de la barra
                if (VerificarPlanoBarraPerpendicularVista(vista, barra))
                {
                    // Agrega la barra a la lista
                    listaBarras.Add(barra);
                }
            }

            return listaBarras;
        }

        ///<summary> Devuelve verdadero si el plano de una armadura es perpendicular a una vista </summary>
        public static bool VerificarPlanoBarraPerpendicularVista(View vista, Rebar barra)
        {
            // Crea el booleano
            bool respuesta = false;

            // Verifica que la forma de la barra sea basada en forma
            if (barra.IsRebarShapeDriven())
            {
                // Obtiene el RebarShapeDrivenAccessor
                RebarShapeDrivenAccessor RSDA = barra.GetShapeDrivenAccessor();

                // Obtiene el vector normal de la barra
                XYZ normalBarra = RSDA.Normal;

                // Obtiene el vector en dirección de la vista
                XYZ direccionVista = vista.ViewDirection;

                // Realiza el producto vectorial
                XYZ vector = normalBarra.CrossProduct(direccionVista);

                // Verifica que el vector sea cero, es decir, que son paralelos
                if (vector.IsZeroLength())
                {
                    respuesta = true;
                }
            }

            return respuesta;
        }

        ///<summary> Obtiene una lista de las curvas centrales de una armadura </summary>
        public static List<Curve> ObtenerCurvasCentralesArmadura(View vista, Rebar barra)
        {
            // Crea la lista de las lineas a devolver
            List<Curve> lista = new List<Curve>();

            // Obtiene una lista con curvas de la linea central de la barra
            lista = barra.GetCenterlineCurves(superposicion, ganchos, radioDeGiro, multiplePlano, posicionBarra).ToList();

            return lista;
        }

        ///<summary> Obtiene las curvas del borde de una barra de armadura </summary>        Verificar BUG
        public static List<Curve> ObtenerCurvaDeBordeDeBarra(View vista, Rebar barra)
        {
            Document doc = barra.Document;

            // Crea la lista a devolver
            List<Curve> curvas = new List<Curve>();
            
            // Crea las preferencias para analizar la geometría
            Options opcion = new Options();

            // Activa el cálculo de referencias a objetos
            opcion.View = vista;

            // Crea una representación geométrica del elemento
            GeometryElement geoElem = barra.get_Geometry(opcion);

            // Declara un contador
            int i = 0;

            // Recorre todos los solidos
            foreach (Solid solido in geoElem)
            {
                // Verifica que sea menor a la posición elegida
                if (i > Jump.Properties.Settings.Default.posicionBarraADibujar)
                {
                    // Cierra el bucle de solido
                    break;
                }

                // Aumenta el contador
                i++;

                // Recorre todos los borde
                foreach (Edge borde in solido.Edges)
                {
                    // Obtiene la linea del borde
                    Curve curva = borde.AsCurve();

                    // Agrega a la lista
                    curvas.Add(curva);
                }
            }

            return curvas;
        }

        ///<summary> Verifica la continuidad de las curvas </summary>
        public static List<Curve> VerificarContinuidadCurvas (List<Curve> curvas)
        {
            // Lista con curvas ordenadas
            List<Curve> lista = new List<Curve>();

            return lista;
        }

        ///<summary> Dibuja una línea de una barra y cambia el estilo en función del DataGridView </summary>
        public static List<CurveElement> DibujarArmaduraSegunDatagridview(System.Windows.Forms.DataGridView dgv, Document doc, View vista, Rebar barra)
        {
            // Obtiene el diámetro de la barra
            RebarBarType barraDiametro = doc.GetElement(barra.GetTypeId()) as RebarBarType;

            // Crea la lista de las lineas a devolver
            List<CurveElement> listaLineas = new List<CurveElement>();

            // Crea la lista de los estilos de lineas del proyecto
            List<Category> estilos = Tools.ObtenerEstilosDeLinea(doc);

            // Categoría del estilo de línea
            Category cat = null;

            // Crea una lista con las curvas de las armaduras
            List<Curve> listaCurva = new List<Curve>();

            // Verifica que sean las líneas centrales de las armaduras
            if (Jump.Properties.Settings.Default.rbtnArmaduraDibujoLineasCentrales)
            {
                // Obtiene las línea central de la barra
                listaCurva = ObtenerCurvasCentralesArmadura(vista, barra);
            }

            // Verifica que sean las líneas de borde de las armaduras
            if (Jump.Properties.Settings.Default.rbtnArmaduraDibujoLineasBorde)
            {
                // Obtiene las línea de borde de la barra
                listaCurva = ObtenerCurvaDeBordeDeBarra(vista, barra);
            }

            // Crea la curva final
            Curve curvaFinal = null;

            // Recorre todas las curvas de la lista
            foreach (Curve curva in listaCurva)
            {
                try
                {
                    // Recorre el DataGridView
                    for (int i = 0; i < dgv.Rows.Count; i++)
                    {
                        // Compara el nombre de la celda con el diámetro de la barra
                        if (dgv.Rows[i].Cells[AboutJump.nombreColumnaDiametros].Value.ToString() == barraDiametro.Name)
                        {
                            // Proyecta la curva sobre el plano de la vista
                            curvaFinal = ProyectarCurvaSobrePlano(vista, curva);

                            // Crea una serie de curvas con lineas de detalle
                            CurveElement curvaDetalle = doc.Create.NewDetailCurve(vista, curvaFinal) as CurveElement;
                            
                            // Obtiene la categoría del estilo de línea
                            cat = estilos.FirstOrDefault(x => x.Name == dgv.Rows[i].Cells[AboutJump.nombreColumnaEstilosLineas].Value.ToString());

                            // Crea el estilo de gráfico en función del estilo de línea
                            GraphicsStyle gra = cat.GetGraphicsStyle(GraphicsStyleType.Projection);

                            // Asigna el estilo a la línea
                            curvaDetalle.LineStyle = gra;

                            // Agrega la linea a la lista de lineas
                            listaLineas.Add(curvaDetalle);
                        }
                    }
                }

                catch (Exception) { }
            }

            return listaLineas;
        }

        ///<summary> Proyecta una curva sobre el plano de una vista y devuelve la curva proyectado </summary>
        public static Curve ProyectarCurvaSobrePlano(View vista, Curve curva)
        {
            // Crea la lista con los puntos de las curvas
            List<XYZ> puntos = new List<XYZ>();

            // Crea el punto proyectado
            Curve curvaProyectada = curva;

            // Recorre todos los puntos de la curva                     
            foreach (XYZ punto in curva.Tessellate().ToList())
            {
                // Proyecta los puntos sobre el plano
                puntos.Add(ProyectarPuntoSobrePlano(vista, punto));
            }

            // Verifica que sea una linea
            if (curva is Line)
            {
                // Crea la linea
                curvaProyectada = Line.CreateBound(puntos[0], puntos[1]);
            }

            //Verifica que sea un arco
            if (curva is Arc)
            {
                // Verifica que tenga 3 o más puntos
                if (puntos.Count > 2)
                {
                    // Crea el arco
                    curvaProyectada = Arc.Create(puntos[0], puntos[puntos.Count - 1], puntos[1]);
                }
            }

            // Verifica si es un HermiteSpline
            if (curva is HermiteSpline)
            {
                // Castea la curva
                HermiteSpline spline = curva as HermiteSpline;

                // Obtiene si es peridica
                bool periodo = spline.IsPeriodic;

                // Crea el Spline
                HermiteSpline Hermite = HermiteSpline.Create(puntos, periodo);

                // Crea el Nurb
                curvaProyectada = NurbSpline.CreateCurve(Hermite);
            }

            return curvaProyectada;
        }

        ///<summary> Proyecta un punto sobre el plano de una vista y devuelve el punto proyectado </summary>
        public static XYZ ProyectarPuntoSobrePlano(View vista, XYZ punto)
        {
            // Crea el punto proyectado
            XYZ puntoProyectado = punto;

            // Obtiene la transformación de la vista
            Transform traVista = vista.CropBox.Transform;

            // Crea el plano de la vista
            Plane plano = Plane.CreateByOriginAndBasis(traVista.Origin, traVista.BasisX, traVista.BasisY);

            // Obtiene un vector
            XYZ vector = punto - plano.Origin;

            // Obtiene la distancia
            double distancia = plano.Normal.DotProduct(vector);

            // Verifica que el punto esté fuera del plano
            if (distancia != toleranciaComponenteVector)
            {
                // Proyecta el punto sobre el plano
                puntoProyectado = punto - distancia * plano.Normal;
            }

            return puntoProyectado;
        }

        ///<summary> Coloca una nota de texto con la longitud parcial de una armadura en una vista dada, true = arriba o false = abajo </summary>
        public static List<TextNote> CrearTextNoteDeArmadura(Document doc, View vista, Rebar barra, TextNoteType tipoTexto)
        {
            // Crea la lista a devolver
            List<TextNote> listaTexto = new List<TextNote>();

            // Crea el texto que mostrará el TextNote
            string nombre = null;

            // Crea la posición donde se colocará
            XYZ ubicacion = new XYZ();

            // Obtiene las curvas centrales de la armadura
            List<Curve> listaCurvas = ObtenerCurvasCentralesArmadura(vista, barra);

            // Recorre todas las curvas
            foreach (Curve curva in listaCurvas)
            {
                // Obtiene el UnitType de las barras de refuerzo
                UnitType UT = tipoUnidadTexto;

                // Obtiene las unidades
                Units unidades = UnidadesObtenerUnits(doc);

                // Verifica que la curvaDetalle sea de la clase Line
                if (curva.GetType() == typeof(Line))
                {
                    try
                    {
                        // Crea las opciones para el formato de valores
                        FormatValueOptions opcionesValorFormato = new FormatValueOptions();

                        // Asigna las opciones del proyecto
                        opcionesValorFormato.SetFormatOptions(unidades.GetFormatOptions(UT));
                        
                        // Obtiene la longitud de la curva
                        double longitud = curva.ApproximateLength;

                        // Obtiene la longitud de la curva
                        nombre = UnitFormatUtils.Format(unidades, UT, longitud, false, false, opcionesValorFormato);
                        
                        // Obtiene el punto inicial de la curva
                        XYZ puntoInicial = curva.GetEndPoint(0);

                        // Obtiene el punto final de la curva
                        XYZ puntoFinal = curva.GetEndPoint(1);

                        // Obtiene el punto medio de la curva
                        XYZ puntoMedio = (puntoInicial + puntoFinal) / 2;

                        // Crea el texto
                        TextNote texto = TextNote.Create(doc, vista.Id, puntoMedio, nombre, tipoTexto.Id);
                        
                        // Mueve y rota el TextNote a su posición final
                        MoverYRotarTextNote(doc, vista, barra, curva, texto);

                        // Agrega el texto a la lista
                        listaTexto.Add(texto);
                    }

                    catch (Exception) { }
                }
            }

            return listaTexto;
        }

        ///<summary> Rota el TextNote según la dirección de la curva </summary>
        public static void MoverYRotarTextNote(Document doc, View vista, Rebar barra, Curve curva, TextNote texto)
        {
            // Regenera el documento
            doc.Regenerate();

            // Obtiene la transformación en un punto de la curva
            Transform tra = curva.ComputeDerivatives(puntoDeCurvaParaTexto, true);
            
            // Obtiene el vector dirección
            XYZ vectorDireccion = tra.BasisX;

            // Obtiene el vector perpendicular
            XYZ vectorPerpendicular = vectorDireccion.CrossProduct(vista.CropBox.Transform.BasisZ).Normalize();
            
            // Obtiene la caja que contiene al texto
            BoundingBoxXYZ bb = texto.get_BoundingBox(vista);

            // Obtiene el punto medio del texto
            XYZ puntoMedio = (bb.Max + bb.Min) / 2;
            
            // Vector desde el origen de la transformación hasta el punto medio del texto
            XYZ distancia = puntoMedio - tra.Origin;

            // Mueve el texto
            ElementTransformUtils.MoveElement(doc, texto.Id, -distancia);

            // Obtiene la caja que contiene al texto actualizado
            bb = texto.get_BoundingBox(vista);

            // Asigna el nuevo punto medio del texto
            puntoMedio = (bb.Max + bb.Min) / 2;

            // Crea el eje de rotación
            Line eje = Line.CreateBound(puntoMedio, puntoMedio.Add(vista.CropBox.Transform.BasisZ));
            
            // Obtiene el ángulo de rotación
            double angulo = vista.CropBox.Transform.BasisX.AngleTo(vectorDireccion);

            // Lo lleva a coordenadas relativas de la vista
            XYZ vectorDireccionLocal = vista.CropBox.Transform.Inverse.OfVector(vectorDireccion);

            // Asigna el nuevo cuadro del texto
            bb = texto.get_BoundingBox(vista);

            // Verifica que el componente Y es menor que cero
            if (vectorDireccionLocal.Y < 0)
            {
                // Asigna 
                angulo = -angulo;
            }
            
            // Rota el TextNote
            ElementTransformUtils.RotateElement(doc, texto.Id, eje, angulo);

            // Obtiene la transformada de la vista
            Transform vectorTra = vista.CropBox.Transform;

            // Obtiene la altura del texto en la vista según su escala
            double alturaTexto = texto.Height * vista.Scale;

            // Obtiene el ancho del texto en la vista según su escala
            double anchoTexto = texto.Width * vista.Scale;

            // Obtiene el vector diagonal del texto en coordenadas de la vista
            XYZ diagonal = vectorTra.Inverse.OfVector(bb.Max - bb.Min);

            // Obtiene el vector con las distancias del texto en coordenadas de la vista
            XYZ textoDistancia = new XYZ(anchoTexto, alturaTexto, 0);

            // Obtiene el margen del TextNote en coordenadas de la vista
            XYZ margen = (diagonal - textoDistancia) / 2; 

            // Obtiene el diámetro de la barra
            double diametro = barra.GetBendData().BarDiameter;

            // Crea la distancia a mover
            distancia = new XYZ(diametro, diametro, diametro) + margen;

            // Proyecta la distancia sobre el vector perpendicular
            XYZ distanciaMover = vectorPerpendicular.Multiply(distancia.Y);

            // Obtiene el cuadro de la barra
            BoundingBoxXYZ bbarra = barra.get_BoundingBox(null);

            // Obtiene el baricentro de la barra
            XYZ baricentro = (bbarra.Max + bbarra.Min) / 2;

            // Obtiene el punto medio de la curva
            XYZ puntoMedioCurva = tra.Origin;

            // Obtiene el vector desde el baricentro hasta el punto de la curva
            XYZ distanciaCurvaBaricentro = puntoMedioCurva - baricentro;

            // Obtiene el vector perpendicular 
            XYZ vectorEntranteSaliente = vectorDireccion.CrossProduct(distanciaCurvaBaricentro);

            // Lleva el vector perpendicular a coordenadas de la vista
            vectorEntranteSaliente = vectorTra.Inverse.OfVector(vectorEntranteSaliente);

            // Verifica si es cero
            if (vectorEntranteSaliente.Z == 0 || Math.Abs(vectorEntranteSaliente.Z) <= toleranciaComponenteVector)
            {
                // Obtiene la distancia a mover según la configuración
                distanciaMover = TextoArribaOAbajo(vectorTra, distanciaMover);
            }

            // No es mayor que cero
            else
            {
                // Obtiene la distancia a mover según la configuración
                distanciaMover = TextoArribaOAbajo(vectorTra, distanciaMover);
            }

            // Mueve el texto
            ElementTransformUtils.MoveElement(doc, texto.Id, distanciaMover);
        }

        ///<summary> Devuelve la distancia correcta si el TextNote va arriba o abajo según las configuraciones </summary>
        public static XYZ TextoArribaOAbajo(Transform vectorTra, XYZ distancia)
        {
            // Lleva la distancia a coordenadas de la vista 
            XYZ distanciaMover = vectorTra.Inverse.OfVector(distancia);

            // Verifica que en las configuraciones sea para arriba
            if (Properties.Settings.Default.rbtnTextoArmaduraArriba)
            {
                // Verifica que la componente sea menor a cero
                if (distanciaMover.Y < 0)
                {
                    // Asigna el valor negativo
                    distanciaMover = -distanciaMover;
                }

                // Verifica si la distancia es horizontal
                else if (Math.Abs(distanciaMover.Y) <= toleranciaComponenteVector && distanciaMover.X > 0)
                {
                    // Asigna el valor negativo
                    distanciaMover = -distanciaMover;
                }
            }

            // Configuraciones del texto abajo
            else
            {
                // Verifica que la componente sea menor a cero
                if (distanciaMover.Y > 0)
                {
                    // Asigna el valor negativo
                    distanciaMover = -distanciaMover;
                }

                // Verifica si la distancia es horizontal
                else if (Math.Abs(distanciaMover.Y) <= toleranciaComponenteVector && distanciaMover.X < 0)
                {
                    // Asigna el valor negativo
                    distanciaMover = -distanciaMover;
                }
            }

            return vectorTra.OfVector(distanciaMover);
        }

        #endregion

        #region Mover las representaciones de Armaduras
        
        ///<summary> Ordena y mueve las Represetaciones de Armaduras según las opciones </summary>
        public static void OrdenarYMoverRepresentacionArmaduraSegunDireccion(Document doc, View vista, Element elem, List<ArmaduraRepresentacion> armaduras)
        {
            // Crea las listas
            List<ArmaduraRepresentacion> listaArmadurasArriba = new List<ArmaduraRepresentacion>();
            List<ArmaduraRepresentacion> listaArmadurasAbajo = new List<ArmaduraRepresentacion>();
            List<ArmaduraRepresentacion> listaArmadurasIzquierda = new List<ArmaduraRepresentacion>();
            List<ArmaduraRepresentacion> listaArmadurasDerecha = new List<ArmaduraRepresentacion>();

            // Crea una transformada de la vista
            Transform tra = vista.CropBox.Transform;

            // Obtiene el recuadro del elemento
            BoundingBoxXYZ bbElem = ObtenerRecuadroElementoParaleloAVista(doc, vista, elem);

            // Obtiene el baricentro del recuadro del elemento
            XYZ puntoMedioElem = ObtenerBaricentroElemento(bbElem);

            // Recorre la lista de Representación de Armaduras
            foreach (ArmaduraRepresentacion bar in armaduras)
            {
                try
                {
                    // Obtiene el recuadro de la barra
                    BoundingBoxXYZ bbArmadura = ObtenerRecuadroElementoParaleloAVista(doc, vista, bar.Barra);

                    // Obtiene el baricentro del recuadro de la barra
                    XYZ puntoMedioArmadura = ObtenerBaricentroElemento(bbArmadura);

                    // Obtiene la dirección principal de la barra
                    XYZ direccionPrincipal = ObtenerDireccionPrincipalArmadura(doc, vista, bar);

                    // Obtiene la distancia desde la armadura al elemento
                    XYZ distancia = puntoMedioArmadura - puntoMedioElem;

                    // Obtiene la distancia en coordenadas de la vista
                    XYZ distanciaRelativa = tra.Inverse.OfVector(distancia);

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

        ///<summary> Obtiene la dirección principal de la armadura </summary>
        public static XYZ ObtenerDireccionPrincipalArmadura(Document doc, View vista, ArmaduraRepresentacion bar)
        {
            // Crea la dirección
            XYZ direccion = new XYZ();

            // Obtiene la forma de la armadura
            RebarShape formaArmadura = doc.GetElement(bar.Barra.GetShapeId()) as RebarShape;

            // Obtiene una lista con las curvas de la armadura
            List<Curve> listaDirecciones = bar.Barra.GetShapeDrivenAccessor().ComputeDrivingCurves().ToList();//formaArmadura.GetCurvesForBrowser().ToList();

            // Obtiene la definición de la armadura
            RebarShapeDefinition definicionDeFormaArmadura = formaArmadura.GetRebarShapeDefinition();

            // Verifica si la armadura está basa en segmento
            if (definicionDeFormaArmadura is RebarShapeDefinitionBySegments)
            {
                // Obtiene la posición de la dirección principal
                int posicion = (definicionDeFormaArmadura as RebarShapeDefinitionBySegments).MajorSegmentIndex;

                // Obtiene la curva principal
                Curve curva = listaDirecciones[posicion];

                // Obtiene la dirección según el tipo
                direccion = ObtenerVectorNormalDeLaDireccionPrincipal(vista, curva);
            }

            // Está basado en arco
            else
            {
                // Obtiene la lista de arcos
                List<Curve> listaCurvas = formaArmadura.GetCurvesForBrowser().ToList();

                // Verifica que sea mayor a cero
                if (listaCurvas.Count > 0)
                {
                    // Obtiene el primer arco
                    Arc arco = listaCurvas[0] as Arc;

                    // Obtiene la normal
                    direccion = arco.Normal;
                }
            }

            return direccion;
        }
        
        ///<summary> Obtiene el vector normal de la curva </summary>
        public static XYZ ObtenerVectorNormalDeLaDireccionPrincipal(View vista, Curve curva)
        {
            // Crea la dirección
            XYZ direccion = new XYZ();

            // Verifica si es un arco
            if (curva is Arc)
            {
                // Castea
                Arc arco = curva as Arc;

                // Asigna la normal
                direccion = arco.Normal;
            }

            // Verifica si es una elipse
            else if (curva is Ellipse)
            {
                // Castea
                Ellipse elipse = curva as Ellipse;

                // Asigna la normal
                direccion = elipse.Normal;
            }

            // Verifica si es una línea
            else if (curva is Line)
            {
                // Castea
                Line linea = curva as Line;

                // Asigna la normal
                direccion = vista.ViewDirection.CrossProduct(linea.Direction);
            }

            return direccion;
        }

        ///<summary> Organiza una Representación de Armadura según una dirección </summary>
        public static void OrganizarListaSegunDireccionDeBarra(View vista, XYZ distanciaRelativa, ArmaduraRepresentacion bar,
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
                bar.Posicion = traInv.Inverse.OfVector(ProyectarVectorSobreDireccion(distanciaRelativa, traInv.OfVector(vista.RightDirection)));

                // Agrega la armadura a la lista
                listaArmadurasDerecha.Add(bar);
            }

            // Verifica si X es mayor a Y
            else if (Math.Abs(distanciaRelativa.X) >= Math.Abs(distanciaRelativa.Y))
            {
                // Verifica si X es positivo
                if (ObtenerSignoComponenteDeVector(distanciaRelativa.X) == 1)
                {
                    // Proyecta y asigna la posición de la armadura
                    bar.Posicion = traInv.Inverse.OfVector(ProyectarVectorSobreDireccion(distanciaRelativa, traInv.OfVector(vista.RightDirection)));
                    
                    // Agrega la armadura a la lista
                    listaArmadurasDerecha.Add(bar);
                }

                else
                {
                    // Proyecta y asigna la posición de la armadura
                    bar.Posicion = traInv.Inverse.OfVector(ProyectarVectorSobreDireccion(distanciaRelativa, traInv.OfVector(vista.RightDirection.Negate())));
                    
                    // Agrega la armadura a la lista
                    listaArmadurasIzquierda.Add(bar);
                }
            }

            // Y es mayor a X
            else
            {
                // Verifica si Y es positivo
                if (ObtenerSignoComponenteDeVector(distanciaRelativa.Y) == 1)
                {
                    // Proyecta y asigna la posición de la armadura
                    bar.Posicion = traInv.Inverse.OfVector(ProyectarVectorSobreDireccion(distanciaRelativa, traInv.OfVector(vista.UpDirection)));
                    
                    // Agrega la armadura a la lista
                    listaArmadurasArriba.Add(bar);
                }

                else
                {
                    // Proyecta y asigna la posición de la armadura
                    bar.Posicion = traInv.Inverse.OfVector(ProyectarVectorSobreDireccion(distanciaRelativa, traInv.OfVector(vista.UpDirection.Negate())));

                    // Agrega la armadura a la lista
                    listaArmadurasAbajo.Add(bar);
                }
            }
        }

        ///<summary> Ordena y mueve las listas de Representación de Armadura </summary>
        public static void OrdenarYMoverListaConArmadurasRepresentacion(Document doc, View vista, Transform tra, Element elem,
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
        public static void MoverListaConArmaduras(Document doc, View vista, Element elem, XYZ direccion, List<ArmaduraRepresentacion> armaduras)
        {
            // Crea las banderas de las direcciones
            bool banderaArriba = true;
            bool banderaAbajo = true;
            bool banderaDerecha = true;
            bool banderaIzquierda = true;

            // Crea una transformada de la vista
            Transform tra = vista.CropBox.Transform;

            // Recuadro del elemento
            BoundingBoxXYZ bbElem = ObtenerRecuadroElementoParaleloAVista(doc, vista, elem);

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

                    // Recuadro de la barra
                    BoundingBoxXYZ bbBar = bar.ObtenerBoundingBoxDeArmadura();//ObtenerRecuadroElementoParaleloAVista(doc, vista, grupo);

                    // Asigna la transformada de la vista al recuadro
                    bbBar.Transform = tra;

                    // Crea los componentes absolutos
                    double x = Math.Abs(bar.Posicion.X);
                    double y = Math.Abs(bar.Posicion.Y);
                    double z = Math.Abs(bar.Posicion.Z);

                    // Lo lleva a coordenadas de la vista
                    bar.Posicion = tra.Inverse.OfVector(new XYZ(x, y, z));

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
                            distancia = tra.Inverse.OfVector(ProyectarVectorSobreDireccion((bbElem.Max - bbBar.Min), vista.UpDirection)) + barAlto / 2;

                            // Cambia el estado de la bandera
                            banderaArriba = false;
                        }
                        else
                        {
                            distancia += barAlto / 2;
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
                            distancia = tra.Inverse.OfVector(ProyectarVectorSobreDireccion((bbElem.Min - bbBar.Max), vista.UpDirection.Negate())) - barAlto / 2;

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
                            distancia = elementoAncho - bar.Posicion + barAncho;
                            //distancia = tra.Inverse.OfVector(ProyectarVectorSobreDireccion((bbElem.Max - bbBar.Min), vista.RightDirection)) + barAncho;

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
                            distancia = elementoAncho.Negate() + bar.Posicion - barAncho;
                            //distancia = tra.Inverse.OfVector(ProyectarVectorSobreDireccion((bbElem.Min - bbBar.Max), vista.RightDirection.Negate())) - barAncho;

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
                            distancia = elementoAncho - bar.Posicion + barAncho;

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

        #endregion

        #region Actualizador de Armaduras

        /// <summary> Crea el actualizar de armaduras y agrega al registro </summary>
        public static void CrearRegistroActualizadorArmaduras(AddInId AddIn)
        {
            // Obtiene el actualizador para eliminar barras
            ArmaduraEliminacion barraEliminada = new ArmaduraEliminacion(AddIn);

            // Obtiene el actualizador de barras
            ArmaduraActualizacion barraActualizada = new ArmaduraActualizacion(AddIn);

            // Verifica que el actualizador para eliminar barras existe en el registro
            if (UpdaterRegistry.IsUpdaterRegistered(barraEliminada.GetUpdaterId()))
            {
                // Elimina el actualizador
                UpdaterRegistry.UnregisterUpdater(barraEliminada.GetUpdaterId());
            }

            // Verifica que el actualizar existe en el registro
            if (UpdaterRegistry.IsUpdaterRegistered(barraActualizada.GetUpdaterId()))
            {
                // Elimina el actualizador
                UpdaterRegistry.UnregisterUpdater(barraActualizada.GetUpdaterId());
            }

            // Agrega el actualizador para eliminar barras al registro
            UpdaterRegistry.RegisterUpdater(barraEliminada);

            // Agrega el actualizador de barras al registro
            UpdaterRegistry.RegisterUpdater(barraActualizada);

            // Obtiene un filtro de categoría para barras
            ElementCategoryFilter filtro = new ElementCategoryFilter(BuiltInCategory.OST_Rebar);

            // Agrega al disparador
            UpdaterRegistry.AddTrigger(barraEliminada.GetUpdaterId(), filtro, Element.GetChangeTypeElementDeletion());
            UpdaterRegistry.AddTrigger(barraActualizada.GetUpdaterId(), filtro, Element.GetChangeTypeGeometry());
        }

        /// <summary> Elimina el actualizar de armaduras del registro </summary>
        public static void EliminarRegistroActualizadorArmaduras(AddInId AddIn)
        {
            // Obtiene el actualizador para eliminar barras
            ArmaduraEliminacion barraEliminada = new ArmaduraEliminacion(AddIn);

            // Obtiene el actualizador de barras
            ArmaduraActualizacion barraActualizada = new ArmaduraActualizacion(AddIn);

            // Quita del actualizador de barras al registro
            UpdaterRegistry.UnregisterUpdater(barraEliminada.GetUpdaterId());
            UpdaterRegistry.UnregisterUpdater(barraActualizada.GetUpdaterId());

            // Recorre todas los despieces
            foreach (ArmaduraRepresentacion bar in Inicio.listaArmaduraRepresentacion)
            {
                // Obtiene la cantidad de curvas
                int i = bar.CurvasDeArmadura.Count;

                // Obtiene la cantidad de textos
                int j = bar.TextosDeLongitudesParciales.Count;

                // Verifica que existan líneas
                if (i > 0)
                {
                    // Obtiene el total de curvas del historial
                    int total = bar.ListaCurvasId.Count;

                    // Elimina el historial de líneas creadas
                    bar.ListaCurvasId.RemoveRange(0, total - i);
                }

                // Verifica que existan textos
                if (j > 0)
                {
                    // Obtiene el total de textos del historial
                    int total = bar.ListaTextosId.Count;

                    // Elimina el historial de textos creadas
                    bar.ListaTextosId.RemoveRange(0, total - j);
                }
            }
        }

        /// <summary> Guarda la Representación de armaduras en la barra </summary>
        public static void GuardarRepresentacionArmaduraDeBarra(Rebar barra, ArmaduraRepresentacion armadura)
        {
            RepresentacionesEntity representaciones = ObtenerRepresentacionEntityDeBarra(barra);

            List<ArmaduraRepresentacionEntity> listaEntidades = representaciones.ListaRepresentaciones;

            ArmaduraRepresentacionEntity armaduraEntity = new ArmaduraRepresentacionEntity();

            armaduraEntity.Vista = armadura.Vista.Id;

            armaduraEntity.ListaCurvas = armadura.ListaCurvasId;

            armaduraEntity.ListaTextos = armadura.ListaTextosId;

            armaduraEntity.TipoDeTexto = armadura.TipoDeTexto.Id;

            if (armadura.EtiquetaArmadura != null)
            {
                armaduraEntity.Etiqueta = armadura.EtiquetaArmadura.Id;
            }

            armaduraEntity.Posicion = armadura.Posicion;

            listaEntidades.Add(armaduraEntity);

            representaciones.ListaRepresentaciones = listaEntidades;

            barra.SetEntity(representaciones);
        }

        /// <summary> Guarda la lista de Representación de armaduras en la barra </summary>
        public static void GuardarRepresentacionArmaduraDeBarra(Rebar barra, List<ArmaduraRepresentacion> armaduras)
        {
            // Obtiene las armaduras que son de la barra
            List<ArmaduraRepresentacion> listaBarras = armaduras.Where(x => x.Barra.Id == barra.Id).ToList();

            foreach (ArmaduraRepresentacion armadura in listaBarras)
            {
                Tools.GuardarRepresentacionArmaduraDeBarra(barra, armadura);
            }
        }

        /// <summary> Guarda la lista de Representación de armaduras en cada barra correspondiente </summary>
        public static void GuardarRepresentacionArmaduraDeBarra(List<Rebar> barras, List<ArmaduraRepresentacion> armaduras)
        {
            foreach (Rebar barra in barras)
            {
                Tools.GuardarRepresentacionArmaduraDeBarra(barra, armaduras);
            }
        }

        /// <summary> Obtiene la Representación de armaduras de una barra </summary>
        public static RepresentacionesEntity ObtenerRepresentacionEntityDeBarra(Rebar barra)
        {
            RepresentacionesEntity representaciones = barra.GetEntity<RepresentacionesEntity>();

            if (representaciones == null)
            {
                representaciones = new RepresentacionesEntity();
            }

            if (representaciones.ListaRepresentaciones == null)
            {
                representaciones.ListaRepresentaciones = new List<ArmaduraRepresentacionEntity>();
            }

            return representaciones;
        }

        /// <summary> Obtiene la lista de Representación de armaduras de una barra </summary>
        public static List<ArmaduraRepresentacion> ObtenerRepresentacionArmaduraDeBarra(Rebar barra)
        {
            List<ArmaduraRepresentacion> armaduras = new List<ArmaduraRepresentacion>();

            RepresentacionesEntity representaciones = ObtenerRepresentacionEntityDeBarra(barra);

            if (representaciones != null)
            {
                foreach (ArmaduraRepresentacionEntity armRepEnt in representaciones.ListaRepresentaciones)
                {
                    if (armRepEnt.Vista != ElementId.InvalidElementId)
                    {
                        Document doc = barra.Document;

                        View vista = doc.GetElement(armRepEnt.Vista) as View;

                        ArmaduraRepresentacion armadura = new ArmaduraRepresentacion(doc, vista, barra);

                        armadura.CurvasDeArmadura = (from elemID in armRepEnt.ListaCurvas
                                                     where elemID != ElementId.InvalidElementId
                                                     select doc.GetElement(elemID) as CurveElement).ToList();

                        armadura.TextosDeLongitudesParciales = (from elemID in armRepEnt.ListaTextos
                                                                where elemID != ElementId.InvalidElementId
                                                                select doc.GetElement(elemID) as TextNote).ToList();

                        if (armRepEnt.TipoDeTexto != ElementId.InvalidElementId)
                        {
                            armadura.TipoDeTexto = doc.GetElement(armRepEnt.TipoDeTexto) as TextNoteType;
                        }                        

                        IndependentTag etiqueta = doc.GetElement(armRepEnt.Etiqueta) as IndependentTag;

                        if (etiqueta != null && etiqueta.GetTypeId() != ElementId.InvalidElementId)
                        {
                            armadura.EtiquetaArmadura = etiqueta;

                            armadura.TipoEtiquetaArmadura = doc.GetElement(etiqueta.GetTypeId()) as FamilySymbol;
                        }

                        if (armRepEnt.Posicion != null)
                        {
                            armadura.Posicion = armRepEnt.Posicion;
                        }

                        armaduras.Add(armadura);
                    }
                }
            }

            return armaduras;
        }

        /// <summary> Obtiene la lista de Representación de armaduras de una barra </summary>
        public static List<ArmaduraRepresentacion> ObtenerRepresentacionArmaduraDeBarra(List<Rebar> barras)
        {
            List<ArmaduraRepresentacion> armaduras = new List<ArmaduraRepresentacion>();

            foreach (Rebar barra in barras)
            {
                armaduras.AddRange(ObtenerRepresentacionArmaduraDeBarra(barra));
            }

            return armaduras;
        }

        /// <summary> Elimina la Representación de armaduras de la barra </summary>
        public static void EliminarRepresentacionesEnBarra(Rebar barra)
        {
            RepresentacionesEntity representaciones = new RepresentacionesEntity();

            try
            {
                barra.SetEntity(representaciones);
            }
            catch (Exception) { }
        }
        #endregion

        #region Etiquetas Independientes

        ///<summary> Crea una etiqueta según la configuración del usuario </summary>
        public static IndependentTag CrearEtiquetaSegunConfiguracion(Document doc, View vista, Element elem, FamilySymbol tipoEtiqueta, int posicion)
        {
            // Crea la etiqueta a devolver
            IndependentTag etiqueta = null;

            // Verifica que posición sea
            switch (posicion)
            {
                // Arriba a la izquierda
                case ArribaIzquierda:
                    etiqueta = CrearEtiquetaIndependienteArribaIzquierda(doc, vista, elem, tipoEtiqueta);
                    break;

                // Arriba centro
                case ArribaCentro:
                    etiqueta = CrearEtiquetaIndependienteArribaMedio(doc, vista, elem, tipoEtiqueta);
                    break;

                // Arriba a la derecha
                case ArribaDerecha:
                    etiqueta = CrearEtiquetaIndependienteArribaDerecha(doc, vista, elem, tipoEtiqueta);
                    break;

                // Centro a la izquierda
                case CentroIzquierda:
                    etiqueta = CrearEtiquetaIndependienteCentroIzquierda(doc, vista, elem, tipoEtiqueta);
                    break;

                // Centro medio
                case CentroMedio:
                    etiqueta = CrearEtiquetaIndependienteCentroMedio(doc, vista, elem, tipoEtiqueta);
                    break;

                // Centro a la derecha
                case CentroDerecha:
                    etiqueta = CrearEtiquetaIndependienteCentroDerecha(doc, vista, elem, tipoEtiqueta);
                    break;

                // Abajo a la izquierda
                case AbajoIzquierda:
                    etiqueta = CrearEtiquetaIndependienteAbajoIzquierda(doc, vista, elem, tipoEtiqueta);
                    break;

                // Abajo centro
                case AbajoCentro:
                    etiqueta = CrearEtiquetaIndependienteAbajoMedio(doc, vista, elem, tipoEtiqueta);
                    break;

                // Abajo a la derecha
                case AbajoDerecha:
                    etiqueta = CrearEtiquetaIndependienteAbajoDerecha(doc, vista, elem, tipoEtiqueta);
                    break;

                default:
                    break;
            }

            return etiqueta;
        }

        ///<summary> Crea una etiqueta horizontal en la parte superior izquierda sin guía en una vista particular </summary>
        public static IndependentTag CrearEtiquetaIndependienteArribaIzquierda(Document doc, View vista, Element elem, FamilySymbol tipoEtiqueta)
        {
            // Obtiene la referencia del elemento
            Reference referencia = new Reference(elem);

            // Crea la caja que contiene al elemento
            BoundingBoxXYZ bb = ObtenerRecuadroElementoParaleloAVista(doc, vista, elem);

            // Obtiene la ubicación del punto superior izquierdo
            double x = bb.Min.X;
            double y = bb.Min.Y;
            double z = bb.Max.Z;

            // Asigna donde se colocará la etiqueta
            XYZ punto = new XYZ(x, y, z);

            // Crea la etiqueta
            IndependentTag etiqueta = IndependentTag.Create(doc, tipoEtiqueta.Id, vista.Id, referencia, false, TagOrientation.Horizontal, punto);

            // Obtiene la caja que contiene a la etiqueta
            BoundingBoxXYZ bbetiqueta = etiqueta.get_BoundingBox(vista);

            // Obtiene mitad de la altura de la etiqueta
            double altura = (bbetiqueta.Max.Z - bbetiqueta.Min.Z) / 2;

            // Asigna la nueva ubicación a la etiqueta
            XYZ puntoFinal = new XYZ(x, y, (z + altura));

            // Coloca la etiqueta correctamente
            etiqueta.TagHeadPosition = puntoFinal;

            return etiqueta;
        }

        ///<summary> Crea una etiqueta horizontal en la parte superior central sin guía en una vista particular </summary>
        public static IndependentTag CrearEtiquetaIndependienteArribaMedio(Document doc, View vista, Element elem, FamilySymbol tipoEtiqueta)
        {         
            // Obtiene la referencia del elemento
            Reference referencia = new Reference(elem);

            // Crea la caja que contiene al elemento
            BoundingBoxXYZ bb = ObtenerRecuadroElementoParaleloAVista(doc, vista, elem);

            // Obtiene el volumen tridimensional del elemento
            double xMedio = (bb.Max.X - bb.Min.X) / 2;
            double yMedio = (bb.Max.Y - bb.Min.Y) / 2;
            
            // Obtiene la ubicación del punto medio superior
            double x = (bb.Min.X + xMedio);
            double y = (bb.Min.Y + yMedio);
            double z = bb.Max.Z;

            // Asigna donde se colocará la etiqueta
            XYZ punto = new XYZ(x, y, z);

            // Crea la etiqueta
            IndependentTag etiqueta = IndependentTag.Create(doc, tipoEtiqueta.Id, vista.Id, referencia, false, TagOrientation.Horizontal, punto);

            // Obtiene la caja que contiene a la etiqueta
            BoundingBoxXYZ bbetiqueta = etiqueta.get_BoundingBox(vista);

            // Obtiene mitad de la altura de la etiqueta
            double altura = (bbetiqueta.Max.Z - bbetiqueta.Min.Z) / 2;

            // Asigna la nueva ubicación a la etiqueta
            XYZ puntoFinal = new XYZ(x, y, (z + altura));

            // Coloca la etiqueta correctamente
            etiqueta.TagHeadPosition = puntoFinal;

            return etiqueta;
        }

        ///<summary> Crea una etiqueta horizontal en la parte superior derecha sin guía en una vista particular </summary>
        public static IndependentTag CrearEtiquetaIndependienteArribaDerecha(Document doc, View vista, Element elem, FamilySymbol tipoEtiqueta)
        {
            // Obtiene la referencia del elemento
            Reference referencia = new Reference(elem);

            // Crea la caja que contiene al elemento
            BoundingBoxXYZ bb = ObtenerRecuadroElementoParaleloAVista(doc, vista, elem);

            // Obtiene la ubicación del punto superior derecho
            double x = bb.Max.X;
            double y = bb.Max.Y;
            double z = bb.Max.Z;

            // Asigna donde se colocará la etiqueta
            XYZ punto = new XYZ(x, y, z);

            // Crea la etiqueta
            IndependentTag etiqueta = IndependentTag.Create(doc, tipoEtiqueta.Id, vista.Id, referencia, false, TagOrientation.Horizontal, punto);

            // Obtiene la caja que contiene a la etiqueta
            BoundingBoxXYZ bbetiqueta = etiqueta.get_BoundingBox(vista);

            // Obtiene mitad de la altura de la etiqueta
            double altura = (bbetiqueta.Max.Z - bbetiqueta.Min.Z) / 2;

            // Asigna la nueva ubicación a la etiqueta
            XYZ puntoFinal = new XYZ(x, y, (z + altura));

            // Coloca la etiqueta correctamente
            etiqueta.TagHeadPosition = puntoFinal;

            return etiqueta;
        }

        ///<summary> Crea una etiqueta horizontal en la parte media izquierda sin guía en una vista particular </summary>
        public static IndependentTag CrearEtiquetaIndependienteCentroIzquierda(Document doc, View vista, Element elem, FamilySymbol tipoEtiqueta)
        {
            // Obtiene la referencia del elemento
            Reference referencia = new Reference(elem);

            // Crea la caja que contiene al elemento
            BoundingBoxXYZ bb = ObtenerRecuadroElementoParaleloAVista(doc, vista, elem);

            // Obtiene el volumen tridimensional del elemento
            double zMedio = (bb.Max.Z - bb.Min.Z) / 2;

            // Obtiene la ubicación del punto central izquierdo
            double x = bb.Min.X;
            double y = bb.Min.Y;
            double z = (bb.Min.Z + zMedio);

            // Asigna donde se colocará la etiqueta
            XYZ punto = new XYZ(x, y, z);

            // Crea la etiqueta
            IndependentTag etiqueta = IndependentTag.Create(doc, tipoEtiqueta.Id, vista.Id, referencia, false, TagOrientation.Horizontal, punto);

            return etiqueta;
        }

        ///<summary> Crea una etiqueta horizontal en la parte media central sin guía en una vista particular </summary>
        public static IndependentTag CrearEtiquetaIndependienteCentroMedio(Document doc, View vista, Element elem, FamilySymbol tipoEtiqueta)
        {
            // Obtiene la referencia del elemento
            Reference referencia = new Reference(elem);

            // Crea la caja que contiene al elemento
            BoundingBoxXYZ bb = ObtenerRecuadroElementoParaleloAVista(doc, vista, elem);

            // Obtiene el volumen tridimensional del elemento
            double xMedio = (bb.Max.X - bb.Min.X) / 2;
            double yMedio = (bb.Max.Y - bb.Min.Y) / 2;
            double zMedio = (bb.Max.Z - bb.Min.Z) / 2;

            // Obtiene la ubicación del punto central medio
            double x = (bb.Min.X + xMedio);
            double y = (bb.Min.Y + yMedio);
            double z = (bb.Min.Z + zMedio);

            // Asigna donde se colocará la etiqueta
            XYZ punto = new XYZ(x, y, z);

            // Crea la etiqueta
            IndependentTag etiqueta = IndependentTag.Create(doc, tipoEtiqueta.Id, vista.Id, referencia, false, TagOrientation.Horizontal, punto);

            return etiqueta;
        }

        ///<summary> Crea una etiqueta horizontal en la parte media derecha sin guía en una vista particular </summary>
        public static IndependentTag CrearEtiquetaIndependienteCentroDerecha(Document doc, View vista, Element elem, FamilySymbol tipoEtiqueta)
        {
            // Obtiene la referencia del elemento
            Reference referencia = new Reference(elem);

            // Crea la caja que contiene al elemento
            BoundingBoxXYZ bb = ObtenerRecuadroElementoParaleloAVista(doc, vista, elem);

            // Obtiene el volumen tridimensional del elemento
            double zMedio = (bb.Max.Z - bb.Min.Z) / 2;

            // Obtiene la ubicación del punto central derecho
            double x = bb.Max.X;
            double y = bb.Max.Y;
            double z = (bb.Min.Z + zMedio);

            // Asigna donde se colocará la etiqueta
            XYZ punto = new XYZ(x, y, z);

            // Crea la etiqueta
            IndependentTag etiqueta = IndependentTag.Create(doc, tipoEtiqueta.Id, vista.Id, referencia, false, TagOrientation.Horizontal, punto);

            return etiqueta;
        }

        ///<summary> Crea una etiqueta horizontal en la parte inferior izquierda sin guía en una vista particular </summary>
        public static IndependentTag CrearEtiquetaIndependienteAbajoIzquierda(Document doc, View vista, Element elem, FamilySymbol tipoEtiqueta)
        {
            // Obtiene la referencia del elemento
            Reference referencia = new Reference(elem);

            // Crea la caja que contiene al elemento
            BoundingBoxXYZ bb = ObtenerRecuadroElementoParaleloAVista(doc, vista, elem);

            // Obtiene la ubicación del punto inferior izquierdo
            double x = bb.Min.X;
            double y = bb.Min.Y;
            double z = bb.Min.Z;

            // Asigna donde se colocará la etiqueta
            XYZ punto = new XYZ(x, y, z);

            // Crea la etiqueta
            IndependentTag etiqueta = IndependentTag.Create(doc, tipoEtiqueta.Id, vista.Id, referencia, false, TagOrientation.Horizontal, punto);

            // Obtiene la caja que contiene a la etiqueta
            BoundingBoxXYZ bbetiqueta = etiqueta.get_BoundingBox(vista);

            // Obtiene mitad de la altura de la etiqueta
            double altura = (bbetiqueta.Max.Z - bbetiqueta.Min.Z) / 2;

            // Asigna la nueva ubicación a la etiqueta
            XYZ puntoFinal = new XYZ(x, y, (z - altura));

            // Coloca la etiqueta correctamente
            etiqueta.TagHeadPosition = puntoFinal;

            return etiqueta;
        }

        ///<summary> Crea una etiqueta horizontal en la parte inferior central sin guía en una vista particular </summary>
        public static IndependentTag CrearEtiquetaIndependienteAbajoMedio(Document doc, View vista, Element elem, FamilySymbol tipoEtiqueta)
        {
            // Obtiene la referencia del elemento
            Reference referencia = new Reference(elem);

            // Crea la caja que contiene al elemento
            BoundingBoxXYZ bb = ObtenerRecuadroElementoParaleloAVista(doc, vista, elem);

            // Obtiene el volumen tridimensional del elemento
            double xMedio = (bb.Max.X - bb.Min.X) / 2;
            double yMedio = (bb.Max.Y - bb.Min.Y) / 2;

            // Obtiene la ubicación del punto medio inferior
            double x = (bb.Min.X + xMedio);
            double y = (bb.Min.Y + yMedio);
            double z = bb.Min.Z;

            // Asigna donde se colocará la etiqueta
            XYZ punto = new XYZ(x, y, z);

            // Crea la etiqueta
            IndependentTag etiqueta = IndependentTag.Create(doc, tipoEtiqueta.Id, vista.Id, referencia, false, TagOrientation.Horizontal, punto);

            // Obtiene la caja que contiene a la etiqueta
            BoundingBoxXYZ bbetiqueta = etiqueta.get_BoundingBox(vista);

            // Obtiene mitad de la altura de la etiqueta
            double altura = bbetiqueta.Max.Z - bbetiqueta.Min.Z;

            // Asigna la nueva ubicación a la etiqueta
            XYZ puntoFinal = new XYZ(x, y, (z - altura));

            // Coloca la etiqueta correctamente
            etiqueta.TagHeadPosition = puntoFinal;

            return etiqueta;
        }

        ///<summary> Crea una etiqueta horizontal en la parte inferior derecha sin guía en una vista particular </summary>
        public static IndependentTag CrearEtiquetaIndependienteAbajoDerecha(Document doc, View vista, Element elem, FamilySymbol tipoEtiqueta)
        {
            // Obtiene la referencia del elemento
            Reference referencia = new Reference(elem);

            // Crea la caja que contiene al elemento
            BoundingBoxXYZ bb = ObtenerRecuadroElementoParaleloAVista(doc, vista, elem);

            // Obtiene la ubicación del punto inferior derecho
            double x = bb.Max.X;
            double y = bb.Max.Y;
            double z = bb.Min.Z;

            // Asigna donde se colocará la etiqueta
            XYZ punto = new XYZ(x, y, z);

            // Crea la etiqueta
            IndependentTag etiqueta = IndependentTag.Create(doc, tipoEtiqueta.Id, vista.Id, referencia, false, TagOrientation.Horizontal, punto);

            // Obtiene la caja que contiene a la etiqueta
            BoundingBoxXYZ bbetiqueta = etiqueta.get_BoundingBox(vista);

            // Obtiene mitad de la altura de la etiqueta
            double altura = (bbetiqueta.Max.Z - bbetiqueta.Min.Z) / 2;

            // Asigna la nueva ubicación a la etiqueta
            XYZ puntoFinal = new XYZ(x, y, (z - altura));

            // Coloca la etiqueta correctamente
            etiqueta.TagHeadPosition = puntoFinal;

            return etiqueta;
        }

        ///<summary> Obtiene el vector para mover la etiqueta según la dirección dada </summary>
        public static XYZ ObtenerVectorParaMoverEtiqueta(View vista, XYZ direccion, IndependentTag etiqueta, List<Dimension> cotas)
        {
            // Crea el vector
            XYZ vector = new XYZ();

            // Obtiene la transformada de la vista
            Transform tra = vista.CropBox.Transform;

            // Obtiene la dirección en coordenadas de la vista
            XYZ direccionModificada = tra.Inverse.OfVector(direccion);

            // Crea los componentes del vector
            double x = 0;
            double y = 0;
            double z = 0;

            // Obtiene el recuadro de la etiqueta
            BoundingBoxXYZ bbEtiqueta = etiqueta.get_BoundingBox(vista);

            // Obtiene los bordes en coordenadas de la vista
            XYZ etiqueta1 = tra.Inverse.OfPoint(bbEtiqueta.Max);
            XYZ etiqueta2 = tra.Inverse.OfPoint(bbEtiqueta.Min);

            // Obtiene los puntos máximos y mínimos en coordenadas de la vista
            XYZ etiquetaMax = ObtenerPuntoMaximo(etiqueta1, etiqueta2);
            XYZ etiquetaMin = ObtenerPuntoMinimo(etiqueta1, etiqueta2);

            // Recorre todas las cotas
            foreach (Dimension cota in cotas)
            {
                // Verifica que la dirección no sea nula
                if (!direccion.IsZeroLength())
                {
                    // Obtiene el recuadro de la cota
                    BoundingBoxXYZ bbCota = cota.get_BoundingBox(vista);
                    
                    // Obtiene los bordes en coordenadas de la vista
                    XYZ cota1 = tra.Inverse.OfPoint(bbCota.Max);
                    XYZ cota2 = tra.Inverse.OfPoint(bbCota.Min);

                    // Obtiene los puntos máximos y mínimos en coordenadas de la vista
                    XYZ cotaMax = ObtenerPuntoMaximo(cota1, cota2);
                    XYZ cotaMin = ObtenerPuntoMinimo(cota1, cota2);

                    // Obtiene los componentes del vector
                    double xCota = ComponenteDeVectorParaEtiqueta(direccionModificada.X, etiquetaMax.X, etiquetaMin.X, cotaMax.X, cotaMin.X);
                    double yCota = ComponenteDeVectorParaEtiqueta(direccionModificada.Y, etiquetaMax.Y, etiquetaMin.Y, cotaMax.Y, cotaMin.Y);
                    double zCota = ComponenteDeVectorParaEtiqueta(direccionModificada.Z, etiquetaMax.Z, etiquetaMin.Z, cotaMax.Z, cotaMin.Z);
                    
                    // Verifica si los componentes van en la misma dirección 
                    x = ComponenteDeVectorMayorOMenorQuePunto(direccionModificada.X, xCota, x);
                    y = ComponenteDeVectorMayorOMenorQuePunto(direccionModificada.Y, yCota, y);
                    z = ComponenteDeVectorMayorOMenorQuePunto(direccionModificada.Z, zCota, z);
                }
            }
            
            // Transforma el vector de las coordenadas de la vista a coordenadas globales
            vector = tra.OfVector(new XYZ(x, y, z));

            return vector;
        }
        
        ///<summary> Obtiene el vector para mover la etiqueta de profundidad según la dirección dada </summary>
        public static XYZ ObtenerVectorParaMoverEtiqueta(View vista, XYZ direccion, SpotDimension cotaProfundidad, List<Dimension> cotas)
        {
            // Crea el vector
            XYZ vector = new XYZ();

            // Obtiene la transformada de la vista
            Transform tra = vista.CropBox.Transform;

            // Obtiene la dirección en coordenadas de la vista
            XYZ direccionModificada = tra.Inverse.OfVector(direccion);

            // Crea los componentes del vector
            double x = 0;
            double y = 0;
            double z = 0;

            // Obtiene el recuadro de la etiqueta
            BoundingBoxXYZ bbEtiqueta = cotaProfundidad.get_BoundingBox(vista);

            // Obtiene los bordes en coordenadas de la vista
            XYZ etiqueta1 = tra.Inverse.OfPoint(bbEtiqueta.Max);
            XYZ etiqueta2 = tra.Inverse.OfPoint(bbEtiqueta.Min);

            // Obtiene los puntos máximos y mínimos en coordenadas de la vista
            XYZ etiquetaMax = ObtenerPuntoMaximo(etiqueta1, etiqueta2);
            XYZ etiquetaMin = ObtenerPuntoMinimo(etiqueta1, etiqueta2);

            // Recorre todas las cotas
            foreach (Dimension cota in cotas)
            {
                // Verifica que la dirección no sea nula
                if (!direccion.IsZeroLength())
                {
                    // Obtiene el recuadro de la cota
                    BoundingBoxXYZ bbCota = cota.get_BoundingBox(vista);

                    // Obtiene los bordes en coordenadas de la vista
                    XYZ cota1 = tra.Inverse.OfPoint(bbCota.Max);
                    XYZ cota2 = tra.Inverse.OfPoint(bbCota.Min);

                    // Obtiene los puntos máximos y mínimos en coordenadas de la vista
                    XYZ cotaMax = ObtenerPuntoMaximo(cota1, cota2);
                    XYZ cotaMin = ObtenerPuntoMinimo(cota1, cota2);

                    // Obtiene los componentes del vector
                    double xCota = ComponenteDeVectorParaEtiqueta(direccionModificada.X, etiquetaMax.X, etiquetaMin.X, cotaMax.X, cotaMin.X);
                    double yCota = ComponenteDeVectorParaEtiqueta(direccionModificada.Y, etiquetaMax.Y, etiquetaMin.Y, cotaMax.Y, cotaMin.Y);
                    double zCota = ComponenteDeVectorParaEtiqueta(direccionModificada.Z, etiquetaMax.Z, etiquetaMin.Z, cotaMax.Z, cotaMin.Z);

                    // Verifica si los componentes van en la misma dirección 
                    x = ComponenteDeVectorMayorOMenorQuePunto(direccionModificada.X, xCota, x);
                    y = ComponenteDeVectorMayorOMenorQuePunto(direccionModificada.Y, yCota, y);
                    z = ComponenteDeVectorMayorOMenorQuePunto(direccionModificada.Z, zCota, z);
                }
            }

            // Transforma el vector de las coordenadas de la vista a coordenadas globales
            vector = tra.OfVector(new XYZ(x, y, z));

            return vector;
        }
        
        ///<summary> Obtiene la distancia del punto máximo o mínimo entre la cota y la etiqueta </summary>
        public static double ComponenteDeVectorParaEtiqueta(double direccion, double etiquetaMax, double etiquetaMin, double cotaMax, double cotaMin)
        {
            // Crea el componente
            double componente = 0;

            // Verifica que sea mayor a la tolerancia
            if (Math.Abs(direccion) > toleranciaComponenteVector)
            {
                // Verifica que la dirección sea positiva
                if (direccion > toleranciaComponenteVector)
                {
                    // Asigna la distancia entre la etiqueta y la cota
                    componente = (etiquetaMin < cotaMax) ? cotaMax - etiquetaMin : 0;
                }

                // La dirección es negativa
                else
                {
                    // Asigna la distancia entre la etiqueta y la cota
                    componente = (etiquetaMax > cotaMin) ? cotaMin - etiquetaMax : 0;
                }
            }

            return componente;
        }

        ///<summary> Devuelve el máximo o mínimo entre la cota y el punto según la dirección </summary>
        public static double ComponenteDeVectorMayorOMenorQuePunto(double direccion, double cota, double punto)
        {
            // Crea el componente
            double componente = punto;

            // Verifica que la dirección sea mayor a la tolerancia
            if (Math.Abs(direccion) > toleranciaComponenteVector)
            {
                // Verifica si la dirección es positiva
                if (direccion > toleranciaComponenteVector)
                {
                    // Verifica si la cota es mayor al punto
                    if (cota > punto)
                    {
                        componente = cota;
                    }
                }

                // La dirección es negativa
                else
                {
                    // Verifica si la cota es menor al punto
                    if (cota < punto)
                    {
                        componente = cota;
                    }
                }
            }

            return componente;
        }

        ///<summary> Obtiene el punto máximo dado dos puntos </summary>
        public static XYZ ObtenerPuntoMaximo(XYZ punto1, XYZ punto2)
        {
            // Obtiene los mayores
            double x = (punto1.X > punto2.X) ? punto1.X : punto2.X;
            double y = (punto1.Y > punto2.Y) ? punto1.Y : punto2.Y;
            double z = (punto1.Z > punto2.Z) ? punto1.Z : punto2.Z;

            // Crea el punto
            XYZ punto = new XYZ(x, y, z);

            return punto;
        }

        ///<summary> Obtiene el punto mínimo dado dos puntos </summary>
        public static XYZ ObtenerPuntoMinimo(XYZ punto1, XYZ punto2)
        {
            // Obtiene los mayores
            double x = (punto1.X < punto2.X) ? punto1.X : punto2.X;
            double y = (punto1.Y < punto2.Y) ? punto1.Y : punto2.Y;
            double z = (punto1.Z < punto2.Z) ? punto1.Z : punto2.Z;

            // Crea el punto
            XYZ punto = new XYZ(x, y, z);

            return punto;
        }

        ///<summary> Proyecta el vector sobre la dirección dada </summary>
        public static XYZ ProyectarVectorSobreDireccion(XYZ vector, XYZ direccion)
        {
            // Obtiene el producto escalar
            double escalar = vector.DotProduct(direccion) / direccion.GetLength();

            // Obtiene el vector proyectado en la dirección
            XYZ vectorProyeccion = direccion.Multiply(escalar);

            return vectorProyeccion;
        }

        ///<summary> Obtiene la dirección según la posición de la etiqueta </summary>
        public static XYZ DireccionSegunPosicionDeEtiqueta(View vista, int posicion)
        {
            // Crea la dirección
            XYZ direccion = new XYZ();

            // Verifica que posición sea
            switch (posicion)
            {
                // Arriba a la izquierda
                case ArribaIzquierda:
                    direccion = (vista.UpDirection - vista.RightDirection).Normalize();
                    break;

                // Arriba centro
                case ArribaCentro:
                    direccion = vista.UpDirection;
                    break;

                // Arriba a la derecha
                case ArribaDerecha:
                    direccion = (vista.UpDirection + vista.RightDirection).Normalize();
                    break;

                // Centro a la izquierda
                case CentroIzquierda:
                    direccion = -vista.RightDirection;
                    break;

                // Centro medio
                case CentroMedio:
                    direccion = new XYZ();
                    break;

                // Centro a la derecha
                case CentroDerecha:
                    direccion = vista.RightDirection;
                    break;

                // Abajo a la izquierda
                case AbajoIzquierda:
                    direccion = (-vista.UpDirection - vista.RightDirection).Normalize();
                    break;

                // Abajo centro
                case AbajoCentro:
                    direccion = -vista.UpDirection;
                    break;

                // Abajo a la derecha
                case AbajoDerecha:
                    direccion = (-vista.UpDirection + vista.RightDirection).Normalize();
                    break;

                default:
                    break;
            }

            return direccion;
        }

        ///<summary> Obtiene el recuadro del elemento con los ejes paralelos a la vista y en coordenadas globales </summary>
        public static BoundingBoxXYZ ObtenerRecuadroElementoParaleloAVista(Document doc, View vista, Element elem)
        {
            // Crea el recuadro a devolver
            BoundingBoxXYZ bb = new BoundingBoxXYZ();

            // Abre una subtransacción
            using (SubTransaction subT = new SubTransaction(doc))
            {
                subT.Start();

                // Obtiene la transformada de la vista
                Transform tra = vista.CropBox.Transform;

                // Obtiene el cuadro del elemento
                BoundingBoxXYZ bbElem = elem.get_BoundingBox(vista);

                // Obtiene el origen de la vista
                XYZ origen = (bbElem.Max + bbElem.Min) / 2;

                // Crea las transformaciones
                Transform traMover = Transform.CreateTranslation(-origen);
                Transform traEjeZ = null;
                Transform traEjeX = null;

                // Mueve el elemento al origen global
                ElementTransformUtils.MoveElement(doc, elem.Id, -origen);

                // Verifica si el eje vertical de la vista coincide con el eje vertical global
                if (!tra.BasisY.IsAlmostEqualTo(XYZ.BasisZ))
                {
                    // Obtiene el ángulo entre los ejes
                    double angulo = XYZ.BasisZ.AngleTo(tra.BasisY);

                    // Obtiene el vector perpendicular
                    XYZ perpendicular = XYZ.BasisZ.CrossProduct(tra.BasisY);

                    // Crea la transformación
                    traEjeZ = Transform.CreateRotation(perpendicular, -angulo);

                    // Crea la transformación
                    Line linea = Line.CreateBound(new XYZ(), perpendicular);

                    // Rota el elemento
                    ElementTransformUtils.RotateElement(doc, elem.Id, linea, -angulo);
                }

                // Verifica si el eje horizontal de la vista coincide con el eje horizontal global
                if (!tra.BasisX.IsAlmostEqualTo(XYZ.BasisX))
                {
                    // Obtiene el ángulo entre los ejes
                    double angulo = XYZ.BasisX.AngleTo(tra.BasisX);

                    // Crea la transformación
                    traEjeX = Transform.CreateRotation(XYZ.BasisZ, -angulo);

                    // Crea la transformación
                    Line linea = Line.CreateBound(new XYZ(), XYZ.BasisZ);

                    // Rota el elemento
                    ElementTransformUtils.RotateElement(doc, elem.Id, linea, -angulo);
                }

                // Asigna los puntos
                XYZ puntoMaximo = elem.get_BoundingBox(vista).Max;
                XYZ puntoMinimo = elem.get_BoundingBox(vista).Min;

                // Verifica si la transformación vertical no sea nula
                if (traEjeZ != null)
                {
                    // Rota los puntos
                    puntoMaximo = traEjeZ.Inverse.OfPoint(puntoMaximo);
                    puntoMinimo = traEjeZ.Inverse.OfPoint(puntoMinimo);
                }

                // Verifica si la transformación horizontal no sea nula
                if (traEjeX != null)
                {
                    // Rota los puntos
                    puntoMaximo = traEjeX.Inverse.OfPoint(puntoMaximo);
                    puntoMinimo = traEjeX.Inverse.OfPoint(puntoMinimo);
                }

                // Mueve los puntos
                puntoMaximo = traMover.Inverse.OfPoint(puntoMaximo);
                puntoMinimo = traMover.Inverse.OfPoint(puntoMinimo);

                // Asigna los puntos finales al recuadro 
                bb.Max = puntoMaximo;
                bb.Min = puntoMinimo;

                subT.RollBack();
            }

            return bb;
        }

        ///<summary> Obtiene el recuadro del elemento ubicado en el origen </summary>
        public static BoundingBoxXYZ ObtenerRecuadroElementoEnOrigen(Document doc, View vista, Element elem)
        {
            // Crea el recuadro a devolver
            BoundingBoxXYZ bb = new BoundingBoxXYZ();

            // Abre una subtransacción
            using (SubTransaction subT = new SubTransaction(doc))
            {
                subT.Start();

                // Obtiene la transformada de la vista
                Transform tra = vista.CropBox.Transform;

                // Obtiene el cuadro del elemento
                BoundingBoxXYZ bbElem = elem.get_BoundingBox(null);

                // Verifica si el elemento es nulo
                if (bbElem == null)
                {
                    // Obtiene el cuadro en función a la vista
                    bbElem = elem.get_BoundingBox(vista);
                }

                // Obtiene el origen de la vista
                XYZ origen = (bbElem.Max + bbElem.Min) / 2;

                // Crea las transformaciones
                Transform traMover = Transform.CreateTranslation(-origen);
                Transform traEjeZ = null;
                Transform traEjeX = null;

                // Mueve el elemento al origen global
                ElementTransformUtils.MoveElement(doc, elem.Id, -origen);

                // Verifica si el eje vertical de la vista coincide con el eje vertical global
                if (!tra.BasisY.IsAlmostEqualTo(XYZ.BasisZ))
                {
                    // Obtiene el ángulo entre los ejes
                    double angulo = XYZ.BasisZ.AngleTo(tra.BasisY);

                    // Obtiene el vector perpendicular
                    XYZ perpendicular = XYZ.BasisZ.CrossProduct(tra.BasisY);

                    // Crea la transformación
                    traEjeZ = Transform.CreateRotation(perpendicular, -angulo);

                    // Crea la transformación
                    Line linea = Line.CreateBound(new XYZ(), perpendicular);

                    // Rota el elemento
                    ElementTransformUtils.RotateElement(doc, elem.Id, linea, -angulo);
                }

                // Verifica si el eje horizontal de la vista coincide con el eje horizontal global
                if (!tra.BasisX.IsAlmostEqualTo(XYZ.BasisX))
                {
                    // Obtiene el ángulo entre los ejes
                    double angulo = XYZ.BasisX.AngleTo(tra.BasisX);

                    // Crea la transformación
                    traEjeX = Transform.CreateRotation(XYZ.BasisZ, -angulo);

                    // Crea la transformación
                    Line linea = Line.CreateBound(new XYZ(), XYZ.BasisZ);

                    // Rota el elemento
                    ElementTransformUtils.RotateElement(doc, elem.Id, linea, -angulo);
                }

                // Verifica si existe Cuadro
                if (elem.get_BoundingBox(null) == null)
                {
                    // Asigna los puntos finales al recuadro 
                    bb.Max = elem.get_BoundingBox(vista).Max;
                    bb.Min = elem.get_BoundingBox(vista).Min;
                }

                else
                {
                    // Asigna los puntos finales al recuadro 
                    bb.Max = elem.get_BoundingBox(null).Max;
                    bb.Min = elem.get_BoundingBox(null).Min;
                }

                subT.RollBack();
            }

            return bb;
        }

        #endregion

        #region Cotas lineales

        ///<summary> Crea las cotas según las opciones </summary>
        public static List<Dimension> CrearCotaParaElemento(Document doc, View vista, Element elem, DimensionType tipoCota)
        {
            // Crea una lista con las cotas
            List<Dimension> cotas = new List<Dimension>();

            // Verifica que esté activo la cota vertical izquierda
            if (Jump.Properties.Settings.Default.CotaVerticalIzquierda)
            {
                try
                {
                    // Crea la cota vertical izquierda
                    cotas.Add(CrearCotaVerticalIzquierdaParaElemento(doc, vista, elem, tipoCota));
                }
                catch (Exception) { }
            }

            // Verifica que esté activo la cota vertical derecha
            if (Jump.Properties.Settings.Default.CotaVerticalDerecha)
            {
                try
                {
                    // Crea la cota vertical derecha
                    cotas.Add(CrearCotaVerticalDerechaParaElemento(doc, vista, elem, tipoCota));
                }
                catch (Exception) { }
            }

            // Verifica que esté activo la cota horizontal arriba
            if (Jump.Properties.Settings.Default.CotaHorizontalArriba)
            {
                try
                {
                    // Crea la cota horizontal arriba
                    cotas.Add(CrearCotaHorizontalArribaParaElemento(doc, vista, elem, tipoCota));
                }
                catch (Exception) { }
            }

            // Verifica que esté activo la cota horizontal abajo
            if (Jump.Properties.Settings.Default.CotaHorizontalAbajo)
            {
                try
                {
                    // Crea la cota horizontal abajo
                    cotas.Add(CrearCotaHorizontalAbajoParaElemento(doc, vista, elem, tipoCota));
                }
                catch (Exception) { }
            }

            return cotas;
        }

        ///<summary> Crea una cota vertical a la izquierda de un elemento en una vista particular </summary>
        public static Dimension CrearCotaVerticalIzquierdaParaElemento(Document doc, View vista, Element elem, DimensionType tipoCota)
        {
            // Crea la cota
            Dimension cota;

            // Crea un arreglo con las referencias
            ReferenceArray ArregloRef = new ReferenceArray();

            // Agrega las referencias del plano superior e inferior del elemento
            ArregloRef.Append(ReferenciaCaraExtremaElementoEnVista(vista, -vista.UpDirection, elem));
            ArregloRef.Append(ReferenciaCaraExtremaElementoEnVista(vista, vista.UpDirection, elem));
            
            // Crea una caja de sección de la vista
            BoundingBoxXYZ bb = vista.CropBox;

            // Crea las coordenadas de la linea
            XYZ puntoInicial = bb.Transform.OfPoint(bb.Min);
            XYZ puntoFinal = puntoInicial.Add(vista.UpDirection);

            // Verifica que sea basado en punto
            if (elem.Location is LocationPoint)
            {
                // Crea la linea
                Line linea = Line.CreateBound(puntoInicial, puntoFinal);

                // Crea la cota
                cota = doc.Create.NewDimension(vista, linea, ArregloRef, tipoCota);
                
                // Obtiene la distancia a mover
                XYZ direccion = cota.LeaderEndPosition - cota.Origin;

                // Mueve la cota hacia la izquierda
                ElementTransformUtils.MoveElement(doc, cota.Id, direccion);
            }

            // Verifica que sea basado en curva
            else
            {
                // Obtiene el FamilyInstance
                FamilyInstance fi = elem as FamilyInstance;

                // Obtiene la curva de conducción
                Curve curva = (elem.Location as LocationCurve).Curve;

                // Obtiene la caja de sección de la vista
                BoundingBoxXYZ bbox = vista.CropBox;

                // Crea una traslación
                Transform tra = bbox.Transform;

                // Crea el punto inicial
                XYZ punto = bbox.Min;

                // Asigna las coordenadas de la linea
                puntoInicial = tra.OfPoint(punto);
                puntoFinal = puntoInicial.Add(vista.UpDirection);

                // Crea la linea
                Line linea = Line.CreateBound(puntoInicial, puntoFinal);

                // Crea la cota
                cota = doc.Create.NewDimension(vista, linea, ArregloRef, tipoCota);

                // Obtiene la distancia a mover
                XYZ direccion = cota.LeaderEndPosition - cota.Origin;

                // Mueve la cota hacia la izquierda
                ElementTransformUtils.MoveElement(doc, cota.Id, direccion);
            }

            return cota;
        }

        ///<summary> Crea una cota vertical a la derecha de un elemento en una vista particular </summary>
        public static Dimension CrearCotaVerticalDerechaParaElemento(Document doc, View vista, Element elem, DimensionType tipoCota)
        {
            // Crea la cota
            Dimension cota;

            // Crea un arreglo con las referencias
            ReferenceArray ArregloRef = new ReferenceArray();

            // Agrega las referencias del plano superior e inferior del elemento
            ArregloRef.Append(ReferenciaCaraExtremaElementoEnVista(vista, -vista.UpDirection, elem));
            ArregloRef.Append(ReferenciaCaraExtremaElementoEnVista(vista, vista.UpDirection, elem));

            // Crea una caja de sección del elemento
            BoundingBoxXYZ bb = vista.CropBox;

            // Crea las coordenadas de la linea
            XYZ puntoInicial = bb.Transform.OfPoint(bb.Max);
            XYZ puntoFinal = puntoInicial.Subtract(vista.UpDirection);

            // Verifica que sea basado en punto
            if (elem.Location is LocationPoint)
            {
                // Crea la linea
                Line linea = Line.CreateBound(puntoInicial, puntoFinal);

                // Crea la cota
                cota = doc.Create.NewDimension(vista, linea, ArregloRef, tipoCota);

                // Obtiene la distancia a mover
                XYZ direccion = multiplicadorDistanciaCotasLinealesDerecha * (cota.Origin - cota.LeaderEndPosition);

                // Mueve la cota hacia la derecha
                ElementTransformUtils.MoveElement(doc, cota.Id, direccion);
            }

            else
            {
                // Obtiene el FamilyInstance
                FamilyInstance fi = elem as FamilyInstance;

                // Obtiene la curva de conducción
                Curve curva = (elem.Location as LocationCurve).Curve;

                // Obtiene la caja de sección de la vista
                BoundingBoxXYZ bbox = vista.CropBox;

                // Crea una traslación
                Transform tra = bbox.Transform;

                // Crea el punto inicial
                XYZ punto = bbox.Max;

                // Asigna las coordenadas de la linea
                puntoInicial = tra.OfPoint(punto);
                puntoFinal = puntoInicial.Add(vista.UpDirection);

                // Crea la linea
                Line linea = Line.CreateBound(puntoInicial, puntoFinal);

                // Crea la cota
                cota = doc.Create.NewDimension(vista, linea, ArregloRef, tipoCota);

                // Obtiene la distancia a mover
                XYZ direccion = multiplicadorDistanciaCotasLinealesDerecha * (cota.Origin - cota.LeaderEndPosition);

                // Mueve la cota hacia la izquierda
                ElementTransformUtils.MoveElement(doc, cota.Id, direccion);
            }

            return cota;
        }

        ///<summary> Crea una cota horizontal arriba de un elemento en una vista particular </summary>
        public static Dimension CrearCotaHorizontalArribaParaElemento(Document doc, View vista, Element elem, DimensionType tipoCota)
        {
            // Crea un arreglo con las referencias
            ReferenceArray ArregloRef = new ReferenceArray();

            // Agrega las referencias del plano
            ArregloRef.Append(ReferenciaCaraExtremaElementoEnVista(vista, -vista.RightDirection, elem));
            ArregloRef.Append(ReferenciaCaraExtremaElementoEnVista(vista, vista.RightDirection, elem));

            // Crea una caja de sección del elemento
            BoundingBoxXYZ bb = elem.get_BoundingBox(vista);

            // Crea las coordenadas de la linea
            XYZ puntoInicial = bb.Max; 
            XYZ puntoFinal = puntoInicial.Subtract(vista.RightDirection);

            // Crea la linea
            Line linea = Line.CreateBound(puntoInicial, puntoFinal);

            // Crea la cota
            Dimension cota = doc.Create.NewDimension(vista, linea, ArregloRef, tipoCota);

            // Crea una caja de sección del elemento
            BoundingBoxXYZ bbCota = cota.get_BoundingBox(vista);

            // Obtiene la altura del texto
            double zCota = Math.Abs(cota.LeaderEndPosition.Z - cota.Origin.Z);

            // Mueve la cota hacia abajo
            ElementTransformUtils.MoveElement(doc, cota.Id, new XYZ(0, 0, zCota));

            return cota;
        }

        ///<summary> Crea una cota horizontal abajo de un elemento en una vista particular </summary>
        public static Dimension CrearCotaHorizontalAbajoParaElemento(Document doc, View vista, Element elem, DimensionType tipoCota)
        {
            // Crea un arreglo con las referencias
            ReferenceArray ArregloRef = new ReferenceArray();

            // Agrega las referencias del plano
            ArregloRef.Append(ReferenciaCaraExtremaElementoEnVista(vista, -vista.RightDirection, elem));
            ArregloRef.Append(ReferenciaCaraExtremaElementoEnVista(vista, vista.RightDirection, elem));

            // Crea una caja de sección del elemento
            BoundingBoxXYZ bb = elem.get_BoundingBox(vista);

            // Crea las coordenadas de la linea
            XYZ puntoInicial = bb.Min;
            XYZ puntoFinal = puntoInicial.Add(vista.RightDirection);

            // Crea la linea temporal
            Line linea = Line.CreateBound(puntoInicial, puntoFinal);

            // Crea la cota temporal
            Dimension cota = doc.Create.NewDimension(vista, linea, ArregloRef, tipoCota);

            // Crea una caja de sección del elemento
            BoundingBoxXYZ bbCota = cota.get_BoundingBox(vista);

            // Obtiene la altura del texto
            double alturaTexto = Math.Abs(bbCota.Min.Z - cota.LeaderEndPosition.Z) + Math.Abs(cota.TextPosition.Z - cota.LeaderEndPosition.Z);
            double zCota = multiplicadorDistanciaCotasLinealesAbajo * alturaTexto;

            // Mueve la cota hacia abajo
            ElementTransformUtils.MoveElement(doc, cota.Id, new XYZ(0, 0, -zCota));

            return cota;
        }

        #endregion

        #region Cota de profundidad

        ///<summary> Crea una cota de profundidad según las opciones </summary>
        public static SpotDimension CrearCotaProfundidad(Document doc, View vista, Element elem, SpotDimensionType tipoCotaProfundidad, int posicion)
        {
            // Crea la cota de profundidad
            SpotDimension cotaProfundidad = null;

            // Verifica que posición sea
            switch (posicion)
            {
                // Crea la cota de profundidad abajo a la izquierda
                case AbajoIzquierda:
                    cotaProfundidad = CrearCotaProfundidadAbajoIzquierda(doc, vista, elem, tipoCotaProfundidad);
                    break;

                // Crea la cota de profundidad abajo a la derecha
                case AbajoDerecha:
                    cotaProfundidad = CrearCotaProfundidadAbajoDerecha(doc, vista, elem, tipoCotaProfundidad);
                    break;

                default:
                    break;
            }

            return cotaProfundidad;
        }

        ///<summary> Crea una cota de profundidad abajo a la izquierda </summary>
        public static SpotDimension CrearCotaProfundidadAbajoIzquierda(Document doc, View vista, Element elem, SpotDimensionType tipoCotaProfundidad)
        {
            // Obtiene la referencia del elemento
            Reference referencia = ReferenciaCaraExtremaElementoEnVista(vista, -vista.UpDirection, elem);
            
            // Crea la bandera de la geometría
            bool banderaGeometria = false;

            // Obtiene el punto medio de la cara referenciada
            XYZ puntoMedioCara = ObtenerPuntoMedioCaraParaCotaProfundidad(doc, elem, referencia, ref banderaGeometria);

            // Crea la caja que contiene al elemento
            BoundingBoxXYZ bb = elem.get_BoundingBox(null);

            // Coordenadas del origen de la etiqueta
            XYZ puntoOrigen = puntoMedioCara;

            // Obtiene el cuadro de la vista
            BoundingBoxXYZ bbox = vista.get_BoundingBox(null);

            // Obtiene la transformación de la vista
            Transform tra = bbox.Transform;

            // Transforma las coordenadas máximas de la vista a coordenadas del proyecto
            XYZ vistaMin = tra.OfPoint(bbox.Min);

            // Coordenadas del pliegue de la etiqueta abajo a la derecha
            XYZ puntoPliegueTemp = new XYZ(vistaMin.X, vistaMin.Y, puntoMedioCara.Z);

            // Coordenadas después del pliegue
            XYZ puntoFinTemp = new XYZ(vistaMin.X, vistaMin.Y, puntoMedioCara.Z);

            // Coordenadas finales del pliegue
            XYZ puntoPliegue = new XYZ();

            // Coordenadas finales después del pliegue
            XYZ puntoFin = new XYZ();

            // Abre una subtransacción
            using (SubTransaction subT = new SubTransaction(doc))
            {
                subT.Start();

                // Crea la etiqueta temporal
                SpotDimension etiquetaTemporal = doc.Create.NewSpotElevation
                                                    (vista, referencia, puntoOrigen, puntoPliegueTemp, puntoFinTemp, puntoOrigen, false);

                // Crea la caja que contiene a la etiqueta temporal
                BoundingBoxXYZ bbEtiqueta = etiquetaTemporal.get_BoundingBox(vista);

                // Obtiene el volumen tridimensional de la etiqueta temporal
                double xEtiMedio = (bbEtiqueta.Max.X - bbEtiqueta.Min.X);
                double yEtiMedio = (bbEtiqueta.Max.Y - bbEtiqueta.Min.Y);

                // Obtiene el punto del pliegue
                double xPliegue = puntoPliegueTemp.X - xEtiMedio * ObtenerSignoComponenteDeVector(vista.RightDirection.X);
                double yPliegue = puntoPliegueTemp.Y - yEtiMedio * ObtenerSignoComponenteDeVector(vista.RightDirection.Y);

                // Reemplaza las nuevas coordenadas del pliegue
                puntoPliegue = new XYZ(xPliegue, yPliegue, puntoPliegueTemp.Z);

                // Obtiene el punto final
                double xFin = puntoFinTemp.X - xEtiMedio * ObtenerSignoComponenteDeVector(vista.RightDirection.X);
                double yFin = puntoFinTemp.Y - yEtiMedio * ObtenerSignoComponenteDeVector(vista.RightDirection.Y);

                // Reemplaza las nuevas coordenadas después del pliegue
                puntoFin = new XYZ(xFin, yFin, puntoFinTemp.Z);

                subT.RollBack();
            }

            // Crea la etiqueta
            SpotDimension etiquetaProfundidad = doc.Create.NewSpotElevation
                                                (vista, referencia, puntoOrigen, puntoPliegue, puntoFin, puntoOrigen, true);

            // Cambia el estilo de etiqueta
            etiquetaProfundidad.DimensionType = tipoCotaProfundidad;

            return etiquetaProfundidad;
        }

        ///<summary> Crea una cota de profundidad abajo a la derecha </summary>
        public static SpotDimension CrearCotaProfundidadAbajoDerecha(Document doc, View vista, Element elem, SpotDimensionType tipoCotaProfundidad)
        {
            // Obtiene la referencia del elemento
            Reference referencia = ReferenciaCaraExtremaElementoEnVista(vista, -vista.UpDirection, elem);

            // Crea la bandera de la geometría
            bool banderaGeometria = false;

            // Obtiene el punto medio de la cara referenciada
            XYZ puntoMedioCara = ObtenerPuntoMedioCaraParaCotaProfundidad(doc, elem, referencia, ref banderaGeometria);

            // Coordenadas del origen de la etiqueta
            XYZ puntoOrigen = puntoMedioCara;

            // Obtiene el cuadro de la vista
            BoundingBoxXYZ bbox = vista.get_BoundingBox(null);

            // Obtiene la transformación de la vista
            Transform tra = bbox.Transform;

            // Transforma las coordenadas máximas de la vista a coordenadas del proyecto
            XYZ vistaMax = tra.OfPoint(bbox.Max);

            // Coordenadas del pliegue de la etiqueta abajo a la derecha
            XYZ puntoPliegueTemp = new XYZ(vistaMax.X, vistaMax.Y, puntoMedioCara.Z);

            // Coordenadas después del pliegue
            XYZ puntoFinTemp = new XYZ(vistaMax.X, vistaMax.Y, puntoMedioCara.Z);

            // Coordenadas finales del pliegue
            XYZ puntoPliegue = new XYZ();

            // Coordenadas finales después del pliegue
            XYZ puntoFin = new XYZ();

            // Abre una subtransacción
            using (SubTransaction subT = new SubTransaction(doc))
            {
                subT.Start();

                // Crea la etiqueta temporal
                SpotDimension etiquetaTemporal = doc.Create.NewSpotElevation
                                                    (vista, referencia, puntoOrigen, puntoPliegueTemp, puntoFinTemp, puntoOrigen, false);

                // Crea la caja que contiene a la etiqueta temporal
                BoundingBoxXYZ bbEtiqueta = etiquetaTemporal.get_BoundingBox(vista);

                // Obtiene el volumen tridimensional de la etiqueta temporal
                double xEtiMedio = (bbEtiqueta.Max.X - bbEtiqueta.Min.X) / 4;
                double yEtiMedio = (bbEtiqueta.Max.Y - bbEtiqueta.Min.Y) / 4;

                // Obtiene el punto del pliegue
                double xPliegue = puntoPliegueTemp.X + xEtiMedio * ObtenerSignoComponenteDeVector(vista.RightDirection.X);
                double yPliegue = puntoPliegueTemp.Y + yEtiMedio * ObtenerSignoComponenteDeVector(vista.RightDirection.Y);

                // Reemplaza las nuevas coordenadas del pliegue
                puntoPliegue = new XYZ(xPliegue, yPliegue, puntoPliegueTemp.Z);

                // Obtiene el punto final
                double xFin = puntoFinTemp.X + xEtiMedio * ObtenerSignoComponenteDeVector(vista.RightDirection.X);
                double yFin = puntoFinTemp.Y + yEtiMedio * ObtenerSignoComponenteDeVector(vista.RightDirection.Y);

                // Reemplaza las nuevas coordenadas después del pliegue
                puntoFin = new XYZ(xFin, yFin, puntoFinTemp.Z);

                subT.RollBack();
            }

            // Crea la etiqueta
            SpotDimension etiquetaProfundidad = doc.Create.NewSpotElevation
                                                (vista, referencia, puntoOrigen, puntoPliegue, puntoFin, puntoOrigen, true);

            // Cambia el estilo de etiqueta
            etiquetaProfundidad.DimensionType = tipoCotaProfundidad;

            return etiquetaProfundidad;
        }

        ///<summary> Obtiene el signo del componente de un vector </summary>
        public static int ObtenerSignoComponenteDeVector(double componente)
        {            
            // Verifica si es cero
            if (componente == 0)
            {
                return 0;
            }

            // No es cero
            else
            {
                // Verifica que el valor sea mayor a cero
                if (componente > 0)
                {
                    return 1;
                }

                // Es menor a cero
                else
                {
                    return -1;
                }
            }
        }

        ///<summary> Verifica si el vector que mueve la cota de profundiad es mayor que el ancho del elemento </summary>
        public static XYZ VerificarAnchoDeElementoParaMoverCotaProfundidad(View vista, XYZ vector)
        {
            // Obtiene la transformada de la vista
            Transform tra = vista.CropBox.Transform;

            // Obtiene el vector en coordenadas de la vista
            XYZ vectorTransformado = tra.Inverse.OfVector(vector);

            // Crea los componentes del vector
            double x = vectorTransformado.X;
            double y = vectorTransformado.Y;
            double z = vectorTransformado.Z;

            // Obtiene el volumen tridimensional de la vista
            double xVista = (vista.CropBox.Max.X - vista.CropBox.Min.X) / 2;
            double yVista = (vista.CropBox.Max.Y - vista.CropBox.Min.Y) / 2;
            double zVista = (vista.CropBox.Max.Z - vista.CropBox.Min.Z) / 2;
            
            // Verifica si el vector es mayor al volumen 3D de la vista
            if (Math.Abs(x) > Math.Abs(xVista))
            {
                x = 0;
            }

            // Verifica si el vector es mayor al volumen 3D de la vista
            if (Math.Abs(y) > Math.Abs(yVista))
            {
                y = 0;
            }

            // Verifica si el vector es mayor al volumen 3D de la vista
            if (Math.Abs(z) > Math.Abs(zVista))
            {
                z = 0;
            }

            // Crea el vector que mueve la cota profundidad y lo lleva a coordenadas globales
            XYZ distancia = tra.OfVector(new XYZ(x, y, z));
            
            return distancia;
        }
        
        #endregion

        #region Referencias 

        ///<summary> Obtiene un string de la referencia del plano más alejado de un elemento en una dirección dada </summary>
        public static Reference ReferenciaCaraExtremaElementoEnVista(View vista, XYZ direccion, Element elem)
        {
            // Crea el string de la referencia a devolver
            Reference referencia = null;

            // Crea la bandera de la geometría
            bool banderaGeometria = false;

            // Crea la variable distancia
            double distancia = double.MinValue;
            
            // Agrega los solidos a la lista
            List<Solid> solidos = ObtenerGeometriaSolidosDeElemento(elem, ref banderaGeometria);

            // Verifica si la geometría se recupera de GetSymbolGeometry
            if (banderaGeometria)
            {
                // Modifica la dirección a coordenadas de la familia
                direccion = TransformarDireccionDeVista(elem, direccion);
            }

            // Recorre todos los solidos de la geometría
            foreach (Solid solido in solidos.Where(x => x != null && x.Volume > 0))
            {
                // Recorre todas las caras del solido
                foreach (Face cara in solido.Faces)
                {
                    // Obtiene las coordenadas 3D de la cara en un punto dado
                    XYZ puntoMedio = ObtenerPuntoMedioCara(cara);

                    // Obtiene el vector normal de la cara en un punto dado
                    XYZ normal = ObtenerVectorNormalCara(cara);

                    // Verifica que el vector sea paralelo a la dirección
                    if (normal.CrossProduct(direccion).IsZeroLength() && normal.IsAlmostEqualTo(direccion))
                    {
                        // Distancia del punto medio de la cara al baricentro del elemento en el sentido del vector dirección de la vista
                        double distanciaACara = DistanciaBaricentroElementoACara(vista, direccion, elem, cara, ref banderaGeometria);

                        // Verifica que la distancia sea mayor
                        if (distanciaACara > distancia)
                        {
                            // Asigna la nueva distancia
                            distancia = distanciaACara;

                            // Asigna la referencia de la cara
                            referencia = cara.Reference;
                        }
                    }
                }
            }

            // Vuelve al estado original la bandera de geometría
            banderaGeometria = false;

            return referencia;
        }

        ///<summary> Obtiene la geometría del elemento en detalle fino</summary>
        public static List<Solid> ObtenerGeometriaSolidosDeElemento(Element elem, ref bool banderaGeometria)
        {
            // Crea la lista a devolver
            List<Solid> solidos = new List<Solid>();

            // Crea las preferencias para analizar la geometría
            Options opcion = new Options();

            // Activa el cálculo de referencias a objetos
            opcion.ComputeReferences = true;

            // Asigna el nivel de detalle alto para recuperar la geometría 
            opcion.DetailLevel = ViewDetailLevel.Fine;

            // Crea una representación geométrica del elemento
            GeometryElement geoElem = elem.get_Geometry(opcion);

            // Agrega los solidos a la lista
            solidos.AddRange(AgregarSolido(geoElem, ref banderaGeometria));

            return solidos;
        }

        ///<summary> Agrega todos los solidos de un GeometryElement </summary>
        public static List<Solid> AgregarSolido(GeometryElement geoElem, ref bool banderaGeometria)
        {
            // Crea una bandera
            bool bandera = false;
            
            // Crea la lista a devolver
            List <Solid> solidos = new List<Solid>();

            // Recorre todos las geometrías primitivas
            foreach (GeometryObject geoObje in geoElem)
            {
                // Verifica que sea solido
                if (geoObje is Solid)
                {
                    // Castea a solido
                    Solid solido = geoObje as Solid;

                    // Agrega a la lista
                    solidos.Add(solido);

                    // Cambia el estado de la bandera
                    bandera = true;
                }
            }

            // Verifica si existió algún solido 
            if (!bandera)
            {
                // Recorre todos las geometrías primitivas
                foreach (GeometryObject geoObje in geoElem)
                {
                    // Verifica que sea GeometryInstance
                    if (geoObje is GeometryInstance)
                    {
                        // Castea a solido
                        GeometryInstance geoInst = geoObje as GeometryInstance;

                        // Obtiene la geometría del simbolo
                        GeometryElement geoElemSymb = geoInst.GetSymbolGeometry();

                        // Cambia el estado de la bandera general
                        banderaGeometria = true;

                        // Agrega los solidos a la lista
                        solidos.AddRange(AgregarSolido(geoElemSymb, ref banderaGeometria));
                    }
                }
            }

            return solidos;
        }

        ///<summary> Transforma la dirección de una vista del proyecto al sistema de coordenadas local de la familia </summary>
        public static XYZ TransformarDireccionDeVista(Element elem, XYZ direccion)
        {
            // Castea el elemento a FamilyInstance
            FamilyInstance fi = elem as FamilyInstance;

            // Obtiene la transformación del elemento
            Transform transformada = fi.GetTotalTransform();

            // Modifica el origen de la transformación
            transformada.Origin = XYZ.Zero;

            // Obtiene el vector unitario que apunta en dirección de la vista
            XYZ direccionModificada = transformada.Inverse.OfVector(direccion.Normalize());

            return direccionModificada;
        }

        ///<summary> Obtiene la distancia de la cara al baricentro del elemento en la proyección de un vector dado sobre una vista </summary>
        public static double DistanciaBaricentroElementoACara(View vista, XYZ direccion, Element elem, Face cara, ref bool banderaGeometria)
        {
            // Crea la variable
            double distanciaAbsoluta;

            // Obtiene el punto medio de la cara
            XYZ puntoMedioCara = ObtenerPuntoMedioCara(cara);

            // Verifica si la geometría se recupera de GetSymbolGeometry
            if (banderaGeometria)
            {            
                // Castea el elemento a FamilyInstance
                FamilyInstance fi = elem as FamilyInstance;

                // Obtiene la transformación del elemento
                Transform transformada = fi.GetTotalTransform();

                // Verifica que sea basado en curva
                if (elem.Location is LocationCurve)
                {
                    // Lleva la coordenada local a la del proyecto
                    puntoMedioCara = transformada.OfPoint(transformada.Origin);
                }

                // Verifica que sea basado en punto
                if (elem.Location is LocationPoint)
                {
                    // Lleva la coordenada local a la del proyecto
                    puntoMedioCara = puntoMedioCara.Add(transformada.Origin);
                }
            }

            // Obtiene el volumen en 3D del elemento
            BoundingBoxXYZ bb = elem.get_BoundingBox(vista);

            // Obtiene el baricentro del elemento en 3D
            XYZ baricentroElemento = ObtenerBaricentroElemento(bb);

            // Distancia entre el punto medio de la cara y el baricentro del elemento
            XYZ vectorCaraABaricentro = puntoMedioCara - baricentroElemento;
            
            // Obtiene la proyección de la distancia en el sentido del vector
            double distancia = vectorCaraABaricentro.DotProduct(direccion) / direccion.GetLength();

            // Obtiene el valor absoluto de la distancia
            distanciaAbsoluta = Math.Abs(distancia);

            return distanciaAbsoluta;
        }

        ///<summary> Obtiene el baricentro de un elemento </summary>
        public static XYZ ObtenerBaricentroElemento(BoundingBoxXYZ bb)
        {
            // Crea el punto
            XYZ baricentroElemento = new XYZ();

            // Obtiene las coordenadas del baricentro del elemento
            double xMed = bb.Min.X + (bb.Max.X - bb.Min.X) / 2;
            double yMed = bb.Min.Y + (bb.Max.Y - bb.Min.Y) / 2;
            double zMed = bb.Min.Z + (bb.Max.Z - bb.Min.Z) / 2;

            // Asigna el baricentro del elemento en 3D
            baricentroElemento = new XYZ(xMed, yMed, zMed);

            return baricentroElemento;
        }

        ///<summary> Obtiene el punto medio de una cara </summary>
        public static XYZ ObtenerPuntoMedioCara(Face cara)
        {
            // Crea una caja bidimensional
            BoundingBoxUV bb = cara.GetBoundingBox();

            // Crea las coordenadas de un objeto en un espacio 2D
            UV minUV = bb.Min;
            UV maxUV = bb.Max;

            // Obtiene el punto medio de la caja bidimensional
            UV medioUV = minUV + (maxUV - minUV) / 2;

            // Obtiene las coordenadas 3D de la cara en un punto dado
            XYZ puntoMedio = cara.Evaluate(medioUV);

            return puntoMedio;
        }

        ///<summary> Obtiene la normal en el punto medio de una cara </summary>
        public static XYZ ObtenerVectorNormalCara(Face cara)
        {
            // Crea una caja bidimensional
            BoundingBoxUV bb = cara.GetBoundingBox();

            // Crea las coordenadas de un objeto en un espacio 2D
            UV minUV = bb.Min;
            UV maxUV = bb.Max;

            // Obtiene el punto medio de la caja bidimensional
            UV medioUV = minUV + 0.5 * (maxUV - minUV);

            // Obtiene el vector normal de la cara en un punto dado
            XYZ normal = cara.ComputeNormal(medioUV);

            return normal;
        }

        ///<summary> Obtiene el punto medio de una cara para la cota de profundidad </summary>
        public static XYZ ObtenerPuntoMedioCaraParaCotaProfundidad(Document doc, Element elem, Reference referencia, ref bool banderaGeometria)
        {
            // Crea el punto medio de la cara
            XYZ puntoMedioCara = new XYZ();

            // Obtiene el string de la referencia de la cara que se busca
            string caraReferencia = referencia.ConvertToStableRepresentation(doc);

            // Obtiene todos los solidos del elemento
            List<Solid> listaSolidos = ObtenerGeometriaSolidosDeElemento(elem, ref banderaGeometria);

            // Recorre todos los solidos
            foreach (Solid solido in listaSolidos.Where(x => x != null && x.Volume > 0))
            {
                // Recorre todas las caras del solido
                foreach (Face cara in solido.Faces)
                {
                    // Obtiene la referencia de la cara
                    Reference r = cara.Reference;

                    // Compara el string de la cara con la cara buscada
                    if (r.ConvertToStableRepresentation(doc) == caraReferencia)
                    {
                        // Verifica si la geometría se obtuvo de la familia
                        if (banderaGeometria)
                        {
                            // Castea el elemento a FamilyInstance
                            FamilyInstance fi = elem as FamilyInstance;

                            // Obtiene la transformación del elemento
                            Transform transformacion = fi.GetTotalTransform();

                            // Obtiene la coordenada del punto medio de la cara
                            XYZ medioCara = ObtenerPuntoMedioCara(cara);

                            // Asigna la nueva coordenada de la cara
                            puntoMedioCara = transformacion.OfPoint(medioCara);
                        }

                        else
                        {
                            // Asigna la nueva coordenada de la cara
                            puntoMedioCara = ObtenerPuntoMedioCara(cara);
                        }
                    }
                }
            }

            // Vuelve al estado original la bandera de geometría
            banderaGeometria = false;

            return puntoMedioCara;
        }

        #endregion

        #region Áreas y Volumenes

        ///<summary> Obtiene el área del material de un elemento </summary>
        public static double ObtenerAreaMaterial(Element elem)
        {
            // Crea la variable 
            double area = 0;
            bool b = false;

            // Obtiene el ID del elemento
            ElementId elemId = elem.Id;

            // Obtiene el área
            area = elem.GetMaterialArea(elemId, b);

            return area;
        }

        ///<summary> Obtiene una lista con el área del material de cada elemento </summary>
        public static List<double> ObtenerAreaMaterial(List<Element> lista)
        {
            // Crea la variable 
            List<double> area = new List<double>();
            bool b = false;

            // Obtiene el área
            foreach (Element elem in lista)
            {
                // Obtiene el ID del elemento actual
                ElementId elemId = elem.Id;

                // Obtiene el área del elemento actual
                area.Add(elem.GetMaterialArea(elemId, b));
            }

            return area;
        }

        ///<summary> Obtiene el volumen del material de un elemento </summary>
        public static double ObtenerVolumenMaterial(Element elem)
        {
            // Crea la variable 
            double volumen = 0;

            // Obtiene el ID del elemento
            ElementId elemId = elem.Id;

            // Obtiene el volumen
            volumen = elem.GetMaterialVolume(elemId);

            return volumen;
        }

        ///<summary> Obtiene una lista con el volumen del material de cada elemento </summary>
        public static List<double> ObtenerVolumenMaterial(List<Element> lista)
        {
            // Crea la variable 
            List<double> volumen = new List<double>();

            // Obtiene el volumen
            foreach (Element elem in lista)
            {
                // Obtiene el ID del elemento actual
                ElementId elemId = elem.Id;

                // Obtiene el volumen del elemento actual
                volumen.Add(elem.GetMaterialVolume(elemId));
            }

            return volumen;
        }

        #endregion

        #region Sección Longitudinal y Transversal

        ///<summary> Crea una sección XX de un elemento </summary>
        public static View VistaXX(Document doc, Element elem)
        {
            // Crea la vista a devolver
            View vistaXX = null;

            // Verifica que la vista sea Global
            if (Jump.Properties.Settings.Default.rbtnGeneralVistaGlobal == true)
            {
                // Verifica que sea basado en punto
                if (elem.Location is LocationPoint)
                {
                    // Crea una sección transversal del elemento
                    vistaXX = Tools.SeccionXXGlobalBasadoEnPunto(doc, elem);
                }

                // Verifica que sea basado en linea
                else
                {
                    vistaXX = Tools.SeccionLongitudinalBasadoEnCurva(doc, elem);
                }
            }

            // Verifica que la vista sea Local
            else
            {
                // Verifica que sea basado en punto
                if (elem.Location is LocationPoint)
                {
                    // Crea una sección transversal del elemento
                    vistaXX = Tools.SeccionXXLocalBasadoEnPunto(doc, elem);
                }

                // Verifica que sea basado en linea
                else
                {
                    vistaXX = Tools.SeccionLongitudinalBasadoEnCurva(doc, elem);
                }
            }

            return vistaXX;
        }

        ///<summary> Crea una sección YY de un elemento </summary>
        public static View VistaYY(Document doc, Element elem)
        {
            // Crea la vista a devolver
            View vistaYY = null;

            // Verifica que la vista sea Global
            if (Jump.Properties.Settings.Default.rbtnGeneralVistaGlobal == true)
            {
                // Verifica que sea basado en punto
                if (elem.Location is LocationPoint)
                {
                    // Crea una sección transversal del elemento
                    vistaYY = Tools.SeccionYYGlobalBasadoEnPunto(doc, elem);
                }

                // Verifica que sea basado en linea
                else
                {
                    vistaYY = Tools.SeccionTransversalBasadoEnCurva(doc, elem);
                }
            }

            // Verifica que la vista sea Local
            else
            {
                // Verifica que sea basado en punto
                if (elem.Location is LocationPoint)
                {
                    // Crea una sección transversal del elemento
                    vistaYY = Tools.SeccionYYLocalBasadoEnPunto(doc, elem);
                }

                // Verifica que sea basado en linea
                else
                {
                    vistaYY = Tools.SeccionTransversalBasadoEnCurva(doc, elem);
                }
            }
            return vistaYY;
        }

        ///<summary> Crea una sección longitudinal de un elemento basado en curva o línea </summary>
        public static View SeccionLongitudinalBasadoEnCurva(Document doc, Element elem)
        {
            try
            {
                // Obtiene al FamilyInstance
                FamilyInstance fi = elem as FamilyInstance;

                // Obtiene el Family
                FamilySymbol fs = fi.Symbol;

                // Toma la ubicación del elemento basado en curva
                LocationCurve lc = elem.Location as LocationCurve;

                // Obtiene la longitud de la linea
                Curve curva = lc.Curve;

                // Obtiene los puntos iniciales y finales se la sección
                XYZ inicial = curva.GetEndPoint(0);
                XYZ final = curva.GetEndPoint(1);

                // Obtiene la longitud de la linea
                XYZ longitud = inicial - final;

                // Determina la vista que se usa
                ViewFamilyType vft = new FilteredElementCollector(doc)
                                        .OfClass(typeof(ViewFamilyType))
                                        .Cast<ViewFamilyType>()
                                        .FirstOrDefault<ViewFamilyType>(v => ViewFamily.Section == v.ViewFamily);

                // Obtiene la transformación de la curva
                Transform curvaTransformada = curva.ComputeDerivatives(corteTransversalBasadoLinea, true);

                // Crea los vectores de dirección de la vista
                XYZ direccion = longitud.Normalize();
                XYZ arriba = XYZ.BasisZ;
                XYZ viewDir = direccion.CrossProduct(arriba);

                // Crea la transformación
                Transform tra = Transform.Identity;

                // Establece el origen de la transformación
                tra.Origin = curvaTransformada.Origin;

                // Determina la dirección de la vista
                tra.BasisX = direccion;
                tra.BasisY = arriba;
                tra.BasisZ = viewDir;

                // Crea la caja de sección
                BoundingBoxXYZ cajaSeccion = new BoundingBoxXYZ();

                // Cambia la transformacion de la caja de sección
                cajaSeccion.Transform = tra;

                // Crea la caja de sección
                BoundingBoxXYZ bbelem = elem.get_BoundingBox(null);
                BoundingBoxXYZ bb = fs.get_BoundingBox(null);

                // Obtiene el volumen tridimensional del elemento
                double x = (curva.Length) / 2;
                double y = (bb.Max.Y - bb.Min.Y) / 2;
                double z = (bb.Max.Z - bb.Min.Z);

                // Obtiene la cota en la parte superior e inferior
                double cotaSuperior = CotaElevacionParteSuperior(elem);
                double cotaInferior = CotaElevacionParteInferior(elem);

                // Crea la cota
                double cota = 0;

                // Verifica que las dos cotas sean negativas
                if (cotaSuperior < 0 && cotaInferior < 0)
                {
                    // Asigna la cota inferior
                    cota = cotaInferior;
                }

                // Obtiene las coordenadas máximas y mínimas
                XYZ min = new XYZ(-x, bbelem.Min.Z - cota, -y);
                XYZ max = new XYZ(x, bbelem.Max.Z - cota, y);

                // Asigna los valores a la caja de sección
                cajaSeccion.Min = min;
                cajaSeccion.Max = max;

                // Crear la sección del elemento
                View seccion = ViewSection.CreateSection(doc, vft.Id, cajaSeccion) as View;

                return seccion;
            }

            catch (Exception)
            {
                return null;
            }
        }

        ///<summary> Crea una sección transversal de un elemento basado en curva o línea </summary>
        public static View SeccionTransversalBasadoEnCurva(Document doc, Element elem)
        {
            try
            {
                // Obtiene al FamilyInstance
                FamilyInstance fi = elem as FamilyInstance;

                // Obtiene el Family
                FamilySymbol fs = fi.Symbol;

                // Toma la ubicación del elemento basado en curva
                LocationCurve lc = elem.Location as LocationCurve;

                // Obtiene la longitud de la linea
                Curve curva = lc.Curve;

                // Obtiene los puntos iniciales y finales se la sección
                XYZ inicial = curva.GetEndPoint(0);
                XYZ final = curva.GetEndPoint(1);

                // Obtiene la longitud de la linea
                XYZ longitud = final - inicial;

                // Determina la vista que se usa
                ViewFamilyType vft = new FilteredElementCollector(doc)
                                        .OfClass(typeof(ViewFamilyType))
                                        .Cast<ViewFamilyType>()
                                        .FirstOrDefault<ViewFamilyType>(v => ViewFamily.Section == v.ViewFamily);

                // Obtiene la transformación de la curva
                Transform curvaTransformada = curva.ComputeDerivatives(corteTransversalBasadoLinea, true);

                // Crea los vectores de dirección de la vista
                XYZ direccion = longitud.Normalize();
                XYZ arriba = XYZ.BasisZ;
                XYZ viewDir = arriba.CrossProduct(direccion);

                // Crea la transformación
                Transform tra = Transform.Identity;

                // Establece el origen de la transformación
                tra.Origin = curvaTransformada.Origin;

                // Determina la dirección de la vista
                tra.BasisX = viewDir;
                tra.BasisY = arriba;
                tra.BasisZ = direccion;

                // Crea la caja de sección
                BoundingBoxXYZ cajaSeccion = new BoundingBoxXYZ();

                // Cambia la transformacion de la caja de sección
                cajaSeccion.Transform = tra;

                // Crea la caja de sección
                BoundingBoxXYZ bbelem = elem.get_BoundingBox(null);
                BoundingBoxXYZ bb = fs.get_BoundingBox(null);

                // Obtiene el volumen tridimensional del elemento
                double x = (curva.Length) / 2;
                double y = (bb.Max.Y - bb.Min.Y) / 2;
                double z = (bb.Max.Z - bb.Min.Z);

                // Obtiene la cota en la parte superior e inferior
                double cotaSuperior = CotaElevacionParteSuperior(elem);
                double cotaInferior = CotaElevacionParteInferior(elem);

                // Crea la cota
                double cota = 0;

                // Verifica que las dos cotas sean negativas
                if (cotaSuperior < 0 && cotaInferior < 0)
                {
                    // Asigna la cota inferior
                    cota = cotaInferior;
                }

                // Obtiene las coordenadas máximas y mínimas
                XYZ min = new XYZ(-y, bbelem.Min.Z - cota, -x);
                XYZ max = new XYZ(y, bbelem.Max.Z - cota, x);

                // Asigna los valores a la caja de sección
                cajaSeccion.Min = min;
                cajaSeccion.Max = max;
                
                // Crear la sección del elemento
                View seccion = ViewSection.CreateSection(doc, vft.Id, cajaSeccion) as View;
                
                return seccion;
            }

            catch (Exception)
            {
                return null;
            }
        }

        ///<summary> Crea una sección en sentido X-X Global de un elemento basado en un punto de ubicación </summary>
        public static View SeccionXXGlobalBasadoEnPunto(Document doc, Element elem)
        {
            try
            {
                // Determina la vista que se usa
                ViewFamilyType vft = new FilteredElementCollector(doc)
                                        .OfClass(typeof(ViewFamilyType))
                                        .Cast<ViewFamilyType>()
                                        .FirstOrDefault<ViewFamilyType>(v => ViewFamily.Section == v.ViewFamily);

                // Crea la caja que contiene al elemento
                BoundingBoxXYZ bb = elem.get_BoundingBox(null);

                // Obtiene el volumen tridimensional del elemento
                double x = (bb.Max.X - bb.Min.X);
                double y = (bb.Max.Y - bb.Min.Y);
                double z = (bb.Max.Z - bb.Min.Z);

                // Crea las coordenadas XYZ para el volumen final de la sección
                XYZ MaxPt = new XYZ(x, z, y) / 2;
                XYZ MinPt = new XYZ(-x, -z, -y) / 2;

                // Crea la caja que contiene al elemento
                BoundingBoxXYZ bbox = new BoundingBoxXYZ();
                
                // Habilita la caja
                bbox.Enabled = true;

                // Asigna las coordenadas a la caja
                bbox.Max = MaxPt;
                bbox.Min = MinPt;

                // Encuentra el punto medio del elemento
                XYZ puntomedio = 0.5 * (bb.Max + bb.Min);

                // Crea la transformación
                Transform tra = Transform.Identity;

                // Establece el origen de la transformación
                tra.Origin = puntomedio;
                
                // Determina la dirección de la vista
                tra.BasisX = -XYZ.BasisX;
                tra.BasisY = XYZ.BasisZ;
                tra.BasisZ = XYZ.BasisY;

                // Asigna la transformación a la caja
                bbox.Transform = tra;

                // Crear la sección del elemento
                View seccion = ViewSection.CreateSection(doc, vft.Id, bbox) as View;

                return seccion;
            }

            catch (Exception)
            {
                return null;
            }
        }

        ///<summary> Crea una sección en sentido Y-Y Global de un elemento basado en un punto de ubicación </summary>
        public static View SeccionYYGlobalBasadoEnPunto(Document doc, Element elem)
        {
            try
            {
                // Determina la vista que se usa
                ViewFamilyType vft = new FilteredElementCollector(doc)
                                        .OfClass(typeof(ViewFamilyType))
                                        .Cast<ViewFamilyType>()
                                        .FirstOrDefault<ViewFamilyType>(v => ViewFamily.Section == v.ViewFamily);

                // Crea la caja que contiene al elemento
                BoundingBoxXYZ bb = elem.get_BoundingBox(null);

                // Obtiene el volumen tridimensional del elemento
                double x = (bb.Max.X - bb.Min.X);
                double y = (bb.Max.Y - bb.Min.Y);
                double z = (bb.Max.Z - bb.Min.Z);

                // Crea las coordenadas XYZ para el volumen final de la sección
                XYZ MaxPt = new XYZ(y, z, x) / 2;
                XYZ MinPt = new XYZ(-y, -z, -x) / 2;

                // Crea la caja que contiene al elemento
                BoundingBoxXYZ bbox = new BoundingBoxXYZ();

                // Habilita la caja
                bbox.Enabled = true;

                // Asigna las coordenadas a la caja
                bbox.Max = MaxPt;
                bbox.Min = MinPt;

                // Encuentra el punto medio del elemento
                XYZ puntomedio = 0.5 * (bb.Max + bb.Min);

                // Crea la transformación
                Transform tra = Transform.Identity;

                // Establece el origen de la transformación
                tra.Origin = puntomedio;

                // Determina la dirección de la vista
                tra.BasisX = -XYZ.BasisY;
                tra.BasisY = XYZ.BasisZ;
                tra.BasisZ = -XYZ.BasisX;

                // Asigna la transformación a la caja
                bbox.Transform = tra;

                // Crear la sección del elemento
                View seccion = ViewSection.CreateSection(doc, vft.Id, bbox) as View;

                return seccion;
            }

            catch (Exception)
            {
                return null;
            }
        }

        ///<summary> Crea una sección en sentido X-X Local de un elemento basado en un punto de ubicación </summary>
        public static View SeccionXXLocalBasadoEnPunto(Document doc, Element elem)
        {
            try
            {
                // Determina la vista que se usa
                ViewFamilyType vft = new FilteredElementCollector(doc)
                                        .OfClass(typeof(ViewFamilyType))
                                        .Cast<ViewFamilyType>()
                                        .FirstOrDefault<ViewFamilyType>(v => ViewFamily.Section == v.ViewFamily);

                // Obtiene al FamilyInstance
                FamilyInstance fi = elem as FamilyInstance;

                // Obtiene el Family
                FamilySymbol fs = fi.Symbol;

                // Obtiene el LocationPoint
                LocationPoint lp = fi.Location as LocationPoint;

                // Obtiene la transformación de las coordenadas de la familia al proyecto
                Transform transformacion = Transform.CreateRotation(XYZ.BasisZ, lp.Rotation);

                // Crea la caja que contiene al elemento
                BoundingBoxXYZ bbelem = elem.get_BoundingBox(null);
                BoundingBoxXYZ bb = fs.get_BoundingBox(null);

                // Obtiene el volumen tridimensional del elemento
                double x = (bb.Max.X - bb.Min.X);
                double y = (bb.Max.Y - bb.Min.Y);
                double z = (bb.Max.Z - bb.Min.Z);

                // Crea las coordenadas XYZ para el volumen final de la sección
                XYZ MaxPt = new XYZ(x, z, y) / 2;
                XYZ MinPt = new XYZ(-x, -z, -y) / 2;
                
                // Crea la caja que contiene al elemento
                BoundingBoxXYZ bbox = new BoundingBoxXYZ();

                // Habilita la caja
                bbox.Enabled = true;

                // Asigna las coordenadas a la caja
                bbox.Max = MaxPt;
                bbox.Min = MinPt;

                // Encuentra el punto medio del elemento
                XYZ puntomedio = 0.5 * (bbelem.Max + bbelem.Min);

                // Crea la transformación
                Transform tra = Transform.Identity;

                // Establece el origen de la transformación
                tra.Origin = puntomedio;

                // Determina la dirección de la vista
                tra.BasisX = transformacion.OfVector(-XYZ.BasisX);
                tra.BasisY = XYZ.BasisZ;
                tra.BasisZ = transformacion.OfVector(XYZ.BasisY);

                // Asigna la transformación a la caja
                bbox.Transform = tra;
                
                // Crear la sección del elemento
                View seccion = ViewSection.CreateSection(doc, vft.Id, bbox) as View;

                return seccion;
            }

            catch (Exception)
            {
                return null;
            }
        }

        ///<summary> Crea una sección en sentido Y-Y Global de un elemento basado en un punto de ubicación </summary>
        public static View SeccionYYLocalBasadoEnPunto(Document doc, Element elem)
        {
            try
            {
                // Determina la vista que se usa
                ViewFamilyType vft = new FilteredElementCollector(doc)
                                        .OfClass(typeof(ViewFamilyType))
                                        .Cast<ViewFamilyType>()
                                        .FirstOrDefault<ViewFamilyType>(v => ViewFamily.Section == v.ViewFamily);

                // Obtiene al FamilyInstance
                FamilyInstance fi = elem as FamilyInstance;

                // Obtiene el Family
                FamilySymbol fs = fi.Symbol;

                // Obtiene el LocationPoint
                LocationPoint lp = fi.Location as LocationPoint;

                // Obtiene la transformación de las coordenadas de la familia al proyecto
                Transform transformacion = Transform.CreateRotation(XYZ.BasisZ, lp.Rotation);

                // Crea la caja que contiene al elemento
                BoundingBoxXYZ bbelem = elem.get_BoundingBox(null);
                BoundingBoxXYZ bb = fs.get_BoundingBox(null);

                // Obtiene el volumen tridimensional del elemento
                double x = (bb.Max.X - bb.Min.X);
                double y = (bb.Max.Y - bb.Min.Y);
                double z = (bb.Max.Z - bb.Min.Z);

                // Crea las coordenadas XYZ para el volumen final de la sección
                XYZ MaxPt = new XYZ(y, z, x) / 2;
                XYZ MinPt = new XYZ(-y, -z, -x) / 2;

                // Crea la caja que contiene al elemento
                BoundingBoxXYZ bbox = new BoundingBoxXYZ();

                // Habilita la caja
                bbox.Enabled = true;

                // Asigna las coordenadas a la caja
                bbox.Max = MaxPt;
                bbox.Min = MinPt;

                // Encuentra el punto medio del elemento
                XYZ puntomedio = 0.5 * (bbelem.Max + bbelem.Min);

                // Crea la transformación
                Transform tra = Transform.Identity;

                // Establece el origen de la transformación
                tra.Origin = puntomedio;

                // Determina la dirección de la vista
                tra.BasisX = transformacion.OfVector(-XYZ.BasisY);
                tra.BasisY = XYZ.BasisZ;
                tra.BasisZ = transformacion.OfVector(-XYZ.BasisX);

                // Asigna la transformación a la caja
                bbox.Transform = tra;

                // Crear la sección del elemento
                View seccion = ViewSection.CreateSection(doc, vft.Id, bbox) as View;

                return seccion;
            }

            catch (Exception)
            {
                return null;
            }
        }

        ///<summary> Obtiene la cota de elevación en la parte superior del elemento </summary>
        public static double CotaElevacionParteSuperior(Element elem)
        {
            // Crea la cota
            double cota = 0;

            // Obtiene el parámetro
            Parameter param = elem.get_Parameter(BuiltInParameter.STRUCTURAL_ELEVATION_AT_TOP);

            // Verifica que el parámetro no sea nulo
            if (param != null)
            {
                // Asigna el valor del parámetro a la cota
                cota = param.AsDouble();
            }

            return cota;
        }

        ///<summary> Obtiene la cota de elevación en la parte inferior del elemento </summary>
        public static double CotaElevacionParteInferior(Element elem)
        {
            // Crea la cota
            double cota = 0;

            // Obtiene el parámetro
            Parameter param = elem.get_Parameter(BuiltInParameter.STRUCTURAL_ELEVATION_AT_BOTTOM);

            // Verifica que el parámetro no sea nulo
            if (param != null)
            {
                // Asigna el valor del parámetro a la cota
                cota = param.AsDouble();
            }

            return cota;
        }

        ///<summary> Cambia las configuraciones de visualización de la vista </summary>
        public static View CambiarConfiguracionVista(System.Windows.Forms.ComboBox comboEscala, Document doc, View vista, ViewDetailLevel nivelDetalle)
        {
            // Crea la variable
            int escala;
            string numeroEscala;

            // Asigna el texto seleccionado del combobox
            numeroEscala = comboEscala.SelectedItem.ToString();

            // Elimina los primeros 4 caracteres
            numeroEscala = numeroEscala.Remove(0, 4);

            // Convierte el texto en número
            escala = Convert.ToInt32(numeroEscala);

            // Cambia la escala de la vista
            vista.Scale = escala;

            // Cambia el nivel de detalle de la vista
            vista.DetailLevel = nivelDetalle;

            // Activa el cuadro de recorte
            vista.CropBoxActive = false;

            // Desactiva la visibilidad del cuadro de recorte
            vista.CropBoxVisible = false;

            // Oculta las categorías
            vista.SetCategoryHidden(Category.GetCategory(doc, BuiltInCategory.OST_Grids).Id, true);
            vista.SetCategoryHidden(Category.GetCategory(doc, BuiltInCategory.OST_Sections).Id, true);

            return vista;
        }
        
        ///<summary> Oculta todo excepto el elemento y sus armaduras en una vista </summary>
        public static void MostrarSolamenteElementoYBarrasEnVista(Document doc, View vista, Element elem)
        {
            // Obtiene todos los elementos de la vista
            List<Element> todosElementos = Tools.ObtenerTodosElementosEnUnaVista(doc, vista);

            // Quita el elemento de la lista
            todosElementos.Remove(elem);

            // Obtiene todas las armaduras del elemento
            List<Rebar> armaduras = Tools.ObtenerArmadurasDeElemento(elem, vista);

            // Crea una lista de los elementos a mostrar
            List<Element> ElementosMostar = new List<Element>();

            // Agrega el elemento a la lista
            ElementosMostar.Add(elem);

            // Crea una lista que contendra los sub elementos
            List<Element> subElementos = ObtenerTodosSubElementos(doc, elem);

            // Agrega los subelementos
            ElementosMostar.AddRange(subElementos);

            // Recorre todas las barras
            foreach (Rebar barra in armaduras)
            {
                // Quita la barra de la lista para ocultar
                todosElementos.Remove(barra);

                // Agrega la barra a la lista para mostrar
                ElementosMostar.Add(barra);
            }

            // Oculta los elementos
            Tools.OcultarElementosVista(doc, vista, todosElementos);

            // Muestra todos los elementos de la lista
            Tools.MostrarElementosVista(doc, vista, ElementosMostar);
        }

        ///<summary> Elimina de la lista los subelementos </summary>
        public static List<Element> EliminarSubelementos(List<Element> lista)
        {
            // Elimina de la lista
            lista.RemoveAll(x => (x as FamilyInstance).SuperComponent != null);

            return lista;
        }

        ///<summary> Obtiene una lista de todos los elementos que se muestran en una vista </summary>
        public static List<Element> ObtenerTodosElementosEnUnaVista(Document doc, View vista)
        {
            // Crea la lista a devolver
            List<Element> lista = new List<Element>();

            // Crea un filtro
            FilteredElementCollector filtro = new FilteredElementCollector(doc, vista.Id);

            // Obtiene los elementos de la vista
            lista = filtro.ToElements().ToList();

            return lista;
        }

        ///<summary> Ajusta el recuadro de una vista con todos los elementos </summary>
        public static View AjustarRecuadroDeVista(Document doc, View vista, List<Element> lista)
        {
            // Obtiene el recuadro de la vista
            BoundingBoxXYZ bbVista = vista.CropBox;

            // Obtiene la transformada inversa de la vista
            Transform traInv = bbVista.Transform.Inverse;

            // Verifica que existan elementos
            if (lista.Count > 0)
            {
                // Obtiene la mínima coordenada
                double xMin = Math.Min(bbVista.Min.X, lista.Min(x => traInv.OfPoint(x.get_BoundingBox(vista).Min).X));
                double yMin = Math.Min(bbVista.Min.Y, lista.Min(x => traInv.OfPoint(x.get_BoundingBox(vista).Min).Y));

                // Obtiene la máxima coordenada
                double xMax = Math.Max(bbVista.Max.X, lista.Max(x => traInv.OfPoint(x.get_BoundingBox(vista).Max).X));
                double yMax = Math.Max(bbVista.Max.Y, lista.Max(x => traInv.OfPoint(x.get_BoundingBox(vista).Max).Y));

                // Crea los puntos
                vista.CropBox.Min = new XYZ(xMin, yMin, bbVista.Min.Z);
                vista.CropBox.Max = new XYZ(xMax, yMax, bbVista.Max.Z);
            }

            return vista;
        }

        #endregion

        #region Ordenar según posición

        ///<summary> Devuelve una lista de elementos según el checkbox seleccionado en un groupbox </summary>
        public static List<Element> OrdenarListaSegunCheckboxEnGroupBox(System.Windows.Forms.GroupBox grupo, List<Element> lista)
        {
            // Crea la lista a devolver
            List<Element> li = new List<Element>();

            // Pasa los radiobutton a una lista
            List<System.Windows.Forms.RadioButton> listaRadioButton = grupo.Controls.OfType<System.Windows.Forms.RadioButton>().ToList();

            // Obtiene en radio button activo
            System.Windows.Forms.RadioButton radio = listaRadioButton.FirstOrDefault(x => x.Checked == true);

            // Ordena la lista
            switch (radio.TabIndex)
            {
                // Enumeración de izquierda a derecha, arriba hacia abajo
                case 0:
                    li = Tools.OrdenarIzquierdaDerechaArribaAbajo(lista);
                    break;

                // Enumeración de arriba hacia abajo, izquierda a derecha
                case 1:
                    li = Tools.OrdenarArribaAbajoIzquierdaDerecha(lista);
                    break;

                // Enumeración de izquierda a derecha, abajo hacia arriba
                case 2:
                    li = Tools.OrdenarIzquierdaDerechaAbajoArriba(lista);
                    break;

                // Enumeración de abajo hacia arriba, izquierda a derecha
                case 3:
                    li = Tools.OrdenarAbajoArribaIzquierdaDerecha(lista);
                    break;

                // Enumeración de derecha a izquierda, arriba hacia abajo
                case 4:
                    li = Tools.OrdenarDerechaIzquierdaArribaAbajo(lista);
                    break;

                // Enumeración de arriba hacia abajo, derecha a izquierda
                case 5:
                    li = Tools.OrdenarArribaAbajoDerechaIzquierda(lista);
                    break;

                // Enumeración de derecha a izquierda, abajo hacia arriba
                case 6:
                    li = Tools.OrdenarDerechaIzquierdaAbajoArriba(lista);
                    break;

                // Enumeración de abajo hacia arriba, derecha a izquierda
                case 7:
                    li = Tools.OrdenarAbajoArribaDerechaIzquierda(lista);
                    break;

                // Enumeración de abajo hacia arriba, derecha a izquierda
                default:
                    li = lista;
                    break;
            }

            return li;
        }

        ///<summary> Ordena una lista de elementos de izquierda a derecha, arriba hacia abajo y la devuelve </summary>
        public static List<Element> OrdenarIzquierdaDerechaArribaAbajo(List<Element> lista)
        {
            // Ordena la lista XYZ con una precisión determinada
            lista = lista.OrderByDescending(pos => Math.Round((pos.Location as LocationPoint).Point.Y, precisionOrdenarY))
                         .ThenBy(pos => Math.Round((pos.Location as LocationPoint).Point.X, precisionOrdenarX)).ToList();

            return lista;
        }

        ///<summary> Ordena una lista de elementos de arriba hacia abajo, izquierda a derecha y la devuelve </summary>
        public static List<Element> OrdenarArribaAbajoIzquierdaDerecha(List<Element> lista)
        {
            // Ordena la lista XYZ con una precisión determinada
            lista = lista.OrderBy(pos => Math.Round((pos.Location as LocationPoint).Point.X, precisionOrdenarX))
                         .ThenByDescending(pos => Math.Round((pos.Location as LocationPoint).Point.Y, precisionOrdenarY)).ToList();

            return lista;
        }

        ///<summary> Ordena una lista de elementos de izquierda a derecha, abajo hacia arriba y la devuelve </summary>
        public static List<Element> OrdenarIzquierdaDerechaAbajoArriba(List<Element> lista)
        {
            // Ordena la lista XYZ con una precisión determinada
            lista = lista.OrderBy(pos => Math.Round((pos.Location as LocationPoint).Point.Y, precisionOrdenarY))
                         .ThenBy(pos => Math.Round((pos.Location as LocationPoint).Point.X, precisionOrdenarX)).ToList();

            return lista;
        }

        ///<summary> Ordena una lista de elementos de abajo hacia arriba, izquierda a derecha y la devuelve </summary>
        public static List<Element> OrdenarAbajoArribaIzquierdaDerecha(List<Element> lista)
        {
            // Ordena la lista XYZ con una precisión determinada
            lista = lista.OrderBy(pos => Math.Round((pos.Location as LocationPoint).Point.X, precisionOrdenarX))
                         .ThenBy(pos => Math.Round((pos.Location as LocationPoint).Point.Y, precisionOrdenarY)).ToList();

            return lista;
        }

        ///<summary> Ordena una lista de elementos de derecha a izquierda, arriba hacia abajo y la devuelve </summary>
        public static List<Element> OrdenarDerechaIzquierdaArribaAbajo(List<Element> lista)
        {
            // Ordena la lista XYZ con una precisión determinada
            lista = lista.OrderByDescending(pos => Math.Round((pos.Location as LocationPoint).Point.Y, precisionOrdenarY))
                         .ThenByDescending(pos => Math.Round((pos.Location as LocationPoint).Point.X, precisionOrdenarX)).ToList();

            return lista;
        }

        ///<summary> Ordena una lista de elementos de arriba hacia abajo, derecha a izquierda y la devuelve </summary>
        public static List<Element> OrdenarArribaAbajoDerechaIzquierda(List<Element> lista)
        {
            // Ordena la lista XYZ con una precisión determinada
            lista = lista.OrderByDescending(pos => Math.Round((pos.Location as LocationPoint).Point.X, precisionOrdenarX))
                         .ThenByDescending(pos => Math.Round((pos.Location as LocationPoint).Point.Y, precisionOrdenarY)).ToList();

            return lista;
        }

        ///<summary> Ordena una lista de elementos de derecha a izquierda, abajo hacia arriba y la devuelve </summary>
        public static List<Element> OrdenarDerechaIzquierdaAbajoArriba(List<Element> lista)
        {
            // Ordena la lista XYZ con una precisión determinada
            lista = lista.OrderBy(pos => Math.Round((pos.Location as LocationPoint).Point.Y, precisionOrdenarY))
                         .ThenByDescending(pos => Math.Round((pos.Location as LocationPoint).Point.X, precisionOrdenarX)).ToList();

            return lista;
        }

        ///<summary> Ordena una lista de elementos de abajo hacia arriba, derecha a izquierda y la devuelve </summary>
        public static List<Element> OrdenarAbajoArribaDerechaIzquierda(List<Element> lista)
        {
            // Ordena la lista XYZ con una precisión determinada
            lista = lista.OrderByDescending(pos => Math.Round((pos.Location as LocationPoint).Point.X, precisionOrdenarX))
                         .ThenBy(pos => Math.Round((pos.Location as LocationPoint).Point.Y, precisionOrdenarY)).ToList();

            return lista;
        }

        #endregion

        #region DataGridView con diámetros de barras y estilos de líneas

        ///<summary> Crea el DataGridView de diámetros y estilos de líneas y lo rellena con las configuraciones que el usuario hizo </summary>
        public static System.Windows.Forms.DataGridView ObtenerDataGridViewDeDiametrosYEstilos(System.Windows.Forms.DataGridView dgv, Document doc, string IdiomaDelPrograma)
        {
            if (dgv.Columns.Count == 0)
            {
                dgv = Tools.CrearDataGridViewDeDiametrosYEstilos(IdiomaDelPrograma);
            }

            Tools.RellenarDataGridViewDeDiametrosYEstilos(dgv, doc);

            DGVEntity dgvEntidad = Tools.ObtenerEntityDiametrosYEstilos(doc);

            Dictionary<ElementId, ElementId> diccionario = dgvEntidad.DGVDiametrosYEstilos;

            if (diccionario.Count > 0)
            {
                List<RebarBarType> diametros = Tools.ObtenerTodosTiposSegunClase(doc, typeof(RebarBarType)).Cast<RebarBarType>().ToList();
                List<Category> estilos = Tools.ObtenerEstilosDeLinea(doc);

                for (int i = 0; i < dgv.Rows.Count; i++)
                {
                    try
                    {
                        RebarBarType tipoDiametro = diametros.FirstOrDefault(x => x.Name == dgv.Rows[i].Cells[AboutJump.nombreColumnaDiametros].Value.ToString());
                        Category categEstilo = estilos.FirstOrDefault(x => x.Name == dgv.Rows[i].Cells[AboutJump.nombreColumnaEstilosLineas].Value.ToString());

                        if (diccionario.ContainsKey(tipoDiametro.Id))
                        {
                            ElementId elemId = diccionario[tipoDiametro.Id];

                            Category estilo = estilos.FirstOrDefault(x => x.Id == elemId);

                            System.Windows.Forms.DataGridViewComboBoxCell dgvComboCell = dgv.Rows[i].Cells[AboutJump.nombreColumnaEstilosLineas] as System.Windows.Forms.DataGridViewComboBoxCell;

                            if (dgvComboCell.Items.Contains(estilo.Name))
                            {
                                dgv.Rows[i].Cells[AboutJump.nombreColumnaEstilosLineas].Value = dgvComboCell.Items[dgvComboCell.Items.IndexOf(estilo.Name)];
                            }
                        }
                    }
                    catch (Exception) { continue; }
                }
            }

            return dgv;
        }

        ///<summary> Crea el DataGridView de diámetros y estilos de líneas </summary>
        public static System.Windows.Forms.DataGridView CrearDataGridViewDeDiametrosYEstilos(string IdiomaDelPrograma)
        {
            // Crea el DataGridView
            System.Windows.Forms.DataGridView dgv = new System.Windows.Forms.DataGridView();

            // Desactiva que el usuario pueda agregar filas
            dgv.AllowUserToAddRows = false;

            // Crea la columna de los diámetros
            System.Windows.Forms.DataGridViewTextBoxColumn dgvText = new System.Windows.Forms.DataGridViewTextBoxColumn();
            
            // Asigna el encabezado de la columna
            dgvText.HeaderText = Language.ObtenerTexto(IdiomaDelPrograma, "Conf3-2");

            // Asigna el nombre de la columna
            dgvText.Name = AboutJump.nombreColumnaDiametros;

            // Asigna que sea solo de lectura
            dgvText.ReadOnly = false;

            // Asigna el ancho mínimo
            dgvText.MinimumWidth = 50;

            // Crea la columna del combobox de los estilos de líneas
            System.Windows.Forms.DataGridViewComboBoxColumn dgvCombo = new System.Windows.Forms.DataGridViewComboBoxColumn();

            // Estilo de lista desplegable
            dgvCombo.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.DropDownButton;

            // Asigna el encabezado de la columna
            dgvCombo.HeaderText = Language.ObtenerTexto(IdiomaDelPrograma, "Conf3-3");

            // Asigna el nombre de la columna
            dgvCombo.Name = AboutJump.nombreColumnaEstilosLineas;

            // Asigna que sea solo de lectura
            dgvCombo.ReadOnly = false;

            // Asigna el ancho mínimo
            dgvCombo.MinimumWidth = 50;

            // Agrega las columnas al DataGridView
            dgv.Columns.Add(dgvText);
            dgv.Columns.Add(dgvCombo);

            return dgv;
        }

        ///<summary> Rellena un DataGridView con los diámetros de barras ordenados de menor a mayor y estilos de líneas </summary>
        public static void RellenarDataGridViewDeDiametrosYEstilos(System.Windows.Forms.DataGridView dgv, Document doc)
        {
            // Limpia el DataGrid
            dgv.Rows.Clear();
            dgv.Refresh();

            System.Windows.Forms.DataGridViewComboBoxColumn dgvCombo = dgv.Columns[AboutJump.nombreColumnaEstilosLineas] as System.Windows.Forms.DataGridViewComboBoxColumn;

            // Crea las clases a buscar
            Type claseDiametro = typeof(RebarBarType);

            // Crea la lista de los diámetros de barras de acero del proyecto
            List<Element> diametrosTemp = new List<Element>();
            List<RebarBarType> diametros = new List<RebarBarType>();

            // Obtiene todos los diámetros de barras del proyecto
            diametrosTemp = Tools.ObtenerTodosTiposSegunClase(doc, claseDiametro);

            // Recorre la lista de diámetros
            foreach (Element elem in diametrosTemp)
            {
                // Agrega el tipo de barra a la lista
                diametros.Add(elem as RebarBarType);
            }

            // Ordena la lista por el diámetro de barra
            diametros = diametros.OrderBy(x => x.BarDiameter).ToList();

            // Crea la lista de los estilos de lineas del proyecto
            List<Category> estilos = new List<Category>();

            // Obtiene todos los estilos de lineas del proyecto
            estilos = Tools.ObtenerEstilosDeLinea(doc).Cast<Category>().ToList();

            // Verifica que tenga diámetros la lista
            if (diametros.Count > 0)
            {
                // Recorre el DataGrid y agrega los valores de diámetros y estilos de linea
                for (int i = 0; i < diametros.Count; i++)
                {
                    // Limpia el combobox
                    dgvCombo.Items.Clear();

                    // Agrega una fila al Datagrid
                    dgv.Rows.Add();

                    // Agrega los diámetros a la primera columna
                    dgv.Rows[i].Cells[AboutJump.nombreColumnaDiametros].Value = diametros[i].Name;
                    dgv.Rows[i].Cells[AboutJump.nombreColumnaDiametros].ReadOnly = true;

                    // Agrega los estilos de linea al combobox
                    foreach (Category cat in estilos)
                    {
                        // Agrega el estilo al combobox
                        dgvCombo.Items.Add(cat.Name);
                    }

                    // Verifica la lista contenga elementos
                    if (estilos.Count > 0)
                    {
                        // Castea el combobox
                        System.Windows.Forms.DataGridViewComboBoxCell dgvComboCell = dgv.Rows[i].Cells[AboutJump.nombreColumnaEstilosLineas] as System.Windows.Forms.DataGridViewComboBoxCell;

                        // Asigna el primer elemento a la lista desplegable
                        dgv.Rows[i].Cells[AboutJump.nombreColumnaEstilosLineas].Value = dgvComboCell.Items[0];
                    }
                }
            }
        }

        ///<summary> Guarda el DataGridView en el documento </summary>
        public static void GuardarDataGridViewEnDocumento(System.Windows.Forms.DataGridView dgv, Document doc)
        {
            if (dgv.Columns.Count > 0 && dgv.Rows.Count > 0)
            {
                List<RebarBarType> diametros = Tools.ObtenerTodosTiposSegunClase(doc, typeof(RebarBarType)).Cast<RebarBarType>().ToList();
                List<Category> estilos = Tools.ObtenerEstilosDeLinea(doc);

                DGVEntity DGVDiametros = new DGVEntity();
                Dictionary<ElementId, ElementId> diccionario = new Dictionary<ElementId, ElementId>();

                for (int i = 0; i < dgv.Rows.Count; i++)
                {
                    try
                    {
                        System.Windows.Forms.DataGridViewTextBoxCell dgvTextCell = dgv.Rows[i].Cells[AboutJump.nombreColumnaDiametros] as System.Windows.Forms.DataGridViewTextBoxCell;
                        System.Windows.Forms.DataGridViewComboBoxCell dgvComboCell = dgv.Rows[i].Cells[AboutJump.nombreColumnaEstilosLineas] as System.Windows.Forms.DataGridViewComboBoxCell;

                        RebarBarType tipoDiametro = diametros.FirstOrDefault(x => x.Name == dgvTextCell.Value.ToString());
                        Category categEstilo = estilos.FirstOrDefault(x => x.Name == dgvComboCell.Value.ToString());

                        diccionario.Add(tipoDiametro.Id, categEstilo.Id);
                    }
                    catch (Exception e) { TaskDialog.Show("0", e.Message + " " + e.StackTrace); }
                }

                DGVDiametros.DGVDiametrosYEstilos = diccionario;

                doc.ProjectInformation.SetEntity(DGVDiametros);
            }
        }

        ///<summary> Obtiene el DataGridViewEntity del proyecto </summary>
        public static DGVEntity ObtenerEntityDiametrosYEstilos(Document doc)
        {
            DGVEntity DGVDiametros = doc.ProjectInformation.GetEntity<DGVEntity>();

            if (DGVDiametros == null)
            {
                DGVDiametros = new DGVEntity();
            }

            if (DGVDiametros.DGVDiametrosYEstilos == null)
            {
                DGVDiametros.DGVDiametrosYEstilos = new Dictionary<ElementId, ElementId>();
            }

            return DGVDiametros;
        }

        ///<summary> Hace que el combobox se despliegue con un solo click </summary>
        public static void DesplegarComboboxConUnClick(System.Windows.Forms.DataGridView dgv,
                                                       System.Windows.Forms.DataGridViewCellEventArgs e)
        {
            // Verifica que no se elija el encabezado y la primera columna
            if (e.RowIndex != -1 && e.ColumnIndex == 1)
            {
                // Ocurre la magia
                dgv.BeginEdit(true);
                System.Windows.Forms.ComboBox combo = (System.Windows.Forms.ComboBox)dgv.EditingControl;
                combo.DroppedDown = true;
            }
        }

        #endregion

        #region Rellenar Combobox, ListBox, verificar CheckBox

        ///<summary> Rellena un Combobox con una lista de Element </summary>
        public static void RellenarCombobox<T>(System.Windows.Forms.ComboBox combo, List<T> lista)
        {
            // Limpia el combobox
            combo.Items.Clear();

            combo.DataSource = lista;   

            combo.DisplayMember = AboutJump.parametroMostrarUsuario;

            combo.ValueMember = AboutJump.parametroId;

            // Verifica la lista contenga elementos 
            if (lista.Count > 0)
            {
                // Asigna el primer elemento a la lista desplegable
                combo.SelectedIndex = 0;
            }
        }

        ///<summary> Rellena un Combobox con las escalas </summary>
        public static void RellenarComboboxEscalas(System.Windows.Forms.ComboBox combo)
        {
            // Limpia el combobox
            combo.Items.Clear();

            // Recorre todos los elementos de la lista
            foreach (int escala in listaEscalas)
            {
                // Agrega el objeto al combobox
                combo.Items.Add(escalaInicio + escala);
            }
        }

        ///<summary> Rellena un Combobox con las categorias </summary>
        public static void RellenarComboboxCategorias(System.Windows.Forms.ComboBox combo, List<Category> categorias)
        {
            // Limpia el combobox
            combo.Items.Clear();

            // Recorre la lista y agrega las cotas al combobox
            foreach (Category cat in categorias)
            {
                combo.Items.Add(cat.Name);
            }

            // Verifica la lista contenga elementos 
            if (categorias.Count > 0)
            {
                // Asigna el primer elemento a la lista desplegable
                combo.SelectedIndex = 0;
            }
        }

        ///<summary> Rellena una ListBox con los elementos de una lista </summary>
        public static void RellenarListBoxDeElementos(System.Windows.Forms.ListBox listbox, Document doc, List<Element> lista)
        {
            // Recorre la lista y agrega a la listbox elementos
            try
            {
                foreach (Element elem in lista)
                {
                    // Obtiene el FamilySymbol del elemento
                    FamilySymbol sym = doc.GetElement(elem.GetTypeId()) as FamilySymbol;

                    // Crea el nombre a mostrar y luego el ID del elemento
                    string nombre = sym.Family.Name + ": " + elem.Name + " <" + elem.Id.ToString() + ">";

                    // Agrega el objeto y asigna el nombre 
                    listbox.Items.Add(nombre);
                }
            }
            catch (Exception) { }
        }

        ///<summary> Rellena una ListBox con los elementos de una lista </summary>
        public static void RellenarListBoxDeParametros(System.Windows.Forms.ListBox listbox, List<Parameter> lista)
        {
            // Recorre los parametros del primer elemento y agrega a la listbox
            try
            {
                // Recorre todos los parámetros de ejemplar
                foreach (Parameter param in lista)
                {
                    // Agrega el nombre
                    listbox.Items.Add(param.Definition.Name);
                }

                // Verifica la lista contenga elementos 
                if (lista.Count > 0)
                {
                    // Asigna el primer elemento de la lista
                    listbox.SelectedIndex = 0;
                }

            }
            catch (Exception) { }
        }

        ///<summary> Enumera una lista de elementos en base al parámetro seleccionado, prefijo, n° inicial y sufijo </summary>
        public static void EnumeracionParametrosEjemplarListaElementos(System.Windows.Forms.ListBox listbox, 
                                                                       List<Element> listaElementos,
                                                                       string prefijo, string inicial,
                                                                       string incremento, string sufijo)
        {
            // Crea el string para la enumeración
            string enumeracion = null;
            int numeroInicial = 0;
            int numeroIncremento = 0;
            int i = 0;

            // Obtiene el nombre del parámetro seleccionado
            string parametroSeleccionado = listbox.SelectedItem.ToString();

            // Verifica que inicial tenga valor
            if (inicial != null && inicial != "")
            {
                numeroInicial = Convert.ToInt32(inicial);
            }

            // En caso que no tenga valor lo asigna cero
            else
            {
                numeroInicial = 0;
            }

            // Verifica que inicial e incremento tengan valor para realizar la enumeración
            if (incremento != null && incremento != "")
            {
                // Recorre la lista
                foreach (Element elem in listaElementos)
                {
                    // Busca el parámetro en el elemento
                    Parameter parametro = elem.LookupParameter(parametroSeleccionado);

                    // Genera la enumeración
                    i = numeroInicial + numeroIncremento;
                    enumeracion = prefijo + i.ToString() + sufijo;

                    // Asigna la enumeración al parámetro
                    parametro.Set(enumeracion);

                    // Incrementa 
                    numeroIncremento += Convert.ToInt32(incremento);
                }
            }
        }

        ///<summary> Enumera un elemento en base al parámetro seleccionado, prefijo, valor actual y sufijo </summary>
        public static void EnumeracionParametrosEjemplarListaElementos(System.Windows.Forms.ListBox listbox,
                                                                       Element elemento, string prefijo, int valorActual, string sufijo)
        {
            // Obtiene el nombre del parámetro seleccionado
            string parametroSeleccionado = listbox.SelectedItem.ToString();
                        
            // Busca el parámetro en el elemento
            Parameter parametro = elemento.LookupParameter(parametroSeleccionado);

            // Asigna la enumeración al parámetro
            parametro.Set(prefijo + valorActual.ToString() + sufijo);
            
        }

        /// <summary> Valida que los textos ingresados en un TexteBox sean solamente números </summary>
        public static void VerificarSoloNumero(System.Windows.Forms.KeyPressEventArgs e)
        {
            // Verifica que sólo se introduzcan números
            if (Char.IsDigit(e.KeyChar))
            {
                e.Handled = false;
            }

            else
            {
                // Permite teclas de control como borrar
                if (Char.IsControl(e.KeyChar))
                {
                    e.Handled = false;
                }

                // Desactiva el resto de teclas
                else
                {
                    e.Handled = true;
                }
            }
        }

        #endregion

        #region Importar y exportar desde Excel

        /// <summary> Carga los diámetros y estilos de lineas de un archivo CSV </summary>
        public static void CargarDiametrosYEstilos(System.Windows.Forms.DataGridView dgv,
                                                   System.Windows.Forms.DataGridViewComboBoxColumn dgvCombo,
                                                   Document doc, string rutaArchivo)
        {
            using (SLDocument slDoc = new SLDocument(rutaArchivo))
            {
                // Crea un DataGridViewComboboxColumn
                System.Windows.Forms.DataGridViewComboBoxColumn dgvCombobox = new System.Windows.Forms.DataGridViewComboBoxColumn();

                // Crea los contadores
                int i = 0;
                int j = 0;

                // Recorre las filas
                while (!string.IsNullOrEmpty(slDoc.GetCellValueAsString(i + 1, j + 1)))
                {
                    try
                    {
                        // Verifica que la celda 
                        if (dgv.Rows[i].Cells[j].Value.ToString() == slDoc.GetCellValueAsString(i + 1, j + 1))
                        {
                            // Limpia el DataGridViewComboboxColumn
                            dgvCombobox.Items.Clear();

                            // Asigna el valor del excel
                            dgvCombobox.Items.Add(slDoc.GetCellValueAsString(i + 1, j + 2));

                            // Asigna el valor al DataGridViewComboboxColumn
                            dgv.Rows[i].Cells[AboutJump.nombreColumnaEstilosLineas].Value = dgvCombobox.Items[0];
                        }

                        // Aumenta el contador
                        i++;
                    }
                    catch (Exception) { }
                }
            }
        }

        /// <summary> Guarda los diámetros y estilos de lineas en un archivo externo </summary>
        public static void GuardarDiametrosYEstilos(System.Windows.Forms.DataGridView dgv, string rutaArchivo)
        {
            using (SLDocument slDoc = new SLDocument())
            {
                // Crea una DataTable
                DataTable dt = new DataTable();

                // Recorre las columnas
                for (int i = 0; i < dgv.Columns.Count; i++)
                {
                    // Agrega la columna
                    dt.Columns.Add(dgv.Columns[i].Name);
                }

                // Recorre las filas
                for (int i = 0; i < dgv.Rows.Count; i++)
                {
                    // Asigna el valor del DataGridView a la matriz
                    dt.Rows.Add(dgv.Rows[i].Cells[AboutJump.nombreColumnaDiametros].Value.ToString(), dgv.Rows[i].Cells[AboutJump.nombreColumnaEstilosLineas].Value.ToString());
                }

                // Asigna la matriz al excel
                slDoc.ImportDataTable(celdaExcelInsertarDiametros, dt, celdaExcelEncabezadoDiametros);

                // Cambia el nombre de la pestaña
                slDoc.RenameWorksheet(SLDocument.DefaultFirstSheetName, pestanaExcelDiametros);

                // Ajusta el ancho de las columnas
                for (int i = 1; i <= dgv.Columns.Count; i++)
                {
                    slDoc.AutoFitColumn(i);
                }

                // Guarda el archivo de excel
                slDoc.SaveAs(rutaArchivo);
            }
        }

        #endregion
    }
}
