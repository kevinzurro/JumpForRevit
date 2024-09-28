using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.ApplicationServices;
using System.Diagnostics;
using Jump.Models;
using Jump.ViewModels;
using Jump.Views;
using System.Windows.Controls;

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

            string IdiomaDelPrograma = Tools.ObtenerIdiomaDelPrograma();

            using (TransactionGroup tg = new TransactionGroup(doc, Language.ObtenerTexto(IdiomaDelPrograma, "Conf1")))
            {
                tg.Start();

                try
                {
                    ConfiguracionesModel model = new ConfiguracionesModel(uiApp);

                    ConfigGeneralViewModel ConfigGeneral = new ConfigGeneralViewModel(model);
                    ConfigEtiquetasViewModel ConfigEtiquetas = new ConfigEtiquetasViewModel(model);

                    WinConfiguraciones Configuraciones = new WinConfiguraciones();

                    Configuraciones.DataContext = ConfigGeneral;

                    Configuraciones.ShowDialog();

                    if (Configuraciones.DialogResult == true)
                    {
                        tg.Assimilate();
                    }
                    else
                    {
                        tg.RollBack();
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message + "\n" + e.StackTrace);
                    tg.RollBack();
                }
            }

            //using (Transaction tra = new Transaction(doc, Language.ObtenerTexto(IdiomaDelPrograma, "Conf1")))
            //{
            //    tra.Start();

            //    frmConfiguraciones inicioConfiguraciones = new frmConfiguraciones(doc);

            //    inicioConfiguraciones.ShowDialog();

            //    if (inicioConfiguraciones.bandera)
            //    {
            //        tra.Commit();
            //    }
            //    else
            //    {
            //        tra.RollBack();
            //    }
            //}

            return Result.Succeeded;
        }
    }
}