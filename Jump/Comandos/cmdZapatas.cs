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
    class cmdZapatas : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // Variables necesarias
            UIApplication uiApp = commandData.Application;
            UIDocument uiDoc = uiApp.ActiveUIDocument;
            Application app = uiApp.Application;
            Document doc = uiDoc.Document;

            Tools.AddinManager();

            using (TransactionGroup tra = new TransactionGroup(doc))
            {
                tra.Start();

                frmDetalleAutomatico Zapata = new frmDetalleAutomatico(doc, uiDoc);

                Zapata.clase = typeof(FamilyInstance);
                Zapata.categoria = BuiltInCategory.OST_StructuralFoundation;
                Zapata.categoriaEtiqueta = BuiltInCategory.OST_StructuralFoundationTags;
                Zapata.indiceComboboxEscalaVista = Properties.Settings.Default.ZapataIndiceComboboxEscalaVista;
                Zapata.posicionEtiquetaIndependienteElemento = Jump.Properties.Settings.Default.ZapataEtiquetaIndependiente;
                Zapata.posicionEtiquetaCotaProfundidad = Jump.Properties.Settings.Default.ZapataEtiquetaCotaProfundidad;
                Zapata.posicionEtiquetaIndependienteArmadura = Jump.Properties.Settings.Default.ArmaduraEtiquetaIndependiente;
                Zapata.cotaHorizontalArriba = Jump.Properties.Settings.Default.ZapataCotaLinealArriba;
                Zapata.cotaHorizontalAbajo = Jump.Properties.Settings.Default.ZapataCotaLinealAbajo;
                Zapata.cotaVerticalIzquierda = Jump.Properties.Settings.Default.ZapataCotaLinealIzquierda;
                Zapata.cotaVerticalDerecha = Jump.Properties.Settings.Default.ZapataCotaLinealDerecha;
                Zapata.clave = "Zap";

                Zapata.ShowDialog();

                // Guarda el indice en las configuraciones
                Properties.Settings.Default.ZapataIndiceComboboxEscalaVista = Zapata.indiceComboboxEscalaVista;
                Properties.Settings.Default.Save();

                if (Zapata.bandera)
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
