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
    class ArmaduraEliminacion : IUpdater
    {
        // Variable necesarias
        string IdiomaDelPrograma = Tools.ObtenerIdiomaDelPrograma();
        AddInId addinID = null;
        UpdaterId updaterID = null;

        // Guid para eliminar los despieces
        string GuidEliminarBarra = "266bb163-66e6-4cf8-9ceb-54d931521116";

        // Contructor de la clase
        public ArmaduraEliminacion(AddInId AddID)
        {
            // Asigna el ID del complemento
            this.addinID = AddID;

            // Crea un nuevo UpdaterId
            this.updaterID = new UpdaterId(addinID, new Guid(GuidEliminarBarra));
        }

        /// <summary> Método cada vez que existe alguna eliminación en Revit </summary>
        public void Execute(UpdaterData data)
        {
            // Documento del proyecto
            Document doc;

            // Obtiene el documento
            doc = data.GetDocument();

            // Obtiene todos los ID de los elementos eliminados
            List<ElementId> elementosId = data.GetDeletedElementIds().ToList();

            // Obtiene todas las barras del proyecto
            List<Element> colectorBarras = new FilteredElementCollector(doc).
                                                OfCategory(BuiltInCategory.OST_Rebar).
                                                OfClass(typeof(Rebar)).ToList();

            // Obtiene todas ID de las barras del proyecto
            List<ElementId> colectorBarrasId = Tools.ObtenerIdElemento(colectorBarras);

            // Obtiene todas las barras eliminadas
            List<ElementId> barrasEliminadas = Tools.ObtenerElementosIDCoincidentesConLista(colectorBarrasId, elementosId);

            // Recorre todas los despieces creados
            foreach (ArmaduraRepresentacion bar in Inicio.listaArmaduraRepresentacion)
            {
                try
                {
                    // Verifica que entre las barras eliminadas haya algun despiece
                    if (barrasEliminadas.Contains(bar.Barra.Id))
                    {
                        // Elimina todo el despiece
                        bar.Eliminar();
                    }
                }
                catch (Exception)
                {
                    // Elimina el despiece
                    bar.Eliminar();
                }
            }
        }

        /// <summary> Devuelve el UpdaterId </summary>
        public UpdaterId GetUpdaterId()
        {
            return updaterID;
        }

        /// <summary> Tipo del cambio que realizará el actualizador </summary>
        public ChangePriority GetChangePriority()
        {
            // Tipo de prioridad
            return ChangePriority.Rebar;
        }

        /// <summary> Obtiene la información adicional del actualizador </summary>
        public string GetAdditionalInformation()
        {
            // Información del actualizador
            string info = Language.ObtenerTexto(IdiomaDelPrograma, "EliBar1");

            return info;
        }

        /// <summary> Obtiene el nombre del actualizador </summary>
        public string GetUpdaterName()
        {
            // Nombre del actualizador
            string nombre = Language.ObtenerTexto(IdiomaDelPrograma, "EliBar2");

            return nombre;
        }
    }
}
