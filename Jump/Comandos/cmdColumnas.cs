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
    public class cmdColumnas : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // Variables necesarias
            UIApplication uiApp = commandData.Application;
            UIDocument uiDoc = uiApp.ActiveUIDocument;
            Application app = uiApp.Application;
            Document doc = uiDoc.Document;

            Reference referencia = uiDoc.Selection.PickObject(ObjectType.Element, "Seleccionar elemento");

            Element elem = doc.GetElement(referencia);

            FamilyInstance fi = elem as FamilyInstance;

            using(Transaction t = new Transaction(doc, "Mueve elemento"))
            {
                t.Start();

                ElementTransformUtils.MoveElement(doc, elem.Id, -fi.GetTransform().Origin);

                t.Commit();
            }

            return Result.Succeeded;
        }
    }
}
