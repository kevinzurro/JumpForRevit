using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Resources;
using System.Reflection;
using System.Globalization;
using System.Diagnostics;
using System.Windows.Controls;
using System.Threading;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.ApplicationServices;
using Jump.Models;
using Jump.ViewModels;
using Jump.Views.Windows;

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

                    ConfiguracionViewModel vmConfiguraciones = new ConfiguracionViewModel(model);

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

                    Configuraciones.Close();
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