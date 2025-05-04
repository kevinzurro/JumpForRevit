using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media.TextFormatting;

namespace Jump
{
    public  class MenuItem : ModelMenu
    {
        private Int64 id;
        private Familia familia;
        private string nombreCompleto;
        private string nombre;
        private bool isChecked;
        private bool isExpanded;
        private ObservableCollection<MenuItem> items;

        public Int64 ID
        {
            get { return id; }
            set
            {
                if (id != value)
                {
                    id = value;
                    OnPropertyChanged(nameof(ID));
                }
            }
        }

        public Familia Familia
        {
            get { return familia; }
            set
            {
                if (familia != value)
                {
                    familia = value;
                    OnPropertyChanged(nameof(Familia));
                }
            }
        }

        public string Nombre
        {
            get { return nombre; }
            set
            {
                if (nombre != value)
                {
                    nombre = value;
                    OnPropertyChanged(nameof(Nombre));
                }
            }
        }

        public string NombreCompleto
        {
            get { return nombreCompleto; }
            set
            {
                if (nombreCompleto != value)
                {
                    nombreCompleto = value;
                    OnPropertyChanged(nameof(NombreCompleto));
                }
            }
        }

        public bool IsChecked
        {
            get { return isChecked; }
            set
            {
                if (isChecked != value)
                {
                    isChecked = value;
                    
                    CambiarChecked(this, IsChecked);

                    OnPropertyChanged(nameof(IsChecked));
                }
            }
        }

        public bool IsExpanded
        {
            get { return isExpanded; }
            set
            {
                if (isExpanded != value)
                {
                    isExpanded = value;

                    OnPropertyChanged(nameof(IsExpanded));
                }
            }
        }

        public ObservableCollection<MenuItem> Items
        {
            get { return items; }
            set
            {
                if (items != value)
                {
                    items = value;

                    OnPropertyChanged(nameof(Items));
                }
            }
        }

        public MenuItem(Familia fa)
        {
            Items = new ObservableCollection<MenuItem>();

            familia = fa;

            ID = fa.ID;

            Nombre = fa.Nombre;
            NombreCompleto = fa.NombreCompleto;

            IsChecked = false;
            IsExpanded = false;
        }

        public RelayCommand CambiarCheckedCommand => new RelayCommand(execute => CambiarChecked(this, isChecked), canExecute => { return true; });

        /// <summary> Cambiar el estado del check del item y de sus hijos </summary>
        private void CambiarChecked(MenuItem item, bool check)
        {
            foreach (MenuItem i in item.Items)
            {
                i.IsChecked = check;
            }
        }
    }
}
