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
        UnitType tipoUnidad = UnitType.UT_Length;
        public bool bandera = false;
        List<RebarBarType> diametros = new List<RebarBarType>();

        // Constructor del formulario
        public frmConfiguraciones(Document doc)
        {
            InitializeComponent();

            // Variable necesarias
            this.IdiomaDelPrograma = Tools.ObtenerIdiomaDelPrograma();
            this.doc = doc;
            this.diametros = Tools.ObtenerTodosTiposSegunClase(doc, typeof(RebarBarType)).Cast<RebarBarType>().ToList();

            // Llama a las funciones
            this.dgvEstiloLinea = Tools.ObtenerDataGridViewDeDiametrosYEstilos(this.dgvEstiloLinea, doc, IdiomaDelPrograma);
            AgregarListaPosicionDeEtiquetas();
            CargarImagenesPredeterminadas();
        }

        /// <summary> Agrega las posiciones de las etiquetas independientes </summary>
        private void AgregarListaPosicionDeEtiquetas()
        {
            // Agrega la lista de posiciones a la lista desplegable
            this.cmbEtiquetaArmadura.DataSource = AboutJump.Posiciones(this.IdiomaDelPrograma);
            this.cmbEtiquetaAreaRefuerzo.DataSource = AboutJump.Posiciones(this.IdiomaDelPrograma);
            this.cmbEtiquetaArmaduraEnSistema.DataSource = AboutJump.Posiciones(this.IdiomaDelPrograma);
            this.cmbEtiquetaColumnas.DataSource = AboutJump.Posiciones(this.IdiomaDelPrograma);
            this.cmbEtiquetaLosas.DataSource = AboutJump.Posiciones(this.IdiomaDelPrograma);
            this.cmbEtiquetaMuros.DataSource = AboutJump.Posiciones(this.IdiomaDelPrograma);
            this.cmbEtiquetaPilotes.DataSource = AboutJump.Posiciones(this.IdiomaDelPrograma);
            this.cmbEtiquetaPlatea.DataSource = AboutJump.Posiciones(this.IdiomaDelPrograma);
            this.cmbEtiquetaVigas.DataSource = AboutJump.Posiciones(this.IdiomaDelPrograma);
            this.cmbEtiquetaZapatas.DataSource = AboutJump.Posiciones(this.IdiomaDelPrograma);
            this.cmbEtiquetaZapataCorrida.DataSource = AboutJump.Posiciones(this.IdiomaDelPrograma);
            
            // Asigna el indice de la lista desplegable
            this.cmbEtiquetaArmadura.SelectedIndex = Properties.Settings.Default.EtiquetaIndependienteArmadura;
            this.cmbEtiquetaAreaRefuerzo.SelectedIndex = Properties.Settings.Default.EtiquetaIndependienteAreaRefuerzo;
            this.cmbEtiquetaArmaduraEnSistema.SelectedIndex = Properties.Settings.Default.EtiquetaIndependienteArmaduraEnSistema;
            this.cmbEtiquetaColumnas.SelectedIndex = Properties.Settings.Default.ColumnasEtiquetaIndependiente;
            this.cmbEtiquetaLosas.SelectedIndex = Properties.Settings.Default.LosasEtiquetaIndependiente;
            this.cmbEtiquetaMuros.SelectedIndex = Properties.Settings.Default.MurosEtiquetaIndependiente;
            this.cmbEtiquetaPilotes.SelectedIndex = Properties.Settings.Default.PilotesEtiquetaIndependiente;
            this.cmbEtiquetaPlatea.SelectedIndex = Properties.Settings.Default.PlateaEtiquetaIndependiente;
            this.cmbEtiquetaVigas.SelectedIndex = Properties.Settings.Default.VigasEtiquetaIndependiente;            
            this.cmbEtiquetaZapatas.SelectedIndex = Properties.Settings.Default.ZapatasEtiquetaIndependiente;
            this.cmbEtiquetaZapataCorrida.SelectedIndex = Properties.Settings.Default.ZapatasCorridaEtiquetaIndependiente;
        }

        /// <summary> Asigna las imagenes predeterminadas cuando carga el formulario </summary>
        private void CargarImagenesPredeterminadas()
        {
            // Asigna la imagen
            this.pcbxGeneral.BackgroundImage = Jump.Iconos_e_Imagenes.Imagenes.Configuraciones_Precision;
            //this.pcbxArmaduras.BackgroundImage = Jump.Iconos_e_Imagenes.Imagenes.;
            this.pcbxEtiquetaPosicion.BackgroundImage = Jump.Iconos_e_Imagenes.Imagenes.Configuraciones_Viga;
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
            lblPresicionOrdenarUnidadX.Text = ObtenerSimboloUnidad(tipoUnidad);
            lblPresicionOrdenarUnidadY.Text = ObtenerSimboloUnidad(tipoUnidad);
            gbxVista.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Conf1-3-1");
            rbtnVistaLocal.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Conf1-3-2");
            rbtnVistaGlobal.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Conf1-3-3");
            
            this.pcbxGeneral.BackgroundImage = Iconos_e_Imagenes.Imagenes.Configuraciones_Precision;
            this.txtPrecisionOrdenarX.Text = Properties.Settings.Default.precisionOrdenarX.ToString();
            this.txtPrecisionOrdenarY.Text = Properties.Settings.Default.precisionOrdenarY.ToString();
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
            tbpgEstiloLinea.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Conf3-1");
            lblDiametroEstilo.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Conf3-4");

            // Pestaña Etiquetas de elementos
            tbpgEtiquetas.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Conf4-1");
            gbxEtiquetaIndependiente.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Conf4-1-1");
            lblElementoEtiquetasIdependientes.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Conf4-1-2");
            lblPosicionEtiqueta.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Conf4-1-3");
            lblEtiquetaPilotes.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Conf4-1-1-1");
            lblEtiquetaZapataCorrida.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Conf4-1-1-2");
            lblEtiquetaPlatea.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Conf4-1-1-3");
            lblEtiquetaZapatas.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Conf4-1-1-4");
            lblEtiquetaColumnas.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Conf4-1-1-5");
            lblEtiquetaMuros.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Conf4-1-1-6");
            lblEtiquetaVigas.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Conf4-1-1-7");
            lblEtiquetaLosas.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Conf4-1-1-8");
            gbxEtiquetaArmadura.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Conf4-2-1");
            lblArmaduraEtiquetaIndependiente.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Conf4-2-2");
            lblArmaduraPosicionEtiqueta.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Conf4-2-3");
            lblEtiquetaArmadura.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Conf4-2-1-1");
            lblEtiquetaAreaRefuerzo.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Conf4-2-1-2");
            lblEtiquetaArmaduraEnSistema.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Conf4-2-1-3");

            // Pestaña Cotas
            tbpgCotas.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Conf5-1");
            gbxCotasLineal.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Conf5-1-1");
            gbxCotasProfundidad.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Conf5-2-1");
        }

        /// <summary> Valida que los textos ingresados sean solamente números </summary>
        private string ObtenerSimboloUnidad(UnitType tipo)
        {
            // Obtiene la unidad
            DisplayUnitType DUT = Tools.ObtenerUnidadDelProyecto(doc, tipo);

            // Obtiene el símbolo de la unidad
            string unidad = LabelUtils.GetLabelFor(DUT);

            return unidad;
        }

        /// <summary> Valida que los textos ingresados sean solamente números </summary>
        private void VerificarSoloNumero(object sender, KeyPressEventArgs e)
        {
            // Verifica que solo se ingresen numeros en el TextBox
            Tools.VerificarSoloNumero(e);
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

        /// <summary> Cambia todas las armaduras en función de los nuevos estilos de líneas </summary>
        private void CambiarDibujoArmaduras()
        {
            if (Jump.Properties.Settings.Default.ActualizarBarrasAutomaticamente)
            {
                List<Element> barras = new List<Element>();

                List<Element> elementos = Tools.ObtenerTodosEjemplaresSegunClase(this.doc, typeof(Rebar));

                foreach (RebarBarType tipo in this.diametros)
                {
                    barras.AddRange(elementos.Where(x => x.GetTypeId() == tipo.Id));
                }

                Tools.ActualizarRepresentacionArmadura(this.dgvEstiloLinea, barras);
            }
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
                    diametros = diametros.OrderBy(x => x.BarDiameter).ToList();
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

            // Pestaña General
            Properties.Settings.Default.precisionOrdenarX = Convert.ToInt32(this.txtPrecisionOrdenarX.Text);
            Properties.Settings.Default.precisionOrdenarY = Convert.ToInt32(this.txtPrecisionOrdenarY.Text);
            Properties.Settings.Default.rbtnGeneralVistaGlobal = this.rbtnVistaGlobal.Checked;
            Properties.Settings.Default.rbtnGeneralVistaLocal = this.rbtnVistaLocal.Checked;

            // Pestaña Armaduras
            Properties.Settings.Default.rbtnArmaduraEnumeracionPorElemento = this.rbtnEnumeracionElemento.Checked;
            Properties.Settings.Default.rbtnArmaduraEnumeracionPorProyecto = this.rbtnEnumeracionProyecto.Checked;
            Properties.Settings.Default.rbtnArmaduraDibujoLineasCentrales = this.rbtnLineasCentrales.Checked;
            Properties.Settings.Default.rbtnArmaduraDibujoLineasBorde = this.rbtnLineasBorde.Checked;
            Properties.Settings.Default.rbtnTextoArmaduraArriba = this.rbtnTextoArriba.Checked;
            Properties.Settings.Default.rbtnTextoArmaduraAbajo = this.rbtnTextoAbajo.Checked;
            Properties.Settings.Default.EtiquetaIndependienteArmadura = this.cmbEtiquetaArmadura.SelectedIndex;
            Properties.Settings.Default.EtiquetaIndependienteAreaRefuerzo = this.cmbEtiquetaAreaRefuerzo.SelectedIndex;
            Properties.Settings.Default.EtiquetaIndependienteArmaduraEnSistema = this.cmbEtiquetaArmaduraEnSistema.SelectedIndex;

            // Pestaña Estilos de líneas
            Tools.GuardarDataGridViewEnDocumento(this.dgvEstiloLinea, this.doc);
            CambiarDibujoArmaduras();

            // Pestaña Etiquetas de elementos
            Properties.Settings.Default.ColumnasEtiquetaIndependiente = this.cmbEtiquetaColumnas.SelectedIndex;
            Properties.Settings.Default.LosasEtiquetaIndependiente = this.cmbEtiquetaLosas.SelectedIndex;
            Properties.Settings.Default.MurosEtiquetaIndependiente = this.cmbEtiquetaMuros.SelectedIndex;
            Properties.Settings.Default.PilotesEtiquetaIndependiente = this.cmbEtiquetaPilotes.SelectedIndex;
            Properties.Settings.Default.PlateaEtiquetaIndependiente = this.cmbEtiquetaPlatea.SelectedIndex;
            Properties.Settings.Default.VigasEtiquetaIndependiente = this.cmbEtiquetaVigas.SelectedIndex;
            Properties.Settings.Default.ZapatasEtiquetaIndependiente = this.cmbEtiquetaZapatas.SelectedIndex;
            Properties.Settings.Default.ZapatasCorridaEtiquetaIndependiente = this.cmbEtiquetaZapataCorrida.SelectedIndex;

            // Guarda las configuraciones
            Properties.Settings.Default.Save();

            this.Close();
        }
    }
}
