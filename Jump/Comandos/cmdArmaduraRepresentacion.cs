using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.UI.Selection;

namespace Jump
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    internal class cmdArmaduraRepresentacion : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // Variables necesarias
            UIApplication uiApp = commandData.Application;
            UIDocument uiDoc = uiApp.ActiveUIDocument;
            Application app = uiApp.Application;
            Document doc = uiDoc.Document;

            using (Transaction tra = new Transaction(doc, "Detail"))
            {
                tra.Start();
                try
                {
                    List<Curve> curvas = new List<Curve>();
                    List<Rebar> barras = new List<Rebar>();
                    View vista = doc.ActiveView;
                    Options opcion = new Options();
                    opcion.View = vista;
                    List<Reference> referencias = uiDoc.Selection.PickObjects(ObjectType.Element).ToList();

                    foreach (Reference referencia in referencias)
                    {
                        barras.Add(doc.GetElement(referencia) as Rebar);
                    }

                    foreach (Rebar barra in barras)
                    {
                        GeometryElement geoElem = barra.get_Geometry(opcion);

                        foreach (Solid solido in geoElem)
                        {
                            foreach (Edge borde in solido.Edges)
                            {
                                Curve curva = borde.AsCurve();

                                curvas.Add(curva);
                            }
                        }
                    }

                    foreach (Curve curva in curvas)
                    {
                        Curve curvaProyectada = ProyectarCurvaSobrePlano(vista, curva);

                        CurveElement curvaDetalle = doc.Create.NewDetailCurve(vista, curvaProyectada) as CurveElement;
                    }
                }
                catch (Exception e)
                {
                    TaskDialog.Show("0", e.Message + " " + e.StackTrace);
                }
                tra.Commit();
            }



            return Result.Succeeded;
        }

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
            if (distancia != 1.0e-9)
            {
                // Proyecta el punto sobre el plano
                puntoProyectado = punto - distancia * plano.Normal;
            }

            return puntoProyectado;
        }
    }
}
