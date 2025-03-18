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
        public ConfigGeneralViewModel(ConfiguracionesModel model)
        {
            ConfigModelo = model;
            ImagenPreview = "pack://application:,,,/Jump;component/Resources/Configuracion_Precision.png";
        }

        public string TxtUnidadDistancia
        {
            get { return ConfigModelo.ObtenerUnidadDistancia(); }
        }

        public double PrecisionOrdenarX
        {
            get { return ConfigModelo.ConvertirDesdeUnidadesInternas(Properties.Settings.Default.ConfiguracionPrecisionOrdenarX); }
            set
            {
                if (Properties.Settings.Default.ConfiguracionPrecisionOrdenarX != value)
                {
                    Properties.Settings.Default.ConfiguracionPrecisionOrdenarX = ConfigModelo.ConvertirAUnidadesInternas(value);

                    Properties.Settings.Default.Save();

                    OnPropertyChanged(nameof(PrecisionOrdenarX));
                }
            }
        }
        
        public double PrecisionOrdenarY
        {
            get { return ConfigModelo.ConvertirDesdeUnidadesInternas(Properties.Settings.Default.ConfiguracionPrecisionOrdenarY); }
            set
            {
                if (Properties.Settings.Default.ConfiguracionPrecisionOrdenarY != value)
                {
                    Properties.Settings.Default.ConfiguracionPrecisionOrdenarY = ConfigModelo.ConvertirAUnidadesInternas(value);

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

    }
}
