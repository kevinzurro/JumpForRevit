using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.DB.ExtensibleStorage;

namespace Jump
{
    public class ArmaduraRepresentacion
    {
        // Variables necesarias
        Document doc;
        View vistaBarra;
        Rebar barraRefuerzo;
        ElementId armaduraID;
        List<CurveElement> listaCurvas;
        List<TextNote> listaTextos;
        IndependentTag etiquetaArmadura;
        TextNoteType tipoEtiquetaTexto;
        FamilySymbol tipoEtiquetaArmadura;
        XYZ posicion;
        List<ElementId> listaCurvasId = new List<ElementId>();
        List<ElementId> listaTextosId = new List<ElementId>();

        // Constructor la armadura
        public ArmaduraRepresentacion(Document doc, View vista, Rebar barra)
        {
            // Variable necesarias
            this.doc = doc;            
            this.vistaBarra = vista;
            this.barraRefuerzo = barra;
            this.armaduraID = barra.Id;
        }

        /// <summary> Obtiene el documento </summary>
        public Document Documento
        {
            get { return this.doc; }
        }

        /// <summary> Obtiene la vista de la barra </summary>
        public View Vista
        {
            get { return this.vistaBarra; }
        }

        /// <summary> Obtiene la barra </summary>
        public Rebar Barra
        {
            get { return this.barraRefuerzo; }
        }

        /// <summary> ID de la Representación de la armadura, es igual al ID de la Barra </summary>
        public ElementId Id
        {
            get { return this.armaduraID; }

            set { this.armaduraID = value; }
        }

        /// <summary> Asigna u obtiene las curvas </summary>
        public List<CurveElement> CurvasDeArmadura
        {
            get { return this.listaCurvas; }

            set
            {
                this.listaCurvas = value;

                this.listaCurvasId.Clear();

                // Recorre la lista
                foreach (CurveElement curva in this.listaCurvas)
                {
                    //Agrega el ID a la lista
                    listaCurvasId.Add(curva.Id);
                }
            }
        }

        /// <summary> Asigna u obtiene los textos de las longitudes parciales </summary>
        public List<TextNote> TextosDeLongitudesParciales
        {
            get { return this.listaTextos; }

            set
            {
                this.listaTextos = value;

                this.listaTextosId.Clear();

                // Recorre la lista
                foreach (TextNote texto in this.listaTextos)
                {
                    //Agrega el ID a la lista
                    listaTextosId.Add(texto.Id);
                }
            }
        }

        /// <summary> Asigna u obtiene el ElementId de las curvas </summary>
        public List<ElementId> ListaCurvasId
        {
            get { return listaCurvasId; }

            set { this.listaCurvasId = value; }
        }

        /// <summary> Asigna u obtiene el ElementId de textos de longitudes parciales </summary>
        public List<ElementId> ListaTextosId
        {
            get { return listaTextosId; }

            set { this.listaTextosId = value; }
        }

        /// <summary> Asigna u obtiene la etiqueta para la armadura </summary>
        public IndependentTag EtiquetaArmadura
        {
            get { return this.etiquetaArmadura; }

            set { this.etiquetaArmadura = value; }
        }

        /// <summary> Asigna u obtiene el tipo de texto </summary>
        public TextNoteType TipoDeTexto
        {
            get { return this.tipoEtiquetaTexto; }

            set { this.tipoEtiquetaTexto = value; }
        }

        /// <summary> Asigna u obtiene el tipo de etiqueta para la armadura </summary>
        public FamilySymbol TipoEtiquetaArmadura
        {
            get { return this.tipoEtiquetaArmadura; }

            set { this.tipoEtiquetaArmadura = value; }
        }

        /// <summary> Asigna u obtiene la posición de la Representación de la armadura en coordenadas globales </summary>
        public XYZ Posicion
        {
            get { return this.posicion; }

            set { this.posicion = value; }
        }

        /// <summary> Obtiene el punto medio de la Representación en coordenadas globales </summary>
        public XYZ PuntoMedio
        {
            get
            {
                return Tools.ObtenerBaricentroElemento(this.Barra.get_BoundingBox(this.Vista));
            }
        }

        /// <summary> Dibuja la armadura junto con su textos </summary>
        public void DibujarArmaduraSegunDatagridview(System.Windows.Forms.DataGridView dgw)
        {
            // Dibuja las armaduras y asigna los estilos de líneas en función de cada diámetro
            this.CurvasDeArmadura = Tools.DibujarArmaduraSegunDatagridview(dgw, this.doc, this.vistaBarra, this.barraRefuerzo);

            // Crea notas de texto con la longitud parcial de la barra
            this.TextosDeLongitudesParciales = Tools.CrearTextNoteDeArmadura(this.doc, this.vistaBarra, this.barraRefuerzo, this.TipoDeTexto);

            Inicio.listaArmaduraRepresentacion.Add(this);
        }

