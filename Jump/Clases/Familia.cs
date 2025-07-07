using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Autodesk.Revit.DB;

namespace Jump
{
    public class Familia
    {
        private Document doc;
        private Int64 id;
        private string nombre;
        private string sistema;

        /// <summary> Crea un elemento de familia </summary>
        public Familia(Element elem)
        {
            this.doc = elem.Document;

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

        /// <summary> Obtiene el Elemento desde la familia </summary>
        public static Element ObtenerElemento(Familia familia)
        {
            Element elem = familia.doc.GetElement(new ElementId(familia.ID));

            return elem;
        }

        /// <summary> Obtiene una lista de elementos desde una lista de familias </summary>
        public static List<Element> ObtenerElemento(List<Familia> familias)
        {
            List<Element> lista = new List<Element>();

            foreach (Familia fa in familias)
            {
                Element elem = Familia.ObtenerElemento(fa);

                if (elem != null)
                {
                    lista.Add(elem);
                }
            }

            return lista;
        }

        /// <summary> Obtiene una colección observable de familias con los elementos de revit </summary>
        public static ObservableCollection<Familia> ObtenerFamilia(List<Element> elementos)
        {
            ObservableCollection<Familia> familias = new ObservableCollection<Familia>();

            foreach (Element elem in elementos)
            {
                familias.Add(new Familia(elem));
            }

            return familias;
        }

    }
}
