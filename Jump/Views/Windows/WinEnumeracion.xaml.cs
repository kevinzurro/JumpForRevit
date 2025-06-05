using Jump.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
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

namespace Jump.Views.Windows
{
    public partial class WinEnumeracion : Window, IDisposable
    {
        public WinEnumeracion(EnumeracionViewModel vmEnumeracion)
        {
            InitializeComponent();
            DataContext = vmEnumeracion;
        }

        public void Dispose()
        {
            try
            {
                this.Dispose();
            }
            catch (Exception) { }
        }
    }
}
