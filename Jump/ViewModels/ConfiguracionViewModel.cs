using Autodesk.Revit.DB.Visual;
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
        private ConfiguracionesModel mConfiguracion;
        private ViewModelBase vistaActual;
        private ConfigGeneralViewModel vmGeneral;
        private ConfigEtiquetasViewModel vmEtiquetas;

        public ConfiguracionViewModel() { }

        public ConfiguracionViewModel(ConfiguracionesModel model,
                                      ConfigGeneralViewModel general,
                                      ConfigEtiquetasViewModel etiquetas) 
        {
            Modelo = model;
            GeneralVM = general;
            EtiquetasVM = etiquetas;

            VistaActual = GeneralVM;
        }

        public ConfiguracionesModel Modelo
        {
            get { return mConfiguracion; }
            set
            {
                mConfiguracion = value;
            }
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

        public RelayCommand CambiarVistaGeneralCommand => new RelayCommand(execute => CambiarVista(GeneralVM), canExecute => { return true; });

        public RelayCommand CambiarVistaEtiquetasCommand => new RelayCommand(execute => CambiarVista(EtiquetasVM), canExecute => { return true; });

        public RelayCommand CambiarVistaCotasCommand => new RelayCommand(execute => CambiarVista(GeneralVM), canExecute => { return true; });

        private void CambiarVista(ViewModelBase vm) 
        {
            VistaActual = vm;
        }
    }
}
