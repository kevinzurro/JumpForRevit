using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.ApplicationServices;
using Jump.Models;
using Jump.ViewModels;
using Jump.Views.Windows;
using System.Diagnostics;

namespace Jump
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    class cmdNodosAnaliticosVerificacion : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // Variables necesarias
            UIApplication uiApp = commandData.Application;
            UIDocument uiDoc = uiApp.ActiveUIDocument;
            Application app = uiApp.Application;
            Document doc = uiDoc.Document;

            Tools.AddinManager();

            using (TransactionGroup tg = new TransactionGroup(doc, Language.ObtenerTexto(AboutJump.IdiomaAddin, "NodAnaVer1-1")))
            {
                tg.Start();

                try
                {
                    NodoAnaliticoModel mNodoAnalitico = new NodoAnaliticoModel(uiApp);

                    NodoAnaliticoVerificacionViewModel vmNodoAnalitico = new NodoAnaliticoVerificacionViewModel(mNodoAnalitico);

                    WinNodoAnaliticoVerificacion VerificarNodos = new WinNodoAnaliticoVerificacion(vmNodoAnalitico);

                    VerificarNodos.ShowDialog();

                    if (VerificarNodos.DialogResult == true)
                    {
                        tg.Assimilate();
                    }
                    else
                    {
                        tg.RollBack();
                    }
                }
                catch (Exception)
                {
                    tg.RollBack();
                }
            }

            return Result.Succeeded;
        }
    }
}
