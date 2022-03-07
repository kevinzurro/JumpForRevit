using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.Structure;

namespace Jump
{
    public partial class frmOrdenYEnumeracion : System.Windows.Forms.Form
    {
        // Variable necesarias
        string IdiomaDelPrograma;
        Document doc;
        UIDocument uiDoc;

        // Parámetros generales
        List<Element> listaZapatas = new List<Element>();
        List<Element> elementos = new List<Element>();
        List<Parameter> parametrosEjemplar = new List<Parameter>();

        // Constructor del formulario
        public frmOrdenYEnumeracion(Document doc, UIDocument uiDoc)
        {
            InitializeComponent();

            Tools.AddinManager();

            // Variable necesarias
            this.IdiomaDelPrograma = Tools.ObtenerIdiomaDelPrograma();
            this.doc = doc;
            this.uiDoc = uiDoc;
        }
    }
}
