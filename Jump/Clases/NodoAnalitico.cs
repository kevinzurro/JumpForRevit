using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jump
{
    public class NodoAnalitico
    {
        private Int64 id;
        private XYZ xyz;
        private ReferencePoint punto;
        private ObservableCollection<NodoAnalitico> nodosCercanos = new ObservableCollection<NodoAnalitico>();

        public NodoAnalitico(ReferencePoint punto)
        {
            ID = punto.Id.Value;
            XYZ = punto.Position;
            Punto = punto;
        }

        public Int64 ID
        {
            get { return this.id; }
            set { this.id = value; }
        }

        public XYZ XYZ
        {
            get { return this.xyz; }
            set { this.xyz = value; }
        }

        public string Nombre
        {
            get { return ("ID: " + ID.ToString()); }
        }

        public string NombreYPosicion
        {
            get { return (Nombre + " " + XYZ.ToString()); }
        }

        public ReferencePoint Punto
        {
            get { return this.punto; }
            set { this.punto = value; }
        }

        public ObservableCollection<NodoAnalitico> NodosCercanos
        {
            get { return this.nodosCercanos; }
            set { this.nodosCercanos = value; }
        }

    }
}
