using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB.ExtensibleStorage;

namespace Jump
{
    class ArmaduraEliminacion : IUpdater
    {
        // Variable necesarias
        string IdiomaDelPrograma = Tools.ObtenerIdiomaDelPrograma();
        AddInId addinID = null;
        UpdaterId updaterID = null;

        // Contructor de la clase
        public ArmaduraEliminacion(AddInId AddID)
        {
            // Asigna el ID del complemento
            this.addinID = AddID;

            // Crea un nuevo UpdaterId
            this.updaterID = new UpdaterId(addinID, AboutJump.GuidEliminarBarra);
        }

        /// <summary> Método cada vez que existe alguna eliminación en Revit </summary>
        public void Execute(UpdaterData data)
        {
            // Documento del proyecto
            Document doc = data.GetDocument();

            // Obtiene todos los ID de los elementos eliminados
            List<ElementId> elementosEliminadosId = data.GetDeletedElementIds().ToList();

            // Compara el ID de los elementos y devuelve los coincidentes
            List<ArmaduraRepresentacion> lista = Inicio.listaArmaduraRepresentacion.Where(x => elementosEliminadosId.Any(y => y == x.Id)).ToList();

            // Recorre todos los despieces creados
            foreach (ArmaduraRepresentacion armadura in lista)
            {
                try
                {
                    // Elimina todo el despiece
                    armadura.Eliminar();
                }
                catch (Exception) { }
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
