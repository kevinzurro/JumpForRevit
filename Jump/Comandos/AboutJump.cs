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
        static string guidEsquema = "704efbf5-1d59-41e1-a325-012784bb6a64";
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
