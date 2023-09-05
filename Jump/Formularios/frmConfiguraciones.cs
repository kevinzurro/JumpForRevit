using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Resources;
using System.Windows.Forms;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.Structure;

namespace Jump
{
    public partial class frmConfiguraciones : System.Windows.Forms.Form
    {
        // Variable necesarias
        string IdiomaDelPrograma;
        Document doc;
        ForgeTypeId tipoUnidad;
        public bool bandera = false;
        List<RebarBarType> diametros = new List<RebarBarType>();
        List<string> posiciones;
        string coma = ",";

        // Constructor del formulario
        public frmConfiguraciones(Document doc)
        {
            InitializeComponent();

            // Variable necesarias
            this.IdiomaDelPrograma = Tools.ObtenerIdiomaDelPrograma();
            this.doc = doc;
            this.diametros = Tools.ObtenerTodosTiposSegunClase(doc, typeof(RebarBarType)).Cast<RebarBarType>().ToList();
            FormatOptions forOpt = doc.GetUnits().GetFormatOptions(SpecTypeId.Length);
            this.tipoUnidad = forOpt.GetUnitTypeId();
            this.posiciones = AboutJump.Posiciones(this.IdiomaDelPrograma);

            // Llama a las funciones
            //this.dgvEstiloLinea = Tools.ObtenerDataGridViewDeDiametrosYEstilos(this.dgvEstiloLinea, doc, IdiomaDelPrograma);
            CargarImagenesPredeterminadas();
        }

        /// <summary> Carga las posiciones para etiquetas profundidad </summary>
        private void CargarCotaProfundidad(System.Windows.Forms.ComboBox combo, int posicion)
        {
            combo.Items.Add(Language.ObtenerTexto(this.IdiomaDelPrograma, "Pos" + ((int)Posicion.AbajoIzquierda).ToString()));
            combo.Items.Add(Language.ObtenerTexto(this.IdiomaDelPrograma, "Pos" + ((int)Posicion.AbajoDerecha).ToString()));

            string seleccion = posiciones[posicion];

            combo.SelectedItem = seleccion;
        }

        /// <summary> Obtiene la posición para etiquetas profundidad </summary>
        private int ObtenerPosicionCotaProfundidad(System.Windows.Forms.ComboBox combo)
        {
            int posicion = 0;

            for (int i = 0; i < posiciones.Count; i++)
            {
                try
                {
                    if (posiciones[i].ToString() == combo.SelectedItem.ToString())
                    {
                        posicion = i;

                        break;
                    }
                }
                catch (Exception) { continue; }
            }

            return posicion;
        }

        /// <summary> Asigna las imagenes predeterminadas cuando carga el formulario </summary>
        private void CargarImagenesPredeterminadas()
        {
            // Asigna la imagen
            this.pcbxGeneral.BackgroundImage = Jump.Iconos_e_Imagenes.Imagenes.Configuraciones_Precision;
            this.pcbxEtiquetaPosicion.BackgroundImage = Jump.Iconos_e_Imagenes.Imagenes.Configuraciones_Viga;
            this.pcbxCotaPosicion.BackgroundImage = Jump.Iconos_e_Imagenes.Imagenes.Configuraciones_Viga;
        }

