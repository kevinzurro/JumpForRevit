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
using System.Windows.Forms.Integration;
using System.Windows.Controls;

namespace Jump
{
    public partial class frmNodoAnaliticoVerificacion : System.Windows.Forms.Form
    {
        // Variable necesarias
        string IdiomaDelPrograma;
        Document doc;
        Type claseNodos = typeof(ReferencePoint);
        BuiltInCategory categoria = BuiltInCategory.OST_AnalyticalNodes;
        double error = Properties.Settings.Default.ConfiguracionPrecisionNodoAnalitico;
        TransactionGroup tg;

        List<ReferencePoint> nodosTodos = new List<ReferencePoint>();
        List<ReferencePoint> nodos = new List<ReferencePoint>();
        List<Nodo> nodosCercanos = new List<Nodo>();

        // Constructor del formulario
        public frmNodoAnaliticoVerificacion(Document doc)
        {
            InitializeComponent();

            // Variable necesarias
            this.IdiomaDelPrograma = Tools.ObtenerIdiomaDelPrograma();
            this.doc = doc;

            this.tg = new TransactionGroup(this.doc, Language.ObtenerTexto(IdiomaDelPrograma, "NodAnaVer2-1"));
            tg.Start();

            this.nodosTodos = Tools.ObtenerTodosEjemplaresSegunClaseYCategoria(this.doc, this.claseNodos, this.categoria).Cast<ReferencePoint>().ToList();

            VerificarProximidad();
        }

        private void frmNodoAnaliticoVerificacion_Load(object sender, EventArgs e)
        {
            this.Text = Language.ObtenerTexto(IdiomaDelPrograma, "NodAnaVer4");
            btnAceptar.Text = Language.ObtenerTexto(IdiomaDelPrograma, "NodAnaVer5");
            btnCancelar.Text = Language.ObtenerTexto(IdiomaDelPrograma, "NodAnaVer6");
            lblNodos.Text = Language.ObtenerTexto(IdiomaDelPrograma, "NodAnaVer1-1");
            lblProximidad.Text = Language.ObtenerTexto(IdiomaDelPrograma, "NodAnaVer1-2");
        }

        /// <summary> Verifica la proximidad de los nodos analíticos </summary>
        private void VerificarProximidad()
        {
            for (int i = 0; i < nodosTodos.Count; i++)
            {
                Nodo nodo = new Nodo();

                nodo.Punto = nodosTodos[i];

                for (int j = i + 1; j < nodosTodos.Count; j++)
                {
                    double distancia = nodosTodos[i].Position.DistanceTo(nodosTodos[j].Position);

                    if (distancia <= error)
                    {
                        nodo.Puntos.Add(nodosTodos[j]);

                        if (!nodosCercanos.Contains(nodo))
                        {
                            nodosCercanos.Add(nodo);
                        }
                    }
                }
            }

            this.nodos = this.nodosTodos.Where(x => nodosCercanos.Any(y => y.Punto.Id == x.Id)).ToList();

            try
            {
                foreach (ReferencePoint punto in nodos)
                {
                    this.lstNodos.Items.Add(punto);
                }

                this.lstNodos.DisplayMember = AboutJump.parametroId;

                this.lstNodos.ValueMember = AboutJump.parametroId;
            }
            catch (Exception) { }
        }

        /// <summary> Rellena el listbox con los nodos cercanos </summary>
        private void lstNodos_Click(object sender, EventArgs e)
        {
            this.lstProximidad.Items.Clear();

            try
            {
                List<ReferencePoint> puntos = nodosCercanos[lstNodos.SelectedIndex].Puntos;

                foreach (ReferencePoint punto in puntos)
                {
                    this.lstProximidad.Items.Add(punto);
                }

                this.lstProximidad.DisplayMember = AboutJump.parametroId;

                this.lstProximidad.ValueMember = AboutJump.parametroId;
            }
            catch (Exception) { }
        }

        /// <summary> Aisla los elementos en la vista </summary>
        private void lstProximidad_SelectedIndexChanged(object sender, EventArgs e)
        {
            Autodesk.Revit.DB.View vista = this.doc.ActiveView;

            using (Transaction tra = new Transaction(this.doc, "Aislar"))
            {
                tra.Start();

                try
                {
                    if (vista.IsTemporaryHideIsolateActive())
                    {
                        vista.DisableTemporaryViewMode(TemporaryViewMode.TemporaryHideIsolate);
                    }

                    List<ReferencePoint> ocultar = nodosTodos;

                    ReferencePoint punto1 = lstNodos.SelectedItem as ReferencePoint;
                    ReferencePoint punto2 = lstProximidad.SelectedItem as ReferencePoint;

                    ocultar.Remove(punto1);
                    ocultar.Remove(punto2);

                    vista.HideElementsTemporary(Tools.ObtenerIdElemento(ocultar.Cast<Element>().ToList()));
                }
                catch (Exception) { }

                tra.Commit();
            }
        }

        /// <summary> Cancela la transacción </summary>
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.tg.RollBack();

            this.Close();
        }

        /// <summary> Acepta la transacción </summary>
        private void btnAceptar_Click(object sender, EventArgs e)
        {
            this.tg.Assimilate();

            this.Close();
        }

        /// <summary> Cierra el formulario </summary>
        private void frmNodoAnaliticoVerificacion_KeyDown(object sender, KeyEventArgs e)
        {
            // Verifica que sea la tecla Esc
            if (e.KeyCode == Keys.Escape)
            {
                // Cierra el formulario
                this.Close();
            }
        }
    }

    class Nodo
    {
        ReferencePoint punto;
        List<ReferencePoint> puntos = new List<ReferencePoint>();

        public ReferencePoint Punto
        {
            get { return this.punto; }
            set { this.punto = value; }
        }

        public List<ReferencePoint> Puntos
        {
            get { return this.puntos; }
            set { this.puntos = value; }
        }
    }
}
