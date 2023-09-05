using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Exceptions;

namespace Jump
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class cmdLosas : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiApp = commandData.Application;
            UIDocument uiDoc = uiApp.ActiveUIDocument;
            Application app = uiApp.Application;
            Document doc = uiDoc.Document;

            Tools.AddinManager();

            using (Transaction tra = new Transaction(doc, "Test"))
            {
                tra.Start();

                bool bandera = true;

                View vista = doc.ActiveView;

                while (bandera)
                {
                    try
                    {
                        Element elem = doc.GetElement(uiDoc.Selection.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Element));

                        //Test.MoverElementoAOrigen(doc, vista, elem);
                        Test.CrearRecuadroElemento(doc, vista, elem.get_BoundingBox(vista));
                    }
                    catch (Exception)
                    {
                        bandera = false;
                    }
                }

                tra.Commit();
            }
            return Result.Succeeded;
        }            
    }
}
