using Jump.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Jump
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangingEventHandler PropertyChanging;
        public event PropertyChangedEventHandler PropertyChanged;
        private ConfiguracionesModel mConfiguracion;
        private string imagenPreview = null;

        protected virtual void OnPropertyChanging(string propertyName)
        {
            if (PropertyChanging != null)
            {
                PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public ConfiguracionesModel ConfigModelo
        {
            get { return mConfiguracion; }
            set
            {
                mConfiguracion = value as ConfiguracionesModel;
            }
        }

        public string ImagenPreview
        {
            get { return imagenPreview; }
            set
            {
                if (imagenPreview != value)
                {
                    imagenPreview = value;

                    OnPropertyChanged(nameof(ImagenPreview));
                }
            }
        }

        public RelayCommand CambiarImagenCommand => new RelayCommand(execute => CambiarImagen(execute), canExecute => { return true; });

        private void CambiarImagen(object imagen)
        {
            ImagenPreview = imagen as string;
        }
    }
}
