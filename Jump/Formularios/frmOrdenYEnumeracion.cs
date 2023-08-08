using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.Structure;

namespace Jump
{
    public partial class frmOrdenYEnumeracion : System.Windows.Forms.Form
    {
        // Variable necesarias
        string IdiomaDelPrograma;
        Document doc;
        UIDocument uiDoc;

        // Parámetros generales
        List<Element> listaElementosEnumerar = new List<Element>();
        List<Element> elementos = new List<Element>();
        List<Parameter> parametrosEjemplar = new List<Parameter>();
        List<Category> listaCategorias = new List<Category>();
        BuiltInCategory categoria;

        // Constructor del formulario
        public frmOrdenYEnumeracion(Document doc, UIDocument uiDoc)
        {
            InitializeComponent();

            Tools.AddinManager();

            // Variable necesarias
            this.IdiomaDelPrograma = Tools.ObtenerIdiomaDelPrograma();
            this.doc = doc;
            this.uiDoc = uiDoc;

            // Llama a las funciones
            CargarCategoriasEstructurales();
            AgregarElementos();
            AgregarParametros();
        }

        /// <summary> Carga las categorias estructurales a la lista </summary>
        private void CargarCategoriasEstructurales()
        {
            // Obtiene todas las categorías del documento
            Categories categorias = this.doc.Settings.Categories;

            // Agrega las categorías
            foreach (Category cat in categorias)
            {
                this.listaCategorias.Add(cat);
            }

            // Ordena la lista alfabéticamente
            listaCategorias = listaCategorias.OrderBy(x => x.Name).ToList();

            // Limpia y rellena el combobox
            Tools.RellenarCombobox(this.cmbCategorias, listaCategorias);

            // Asigna la categoría
            this.categoria = (BuiltInCategory)listaCategorias.FirstOrDefault<Category>().Id.IntegerValue;
        }

        /// <summary> Agrega los elementos a enumerar a la lista </summary>
        private void AgregarElementos()
        {
            // Limpia la lista
            this.lstElementos.Items.Clear();

            // Asigna todos los elementos a enumerar del proyecto a la lista
            this.elementos = Tools.ObtenerTodosEjemplaresSegunCategoria(doc, categoria);

            // Agrega los elementos a la listbox
            Tools.RellenarListBoxDeElementos(this.lstElementos, doc, this.elementos);
        }
        
        /// <summary> Agrega los parámetros de ejemplar a la lista </summary>
        private void AgregarParametros()
        {
            // Limpia la lista
            lstParametros.Items.Clear();

            // Obtiene una lista con los parámetros de ejemplar
            List<Parameter> listaParametros = Tools.ObtenerParametrosEjemplar(doc, categoria);

            // Obtiene una lista con parámetros de ejemplar modificables
            this.parametrosEjemplar = Tools.ObtenerParametrosEjemplarModificables(doc, listaParametros);

            // Agrega los parámetros a la listbox
            Tools.RellenarListBoxDeParametros(this.lstParametros, this.parametrosEjemplar);
        }

        /// <summary> Carga la lista de los elementos según las opciones marcadas "Selección" </summary>
        private void ObtenerElementosSegunOpciones()
        {
            // Agrega todas los elementos a enumerar del proyecto a la lista
            if (this.rbtnTodos.Checked)
            {
                // Obtiene todos los elementos a enumerar
                this.elementos = Tools.ObtenerTodosEjemplaresSegunCategoria(doc, categoria);
                
                // Asigna todos los elementos a enumerar
                this.listaElementosEnumerar = this.elementos;
            }

            // Agrega los elementos a enumerar seleccionadas en el proyecto a la lista
            if (this.rbtnElementosSeleccionados.Checked)
            {
                // Obtiene los elementos seleccionados en el proyecto
                List<Element> listaSeleccionados = Tools.ObtenerElementosSeleccionadosEnProyecto(this.uiDoc, this.doc, this.elementos);

                // Obtiene los elementos seleccionados que coinciden con la lista de elementos a enumerar
                this.listaElementosEnumerar = Tools.ObtenerElementosCoincidentesConLista(this.elementos, listaSeleccionados);
            }

            // Agrega los elementos a enumerar seleccionadas de la listabox
            if (this.rbtnConjuntoDeLaLista.Checked)
            {
                // Obtiene los elementos seleccionados de la listbox y agrega a la lista
                this.listaElementosEnumerar = Tools.ObtenerElementosDeUnListbox(this.lstElementos, this.doc, this.elementos);
            }
        }

        /// <summary> Cambia los elementos al cambiar la categoría seleccionada </summary>
        private void cmbCategorias_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Obtiene la categoría seleccionada
            Category categoriaSeleccionada = (Category)this.cmbCategorias.SelectedItem;

            // Asigna 
            this.categoria = (BuiltInCategory)categoriaSeleccionada.Id.IntegerValue;

            // Agrega los nuevos elementos
            AgregarElementos();

            // Agrega los parámetros
            AgregarParametros();
        }

