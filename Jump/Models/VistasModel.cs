using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.UI.Events;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Diagnostics;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;

namespace Jump.Models
{
    public class VistasModel : ModelBase
    {
        public VistasModel(UIApplication UIApp)
        {
            this.UIApp = UIApp;
            this.UIDoc = UIApp.ActiveUIDocument;
            this.Appli = UIApp.Application;
            this.Doc = UIDoc.Document;
        }

        /// <summary> Obtiene todos los tipos de planos </summary>
        public ObservableCollection<Familia> ObtenerTodosLosTiposDePlanos()
        {
            ObservableCollection<Familia> planos = new ObservableCollection<Familia>();

            List<Element> TodosPlanos = Tools.ObtenerTodosTiposSegunCategoria(Doc, BuiltInCategory.OST_TitleBlocks);

            foreach (FamilySymbol fs in TodosPlanos)
            {
                planos.Add(new Familia(fs));
            }

            return planos;
        }

        /// <summary> Obtiene todas las vistas del proyecto </summary>
        public ObservableCollection<MenuItem> ObtenerTodasLasVistas()
        {
            List<View> vistas = Tools.ObtenerTodosEjemplaresSegunClase(this.Doc, typeof(View)).Cast<View>().Where(x => !x.IsTemplate).ToList();

            ObservableCollection<MenuItem> obsVistas = CrearTreeViewDeVistas(vistas);

            return obsVistas;
        }

        /// <summary> Obtiene todas las vistas que puedan crearse en un plano </summary>
        public ObservableCollection<MenuItem> ObtenerVistasHabilitadasParaPlanos()
        {
            List<View> vistasExistentes = new List<View>();

            List<ViewSheet> planos = new FilteredElementCollector(this.Doc).OfClass(typeof(ViewSheet)).WhereElementIsNotElementType().Cast<ViewSheet>().ToList();

            foreach (ViewSheet plano in planos)
            {
                vistasExistentes.AddRange(Tools.ObtenerElementoSegunID(this.Doc, plano.GetAllPlacedViews().ToList()).Cast<View>());
            }

            List<View> vistas = Tools.ObtenerTodosEjemplaresSegunClase(this.Doc, typeof(View)).Cast<View>().Where(x => !x.IsTemplate).ToList();

            List<View> vistasHabilitadas = Tools.ObtenerElementosNoCoincidentesConLista(vistas.Cast<Element>().ToList(), vistasExistentes.Cast<Element>().ToList()).Cast<View>().ToList();

            ObservableCollection<MenuItem> obsVistas = CrearTreeViewDeVistas(vistasHabilitadas);

            return obsVistas;
        }

        /// <summary> Obtiene una colección de tipos de vistas con sus vistas </summary>
        private ObservableCollection<MenuItem> CrearTreeViewDeVistas(List<View> vistas)
        {
            ObservableCollection<MenuItem> obsVistas = new ObservableCollection<MenuItem>();

            List<ViewFamilyType> tipoVistas = new FilteredElementCollector(this.Doc).
                                                  OfClass(typeof(ViewFamilyType)).
                                                  WhereElementIsElementType().
                                                  Cast<ViewFamilyType>().
                                                  Where(x => x.ViewFamily != ViewFamily.Sheet).ToList();

            tipoVistas = tipoVistas.OrderBy(x => x.FamilyName).ThenBy(x => x.Name).ToList();

            foreach (ViewFamilyType tipo in tipoVistas)
            {
                MenuItem nodoPrin = new MenuItem(new Familia(tipo));

                nodoPrin.Nombre = nodoPrin.NombreCompleto;

                List<View> vistasDelTipo = vistas.Where(x => x.GetTypeId() == tipo.Id).OrderBy(x => x.Name).ToList();

                foreach (View vista in vistasDelTipo)
                {
                    MenuItem nodo = new MenuItem(new Familia(vista));
                    
                    nodoPrin.Items.Add(nodo);
                }

                if (nodoPrin.Items.Count > 0)
                {
                    obsVistas.Add(nodoPrin);
                }
            }
            return obsVistas;
        }

