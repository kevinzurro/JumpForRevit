using Jump;
using Jump.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Resources;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Jump.ViewModels
{
    public class ConfigEtiquetasViewModel : ViewModelBase
    {
        ObservableCollection<KeyValuePair<int, string>> posicionEtiquetas;
        ObservableCollection<string> posiciones;

        public ConfigEtiquetasViewModel(ConfiguracionesModel model)
        {
            ConfigModelo = model;
            ImagenPreview = "pack://application:,,,/Jump;component/Resources/Configuracion_Etiquetas_Viga.png";

            PosicionesEtiquetas = PosicionTools.PosicionesTodas;;
        }

        public ObservableCollection<KeyValuePair<int, string>> PosicionesEtiquetas
        {
            get { return posicionEtiquetas; }
            set
            {
                posicionEtiquetas = value;
                OnPropertyChanged(nameof(PosicionesEtiquetas));
            }
        }

        public int EtiquetaViga
        {
            get { return Properties.Settings.Default.VigaEtiquetaIndependiente; }
            set
            {
                if (Properties.Settings.Default.VigaEtiquetaIndependiente != value)
                {
                    Properties.Settings.Default.VigaEtiquetaIndependiente = value;

                    Properties.Settings.Default.Save();

                    OnPropertyChanged(nameof(EtiquetaViga));
                }
            }
        }

        public int EtiquetaMuro
        {
            get { return Properties.Settings.Default.MuroEtiquetaIndependiente; }
            set
            {
                if (Properties.Settings.Default.MuroEtiquetaIndependiente != value)
                {
                    Properties.Settings.Default.MuroEtiquetaIndependiente = value;

                    Properties.Settings.Default.Save();

                    OnPropertyChanged(nameof(EtiquetaMuro));
                }
            }
        }

        public int EtiquetaColumna
        {
            get { return Properties.Settings.Default.ColumnaEtiquetaIndependiente; }
            set
            {
                if (Properties.Settings.Default.ColumnaEtiquetaIndependiente != value)
                {
                    Properties.Settings.Default.ColumnaEtiquetaIndependiente = value;

                    Properties.Settings.Default.Save();

                    OnPropertyChanged(nameof(EtiquetaColumna));
                }
            }
        }

        public int EtiquetaLosa
        {
            get { return Properties.Settings.Default.LosaEtiquetaIndependiente; }
            set
            {
                if (Properties.Settings.Default.LosaEtiquetaIndependiente != value)
                {
                    Properties.Settings.Default.LosaEtiquetaIndependiente = value;

                    Properties.Settings.Default.Save();

                    OnPropertyChanged(nameof(EtiquetaLosa));
                }
            }
        }

        public int EtiquetaZapata
        {
            get { return Properties.Settings.Default.ZapataEtiquetaIndependiente; }
            set
            {
                if (Properties.Settings.Default.ZapataEtiquetaIndependiente != value)
                {
                    Properties.Settings.Default.ZapataEtiquetaIndependiente = value;

                    Properties.Settings.Default.Save();

                    OnPropertyChanged(nameof(EtiquetaZapata));
                }
            }
        }

        public int EtiquetaZapataCorrida
        {
            get { return Properties.Settings.Default.ZapataCorridaEtiquetaIndependiente; }
            set
            {
                if (Properties.Settings.Default.ZapataCorridaEtiquetaIndependiente != value)
                {
                    Properties.Settings.Default.ZapataCorridaEtiquetaIndependiente = value;

                    Properties.Settings.Default.Save();

                    OnPropertyChanged(nameof(EtiquetaZapataCorrida));
                }
            }
        }

        public int EtiquetaPlatea
        {
            get { return Properties.Settings.Default.PlateaEtiquetaIndependiente; }
            set
            {
                if (Properties.Settings.Default.PlateaEtiquetaIndependiente != value)
                {
                    Properties.Settings.Default.PlateaEtiquetaIndependiente = value;

                    Properties.Settings.Default.Save();

                    OnPropertyChanged(nameof(EtiquetaPlatea));
                }
            }
        }

        public int EtiquetaPilote
        {
            get { return Properties.Settings.Default.PiloteEtiquetaIndependiente; }
            set
            {
                if (Properties.Settings.Default.PiloteEtiquetaIndependiente != value)
                {
                    Properties.Settings.Default.PiloteEtiquetaIndependiente = value;

                    Properties.Settings.Default.Save();

                    OnPropertyChanged(nameof(EtiquetaPilote));
                }
            }
        }

        public int EtiquetaArmadura
        {
            get { return Properties.Settings.Default.ArmaduraEtiquetaIndependiente; }
            set
            {
                if (Properties.Settings.Default.ArmaduraEtiquetaIndependiente != value)
                {
                    Properties.Settings.Default.ArmaduraEtiquetaIndependiente = value;

                    Properties.Settings.Default.Save();

                    OnPropertyChanged(nameof(EtiquetaArmadura));
                }
            }
        }

        public int EtiquetaAreaRefuerzo
        {
            get { return Properties.Settings.Default.AreaRefuerzoEtiquetaIndependiente; }
            set
            {
                if (Properties.Settings.Default.AreaRefuerzoEtiquetaIndependiente != value)
                {
                    Properties.Settings.Default.AreaRefuerzoEtiquetaIndependiente = value;

                    Properties.Settings.Default.Save();

                    OnPropertyChanged(nameof(EtiquetaAreaRefuerzo));
                }
            }
        }

        public int EtiquetaArmaduraEnSistema
        {
            get { return Properties.Settings.Default.ArmaduraEnSistemaEtiquetaIndependiente; }
            set
            {
                if (Properties.Settings.Default.ArmaduraEnSistemaEtiquetaIndependiente != value)
                {
                    Properties.Settings.Default.ArmaduraEnSistemaEtiquetaIndependiente = value;

                    Properties.Settings.Default.Save();

                    OnPropertyChanged(nameof(EtiquetaArmaduraEnSistema));
                }
            }
        }

    }
}
