using Jump;
using Jump.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Resources;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Jump.ViewModels
{
    public class ConfigGeneralViewModel : ViewModelBase
    {
        private ConfiguracionesModel mConfiguracion;
        private string imagenPreview = "pack://application:,,,/Jump;component/Resources/Configuracion_Precision.png";

        public ConfigGeneralViewModel(ConfiguracionesModel model)
        {
            Modelo = model;
        }

        public ConfiguracionesModel Modelo
        {
            get { return mConfiguracion; }
            set
            {
                mConfiguracion = value as ConfiguracionesModel;
            }
        }

        public string TxtUnidadDistancia
        {
            get { return Modelo.ObtenerUnidadDistancia(); }
        }

        public double PrecisionOrdenarX
        {
            get { return Modelo.ConvertirDesdeUnidadesInternas(Properties.Settings.Default.ConfiguracionPrecisionOrdenarX); }
            set
            {
                if (Properties.Settings.Default.ConfiguracionPrecisionOrdenarX != value)
                {
                    Properties.Settings.Default.ConfiguracionPrecisionOrdenarX = Modelo.ConvertirAUnidadesInternas(value);

                    Properties.Settings.Default.Save();

                    OnPropertyChanged(nameof(PrecisionOrdenarX));
                }
            }
        }
        
        public double PrecisionOrdenarY
        {
            get { return Modelo.ConvertirDesdeUnidadesInternas(Properties.Settings.Default.ConfiguracionPrecisionOrdenarY); }
            set
            {
                if (Properties.Settings.Default.ConfiguracionPrecisionOrdenarY != value)
                {
                    Properties.Settings.Default.ConfiguracionPrecisionOrdenarY = Modelo.ConvertirAUnidadesInternas(value);

                    Properties.Settings.Default.Save();

                    OnPropertyChanged(nameof(PrecisionOrdenarY));
                }
            }
        }

        public double PrecisionNodoAnalitico
        {
            get { return Properties.Settings.Default.ConfiguracionPrecisionNodoAnalitico; }
            set
            {
                if (Properties.Settings.Default.ConfiguracionPrecisionNodoAnalitico != value)
                {
                    Properties.Settings.Default.ConfiguracionPrecisionNodoAnalitico = value;

                    Properties.Settings.Default.Save();

                    OnPropertyChanged(nameof(PrecisionNodoAnalitico));
                }
            }
        }

        public bool VistaLocal
        {
            get { return Properties.Settings.Default.ConfiguracionVistaLocal; }
            set
            {
                if (Properties.Settings.Default.ConfiguracionVistaLocal != value)
                {
                    Properties.Settings.Default.ConfiguracionVistaLocal = value;

                    Properties.Settings.Default.Save();

                    OnPropertyChanged(nameof(VistaLocal));
                }
            }
        }

        public bool VistaGlobal
        {
            get { return Properties.Settings.Default.ConfiguracionVistaGlobal; }
            set
            {
                if (Properties.Settings.Default.ConfiguracionVistaGlobal != value)
                {
                    Properties.Settings.Default.ConfiguracionVistaGlobal = value;

                    Properties.Settings.Default.Save();

                    OnPropertyChanged(nameof(VistaGlobal));
                }
            }
        }

        public double CorteTransversal
        {
            get { return Properties.Settings.Default.ConfiguracionCorteTransversalBasadoLinea * 100; }
            set
            {
                if (value >= 0 && value <= 100 &&
                    Properties.Settings.Default.ConfiguracionCorteTransversalBasadoLinea != value / 100)
                {
                    Properties.Settings.Default.ConfiguracionCorteTransversalBasadoLinea = value / 100;

                    Properties.Settings.Default.Save();

                    OnPropertyChanged(nameof(CorteTransversal));
                }
            }
        }

        public double PuntoAEvaluarOrden
        {
            get { return Properties.Settings.Default.ConfiguracionPuntoParaEvaluarLinea * 100; }
            set
            {
                if (value >= 0 && value <= 100 &&
                    Properties.Settings.Default.ConfiguracionPuntoParaEvaluarLinea != value / 100)
                {
                    Properties.Settings.Default.ConfiguracionPuntoParaEvaluarLinea = value / 100;

                    Properties.Settings.Default.Save();

                    OnPropertyChanged(nameof(PuntoAEvaluarOrden));
                }
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
