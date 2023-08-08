using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.UI.Selection;

namespace Jump
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class cmdColumnas : IExternalCommand
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

                frmDetalleAutomatico Columna = new frmDetalleAutomatico(doc, uiDoc);

                Columna.clase = typeof(FamilyInstance);
                Columna.categoria = BuiltInCategory.OST_StructuralColumns;
                Columna.categoriaEtiqueta = BuiltInCategory.OST_StructuralColumnTags;
                Columna.indiceComboboxTextoBarra = Properties.Settings.Default.indiceComboboxTextoBarra;
                Columna.indiceComboboxEscalaVista = Properties.Settings.Default.indiceComboboxEscalaVistaColumna;
                Columna.posicionEtiquetaIndependienteElemento = Jump.Properties.Settings.Default.EtiquetaIndependienteColumnas;
                Columna.posicionEtiquetaCotaProfundidad = Jump.Properties.Settings.Default.EtiquetaCotaProfundidadColumnas;
                Columna.posicionEtiquetaIndependienteArmadura = Jump.Properties.Settings.Default.EtiquetaIndependienteArmadura;
                Columna.cotaVerticalIzquierda = true;
                Columna.cotaVerticalDerecha = true;
                Columna.cotaHorizontalArriba = true;
                Columna.cotaHorizontalAbajo = true;
                Columna.clave = "Col";

                Columna.ShowDialog();

                // Guarda el indice en las configuraciones
                Properties.Settings.Default.indiceComboboxTextoBarra = Columna.indiceComboboxTextoBarra;
                Properties.Settings.Default.indiceComboboxEscalaVistaColumna = Columna.indiceComboboxEscalaVista;
                Properties.Settings.Default.Save();

                if (Columna.bandera)
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
