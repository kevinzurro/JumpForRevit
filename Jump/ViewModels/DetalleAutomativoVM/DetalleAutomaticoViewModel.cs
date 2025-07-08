using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Jump.Models;

namespace Jump.ViewModels
{
    public class DetalleAutomaticoViewModel : ViewModelBase
    {
        private bool vistaXX = true;
        private bool vistaYY = true;
        private bool vistaPlanta = true;

        private ViewModelBase vistaActual;
        private DetalleAutomaticoModel modelo;
        private DetAutUserControlViewModelXX detAutoSeccXX;
        private DetAutUserControlViewModelYY detAutoSeccYY;
        private DetAutUserControlViewModelPlanta detAutoPlanta;

        public DetalleAutomaticoViewModel(DetalleAutomaticoModel model)
        {
            Modelo = model;

            DetAutoSeccXX = new DetAutUserControlViewModelXX(model);
            DetAutoSeccYY = new DetAutUserControlViewModelYY(model);
            DetAutoPlanta = new DetAutUserControlViewModelPlanta(model);

            VistaActual = DetAutoSeccXX;
        }

        public bool VistaXX
        {
            get { return vistaXX; }
            set
            {
                vistaXX = value;
                OnPropertyChanged(nameof(VistaXX));
            }
        }

        public bool VistaYY
        {
            get { return vistaYY; }
            set
            {
                vistaYY = value;
                OnPropertyChanged(nameof(VistaYY));
            }
        }

        public bool VistaPlanta
        {
            get { return vistaPlanta; }
            set
            {
                vistaPlanta = value;
                OnPropertyChanged(nameof(VistaPlanta));
            }
        }

        public new DetalleAutomaticoModel Modelo
        {
            get { return modelo; }
            set { modelo = value; }
        }

        public ViewModelBase VistaActual
        {
            get { return vistaActual; }
            set
            {
                if (vistaActual != value)
                {
                    vistaActual = value;

                    if ((vistaActual as DetAutUserControlViewModel).VistaActual != null)
                    {
                        (vistaActual as DetAutUserControlViewModel).VistaActual.Dispose();
                    }

                    OnPropertyChanged(nameof(VistaActual));
                }
            }
        }

        public DetAutUserControlViewModelXX DetAutoSeccXX
        {
            get { return detAutoSeccXX; }
            set
            {
                if (detAutoSeccXX != value)
                {
                    detAutoSeccXX = value;
                    OnPropertyChanged(nameof(DetAutoSeccXX));
                }
            }
        }

        public DetAutUserControlViewModelYY DetAutoSeccYY
        {
            get { return detAutoSeccYY; }
            set
            {
                if (detAutoSeccYY != value)
                {
                    detAutoSeccYY = value;
                    OnPropertyChanged(nameof(DetAutoSeccYY));
                }
            }
        }

        public DetAutUserControlViewModelPlanta DetAutoPlanta
        {
            get { return detAutoPlanta; }
            set
            {
                if (detAutoPlanta != value)
                {
                    detAutoPlanta = value;
                    OnPropertyChanged(nameof(DetAutoPlanta));
                }
            }
        }

        public RelayCommand SeccionXXCommand => new RelayCommand(execute => CambiarVista(DetAutoSeccXX), canExecute => { return true; });

        public RelayCommand SeccionYYCommand => new RelayCommand(execute => CambiarVista(DetAutoSeccYY), canExecute => { return true; });

        public RelayCommand PlanoEstructuralCommand => new RelayCommand(execute => CambiarVista(DetAutoPlanta), canExecute => { return true; });

        private void CambiarVista(ViewModelBase VM)
        {
            VistaActual = VM;
        }

        public RelayCommand AceptarCommand => new RelayCommand(execute => CrearVistasYEtiquetas(execute), canExecute => { return true; });

        private void CrearVistasYEtiquetas(object parameter)
        {
            Modelo.CrearVistasYEtiquetas(this);

            if (parameter is Window window)
            {
                window.DialogResult = true;
                window.Close();
            }
        }
    }
}
