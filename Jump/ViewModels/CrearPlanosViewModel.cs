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
    public class CrearPlanosViewModel : ViewModelBase
    {
        private ObservableCollection<MenuItem> vistasParaMostrar;
        private ObservableCollection<Familia> vistasSeleccionadas;
        private ObservableCollection<Familia> planos;
        private bool unPlanoPorVista = true;
        private Familia tipoPlano;
        private VistasModel mVistas;

        public CrearPlanosViewModel(VistasModel modelo)
        {
            ModeloVista = modelo;
        }

        public VistasModel ModeloVista
        {
            get { return mVistas; }
            set 
            {
                mVistas = value as VistasModel;

                VistasParaMostrar = mVistas.ObtenerVistasHabilitadasParaPlanos();
                
                Planos = mVistas.ObtenerTodosLosTiposDePlanos();
            }
        }

        public ObservableCollection<MenuItem> VistasParaMostrar
        {
            get { return vistasParaMostrar; }
            set
            {
                if (vistasParaMostrar != value)
                {
                    vistasParaMostrar = value;
                    OnPropertyChanged(nameof(VistasParaMostrar));
                }
            }
        }

        public ObservableCollection<Familia> VistasSeleccionadas
        {
            get { return vistasSeleccionadas; }
            set
            {
                if (vistasSeleccionadas != value)
                {
                    vistasSeleccionadas = value;
                    OnPropertyChanged(nameof(VistasSeleccionadas));
                }
            }
        }

        public ObservableCollection<Familia> Planos
        {
            get { return planos; }
            set
            {
                if (planos != value)
                {
                    planos = value;
                    OnPropertyChanged(nameof(Planos));
                }
            }
        }

        public Familia TipoPlano
        {
            get { return tipoPlano; }
            set
            {
                if (tipoPlano != value)
                {
                    tipoPlano = value;
                    OnPropertyChanged(nameof(TipoPlano));
                }
            }
        }

        public bool UnPlanoPorVista
        {
            get { return unPlanoPorVista; }
            set
            {
                if (unPlanoPorVista != value)
                {
                    unPlanoPorVista = value;
                    OnPropertyChanged(nameof(UnPlanoPorVista));
                }
            }
        }

        public RelayCommand TodosCommand => new RelayCommand(execute => TodosTreeView(), canExecute => { return true; });

        public RelayCommand NingunoCommand => new RelayCommand(execute => NingunoTreeView(), canExecute => { return true; });

        public RelayCommand ExpandirCommand => new RelayCommand(execute => ExpandirTreeView(), canExecute => { return true; });

        public RelayCommand ContraerCommand => new RelayCommand(execute => ContraerTreeView(), canExecute => { return true; });

        public RelayCommand CrearPlanosCommand => new RelayCommand(execute => CrearPlanos(execute), canExecute => { return true; });
        
        private void TodosTreeView()
        {
            foreach (MenuItem item in VistasParaMostrar)
            {
                item.IsChecked = true;
            }
        }

        private void NingunoTreeView()
        {
            foreach (MenuItem item in VistasParaMostrar)
            {
                item.IsChecked = false;
            }
        }

        private void ExpandirTreeView()
        {
            foreach (MenuItem item in VistasParaMostrar)
            {
                item.IsExpanded = true;
            }
        }

        private void ContraerTreeView()
        {
            foreach (MenuItem item in VistasParaMostrar)
            {
                item.IsExpanded = false;
            }
        }

        private void CrearPlanos(object parameter)
        {
            VistasSeleccionadas = new ObservableCollection<Familia>();

            foreach (MenuItem item in VistasParaMostrar)
            {
                foreach (MenuItem itemHijo in item.Items)
                {
                    if (itemHijo.IsChecked)
                    {
                        VistasSeleccionadas.Add(itemHijo.Familia);
                    }
                }
            }

            if (TipoPlano != null && VistasSeleccionadas.Count > 0)
            {
                mVistas.CrearPlanos(VistasSeleccionadas, TipoPlano, UnPlanoPorVista);
            }

            if (parameter is Window window)
            {
                window.DialogResult = true;
                window.Close();
            }
        }
    }
}
