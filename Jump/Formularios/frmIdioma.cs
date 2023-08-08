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
using Jump.Properties;
using System.IO;
using System.Resources;

namespace Jump
{
    public partial class frmIdioma : System.Windows.Forms.Form
    {
        // Variable necesarias
        string IdiomaDelPrograma;
        
        public frmIdioma()
        {            
            InitializeComponent();

            Tools.AddinManager();

            // Variable necesarias
            this.IdiomaDelPrograma = Tools.ObtenerIdiomaDelPrograma();

            // Llama a las funciones
            CargarIdiomasYPaises();
        }

        ///<summary> Carga los idiomas y paises </summary>
        private void CargarIdiomasYPaises()
        {
            // Carga los paises en el vector
            Pais.CargarPaisesDisponibles();

            // Verifica que los idiomas disponibles estén cargados
            Tools.CargarIdiomas();

            // Devuelve los idiomas disponibles
            foreach (string i in Language.IdiomasDisponibles)
            {
                lstIdioma.Items.Add(i);
            }

            // Devuelve los paises disponibles
            foreach (string a in Pais.PaisesDisponibles)
            {
                lstPaises.Items.Add(a);
            }
        }

        ///<summary> Carga el formulario </summary>
        private void frmIdioma_Load(object sender, EventArgs e)
        {
            // Asignación de textos según el idioma
            this.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Idi4");
            btnAceptar.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Idi5");
            btnCancelar.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Idi6");
            txtTituloIdioma.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Idi1-1");
            gbxSeleccionIdioma.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Idi1-2");
        }

        ///<summary> Cambia el idioma por el seleccionado </summary>
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (lstIdioma.SelectedItem != null)
            {
                // Guarda el idioma seleccionado
                Properties.Settings.Default["IdiomaDelPrograma"] = lstIdioma.SelectedItem;
                Properties.Settings.Default.Save();
                Close();
            }          
        }

        ///<summary> Cambia el picturebox según el pais </summary>
        private void lstPaises_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Busca el nombre del pais seleccionado
            string imag = lstPaises.Text;
            string imagen;

            // Cambia los espacios por guion bajo
            try
            {
                imagen = imag.Replace(" ", "_");
            }
            catch (Exception)
            {
                imagen = imag;
            }

            // Busca el recurso en la carpeta
            ResourceManager rm = Iconos_e_Imagenes.Banderas.ResourceManager;

            // Crea un bitmap con el objeto según el pais seleccionado
            Bitmap bandera = (Bitmap)rm.GetObject(imagen);

            // Asigna el bitmap al picturebox
            pcbxBandera.Image =  bandera;
        }

        /// <summary> Cierra el formulario cuando se presiona la tecla Esc </summary>
        private void frmCerrar_KeyDown(object sender, KeyEventArgs e)
        {
            // Verifica que sea la tecla Esc
            if (e.KeyCode == Keys.Escape)
            {
                // Cierra el formulario
                this.Close();
            }
        }

        /// <summary> Cierra el formulario </summary>
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
