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
using System.Windows.Controls;

namespace Jump
{
    public partial class frmCrearPlanos : System.Windows.Forms.Form
    {
        // Variable necesarias
        string IdiomaDelPrograma;
        Document doc;
        UIDocument uiDoc;
        Type claseVista = typeof(Autodesk.Revit.DB.View);
        Type claseTipoPlano = typeof(Autodesk.Revit.DB.FamilyInstance);
        BuiltInCategory categoriaTipoPlano = BuiltInCategory.OST_TitleBlocks;

        List<Element> todosVistas = new List<Element>();
        List<FamilySymbol> todosTiposDePlanos = new List<FamilySymbol>();

        public frmCrearPlanos(Document doc)
        {
            InitializeComponent();

            Tools.AddinManager();

            // Variable necesarias
            this.IdiomaDelPrograma = Tools.ObtenerIdiomaDelPrograma();
            this.doc = doc;

            CargarVistaYTipoPlanos();
        }

        // <summary> Carga las vistas del proyecto y los tipos de planos </summary>
        private void CargarVistaYTipoPlanos()
        {
            this.todosVistas = Tools.ObtenerTodosEjemplaresSegunClase(this.doc, claseVista);
            this.todosTiposDePlanos = new FilteredElementCollector(this.doc).OfCategory(categoriaTipoPlano).WhereElementIsElementType().Cast<FamilySymbol>().ToList();

            // Agrega los elementos a la listbox
            this.lstVistas.DataSource = this.todosVistas;
            this.lstVistas.DisplayMember = AboutJump.parametroMostrarUsuario;
            this.lstVistas.ValueMember = AboutJump.parametroId;

            this.lstTipoPlano.DataSource = this.todosTiposDePlanos;
            this.lstTipoPlano.DisplayMember = AboutJump.parametroMostrarUsuarioPlanos;
            this.lstTipoPlano.ValueMember = AboutJump.parametroId;
        }

        //private void CargarTreeView()
        //{
        //    List<ViewFamilyType> tipoVistas = new FilteredElementCollector(this.doc).OfClass(typeof(ViewFamilyType)).WhereElementIsElementType().Cast<ViewFamilyType>().ToList();

        //    tipoVistas = tipoVistas.OrderBy(x => x.Name).ToList();

        //    foreach (ViewFamilyType tipo in tipoVistas)
        //    {
        //        TreeNode nodoPrin = new TreeNode(tipo.FamilyName);

        //        List<Element> vistas = this.todosVistas;

        //        vistas = vistas.Where(x => x.GetTypeId() == tipo.Id).ToList();

        //        foreach (Autodesk.Revit.DB.View vista in vistas)
        //        {
        //            TreeNode nodoSecu = new TreeNode();

        //            nodoSecu.Name = vista.Id.Value.ToString();
        //            nodoSecu.Text = vista.Name;

        //            nodoPrin.Nodes.Add(nodoSecu);
        //        }

        //        if (nodoPrin.Nodes.Count > 0)
        //        {
        //            this.trvVistas.Nodes.Add(nodoPrin);

        //            this.trvVistas.ExpandAll();
        //        }
        //    }
        //}

        /// <summary> Carga el formulario </summary>
        private void frmCrearPlanos_Load(object sender, EventArgs e)
        {
            // Asignación de textos según el idioma
            this.Text = Language.ObtenerTexto(IdiomaDelPrograma, "CreaPlano4");
            btnAceptar.Text = Language.ObtenerTexto(IdiomaDelPrograma, "CreaPlano5");
            btnCancelar.Text = Language.ObtenerTexto(IdiomaDelPrograma, "CreaPlano6");
            gbxVistas.Text = Language.ObtenerTexto(IdiomaDelPrograma, "CreaPlano1-1");
            gbxTipoPlano.Text = Language.ObtenerTexto(IdiomaDelPrograma, "CreaPlano2-1");
            chbPlanoIndividual.Text = Language.ObtenerTexto(IdiomaDelPrograma, "CreaPlano3-1");
        }

