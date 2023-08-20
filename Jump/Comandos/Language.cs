﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.Structure;
using System.Windows.Media.Imaging;
using System.Collections;

namespace Jump
{
    public class Language
    {
        // Crear un vector principal con los idiomas disponibles
        public static List<string> IdiomasDisponibles = new List<string>();

        // Crear cada uno de los vectores secundarios para los diferentes idiomas
        private static Dictionary<string, string> Aleman = new Dictionary<string, string>();
        private static Dictionary<string, string> Espanol = new Dictionary<string, string>();
        private static Dictionary<string, string> Frances = new Dictionary<string, string>();
        private static Dictionary<string, string> Holandes = new Dictionary<string, string>();
        private static Dictionary<string, string> Ingles = new Dictionary<string, string>();
        private static Dictionary<string, string> Italiano = new Dictionary<string, string>();
        private static Dictionary<string, string> Japones = new Dictionary<string, string>();
        private static Dictionary<string, string> Portugues = new Dictionary<string, string>();

        ///<summary> Carga los idiomas disponibles al vector principal </summary>
        public static void CargarIdiomasDisponibles()
        {
            // Limpia el contenido del vector de idiomas disponibles
            IdiomasDisponibles.Clear();

            // Agregar los idiomas disponibles al vector principal

            //IdiomasDisponibles.Add("English);
            IdiomasDisponibles.Add("Español");
            //IdiomasDisponibles.Add("French");
            //IdiomasDisponibles.Add("German");
            //IdiomasDisponibles.Add("Italian");
            //IdiomasDisponibles.Add("Japanese");
            //IdiomasDisponibles.Add("Korean");
            //IdiomasDisponibles.Add("Polish");
            //IdiomasDisponibles.Add("Simplified Chinese");
            //IdiomasDisponibles.Add("Traditional Chinese");
            //IdiomasDisponibles.Add("Português");
            //IdiomasDisponibles.Add("Russian");
            //IdiomasDisponibles.Add("Czech");
            //IdiomasDisponibles.Add("Nederlands");
        }

        ///<summary> Carga los textos para cada uno de los vectores secundarios </summary>
        public static void CargaTextosDeCadaIdioma()
        {
            // Limpia los vectores secundarios
            Aleman.Clear();
            Espanol.Clear();
            Frances.Clear();
            Holandes.Clear();
            Ingles.Clear();
            Italiano.Clear();
            Japones.Clear();
            Portugues.Clear();

            // Cargar los distintos idiomas
            CargarAleman();
            CargarEspanol();
            CargarFrances();
            CargarHolandes();
            CargarIngles();
            CargarItaliano();
            CargarJapones();
            CargarPortugues();
        }

        ///<summary> Carga el idioma Aleman </summary>
        private static void CargarAleman()
        {
            // Agregar los textos de aleman al vector secundario Aleman[]
            Aleman.Add("Idi4", "Sprache auswählen");
            Aleman.Add("Idi5", "Verfügbare Sprachen");
            Aleman.Add("Idi6", "Sparen");
        }