        /// <summary> Mueve la Representación de la Armadura con la etiqueta una distancia dada </summary>
        public void MoverArmaduraRepresentacionConEtiqueta(XYZ distancia)
        {
            // Crea la lista
            List<Element> lista = new List<Element>();

            // Agrega las curvas a la lista
            lista.AddRange(CurvasDeArmadura);

            // Agrega los textos a la lista
            lista.AddRange(TextosDeLongitudesParciales);

            ElementTransformUtils.MoveElements(this.doc, Tools.ObtenerIdElemento(lista), distancia);

            // Verifica que la etiqueta no sea nula
            if (EtiquetaArmadura != null)
            {
                try
                {
                    // Mueve la etiqueta
                    ElementTransformUtils.MoveElement(Documento, EtiquetaArmadura.Id, distancia);                    
                }
                catch (Exception) { }
            }
        }

        /// <summary> Mueve la Representación de la Armadura una distancia dada </summary>
        public void MoverArmaduraRepresentacion(XYZ distancia)
        {
            // Crea la lista
            List<Element> lista = new List<Element>();

            // Agrega las curvas a la lista
            lista.AddRange(CurvasDeArmadura);

            // Agrega los textos a la lista
            lista.AddRange(TextosDeLongitudesParciales);

            // Mueve el grupo
            ElementTransformUtils.MoveElements(Documento, Tools.ObtenerIdElemento(lista), distancia);
        }

        /// <summary> Obtiene el BoundingBoxXYZ con los textos y curvas de detalle en coordenadas globales </summary>
        public BoundingBoxXYZ ObtenerBoundingBoxDeArmadura()
        {
            // Crea la lista
            List<Element> lista = new List<Element>();

            // Verifica que la etiqueta no sea nula
            if (EtiquetaArmadura != null)
            {
                lista.Add(EtiquetaArmadura);
            }

            // Agrega las curvas a la lista
            lista.AddRange(CurvasDeArmadura);

            // Agrega los textos a la lista
            lista.AddRange(TextosDeLongitudesParciales);

            BoundingBoxXYZ bb = new BoundingBoxXYZ();

            bb.Transform = Transform.Identity;

            double xMax = lista.Max(x => x.get_BoundingBox(this.Vista).Max.X);
            double yMax = lista.Max(x => x.get_BoundingBox(this.Vista).Max.Y);
            double zMax = lista.Max(x => x.get_BoundingBox(this.Vista).Max.Z);

            double xMin = lista.Min(x => x.get_BoundingBox(this.Vista).Min.X);
            double yMin = lista.Min(x => x.get_BoundingBox(this.Vista).Min.Y);
            double zMin = lista.Min(x => x.get_BoundingBox(this.Vista).Min.Z);

            bb.Max = new XYZ(xMax, yMax, zMax);
            bb.Min = new XYZ(xMin, yMin, zMin);

            return bb;
        }

        /// <summary> Elimina la representación de la armadura </summary>
        public void Eliminar()
        {
            // Verifica que no sea nulo
            if (this.listaCurvas != null)
            {
                // Recorre las curvas
                foreach (CurveElement curva in this.listaCurvas)
                {
                    try
                    {
                        // Verifica que no sea nulo
                        if (curva != null)
                        {
                            // Elimina la curva
                            doc.Delete(curva.Id);
                        }
                    }
                    catch (Exception)
                    {
                        EliminarCurvasPorId();

                        break;
                    }
                }
            }

            // Verifica que no sea nulo
            if (this.listaTextos != null)
            {
                // Recorre los textos
                foreach (TextNote texto in this.listaTextos)
                {
                    try
                    {
                        // Verifica que no sea nulo
                        if (texto != null)
                        {
                            // Elimina el texto
                            doc.Delete(texto.Id);
                        }
                    }
                    catch (Exception)
                    {
                        EliminarTextosPorId();

                        break;
                    }
                }
            }

            Tools.EliminarRepresentacionesEnBarra(this.Barra);
        }

        /// <summary> Elimina las curvas de la armadura por el Id del historial </summary>
        private void EliminarCurvasPorId()
        {
            // Recorre las curvas
            foreach (ElementId curvaID in this.listaCurvasId)
            {
                try
                {
                    // Verifica que no sea nulo
                    if (curvaID != null)
                    {
                        // Elimina la curva
                        doc.Delete(curvaID);
                    }
                }
                catch (Exception) { }
            }
        }

        /// <summary> Elimina los textos de la armadura por el Id del historial </summary>
        private void EliminarTextosPorId()
        {
            // Recorre las curvas
            foreach (ElementId textoID in this.listaTextosId)
            {
                try
                {
                    // Verifica que no sea nulo
                    if (textoID != null)
                    {
                        // Elimina la curva
                        doc.Delete(textoID);
                    }
                }
                catch (Exception) { }
            }
        }
    }
}
