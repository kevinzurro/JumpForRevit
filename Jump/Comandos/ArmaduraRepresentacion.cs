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

        /// <summary> Obtiene el punto medio de la Representación en coordenadas globales </summary>
        public XYZ PuntoMedio
        {
            get
            {
                return Tools.ObtenerBaricentroElemento(this.Barra.get_BoundingBox(this.Vista));
            }
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

        /// <summary> Dibuja la armadura junto con su textos </summary>
        public void DibujarArmaduraSegunDatagridview(System.Windows.Forms.DataGridView dgw)
        {
            // Dibuja las armaduras y asigna los estilos de líneas en función de cada diámetro
            this.CurvasDeArmadura = Tools.DibujarArmaduraSegunDatagridview(dgw, this.doc, this.vistaBarra, this.barraRefuerzo);

            // Crea notas de texto con la longitud parcial de la barra
            this.TextosDeLongitudesParciales = Tools.CrearTextNoteDeArmadura(this.doc, this.vistaBarra, this.barraRefuerzo, this.TipoDeTexto);
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

        /// <summary> Obtine la Representación de la armadura de la entidad de la barra </summary>
        public List<ArmaduraRepresentacion> ObtenerRepresentacionArmaduraDeEntidad()
        {
            List<ArmaduraRepresentacion> armaduras = new List<ArmaduraRepresentacion>();

            Schema esquema = AboutJump.Esquema(this.Documento);

            Entity entidad = this.Barra.GetEntity(esquema);

            if (entidad.IsValid() && entidad != null)
            {
                armaduras = entidad.Get<IList<ArmaduraRepresentacion>>(esquema.GetField(AboutJump.AlmacenamientoArmaduraRepresentacion)).ToList();
            }
            
            return armaduras;
        }

        /// <summary> Asigna la Representación de la armadura a la entidad de la barra </summary>
        public void AsignarRepresentacionArmdauraAEntidad(ArmaduraRepresentacion armaduraNueva)
        {
            Schema esquema = AboutJump.Esquema(this.Documento);

            Entity entidad = this.Barra.GetEntity(esquema);

            List<ArmaduraRepresentacion> armadurasEnEntidad = ObtenerRepresentacionArmaduraDeEntidad();

            armadurasEnEntidad.Add(armaduraNueva);

            entidad.Set<IList<ArmaduraRepresentacion>>(esquema.GetField(AboutJump.AlmacenamientoArmaduraRepresentacion), armadurasEnEntidad);

            this.Barra.SetEntity(entidad);
        }

        /// <summary> Asigna las Representaciones de la armaduras a la entidad de la barra </summary>
        public void AsignarRepresentacionArmdauraAEntidad(List<ArmaduraRepresentacion> armadurasNuevas)
        {
            Schema esquema = AboutJump.Esquema(this.Documento);

            Entity entidad = this.Barra.GetEntity(esquema);

            List<ArmaduraRepresentacion> armadurasEnEntidad = ObtenerRepresentacionArmaduraDeEntidad();

            armadurasEnEntidad.AddRange(armadurasNuevas);

            entidad.Set<IList<ArmaduraRepresentacion>>(esquema.GetField(AboutJump.AlmacenamientoArmaduraRepresentacion), armadurasEnEntidad);

            this.Barra.SetEntity(entidad);
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
