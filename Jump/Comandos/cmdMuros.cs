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
            Tools.CrearRegistroActualizadorArmaduras(uiApp.ActiveAddInId);

            using (TransactionGroup tra = new TransactionGroup(doc))
            {
                tra.Start();

                frmDetalleAutomatico Muro = new frmDetalleAutomatico(doc, uiDoc);
                
                Muro.clase = typeof(Wall);
                Muro.categoria = BuiltInCategory.OST_Walls;
                Muro.categoriaEtiqueta = BuiltInCategory.OST_StructuralFramingTags;
                Muro.indiceComboboxTextoBarra = Properties.Settings.Default.indiceComboboxTextoBarra;
                Muro.indiceComboboxEscalaVista = Properties.Settings.Default.MurosIndiceComboboxEscalaVista;
                Muro.posicionEtiquetaIndependienteElemento = Jump.Properties.Settings.Default.MurosEtiquetaIndependiente;
                Muro.posicionEtiquetaCotaProfundidad = Jump.Properties.Settings.Default.MurosEtiquetaCotaProfundidad;
                Muro.posicionEtiquetaIndependienteArmadura = Jump.Properties.Settings.Default.EtiquetaIndependienteArmadura;
                Muro.cotaVerticalIzquierda = true;
                Muro.cotaVerticalDerecha = true;
                Muro.cotaHorizontalArriba = true;
                Muro.cotaHorizontalAbajo = true;
                Muro.clave = "Mur";

                Muro.ShowDialog();

                // Guarda el indice en las configuraciones
                Properties.Settings.Default.indiceComboboxTextoBarra = Muro.indiceComboboxTextoBarra;
                Properties.Settings.Default.MurosIndiceComboboxEscalaVista = Muro.indiceComboboxEscalaVista;
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
