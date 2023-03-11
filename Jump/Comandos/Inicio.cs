using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Collections;
using System.IO;
using Jump.Properties;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.DB.Events;

namespace Jump
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    public class Inicio : IExternalApplication
    {
        // Obtiene el idioma del programa
        public static string IdiomaDelPrograma = Tools.ObtenerIdiomaDelPrograma();
        
        // Ruta del ensamblado o de la addin que se está ejecutando
        public static string RutaDelEnsamblado = System.Reflection.Assembly.GetExecutingAssembly().Location;

        // Lista de representaciones de armaduras
        public static List<ArmaduraRepresentacion> listaArmaduraRepresentacion = new List<ArmaduraRepresentacion>();
        
        /// <summary> Inicio de la aplicación </summary>
        public Result OnStartup(UIControlledApplication application)
        {
            // Crear la pestaña con el nombre de la addin
            application.CreateRibbonTab(AboutJump.NombreAddin);

            #region Idioma

            // Carga los textos para cada uno de los idiomas de los vectores secundarios
            Language.CargarIdiomasDisponibles();

            // Verifica el idioma de la interfaz
            IdiomaDelPrograma = Tools.VerificarIdioma(IdiomaDelPrograma);

            // Carga los textos para cada uno de los vectores secundarios
            Language.CargaTextosDeCadaIdioma();

            #endregion

            #region Paneles

            //Crear los paneles
            RibbonPanel panelDetalleArmado = application.CreateRibbonPanel(AboutJump.NombreAddin, Language.ObtenerTexto(IdiomaDelPrograma, "Tit1"));
            RibbonPanel panelVisibilidad = application.CreateRibbonPanel(AboutJump.NombreAddin, Language.ObtenerTexto(IdiomaDelPrograma, "Tit2"));
            RibbonPanel panelHerramienta = application.CreateRibbonPanel(AboutJump.NombreAddin, Language.ObtenerTexto(IdiomaDelPrograma, "Tit3"));

            #endregion

            #region Botones

            // Crea los botones
            PushButton botonDespieceArmadura = panelDetalleArmado.AddItem(new PushButtonData("botonDespieceArmadura", Language.ObtenerTexto(IdiomaDelPrograma, "DetArm1"), RutaDelEnsamblado, "Jump.cmdArmaduraRepresentacion")) as PushButton;
            PushButton botonPilote = panelDetalleArmado.AddItem(new PushButtonData("botonPilote", Language.ObtenerTexto(IdiomaDelPrograma, "Pil1"), RutaDelEnsamblado, "Jump.cmdPilotes")) as PushButton;
            PushButton botonZapataCorrida = panelDetalleArmado.AddItem(new PushButtonData("botonZapataCorrida", Language.ObtenerTexto(IdiomaDelPrograma, "ZapCor1"), RutaDelEnsamblado, "Jump.cmdZapataCorrida")) as PushButton;
            PushButton botonPlatea = panelDetalleArmado.AddItem(new PushButtonData("botonPlatea", Language.ObtenerTexto(IdiomaDelPrograma, "Pla1"), RutaDelEnsamblado, "Jump.cmdPlatea")) as PushButton;
            PushButton botonZapata = panelDetalleArmado.AddItem(new PushButtonData("botonZapata", Language.ObtenerTexto(IdiomaDelPrograma, "Zap1"), RutaDelEnsamblado, "Jump.cmdZapatas")) as PushButton;
            PushButton botonColumna = panelDetalleArmado.AddItem(new PushButtonData("botonColumna", Language.ObtenerTexto(IdiomaDelPrograma, "Col1"), RutaDelEnsamblado, "Jump.cmdColumnas")) as PushButton;
            PushButton botonMuro = panelDetalleArmado.AddItem(new PushButtonData("botonMuro", Language.ObtenerTexto(IdiomaDelPrograma, "Mur1"), RutaDelEnsamblado, "Jump.cmdMuros")) as PushButton;
            PushButton botonViga = panelDetalleArmado.AddItem(new PushButtonData("botonViga", Language.ObtenerTexto(IdiomaDelPrograma, "Vig1"), RutaDelEnsamblado, "Jump.cmdVigas")) as PushButton;
            PushButton botonLosa = panelDetalleArmado.AddItem(new PushButtonData("botonLosa", Language.ObtenerTexto(IdiomaDelPrograma, "Los1"), RutaDelEnsamblado, "Jump.cmdLosas")) as PushButton;
            PushButton botonElemenEstructural = panelVisibilidad.AddItem(new PushButtonData("botonElemenEstructural", Language.ObtenerTexto(IdiomaDelPrograma, "EleEst1"), RutaDelEnsamblado, "Jump.cmdVisibilidadEstructural")) as PushButton;
            PushButton botonElemenAnalitico = panelVisibilidad.AddItem(new PushButtonData("botonElemenAnalitico", Language.ObtenerTexto(IdiomaDelPrograma, "EleAna1"), RutaDelEnsamblado, "Jump.cmdVisibilidadAnalitica")) as PushButton;
            PushButton botonOrdenEnumeracion = panelHerramienta.AddItem(new PushButtonData("botonOrdenEnumeracion", Language.ObtenerTexto(IdiomaDelPrograma, "OrdYEnu1"), RutaDelEnsamblado, "Jump.cmdOrdenYEnumeracion")) as PushButton;
            PushButton botonIdioma = panelHerramienta.AddItem(new PushButtonData("botonIdioma", Language.ObtenerTexto(IdiomaDelPrograma, "Idi1"), RutaDelEnsamblado, "Jump.cmdIdioma")) as PushButton;
            PushButton botonConfiguracion = panelHerramienta.AddItem(new PushButtonData("botonConfiguracion", Language.ObtenerTexto(IdiomaDelPrograma, "Conf1"), RutaDelEnsamblado, "Jump.cmdConfiguraciones")) as PushButton;

            // Crea los botones para la visibilidad de la armadura que van en el botón desplegable
            PushButtonData botonArmaSolido = new PushButtonData("botonArmaSolido", Language.ObtenerTexto(IdiomaDelPrograma, "VisArmSol"), RutaDelEnsamblado, "Jump.cmdArmaduraSolido");
            PushButtonData botonArmaFilamento = new PushButtonData("botonArmaFilamento", Language.ObtenerTexto(IdiomaDelPrograma, "VisArmFil"), RutaDelEnsamblado, "Jump.cmdArmaduraFilamento");
            PushButtonData botonArmaSinTapa = new PushButtonData("botonArmaSinTapa", Language.ObtenerTexto(IdiomaDelPrograma, "VisArmSinTap"), RutaDelEnsamblado, "Jump.cmdArmaduraSinTapar");
            PushButtonData botonArmaTapada = new PushButtonData("botonArmaTapada", Language.ObtenerTexto(IdiomaDelPrograma, "VisArmTap"), RutaDelEnsamblado, "Jump.cmdArmaduraTapada");

            // Crear el boton desplegable para las armaduras
            PulldownButton botonArmaVisibilidad = panelVisibilidad.AddItem(new PulldownButtonData("botonArmaVisibilidad", Language.ObtenerTexto(IdiomaDelPrograma, "VisArm1"))) as PulldownButton;

            #endregion

            #region Imagenes de botones

            // Agregar la imagen al botón
            botonDespieceArmadura.LargeImage = new BitmapImage(new Uri("pack://application:,,,/Jump;component/Resources/Boton_Pilote.png"));
            botonPilote.LargeImage = new BitmapImage(new Uri("pack://application:,,,/Jump;component/Resources/Boton_Pilote.png"));
            botonZapataCorrida.LargeImage = new BitmapImage(new Uri("pack://application:,,,/Jump;component/Resources/Boton_Zapata_Corrida.png"));
            botonPlatea.LargeImage = new BitmapImage(new Uri("pack://application:,,,/Jump;component/Resources/Boton_Platea.png"));
            botonZapata.LargeImage = new BitmapImage(new Uri("pack://application:,,,/Jump;component/Resources/Boton_Zapata.png"));
            botonColumna.LargeImage = new BitmapImage(new Uri("pack://application:,,,/Jump;component/Resources/Boton_Columna.png"));
            botonMuro.LargeImage = new BitmapImage(new Uri("pack://application:,,,/Jump;component/Resources/Boton_Muro.png"));
            botonViga.LargeImage = new BitmapImage(new Uri("pack://application:,,,/Jump;component/Resources/Boton_Viga.png"));
            botonLosa.LargeImage = new BitmapImage(new Uri("pack://application:,,,/Jump;component/Resources/Boton_Losa.png"));
            botonArmaSolido.LargeImage = new BitmapImage(new Uri("pack://application:,,,/Jump;component/Resources/Armadura_solida.png"));
            botonArmaFilamento.LargeImage = new BitmapImage(new Uri("pack://application:,,,/Jump;component/Resources/Armadura_filamento.png"));
            botonArmaSinTapa.LargeImage = new BitmapImage(new Uri("pack://application:,,,/Jump;component/Resources/Armadura_sin_tapar.png"));
            botonArmaTapada.LargeImage = new BitmapImage(new Uri("pack://application:,,,/Jump;component/Resources/Armadura_tapada.png"));
            botonArmaVisibilidad.LargeImage = new BitmapImage(new Uri("pack://application:,,,/Jump;component/Resources/Armadura_visibilidad.png"));
            botonElemenEstructural.LargeImage = new BitmapImage(new Uri("pack://application:,,,/Jump;component/Resources/Boton_Elem_Estructural.png"));
            botonElemenAnalitico.LargeImage = new BitmapImage(new Uri("pack://application:,,,/Jump;component/Resources/Boton_Elem_Analitico.png"));
            botonOrdenEnumeracion.LargeImage = new BitmapImage(new Uri("pack://application:,,,/Jump;component/Resources/Boton_Orden.png"));
            botonIdioma.LargeImage = new BitmapImage(new Uri("pack://application:,,,/Jump;component/Resources/Boton_Idioma.png"));
            botonConfiguracion.LargeImage = new BitmapImage(new Uri("pack://application:,,,/Jump;component/Resources/Boton_Configuracion.png"));

            #endregion

            #region Botones desplegables de visibilidad de armadura

            // Agregar los botones de visibilidad al boton desplegable
            botonArmaVisibilidad.AddPushButton(botonArmaSolido);
            botonArmaVisibilidad.AddPushButton(botonArmaFilamento);
            botonArmaVisibilidad.AddPushButton(botonArmaSinTapa);
            botonArmaVisibilidad.AddPushButton(botonArmaTapada);

            #endregion

            #region Descripción corta de los botones

            // Crear la descripción corta de los botones
            botonDespieceArmadura.ToolTip = Language.ObtenerTexto(IdiomaDelPrograma, "DetArm2");
            botonPilote.ToolTip = Language.ObtenerTexto(IdiomaDelPrograma, "Pil2");
            botonZapataCorrida.ToolTip = Language.ObtenerTexto(IdiomaDelPrograma, "ZapCor2");
            botonPlatea.ToolTip = Language.ObtenerTexto(IdiomaDelPrograma, "Pla2");
            botonZapata.ToolTip = Language.ObtenerTexto(IdiomaDelPrograma, "Zap2");
            botonColumna.ToolTip = Language.ObtenerTexto(IdiomaDelPrograma, "Col2");
            botonMuro.ToolTip = Language.ObtenerTexto(IdiomaDelPrograma, "Mur2");
            botonViga.ToolTip = Language.ObtenerTexto(IdiomaDelPrograma, "Vig2");
            botonLosa.ToolTip = Language.ObtenerTexto(IdiomaDelPrograma, "Los2");
            botonElemenEstructural.ToolTip = Language.ObtenerTexto(IdiomaDelPrograma, "EleEst2");
            botonElemenAnalitico.ToolTip = Language.ObtenerTexto(IdiomaDelPrograma, "EleAna2");
            botonArmaVisibilidad.ToolTip = Language.ObtenerTexto(IdiomaDelPrograma, "VisArm2");
            botonOrdenEnumeracion.ToolTip = Language.ObtenerTexto(IdiomaDelPrograma, "OrdYEnu2");
            botonIdioma.ToolTip = Language.ObtenerTexto(IdiomaDelPrograma, "Idi2");
            botonConfiguracion.ToolTip = Language.ObtenerTexto(IdiomaDelPrograma, "Conf2");

            #endregion

            #region Descripción larga de los botones

            // Crear la descripción larga de los botones
            botonDespieceArmadura.LongDescription = Language.ObtenerTexto(IdiomaDelPrograma, "DetArm3");
            botonPilote.LongDescription = Language.ObtenerTexto(IdiomaDelPrograma, "Pil3");
            botonZapataCorrida.LongDescription = Language.ObtenerTexto(IdiomaDelPrograma, "ZapCor3");
            botonPlatea.LongDescription = Language.ObtenerTexto(IdiomaDelPrograma, "Pla3");
            botonZapata.LongDescription = Language.ObtenerTexto(IdiomaDelPrograma, "Zap3");
            botonColumna.LongDescription = Language.ObtenerTexto(IdiomaDelPrograma, "Col3");
            botonMuro.LongDescription = Language.ObtenerTexto(IdiomaDelPrograma, "Mur3");
            botonViga.LongDescription = Language.ObtenerTexto(IdiomaDelPrograma, "Vig3");
            botonLosa.LongDescription = Language.ObtenerTexto(IdiomaDelPrograma, "Los3");
            botonElemenEstructural.LongDescription = Language.ObtenerTexto(IdiomaDelPrograma, "EleEst3");
            botonElemenAnalitico.LongDescription = Language.ObtenerTexto(IdiomaDelPrograma, "EleAna3");
            botonArmaVisibilidad.LongDescription = Language.ObtenerTexto(IdiomaDelPrograma, "VisArm3");
            botonOrdenEnumeracion.LongDescription = Language.ObtenerTexto(IdiomaDelPrograma, "OrdYEnu3");
            botonIdioma.LongDescription = Language.ObtenerTexto(IdiomaDelPrograma, "Idi3");
            botonConfiguracion.LongDescription = Language.ObtenerTexto(IdiomaDelPrograma, "Conf3");

            #endregion

            #region Actualizador de barras

            // Crea el actualizador de armaduras
            Tools.CrearRegistroActualizadorArmaduras(application.ActiveAddInId);

            #endregion

            #region Eventos de Revit

            try
            {
                // Registra el evento cuando se abre un documento
                application.ControlledApplication.DocumentOpened += new EventHandler
                           <Autodesk.Revit.DB.Events.DocumentOpenedEventArgs>(application_DocumentOpened);
            }
            catch (Exception) { }

            try
            {
                // Registra el evento cuando se guardó un documento
                application.ControlledApplication.DocumentSavedAs += new EventHandler
                           <Autodesk.Revit.DB.Events.DocumentSavedAsEventArgs>(application_DocumentSavedAs);
            }
            catch (Exception) { }

            try
            {
                // Registra el evento cuando se guardó un documento
                application.ControlledApplication.DocumentSaved += new EventHandler
                           <Autodesk.Revit.DB.Events.DocumentSavedEventArgs>(application_DocumentSaved);
            }
            catch (Exception) { }

            #endregion

            return Result.Succeeded;
        }
        
        /// <summary> Finalizar la aplicación </summary>
        public Result OnShutdown(UIControlledApplication application)
        {
            // Elimina el actualizador de armaduras
            Tools.EliminarRegistroActualizadorArmaduras(application.ActiveAddInId);

            return Result.Succeeded;
        }

        #region Eventos de Revit

        /// <summary> Evento cuando se terminó de abrir un documento </summary>
        public void application_DocumentOpened(object sender, DocumentOpenedEventArgs args)
        {
            // Obtiene el documento
            Document doc = args.Document;

            // Obtiene la ruta del documento
            string rutaDocumento = doc.PathName;

            // Obtiene la ruta del archivo
            string rutaArchivo = Tools.ObtenerRutaArchivoDiametroYEstilo(doc);

            try
            {
                // Crea el DataGridView
                System.Windows.Forms.DataGridView dgv = Tools.CrearDataGridViewDeDiametrosYEstilos(IdiomaDelPrograma);

                // Agregas las filas al DataGridView
                dgv.Rows.Add(Tools.CrearDataGridViewDeDiametrosYEstilos(IdiomaDelPrograma).Rows);

                // Verifica que exista ruta
                if (rutaDocumento != "" && !doc.IsDetached)
                {
                    // Carga el DataGridView
                    Tools.AgregarDiametrosYEstilos(dgv, dgv.Columns[Tools.nombreColumnaEstilosLineas] as System.Windows.Forms.DataGridViewComboBoxColumn, doc);
                }

                // No existe el nombre
                else
                {
                    // Crea un archivo temporal
                    string rutaTemporal = Tools.CrearRutaTemporalArchivoDiametroYEstilo(doc);

                    // Completa el DataGridView
                    Tools.AgregarDiametrosYEstilos(dgv, dgv.Columns[Tools.nombreColumnaEstilosLineas] as System.Windows.Forms.DataGridViewComboBoxColumn, doc);

                    // Guarda el archivo
                    Tools.GuardarDiametrosYEstilos(dgv, rutaTemporal);
                }
            }
            catch (Exception ) { }

            try
            {
                List<ArmaduraRepresentacion> armaduras = Tools.ObtenerRepresentacionArmaduraDeJson(doc);

                if (armaduras.Count > 0)
                {
                    // Carga las representaciones de las armaduras a la lista
                    Inicio.listaArmaduraRepresentacion.AddRange(armaduras);
                }
            }
            catch (Exception) { }

        }

        /// <summary> Evento cuando se guardó un documento con nombre distinto o por primera vez </summary>
        public void application_DocumentSavedAs(object sender, DocumentSavedAsEventArgs args)
        {
            // Obtiene el documento
            Document doc = args.Document;

            // Verifica que se guardó correctamente el documento
            if (args.Status == RevitAPIEventStatus.Succeeded)
            {
                // Obtiene la ruta del archivo
                string rutaArchivo = Tools.ObtenerRutaArchivoDiametroYEstilo(doc);

                try
                {
                    // Crea el DataGridView
                    System.Windows.Forms.DataGridView dgv = Tools.CrearDataGridViewDeDiametrosYEstilos(IdiomaDelPrograma);

                    // Agregas las filas al DataGridView
                    dgv.Rows.Add(Tools.CrearDataGridViewDeDiametrosYEstilos(IdiomaDelPrograma).Rows);

                    // Carga el DataGridView
                    Tools.AgregarDiametrosYEstilos(dgv, dgv.Columns[Tools.nombreColumnaEstilosLineas] as System.Windows.Forms.DataGridViewComboBoxColumn, doc);

                    // Guarda el DataGridView
                    Tools.GuardarDiametrosYEstilos(dgv, rutaArchivo);
                }
                catch (Exception) { }

                try
                {
                    List<ArmaduraRepresentacion> armaduras = listaArmaduraRepresentacion.Where(x => x.Documento == doc).ToList();

                    Tools.GuardarRepresentacionArmaduraEnJson(doc, armaduras);
                }
                catch (Exception) { }
            }
        }

        /// <summary> Evento cuando se guardó un documento con nombre distinto o por primera vez </summary>
        public void application_DocumentSaved(object sender, DocumentSavedEventArgs args)
        {
            Document doc = args.Document;

            try
            {
                List<ArmaduraRepresentacion> armaduras = listaArmaduraRepresentacion.Where(x => x.Documento == doc).ToList();

                Tools.GuardarRepresentacionArmaduraEnJson(doc, armaduras);
            }
            catch (Exception) { }
        }

        #endregion
    }
}