        ///<summary> Carga el idioma Español </summary>
        private static void CargarEspanol()
        {
            // Agregar los textos de español al vector secundario Español[]
            Espanol.Add("Tit1", "Detalles de armado");
            Espanol.Add("Tit2", "Visibilidad");
            Espanol.Add("Tit3", "Herramientas");

            Espanol.Add("Pil1", "Pilotes");
            Espanol.Add("Pil2", "Pilotes descripción corta");
            Espanol.Add("Pil3", "Pilotes descripción larga");
            Espanol.Add("Pil4", "Pilotes");
            Espanol.Add("Pil5", "Aceptar");
            Espanol.Add("Pil6", "Cancelar");

            Espanol.Add("ZapCor1", "Zapata \nCorrida");
            Espanol.Add("ZapCor2", "Zapata Corrida descripción corta");
            Espanol.Add("ZapCor3", "Zapata Corrida descripción larga");
            Espanol.Add("ZapCor4", "Zapata Corrida");
            Espanol.Add("ZapCor5", "Aceptar");
            Espanol.Add("ZapCor6", "Cancelar");
            Espanol.Add("ZapCor1-2", "Selección");
            Espanol.Add("ZapCor1-3", "Todos");
            Espanol.Add("ZapCor1-4", "Elementos seleccionados");
            Espanol.Add("ZapCor1-5", "Elementos de la lista");
            Espanol.Add("ZapCor2-1", "Etiquetas");
            Espanol.Add("ZapCor2-2", "Escala");
            Espanol.Add("ZapCor2-3", "Etiqueta de zapata corrida");
            Espanol.Add("ZapCor2-4", "Etiqueta de armadura");
            Espanol.Add("ZapCor2-5", "Longitud parcial de la barra");
            Espanol.Add("ZapCor2-6", "Cota lineal");
            Espanol.Add("ZapCor2-7", "Cota de elevación");
            Espanol.Add("ZapCor3-1", "Vista previa");
            Espanol.Add("ZapCor4-1", "Sección");
            Espanol.Add("ZapCor4-2", "Vista X-X");
            Espanol.Add("ZapCor4-3", "Vista Y-Y");
            Espanol.Add("ZapCor5-1", "Detalle de zapatas corridas");

            Espanol.Add("Pla1", "Platea");
            Espanol.Add("Pla2", "Platea descripción corta");
            Espanol.Add("Pla3", "Platea descripción larga");
            Espanol.Add("Pla4", "Platea");
            Espanol.Add("Pla5", "Aceptar");
            Espanol.Add("Pla6", "Cancelar");
            Espanol.Add("Pla1-2", "Selección");
            Espanol.Add("Pla1-3", "Todos");
            Espanol.Add("Pla1-4", "Elementos seleccionados");
            Espanol.Add("Pla1-5", "Elementos de la lista");
            Espanol.Add("Pla2-1", "Etiquetas");
            Espanol.Add("Pla2-2", "Escala");
            Espanol.Add("Pla2-3", "Etiqueta de platea");
            Espanol.Add("Pla2-4", "Etiqueta de armadura");
            Espanol.Add("Pla2-5", "Longitud parcial de la barra");
            Espanol.Add("Pla2-6", "Cota lineal");
            Espanol.Add("Pla2-7", "Cota de elevación");
            Espanol.Add("Pla3-1", "Vista previa");
            Espanol.Add("Pla4-1", "Sección");
            Espanol.Add("Pla4-2", "Vista X-X");
            Espanol.Add("Pla4-3", "Vista Y-Y");
            Espanol.Add("Pla5-1", "Detalle de plateas");

            Espanol.Add("Zap1", "Zapatas");
            Espanol.Add("Zap2", "Zapatas descripción corta");
            Espanol.Add("Zap3", "Zapatas descripción larga");
            Espanol.Add("Zap4", "Zapatas");
            Espanol.Add("Zap5", "Aceptar");
            Espanol.Add("Zap6", "Cancelar");
            Espanol.Add("Zap1-2", "Selección");
            Espanol.Add("Zap1-3", "Todos");
            Espanol.Add("Zap1-4", "Elementos seleccionados");
            Espanol.Add("Zap1-5", "Elementos de la lista");
            Espanol.Add("Zap2-1", "Etiquetas");
            Espanol.Add("Zap2-2", "Escala");
            Espanol.Add("Zap2-3", "Etiqueta de cimentación");
            Espanol.Add("Zap2-4", "Etiqueta de armadura");
            Espanol.Add("Zap2-5", "Longitud parcial de la barra");
            Espanol.Add("Zap2-6", "Cota lineal");
            Espanol.Add("Zap2-7", "Cota de elevación");
            Espanol.Add("Zap3-1", "Vista previa");
            Espanol.Add("Zap4-1", "Sección");
            Espanol.Add("Zap4-2", "Vista X-X");
            Espanol.Add("Zap4-3", "Vista Y-Y");
            Espanol.Add("Zap5-1", "Detalle de zapatas");

            Espanol.Add("Col1", "Columnas");
            Espanol.Add("Col2", "Columnas descripción corta");
            Espanol.Add("Col3", "Columnas descripción larga");
            Espanol.Add("Col4", "Columnas");
            Espanol.Add("Col5", "Aceptar");
            Espanol.Add("Col6", "Cancelar");
            Espanol.Add("Col1-2", "Selección");
            Espanol.Add("Col1-3", "Todos");
            Espanol.Add("Col1-4", "Elementos seleccionados");
            Espanol.Add("Col1-5", "Elementos de la lista");
            Espanol.Add("Col2-1", "Etiquetas");
            Espanol.Add("Col2-2", "Escala");
            Espanol.Add("Col2-3", "Etiqueta de columna");
            Espanol.Add("Col2-4", "Etiqueta de armadura");
            Espanol.Add("Col2-5", "Longitud parcial de la barra");
            Espanol.Add("Col2-6", "Cota lineal");
            Espanol.Add("Col2-7", "Cota de elevación");
            Espanol.Add("Col3-1", "Vista previa");
            Espanol.Add("Col4-1", "Sección");
            Espanol.Add("Col4-2", "Vista X-X");
            Espanol.Add("Col4-3", "Vista Y-Y");
            Espanol.Add("Col5-1", "Detalle de columnas");

            Espanol.Add("Mur1", "Muros");
            Espanol.Add("Mur2", "Muros descripción corta");
            Espanol.Add("Mur3", "Muros descripción larga");
            Espanol.Add("Mur4", "Muros");
            Espanol.Add("Mur5", "Aceptar");
            Espanol.Add("Mur6", "Cancelar");
            Espanol.Add("Mur1-2", "Selección");
            Espanol.Add("Mur1-3", "Todos");
            Espanol.Add("Mur1-4", "Elementos seleccionados");
            Espanol.Add("Mur1-5", "Elementos de la lista");
            Espanol.Add("Mur2-1", "Etiquetas");
            Espanol.Add("Mur2-2", "Escala");
            Espanol.Add("Mur2-3", "Etiqueta de muro");
            Espanol.Add("Mur2-4", "Etiqueta de armadura");
            Espanol.Add("Mur2-5", "Longitud parcial de la barra");
            Espanol.Add("Mur2-6", "Cota lineal");
            Espanol.Add("Mur2-7", "Cota de elevación");
            Espanol.Add("Mur3-1", "Vista previa");
            Espanol.Add("Mur4-1", "Sección");
            Espanol.Add("Mur4-2", "Vista X-X");
            Espanol.Add("Mur4-3", "Vista Y-Y");
            Espanol.Add("Mur5-1", "Detalle de muros");

            Espanol.Add("Vig1", "Vigas");
            Espanol.Add("Vig2", "Vigas descripción corta");
            Espanol.Add("Vig3", "Vigas descripción larga");
            Espanol.Add("Vig4", "Vigas");
            Espanol.Add("Vig5", "Aceptar");
            Espanol.Add("Vig6", "Cancelar");
            Espanol.Add("Vig1-2", "Selección");
            Espanol.Add("Vig1-3", "Todos");
            Espanol.Add("Vig1-4", "Elementos seleccionados");
            Espanol.Add("Vig1-5", "Elementos de la lista");
            Espanol.Add("Vig2-1", "Etiquetas");
            Espanol.Add("Vig2-2", "Escala");
            Espanol.Add("Vig2-3", "Etiqueta de viga");
            Espanol.Add("Vig2-4", "Etiqueta de armadura");
            Espanol.Add("Vig2-5", "Longitud parcial de la barra");
            Espanol.Add("Vig2-6", "Cota lineal");
            Espanol.Add("Vig2-7", "Cota de elevación");
            Espanol.Add("Vig3-1", "Vista previa");
            Espanol.Add("Vig4-1", "Sección");
            Espanol.Add("Vig4-2", "Vista X-X");
            Espanol.Add("Vig4-3", "Vista Y-Y");
            Espanol.Add("Vig5-1", "Detalle de vigas");

            Espanol.Add("Los1", "Losas");
            Espanol.Add("Los2", "Losas descripción corta");
            Espanol.Add("Los3", "Losas descripción larga");
            Espanol.Add("Los4", "Losas");
            Espanol.Add("Los5", "Aceptar");
            Espanol.Add("Los6", "Cancelar");

            Espanol.Add("VisArm1", "Visibilidad \nde Armadura");
            Espanol.Add("VisArm2", "Visiblidad de armadura descripción corta");
            Espanol.Add("VisArm3", "Visiblidad de armadura descripción larga");
            Espanol.Add("VisArmSol", "Solido");
            Espanol.Add("VisArmFil", "Filamento");
            Espanol.Add("VisArmSinTap", "Sin tapar");            
            Espanol.Add("VisArmTap", "Tapada");

            Espanol.Add("DetArm1", "Despiece \nde barra");
            Espanol.Add("DetArm2", "Despiece de barra descripción corta");
            Espanol.Add("DetArm3", "Despiece de barra descripción larga");
            Espanol.Add("DetArm4", "Despiece de barra");
            Espanol.Add("DetArm1-1", "Etiqueta de armadura");
            Espanol.Add("DetArm1-2", "Longitud parcial de la barra");
            Espanol.Add("DetArm2-1", "Despiece de armadura");
            Espanol.Add("DetArm2-2", "Seleccione una barra");
            Espanol.Add("DetArm2-3", "Seleccione un punto donde quiere colocar el detalle");

            Espanol.Add("Conf1", "Configuraciones");
            Espanol.Add("Conf2", "Permite cambiar las configuraciones predeterminadas");
            Espanol.Add("Conf3", "Abre la ventana de configuraciones que permite cambiar los parámetros predeterminados");
            Espanol.Add("Conf4", "Configuraciones");
            Espanol.Add("Conf5", "Aceptar");
            Espanol.Add("Conf6", "Cancelar");
            Espanol.Add("Conf1-1", "General");
            Espanol.Add("Conf1-2-1", "Configuraciones");
            Espanol.Add("Conf1-2-2", "Precisión para ordenar los elementos");
            Espanol.Add("Conf1-2-3", "La precisión en X como en Y se utiliza para el Orden y enumeración de los elementos");
            Espanol.Add("Conf1-3-1", "Corte transversal");
            Espanol.Add("Conf1-3-2", "Local    -    En las coordenadas de la familia ( X ´ ; Y ´ )");
            Espanol.Add("Conf1-3-3", "Global   -    En las coordenadas del proyecto ( X ; Y )");
            Espanol.Add("Conf2-1", "Armaduras");
            Espanol.Add("Conf2-2-1", "Enumeración");
            Espanol.Add("Conf2-2-2", "Por elemento");
            Espanol.Add("Conf2-2-3", "Por proyecto");
            Espanol.Add("Conf2-3-1", "Dibujo de las armaduras");
            Espanol.Add("Conf2-3-2", "Líneas centrales");
            Espanol.Add("Conf2-3-3", "Líneas de borde");
            Espanol.Add("Conf2-4-1", "Posición del texto");
            Espanol.Add("Conf2-4-2", "Arriba");
            Espanol.Add("Conf2-4-3", "Abajo");
            Espanol.Add("Conf3-1", "Estilos de líneas");
            Espanol.Add("Conf3-2", "Diámetro");
            Espanol.Add("Conf3-3", "Estilo de línea");
            Espanol.Add("Conf3-4", "Jump representa a las barras de acero como Línea de detalle, permitiendo utilizar distintos estilos de líneas para cada diámetro en particular.");
            Espanol.Add("Conf3-5", "Guardado de estilos de líneas");
            Espanol.Add("Conf4-1", "Etiquetas");
            Espanol.Add("Conf4-1-1", "Etiquetas independientes");
            Espanol.Add("Conf4-1-2", "Elemento");
            Espanol.Add("Conf4-1-3", "Posición de la etiqueta");
            Espanol.Add("Conf4-1-1-1", "Pilotes");
            Espanol.Add("Conf4-1-1-2", "Zapata corrida");
            Espanol.Add("Conf4-1-1-3", "Platea");
            Espanol.Add("Conf4-1-1-4", "Zapatas");
            Espanol.Add("Conf4-1-1-5", "Columnas");
            Espanol.Add("Conf4-1-1-6", "Muros");
            Espanol.Add("Conf4-1-1-7", "Vigas");
            Espanol.Add("Conf4-1-1-8", "Losas");
            Espanol.Add("Conf4-2-1", "Etiqueta de armaduras");
            Espanol.Add("Conf4-2-2", "Tipo de armadura");
            Espanol.Add("Conf4-2-3", "Posición de la etiqueta");
            Espanol.Add("Conf4-2-1-1", "Armadura");
            Espanol.Add("Conf4-2-1-2", "Área de refuerzo");
            Espanol.Add("Conf4-2-1-3", "Armadura en sistema");
            Espanol.Add("Conf5-1", "Cotas");
            Espanol.Add("Conf5-1-1", "Cotas lineales");
            Espanol.Add("Conf5-2-1", "Cotas de profundidad");

            Espanol.Add("OrdYEnu1", "Orden \ny enumeración");
            Espanol.Add("OrdYEnu2", "Orden y enumeración descripción corta");
            Espanol.Add("OrdYEnu3", "Orden y enumeración descripción larga");
            Espanol.Add("OrdYEnu4", "Orden y enumeración");
            Espanol.Add("OrdYEnu5", "Aceptar");
            Espanol.Add("OrdYEnu6", "Cancelar");
            Espanol.Add("OrdYEnu1-1", "Categoría");
            Espanol.Add("OrdYEnu1-2", "Todos");
            Espanol.Add("OrdYEnu1-3", "Elementos seleccionados");
            Espanol.Add("OrdYEnu1-4", "Elementos de la lista");
            Espanol.Add("OrdYEnu2-1", "Orden");
            Espanol.Add("OrdYEnu3-1", "Enumeración");
            Espanol.Add("OrdYEnu3-2", "Prefijo");
            Espanol.Add("OrdYEnu3-3", "Número inicial");
            Espanol.Add("OrdYEnu3-4", "Incremento");
            Espanol.Add("OrdYEnu3-5", "Sufijo");
            Espanol.Add("OrdYEnu3-6", "Vista previa");
            Espanol.Add("OrdYEnu3-7", "Parámetro");
            Espanol.Add("OrdYEnu3-8", "Enumeración");
            Espanol.Add("OrdYEnu4-1", "Comando de enumeración");

            Espanol.Add("Idi1", "Idioma");
            Espanol.Add("Idi2", "Permite cambiar el idioma");
            Espanol.Add("Idi3", "Abre una ventana que permite cambiar el idioma de la interfaz");
            Espanol.Add("Idi4", "Idioma");
            Espanol.Add("Idi5", "Aceptar");
            Espanol.Add("Idi6", "Cancelar");
            Espanol.Add("Idi1-1", "Seleccione el idioma");
            Espanol.Add("Idi1-2", "Idioma");

            Espanol.Add("EleEst1", "Elementos \nestructurales");
            Espanol.Add("EleEst2", "Oculta los elementos que no son estructurales");
            Espanol.Add("EleEst3", "Oculta todos los elementos no estructurales en la vista actual");

            Espanol.Add("EleAna1", "Elementos \nanalíticos");
            Espanol.Add("EleAna2", "Oculta los elementos que no son análiticos");
            Espanol.Add("EleAna3", "Oculta todos los elementos dejando a los elementos análiticos visibles en la vista");

            Espanol.Add("ActBar1", "Actualizar la forma de las barras");
            Espanol.Add("ActBar2", "Actualizador de barras");

            Espanol.Add("EliBar1", "Eliminar la representación de las barras");
            Espanol.Add("EliBar2", "Eliminador de barras");

            Espanol.Add("BarPro1", "Procesando ");
            Espanol.Add("BarPro2", " elementos de ");

            Espanol.Add("Pos0", "1 - Arriba Izquierda");            
            Espanol.Add("Pos1", "2 - Arriba Centro");
            Espanol.Add("Pos2", "3 - Arriba Derecha");
            Espanol.Add("Pos3", "4 - Medio Izquierda");
            Espanol.Add("Pos4", "5 - Medio Centro");
            Espanol.Add("Pos5", "6 - Medio Derecha");
            Espanol.Add("Pos6", "7 - Abajo Izquierda");
            Espanol.Add("Pos7", "8 - Abajo Centro");
            Espanol.Add("Pos8", "9 - Abajo Derecha");
            Espanol.Add("Pos10", "Arriba");
            Espanol.Add("Pos11", "Abajo");
            Espanol.Add("Pos12", "Izquierda");
            Espanol.Add("Pos13", "Derecha");
        }

