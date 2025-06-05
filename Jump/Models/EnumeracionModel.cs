using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.UI.Events;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Diagnostics;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;

namespace Jump.Models
{
    public class EnumeracionModel : ModelBase
    {
        private ObservableCollection<ObjetoRevit> todasCategorias = new ObservableCollection<ObjetoRevit>();

        public EnumeracionModel(UIApplication UIApp)
        {
            this.UIApp = UIApp;
            this.UIDoc = UIApp.ActiveUIDocument;
            this.Appli = UIApp.Application;
            this.Doc = UIDoc.Document;

            // Obtiene todas las categorías del documento
            Categories categorias = this.Doc.Settings.Categories;

            foreach (Category cat in categorias)
            {
                ObjetoRevit or = new ObjetoRevit(cat);

                todasCategorias.Add(or);
            }

            todasCategorias = new ObservableCollection<ObjetoRevit>(todasCategorias.OrderBy(x => x.Nombre));
        }

        public ObservableCollection<ObjetoRevit> ObtenerCategorias()
        {
            return todasCategorias;
        }

        public ObservableCollection<ObjetoRevit> ObtenerParametrosDeCategoria(ObjetoRevit categoria)
        {
            Category cat = Category.GetCategory(this.Doc, new ElementId(categoria.ID));

            List<Parameter> todosParametros = Tools.ObtenerParametrosEjemplar(this.Doc, cat.BuiltInCategory);

            ObservableCollection<ObjetoRevit> parametros = new ObservableCollection<ObjetoRevit>();

            foreach (Parameter param in todosParametros)
            {
                ObjetoRevit or = new ObjetoRevit(param);

                parametros.Add(or);
            }

            return parametros;
        }

        public ObservableCollection<Familia> ObtenerTodosLosElementosDeCategoria(ObjetoRevit categoria)
        {
            Category cat = Category.GetCategory(this.Doc, new ElementId(categoria.ID));

            List<Element> lista = Tools.ObtenerTodosEjemplaresSegunCategoria(this.Doc, cat.BuiltInCategory);

            ObservableCollection<Familia> elementos = new ObservableCollection<Familia>();

            foreach (Element elem in lista)
            {
                Familia fa = new Familia(elem);

                elementos.Add(fa);
            }

            return elementos;
        }
    }
}
