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
            Tools.CrearRegistroActualizadorArmaduras(uiApp.ActiveAddInId);

            using (TransactionGroup tra = new TransactionGroup(doc))
            {
                tra.Start();

                frmDetalleAutomatico Viga = new frmDetalleAutomatico(doc, uiDoc);

                Viga.clase = typeof(FamilyInstance);
                Viga.categoria = BuiltInCategory.OST_StructuralFraming;
                Viga.categoriaEtiqueta = BuiltInCategory.OST_StructuralFramingTags;
                Viga.indiceComboboxTextoBarra = Properties.Settings.Default.indiceComboboxTextoBarra;
                Viga.indiceComboboxEscalaVista = Properties.Settings.Default.VigasIndiceComboboxEscalaVista;
                Viga.posicionEtiquetaIndependienteElemento = Jump.Properties.Settings.Default.VigasEtiquetaIndependiente;
                Viga.posicionEtiquetaCotaProfundidad = Jump.Properties.Settings.Default.VigasEtiquetaCotaProfundidad;
                Viga.posicionEtiquetaIndependienteArmadura = Jump.Properties.Settings.Default.EtiquetaIndependienteArmadura;
                Viga.cotaVerticalIzquierda = true;
                Viga.cotaVerticalDerecha = true;
                Viga.cotaHorizontalArriba = true;
                Viga.cotaHorizontalAbajo = true;
                Viga.clave = "Vig";

                Viga.ShowDialog();

                // Guarda el indice en las configuraciones
                Properties.Settings.Default.indiceComboboxTextoBarra = Viga.indiceComboboxTextoBarra;
                Properties.Settings.Default.VigasIndiceComboboxEscalaVista = Viga.indiceComboboxEscalaVista;
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