        ///<summary> Carga el idioma Frances </summary>
        private static void CargarFrances()
        {
            // Agregar los textos de frances al vector secundario Frances[]
            Frances.Add("Idi4", "Choisir la langue");
            Frances.Add("Idi5", "Langues disponibles");
            Frances.Add("Idi6", "Sauver");
        }

        ///<summary> Carga el idioma Holandes </summary>
        private static void CargarHolandes()
        {
            // Agregar los textos de holandes al vector secundario Holandes[]
            Holandes.Add("Idi4", "Selecteer taal");
            Holandes.Add("Idi5", "Beschikbare talen");
            Holandes.Add("Idi6", "Opslaan");
        }

        ///<summary> Carga el idioma Ingles </summary>
        private static void CargarIngles()
        {
            // Agregar los textos de ingles al vector secundario Ingles[]
            Ingles.Add("Idi4", "Select Language");
            Ingles.Add("Idi5", "Available languages");
            Ingles.Add("Idi6", "Save");
        }      
        
        ///<summary> Carga el idioma Italiano </summary>
        private static void CargarItaliano()
        {
            // Agregar los textos de italiano al vector secundario Italiano[]
            Italiano.Add("Idi4", "Seleziona la lingua");
            Italiano.Add("Idi5", "Lingue disponibili");
            Italiano.Add("Idi6", "Salva");
        }