        /// <summary> Cierra el formulario cuando se presiona la tecla Esc </summary>
        private void frmCrearPlanos_KeyDown(object sender, KeyEventArgs e)
        {
            // Verifica que sea la tecla Esc
            if (e.KeyCode == Keys.Escape)
            {
                // Cierra el formulario
                this.Close();
            }
        }

        /// <summary> Ejecuta todas las acciones </summary>
        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (this.lstVistas.SelectedItems.Count > 0 && this.lstTipoPlano.SelectedItems.Count > 0)
            {
                frmBarraProgreso barra = new frmBarraProgreso(this.lstVistas.SelectedItems.Count);

                barra.Show();

                List<Element> vistasSeleccionadas = lstVistas.SelectedItems.Cast<Element>().ToList();

                ElementId tipoDePlano = (this.lstTipoPlano.SelectedItem as FamilySymbol).Id;

                int contador = 0;

                using (Transaction tra = new Transaction(this.doc, Language.ObtenerTexto(IdiomaDelPrograma, "CreaPlano4-1")))
                {
                    tra.Start();

                    if (this.chbPlanoIndividual.Checked)
                    {
                        foreach (Autodesk.Revit.DB.View vista in vistasSeleccionadas)
                        {
                            try
                            {
                                ViewSheet plano = ViewSheet.Create(this.doc, tipoDePlano);

                                Viewport viewport = Viewport.Create(this.doc, plano.Id, vista.Id, XYZ.Zero);

                                this.doc.Regenerate();

                                UV planoMin = plano.Outline.Min;
                                UV planoMax = plano.Outline.Max;

                                XYZ planoCentro = new XYZ(planoMax.U + planoMin.U, planoMax.V + planoMin.V, 0) / 2;

                                XYZ viewportCentro = (viewport.GetBoxOutline().MaximumPoint + viewport.GetBoxOutline().MinimumPoint) / 2;

                                XYZ distancia = planoCentro - viewportCentro;

                                ElementTransformUtils.MoveElement(this.doc, viewport.Id, distancia);

                                contador++;
                            }
                            catch (Exception) { }

                            barra.Incrementar();
                        }
                    }
                    else
                    {
                        ViewSheet plano = ViewSheet.Create(this.doc, tipoDePlano);

                        this.doc.Regenerate();

                        foreach (Autodesk.Revit.DB.View vista in vistasSeleccionadas)
                        {
                            try
                            {
                                Viewport viewport = Viewport.Create(this.doc, plano.Id, vista.Id, XYZ.Zero);

                                this.doc.Regenerate();

                                UV planoMin = plano.Outline.Min;
                                UV planoMax = plano.Outline.Max;

                                XYZ planoCentro = new XYZ(planoMax.U + planoMin.U, planoMax.V + planoMin.V, 0) / 2;
                                XYZ viewportCentro = (viewport.GetBoxOutline().MaximumPoint + viewport.GetBoxOutline().MinimumPoint) / 2;
                                XYZ distancia = planoCentro - viewportCentro;

                                ElementTransformUtils.MoveElement(this.doc, viewport.Id, distancia);
                            }
                            catch (Exception) { }

                            barra.Incrementar();
                        }

                        contador++;
                    }

                    if (contador > 0)
                    {
                        tra.Commit();
                    }
                    else
                    {
                        tra.RollBack();
                    }
                }

                barra.Close();

                TaskDialog.Show(Language.ObtenerTexto(IdiomaDelPrograma, "CreaPlano4-2"),
                                contador.ToString() + Language.ObtenerTexto(IdiomaDelPrograma, "CreaPlano4-3"));
            }
        }

        /// <summary> Botón cancelar del formulario </summary>
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            // Cierra el formulario
            this.Close();
        }
    }
}
