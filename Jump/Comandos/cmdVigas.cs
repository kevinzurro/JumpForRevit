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

                frmDetalleAutomatico Viga = new frmDetalleAutomatico(doc);

                Viga.clase = typeof(FamilyInstance);
                Viga.categoria = BuiltInCategory.OST_StructuralFraming;
                Viga.categoriaEtiqueta = BuiltInCategory.OST_StructuralFramingTags;
                Viga.indiceComboboxEscalaVista = Properties.Settings.Default.VigaIndiceComboboxEscalaVista;
                Viga.posicionEtiquetaIndependienteElemento = Jump.Properties.Settings.Default.VigaEtiquetaIndependiente;
                Viga.posicionEtiquetaCotaProfundidad = Jump.Properties.Settings.Default.VigaCotaProfundidad;
                Viga.posicionEtiquetaIndependienteArmadura = Jump.Properties.Settings.Default.ArmaduraEtiquetaIndependiente;
                Viga.listaSeleccionados = uiDoc.Selection.GetElementIds().ToList();
                Viga.cotaHorizontalArriba = Jump.Properties.Settings.Default.VigaCotaLinealArriba;
                Viga.cotaHorizontalAbajo = Jump.Properties.Settings.Default.VigaCotaLinealAbajo;
                Viga.cotaVerticalIzquierda = Jump.Properties.Settings.Default.VigaCotaLinealIzquierda;
                Viga.cotaVerticalDerecha = Jump.Properties.Settings.Default.VigaCotaLinealDerecha;
                Viga.clave = "Vig";

                Viga.ShowDialog();

                // Guarda el indice en las configuraciones
                Properties.Settings.Default.VigaIndiceComboboxEscalaVista = Viga.indiceComboboxEscalaVista;
                Properties.Settings.Default.Save();

                if (Viga.bandera)
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
