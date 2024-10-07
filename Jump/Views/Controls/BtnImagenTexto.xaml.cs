using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Jump.Views.Controls
{
    public partial class BtnImagenTexto : UserControl, IDisposable
    {
        public BtnImagenTexto()
        {
            InitializeComponent();
            DataContext = this;
        }

        public void Dispose()
        {
            this.Dispose();
        }

        public static readonly DependencyProperty ButtonCommandProperty =
                               DependencyProperty.Register(nameof(ButtonCommand),
                               typeof(ICommand),
                               typeof(BtnImagenTexto),
                               new PropertyMetadata(null));

        public ICommand ButtonCommand
        {
            get { return (ICommand)GetValue(ButtonCommandProperty); }
            set { SetValue(ButtonCommandProperty, value); }
        }

        //public static readonly DependencyProperty TitleProperty =
        //                       DependencyProperty.Register(nameof(Title),
        //                       typeof(string),
        //                       typeof(BtnImagenTexto),
        //                       new PropertyMetadata(string.Empty));

        //public string Title
        //{
        //    get { return (string)GetValue(TitleProperty); }
        //    set { SetValue(TitleProperty, value); }
        //}

        public string Title { get; set; }

        public string Image { get; set; }
    }
}
