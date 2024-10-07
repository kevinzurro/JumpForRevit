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
using Jump.Views.Windows;
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

                    ConfigGeneralViewModel vmConfigGeneral = new ConfigGeneralViewModel(model);
                    ConfigEtiquetasViewModel vmConfigEtiquetas = new ConfigEtiquetasViewModel(model);

                    ConfiguracionViewModel vmConfiguraciones = new ConfiguracionViewModel(model, vmConfigGeneral, vmConfigEtiquetas);
                    //ConfiguracionViewModel vmConfiguraciones = new ConfiguracionViewModel();
                    //vmConfiguraciones.Modelo = model;
                    //vmConfiguraciones.VistaActual = vmConfigGeneral;
                    //vmConfiguraciones.GeneralVM = vmConfigGeneral;
                    //vmConfiguraciones.EtiquetasVM = vmConfigEtiquetas;

                    WinConfiguraciones Configuraciones = new WinConfiguraciones();
                    
                    Configuraciones.DataContext = vmConfiguraciones;

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

            return Result.Succeeded;
        }
    }
}