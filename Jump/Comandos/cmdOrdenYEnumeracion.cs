using System;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.Globalization;
using System.Resources;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.ApplicationServices;
using Jump.Languages;

namespace Jump
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    class cmdOrdenYEnumeracion : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // Variables necesarias
            UIApplication uiApp = commandData.Application;
            UIDocument uiDoc = uiApp.ActiveUIDocument;
            Application app = uiApp.Application;
            Document doc = uiDoc.Document;

            // Resources.Titulo1 = Ingles
            // Resources.es-ES.Titulo1 = Español

            Debug.WriteLine(CultureInfo.CurrentUICulture is null); //return false (es-ES)

            Debug.WriteLine(Thread.CurrentThread.CurrentUICulture is null); //return false (es-ES)

            Debug.WriteLine(Resources.Culture is null); //return true (null)

            Debug.WriteLine(Resources.Titulo1); // return Ingles

            Resources.Culture = new CultureInfo("es-ES");

            Debug.WriteLine(Resources.Culture is null); //return false (es-ES)

            Debug.WriteLine(Resources.Titulo1); // return Ingles however, it should return to Español

            return Result.Succeeded;
        }
    }
}