        /// <summary> Carga el formulario </summary>
        private void frmOrdenYEnumeracion_Load(object sender, EventArgs e)
        {
            // Asignación de textos según el idioma
            this.Text = Language.ObtenerTexto(IdiomaDelPrograma, "OrdYEnu4");
            btnAceptar.Text = Language.ObtenerTexto(IdiomaDelPrograma, "OrdYEnu5");
            btnCancelar.Text = Language.ObtenerTexto(IdiomaDelPrograma, "OrdYEnu6");
            gbxSeleccion.Text = Language.ObtenerTexto(IdiomaDelPrograma, "OrdYEnu1-1");
            rbtnTodos.Text = Language.ObtenerTexto(IdiomaDelPrograma, "OrdYEnu1-2");
            rbtnElementosSeleccionados.Text = Language.ObtenerTexto(IdiomaDelPrograma, "OrdYEnu1-3");
            rbtnConjuntoDeLaLista.Text = Language.ObtenerTexto(IdiomaDelPrograma, "OrdYEnu1-4");
            gbxOrden.Text = Language.ObtenerTexto(IdiomaDelPrograma, "OrdYEnu2-1");
            gbxEnumeracion.Text = Language.ObtenerTexto(IdiomaDelPrograma, "OrdYEnu3-1");
            lblPrefijo.Text = Language.ObtenerTexto(IdiomaDelPrograma, "OrdYEnu3-2");
            lblNumeroInicial.Text = Language.ObtenerTexto(IdiomaDelPrograma, "OrdYEnu3-3");
            lblIncremento.Text = Language.ObtenerTexto(IdiomaDelPrograma, "OrdYEnu3-4");
            lblSufijo.Text = Language.ObtenerTexto(IdiomaDelPrograma, "OrdYEnu3-5");
            gbxPreview.Text = Language.ObtenerTexto(IdiomaDelPrograma, "OrdYEnu3-6");
            lblParametroElegido.Text = Language.ObtenerTexto(IdiomaDelPrograma, "OrdYEnu3-7");
            lblVistaPrevia.Text = Language.ObtenerTexto(IdiomaDelPrograma, "OrdYEnu3-8");
        }
        
        /// <summary> Cambia el texto del label según los datos ingresados </summary>
        private void txtPrefijo_TextChanged(object sender, EventArgs e)
        {
            // Limpia y asigna el texto
            lblVistaPrevia.Text = null;
            lblVistaPrevia.Text = txtPrefijo.Text + txtNumeroInicial.Text + txtSufijo.Text;
        }

        /// <summary> Valida que los textos ingresados sean solamente números </summary>
        private void verificarSoloNumero(object sender, KeyPressEventArgs e)
        {
            // Verifica que solo se ingresen numeros en el TextBox
            Tools.VerificarSoloNumero(e);
        }

        /// <summary> Cambia el texto del label según el parámetro elegido </summary>
        private void lstParametros_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Asigna el texto
            lblParametroElegido.Text = lstParametros.Text;
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

        /// <summary> Ejecuta todas las acciones </summary>
        private void btnEjecutar_Click(object sender, EventArgs e)
        {
            // Abre una transacción
            using (Transaction t = new Transaction(this.doc, Language.ObtenerTexto(IdiomaDelPrograma, "OrdYEnu4-1")))
            {
                t.Start();

                // Limpia la lista de elementos a enumerar
                this.listaElementosEnumerar.Clear();

                // Obtiene los elementos a enumerar
                ObtenerElementosSegunOpciones();
                
                // Verifica que la lista de elementos a enumerar contenga elementos para poder continuar
                if (this.listaElementosEnumerar.Count > 0)
                {
                    // Llama al formulario barra de progreso
                    frmBarraProgreso barraProgreso = new frmBarraProgreso(this.listaElementosEnumerar.Count);

                    // Muestra el formulario
                    barraProgreso.Show();

                    // Crea la variable para enumerar
                    int valorActual = 0;

                    // Verifica que inicial tenga valor
                    if (this.txtNumeroInicial.Text != null && this.txtNumeroInicial.Text != "")
                    {
                        valorActual = Int32.Parse(this.txtNumeroInicial.Text);
                    }
                    
                    // Ordena la lista
                    try
                    {
                        this.listaElementosEnumerar = Tools.OrdenarListaSegunCheckboxEnGroupBox(this.gbxOrden, this.listaElementosEnumerar);
                    }
                    catch (Exception) { }

                    // Recorre todos los elementos de la lista
                    foreach (Element elem in this.listaElementosEnumerar)
                    {
                        // Verifica que el usuario cargó un número inicial
                        if (this.txtNumeroInicial.Text != null && this.txtNumeroInicial.Text != "")
                        {
                            // Asigna la enumeración definida según los parámetros ingresados
                            Tools.EnumeracionParametrosEjemplarListaElementos(this.lstParametros, elem,
                                                                              this.txtPrefijo.Text, valorActual,
                                                                              this.txtSufijo.Text);
                        }
                        
                        try
                        {
                            // Incrementa la enumeración
                            valorActual += Convert.ToInt32(this.txtIncremento.Text);
                        }
                        catch (Exception) { }
                    }

                    // Cierra el formulario barra de progreso
                    barraProgreso.Close();
                }

                t.Commit();
            }

            // Cierra el formulario
            //this.Close();
        }
    }
}
