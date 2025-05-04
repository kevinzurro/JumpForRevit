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

namespace Jump
{
    public class NodoAnaliticoModel : ModelBase
    {
        Type claseNodos = typeof(ReferencePoint);
        BuiltInCategory categoria = BuiltInCategory.OST_AnalyticalNodes;
        double error = Properties.Settings.Default.ConfiguracionPrecisionNodoAnalitico;

        List<ReferencePoint> todosNodosRefPoi = new List<ReferencePoint>();
        ObservableCollection<NodoAnalitico> todosNodos = new ObservableCollection<NodoAnalitico>();
        ObservableCollection<NodoAnalitico> nodosConCercania = new ObservableCollection<NodoAnalitico>();
        ObservableCollection<NodoAnalitico> nodosOrdenados = new ObservableCollection<NodoAnalitico>();

        public NodoAnaliticoModel(UIApplication UIApp)
        {
            this.UIApp = UIApp;
            this.UIDoc = UIApp.ActiveUIDocument;
            this.Appli = UIApp.Application;
            this.Doc = UIDoc.Document;

            todosNodosRefPoi = Tools.ObtenerTodosEjemplaresSegunClaseYCategoria
                (this.Doc, this.claseNodos, this.categoria).
                Cast<ReferencePoint>().ToList();

            foreach (ReferencePoint punto in todosNodosRefPoi)
            {
                todosNodos.Add(new NodoAnalitico(punto));
            }

            VerificarProximidad();
        }

        private void VerificarProximidad()
        {
            for (int i = 0; i < todosNodos.Count; i++)
            {
                NodoAnalitico nodo = todosNodos[i];

                nodo.Punto = todosNodos[i].Punto;

                for (int j = i + 1; j < todosNodos.Count ; j++)
                {
                    double distancia = todosNodos[i].Punto.Position.DistanceTo(todosNodos[j].Punto.Position);

                    if (distancia <= error)
                    {
                        nodo.NodosCercanos.Add(todosNodos[j]);

                        nodosConCercania.Add(nodo);

                        todosNodos.Remove(todosNodos[j]);
                    }
                }
            }

            this.nodosOrdenados = new ObservableCollection<NodoAnalitico>(this.todosNodos.Where(x => nodosConCercania.Any(y => y.Punto.Id.Value == x.Punto.Id.Value)).ToList());
        }

        public ObservableCollection<NodoAnalitico> ObtenerNodosConCercania()
        {
            return nodosOrdenados;
        }

        public void AislarNodosEnLaVista(NodoAnalitico nodoPrincipal, NodoAnalitico nodoParaAislar)
        {
            View vista = this.Doc.ActiveView;

            using (Transaction tra = new Transaction(this.Doc, Language.ObtenerTexto(AboutJump.IdiomaAddin, "NodAnaVer2-1")))
            {
                tra.Start();

                try
                {
                    if (vista.IsTemporaryHideIsolateActive())
                    {
                        vista.DisableTemporaryViewMode(TemporaryViewMode.TemporaryHideIsolate);
                    }

                    ElementSet nodosAislados = new ElementSet();

                    nodosAislados.Insert(nodoPrincipal.Punto);
                    nodosAislados.Insert(nodoParaAislar.Punto);

                    List<ReferencePoint> ocultar = todosNodosRefPoi;

                    ocultar.Remove(nodoPrincipal.Punto);
                    ocultar.Remove(nodoParaAislar.Punto);

                    vista.HideElementsTemporary(Tools.ObtenerIdElemento(ocultar.Cast<Element>().ToList()));

                    this.UIDoc.ShowElements(nodosAislados);
                }
                catch (Exception) { }

                tra.Commit();
            }
        }

        public void MoverrNodos(NodoAnalitico nodoPrincipal, NodoAnalitico nodoParaAislar)
        {
            View vista = this.Doc.ActiveView;

            using (Transaction tra = new Transaction(this.Doc, Language.ObtenerTexto(AboutJump.IdiomaAddin, "NodAnaVer2-2")))
            {
                tra.Start();

                try
                {
                    XYZ distancia = nodoPrincipal.XYZ.Subtract(nodoParaAislar.XYZ);

                    ElementTransformUtils.MoveElement(this.Doc, nodoParaAislar.Punto.Id, distancia);
                }
                catch (Exception) { }

                tra.Commit();
            }
        }
    }
}
