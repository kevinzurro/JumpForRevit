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

            frmDetalleAutomatico Zapata = new frmDetalleAutomatico(doc, uiDoc);

            Zapata.clase = typeof(FamilyInstance);
            Zapata.categoria = BuiltInCategory.OST_StructuralFoundation;
            Zapata.categoriaEtiqueta = BuiltInCategory.OST_StructuralFoundationTags;
            Zapata.indiceComboboxTextoBarra = Properties.Settings.Default.zapataIndiceComboboxTextoBarra;
            Zapata.indiceComboboxEscalaVista = Properties.Settings.Default.zapataIndiceComboboxEscalaVista;
            Zapata.posicionEtiquetaIndependienteElemento = Jump.Properties.Settings.Default.EtiquetaIndependienteZapatas;
            Zapata.posicionEtiquetaCotaProfundidad = Jump.Properties.Settings.Default.EtiquetaCotaProfundidad;
            Zapata.posicionEtiquetaIndependienteArmadura = Jump.Properties.Settings.Default.EtiquetaIndependienteArmadura;
            Zapata.clave = "Zap";

            Zapata.ShowDialog();

            // Guarda el indice en las configuraciones
            Properties.Settings.Default.zapataIndiceComboboxTextoBarra = Zapata.indiceComboboxTextoBarra;
            Properties.Settings.Default.zapataIndiceComboboxEscalaVista = Zapata.indiceComboboxEscalaVista;
            Properties.Settings.Default.Save();

            return Result.Succeeded;
        }
    }
}
