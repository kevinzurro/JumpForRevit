using System;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;


namespace Jump
{
    public class ObjetoRevit
    {
        private Int64 id;
        private string nombre;
        private bool esModificable = true;
        private bool esVisible = true;
        private string tipo;

        /// <summary> Cualquier objeto de revit </summary>
        public ObjetoRevit(object obj)
        {
            if (obj is Category cat)
            {
                ID = cat.Id.Value;
                Nombre = cat.Name;
                EsModificable = !cat.IsReadOnly;
            }

            if (obj is Parameter param)
            {
                ID = param.Id.Value;
                Nombre = param.Definition.Name;
                EsModificable = !param.IsReadOnly;
                EsVisible = (param.Definition as InternalDefinition).Visible;
                Tipo = param.StorageType.ToString();
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

        public bool EsModificable
        {
            get { return esModificable; }
            set { esModificable = value; }
        }

        public bool EsVisible
        {
            get { return esVisible; }
            set { esVisible = value; }
        }

        public string Tipo
        {
            get { return tipo; }
            set { tipo = value; }
        }
    }
}
