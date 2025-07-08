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

                DetalleAutomaticoModel model = new DetalleAutomaticoModel(uiApp);

                model.Clave = "Zap";
                model.Clase = typeof(FamilyInstance);
                model.Categoria = BuiltInCategory.OST_StructuralFoundation;
                model.CategoriaEtiqueta = BuiltInCategory.OST_StructuralFoundationTags;
                model.IndiceComboboxEscalaVista = Properties.Settings.Default.ZapataIndiceComboboxEscalaVista;
                model.PosicionEtiquetaIndependienteElemento = Jump.Properties.Settings.Default.ZapataEtiquetaIndependiente;
                model.PosicionEtiquetaCotaProfundidad = Jump.Properties.Settings.Default.ZapataCotaProfundidad;
                model.PosicionEtiquetaIndependienteArmadura = Jump.Properties.Settings.Default.ArmaduraEtiquetaIndependiente;
                model.CotaHorizontalArriba = Jump.Properties.Settings.Default.ZapataCotaLinealArriba;
                model.CotaHorizontalAbajo = Jump.Properties.Settings.Default.ZapataCotaLinealAbajo;
                model.CotaVerticalIzquierda = Jump.Properties.Settings.Default.ZapataCotaLinealIzquierda;
                model.CotaVerticalDerecha = Jump.Properties.Settings.Default.ZapataCotaLinealDerecha;

                DetalleAutomaticoViewModel zapataVM = new DetalleAutomaticoViewModel(model);

                WinDetalleAutomatico Zapata = new WinDetalleAutomatico();

                Zapata.DataContext = zapataVM;

                Zapata.ShowDialog();

                //frmDetalleAutomatico Zapata = new frmDetalleAutomatico(doc);

                //Zapata.clase = typeof(FamilyInstance);
                //Zapata.categoria = BuiltInCategory.OST_StructuralFoundation;
                //Zapata.categoriaEtiqueta = BuiltInCategory.OST_StructuralFoundationTags;
                //Zapata.indiceComboboxEscalaVista = Properties.Settings.Default.ZapataIndiceComboboxEscalaVista;
                //Zapata.posicionEtiquetaIndependienteElemento = Jump.Properties.Settings.Default.ZapataEtiquetaIndependiente;
                //Zapata.posicionEtiquetaCotaProfundidad = Jump.Properties.Settings.Default.ZapataCotaProfundidad;
                //Zapata.posicionEtiquetaIndependienteArmadura = Jump.Properties.Settings.Default.ArmaduraEtiquetaIndependiente;
                //Zapata.listaSeleccionados = uiDoc.Selection.GetElementIds().ToList();
                //Zapata.cotaHorizontalArriba = Jump.Properties.Settings.Default.ZapataCotaLinealArriba;
                //Zapata.cotaHorizontalAbajo = Jump.Properties.Settings.Default.ZapataCotaLinealAbajo;
                //Zapata.cotaVerticalIzquierda = Jump.Properties.Settings.Default.ZapataCotaLinealIzquierda;
                //Zapata.cotaVerticalDerecha = Jump.Properties.Settings.Default.ZapataCotaLinealDerecha;
                //Zapata.clave = "Zap";

                //Zapata.ShowDialog();

                // Guarda el indice en las configuraciones
                Properties.Settings.Default.ZapataIndiceComboboxEscalaVista = model.IndiceComboboxEscalaVista;
                Properties.Settings.Default.Save();

                if (Zapata.DialogResult == true)
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
