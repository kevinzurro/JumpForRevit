using System;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows;
using System.Collections.ObjectModel;
using Autodesk.Revit.DB;

namespace Jump.Views
{
    /// <summary> Lógica de interacción para WinCrearPlanos.xaml </summary>
    public partial class WinCrearPlanos : Window, IDisposable
    {
        public WinCrearPlanos()
        {
            InitializeComponent();
        }

        public void Dispose()
        {
            this.Dispose();
        }
    }
}
