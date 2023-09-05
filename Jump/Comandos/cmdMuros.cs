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
    public class cmdMuros : IExternalCommand
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

                frmDetalleAutomatico Muro = new frmDetalleAutomatico(doc, uiDoc);
                
                Muro.clase = typeof(Wall);
                Muro.categoria = BuiltInCategory.OST_Walls;
                Muro.categoriaEtiqueta = BuiltInCategory.OST_StructuralFramingTags;
                Muro.indiceComboboxEscalaVista = Properties.Settings.Default.MuroIndiceComboboxEscalaVista;
                Muro.posicionEtiquetaIndependienteElemento = Jump.Properties.Settings.Default.MuroEtiquetaIndependiente;
                Muro.posicionEtiquetaCotaProfundidad = Jump.Properties.Settings.Default.MuroEtiquetaCotaProfundidad;
                Muro.posicionEtiquetaIndependienteArmadura = Jump.Properties.Settings.Default.ArmaduraEtiquetaIndependiente;
                Muro.cotaHorizontalArriba = Jump.Properties.Settings.Default.MuroCotaLinealArriba;
                Muro.cotaHorizontalAbajo = Jump.Properties.Settings.Default.MuroCotaLinealAbajo;
                Muro.cotaVerticalIzquierda = Jump.Properties.Settings.Default.MuroCotaLinealIzquierda;
                Muro.cotaVerticalDerecha = Jump.Properties.Settings.Default.MuroCotaLinealDerecha;
                Muro.clave = "Mur";

                Muro.ShowDialog();

                // Guarda el indice en las configuraciones
                Properties.Settings.Default.MuroIndiceComboboxEscalaVista = Muro.indiceComboboxEscalaVista;
                Properties.Settings.Default.Save();

                if (Muro.bandera)
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
