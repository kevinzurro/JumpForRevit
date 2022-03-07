using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
    class cmdVisibilidadAnalitica : IExternalCommand
    {
        // Crear los parámetro de la vista actual
        View vistaActual;
        string IdiomaDelPrograma;

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // Variables necesarias
            UIApplication uiApp = commandData.Application;
            UIDocument uiDoc = uiApp.ActiveUIDocument;
            Application app = uiApp.Application;
            Document doc = uiDoc.Document;

            Tools.AddinManager();

            // Crear los parámetro de la vista actual
            this.vistaActual = doc.ActiveView;
            this.IdiomaDelPrograma = Tools.ObtenerIdiomaDelPrograma();

            // Llama a la función de visibilidad
            VisibilidadAnalitica(doc);

            return Result.Succeeded;
        }

        /// <summary> Muestra los elementos analíticos en la vista actual </summary>
        private void VisibilidadAnalitica(Document doc)
        {
            // Crea una lista de elementos a apagar y prender la visibilidad
            List<Element> todo = Tools.ObtenerTodosEjemplares(doc);
            List<Element> analitico = Tools.ObtenerTodosEjemplaresAnaliticos(doc); ;

            // Empieza la transacción
            using (Transaction t = new Transaction(doc, Language.ObtenerTexto(IdiomaDelPrograma, "EleAna1")))
            {
                t.Start();

                // Llama al método que cambia la visibilidad de los elementos
                Tools.OcultarElementosVista(doc, vistaActual, todo);
                Tools.MostrarElementosVista(doc, vistaActual, analitico);

                // Verifica que la categoría de modelos analíticos esté prendida
                if (vistaActual.AreAnalyticalModelCategoriesHidden)
                {
                    vistaActual.AreAnalyticalModelCategoriesHidden = false;
                }

                t.Commit();
            };
        }
    }
}
