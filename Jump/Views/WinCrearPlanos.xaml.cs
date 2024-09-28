using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Collections.ObjectModel;
using Jump.ViewModels;

namespace Jump.Views
{
    public partial class WinCrearPlanos : Window, IDisposable
    {
        public WinCrearPlanos(CrearPlanosViewModel mvCrearPlanos)
        {
            InitializeComponent();
            DataContext = mvCrearPlanos;
        }

        public void Dispose()
        {
            this.Dispose();
        }
    }
}
