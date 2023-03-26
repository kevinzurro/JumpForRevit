using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.DB.ExtensibleStorage;

namespace Jump
{
    public static class AboutJump
    {
        public static string NombreAddin = "Jump";
        static string version = "1.0";
        public const string nombreEsquema = "RebarRepresentationJumpForRevit";
        public const string nombreEsquemaArmaduras = "RepresentationsJumpForRevit";
        public static string armaduraRepresentacion = "RepresentacionDeArmaduras";
        static string guidEsquema = "2538c87f-9d93-422f-80c8-4c4acdbd634a";
        static string guidEliminarBarra = "266bb163-66e6-4cf8-9ceb-54d931521116";
        static string guidActualizarBarra = "7907bd48-73fd-457d-acf9-5311d6b2c0f8";

        /// <summary> Obtiene la versión de la addin </summary>
        public static string Version
        {
            get { return version; }
        }

        public static Guid GuidEsquema
        {
            get { return new Guid(guidEsquema); }
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
}
