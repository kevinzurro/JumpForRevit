using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Collections.ObjectModel;
using Jump.ViewModels;

namespace Jump.Views.Windows
{
    public partial class WinConfiguraciones : Window, IDisposable
    {
        public WinConfiguraciones()
        {
            InitializeComponent();
        }

        public void Dispose()
        {
            this.Dispose();
        }
    }
}
