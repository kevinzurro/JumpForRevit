using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.DB.ExtensibleStorage;
using Revit.ES.Extension;
using Revit.ES.Extension.Attributes;
using Revit.ES.Extension.ElementExtensions;

namespace Jump
{
    public static class AboutJump
    {
        public static string NombreAddin = "Jump";
        static string version = "1.0";
        static string idiomaDelPrograma = "Español";
        public const string nombreEsquemaArmaduraRepresentacion = "RebarRepresentationJumpForRevit";
        public const string nombreEsquemaRepresentaciones = "RepresentationsJumpForRevit";
        public const string nombreEsquemaDataGridView = "DGVDiameterAndStyleJumpForRevit";
        public const string nombreDataStorageDGV = "DGVDataStorageJumpForRevit";
        public const string guidArmaduraRepresentacionEntity = "ed2d5175-56e3-42a2-a433-aedfda1ccedd";
        public const string guidRepresentacionesEntity = "b8e4e02b-3886-4918-84f0-d8df893fc5a1";
        public const string guidDGVEntity = "e56579f8-6c73-4fcc-ab1f-c545d2d01163";
        public const string guidDataStorageDGV = "0020d702-4314-410a-bb89-4ea08707fab4";
        const string guidEliminarBarra = "266bb163-66e6-4cf8-9ceb-54d931521116";
        const string guidActualizarBarra = "7907bd48-73fd-457d-acf9-5311d6b2c0f8";

        // DataGridView de diámetros y estilos de líneas
        public const string nombreColumnaDiametros = "Diametro";
        public const string nombreColumnaEstilosLineas = "EstiloLinea";

        // DataGridView de diámetros y estilos de líneas
        public const string parametroMostrarUsuario = "Name";
        public const string parametroId = "Id";

        /// <summary> Obtiene o establece el idioma del plugin </summary>
        public static string Idioma
        {
            get { return idiomaDelPrograma; }

            set{ idiomaDelPrograma = value; }
        }

        /// <summary> Obtiene la versión de la addin </summary>
        public static string Version
        {
            get { return version; }
        }

        public static Guid GuidEliminarBarra
        {
            get { return new Guid(guidEliminarBarra); }
        }

        public static Guid GuidActualizarBarra
        {
            get { return new Guid(guidActualizarBarra); }
        }
    }

    [Schema(AboutJump.guidArmaduraRepresentacionEntity, AboutJump.nombreEsquemaArmaduraRepresentacion)]
    public class ArmaduraRepresentacionEntity : IRevitEntity
    {
        [Field]
        public ElementId Vista { get; set; }

        [Field]
        public List<ElementId> ListaCurvas { get; set; }

        [Field]
        public List<ElementId> ListaTextos { get; set; }

        [Field]
        public ElementId TipoDeTexto { get; set; }

        [Field]
        public ElementId Etiqueta { get; set; }

        [Field(Documentation = "XYZ Position", UnitType = UnitType.UT_Length)]
        public XYZ Posicion { get; set; }
    }

    [Schema(AboutJump.guidRepresentacionesEntity, AboutJump.nombreEsquemaRepresentaciones)]
    public class RepresentacionesEntity : IRevitEntity
    {
        [Field]
        public List<ArmaduraRepresentacionEntity> ListaRepresentaciones { get; set; }
    }

    [Schema(AboutJump.guidDGVEntity, AboutJump.nombreEsquemaDataGridView)]
    public class DGVEntity : IRevitEntity
    {
        [Field]
        public Dictionary<ElementId, ElementId> DGVDiametrosYEstilos { get; set; }
    }
}