        /// <summary> Carga el formulario </summary>
        private void frmConfiguraciones_Load(object sender, EventArgs e)
        {
            this.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Conf4");
            btnAceptar.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Conf5");
            btnCancelar.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Conf6");

            // Pestaña General
            tbpgGeneral.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Conf1-1");
            gbxConfiguraciones.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Conf1-2-1");
            lblprecisionOrdenar.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Conf1-2-2");
            lblPrecisionOrdenarDescripcion.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Conf1-2-3");
            lblPresicionOrdenarUnidadX.Text = LabelUtils.GetLabelForUnit(this.tipoUnidad);
            lblPresicionOrdenarUnidadY.Text = LabelUtils.GetLabelForUnit(this.tipoUnidad);
            gbxVista.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Conf1-3-1");
            rbtnVistaLocal.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Conf1-3-2");
            rbtnVistaGlobal.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Conf1-3-3");

            double precisionX = Properties.Settings.Default.precisionOrdenarX;
            double precisionY = Properties.Settings.Default.precisionOrdenarY;

            this.pcbxGeneral.BackgroundImage = Iconos_e_Imagenes.Imagenes.Configuraciones_Precision;
            this.txtPrecisionOrdenarX.Text = UnitUtils.ConvertFromInternalUnits(precisionX, this.tipoUnidad).ToString();
            this.txtPrecisionOrdenarY.Text = UnitUtils.ConvertFromInternalUnits(precisionY, this.tipoUnidad).ToString();
            this.rbtnVistaGlobal.Checked = Properties.Settings.Default.rbtnGeneralVistaGlobal;
            this.rbtnVistaLocal.Checked = Properties.Settings.Default.rbtnGeneralVistaLocal;

            gbxArmadura.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Conf2-1");
            lblArmaduraEnumeracion.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Conf2-2-1");
            rbtnEnumeracionElemento.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Conf2-2-2");
            rbtnEnumeracionProyecto.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Conf2-2-3");
            lblArmaduraDibujo.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Conf2-3-1");
            rbtnLineasCentrales.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Conf2-3-2");
            rbtnLineasBorde.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Conf2-3-3");
            lblArmaduraPosicionTexto.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Conf2-4-1");
            rbtnTextoArriba.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Conf2-4-2");
            rbtnTextoAbajo.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Conf2-4-3");

            this.rbtnEnumeracionElemento.Checked = Properties.Settings.Default.rbtnArmaduraEnumeracionPorElemento;
            this.rbtnEnumeracionProyecto.Checked = Properties.Settings.Default.rbtnArmaduraEnumeracionPorProyecto;
            this.rbtnLineasCentrales.Checked = Properties.Settings.Default.rbtnArmaduraDibujoLineasCentrales;
            this.rbtnLineasBorde.Checked = Properties.Settings.Default.rbtnArmaduraDibujoLineasBorde;
            this.rbtnTextoArriba.Checked = Properties.Settings.Default.rbtnTextoArmaduraArriba;
            this.rbtnTextoAbajo.Checked = Properties.Settings.Default.rbtnTextoArmaduraAbajo;

            // Pestaña Estilos de líneas
            //tbpgEstiloLinea.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Conf3-1");
            //lblDiametroEstilo.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Conf3-4");

            // Pestaña Etiquetas de elementos
            tbpgEtiquetas.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Conf4-1");
            gbxEtiquetaIndependiente.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Conf4-1-1");
            lblElementoEtiquetasIdependientes.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Conf4-1-2");
            lblPosicionEtiqueta.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Conf4-1-3");
            lblPiloteEtiqueta.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Conf4-1-1-1");
            lblZapataCorridaEtiqueta.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Conf4-1-1-2");
            lblPlateaEtiqueta.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Conf4-1-1-3");
            lblZapataEtiqueta.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Conf4-1-1-4");
            lblColumnaEtiqueta.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Conf4-1-1-5");
            lblMuroEtiqueta.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Conf4-1-1-6");
            lblVigaEtiqueta.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Conf4-1-1-7");
            lblLosaEtiqueta.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Conf4-1-1-8");
            gbxEtiquetaArmadura.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Conf4-2-1");
            lblArmaduraEtiquetaIndependiente.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Conf4-2-2");
            lblArmaduraPosicionEtiqueta.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Conf4-2-3");
            lblArmaduraEtiqueta.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Conf4-2-1-1");
            lblAreaRefuerzoEtiqueta.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Conf4-2-1-2");
            lblArmaduraEnSistemaEtiqueta.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Conf4-2-1-3");

            // Agrega la lista de posiciones a la lista desplegable
            this.cmbArmaduraEtiqueta.DataSource = AboutJump.Posiciones(this.IdiomaDelPrograma);
            this.cmbAreaRefuerzoEtiqueta.DataSource = AboutJump.Posiciones(this.IdiomaDelPrograma);
            this.cmbArmaduraEnSistemaEtiqueta.DataSource = AboutJump.Posiciones(this.IdiomaDelPrograma);
            this.cmbColumnaEtiqueta.DataSource = AboutJump.Posiciones(this.IdiomaDelPrograma);
            this.cmbLosaEtiqueta.DataSource = AboutJump.Posiciones(this.IdiomaDelPrograma);
            this.cmbMuroEtiqueta.DataSource = AboutJump.Posiciones(this.IdiomaDelPrograma);
            this.cmbPiloteEtiqueta.DataSource = AboutJump.Posiciones(this.IdiomaDelPrograma);
            this.cmbPlateaEtiqueta.DataSource = AboutJump.Posiciones(this.IdiomaDelPrograma);
            this.cmbVigaEtiqueta.DataSource = AboutJump.Posiciones(this.IdiomaDelPrograma);
            this.cmbZapataEtiqueta.DataSource = AboutJump.Posiciones(this.IdiomaDelPrograma);
            this.cmbZapataCorridaEtiqueta.DataSource = AboutJump.Posiciones(this.IdiomaDelPrograma);

            // Asigna el indice de la lista desplegable
            this.cmbArmaduraEtiqueta.SelectedIndex = Properties.Settings.Default.ArmaduraEtiquetaIndependiente;
            this.cmbAreaRefuerzoEtiqueta.SelectedIndex = Properties.Settings.Default.AreaRefuerzoEtiquetaIndependiente;
            this.cmbArmaduraEnSistemaEtiqueta.SelectedIndex = Properties.Settings.Default.ArmaduraEnSistemaEtiquetaIndependiente;
            this.cmbColumnaEtiqueta.SelectedIndex = Properties.Settings.Default.ColumnaEtiquetaIndependiente;
            this.cmbLosaEtiqueta.SelectedIndex = Properties.Settings.Default.LosaEtiquetaIndependiente;
            this.cmbMuroEtiqueta.SelectedIndex = Properties.Settings.Default.MuroEtiquetaIndependiente;
            this.cmbPiloteEtiqueta.SelectedIndex = Properties.Settings.Default.PiloteEtiquetaIndependiente;
            this.cmbPlateaEtiqueta.SelectedIndex = Properties.Settings.Default.PlateaEtiquetaIndependiente;
            this.cmbVigaEtiqueta.SelectedIndex = Properties.Settings.Default.VigaEtiquetaIndependiente;
            this.cmbZapataEtiqueta.SelectedIndex = Properties.Settings.Default.ZapataEtiquetaIndependiente;
            this.cmbZapataCorridaEtiqueta.SelectedIndex = Properties.Settings.Default.ZapataCorridaEtiquetaIndependiente;

            // Pestaña Cotas
            tbpgCotas.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Conf5-1");
            gbxCotasLineal.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Conf5-1-1");
            lblCotasLineales.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Conf5-1-2");
            lblCotasProfundidad.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Conf5-1-3");
            lblPiloteCotaLineal.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Conf5-1-1-1");

            // Asigna los textos a las posiciones de las cotas lineales
            this.chbVigaCotaArriba.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Pos10");
            this.chbVigaCotaAbajo.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Pos11");
            this.chbVigaCotaIzquierda.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Pos12");
            this.chbVigaCotaDerecha.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Pos13");
            this.chbMuroCotaArriba.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Pos10");
            this.chbMuroCotaAbajo.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Pos11");
            this.chbMuroCotaIzquierda.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Pos12");
            this.chbMuroCotaDerecha.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Pos13");
            this.chbColumnaCotaArriba.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Pos10");
            this.chbColumnaCotaAbajo.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Pos11");
            this.chbColumnaCotaIzquierda.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Pos12");
            this.chbColumnaCotaDerecha.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Pos13");
            this.chbLosaCotaArriba.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Pos10");
            this.chbLosaCotaAbajo.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Pos11");
            this.chbLosaCotaIzquierda.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Pos12");
            this.chbLosaCotaDerecha.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Pos13");
            this.chbZapataCotaArriba.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Pos10");
            this.chbZapataCotaAbajo.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Pos11");
            this.chbZapataCotaIzquierda.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Pos12");
            this.chbZapataCotaDerecha.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Pos13");
            this.chbZapataCorridaCotaArriba.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Pos10");
            this.chbZapataCorridaCotaAbajo.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Pos11");
            this.chbZapataCorridaCotaIzquierda.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Pos12");
            this.chbZapataCorridaCotaDerecha.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Pos13");
            this.chbPlateaCotaArriba.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Pos10");
            this.chbPlateaCotaAbajo.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Pos11");
            this.chbPlateaCotaIzquierda.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Pos12");
            this.chbPlateaCotaDerecha.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Pos13");
            this.chbPiloteCotaArriba.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Pos10");
            this.chbPiloteCotaAbajo.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Pos11");
            this.chbPiloteCotaIzquierda.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Pos12");
            this.chbPiloteCotaDerecha.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Pos13");

            // Asigna los estados de Check a las cotas lineales
            this.chbVigaCotaArriba.Checked = Properties.Settings.Default.VigaCotaLinealArriba;
            this.chbVigaCotaAbajo.Checked = Properties.Settings.Default.VigaCotaLinealAbajo;
            this.chbVigaCotaIzquierda.Checked = Properties.Settings.Default.VigaCotaLinealIzquierda;
            this.chbVigaCotaDerecha.Checked = Properties.Settings.Default.VigaCotaLinealDerecha;
            this.chbMuroCotaArriba.Checked = Properties.Settings.Default.MuroCotaLinealArriba;
            this.chbMuroCotaAbajo.Checked = Properties.Settings.Default.MuroCotaLinealAbajo;
            this.chbMuroCotaIzquierda.Checked = Properties.Settings.Default.MuroCotaLinealIzquierda;
            this.chbMuroCotaDerecha.Checked = Properties.Settings.Default.MuroCotaLinealDerecha;
            this.chbColumnaCotaArriba.Checked = Properties.Settings.Default.ColumnaCotaLinealArriba;
            this.chbColumnaCotaAbajo.Checked = Properties.Settings.Default.ColumnaCotaLinealAbajo;
            this.chbColumnaCotaIzquierda.Checked = Properties.Settings.Default.ColumnaCotaLinealIzquierda;
            this.chbColumnaCotaDerecha.Checked = Properties.Settings.Default.ColumnaCotaLinealDerecha;
            this.chbLosaCotaArriba.Checked = Properties.Settings.Default.LosaCotaLinealArriba;
            this.chbLosaCotaAbajo.Checked = Properties.Settings.Default.LosaCotaLinealAbajo;
            this.chbLosaCotaIzquierda.Checked = Properties.Settings.Default.LosaCotaLinealIzquierda;
            this.chbLosaCotaDerecha.Checked = Properties.Settings.Default.LosaCotaLinealDerecha;
            this.chbZapataCotaArriba.Checked = Properties.Settings.Default.ZapataCotaLinealArriba;
            this.chbZapataCotaAbajo.Checked = Properties.Settings.Default.ZapataCotaLinealAbajo;
            this.chbZapataCotaIzquierda.Checked = Properties.Settings.Default.ZapataCotaLinealIzquierda;
            this.chbZapataCotaDerecha.Checked = Properties.Settings.Default.ZapataCotaLinealDerecha;
            this.chbZapataCorridaCotaArriba.Checked = Properties.Settings.Default.ZapataCorridaCotaLinealArriba;
            this.chbZapataCorridaCotaAbajo.Checked = Properties.Settings.Default.ZapataCorridaCotaLinealAbajo;
            this.chbZapataCorridaCotaIzquierda.Checked = Properties.Settings.Default.ZapataCorridaCotaLinealIzquierda;
            this.chbZapataCorridaCotaDerecha.Checked = Properties.Settings.Default.ZapataCorridaCotaLinealDerecha;
            this.chbPlateaCotaArriba.Checked = Properties.Settings.Default.PlateaCotaLinealArriba;
            this.chbPlateaCotaAbajo.Checked = Properties.Settings.Default.PlateaCotaLinealAbajo;
            this.chbPlateaCotaIzquierda.Checked = Properties.Settings.Default.PlateaCotaLinealIzquierda;
            this.chbPlateaCotaDerecha.Checked = Properties.Settings.Default.PlateaCotaLinealDerecha;
            this.chbPiloteCotaArriba.Checked = Properties.Settings.Default.PiloteCotaLinealArriba;
            this.chbPiloteCotaAbajo.Checked = Properties.Settings.Default.PiloteCotaLinealAbajo;
            this.chbPiloteCotaIzquierda.Checked = Properties.Settings.Default.PiloteCotaLinealIzquierda;
            this.chbPiloteCotaDerecha.Checked = Properties.Settings.Default.PiloteCotaLinealDerecha;

            // Agrega las posiciones de cota profundidad
            CargarCotaProfundidad(this.cmbVigaCotaProfundidad, Properties.Settings.Default.VigaEtiquetaCotaProfundidad);
            CargarCotaProfundidad(this.cmbMuroCotaProfundidad, Properties.Settings.Default.MuroEtiquetaCotaProfundidad);
            CargarCotaProfundidad(this.cmbColumnaCotaProfundidad, Properties.Settings.Default.ColumnaEtiquetaCotaProfundidad);
            CargarCotaProfundidad(this.cmbLosaCotaProfundidad, Properties.Settings.Default.LosaEtiquetaCotaProfundidad);
            CargarCotaProfundidad(this.cmbZapataCotaProfundidad, Properties.Settings.Default.ZapataEtiquetaCotaProfundidad);
            CargarCotaProfundidad(this.cmbZapataCorridaCotaProfundidad, Properties.Settings.Default.ZapataCorridaEtiquetaCotaProfundidad);
            CargarCotaProfundidad(this.cmbPlateaCotaProfundidad, Properties.Settings.Default.PlateaEtiquetaCotaProfundidad);
            CargarCotaProfundidad(this.cmbPiloteCotaProfundidad, Properties.Settings.Default.PiloteEtiquetaCotaProfundidad);
        }

