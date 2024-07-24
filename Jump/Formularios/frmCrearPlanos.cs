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

        List<Autodesk.Revit.DB.View> todosVistas = new List<Autodesk.Revit.DB.View>();
        List<FamilySymbol> todosTiposDePlanos = new List<FamilySymbol>();

        public frmCrearPlanos(Document doc)
        {
            InitializeComponent();

            Tools.AddinManager();

            // Variable necesarias
            this.IdiomaDelPrograma = Tools.ObtenerIdiomaDelPrograma();
            this.doc = doc;

            CargarVistaYTipoPlanos();
            CargarTreeView();
        }

        /// <summary> Carga las vistas del proyecto y los tipos de planos </summary>
        private void CargarVistaYTipoPlanos()
        {
            this.todosVistas = Tools.ObtenerTodosEjemplaresSegunClase(this.doc, claseVista).Cast<Autodesk.Revit.DB.View>().Where(x => !x.IsTemplate).ToList();
            this.todosTiposDePlanos = new FilteredElementCollector(this.doc).OfCategory(categoriaTipoPlano).WhereElementIsElementType().Cast<FamilySymbol>().ToList();

            this.lstTipoPlano.DataSource = this.todosTiposDePlanos;
            this.lstTipoPlano.DisplayMember = AboutJump.parametroMostrarUsuarioPlanos;
            this.lstTipoPlano.ValueMember = AboutJump.parametroId;
        }

        /// <summary> Carga el TreeViewlas con las vistas </summary>
        private void CargarTreeView()
        {
            List<ViewFamilyType> tipoVistas = new FilteredElementCollector(this.doc).OfClass(typeof(ViewFamilyType)).WhereElementIsElementType().Cast<ViewFamilyType>().ToList();

            tipoVistas = tipoVistas.OrderBy(x => x.FamilyName).ThenBy(x => x.Name).ToList();

            foreach (ViewFamilyType tipo in tipoVistas)
            {
                TreeNode nodoPrin = new TreeNode(tipo.FamilyName + " (" + tipo.Name + ")");

                List<Autodesk.Revit.DB.View> vistas = this.todosVistas;

                vistas = vistas.Where(x => x.GetTypeId() == tipo.Id).ToList();

                foreach (Autodesk.Revit.DB.View vista in vistas)
                {
                    TreeNode nodoSecu = new TreeNode();

                    nodoSecu.Name = vista.Id.Value.ToString();
                    nodoSecu.Text = vista.Name;
                    
                    nodoPrin.Nodes.Add(nodoSecu);
                }
                
                if (nodoPrin.Nodes.Count > 0)
                {
                    this.trvVistas.Nodes.Add(nodoPrin);

                    this.trvVistas.ExpandAll();
                }
            }
        }

        /// <summary> Expande todo el TreeView </summary>
        private void btnExpandir_Click(object sender, EventArgs e)
        {
            this.trvVistas.ExpandAll();
        }

        /// <summary> Contrae todo el TreeView </summary>
        private void btnContraer_Click(object sender, EventArgs e)
        {
            this.trvVistas.CollapseAll();
        }

        /// <summary> Evento cuando se hace click en un nodo </summary>
        private void trvVistas_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Level == 0)
            {
                bool estado = e.Node.Checked;
                
                foreach (TreeNode nodoSecu in e.Node.Nodes)
                {
                    nodoSecu.Checked = estado;
                }
            }
        }

        /// <summary> Selecciona todos los nodos del TreeView </summary>
        private void btnSeleccionarTodos_Click(object sender, EventArgs e)
        {
            CambiarEstadoDeNodo(this.trvVistas.Nodes, true);
        }

        /// <summary> No selecciona ninguno de los nodos del TreeView </summary>
        private void btnSeleccionarNinguno_Click(object sender, EventArgs e)
        {
            CambiarEstadoDeNodo(this.trvVistas.Nodes, false);
        }

        // <summary> Cambia el checked del nodo y sus hijos </summary>
        private void CambiarEstadoDeNodo(TreeNodeCollection nodos, bool estado)
        {
            foreach (TreeNode nodo in nodos)
            {
                nodo.Checked = estado;

                if (nodo.Nodes.Count > 0)
                {
                    CambiarEstadoDeNodo(nodo.Nodes, estado);
                }
            }
        }

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
            btnSeleccionarTodos.Text = Language.ObtenerTexto(IdiomaDelPrograma, "CreaPlano3-2");
            btnSeleccionarNinguno.Text = Language.ObtenerTexto(IdiomaDelPrograma, "CreaPlano3-3");
            btnExpandir.Text = Language.ObtenerTexto(IdiomaDelPrograma, "CreaPlano3-4");
            btnContraer.Text = Language.ObtenerTexto(IdiomaDelPrograma, "CreaPlano3-5");
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

        /// <summary> Obtiene una lista con todos los TreeNodes </summary>
        private List<TreeNode> ObtenerTodosLosNodos(TreeNodeCollection nodos)
        {
            List<TreeNode> todosNodos = new List<TreeNode>();

            foreach (TreeNode nodo in nodos)
            {
                todosNodos.Add(nodo);

                todosNodos.AddRange(ObtenerTodosLosNodos(nodo.Nodes));
            }

            return todosNodos;
        }

        /// <summary> Ejecuta todas las acciones </summary>
        private void btnAceptar_Click(object sender, EventArgs e)
        {
            List<Autodesk.Revit.DB.View> vistasSeleccionadas = new List<Autodesk.Revit.DB.View>();

            List<TreeNode> todosNodos = ObtenerTodosLosNodos(this.trvVistas.Nodes);

            foreach (TreeNode nodo in todosNodos)
            {
                if (nodo.Level != 0 && nodo.Checked)
                {
                    Autodesk.Revit.DB.View vista = this.todosVistas.Where(x => x.Id.Value.ToString() == nodo.Name).FirstOrDefault();

                    if (vista != null)
                    {
                        vistasSeleccionadas.Add(vista);

                    }
                }
            }

            if (vistasSeleccionadas.Count > 0 && this.lstTipoPlano.SelectedItems.Count > 0)
            {
                frmBarraProgreso barraProgreso = new frmBarraProgreso(vistasSeleccionadas.Count);

                barraProgreso.Show();

                ElementId tipoDePlano = (this.lstTipoPlano.SelectedItem as FamilySymbol).Id;

                int contador = 0;

                using (Transaction tra = new Transaction(this.doc, Language.ObtenerTexto(IdiomaDelPrograma, "CreaPlano4-1")))
                {
                    tra.Start();

                    if (this.chbPlanoIndividual.Checked)
                    {
                        foreach (Autodesk.Revit.DB.View vista in vistasSeleccionadas)
                        {
                            if (barraProgreso.Cancelado())
                            {
                                break;
                            }

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

                            barraProgreso.Incrementar();
                        }
                    }
                    else
                    {
                        ViewSheet plano = ViewSheet.Create(this.doc, tipoDePlano);

                        UV planoMin = plano.Outline.Min;
                        UV planoMax = plano.Outline.Max;

                        XYZ planoCentro = new XYZ(planoMax.U + planoMin.U, planoMax.V + planoMin.V, 0) / 2;

                        double anchoPlano = (planoMax.U - planoMin.U);

                        XYZ moverViewport = new XYZ();

                        this.doc.Regenerate();

                        int n = 0;

                        foreach (Autodesk.Revit.DB.View vista in vistasSeleccionadas)
                        {
                            if (barraProgreso.Cancelado())
                            {
                                break;
                            }

                            try
                            {
                                Viewport viewport = Viewport.Create(this.doc, plano.Id, vista.Id, XYZ.Zero);

                                this.doc.Regenerate();

                                XYZ dimensionesViewport = viewport.GetBoxOutline().MaximumPoint - viewport.GetBoxOutline().MinimumPoint;

                                XYZ viewportCentro = (viewport.GetBoxOutline().MaximumPoint + viewport.GetBoxOutline().MinimumPoint) / 2;

                                XYZ distanciaCentro = planoCentro - viewportCentro;

                                ElementTransformUtils.MoveElement(this.doc, viewport.Id, distanciaCentro + moverViewport);

                                if ((moverViewport + dimensionesViewport).X >= anchoPlano)
                                {
                                    n++;

                                    moverViewport = new XYZ(0, dimensionesViewport.Negate().Y, 0).Multiply(n);
                                }
                                else
                                {
                                    moverViewport = moverViewport + Tools.ProyectarVectorSobreDireccion(dimensionesViewport, plano.RightDirection);
                                }
                            }
                            catch (Exception) { }

                            barraProgreso.Incrementar();
                        }

                        contador++;
                    }

                    if (contador > 0 && !barraProgreso.Cancelado())
                    {
                        tra.Commit();

                        barraProgreso.Close();

                        TaskDialog.Show(Language.ObtenerTexto(IdiomaDelPrograma, "CreaPlano4-2"),
                                        contador.ToString() + Language.ObtenerTexto(IdiomaDelPrograma, "CreaPlano4-3"));
                    }
                    else
                    {
                        tra.RollBack();

                        barraProgreso.Close();
                    }
                }

                try{ CambiarEstadoDeNodo(this.trvVistas.Nodes, false); }catch (Exception) { }
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