        /// <summary> Crea los planos de las vistas </summary>
        public void CrearPlanos(ObservableCollection<Familia> vistasSeleccionadas, Familia tipoPlano, bool unPlanoPorVista)
        {
            List<View> vistas = ObtenerVistas(vistasSeleccionadas);

            frmBarraProgreso barraProgreso = new frmBarraProgreso(vistasSeleccionadas.Count);

            barraProgreso.Show();

            int contadorPlanos = 0;

            using (Transaction tra = new Transaction(this.Doc, Language.ObtenerTexto(IdiomaDelPrograma, "CreaPlano4-1")))
            {
                tra.Start();

                if (unPlanoPorVista)
                {
                    foreach (View vista in vistas)
                    {
                        ViewSheet plano = ViewSheet.Create(this.Doc, new ElementId(tipoPlano.ID));

                        this.Doc.Regenerate();

                        UV planoMin = plano.Outline.Min;
                        UV planoMax = plano.Outline.Max;

                        XYZ planoCentro = new XYZ(planoMax.U + planoMin.U, planoMax.V + planoMin.V, 0) / 2;

                        if (barraProgreso.Cancelado())
                        {
                            break;
                        }

                        try
                        {
                            ColocarVistaEnPlano(plano, vista, planoCentro);

                            contadorPlanos++;
                        }
                        catch (Exception) { }

                        barraProgreso.Incrementar();
                    }
                }
                else
                {
                    ViewSheet plano = ViewSheet.Create(this.Doc, new ElementId(tipoPlano.ID));

                    this.Doc.Regenerate();

                    UV planoMin = plano.Outline.Min;
                    UV planoMax = plano.Outline.Max;

                    double anchoPlano = planoMax.U - planoMin.U;

                    double xPos = planoMin.U;
                    double yPos = planoMax.V;

                    double alturaMax = double.MinValue;

                    foreach (View vista in vistas)
                    {
                        if (barraProgreso.Cancelado())
                        {
                            break;
                        }

                        try
                        {
                            // Coloca la vista inicialmente en la esquina superior izquierda
                            XYZ punto = new XYZ(planoMin.U, planoMax.V, 0);

                            Element elem = ColocarVistaEnPlano(plano, vista, punto);

                            this.Doc.Regenerate();

                            if (elem != null)
                            {
                                BoundingBoxXYZ bb = elem.get_BoundingBox(plano);

                                double elemAncho = bb.Max.X - bb.Min.X;
                                double elemAlto = bb.Max.Y - bb.Min.Y;

                                // Comprobar si el elemento entra en la fila actual; de lo contrario mueve a la siguiente fila
                                if ((xPos + elemAncho) > (planoMin.U + anchoPlano))
                                {
                                    // Restablecer xPos al inicio de la nueva fila
                                    xPos = planoMin.U;

                                    // Mover hacia abajo usando la altura más alta de la fila anterior
                                    yPos -= alturaMax;

                                    // Reiniciar la altura máxima para la nueva fila
                                    alturaMax = double.MinValue;
                                }

                                // Mover el elemento a su nueva posición
                                XYZ nuevaPos = new XYZ(xPos - bb.Min.X, yPos - bb.Max.Y, 0);

                                ElementTransformUtils.MoveElement(this.Doc, elem.Id, nuevaPos);

                                // Actualizar xPos para el siguiente elemento en la fila actual
                                xPos += elemAncho;
                                
                                // Actualizar la altura máxima de la fila si este elemento es el más alto
                                if (elemAlto > alturaMax)
                                {
                                    alturaMax = elemAlto;
                                }
                            }
                        }
                        catch (Exception) { }

                        barraProgreso.Incrementar();
                    }

                    contadorPlanos++;
                }

                if (contadorPlanos > 0 && !barraProgreso.Cancelado())
                {
                    tra.Commit();

                    barraProgreso.Close();

                    if (unPlanoPorVista)
                    {
                        TaskDialog.Show(Language.ObtenerTexto(IdiomaDelPrograma, "CreaPlano4-2"),
                                   contadorPlanos.ToString() + Language.ObtenerTexto(IdiomaDelPrograma, "CreaPlano4-3"));
                    }
                    else
                    {
                        TaskDialog.Show(Language.ObtenerTexto(IdiomaDelPrograma, "CreaPlano4-2"),
                                   contadorPlanos.ToString() + Language.ObtenerTexto(IdiomaDelPrograma, "CreaPlano4-4"));
                    }
                }
                else
                {
                    tra.RollBack();

                    barraProgreso.Close();
                }
            }
        }

        /// <summary> Obtiene las vistas a partir de las familias </summary>
        public List<View> ObtenerVistas(ObservableCollection<Familia> familias)
        {
            List<View> vistas = new List<View>();

            foreach (Familia familia in familias)
            {
                vistas.Add(Doc.GetElement(new ElementId(familia.ID)) as View);
            }

            return vistas;
        }

        /// <summary> Coloca la vista en el plano </summary>
        public Element ColocarVistaEnPlano(ViewSheet plano, View vista, XYZ origen)
        {
            Element elem = null;

            if (vista is ViewSchedule)
            {
                ScheduleSheetInstance tabla = ScheduleSheetInstance.Create(this.Doc, plano.Id, vista.Id, origen);

                elem = tabla;
            }
            else
            {
                Viewport viewport = Viewport.Create(this.Doc, plano.Id, vista.Id, origen);

                elem = viewport;
            }

            return elem;
        }
    }
}
