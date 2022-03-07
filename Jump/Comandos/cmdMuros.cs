using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.ApplicationServices;

namespace Jump
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class cmdMuros : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // Variables necesarias
            UIApplication uiApp = commandData.Application;
            UIDocument uiDoc = uiApp.ActiveUIDocument;
            Application app = uiApp.Application;
            Document doc = uiDoc.Document;

            // 
            List<Element> muros = new List<Element>();
            muros = Tools.ObtenerTodosEjemplaresSegunCategoria(doc, BuiltInCategory.OST_Walls);
            using (Transaction t = new Transaction(doc, "seccion muro"))
            {
                t.Start();
                foreach (Element elem in muros)
                {
                    View vista = Tools.SeccionLongitudinalBasadoEnCurva(doc, elem);
                }
                t.Commit();
            };
            return Result.Succeeded;
        }
    }
}
