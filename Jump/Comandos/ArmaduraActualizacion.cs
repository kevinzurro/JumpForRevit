using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.ApplicationServices;

namespace Jump
{
    class ArmaduraActualizacion : IUpdater
    {
        // Variable necesarias
        string IdiomaDelPrograma = Tools.ObtenerIdiomaDelPrograma();
        AddInId addinID = null;
        UpdaterId updaterID = null;
        List<TextNoteType> etiquetasLongitud = new List<TextNoteType>();
        System.Windows.Forms.ComboBox cmbEtiquetaLongitud = new System.Windows.Forms.ComboBox();
        System.Windows.Forms.DataGridView dgvEstiloLinea = new System.Windows.Forms.DataGridView();

        // Guid para actualizar barras
        string GuidActualizarBarra = "7907bd48-73fd-457d-acf9-5311d6b2c0f8";
        
        // Contructor de la clase
        public ArmaduraActualizacion(AddInId AddID)
        {
            // Asigna el ID del complemento
            this.addinID = AddID;

            // Crea un nuevo UpdaterId
            this.updaterID = new UpdaterId(addinID, new Guid(GuidActualizarBarra));

            // Configura el DataGridView
            ConfigurarDataGridView();
        }

        /// <summary> Configura el DataGridView </summary>
        public void ConfigurarDataGridView()
        {
            // Crea el DataGridView de los diámetros y estilos
            this.dgvEstiloLinea = Tools.CrearDataGridViewDeDiametrosYEstilos(IdiomaDelPrograma);
        }

        /// <summary> Carga los combobox de las etiquetas </summary>
        private void CargarComboboxEtiquetas(Document doc)
        {
            // Completa la lista           
            this.etiquetasLongitud.AddRange(Tools.ObtenerEstilosTexto(doc));

            // Rellena el combobox
            Tools.RellenarComboboxEtiquetaTexto(this.cmbEtiquetaLongitud, etiquetasLongitud);

            // Asigna el texto seleccionado
            this.cmbEtiquetaLongitud.SelectedIndex = Jump.Properties.Settings.Default.zapataIndiceComboboxTextoBarra;
        }

        /// <summary> Método cada vez que existe algún cambio en Revit </summary>
        public void Execute(UpdaterData data)
        {
            // Documento del proyecto
            Document doc;

            // Verifica que esté activo la actualización automatica de barras
            if (Jump.Properties.Settings.Default.ActualizarBarrasAutomaticamente)
            {
                // Obtiene el documento
                doc = data.GetDocument();
                
                // Agregas las filas al DataGridView
                this.dgvEstiloLinea.Rows.Add(Tools.CrearDataGridViewDeDiametrosYEstilos(IdiomaDelPrograma).Rows);

                // Obtiene el ComboboxColumn
                System.Windows.Forms.DataGridViewComboBoxColumn EstiloLinea = this.dgvEstiloLinea.Columns[Tools.nombreColumnaEstilosLineas] as System.Windows.Forms.DataGridViewComboBoxColumn;

                // Obtiene el DataGridView con los diámetros y estilos de líneas
                Tools.AgregarDiametrosYEstilos(this.dgvEstiloLinea, EstiloLinea, doc);

                // Carga el combobox de los estilos de textos
                CargarComboboxEtiquetas(doc);

                // Obtiene todos los ID de los elementos modificados
                List<ElementId> elementosId = data.GetModifiedElementIds().ToList();

                // Obtiene los elementos modificados
                List<Element> elementos = Tools.ObtenerElementoSegunID(doc, elementosId);

                // Obtiene todas las barras del proyecto
                List<Element> colectorBarras = new FilteredElementCollector(doc).
                                                    OfCategory(BuiltInCategory.OST_Rebar).
                                                    OfClass(typeof(Rebar)).ToList();

                // Obtiene todas las barras modificadas
                List<Element> barrasModificadas = Tools.ObtenerElementosCoincidentesConLista(colectorBarras, elementos);

                // Recorre todas los despieces creados
                foreach (ArmaduraRepresentacion bar in Inicio.listaArmaduraRepresentacion)
                {
                    // Recorre todas las barras modificadas
                    foreach (Rebar barra in barrasModificadas)
                    {
                        try
                        {
                            // Verifica que la barra modificada sea igual al del despiece
                            if (bar.Barra.Id == barra.Id)
                            {
                                // Verifica que las líneas no sean nulas
                                if (bar.CurvasDeArmadura != null && bar.TextosDeLongitudesParciales != null)
                                {
                                    // Elimina el despiece
                                    bar.Eliminar();

                                    // Dibuja las líneas de la nueva geometría y asigna al objeto
                                    bar.CurvasDeArmadura = Tools.DibujarArmaduraSegunDatagridview(this.dgvEstiloLinea, doc, bar.Vista, bar.Barra);

                                    // Crea el texto de la longitud parcial y asigna al objeto
                                    bar.TextosDeLongitudesParciales = Tools.CrearTextNoteDeArmadura(doc, bar.Vista, bar.Barra, bar.TipoDeTexto);

                                    // Mueve la Representación de la Armadura
                                    bar.MoverArmaduraRepresentacion(bar.Posicion);
                                }
                            }
                        }
                        catch (Exception)
                        {
                            // Elimina las curvas
                            bar.Eliminar();
                        }
                    }
                }
            }
        }

        /// <summary> Tipo del cambio que realizará el actualizador </summary>
        public ChangePriority GetChangePriority()
        {
            // Tipo de prioridad
            return ChangePriority.Rebar;
        }

        /// <summary> Devuelve el UpdaterId </summary>
        public UpdaterId GetUpdaterId()
        {
            return updaterID;
        }

        /// <summary> Obtiene la información adicional del actualizador </summary>
        public string GetAdditionalInformation()
        {
            // Información del actualizador
            string info = Language.ObtenerTexto(IdiomaDelPrograma, "ActBar1");

            return info;
        }

        /// <summary> Obtiene el nombre del actualizador </summary>
        public string GetUpdaterName()
        {
            // Nombre del actualizador
            string nombre = Language.ObtenerTexto(IdiomaDelPrograma, "ActBar2");

            return nombre;
        }
    }
}