        ///<summary> Carga el idioma Japones </summary>
        private static void CargarJapones()
        {
            // Agregar los textos de japones al vector secundario Japones[]
            Japones.Add("Idi4", "言語を選択する");
            Japones.Add("Idi5", "利用可能な言語");
            Japones.Add("Idi6", "セーブ");
        }        

        ///<summary> Carga el idioma Portugues </summary>
        private static void CargarPortugues()
        {
            // Agregar los textos de portugues al vector secundario Portugues[]
            Portugues.Add("Idi4", "Selecionar idioma");
            Portugues.Add("Idi5", "Línguas disponíveis");
            Portugues.Add("Idi6", "Salvar");
        }

        ///<summary> Devuelve el texto según el idioma </summary>
        public static string ObtenerTexto(string IdiomaDelPrograma, string key)
        {
            // Crear la variable a devolver
            string texto = null;

            // Devuelve el texto según el idioma seleccionado
            switch (IdiomaDelPrograma)
            {
                case "Deutsch":
                    texto = Aleman[key];
                    break;

                case "English":
                    texto = Ingles[key];
                    break;

                case "Español":
                    texto = Espanol[key];
                    break;

                case "French":
                    texto = Frances[key];
                    break;

                case "Italian":
                    texto = Italiano[key];
                    break;

                case "Japanese":
                    texto = Japones[key];
                    break;

                case "Nederlands":
                    texto = Holandes[key];
                    break;
                    
                case "Português":
                    texto = Portugues[key];
                    break;

                default:
                    break;
            }
            return texto;
        }
    }
}