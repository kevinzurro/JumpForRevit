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
    public class ConfiguracionViewModel : ViewModelBase
    {
        private ViewModelBase vistaActual;
        private ConfigGeneralViewModel vmGeneral;
        private ConfigEtiquetasViewModel vmEtiquetas;
        private ConfigCotasViewModel vmCotas;
        private ConfigCotasProfundidadViewModel vmCotasProfundidad;

        public ConfiguracionViewModel() { }

        public ConfiguracionViewModel(ConfiguracionesModel model) 
        {
            ConfigModelo = model;

            GeneralVM = new ConfigGeneralViewModel(ConfigModelo);
            EtiquetasVM = new ConfigEtiquetasViewModel(ConfigModelo);
            CotasVM = new ConfigCotasViewModel(ConfigModelo);
            CotasProfundidadVM = new ConfigCotasProfundidadViewModel(ConfigModelo);

            VistaActual = GeneralVM;
        }

        public ViewModelBase VistaActual
        {
            get { return vistaActual; }
            set
            {
                vistaActual = value;
                OnPropertyChanged(nameof(VistaActual));
            }
        }

        public ConfigGeneralViewModel GeneralVM
        {
            get { return vmGeneral; }
            set
            {
                vmGeneral = value;
                OnPropertyChanged(nameof(GeneralVM));
            }
        }

        public ConfigEtiquetasViewModel EtiquetasVM
        {
            get { return vmEtiquetas; }
            set
            {
                vmEtiquetas = value;
                OnPropertyChanged(nameof(EtiquetasVM));
            }
        }

        public ConfigCotasViewModel CotasVM
        {
            get { return vmCotas; }
            set
            {
                vmCotas = value;
                OnPropertyChanged(nameof(CotasVM));
            }
        }

        public ConfigCotasProfundidadViewModel CotasProfundidadVM
        {
            get { return vmCotasProfundidad; }
            set
            {
                vmCotasProfundidad = value;
                OnPropertyChanged(nameof(CotasProfundidadVM));
            }
        }

        public RelayCommand CambiarVistaGeneralCommand => new RelayCommand(execute => CambiarVista(GeneralVM), canExecute => { return true; });

        public RelayCommand CambiarVistaEtiquetasCommand => new RelayCommand(execute => CambiarVista(EtiquetasVM), canExecute => { return true; });

        public RelayCommand CambiarVistaCotasCommand => new RelayCommand(execute => CambiarVista(CotasVM), canExecute => { return true; });

        public RelayCommand CambiarVistaCotasProfundidadCommand => new RelayCommand(execute => CambiarVista(CotasProfundidadVM), canExecute => { return true; });

        private void CambiarVista(ViewModelBase vm) 
        {
            VistaActual = vm;
        }
    }
}
