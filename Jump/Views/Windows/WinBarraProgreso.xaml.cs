using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Jump;

namespace Jump.Views.Windows
{
    public partial class WinBarraProgreso : Window
    {
        int total;
        int contador = 0;
        bool cancelado = false;

        public WinBarraProgreso(int totalElementos)
        {
            InitializeComponent();
            DataContext = this;

            this.total = totalElementos;

            this.pbrBarra.Value = 0;
            this.pbrBarra.Maximum = total;

            CambiarTexto();
        }

        public bool Cancelado
        { 
            get { return cancelado; }
            set { cancelado = value; }
        }

        public bool Incrementar()
        {
            this.contador++;

            this.pbrBarra.Value = contador;

            CambiarTexto();

            return Cancelado;
        }
        
        private void CambiarTexto()
        {
            this.txtTexto.Text = Jump.Language.ObtenerTexto(AboutJump.IdiomaAddin, "BarPro1") + contador.ToString()
                + Jump.Language.ObtenerTexto(AboutJump.IdiomaAddin, "BarPro2") + total.ToString();
            
            System.Windows.Forms.Application.DoEvents();
        }
        
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Cancelado = true;
        }
    }
}
