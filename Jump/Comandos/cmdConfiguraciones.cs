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
    public class cmdConfiguraciones : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // Variables necesarias
            UIApplication uiApp = commandData.Application;
            UIDocument uiDoc = uiApp.ActiveUIDocument;
            Application app = uiApp.Application;
            Document doc = uiDoc.Document;

            Tools.AddinManager();
            Tools.CrearRegistroActualizadorArmaduras(uiApp.ActiveAddInId);

            string IdiomaDelPrograma = Tools.ObtenerIdiomaDelPrograma();

            using (Transaction tra = new Transaction(doc, Language.ObtenerTexto(IdiomaDelPrograma, "Conf1")))
            {
                tra.Start();

                frmConfiguraciones inicioConfiguraciones = new frmConfiguraciones(doc);

                inicioConfiguraciones.ShowDialog();

                if (inicioConfiguraciones.bandera)
                {
                    tra.Commit();
                }
                else
                {
                    tra.RollBack();
                }
            }

            return Result.Succeeded;
        }
    }
}