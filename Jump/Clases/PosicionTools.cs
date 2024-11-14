using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jump
{
    public class PosicionTools
    {
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
