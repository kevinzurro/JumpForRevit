using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.ApplicationServices;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;

namespace Jump.ViewModels
{
    public class NodoAnaliticoVerificacionViewModel : ViewModelBase
    {
        private ObservableCollection<NodoAnalitico> todosNodos = new ObservableCollection<NodoAnalitico>();
        private ObservableCollection<Familia> tiposDeVinculosAnaliticos = new ObservableCollection<Familia>();
        private NodoAnalitico nodoPrincipal = null;
        private NodoAnalitico nodoSeleccionado = null;
        private Familia tipoVinculoSeleccionado = null;

        public NodoAnaliticoVerificacionViewModel(NodoAnaliticoModel model)
        {
            Modelo = model;

            TodosLosNodos = model.ObtenerNodosConCercania();

            TiposDeVinculosAnaliticos = model.ObtenerVinculosAnaliticos();

            if (TiposDeVinculosAnaliticos.Count >= 1)
            {
                tipoVinculoSeleccionado = TiposDeVinculosAnaliticos.First();
            }
        }

        public ObservableCollection<NodoAnalitico> TodosLosNodos
        {
            get { return todosNodos; }
            set
            {
                if (todosNodos != value)
                {
                    todosNodos = value;
                    OnPropertyChanged(nameof(TodosLosNodos));
                }
            }
        }

        public ObservableCollection<Familia> TiposDeVinculosAnaliticos
        {
            get { return tiposDeVinculosAnaliticos; }
            set
            {
                if (tiposDeVinculosAnaliticos != value)
                {
                    tiposDeVinculosAnaliticos = value;
                    OnPropertyChanged(nameof(TiposDeVinculosAnaliticos));
                }
            }
        }

        public NodoAnalitico NodoPrincipal
        {
            get { return nodoPrincipal; }
            set
            {
                if (nodoPrincipal != value)
                {
                    nodoPrincipal = value;
                    OnPropertyChanged(nameof(NodoPrincipal));
                }
            }
        }

        public NodoAnalitico NodoSeleccionado 
        { 
            get { return nodoSeleccionado; }
            set
            {
                if (nodoSeleccionado != value)
                {
                    nodoSeleccionado = value;
                    OnPropertyChanged(nameof(NodoSeleccionado));
                }
            }
        }

        public Familia TipoVinculoSeleccionado
        {
            get { return tipoVinculoSeleccionado; }
            set
            {
                if (tipoVinculoSeleccionado != value)
                {
                    tipoVinculoSeleccionado = value;
                    OnPropertyChanged(nameof(TipoVinculoSeleccionado));
                }
            }
        }

        public RelayCommand AislarCommand => new RelayCommand(execute => Aislar(), canExecute => { return true; });

        public RelayCommand MoverCommand => new RelayCommand(execute => Mover(), canExecute => { return true; });

        public RelayCommand UnirCommand => new RelayCommand(execute => Unir(), canExecute => { return true; });

        public RelayCommand AceptarCommand => new RelayCommand(execute => Aceptar(execute), canExecute => { return true; });

        private void Aislar()
        {
            (Modelo as NodoAnaliticoModel).AislarNodosEnLaVista(NodoPrincipal, NodoSeleccionado);
        }

        private void Mover()
        {
            (Modelo as NodoAnaliticoModel).MoverNodos(NodoPrincipal, NodoSeleccionado);
        }

        private void Unir()
        {
            (Modelo as NodoAnaliticoModel).UnirNodos(NodoPrincipal, NodoSeleccionado, TipoVinculoSeleccionado);
        }

        private void Aceptar(object parameter)
        {
            if (parameter is Window window)
            {
                window.DialogResult = true;
                window.Close();
            }
        }
    }
}
