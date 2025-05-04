using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Autodesk.Revit.DB;

namespace Jump
{
    public class Familia
    {
        private Int64 id;
        private string nombre;
        private string sistema;

        /// <summary> Crea un elemento de familia </summary>
        public Familia(Element elem)
        {
            Document doc = elem.Document;

            this.id = elem.Id.Value;
            this.nombre = elem.Name;

            if (elem is ElementType)
            {
                this.sistema = (elem as ElementType).FamilyName;
            }
            else
            {
                ElementType tipo = doc.GetElement(elem.GetTypeId()) as ElementType;

                if (tipo != null)
                {
                    this.sistema = tipo.FamilyName;
                }
                else
                {
                    this.sistema = null;
                }
            }
        }

        public Int64 ID
        {
            get { return id; }
            set { id = value; }
        }

        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }

        public string Sistema
        {
            get { return sistema; }
            set { sistema = value; }
        }

        public string NombreCompleto
        {
            get
            {
                if (this.sistema != null)
                {
                    return sistema + ": " + nombre;
                }
                else
                {
                    return nombre;
                }
            }
        }

        public string NombreCompletoID
        {
            get 
            {
                if (this.sistema != null)
                {
                    return sistema + ": " + nombre + " <" + id + ">";
                }
                else
                {
                    return nombre + " <" + id + ">";
                }
            }
        }
    }
}
