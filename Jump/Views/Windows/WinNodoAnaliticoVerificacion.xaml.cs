using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Collections.ObjectModel;
using Jump.ViewModels;

namespace Jump.Views.Windows
{
    public partial class WinNodoAnaliticoVerificacion : Window, IDisposable
    {
        public WinNodoAnaliticoVerificacion(NodoAnaliticoVerificacionViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }

        public void Dispose()
        {
            this.Dispose();
        }
    }
}