        /// <summary> Valida que los textos ingresados sean solamente números </summary>
        private void VerificarSoloNumero(object sender, KeyPressEventArgs e)
        {
            // Verifica que solo se ingresen numeros en el TextBox
            Tools.VerificarSoloNumero(e);

            string caracter = (doc.GetUnits().DecimalSymbol.ToString() == "Dot") ? ".": coma;

            if (e.KeyChar.ToString() == caracter && !(sender as System.Windows.Forms.TextBox).Text.Contains(coma))
            {
                e.KeyChar = Char.Parse(coma);

                e.Handled = false;
            }
        }

        /// <summary> Cambia la imagen cuando el mouse pasa por arriba del radiobutton </summary>
        private void CambiarImagenGeneral_MouseMove(object sender, MouseEventArgs e)
        {
            // Asigna la imagen
            this.pcbxGeneral.BackgroundImage = Jump.Iconos_e_Imagenes.Imagenes.Configuraciones_Precision;
        }

        /// <summary> Cambia la imagen cuando el mouse pasa por arriba del radiobutton </summary>
        private void CambiarImagen_MouseMove(object sender, MouseEventArgs e)
        {
            // Verifica que sea el radiobutton enumeración por proyecto
            if (sender == this.rbtnEnumeracionProyecto)
            {
                //this.pcbxGeneral.BackgroundImage = Jump.Iconos_e_Imagenes.Imagenes.;
            }

            // Verifica que sea el radiobutton enumeración por elemento
            else if (sender == this.rbtnEnumeracionElemento)
            {
                //this.pcbxGeneral.BackgroundImage = Jump.Iconos_e_Imagenes.Imagenes.;
            }

            // Verifica que sea el radiobutton de corte local
            else if (sender == this.rbtnVistaLocal)
            {
                this.pcbxGeneral.BackgroundImage = Jump.Iconos_e_Imagenes.Imagenes.Configuraciones_Vista_Local;
            }

            // Verifica que sea el radiobutton de corte global
            else if (sender == this.rbtnVistaGlobal)
            {
                this.pcbxGeneral.BackgroundImage = Jump.Iconos_e_Imagenes.Imagenes.Configuraciones_Vista_Global;
            }

            // Verifica que sea el radiobutton lineas centrales
            else if (sender == this.rbtnLineasCentrales)
            {
                this.pcbxGeneral.BackgroundImage = Jump.Iconos_e_Imagenes.Imagenes.Configuraciones_LineasCentrales;
            }

            // Verifica que sea el radiobutton lineas de borde
            else if (sender == this.rbtnLineasBorde)
            {
                this.pcbxGeneral.BackgroundImage = Jump.Iconos_e_Imagenes.Imagenes.Configuraciones_LineasDeBorde;
            }

            // Verifica que sea el radiobutton de texto de armaduras arriba
            else if (sender == this.rbtnTextoArriba)
            {
                this.pcbxGeneral.BackgroundImage = Jump.Iconos_e_Imagenes.Imagenes.Configuraciones_TextoArriba;
            }

            // Verifica que sea el radiobutton de texto de armaduras abajo
            else if (sender == this.rbtnTextoAbajo)
            {
                this.pcbxGeneral.BackgroundImage = Jump.Iconos_e_Imagenes.Imagenes.Configuraciones_TextoAbajo;
            }
        }
        
