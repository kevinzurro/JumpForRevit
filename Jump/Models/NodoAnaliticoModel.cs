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
        Type claseVinculos = typeof(AnalyticalLinkType);
        BuiltInCategory categoriaNodos = BuiltInCategory.OST_AnalyticalNodes;
        BuiltInCategory categoriaVinculos = BuiltInCategory.OST_LinksAnalytical;
        double error = Properties.Settings.Default.ConfiguracionPrecisionNodoAnalitico;

        List<ReferencePoint> todosNodosRefPoi = new List<ReferencePoint>();
        ObservableCollection<NodoAnalitico> todosNodos = new ObservableCollection<NodoAnalitico>();
        ObservableCollection<NodoAnalitico> nodosConCercania = new ObservableCollection<NodoAnalitico>();
        ObservableCollection<NodoAnalitico> nodosOrdenados = new ObservableCollection<NodoAnalitico>();
        ObservableCollection<Familia> tiposDeVinculosAnaliticos = new ObservableCollection<Familia>();

        public NodoAnaliticoModel(UIApplication UIApp)
        {
            this.UIApp = UIApp;
            this.UIDoc = UIApp.ActiveUIDocument;
            this.Appli = UIApp.Application;
            this.Doc = UIDoc.Document;

            todosNodosRefPoi = Tools.ObtenerTodosEjemplaresSegunClaseYCategoria
                (this.Doc, this.claseNodos, this.categoriaNodos).
                Cast<ReferencePoint>().ToList();

            foreach (ReferencePoint punto in todosNodosRefPoi)
            {
                todosNodos.Add(new NodoAnalitico(punto));
            }

            VerificarProximidad();

            List<AnalyticalLinkType> vinculos = Tools.ObtenerTodosTiposSegunClaseYCategoria
                (this.Doc, claseVinculos, categoriaVinculos).Cast<AnalyticalLinkType>().ToList();

            foreach (AnalyticalLinkType vinculo in vinculos)
            {
                tiposDeVinculosAnaliticos.Add(new Familia(vinculo));
            }
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

        public ObservableCollection<Familia> ObtenerVinculosAnaliticos()
        {
            return tiposDeVinculosAnaliticos;
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

        public void MoverNodos(NodoAnalitico nodoPrincipal, NodoAnalitico nodoParaMover)
        {
            using (Transaction tra = new Transaction(this.Doc, Language.ObtenerTexto(AboutJump.IdiomaAddin, "NodAnaVer2-2")))
            {
                tra.Start();

                try
                {
                    XYZ distancia = nodoPrincipal.XYZ.Subtract(nodoParaMover.XYZ);

                    ElementTransformUtils.MoveElement(this.Doc, nodoParaMover.Punto.Id, distancia);
                }
                catch (Exception) { }

                tra.Commit();
            }
        }

        public void UnirNodos(NodoAnalitico nodoPrincipal, NodoAnalitico nodoSecundario, Familia tipoDeVinculo)
        {
            using (Transaction tra = new Transaction(this.Doc, Language.ObtenerTexto(AboutJump.IdiomaAddin, "NodAnaVer2-3")))
            {
                tra.Start();

                try
                {
                    ElementId tipo = new ElementId(tipoDeVinculo.ID);

                    AnalyticalLink.Create(this.Doc, tipo, nodoPrincipal.Punto.GetHubId(), nodoSecundario.Punto.GetHubId());
                }
                catch (Exception) { }

                tra.Commit();
            }
        }
    }
}
