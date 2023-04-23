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
            this.cmbEtiquetaArmadura.DataSource = new List<int>(Tools.listaPosicionEtiquetas);
            this.cmbEtiquetaAreaRefuerzo.DataSource = new List<int>(Tools.listaPosicionEtiquetas);
            this.cmbEtiquetaArmaduraEnSistema.DataSource = new List<int>(Tools.listaPosicionEtiquetas);
            this.cmbEtiquetaColumnas.DataSource = new List<int>(Tools.listaPosicionEtiquetas);
            this.cmbEtiquetaLosas.DataSource = new List<int>(Tools.listaPosicionEtiquetas);
            this.cmbEtiquetaMuros.DataSource = new List<int>(Tools.listaPosicionEtiquetas);
            this.cmbEtiquetaPilotes.DataSource = new List<int>(Tools.listaPosicionEtiquetas);
            this.cmbEtiquetaPlatea.DataSource = new List<int>(Tools.listaPosicionEtiquetas);
            this.cmbEtiquetaVigas.DataSource = new List<int>(Tools.listaPosicionEtiquetas);            
            this.cmbEtiquetaZapatas.DataSource = new List<int>(Tools.listaPosicionEtiquetas);
            this.cmbEtiquetaZapataCorrida.DataSource = new List<int>(Tools.listaPosicionEtiquetas);

            // Asigna el indice de la lista desplegable
            this.cmbEtiquetaArmadura.SelectedItem = Properties.Settings.Default.EtiquetaIndependienteArmadura;
            this.cmbEtiquetaAreaRefuerzo.SelectedItem = Properties.Settings.Default.EtiquetaIndependienteAreaRefuerzo;
            this.cmbEtiquetaArmaduraEnSistema.SelectedItem = Properties.Settings.Default.EtiquetaIndependienteArmaduraEnSistema;
            this.cmbEtiquetaColumnas.SelectedItem = Properties.Settings.Default.EtiquetaIndependienteColumnas;
            this.cmbEtiquetaLosas.SelectedItem = Properties.Settings.Default.EtiquetaIndependienteLosas;
            this.cmbEtiquetaMuros.SelectedItem = Properties.Settings.Default.EtiquetaIndependienteMuros;
            this.cmbEtiquetaPilotes.SelectedItem = Properties.Settings.Default.EtiquetaIndependientePilotes;
            this.cmbEtiquetaPlatea.SelectedItem = Properties.Settings.Default.EtiquetaIndependientePlatea;
            this.cmbEtiquetaVigas.SelectedItem = Properties.Settings.Default.EtiquetaIndependienteVigas;            
            this.cmbEtiquetaZapatas.SelectedItem = Properties.Settings.Default.EtiquetaIndependienteZapatas;
            this.cmbEtiquetaZapataCorrida.SelectedItem = Properties.Settings.Default.EtiquetaIndependienteZapatasCorrida;
        }

        /// <summary> Asigna las imagenes predeterminadas cuando carga el formulario </summary>
        private void CargarImagenesPredeterminadas()
        {
            // Asigna la imagen
            this.pcbxGeneral.BackgroundImage = Jump.Iconos_e_Imagenes.Imagenes.Configuraciones_Precision;
            //this.pcbxArmaduras.BackgroundImage = Jump.Iconos_e_Imagenes.Imagenes.;
            //this.pcbxEtiquetaPosicion.BackgroundImage = Jump.Iconos_e_Imagenes.Imagenes.;
        }

        /// <summary> Carga el formulario </summary>
        private void frmConfiguraciones_Load(object sender, EventArgs e)
        {
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

            // Pestaña Armaduras
            tbpgArmadura.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Conf2-1");
            gbxArmaduraEnumeracion.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Conf2-2-1");
            rbtnEnumeracionElemento.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Conf2-2-2");
            rbtnEnumeracionProyecto.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Conf2-2-3");
            gbxArmaduraDibujo.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Conf2-3-1");
            rbtnLineasCentrales.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Conf2-3-2");
            rbtnLineasBorde.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Conf2-3-3");
            gbxPosicionTexto.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Conf2-4-1");
            rbtnTextoArriba.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Conf2-4-2");
            rbtnTextoAbajo.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Conf2-4-3");
            gbxEtiquetaArmadura.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Conf2-5-1");
            lblArmaduraEtiquetaIndependiente.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Conf2-5-2");
            lblArmaduraPosicionEtiqueta.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Conf2-5-3");
            lblEtiquetaArmadura.Text = Language.ObtenerTexto(IdiomaDelPrograma, "Conf2-5-1-1");

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

        /// <summary> Cambia la imagen cuando el mouse pasa por arriba del radiobutton Local </summary>
        private void CambiarImagenVistaLocal_MouseMove(object sender, MouseEventArgs e)
        {
            // Asigna la imagen
            this.pcbxGeneral.BackgroundImage = Jump.Iconos_e_Imagenes.Imagenes.Zapata_Vista_Local;
        }

        /// <summary> Cambia la imagen cuando el mouse pasa por arriba del radiobutton Global </summary>
        private void CambiarImagenVistaGlobal_MouseMove(object sender, MouseEventArgs e)
        {
            // Asigna la imagen
            this.pcbxGeneral.BackgroundImage = Jump.Iconos_e_Imagenes.Imagenes.Zapata_Vista_Global;
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
                    this.pcbxGeneral.BackgroundImage = Jump.Iconos_e_Imagenes.Imagenes.Zapata_Vista_Local;
                }

                // Verifica que sea la vista global 
                if (this.rbtnVistaGlobal.Checked == true)
                {
                    // Asigna la imagen
                    this.pcbxGeneral.BackgroundImage = Jump.Iconos_e_Imagenes.Imagenes.Zapata_Vista_Global;
                }
            }
        }

        /// <summary> Cambia la imagen cuando el mouse pasa por arriba del radiobutton </summary>
        private void CambiarImagenArmadura_MouseMove(object sender, MouseEventArgs e)
        {
            // Verifica que sea el radiobutton lineas centrales
            if (sender == this.rbtnLineasCentrales)
            {
                // Asigna la imagen
                this.pcbxArmaduras.BackgroundImage = Jump.Iconos_e_Imagenes.Imagenes.Configuraciones_LineasCentrales;
            }

            // Verifica que sea el radiobutton lineas de borde
            if (sender == this.rbtnLineasBorde)
            {
                // Asigna la imagen
                this.pcbxArmaduras.BackgroundImage = Jump.Iconos_e_Imagenes.Imagenes.Configuraciones_LineasDeBorde;
            }
        }
        
        /// <summary> Cambia la imagen cuando el mouse queda dentro del groupbox </summary>
        private void gbxCambiarImagenArmadura_MouseHover(object sender, EventArgs e)
        {
            // Verifica que sea el groupbox precisión
            if (sender == this.gbxArmaduraEnumeracion)
            {
                // Asigna la imagen
                //this.pcbxArmaduras.BackgroundImage = Jump.Iconos_e_Imagenes.Imagenes.;
            }

            // Verifica que sea el groupbox corte transversal
            if (sender == this.gbxArmaduraDibujo)
            {
                // Verifica que sea la vista local 
                if (this.rbtnLineasCentrales.Checked == true)
                {
                    // Asigna la imagen
                    this.pcbxArmaduras.BackgroundImage = Jump.Iconos_e_Imagenes.Imagenes.Configuraciones_LineasCentrales;
                }

                // Verifica que sea la vista global 
                if (this.rbtnLineasBorde.Checked == true)
                {
                    // Asigna la imagen
                    this.pcbxArmaduras.BackgroundImage = Jump.Iconos_e_Imagenes.Imagenes.Configuraciones_LineasDeBorde;
                }
            }
        }

        /// <summary> Cambia la imagen de la posición de las etiquetas de los elementos estructurales </summary>
        private void cmbCambiarImagenPreview_DropDown(object sender, EventArgs e)
        {
            // Verifica que sea etiqueta armadura
            if (sender == this.cmbEtiquetaArmadura)
            {
                //this.pcbxArmaduras.BackgroundImage = Jump.Iconos_e_Imagenes.Imagenes.;
            }

            // Verifica que sea etiqueta área de refuerzo
            if (sender == this.cmbEtiquetaAreaRefuerzo)
            {
                //this.pcbxArmaduras.BackgroundImage = Jump.Iconos_e_Imagenes.Imagenes.;
            }

            // Verifica que sea etiqueta armadura en sistema
            if (sender == this.cmbEtiquetaArmaduraEnSistema)
            {
                //this.pcbxArmaduras.BackgroundImage = Jump.Iconos_e_Imagenes.Imagenes.;
            }

            // Verifica que sea columnas
            if (sender == this.cmbEtiquetaColumnas)
            {
                //this.pcbxEtiquetaPosicion.BackgroundImage = Jump.Iconos_e_Imagenes.Imagenes.;
            }

            // Verifica que sea losas
            if (sender == this.cmbEtiquetaLosas)
            {
                //this.pcbxEtiquetaPosicion.BackgroundImage = Jump.Iconos_e_Imagenes.Imagenes.;
            }

            // Verifica que sea muros
            if (sender == this.cmbEtiquetaMuros)
            {
                //this.pcbxEtiquetaPosicion.BackgroundImage = Jump.Iconos_e_Imagenes.Imagenes.;
            }

            // Verifica que sea pilotes
            if (sender == this.cmbEtiquetaPilotes)
            {
                //this.pcbxEtiquetaPosicion.BackgroundImage = Jump.Iconos_e_Imagenes.Imagenes.;
            }

            // Verifica que sea plateas
            if (sender == this.cmbEtiquetaPlatea)
            {
                //this.pcbxEtiquetaPosicion.BackgroundImage = Jump.Iconos_e_Imagenes.Imagenes.;
            }

            // Verifica que sea vigas
            if (sender == this.cmbEtiquetaVigas)
            {
                //this.pcbxEtiquetaPosicion.BackgroundImage = Jump.Iconos_e_Imagenes.Imagenes.;
            }

            // Verifica que sea zapatas
            if (sender == this.cmbEtiquetaZapatas)
            {
                //this.pcbxEtiquetaPosicion.BackgroundImage = Jump.Iconos_e_Imagenes.Imagenes.;
            }

            // Verifica que sea zapata corrida
            if (sender == this.cmbEtiquetaZapataCorrida)
            {
                //this.pcbxEtiquetaPosicion.BackgroundImage = Jump.Iconos_e_Imagenes.Imagenes.;
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

            // Pestaña 1
            Properties.Settings.Default.precisionOrdenarX = Convert.ToInt32(this.txtPrecisionOrdenarX.Text);
            Properties.Settings.Default.precisionOrdenarY = Convert.ToInt32(this.txtPrecisionOrdenarY.Text);
            Properties.Settings.Default.rbtnGeneralVistaGlobal = this.rbtnVistaGlobal.Checked;
            Properties.Settings.Default.rbtnGeneralVistaLocal = this.rbtnVistaLocal.Checked;

            // Pestaña 2
            Properties.Settings.Default.rbtnArmaduraEnumeracionPorElemento = this.rbtnEnumeracionElemento.Checked;
            Properties.Settings.Default.rbtnArmaduraEnumeracionPorProyecto = this.rbtnEnumeracionProyecto.Checked;
            Properties.Settings.Default.rbtnArmaduraDibujoLineasCentrales = this.rbtnLineasCentrales.Checked;
            Properties.Settings.Default.rbtnArmaduraDibujoLineasBorde = this.rbtnLineasBorde.Checked;
            Properties.Settings.Default.rbtnTextoArmaduraArriba = this.rbtnTextoArriba.Checked;
            Properties.Settings.Default.rbtnTextoArmaduraAbajo = this.rbtnTextoAbajo.Checked;
            Properties.Settings.Default.EtiquetaIndependienteArmadura = Convert.ToInt32(this.cmbEtiquetaArmadura.SelectedItem);
            Properties.Settings.Default.EtiquetaIndependienteAreaRefuerzo = Convert.ToInt32(this.cmbEtiquetaAreaRefuerzo.SelectedItem);
            Properties.Settings.Default.EtiquetaIndependienteArmaduraEnSistema = Convert.ToInt32(this.cmbEtiquetaArmaduraEnSistema.SelectedItem);

            // Pestaña 3
            Tools.GuardarDataGridViewEnDocumento(this.dgvEstiloLinea, this.doc);
            CambiarDibujoArmaduras();

            // Pestaña 4
            Properties.Settings.Default.EtiquetaIndependienteColumnas = Convert.ToInt32(this.cmbEtiquetaColumnas.SelectedItem);
            Properties.Settings.Default.EtiquetaIndependienteLosas = Convert.ToInt32(this.cmbEtiquetaLosas.SelectedItem);
            Properties.Settings.Default.EtiquetaIndependienteMuros = Convert.ToInt32(this.cmbEtiquetaMuros.SelectedItem);
            Properties.Settings.Default.EtiquetaIndependientePilotes = Convert.ToInt32(this.cmbEtiquetaPilotes.SelectedItem);
            Properties.Settings.Default.EtiquetaIndependientePlatea = Convert.ToInt32(this.cmbEtiquetaPlatea.SelectedItem);
            Properties.Settings.Default.EtiquetaIndependienteVigas = Convert.ToInt32(this.cmbEtiquetaVigas.SelectedItem);
            Properties.Settings.Default.EtiquetaIndependienteZapatas = Convert.ToInt32(this.cmbEtiquetaZapatas.SelectedItem);
            Properties.Settings.Default.EtiquetaIndependienteZapatasCorrida = Convert.ToInt32(this.cmbEtiquetaZapataCorrida.SelectedItem);

            // Guarda las configuraciones
            Properties.Settings.Default.Save();

            this.Close();
        }
    }
}
