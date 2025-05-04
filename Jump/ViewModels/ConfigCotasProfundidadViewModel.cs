using Jump.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jump.ViewModels
{
    public class ConfigCotasProfundidadViewModel : ViewModelBase
    {
        ObservableCollection<KeyValuePair<int, string>> posicionCotasProfundidad;

        public ConfigCotasProfundidadViewModel(ConfiguracionModel model)
        {
            Modelo = model;
            ImagenPreview = "pack://application:,,,/Jump;component/Resources/Configuracion_CotaProfundidad_Viga.png";
            
            PosicionesCotaProfundidad = PosicionTools.PosicionesCotaProfundidad;
        }

        public ObservableCollection<KeyValuePair<int, string>> PosicionesCotaProfundidad
        {
            get { return posicionCotasProfundidad; }
            set
            {
                posicionCotasProfundidad = value;
                OnPropertyChanged(nameof(PosicionesCotaProfundidad));
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
