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
        /// <summary> Inicio de la aplicación </summary>
        public Result OnStartup(UIControlledApplication application)
        {
            // Obtiene el idioma del programa
            Language.ObtenerIdiomaRevit(application.ControlledApplication.Language);

            // Ruta del ensamblado o de la addin que se está ejecutando
            string RutaDelEnsamblado = System.Reflection.Assembly.GetExecutingAssembly().Location;

            #region Idioma

            // Carga los textos para cada uno de los idiomas de los vectores secundarios
            Language.CargarIdiomasDisponibles();

            // Verifica el idioma de la interfaz
            AboutJump.IdiomaAddin = Tools.VerificarIdioma(AboutJump.IdiomaAddin);

            // Carga los textos para cada uno de los vectores secundarios
            Language.CargaTextosDeCadaIdioma();

            #endregion

            #region Paneles

            // Crear la pestaña con el nombre de la addin
            application.CreateRibbonTab(AboutJump.NombreAddin);

            //Crear los paneles
            RibbonPanel panelDetalleArmado = application.CreateRibbonPanel(AboutJump.NombreAddin, Language.ObtenerTexto(AboutJump.IdiomaAddin, "Tit1"));
            RibbonPanel panelVisibilidad = application.CreateRibbonPanel(AboutJump.NombreAddin, Language.ObtenerTexto(AboutJump.IdiomaAddin, "Tit2"));
            RibbonPanel panelHerramienta = application.CreateRibbonPanel(AboutJump.NombreAddin, Language.ObtenerTexto(AboutJump.IdiomaAddin, "Tit3"));

            #endregion

            #region Botones

            // Crea los botones
            //PushButton botonDespieceArmadura = panelDetalleArmado.AddItem(new PushButtonData("botonDespieceArmadura", Language.ObtenerTexto(IdiomaDelPrograma, "DetArm1"), RutaDelEnsamblado, "Jump.cmdArmaduraRepresentacion")) as PushButton;
            //PushButton botonPilote = panelDetalleArmado.AddItem(new PushButtonData("botonPilote", Language.ObtenerTexto(IdiomaDelPrograma, "Pil1"), RutaDelEnsamblado, "Jump.cmdPilotes")) as PushButton;
            //PushButton botonZapataCorrida = panelDetalleArmado.AddItem(new PushButtonData("botonZapataCorrida", Language.ObtenerTexto(IdiomaDelPrograma, "ZapCor1"), RutaDelEnsamblado, "Jump.cmdZapataCorrida")) as PushButton;
            //PushButton botonPlatea = panelDetalleArmado.AddItem(new PushButtonData("botonPlatea", Language.ObtenerTexto(IdiomaDelPrograma, "Pla1"), RutaDelEnsamblado, "Jump.cmdPlatea")) as PushButton;
            PushButton botonZapata = panelDetalleArmado.AddItem(new PushButtonData("botonZapata", Language.ObtenerTexto(AboutJump.IdiomaAddin, "Zap1"), RutaDelEnsamblado, "Jump.cmdZapatas")) as PushButton;
            PushButton botonColumna = panelDetalleArmado.AddItem(new PushButtonData("botonColumna", Language.ObtenerTexto(AboutJump.IdiomaAddin, "Col1"), RutaDelEnsamblado, "Jump.cmdColumnas")) as PushButton;
            PushButton botonMuro = panelDetalleArmado.AddItem(new PushButtonData("botonMuro", Language.ObtenerTexto(AboutJump.IdiomaAddin, "Mur1"), RutaDelEnsamblado, "Jump.cmdMuros")) as PushButton;
            PushButton botonViga = panelDetalleArmado.AddItem(new PushButtonData("botonViga", Language.ObtenerTexto(AboutJump.IdiomaAddin, "Vig1"), RutaDelEnsamblado, "Jump.cmdVigas")) as PushButton;
            //PushButton botonLosa = panelDetalleArmado.AddItem(new PushButtonData("botonLosa", Language.ObtenerTexto(IdiomaDelPrograma, "Los1"), RutaDelEnsamblado, "Jump.cmdLosas")) as PushButton;
            PushButton botonElemenEstructural = panelVisibilidad.AddItem(new PushButtonData("botonElemenEstructural", Language.ObtenerTexto(AboutJump.IdiomaAddin, "EleEst1"), RutaDelEnsamblado, "Jump.cmdVisibilidadEstructural")) as PushButton;
            PushButton botonElemenAnalitico = panelVisibilidad.AddItem(new PushButtonData("botonElemenAnalitico", Language.ObtenerTexto(AboutJump.IdiomaAddin, "EleAna1"), RutaDelEnsamblado, "Jump.cmdVisibilidadAnalitica")) as PushButton;
            PushButton botonOrdenEnumeracion = panelHerramienta.AddItem(new PushButtonData("botonOrdenEnumeracion", Language.ObtenerTexto(AboutJump.IdiomaAddin, "OrdYEnu1"), RutaDelEnsamblado, "Jump.cmdOrdenYEnumeracion")) as PushButton;
            PushButton botonIdioma = panelHerramienta.AddItem(new PushButtonData("botonIdioma", Language.ObtenerTexto(AboutJump.IdiomaAddin, "Idi1"), RutaDelEnsamblado, "Jump.cmdIdioma")) as PushButton;
            PushButton botonConfiguracion = panelHerramienta.AddItem(new PushButtonData("botonConfiguracion", Language.ObtenerTexto(AboutJump.IdiomaAddin, "Conf1"), RutaDelEnsamblado, "Jump.cmdConfiguraciones")) as PushButton;

            // Crea los botones para la visibilidad de la armadura que van en el botón desplegable
            PushButtonData botonArmaSolido = new PushButtonData("botonArmaSolido", Language.ObtenerTexto(AboutJump.IdiomaAddin, "VisArmSol"), RutaDelEnsamblado, "Jump.cmdArmaduraSolido");
            PushButtonData botonArmaFilamento = new PushButtonData("botonArmaFilamento", Language.ObtenerTexto(AboutJump.IdiomaAddin, "VisArmFil"), RutaDelEnsamblado, "Jump.cmdArmaduraFilamento");
            PushButtonData botonArmaSinTapa = new PushButtonData("botonArmaSinTapa", Language.ObtenerTexto(AboutJump.IdiomaAddin, "VisArmSinTap"), RutaDelEnsamblado, "Jump.cmdArmaduraSinTapar");
            PushButtonData botonArmaTapada = new PushButtonData("botonArmaTapada", Language.ObtenerTexto(AboutJump.IdiomaAddin, "VisArmTap"), RutaDelEnsamblado, "Jump.cmdArmaduraTapada");

            // Crear el boton desplegable para las armaduras
            PulldownButton botonArmaVisibilidad = panelVisibilidad.AddItem(new PulldownButtonData("botonArmaVisibilidad", Language.ObtenerTexto(AboutJump.IdiomaAddin, "VisArm1"))) as PulldownButton;

            #endregion

            #region Imagenes de botones

            // Agregar la imagen al botón
            //botonDespieceArmadura.LargeImage = new BitmapImage(new Uri("pack://application:,,,/Jump;component/Resources/Boton_Pilote.png"));
            //botonPilote.LargeImage = new BitmapImage(new Uri("pack://application:,,,/Jump;component/Resources/Boton_Pilote.png"));
            //botonZapataCorrida.LargeImage = new BitmapImage(new Uri("pack://application:,,,/Jump;component/Resources/Boton_Zapata_Corrida.png"));
            //botonPlatea.LargeImage = new BitmapImage(new Uri("pack://application:,,,/Jump;component/Resources/Boton_Platea.png"));
            botonZapata.LargeImage = new BitmapImage(new Uri("pack://application:,,,/Jump;component/Resources/Boton_Zapata.png"));
            botonColumna.LargeImage = new BitmapImage(new Uri("pack://application:,,,/Jump;component/Resources/Boton_Columna.png"));
            botonMuro.LargeImage = new BitmapImage(new Uri("pack://application:,,,/Jump;component/Resources/Boton_Muro.png"));
            botonViga.LargeImage = new BitmapImage(new Uri("pack://application:,,,/Jump;component/Resources/Boton_Viga.png"));
            //botonLosa.LargeImage = new BitmapImage(new Uri("pack://application:,,,/Jump;component/Resources/Boton_Losa.png"));
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
            //botonDespieceArmadura.ToolTip = Language.ObtenerTexto(IdiomaDelPrograma, "DetArm2");
            //botonPilote.ToolTip = Language.ObtenerTexto(IdiomaDelPrograma, "Pil2");
            //botonZapataCorrida.ToolTip = Language.ObtenerTexto(IdiomaDelPrograma, "ZapCor2");
            //botonPlatea.ToolTip = Language.ObtenerTexto(IdiomaDelPrograma, "Pla2");
            botonZapata.ToolTip = Language.ObtenerTexto(AboutJump.IdiomaAddin, "Zap2");
            botonColumna.ToolTip = Language.ObtenerTexto(AboutJump.IdiomaAddin, "Col2");
            botonMuro.ToolTip = Language.ObtenerTexto(AboutJump.IdiomaAddin, "Mur2");
            botonViga.ToolTip = Language.ObtenerTexto(AboutJump.IdiomaAddin, "Vig2");
            //botonLosa.ToolTip = Language.ObtenerTexto(IdiomaDelPrograma, "Los2");
            botonElemenEstructural.ToolTip = Language.ObtenerTexto(AboutJump.IdiomaAddin, "EleEst2");
            botonElemenAnalitico.ToolTip = Language.ObtenerTexto(AboutJump.IdiomaAddin, "EleAna2");
            botonArmaVisibilidad.ToolTip = Language.ObtenerTexto(AboutJump.IdiomaAddin, "VisArm2");
            botonOrdenEnumeracion.ToolTip = Language.ObtenerTexto(AboutJump.IdiomaAddin, "OrdYEnu2");
            botonIdioma.ToolTip = Language.ObtenerTexto(AboutJump.IdiomaAddin, "Idi2");
            botonConfiguracion.ToolTip = Language.ObtenerTexto(AboutJump.IdiomaAddin, "Conf2");

            #endregion

            #region Descripción larga de los botones

            // Crear la descripción larga de los botones
            //botonDespieceArmadura.LongDescription = Language.ObtenerTexto(IdiomaDelPrograma, "DetArm3");
            //botonPilote.LongDescription = Language.ObtenerTexto(IdiomaDelPrograma, "Pil3");
            //botonZapataCorrida.LongDescription = Language.ObtenerTexto(IdiomaDelPrograma, "ZapCor3");
            //botonPlatea.LongDescription = Language.ObtenerTexto(IdiomaDelPrograma, "Pla3");
            botonZapata.LongDescription = Language.ObtenerTexto(AboutJump.IdiomaAddin, "Zap3");
            botonColumna.LongDescription = Language.ObtenerTexto(AboutJump.IdiomaAddin, "Col3");
            botonMuro.LongDescription = Language.ObtenerTexto(AboutJump.IdiomaAddin, "Mur3");
            botonViga.LongDescription = Language.ObtenerTexto(AboutJump.IdiomaAddin, "Vig3");
            //botonLosa.LongDescription = Language.ObtenerTexto(IdiomaDelPrograma, "Los3");
            botonElemenEstructural.LongDescription = Language.ObtenerTexto(AboutJump.IdiomaAddin, "EleEst3");
            botonElemenAnalitico.LongDescription = Language.ObtenerTexto(AboutJump.IdiomaAddin, "EleAna3");
            botonArmaVisibilidad.LongDescription = Language.ObtenerTexto(AboutJump.IdiomaAddin, "VisArm3");
            botonOrdenEnumeracion.LongDescription = Language.ObtenerTexto(AboutJump.IdiomaAddin, "OrdYEnu3");
            botonIdioma.LongDescription = Language.ObtenerTexto(AboutJump.IdiomaAddin, "Idi3");
            botonConfiguracion.LongDescription = Language.ObtenerTexto(AboutJump.IdiomaAddin, "Conf3");

            #endregion

            return Result.Succeeded;
        }
        
        /// <summary> Finalizar la aplicación </summary>
        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }
    }
}
