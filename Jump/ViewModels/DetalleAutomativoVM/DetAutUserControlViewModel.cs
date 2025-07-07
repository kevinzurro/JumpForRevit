using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Jump.Models;

namespace Jump.ViewModels
{
    public class DetAutUserControlViewModel : ViewModelBase
    {
        private DetalleAutomaticoModel modelo;
        private ViewModelBase vistaActual;
        private Familia vistaPrevia;

        private Familia tipoDeVista;
        private ObservableCollection<Familia> tiposDeVista;

        private bool escalabool = true;
        private int escala;
        private ObservableCollection<int> escalas;
        
        private Familia plantilla;
        private ObservableCollection<Familia> plantillas;
        
        private bool etiquetaElementobool;
        private Familia etiquetaElemento;
        private ObservableCollection<Familia> etiquetasParaElementos;
        
        private bool etiquetaArmadurabool;
        private Familia etiquetaArmadura;
        private ObservableCollection<Familia> etiquetasParaArmadura;
        
        private bool detalleArmadurabool;
        private Familia detalleArmadura;
        private ObservableCollection<Familia> detallesParaArmadura;
        
        private bool cotaLinealbool;
        private Familia cotaLineal;
        private ObservableCollection<Familia> cotasLineales;
        
        private bool cotaProfundidadbool;
        private Familia cotaProfundidad;
        private ObservableCollection<Familia> cotasParaProfundidad;

        private bool familiasTodasBool = true;
        private ObservableCollection<Familia> familiasTodas;

        private bool familiasSeleccionadasEnRevitBool;
        private ObservableCollection<Familia> familiasSeleccionadasEnRevit;

        private bool familiasListboxBool;
        private ObservableCollection<Familia> familiasListbox;
        private ObservableCollection<Familia> familiasSeleccionadas;

        public DetAutUserControlViewModel(DetalleAutomaticoModel model)
        {
            Modelo = model;

            FamiliasTodas = Modelo.ObtenerTodasLasFamilias();
            FamiliasSeleccionadasEnRevit = Modelo.ObtenerFamiliasSeleccionadasEnRevit();
            FamiliasListbox = new ObservableCollection<Familia>();
            
            FamiliasSeleccionadas = FamiliasTodas;
            VistaPrevia = FamiliasSeleccionadas.FirstOrDefault();

            Escalas = AboutJump.Escalas;
            Escala = Modelo.IndiceComboboxEscalaVista;

            PlantillasParaLaVista = Modelo.ObtenerTiposDePlantillas();
            PlantillaDeVista = PlantillasParaLaVista.FirstOrDefault();

            EtiquetasParaElementos = Modelo.ObtenerEtiquetasElemento();
            EtiquetaElemento = EtiquetasParaElementos.FirstOrDefault();

            EtiquetasParaArmadura = Modelo.ObtenerEtiquetasArmadura();
            EtiquetaArmadura = EtiquetasParaArmadura.FirstOrDefault();

            DetallesParaArmadura = Modelo.ObtenerTiposDeDetalleDeArmadura();
            DetalleArmadura = DetallesParaArmadura.FirstOrDefault();

            CotasLineales = Modelo.ObtenerTiposDeCotaLineales();
            CotaLineal = CotasLineales.FirstOrDefault();

            CotasParaProfundidad = Modelo.ObtenerTiposDeCotaDeElevacion();
            CotaProfundidad = CotasParaProfundidad.FirstOrDefault();
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
                vistaActual = value;
                OnPropertyChanged(nameof(VistaActual));
            }
        }

        public Familia VistaPrevia
        {
            get { return vistaPrevia; }
            set
            {
                vistaPrevia = value;
                OnPropertyChanged(nameof(VistaPrevia));
            }
        }

        public Familia TipoDeVista
        {
            get { return tipoDeVista; }
            set
            {
                if (tipoDeVista != value)
                {
                    tipoDeVista = value;
                    OnPropertyChanged(nameof(TipoDeVista));
                }
            }
        }

        public ObservableCollection<Familia> TiposDeVistas
        {
            get { return tiposDeVista; }
            set
            {
                if (tiposDeVista != value)
                {
                    tiposDeVista = value;
                    OnPropertyChanged(nameof(TiposDeVistas));
                }
            }
        }

        public bool EscalaBool
        {
            get { return escalabool; }
            set
            {
                if (escalabool != value)
                {
                    escalabool = value;
                    OnPropertyChanged(nameof(EscalaBool));
                    OnPropertyChanged(nameof(PlantillaDeVistaBool));
                }
            }
        }

