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
using System.Windows.Input;
using Jump.Models;
using System.Windows.Controls;

namespace Jump.ViewModels
{
    public class EnumeracionViewModel : ViewModelBase
    {
        private EnumeracionModel modelo;
        private ObservableCollection<ObjetoRevit> categorias = new ObservableCollection<ObjetoRevit>();
        private ObservableCollection<ObjetoRevit> parametros = new ObservableCollection<ObjetoRevit>();
        private ObservableCollection<Familia> elementos = new ObservableCollection<Familia>();
        private ObjetoRevit categoriaSeleccionada = null;
        private ObjetoRevit parametroSeleccionada = null;
        private bool paramEsNumero;
        private string prefijo = null;
        private double inicial = 0;
        private double incremento = 1;
        private string sufijo = null;

        public EnumeracionViewModel(EnumeracionModel model)
        {
            Modelo = model;

            Categorias = model.ObtenerCategorias();
        }

        public new EnumeracionModel Modelo
        {
            get { return modelo; }
            set { modelo = value; }
        }

        public ObservableCollection<ObjetoRevit> Categorias
        {
            get { return categorias; }
            set 
            {
                if (categorias != value)
                {
                    categorias = value;
                    OnPropertyChanged(nameof(Categorias));
                }
            }
        }

        public ObservableCollection<ObjetoRevit> Parametros
        {
            get { return parametros; }
            set
            {
                if (parametros != value)
                {
                    parametros = value;
                    OnPropertyChanged(nameof(Parametros));
                }
            }
        }

        public ObservableCollection<Familia> Elementos
        {
            get { return elementos; }
            set
            {
                if (elementos != value)
                {
                    elementos = value;
                    OnPropertyChanged(nameof(Elementos));
                }
            }
        }

        public ObjetoRevit CategoriaSeleccionada
        {
            get { return categoriaSeleccionada; }
            set
            {
                if (categoriaSeleccionada != value)
                {
                    categoriaSeleccionada = value;

                    Parametros = Modelo.ObtenerParametrosDeCategoria(value);

                    Elementos = Modelo.ObtenerTodosLosElementosDeCategoria(CategoriaSeleccionada);

                    OnPropertyChanged(nameof(CategoriaSeleccionada));
                }
            }
        }

        public ObjetoRevit ParametroSeleccionada
        {
            get { return parametroSeleccionada; }
            set
            {
                if (parametroSeleccionada != value)
                {
                    parametroSeleccionada = value;
                    OnPropertyChanged(nameof(ParametroSeleccionada));
                }
            }
        }

        public bool ParamEsNumero
        {
            get { return paramEsNumero; }
            set
            {
                if (paramEsNumero != value)
                {
                    paramEsNumero = value;
                    OnPropertyChanged(nameof(ParamEsNumero));
                }
            }
        }

        public string Prefijo 
        { 
            get { return prefijo; }
            set
            {
                if (prefijo != value)
                {
                    prefijo = value;
                    OnPropertyChanged(nameof(Prefijo));
                }
            }
        }

        public double Inicial
        {
            get { return inicial; }
            set
            {
                if (inicial != value)
                {
                    inicial = value;
                    OnPropertyChanged(nameof(Inicial));
                }
            }
        }

        public double Incremento
        {
            get { return incremento; }
            set
            {
                if (incremento != value)
                {
                    incremento = value;
                    OnPropertyChanged(nameof(Incremento));
                }
            }
        }

        public string Sufijo
        {
            get { return sufijo; }
            set
            {
                if (sufijo != value)
                {
                    sufijo = value;
                    OnPropertyChanged(nameof(Sufijo));
                }
            }
        }

        public RelayCommand<KeyEventArgs> TeclaCommand => new RelayCommand<KeyEventArgs>(TeclaPresionada);

        public void TeclaPresionada(KeyEventArgs e)
        {
            if (e.Key >= Key.A && e.Key <= Key.Z)
            {
                char letra = e.Key.ToString()[0];

                ObjetoRevit orTecla = Categorias.FirstOrDefault(x => x.Nombre.StartsWith(letra.ToString(), StringComparison.OrdinalIgnoreCase));
                
                if (orTecla != null)
                {
                    CategoriaSeleccionada = orTecla;

                    e.Handled = true;
                }
            }
        }
    }
}
