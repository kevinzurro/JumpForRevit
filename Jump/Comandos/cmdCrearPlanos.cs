using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.ApplicationServices;
using Jump.Views;
using System.Globalization;
using System.Threading;
using System.Resources;
using System.Diagnostics;

namespace Jump
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class cmdCrearPlanos : IExternalCommand
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

            using (TransactionGroup tg = new TransactionGroup(doc, Language.ObtenerTexto(IdiomaDelPrograma, "CreaPlano4-1")))
            {
                tg.Start();
                try
                {
                    VistasModel mVistas = new VistasModel(uiApp);
                    mVistas.IdiomaDelPrograma = IdiomaDelPrograma;

                    CrearPlanosViewModel mvCrearPlanos = new CrearPlanosViewModel();
                    mvCrearPlanos.Modelo = mVistas;

                    WinCrearPlanos CrearPlanos = new WinCrearPlanos();

                    CrearPlanos.DataContext = mvCrearPlanos;

                    CrearPlanos.ShowDialog();

                    if (CrearPlanos.DialogResult == true)
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
                    tg.RollBack();
                }
            }

            return Result.Succeeded;
        }
    }
}
