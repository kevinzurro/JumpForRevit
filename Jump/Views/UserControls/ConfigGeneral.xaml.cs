using System;
using System.Collections.Generic;
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
using System.Windows.Resources;
using System.Windows.Shapes;

namespace Jump.Views.UserControls
{
    public partial class ConfigGeneral : UserControl, IDisposable
    {
        public ConfigGeneral()
        {
            InitializeComponent();
        }

        public void Dispose()
        {
            this.Dispose();
        }
    }
}
