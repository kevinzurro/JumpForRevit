using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jump
{
    public class PosicionTools
    {
        private static Dictionary<int, string> posicionesTotal = new Dictionary<int, string>
        {
            {(int)Posicion.ArribaIzquierda, Language.ObtenerTexto(AboutJump.IdiomaAddin, "Pos" + ((int)Posicion.ArribaIzquierda).ToString())},
            {(int)Posicion.ArribaCentro, Language.ObtenerTexto(AboutJump.IdiomaAddin, "Pos" + ((int)Posicion.ArribaCentro).ToString())},
            {(int)Posicion.ArribaDerecha, Language.ObtenerTexto(AboutJump.IdiomaAddin, "Pos" + ((int)Posicion.ArribaDerecha).ToString())},
            {(int)Posicion.MedioIzquierda, Language.ObtenerTexto(AboutJump.IdiomaAddin, "Pos" + ((int)Posicion.MedioIzquierda).ToString())},
            {(int)Posicion.MedioCentro, Language.ObtenerTexto(AboutJump.IdiomaAddin, "Pos" + ((int)Posicion.MedioCentro).ToString())},
            {(int)Posicion.MedioDerecha, Language.ObtenerTexto(AboutJump.IdiomaAddin, "Pos" + ((int)Posicion.MedioDerecha).ToString())},
            {(int)Posicion.AbajoIzquierda, Language.ObtenerTexto(AboutJump.IdiomaAddin, "Pos" + ((int)Posicion.AbajoIzquierda).ToString())},
            {(int)Posicion.AbajoCentro, Language.ObtenerTexto(AboutJump.IdiomaAddin, "Pos" + ((int)Posicion.AbajoCentro).ToString())},
            {(int)Posicion.AbajoDerecha, Language.ObtenerTexto(AboutJump.IdiomaAddin, "Pos" + ((int)Posicion.AbajoDerecha).ToString())}
        };

        private static Dictionary<int, string> posicionesCotaProfundidad = new Dictionary<int, string>
        {
            {(int)Posicion.AbajoIzquierda, Language.ObtenerTexto(AboutJump.IdiomaAddin, "Pos" + ((int)Posicion.AbajoIzquierda).ToString())},
            {(int)Posicion.AbajoDerecha, Language.ObtenerTexto(AboutJump.IdiomaAddin, "Pos" + ((int)Posicion.AbajoDerecha).ToString())}
        };

        public static ObservableCollection<KeyValuePair<int, string>> PosicionesTodas 
            = new ObservableCollection<KeyValuePair<int, string>>(posicionesTotal.ToList());

        public static ObservableCollection<KeyValuePair<int, string>> PosicionesCotaProfundidad
            = new ObservableCollection<KeyValuePair<int, string>>(posicionesCotaProfundidad.ToList());

        public static List<string> Posiciones(string idioma)
        {
            List<string> posiciones = new List<string>()
            {
                Language.ObtenerTexto(idioma, "Pos" + ((int)Posicion.ArribaIzquierda).ToString()),
                Language.ObtenerTexto(idioma, "Pos" + ((int)Posicion.ArribaCentro)).ToString(),
                Language.ObtenerTexto(idioma, "Pos" + ((int)Posicion.ArribaDerecha)).ToString(),
                Language.ObtenerTexto(idioma, "Pos" + ((int)Posicion.MedioIzquierda)).ToString(),
                Language.ObtenerTexto(idioma, "Pos" + ((int)Posicion.MedioCentro)).ToString(),
                Language.ObtenerTexto(idioma, "Pos" + ((int)Posicion.MedioDerecha)).ToString(),
                Language.ObtenerTexto(idioma, "Pos" + ((int)Posicion.AbajoIzquierda)).ToString(),
                Language.ObtenerTexto(idioma, "Pos" + ((int)Posicion.AbajoCentro)).ToString(),
                Language.ObtenerTexto(idioma, "Pos" + ((int)Posicion.AbajoDerecha)).ToString(),
            };

            return posiciones;
        }

    }
}
