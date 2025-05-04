using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jump
{
    class Nodo
    {
        ReferencePoint punto;
        List<ReferencePoint> puntos = new List<ReferencePoint>();

        public ReferencePoint Punto
        {
            get { return this.punto; }
            set { this.punto = value; }
        }

        public List<ReferencePoint> Puntos
        {
            get { return this.puntos; }
            set { this.puntos = value; }
        }
    }
}
