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
    public partial class frmDetalleArmadura : System.Windows.Forms.Form
    {
        // Variable necesarias
        string IdiomaDelPrograma;
        Document doc;
        List<FamilySymbol> etiquetasArmaduras = new List<FamilySymbol>();
        List<TextNoteType> etiquetasLongitud = new List<TextNoteType>();
        public bool Longitud;
        public bool Armadura;
        public TextNoteType tipoTexto;
        public FamilySymbol tipoEtiqueta;
        public bool banderaCierre = false;

        // Parámetros para las etiquetas y vistas
        BuiltInCategory categoriaEtiquetaArmadura = BuiltInCategory.OST_RebarTags;

        // Constructor del formulario
        public frmDetalleArmadura(Document doc)
        {
            InitializeComponent();

            Tools.AddinManager();

            // Variable necesarias
            this.IdiomaDelPrograma = Tools.ObtenerIdiomaDelPrograma();
            this.doc = doc;

            // Asignación de textos según el idioma
            this.chbEtiquetaArmadura.Text = Language.ObtenerTexto(IdiomaDelPrograma, "DetArm1-1");
            this.chbEtiquetaLongitud.Text = Language.ObtenerTexto(IdiomaDelPrograma, "DetArm1-2");

            // Agrega los tipos de etiquetas
            this.etiquetasArmaduras.AddRange(Tools.ObtenerEtiquetasIndependientes(doc, categoriaEtiquetaArmadura));
            this.etiquetasLongitud.AddRange(Tools.ObtenerEstilosTexto(doc));

            // Rellena el combobox
            Tools.RellenarCombobox(this.cmbEtiquetaArmadura, etiquetasArmaduras);
            Tools.RellenarCombobox(this.cmbEtiquetaLongitud, etiquetasLongitud);
        }

        /// <summary> Cierra el formulario </summary>
        private void frmDetalleArmadura_KeyDown(object sender, KeyEventArgs e)
        {
            // Verifica que sea la tecla Esc
            if (e.KeyCode == Keys.Escape)
            {
                // Cierra el formulario
                this.Close();
            }
        }

        /// <summary> Ejecuta todas las acciones </summary>
        private void btnEjecutar_Click(object sender, EventArgs e)
        {
            this.Armadura = this.chbEtiquetaArmadura.Checked;
            this.Longitud = this.chbEtiquetaLongitud.Checked;

            this.tipoEtiqueta = (FamilySymbol)this.cmbEtiquetaArmadura.SelectedItem;
            this.tipoTexto = (TextNoteType)this.cmbEtiquetaLongitud.SelectedItem;

            this.banderaCierre = true;

            this.Close();
        }
    }
}
