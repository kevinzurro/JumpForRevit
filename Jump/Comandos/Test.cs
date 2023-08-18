using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.ApplicationServices;

namespace Jump
{
    public static class Test
    {
        ///<summary> Crea un cuadro 3D de un recuadro </summary>
        public static void CrearRecuadroElemento(Document doc, View vista, Outline recuadro)
        {
            try
            {
                BoundingBoxXYZ bb = new BoundingBoxXYZ();

                bb.Max = recuadro.MaximumPoint;
                bb.Min = recuadro.MinimumPoint;

                CrearRecuadroElemento(doc, vista, bb);
            }
            catch (Exception) { }
        }

        ///<summary> Crea un cuadro 3D del BoundingBox </summary>
        public static void CrearRecuadroElemento(Document doc, View vista, BoundingBoxXYZ bb)
        {
            try
            {
                if (!doc.IsModifiable)
                {
                    using (Transaction tra = new Transaction(doc, "Pueba"))
                    {
                        tra.Start();

                        CrearReacuadro(doc, vista, bb);

                        tra.Commit();
                    }
                }
                else
                {
                    CrearReacuadro(doc, vista, bb);
                }
            }
            catch (Exception) { }
        }

        static void CrearReacuadro(Document doc, View vista, BoundingBoxXYZ bb)
        {
            double xMin = bb.Min.X;
            double yMin = bb.Min.Y;
            double zMin = bb.Min.Z;
            double xMax = bb.Max.X;
            double yMax = bb.Max.Y;
            double zMax = bb.Max.Z;

            XYZ punto1 = new XYZ(xMin, yMin, zMin);
            XYZ punto2 = new XYZ(xMin, yMin, zMax);
            XYZ punto3 = new XYZ(xMin, yMax, zMax);
            XYZ punto4 = new XYZ(xMax, yMax, zMax);
            XYZ punto5 = new XYZ(xMax, yMax, zMin);
            XYZ punto6 = new XYZ(xMax, yMin, zMin);
            XYZ punto7 = new XYZ(xMin, yMax, zMin);
            XYZ punto8 = new XYZ(xMax, yMin, zMax);

            List<Line> lineas = new List<Line>();

            try { lineas.Add(Line.CreateBound(punto1, punto2)); } catch (Exception) { }
            try { lineas.Add(Line.CreateBound(punto1, punto6)); } catch (Exception) { }
            try { lineas.Add(Line.CreateBound(punto1, punto7)); } catch (Exception) { }
            try { lineas.Add(Line.CreateBound(punto2, punto3)); } catch (Exception) { }
            try { lineas.Add(Line.CreateBound(punto2, punto8)); } catch (Exception) { }
            try { lineas.Add(Line.CreateBound(punto3, punto4)); } catch (Exception) { }
            try { lineas.Add(Line.CreateBound(punto3, punto7)); } catch (Exception) { }
            try { lineas.Add(Line.CreateBound(punto4, punto5)); } catch (Exception) { }
            try { lineas.Add(Line.CreateBound(punto4, punto8)); } catch (Exception) { }
            try { lineas.Add(Line.CreateBound(punto5, punto6)); } catch (Exception) { }
            try { lineas.Add(Line.CreateBound(punto5, punto7)); } catch (Exception) { }
            try { lineas.Add(Line.CreateBound(punto6, punto8)); } catch (Exception) { }

            foreach (Line linea in lineas)
            {
                CurveElement curvaDetalle = doc.Create.NewDetailCurve(vista, linea) as CurveElement;
            }
        }

        ///<summary> Lleva el elemento al centro de las coordenadas del proyecto </summary>
        public static void MoverElementoAOrigen(Document doc, View vista, Element elem)
        {
            try
            {
                XYZ punto = new XYZ();

                if (elem is FamilyInstance)
                {
                    FamilyInstance fi = elem as FamilyInstance;

                    punto = fi.GetTransform().Origin;
                }

                else if (elem.Location is LocationPoint)
                {
                    LocationPoint pt = (LocationPoint)elem.Location;

                    punto = pt.Point;
                }

                else if (elem.Location is LocationCurve)
                {
                    LocationCurve LocCur = (LocationCurve)elem.Location;

                    Curve curva = LocCur.Curve;

                    punto = curva.GetEndPoint(0);
                }

                else
                {
                    punto = Tools.ObtenerBaricentroDeRecuadro(elem.get_BoundingBox(vista));
                }

                ElementTransformUtils.MoveElement(doc, elem.Id, -punto);
            }
            catch (Exception) { }
        }
    }
}
