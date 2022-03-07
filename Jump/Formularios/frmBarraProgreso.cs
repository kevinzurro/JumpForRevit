using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Jump
{
    public partial class frmBarraProgreso : System.Windows.Forms.Form
    {
        // Variable necesarias
        string IdiomaDelPrograma;
        int total;
        int contador = 0;
        string texto = null;

        public frmBarraProgreso(int totalElementos)
        {
            InitializeComponent();

            Tools.AddinManager();

            // Variable necesarias
            this.IdiomaDelPrograma = Tools.ObtenerIdiomaDelPrograma();
            this.total = totalElementos;

            // Resetea las variables a cero
            this.pbrBarraProgreso.Value = 0;
            this.pbrBarraProgreso.Maximum = total;

            // Llama a la función de cambiar el texto
            CambiarTexto();

            // Muestra el formulario
            Show();
            Application.DoEvents();
        }

        ///<summary> Cambia el texto de procesando </summary>
        private void CambiarTexto()
        {
            // Cambia el texto
            this.texto = Language.ObtenerTexto(IdiomaDelPrograma, "BarPro1") + contador.ToString()
                       + Language.ObtenerTexto(IdiomaDelPrograma, "BarPro2") + total.ToString();
            lblProgreso.Text = texto;
        }

        ///<summary> incrementa la barra de progreso </summary>
        public void Incrementar()
        {
            // Incrementa el contador
            ++contador;

            // Cambia el texto
            CambiarTexto();

            // Incrementa la barra de progreso
            pbrBarraProgreso.Value = contador;
            Application.DoEvents();
        }
    }
}