        public int Escala
        {
            get { return escala; }
            set
            {
                if (escala != value)
                {
                    escala = value;

                    Modelo.IndiceComboboxEscalaVista = value;

                    OnPropertyChanged(nameof(Escala));
                }
            }
        }

        public ObservableCollection<int> Escalas
        {
            get { return escalas; }
            set
            {
                if (escalas != value)
                {
                    escalas = value;
                    OnPropertyChanged(nameof(Escalas));
                }
            }
        }

        public bool PlantillaDeVistaBool
        {
            get { return !escalabool; }
            set
            {
                if (!escalabool != value)
                {
                    escalabool = !value;
                    OnPropertyChanged(nameof(EscalaBool));
                    OnPropertyChanged(nameof(PlantillaDeVistaBool));
                }
            }
        }

        public Familia PlantillaDeVista
        {
            get { return plantilla; }
            set
            {
                if (plantilla != value)
                {
                    plantilla = value;
                    OnPropertyChanged(nameof(PlantillaDeVista));
                }
            }
        }

        public ObservableCollection<Familia> PlantillasParaLaVista
        {
            get { return plantillas; }
            set
            {
                if (plantillas != value)
                {
                    plantillas = value;
                    OnPropertyChanged(nameof(PlantillasParaLaVista));
                }
            }
        }

        public bool EtiquetaElementoBool
        {
            get { return etiquetaElementobool; }
            set
            {
                if (etiquetaElementobool != value)
                {
                    etiquetaElementobool = value;
                    OnPropertyChanged(nameof(EtiquetaElementoBool));
                }
            }
        }

        public Familia EtiquetaElemento
        {
            get { return etiquetaElemento; }
            set
            {
                if (etiquetaElemento != value)
                {
                    etiquetaElemento = value;
                    OnPropertyChanged(nameof(EtiquetaElemento));
                }
            }
        }

        public ObservableCollection<Familia> EtiquetasParaElementos
        {
            get { return etiquetasParaElementos; }
            set
            {
                if (etiquetasParaElementos != value)
                {
                    etiquetasParaElementos = value;
                    OnPropertyChanged(nameof(EtiquetasParaElementos));
                }
            }
        }

        public bool EtiquetaArmaduraBool
        {
            get { return etiquetaArmadurabool; }
            set
            {
                if (etiquetaArmadurabool != value)
                {
                    etiquetaArmadurabool = value;
                    OnPropertyChanged(nameof(EtiquetaArmaduraBool));
                }
            }
        }

        public Familia EtiquetaArmadura
        {
            get { return etiquetaArmadura; }
            set
            {
                if (etiquetaArmadura != value)
                {
                    etiquetaArmadura = value;
                    OnPropertyChanged(nameof(EtiquetaArmadura));
                }
            }
        }

        public ObservableCollection<Familia> EtiquetasParaArmadura
        {
            get { return etiquetasParaArmadura; }
            set
            {
                if (etiquetasParaArmadura != value)
                {
                    etiquetasParaArmadura = value;
                    OnPropertyChanged(nameof(EtiquetasParaArmadura));
                }
            }
        }

        public bool DetalleArmaduraBool
        {
            get { return detalleArmadurabool; }
            set
            {
                if (detalleArmadurabool != value)
                {
                    detalleArmadurabool = value;
                    OnPropertyChanged(nameof(DetalleArmaduraBool));
                }
            }
        }

        public Familia DetalleArmadura
        {
            get { return detalleArmadura; }
            set
            {
                if (detalleArmadura != value)
                {
                    detalleArmadura = value;
                    OnPropertyChanged(nameof(DetalleArmadura));
                }
            }
        }

        public ObservableCollection<Familia> DetallesParaArmadura
        {
            get { return detallesParaArmadura; }
            set
            {
                if (detallesParaArmadura != value)
                {
                    detallesParaArmadura = value;
                    OnPropertyChanged(nameof(DetallesParaArmadura));
                }
            }
        }

        public bool CotaLinealBool
        {
            get { return cotaLinealbool; }
            set
            {
                if (cotaLinealbool != value)
                {
                    cotaLinealbool = value;
                    OnPropertyChanged(nameof(CotaLinealBool));
                }
            }
        }

        public Familia CotaLineal
        {
            get { return cotaLineal; }
            set
            {
                if (cotaLineal != value)
                {
                    cotaLineal = value;
                    OnPropertyChanged(nameof(CotaLineal));
                }
            }
        }