        /// <summary> Cambia la imagen cuando el mouse queda dentro del groupbox </summary>
        private void gbxCambiarImagenGeneral_MouseHover(object sender, EventArgs e)
        {
            // Verifica que sea el groupbox precisión
            if (sender == this.gbxConfiguraciones)
            {
                // Asigna la imagen
                this.pcbxGeneral.BackgroundImage = Jump.Iconos_e_Imagenes.Imagenes.Configuraciones_Precision;                
            }

            // Verifica que sea el groupbox corte transversal
            if (sender == this.gbxVista)
            {
                // Verifica que sea la vista local 
                if (this.rbtnVistaLocal.Checked == true)
                {
                    // Asigna la imagen
                    this.pcbxGeneral.BackgroundImage = Jump.Iconos_e_Imagenes.Imagenes.Configuraciones_Vista_Local;
                }

                // Verifica que sea la vista global 
                if (this.rbtnVistaGlobal.Checked == true)
                {
                    // Asigna la imagen
                    this.pcbxGeneral.BackgroundImage = Jump.Iconos_e_Imagenes.Imagenes.Configuraciones_Vista_Global;
                }
            }
        }

        /// <summary> Hace que el combobox se despliegue con un solo click </summary>
        private void dgvDesplegarCombobox_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Tools.DesplegarComboboxConUnClick(this.dgvEstiloLinea, e);
        }

