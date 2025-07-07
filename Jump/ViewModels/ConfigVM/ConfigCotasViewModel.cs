using Jump;
using Jump.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Jump.ViewModels
{
    public class ConfigCotasViewModel : ViewModelBase
    {
        public ConfigCotasViewModel(ConfiguracionModel model)
        {
            Modelo = model;
            ImagenPreview = "pack://application:,,,/Jump;component/Resources/Configuracion_Cotas_Viga.png";
        }

        public bool VigaCotaLinealArriba
        {
            get { return Properties.Settings.Default.VigaCotaLinealArriba; }
            set
            {
                if (Properties.Settings.Default.VigaCotaLinealArriba != value)
                {
                    Properties.Settings.Default.VigaCotaLinealArriba = value;

                    Properties.Settings.Default.Save();

                    OnPropertyChanged(nameof(VigaCotaLinealArriba));
                }
            }
        }

        public bool VigaCotaLinealAbajo
        {
            get { return Properties.Settings.Default.VigaCotaLinealAbajo; }
            set
            {
                if (Properties.Settings.Default.VigaCotaLinealAbajo != value)
                {
                    Properties.Settings.Default.VigaCotaLinealAbajo = value;

                    Properties.Settings.Default.Save();

                    OnPropertyChanged(nameof(VigaCotaLinealAbajo));
                }
            }
        }

        public bool VigaCotaLinealIzquierda
        {
            get { return Properties.Settings.Default.VigaCotaLinealIzquierda; }
            set
            {
                if (Properties.Settings.Default.VigaCotaLinealIzquierda != value)
                {
                    Properties.Settings.Default.VigaCotaLinealIzquierda = value;

                    Properties.Settings.Default.Save();

                    OnPropertyChanged(nameof(VigaCotaLinealIzquierda));
                }
            }
        }

        public bool VigaCotaLinealDerecha
        {
            get { return Properties.Settings.Default.VigaCotaLinealDerecha; }
            set
            {
                if (Properties.Settings.Default.VigaCotaLinealDerecha != value)
                {
                    Properties.Settings.Default.VigaCotaLinealDerecha = value;

                    Properties.Settings.Default.Save();

                    OnPropertyChanged(nameof(VigaCotaLinealDerecha));
                }
            }
        }

        public int VigaCotaProfundidad
        {
            get { return Properties.Settings.Default.VigaCotaProfundidad; }
            set
            {
                if (Properties.Settings.Default.VigaCotaProfundidad != value)
                {
                    Properties.Settings.Default.VigaCotaProfundidad = value;

                    Properties.Settings.Default.Save();

                    OnPropertyChanged(nameof(VigaCotaProfundidad));
                }
            }
        }

        public bool MuroCotaLinealArriba
        {
            get { return Properties.Settings.Default.MuroCotaLinealArriba; }
            set
            {
                if (Properties.Settings.Default.MuroCotaLinealArriba != value)
                {
                    Properties.Settings.Default.MuroCotaLinealArriba = value;

                    Properties.Settings.Default.Save();

                    OnPropertyChanged(nameof(MuroCotaLinealArriba));
                }
            }
        }

        public bool MuroCotaLinealAbajo
        {
            get { return Properties.Settings.Default.MuroCotaLinealAbajo; }
            set
            {
                if (Properties.Settings.Default.MuroCotaLinealAbajo != value)
                {
                    Properties.Settings.Default.MuroCotaLinealAbajo = value;

                    Properties.Settings.Default.Save();

                    OnPropertyChanged(nameof(MuroCotaLinealAbajo));
                }
            }
        }

        public bool MuroCotaLinealIzquierda
        {
            get { return Properties.Settings.Default.MuroCotaLinealIzquierda; }
            set
            {
                if (Properties.Settings.Default.MuroCotaLinealIzquierda != value)
                {
                    Properties.Settings.Default.MuroCotaLinealIzquierda = value;

                    Properties.Settings.Default.Save();

                    OnPropertyChanged(nameof(MuroCotaLinealIzquierda));
                }
            }
        }

        public bool MuroCotaLinealDerecha
        {
            get { return Properties.Settings.Default.MuroCotaLinealDerecha; }
            set
            {
                if (Properties.Settings.Default.MuroCotaLinealDerecha != value)
                {
                    Properties.Settings.Default.MuroCotaLinealDerecha = value;

                    Properties.Settings.Default.Save();

                    OnPropertyChanged(nameof(MuroCotaLinealDerecha));
                }
            }
        }

        public int MuroCotaProfundidad
        {
            get { return Properties.Settings.Default.MuroCotaProfundidad; }
            set
            {
                if (Properties.Settings.Default.MuroCotaProfundidad != value)
                {
                    Properties.Settings.Default.MuroCotaProfundidad = value;

                    Properties.Settings.Default.Save();

                    OnPropertyChanged(nameof(MuroCotaProfundidad));
                }
            }
        }

        public bool ColumnaCotaLinealArriba
        {
            get { return Properties.Settings.Default.ColumnaCotaLinealArriba; }
            set
            {
                if (Properties.Settings.Default.ColumnaCotaLinealArriba != value)
                {
                    Properties.Settings.Default.ColumnaCotaLinealArriba = value;

                    Properties.Settings.Default.Save();

                    OnPropertyChanged(nameof(ColumnaCotaLinealArriba));
                }
            }
        }

        public bool ColumnaCotaLinealAbajo
        {
            get { return Properties.Settings.Default.ColumnaCotaLinealAbajo; }
            set
            {
                if (Properties.Settings.Default.ColumnaCotaLinealAbajo != value)
                {
                    Properties.Settings.Default.ColumnaCotaLinealAbajo = value;

                    Properties.Settings.Default.Save();

                    OnPropertyChanged(nameof(ColumnaCotaLinealAbajo));
                }
            }
        }

        public bool ColumnaCotaLinealIzquierda
        {
            get { return Properties.Settings.Default.ColumnaCotaLinealIzquierda; }
            set
            {
                if (Properties.Settings.Default.ColumnaCotaLinealIzquierda != value)
                {
                    Properties.Settings.Default.ColumnaCotaLinealIzquierda = value;

                    Properties.Settings.Default.Save();

                    OnPropertyChanged(nameof(ColumnaCotaLinealIzquierda));
                }
            }
        }

        public bool ColumnaCotaLinealDerecha
        {
            get { return Properties.Settings.Default.ColumnaCotaLinealDerecha; }
            set
            {
                if (Properties.Settings.Default.ColumnaCotaLinealDerecha != value)
                {
                    Properties.Settings.Default.ColumnaCotaLinealDerecha = value;

                    Properties.Settings.Default.Save();

                    OnPropertyChanged(nameof(ColumnaCotaLinealDerecha));
                }
            }
        }

        public int ColumnaCotaProfundidad
        {
            get { return Properties.Settings.Default.ColumnaCotaProfundidad; }
            set
            {
                if (Properties.Settings.Default.ColumnaCotaProfundidad != value)
                {
                    Properties.Settings.Default.ColumnaCotaProfundidad = value;

                    Properties.Settings.Default.Save();

                    OnPropertyChanged(nameof(ColumnaCotaProfundidad));
                }
            }
        }

        public bool LosaCotaLinealArriba
        {
            get { return Properties.Settings.Default.LosaCotaLinealArriba; }
            set
            {
                if (Properties.Settings.Default.LosaCotaLinealArriba != value)
                {
                    Properties.Settings.Default.LosaCotaLinealArriba = value;

                    Properties.Settings.Default.Save();

                    OnPropertyChanged(nameof(LosaCotaLinealArriba));
                }
            }
        }

        public bool LosaCotaLinealAbajo
        {
            get { return Properties.Settings.Default.LosaCotaLinealAbajo; }
            set
            {
                if (Properties.Settings.Default.LosaCotaLinealAbajo != value)
                {
                    Properties.Settings.Default.LosaCotaLinealAbajo = value;

                    Properties.Settings.Default.Save();

                    OnPropertyChanged(nameof(LosaCotaLinealAbajo));
                }
            }
        }

        public bool LosaCotaLinealIzquierda
        {
            get { return Properties.Settings.Default.LosaCotaLinealIzquierda; }
            set
            {
                if (Properties.Settings.Default.LosaCotaLinealIzquierda != value)
                {
                    Properties.Settings.Default.LosaCotaLinealIzquierda = value;

                    Properties.Settings.Default.Save();

                    OnPropertyChanged(nameof(LosaCotaLinealIzquierda));
                }
            }
        }

        public bool LosaCotaLinealDerecha
        {
            get { return Properties.Settings.Default.LosaCotaLinealDerecha; }
            set
            {
                if (Properties.Settings.Default.LosaCotaLinealDerecha != value)
                {
                    Properties.Settings.Default.LosaCotaLinealDerecha = value;

                    Properties.Settings.Default.Save();

                    OnPropertyChanged(nameof(LosaCotaLinealDerecha));
                }
            }
        }

        public int LosaCotaProfundidad
        {
            get { return Properties.Settings.Default.LosaCotaProfundidad; }
            set
            {
                if (Properties.Settings.Default.LosaCotaProfundidad != value)
                {
                    Properties.Settings.Default.LosaCotaProfundidad = value;

                    Properties.Settings.Default.Save();

                    OnPropertyChanged(nameof(LosaCotaProfundidad));
                }
            }
        }

        public bool ZapataCotaLinealArriba
        {
            get { return Properties.Settings.Default.ZapataCotaLinealArriba; }
            set
            {
                if (Properties.Settings.Default.ZapataCotaLinealArriba != value)
                {
                    Properties.Settings.Default.ZapataCotaLinealArriba = value;

                    Properties.Settings.Default.Save();

                    OnPropertyChanged(nameof(ZapataCotaLinealArriba));
                }
            }
        }

        public bool ZapataCotaLinealAbajo
        {
            get { return Properties.Settings.Default.ZapataCotaLinealAbajo; }
            set
            {
                if (Properties.Settings.Default.ZapataCotaLinealAbajo != value)
                {
                    Properties.Settings.Default.ZapataCotaLinealAbajo = value;

                    Properties.Settings.Default.Save();

                    OnPropertyChanged(nameof(ZapataCotaLinealAbajo));
                }
            }
        }

        public bool ZapataCotaLinealIzquierda
        {
            get { return Properties.Settings.Default.ZapataCotaLinealIzquierda; }
            set
            {
                if (Properties.Settings.Default.ZapataCotaLinealIzquierda != value)
                {
                    Properties.Settings.Default.ZapataCotaLinealIzquierda = value;

                    Properties.Settings.Default.Save();

                    OnPropertyChanged(nameof(ZapataCotaLinealIzquierda));
                }
            }
        }

        public bool ZapataCotaLinealDerecha
        {
            get { return Properties.Settings.Default.ZapataCotaLinealDerecha; }
            set
            {
                if (Properties.Settings.Default.ZapataCotaLinealDerecha != value)
                {
                    Properties.Settings.Default.ZapataCotaLinealDerecha = value;

                    Properties.Settings.Default.Save();

                    OnPropertyChanged(nameof(ZapataCotaLinealDerecha));
                }
            }
        }

        public int ZapataCotaProfundidad
        {
            get { return Properties.Settings.Default.ZapataCotaProfundidad; }
            set
            {
                if (Properties.Settings.Default.ZapataCotaProfundidad != value)
                {
                    Properties.Settings.Default.ZapataCotaProfundidad = value;

                    Properties.Settings.Default.Save();

                    OnPropertyChanged(nameof(ZapataCotaProfundidad));
                }
            }
        }

        public bool ZapataCorridaCotaLinealArriba
        {
            get { return Properties.Settings.Default.ZapataCorridaCotaLinealArriba; }
            set
            {
                if (Properties.Settings.Default.ZapataCorridaCotaLinealArriba != value)
                {
                    Properties.Settings.Default.ZapataCorridaCotaLinealArriba = value;

                    Properties.Settings.Default.Save();

                    OnPropertyChanged(nameof(ZapataCorridaCotaLinealArriba));
                }
            }
        }

        public bool ZapataCorridaCotaLinealAbajo
        {
            get { return Properties.Settings.Default.ZapataCorridaCotaLinealAbajo; }
            set
            {
                if (Properties.Settings.Default.ZapataCorridaCotaLinealAbajo != value)
                {
                    Properties.Settings.Default.ZapataCorridaCotaLinealAbajo = value;

                    Properties.Settings.Default.Save();

                    OnPropertyChanged(nameof(ZapataCorridaCotaLinealAbajo));
                }
            }
        }

        public bool ZapataCorridaCotaLinealIzquierda
        {
            get { return Properties.Settings.Default.ZapataCorridaCotaLinealIzquierda; }
            set
            {
                if (Properties.Settings.Default.ZapataCorridaCotaLinealIzquierda != value)
                {
                    Properties.Settings.Default.ZapataCorridaCotaLinealIzquierda = value;

                    Properties.Settings.Default.Save();

                    OnPropertyChanged(nameof(ZapataCorridaCotaLinealIzquierda));
                }
            }
        }

        public bool ZapataCorridaCotaLinealDerecha
        {
            get { return Properties.Settings.Default.ZapataCorridaCotaLinealDerecha; }
            set
            {
                if (Properties.Settings.Default.ZapataCorridaCotaLinealDerecha != value)
                {
                    Properties.Settings.Default.ZapataCorridaCotaLinealDerecha = value;

                    Properties.Settings.Default.Save();

                    OnPropertyChanged(nameof(ZapataCorridaCotaLinealDerecha));
                }
            }
        }

        public int ZapataCorridaCotaProfundidad
        {
            get { return Properties.Settings.Default.ZapataCorridaCotaProfundidad; }
            set
            {
                if (Properties.Settings.Default.ZapataCorridaCotaProfundidad != value)
                {
                    Properties.Settings.Default.ZapataCorridaCotaProfundidad = value;

                    Properties.Settings.Default.Save();

                    OnPropertyChanged(nameof(ZapataCorridaCotaProfundidad));
                }
            }
        }

        public bool PlateaCotaLinealArriba
        {
            get { return Properties.Settings.Default.PlateaCotaLinealArriba; }
            set
            {
                if (Properties.Settings.Default.PlateaCotaLinealArriba != value)
                {
                    Properties.Settings.Default.PlateaCotaLinealArriba = value;

                    Properties.Settings.Default.Save();

                    OnPropertyChanged(nameof(PlateaCotaLinealArriba));
                }
            }
        }

        public bool PlateaCotaLinealAbajo
        {
            get { return Properties.Settings.Default.PlateaCotaLinealAbajo; }
            set
            {
                if (Properties.Settings.Default.PlateaCotaLinealAbajo != value)
                {
                    Properties.Settings.Default.PlateaCotaLinealAbajo = value;

                    Properties.Settings.Default.Save();

                    OnPropertyChanged(nameof(PlateaCotaLinealAbajo));
                }
            }
        }

        public bool PlateaCotaLinealIzquierda
        {
            get { return Properties.Settings.Default.PlateaCotaLinealIzquierda; }
            set
            {
                if (Properties.Settings.Default.PlateaCotaLinealIzquierda != value)
                {
                    Properties.Settings.Default.PlateaCotaLinealIzquierda = value;

                    Properties.Settings.Default.Save();

                    OnPropertyChanged(nameof(PlateaCotaLinealIzquierda));
                }
            }
        }

        public bool PlateaCotaLinealDerecha
        {
            get { return Properties.Settings.Default.PlateaCotaLinealDerecha; }
            set
            {
                if (Properties.Settings.Default.PlateaCotaLinealDerecha != value)
                {
                    Properties.Settings.Default.PlateaCotaLinealDerecha = value;

                    Properties.Settings.Default.Save();

                    OnPropertyChanged(nameof(PlateaCotaLinealDerecha));
                }
            }
        }

        public int PlateaCotaProfundidad
        {
            get { return Properties.Settings.Default.PlateaCotaProfundidad; }
            set
            {
                if (Properties.Settings.Default.PlateaCotaProfundidad != value)
                {
                    Properties.Settings.Default.PlateaCotaProfundidad = value;

                    Properties.Settings.Default.Save();

                    OnPropertyChanged(nameof(PlateaCotaProfundidad));
                }
            }
        }

        public bool PiloteCotaLinealArriba
        {
            get { return Properties.Settings.Default.PiloteCotaLinealArriba; }
            set
            {
                if (Properties.Settings.Default.PiloteCotaLinealArriba != value)
                {
                    Properties.Settings.Default.PiloteCotaLinealArriba = value;

                    Properties.Settings.Default.Save();

                    OnPropertyChanged(nameof(PiloteCotaLinealArriba));
                }
            }
        }

        public bool PiloteCotaLinealAbajo
        {
            get { return Properties.Settings.Default.PiloteCotaLinealAbajo; }
            set
            {
                if (Properties.Settings.Default.PiloteCotaLinealAbajo != value)
                {
                    Properties.Settings.Default.PiloteCotaLinealAbajo = value;

                    Properties.Settings.Default.Save();

                    OnPropertyChanged(nameof(PiloteCotaLinealAbajo));
                }
            }
        }

        public bool PiloteCotaLinealIzquierda
        {
            get { return Properties.Settings.Default.PiloteCotaLinealIzquierda; }
            set
            {
                if (Properties.Settings.Default.PiloteCotaLinealIzquierda != value)
                {
                    Properties.Settings.Default.PiloteCotaLinealIzquierda = value;

                    Properties.Settings.Default.Save();

                    OnPropertyChanged(nameof(PiloteCotaLinealIzquierda));
                }
            }
        }

        public bool PiloteCotaLinealDerecha
        {
            get { return Properties.Settings.Default.PiloteCotaLinealDerecha; }
            set
            {
                if (Properties.Settings.Default.PiloteCotaLinealDerecha != value)
                {
                    Properties.Settings.Default.PiloteCotaLinealDerecha = value;

                    Properties.Settings.Default.Save();

                    OnPropertyChanged(nameof(PiloteCotaLinealDerecha));
                }
            }
        }

        public int PiloteCotaProfundidad
        {
            get { return Properties.Settings.Default.PiloteCotaProfundidad; }
            set
            {
                if (Properties.Settings.Default.PiloteCotaProfundidad != value)
                {
                    Properties.Settings.Default.PiloteCotaProfundidad = value;

                    Properties.Settings.Default.Save();

                    OnPropertyChanged(nameof(PiloteCotaProfundidad));
                }
            }
        }

    }
}
