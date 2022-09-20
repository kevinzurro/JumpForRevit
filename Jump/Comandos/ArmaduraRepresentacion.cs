using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.Structure;

namespace Jump
{
    public class ArmaduraRepresentacion
    {
        // Variables necesarias
        Document doc;        
        View vistaBarra;
        Rebar barraRefuerzo;
        List<CurveElement> listaCurvas;
        List<TextNote> listaTextos;
        IndependentTag etiquetaArmadura;
        TextNoteType tipoEtiquetaTexto;
        FamilySymbol tipoEtiquetaArmadura;
        XYZ posicion;
        List<Int32> listaCurvasId = new List<int>();
        List<Int32> listaTextosId = new List<int>();

        // Constructor la armadura
        public ArmaduraRepresentacion(Document doc, View vista, Rebar barra)
        {
            // Variable necesarias
            this.doc = doc;            
            this.vistaBarra = vista;
            this.barraRefuerzo = barra;
        }

        /// <summary> Asigna u obtiene las curvas </summary>
        public List<CurveElement> CurvasDeArmadura
        {
            get { return this.listaCurvas; }

            set
            {
                this.listaCurvas = value;

                AgregarListaCurvasId(value);
            }
        }

        /// <summary> Asigna u obtiene el historial de ElementId como entero de curvas creadas </summary>
        public List<Int32> ListaCurvasId
        {
            get { return listaCurvasId; }

            set { this.listaCurvasId = value; }
        }

        /// <summary> Asigna u obtiene los textos de las longitudes parciales </summary>
        public List<TextNote> TextosDeLongitudesParciales
        {
            get { return this.listaTextos; }

            set
            {
                this.listaTextos = value;

                AgregarListaTextosId(value);
            }
        }

        /// <summary> Asigna u obtiene el historial de ElementId como entero de textos creados </summary>
        public List<Int32> ListaTextosId
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

        /// <summary> Obtiene el documento </summary>
        public Document Documento
        {
            get { return this.doc; }
        }

        /// <summary> Asigna u obtiene la posición de la Representación de la armadura en coordenadas globales </summary>
        public XYZ Posicion
        {
            get { return this.posicion; }

            set { this.posicion = value; }
        }

        /// <summary> Obtiene la barra </summary>
        public Rebar Barra
        {
            get { return this.barraRefuerzo; }
        }

        /// <summary> Obtiene la vista de la barra </summary>
        public View Vista
        {
            get { return this.vistaBarra; }
        }

        /// <summary> Agrega la lista de ID de las curvas al historial </summary>
        private void AgregarListaCurvasId(List<CurveElement> lista)
        {
            // Recorre la lista
            foreach (CurveElement curva in lista)
            {
                //Agrega el ID a la lista
                listaCurvasId.Add((Int32)curva.Id.IntegerValue);
            }
        }

        /// <summary> Agrega la lista de ID de los textos al historial </summary>
        private void AgregarListaTextosId(List<TextNote> lista)
        {
            // Recorre la lista
            foreach (TextNote texto in lista)
            {
                //Agrega el ID a la lista
                listaTextosId.Add((Int32)texto.Id.IntegerValue);
            }
        }

        /// <summary> Obtiene los ElementId de las curvas (bool = true) o de los textos (bool = false) </summary>
        public List<ElementId> ObtenerElementosCreadas(bool tipo)
        {
            // Crea la lista a devolver
            List<ElementId> lista = new List<ElementId>();

            // Verifica que sean curvas
            if (tipo)
            {
                // Recorre las curvas creadas
                foreach (Int32 entero in listaCurvasId)
                {
                    // Obtiene el ElementId
                    ElementId elemId = new ElementId(entero);

                    lista.Add(elemId);
                }
            }

            // Verifica que sean textos
            if (tipo)
            {
                // Recorre todos los textos creadps
                foreach (Int32 entero in listaTextosId)
                {
                    // Obtiene el ElementId
                    ElementId elemId = new ElementId(entero);

                    lista.Add(elemId);
                }
            }

            return lista;
        }

        /// <summary> Mueve la Representación de la Armadura con la etiqueta una distancia dada </summary>
        public void MoverArmaduraRepresentacionConEtiqueta(XYZ distancia)
        {
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

            // Crea la lista
            List<Element> lista = new List<Element>();

            // Agrega las curvas a la lista
            lista.AddRange(CurvasDeArmadura);

            // Agrega los textos a la lista
            lista.AddRange(TextosDeLongitudesParciales);

            // Crea el grupo
            Group grupo = Tools.CrearGrupo(Documento, lista);

            // Mueve el grupo
            ElementTransformUtils.MoveElement(Documento, grupo.Id, distancia);

            // Desarma el grupo
            grupo.UngroupMembers();
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

        /// <summary> Obtiene el grupo con los textos y curvas de detalle </summary>
        public Group ObtenerGrupoDeArmadura()
        {
            // Crea la lista
            List<Element> lista = new List<Element>();

            // Agrega las curvas a la lista
            lista.AddRange(CurvasDeArmadura);

            // Agrega los textos a la lista
            lista.AddRange(TextosDeLongitudesParciales);

            // Crea el grupo
            Group grupo = Tools.CrearGrupo(Documento, lista);

            return grupo;
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
        }

        /// <summary> Elimina las curvas de la armadura por el Id del historial </summary>
        private void EliminarCurvasPorId()
        {
            // Obtiene las curvas creadas del historial
            List<ElementId> curvasId = ObtenerElementosCreadas(true);
            
            // Verifica que no sea nulo
            if (curvasId != null)
            {
                // Recorre las curvas
                foreach (ElementId curva in curvasId)
                {
                    try
                    {
                        // Verifica que no sea nulo
                        if (curva != null)
                        {
                            // Elimina la curva
                            doc.Delete(curva);
                        }
                    }
                    catch (Exception) { }
                }
            }
        }

        /// <summary> Elimina los textos de la armadura por el Id del historial </summary>
        private void EliminarTextosPorId()
        {
            // Obtiene los textos creados del historial
            List<ElementId> textosId = ObtenerElementosCreadas(false);

            // Verifica que no sea nulo
            if (textosId != null)
            {
                // Recorre las curvas
                foreach (ElementId texto in textosId)
                {
                    try
                    {
                        // Verifica que no sea nulo
                        if (texto != null)
                        {
                            // Elimina la curva
                            doc.Delete(texto);
                        }
                    }
                    catch (Exception) { }
                }
            }
        }
    }
}