        /// <summary> Guarda los estilos de líneas que el usuario seleccionó </summary>
        private void dgvEstiloLinea_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                RebarBarType tipoDiametro = (RebarBarType)this.diametros.Where(x => x.Name == this.dgvEstiloLinea[AboutJump.nombreColumnaDiametros, e.RowIndex].Value);

                if (!this.diametros.Exists(x => x.Id == tipoDiametro.Id))
                {
                    diametros.Add(tipoDiametro);

                    // Ordena la lista alfabéticamente
                    try
                    {
                        diametros = diametros.OrderBy(x => x.BarModelDiameter).ToList();
                    }
                    catch (Exception)
                    {
                        diametros = diametros.OrderBy(x => x.BarNominalDiameter).ToList();
                    }
                }
            }
            catch (Exception) { }
        }

        /// <summary> Cierra el formulario cuando se presiona la tecla Esc </summary>
        private void frmCerrar_KeyDown(object sender, KeyEventArgs e)
        {
            // Verifica que sea la tecla Esc
            if (e.KeyCode == Keys.Escape)
            {
                // Cierra el formulario
                this.Close();
            }
        }

        /// <summary> Cierra el formulario </summary>
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary> Guarda todas las configuraciones echas </summary>
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            bandera = true;

            double precisionX = Convert.ToDouble(this.txtPrecisionOrdenarX.Text);
            double precisionY = Convert.ToDouble(this.txtPrecisionOrdenarY.Text);
            
            // Pestaña General
            Properties.Settings.Default.precisionOrdenarX = UnitUtils.ConvertToInternalUnits(precisionX, this.tipoUnidad);
            Properties.Settings.Default.precisionOrdenarY = UnitUtils.ConvertToInternalUnits(precisionY, this.tipoUnidad);
            Properties.Settings.Default.rbtnGeneralVistaGlobal = this.rbtnVistaGlobal.Checked;
            Properties.Settings.Default.rbtnGeneralVistaLocal = this.rbtnVistaLocal.Checked;

            // Pestaña Armaduras
            Properties.Settings.Default.rbtnArmaduraEnumeracionPorElemento = this.rbtnEnumeracionElemento.Checked;
            Properties.Settings.Default.rbtnArmaduraEnumeracionPorProyecto = this.rbtnEnumeracionProyecto.Checked;
            Properties.Settings.Default.rbtnArmaduraDibujoLineasCentrales = this.rbtnLineasCentrales.Checked;
            Properties.Settings.Default.rbtnArmaduraDibujoLineasBorde = this.rbtnLineasBorde.Checked;
            Properties.Settings.Default.rbtnTextoArmaduraArriba = this.rbtnTextoArriba.Checked;
            Properties.Settings.Default.rbtnTextoArmaduraAbajo = this.rbtnTextoAbajo.Checked;
            Properties.Settings.Default.ArmaduraEtiquetaIndependiente = this.cmbArmaduraEtiqueta.SelectedIndex;
            Properties.Settings.Default.AreaRefuerzoEtiquetaIndependiente = this.cmbAreaRefuerzoEtiqueta.SelectedIndex;
            Properties.Settings.Default.ArmaduraEnSistemaEtiquetaIndependiente = this.cmbArmaduraEnSistemaEtiqueta.SelectedIndex;

            // Pestaña Estilos de líneas
            //Tools.GuardarDataGridViewEnDocumento(this.dgvEstiloLinea, this.doc);
            //CambiarDibujoArmaduras();

            // Pestaña Etiquetas de elementos
            Properties.Settings.Default.ColumnaEtiquetaIndependiente = this.cmbColumnaEtiqueta.SelectedIndex;
            Properties.Settings.Default.LosaEtiquetaIndependiente = this.cmbLosaEtiqueta.SelectedIndex;
            Properties.Settings.Default.MuroEtiquetaIndependiente = this.cmbMuroEtiqueta.SelectedIndex;
            Properties.Settings.Default.PiloteEtiquetaIndependiente = this.cmbPiloteEtiqueta.SelectedIndex;
            Properties.Settings.Default.PlateaEtiquetaIndependiente = this.cmbPlateaEtiqueta.SelectedIndex;
            Properties.Settings.Default.VigaEtiquetaIndependiente = this.cmbVigaEtiqueta.SelectedIndex;
            Properties.Settings.Default.ZapataEtiquetaIndependiente = this.cmbZapataEtiqueta.SelectedIndex;
            Properties.Settings.Default.ZapataCorridaEtiquetaIndependiente = this.cmbZapataCorridaEtiqueta.SelectedIndex;

            // Pestaña cotas lineales
            Properties.Settings.Default.VigaCotaLinealArriba = this.chbVigaCotaArriba.Checked;
            Properties.Settings.Default.VigaCotaLinealAbajo = this.chbVigaCotaAbajo.Checked;
            Properties.Settings.Default.VigaCotaLinealIzquierda = this.chbVigaCotaIzquierda.Checked;
            Properties.Settings.Default.VigaCotaLinealDerecha = this.chbVigaCotaDerecha.Checked;
            Properties.Settings.Default.MuroCotaLinealArriba = this.chbMuroCotaArriba.Checked;
            Properties.Settings.Default.MuroCotaLinealAbajo = this.chbMuroCotaAbajo.Checked;
            Properties.Settings.Default.MuroCotaLinealIzquierda = this.chbMuroCotaIzquierda.Checked;
            Properties.Settings.Default.MuroCotaLinealDerecha = this.chbMuroCotaDerecha.Checked;
            Properties.Settings.Default.ColumnaCotaLinealArriba = this.chbColumnaCotaArriba.Checked;
            Properties.Settings.Default.ColumnaCotaLinealAbajo = this.chbColumnaCotaAbajo.Checked;
            Properties.Settings.Default.ColumnaCotaLinealIzquierda = this.chbColumnaCotaIzquierda.Checked;
            Properties.Settings.Default.ColumnaCotaLinealDerecha = this.chbColumnaCotaDerecha.Checked;
            Properties.Settings.Default.LosaCotaLinealArriba = this.chbLosaCotaArriba.Checked;
            Properties.Settings.Default.LosaCotaLinealAbajo = this.chbLosaCotaAbajo.Checked;
            Properties.Settings.Default.LosaCotaLinealIzquierda = this.chbLosaCotaIzquierda.Checked;
            Properties.Settings.Default.LosaCotaLinealDerecha = this.chbLosaCotaDerecha.Checked;
            Properties.Settings.Default.ZapataCotaLinealArriba = this.chbZapataCotaArriba.Checked;
            Properties.Settings.Default.ZapataCotaLinealAbajo = this.chbZapataCotaAbajo.Checked;
            Properties.Settings.Default.ZapataCotaLinealIzquierda = this.chbZapataCotaIzquierda.Checked;
            Properties.Settings.Default.ZapataCotaLinealDerecha = this.chbZapataCotaDerecha.Checked;
            Properties.Settings.Default.ZapataCorridaCotaLinealArriba = this.chbZapataCorridaCotaArriba.Checked;
            Properties.Settings.Default.ZapataCorridaCotaLinealAbajo = this.chbZapataCorridaCotaAbajo.Checked;
            Properties.Settings.Default.ZapataCorridaCotaLinealIzquierda = this.chbZapataCorridaCotaIzquierda.Checked;
            Properties.Settings.Default.ZapataCorridaCotaLinealDerecha = this.chbZapataCorridaCotaDerecha.Checked;
            Properties.Settings.Default.PlateaCotaLinealArriba = this.chbPlateaCotaArriba.Checked;
            Properties.Settings.Default.PlateaCotaLinealAbajo = this.chbPlateaCotaAbajo.Checked;
            Properties.Settings.Default.PlateaCotaLinealIzquierda = this.chbPlateaCotaIzquierda.Checked;
            Properties.Settings.Default.PlateaCotaLinealDerecha = this.chbPlateaCotaDerecha.Checked;
            Properties.Settings.Default.PiloteCotaLinealArriba = this.chbPiloteCotaArriba.Checked;
            Properties.Settings.Default.PiloteCotaLinealAbajo = this.chbPiloteCotaAbajo.Checked;
            Properties.Settings.Default.PiloteCotaLinealIzquierda = this.chbPiloteCotaIzquierda.Checked;
            Properties.Settings.Default.PiloteCotaLinealDerecha = this.chbPiloteCotaDerecha.Checked;

            // Cotas de profundidad
            Properties.Settings.Default.VigaEtiquetaCotaProfundidad = ObtenerPosicionCotaProfundidad(this.cmbVigaCotaProfundidad);
            Properties.Settings.Default.MuroEtiquetaCotaProfundidad = ObtenerPosicionCotaProfundidad(this.cmbMuroCotaProfundidad);
            Properties.Settings.Default.ColumnaEtiquetaCotaProfundidad = ObtenerPosicionCotaProfundidad(this.cmbColumnaCotaProfundidad);
            Properties.Settings.Default.LosaEtiquetaCotaProfundidad = ObtenerPosicionCotaProfundidad(this.cmbLosaCotaProfundidad);
            Properties.Settings.Default.ZapataEtiquetaCotaProfundidad = ObtenerPosicionCotaProfundidad(this.cmbZapataCotaProfundidad);
            Properties.Settings.Default.ZapataCorridaEtiquetaCotaProfundidad = ObtenerPosicionCotaProfundidad(this.cmbZapataCorridaCotaProfundidad);
            Properties.Settings.Default.PlateaEtiquetaCotaProfundidad = ObtenerPosicionCotaProfundidad(this.cmbPlateaCotaProfundidad);
            Properties.Settings.Default.PiloteEtiquetaCotaProfundidad = ObtenerPosicionCotaProfundidad(this.cmbPiloteCotaProfundidad);
            
            // Guarda las configuraciones
            Properties.Settings.Default.Save();

            this.Close();
        }
    }
}