        public ObservableCollection<Familia> CotasLineales
        {
            get { return cotasLineales; }
            set
            {
                if (cotasLineales != value)
                {
                    cotasLineales = value;
                    OnPropertyChanged(nameof(CotasLineales));
                }
            }
        }

        public bool CotaProfundidadBool
        {
            get { return cotaProfundidadbool; }
            set
            {
                if (cotaProfundidadbool != value)
                {
                    cotaProfundidadbool = value;
                    OnPropertyChanged(nameof(CotaProfundidadBool));
                }
            }
        }

        public Familia CotaProfundidad
        {
            get { return cotaProfundidad; }
            set
            {
                if (cotaProfundidad != value)
                {
                    cotaProfundidad = value;
                    OnPropertyChanged(nameof(CotaProfundidad));
                }
            }
        }

        public ObservableCollection<Familia> CotasParaProfundidad
        {
            get { return cotasParaProfundidad; }
            set
            {
                if (cotasParaProfundidad != value)
                {
                    cotasParaProfundidad = value;
                    OnPropertyChanged(nameof(CotasParaProfundidad));
                }
            }
        }

        public bool FamiliasTodasBool
        {
            get { return familiasTodasBool; }
            set
            {
                if (familiasTodasBool != value)
                {
                    familiasTodasBool = value;
                    OnPropertyChanged(nameof(FamiliasTodasBool));
                }
            }
        }

        public ObservableCollection<Familia> FamiliasTodas
        {
            get { return familiasTodas; }
            set
            {
                if (familiasTodas != value)
                {
                    familiasTodas = value;
                    OnPropertyChanged(nameof(FamiliasTodas));
                }
            }
        }

        public bool FamiliasSeleccionadasEnRevitBool
        {
            get { return familiasSeleccionadasEnRevitBool; }
            set
            {
                if (familiasSeleccionadasEnRevitBool != value)
                {
                    familiasSeleccionadasEnRevitBool = value;
                    OnPropertyChanged(nameof(FamiliasSeleccionadasEnRevitBool));
                }
            }
        }

        public ObservableCollection<Familia> FamiliasSeleccionadasEnRevit
        {
            get { return familiasSeleccionadasEnRevit; }
            set
            {
                if (familiasSeleccionadasEnRevit != value)
                {
                    familiasSeleccionadasEnRevit = value;
                    OnPropertyChanged(nameof(FamiliasSeleccionadasEnRevit));
                }
            }
        }

        public bool FamiliasListboxBool
        {
            get { return familiasListboxBool; }
            set
            {
                if (familiasListboxBool != value)
                {
                    familiasListboxBool = value;
                    OnPropertyChanged(nameof(FamiliasListboxBool));
                }
            }
        }

        public ObservableCollection<Familia> FamiliasListbox
        {
            get { return familiasListbox; }
            set
            {
                if (familiasListbox != value)
                {
                    familiasListbox = value;

                    OnPropertyChanged(nameof(FamiliasListbox));
                }
            }
        }

        public ObservableCollection<Familia> FamiliasSeleccionadas
        {
            get { return familiasSeleccionadas; }
            set
            {
                if (familiasSeleccionadas != value)
                {
                    familiasSeleccionadas = value;
                    OnPropertyChanged(nameof(FamiliasSeleccionadas));
                }
            }
        }

        public RelayCommand FamiliasTodasCommand => new RelayCommand(execute => CambiarFamiliasSeleccionadas(FamiliasTodas), canExecute => { return true; });

        public RelayCommand FamiliasSeleccionadasEnRevitCommand => new RelayCommand(execute => CambiarFamiliasSeleccionadas(FamiliasSeleccionadasEnRevit), canExecute => { return true; });

        public RelayCommand FamiliasListboxCommand => new RelayCommand(execute => CambiarFamiliasSeleccionadas(FamiliasListbox), canExecute => { return true; });

        public RelayCommand FamiliasListboxSeleccionCommand => new RelayCommand(execute => CambiarFamiliasListbox(FamiliasListbox), canExecute => { return true; });

        private void CambiarFamiliasSeleccionadas(ObservableCollection<Familia> familias)
        {
            FamiliasSeleccionadas = familias;

            VistaPrevia = FamiliasSeleccionadas.FirstOrDefault();
        }

        private void CambiarFamiliasListbox(ObservableCollection<Familia> familias)
        {
            if (FamiliasListboxBool)
            {
                CambiarFamiliasSeleccionadas(familias);
            }
        }
    }
}
