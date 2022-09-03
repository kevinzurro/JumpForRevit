using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.ApplicationServices;
using System.Windows.Media.Imaging;

namespace Jump
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class cmdArmaduraTapada : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // Variables necesarias
            UIApplication uiApp = commandData.Application;
            UIDocument uiDoc = uiApp.ActiveUIDocument;
            Application app = uiApp.Application;
            Document doc = uiDoc.Document;

            Tools.AddinManager();

            string IdiomaDelPrograma = Tools.ObtenerIdiomaDelPrograma();

            // Crear los parámetro de la vista actual
            View vistaActual = doc.ActiveView;

            // Cambia la visibilidad a falso
            bool visibilidad = false;

            // Empieza la transacción
            using (Transaction t = new Transaction(doc, Language.ObtenerTexto(IdiomaDelPrograma, "VisArmTap")))
            {
                t.Start();

                // Llama al método que cambia la visibilidad de todas la barras
                Tools.ActivarVisibilidadArmaduras(doc, vistaActual);
                Tools.ArmaduraVisible(doc, vistaActual, vistaActual.Id, visibilidad);

                t.Commit();
            };
            
            return Result.Succeeded;
        }
    }
}
