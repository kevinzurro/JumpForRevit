using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.ApplicationServices;
using Jump.Views.Windows;
using Jump.Models;
using Jump.ViewModels;
using System.Diagnostics;

namespace Jump
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class cmdVigas : IExternalCommand
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

                model.Clave = "Vig";
                model.Clase = typeof(FamilyInstance);
                model.Categoria = BuiltInCategory.OST_StructuralFraming;
                model.CategoriaEtiqueta = BuiltInCategory.OST_StructuralFramingTags;
                model.IndiceComboboxEscalaVista = Properties.Settings.Default.VigaIndiceComboboxEscalaVista;
                model.PosicionEtiquetaIndependienteElemento = Jump.Properties.Settings.Default.VigaEtiquetaIndependiente;
                model.PosicionEtiquetaCotaProfundidad = Jump.Properties.Settings.Default.VigaCotaProfundidad;
                model.PosicionEtiquetaIndependienteArmadura = Jump.Properties.Settings.Default.ArmaduraEtiquetaIndependiente;
                model.CotaHorizontalArriba = Jump.Properties.Settings.Default.VigaCotaLinealArriba;
                model.CotaHorizontalAbajo = Jump.Properties.Settings.Default.VigaCotaLinealAbajo;
                model.CotaVerticalIzquierda = Jump.Properties.Settings.Default.VigaCotaLinealIzquierda;
                model.CotaVerticalDerecha = Jump.Properties.Settings.Default.VigaCotaLinealDerecha;

                DetalleAutomaticoViewModel vigaVM = new DetalleAutomaticoViewModel(model);

                WinDetalleAutomatico Viga = new WinDetalleAutomatico();

                Viga.DataContext = vigaVM;

                Viga.ShowDialog();

                //// Guarda el indice en las configuraciones
                Properties.Settings.Default.VigaIndiceComboboxEscalaVista = model.IndiceComboboxEscalaVista;
                Properties.Settings.Default.Save();

                if (Viga.DialogResult == true)
                {
                    tra.Commit();
                }
                else
                {
                    tra.RollBack();
                }

                //frmDetalleAutomatico Viga = new frmDetalleAutomatico(doc);

                //Viga.clave = "Vig";
                //Viga.clase = typeof(FamilyInstance);
                //Viga.categoria = BuiltInCategory.OST_StructuralFraming;
                //Viga.categoriaEtiqueta = BuiltInCategory.OST_StructuralFramingTags;
                //Viga.indiceComboboxEscalaVista = Properties.Settings.Default.VigaIndiceComboboxEscalaVista;
                //Viga.posicionEtiquetaIndependienteElemento = Jump.Properties.Settings.Default.VigaEtiquetaIndependiente;
                //Viga.posicionEtiquetaCotaProfundidad = Jump.Properties.Settings.Default.VigaCotaProfundidad;
                //Viga.posicionEtiquetaIndependienteArmadura = Jump.Properties.Settings.Default.ArmaduraEtiquetaIndependiente;
                //Viga.cotaHorizontalArriba = Jump.Properties.Settings.Default.VigaCotaLinealArriba;
                //Viga.cotaHorizontalAbajo = Jump.Properties.Settings.Default.VigaCotaLinealAbajo;
                //Viga.cotaVerticalIzquierda = Jump.Properties.Settings.Default.VigaCotaLinealIzquierda;
                //Viga.cotaVerticalDerecha = Jump.Properties.Settings.Default.VigaCotaLinealDerecha;

                //Viga.ShowDialog();

                //// Guarda el indice en las configuraciones
                //Properties.Settings.Default.VigaIndiceComboboxEscalaVista = Viga.indiceComboboxEscalaVista;
                //Properties.Settings.Default.Save();

                //if (Viga.bandera)
                //{
                //    tra.Commit();
                //}
                //else
                //{
                //    tra.RollBack();
                //}
            }

            return Result.Succeeded;
        }
    }
}
