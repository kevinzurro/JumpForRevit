using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.Structure;
using System.Windows.Controls;

namespace Jump
{
    public  class ArmaduraRepresentacion
    {
        IndependentTag representacion;
        IndependentTag etiqueta;
        XYZ posicion;

        public ArmaduraRepresentacion(IndependentTag representacion, IndependentTag etiqueta, int posicionEtiqueta)
        {
            Document doc = representacion.Document;
            View vista = doc.GetElement(representacion.OwnerViewId) as View;

            this.representacion = representacion;
            this.etiqueta = etiqueta;

            try
            {
                // Verifica que la posición sea
                switch (posicionEtiqueta)
                {
                    // Arriba a la izquierda
                    case (int)Posicion.ArribaIzquierda:
                        this.posicion = vista.UpDirection;
                        break;

                    // Arriba centro
                    case (int)Posicion.ArribaCentro:
                        this.posicion = vista.UpDirection;
                        break;

                    // Arriba a la derecha
                    case (int)Posicion.ArribaDerecha:
                        this.posicion = vista.UpDirection;
                        break;

                    // Centro a la izquierda
                    case (int)Posicion.MedioIzquierda:
                        this.posicion = vista.RightDirection.Negate();
                        break;

                    // Centro medio
                    case (int)Posicion.MedioCentro:
                        this.posicion = new XYZ();
                        break;

                    // Centro a la derecha
                    case (int)Posicion.MedioDerecha:
                        this.posicion = vista.RightDirection;
                        break;

                    // Abajo a la izquierda
                    case (int)Posicion.AbajoIzquierda:
                        this.posicion = vista.UpDirection.Negate();
                        break;

                    // Abajo centro
                    case (int)Posicion.AbajoCentro:
                        this.posicion = vista.UpDirection.Negate();
                        break;

                    // Abajo a la derecha
                    case (int)Posicion.AbajoDerecha:
                        this.posicion = vista.UpDirection.Negate();
                        break;

                    default:
                        break;
                }
            }
            catch (Exception)
            {
                this.posicion = new XYZ();
            }

            if (Etiqueta != null)
            {
                XYZ distancia = this.Etiqueta.get_BoundingBox(vista).Max - this.Etiqueta.get_BoundingBox(vista).Min;

                ElementTransformUtils.MoveElement(doc, this.Etiqueta.Id, Tools.ProyectarVectorSobreDireccionYSentido(distancia, PosicionEtiqueta));
            }
        }

        /// <summary> Obtiene la etiqueta individual de la armadura </summary>
        public IndependentTag Etiqueta
        {
            get { return etiqueta; }
        }

        /// <summary> Obtiene la representación de la armadura </summary>
        public IndependentTag Representacion
        {
            get { return representacion; }
        }

        /// <summary> Obtiene la dirección de la etiqueta en coordenadas globales </summary>
        public XYZ PosicionEtiqueta
        {
            get { return posicion; }
        }
    }
}
