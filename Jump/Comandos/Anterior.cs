using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.ApplicationServices;

namespace Jump
{
    public static class Anterior
    {
        #region Cotas

        ///<summary> Crea una cota vertical a la izquierda de un elemento en una vista particular </summary>
        //public static Dimension CrearCotaVerticalIzquierdaParaElementoOriginal(Document doc, View vista, Element elem, DimensionType tipoCota)
        //{
        //    // Crea la cota
        //    Dimension cota;

        //    // Crea un arreglo con las referencias
        //    ReferenceArray ArregloRef = new ReferenceArray();

        //    // Agrega las referencias del plano superior e inferior del elemento
        //    ArregloRef.Append(ReferenciaCaraExtremaElementoEnVista(vista, -vista.UpDirection, elem).Reference);
        //    ArregloRef.Append(ReferenciaCaraExtremaElementoEnVista(vista, vista.UpDirection, elem).Reference);

        //    // Crea una caja de sección de la vista
        //    BoundingBoxXYZ bb = vista.CropBox;

        //    // Crea las coordenadas de la linea
        //    XYZ puntoInicial = bb.Transform.OfPoint(bb.Min);
        //    XYZ puntoFinal = puntoInicial.Add(vista.UpDirection);

        //    // Verifica que sea basado en punto
        //    if (elem.Location is LocationPoint)
        //    {
        //        // Crea la linea
        //        Line linea = Line.CreateBound(puntoInicial, puntoFinal);

        //        // Crea la cota
        //        cota = doc.Create.NewDimension(vista, linea, ArregloRef, tipoCota);

        //        // Obtiene la distancia a mover
        //        XYZ distancia = cota.LeaderEndPosition - cota.Origin;

        //        // Mueve la cota hacia la izquierda
        //        ElementTransformUtils.MoveElement(doc, cota.Id, distancia);
        //    }

        //    // Verifica que sea basado en curva
        //    else
        //    {
        //        // Obtiene el FamilyInstance
        //        FamilyInstance fi = elem as FamilyInstance;

        //        // Obtiene la curva de conducción
        //        Curve curva = (elem.Location as LocationCurve).Curve;

        //        // Obtiene la caja de sección de la vista
        //        BoundingBoxXYZ bbox = vista.CropBox;

        //        // Crea una traslación
        //        Transform tra = bbox.Transform;

        //        // Crea el punto inicial
        //        XYZ punto = bbox.Min;

        //        // Asigna las coordenadas de la linea
        //        puntoInicial = tra.OfPoint(punto);
        //        puntoFinal = puntoInicial.Add(vista.UpDirection);

        //        // Crea la linea
        //        Line linea = Line.CreateBound(puntoInicial, puntoFinal);

        //        // Crea la cota
        //        cota = doc.Create.NewDimension(vista, linea, ArregloRef, tipoCota);

        //        // Obtiene la distancia a mover
        //        XYZ distancia = cota.LeaderEndPosition - cota.Origin;

        //        // Mueve la cota hacia la izquierda
        //        ElementTransformUtils.MoveElement(doc, cota.Id, distancia);
        //    }

        //    return cota;
        //}

        ///<summary> Crea una cota vertical a la derecha de un elemento en una vista particular </summary>
        //public static Dimension CrearCotaVerticalDerechaParaElementoOriginal(Document doc, View vista, Element elem, DimensionType tipoCota)
        //{
        //    // Crea la cota
        //    Dimension cota;

        //    // Crea un arreglo con las referencias
        //    ReferenceArray ArregloRef = new ReferenceArray();

        //    // Agrega las referencias del plano superior e inferior del elemento
        //    ArregloRef.Append(ReferenciaCaraExtremaElementoEnVista(vista, -vista.UpDirection, elem).Reference);
        //    ArregloRef.Append(ReferenciaCaraExtremaElementoEnVista(vista, vista.UpDirection, elem).Reference);

        //    // Crea una caja de sección del elemento
        //    BoundingBoxXYZ bb = vista.CropBox;

        //    // Crea las coordenadas de la linea
        //    XYZ puntoInicial = bb.Transform.OfPoint(bb.Max);
        //    XYZ puntoFinal = puntoInicial.Subtract(vista.UpDirection);

        //    // Verifica que sea basado en punto
        //    if (elem.Location is LocationPoint)
        //    {
        //        // Crea la linea
        //        Line linea = Line.CreateBound(puntoInicial, puntoFinal);

        //        // Crea la cota
        //        cota = doc.Create.NewDimension(vista, linea, ArregloRef, tipoCota);

        //        // Obtiene la distancia a mover
        //        XYZ distancia = multiplicadorDistanciaCotasLinealesDerechaIzquierda * (cota.Origin - cota.LeaderEndPosition);

        //        // Mueve la cota hacia la derecha
        //        ElementTransformUtils.MoveElement(doc, cota.Id, distancia);
        //    }

        //    else
        //    {
        //        // Obtiene el FamilyInstance
        //        FamilyInstance fi = elem as FamilyInstance;

        //        // Obtiene la curva de conducción
        //        Curve curva = (elem.Location as LocationCurve).Curve;

        //        // Obtiene la caja de sección de la vista
        //        BoundingBoxXYZ bbox = vista.CropBox;

        //        // Crea una traslación
        //        Transform tra = bbox.Transform;

        //        // Crea el punto inicial
        //        XYZ punto = bbox.Max;

        //        // Asigna las coordenadas de la linea
        //        puntoInicial = tra.OfPoint(punto);
        //        puntoFinal = puntoInicial.Add(vista.UpDirection);

        //        // Crea la linea
        //        Line linea = Line.CreateBound(puntoInicial, puntoFinal);

        //        // Crea la cota
        //        cota = doc.Create.NewDimension(vista, linea, ArregloRef, tipoCota);

        //        // Obtiene la distancia a mover
        //        XYZ distancia = multiplicadorDistanciaCotasLinealesDerechaIzquierda * (cota.Origin - cota.LeaderEndPosition);

        //        // Mueve la cota hacia la izquierda
        //        ElementTransformUtils.MoveElement(doc, cota.Id, distancia);
        //    }

        //    return cota;
        //}

        ///<summary> Crea una cota horizontal arriba de un elemento en una vista particular </summary>
        //public static Dimension CrearCotaHorizontalArribaParaElementoOriginal(Document doc, View vista, Element elem, DimensionType tipoCota)
        //{
        //    // Crea un arreglo con las referencias
        //    ReferenceArray ArregloRef = new ReferenceArray();

        //    // Agrega las referencias del plano
        //    ArregloRef.Append(ReferenciaCaraExtremaElementoEnVista(vista, -vista.RightDirection, elem).Reference);
        //    ArregloRef.Append(ReferenciaCaraExtremaElementoEnVista(vista, vista.RightDirection, elem).Reference);

        //    // Crea una caja de sección del elemento
        //    BoundingBoxXYZ bb = elem.get_BoundingBox(vista);

        //    // Crea las coordenadas de la linea
        //    XYZ puntoInicial = bb.Max;
        //    XYZ puntoFinal = puntoInicial.Subtract(vista.RightDirection);

        //    // Crea la linea
        //    Line linea = Line.CreateBound(puntoInicial, puntoFinal);

        //    // Crea la cota
        //    Dimension cota = doc.Create.NewDimension(vista, linea, ArregloRef, tipoCota);

        //    // Crea una caja de sección del elemento
        //    BoundingBoxXYZ bbCota = cota.get_BoundingBox(vista);

        //    // Obtiene la altura del texto
        //    double zCota = Math.Abs(cota.LeaderEndPosition.Z - cota.Origin.Z);

        //    // Mueve la cota hacia abajo
        //    ElementTransformUtils.MoveElement(doc, cota.Id, new XYZ(0, 0, zCota));

        //    return cota;
        //}

        ///<summary> Crea una cota horizontal abajo de un elemento en una vista particular </summary>
        //public static Dimension CrearCotaHorizontalAbajoParaElementoOriginal(Document doc, View vista, Element elem, DimensionType tipoCota)
        //{
        //    // Crea un arreglo con las referencias
        //    ReferenceArray ArregloRef = new ReferenceArray();

        //    // Agrega las referencias del plano
        //    ArregloRef.Append(ReferenciaCaraExtremaElementoEnVista(vista, -vista.RightDirection, elem).Reference);
        //    ArregloRef.Append(ReferenciaCaraExtremaElementoEnVista(vista, vista.RightDirection, elem).Reference);

        //    // Crea una caja de sección del elemento
        //    BoundingBoxXYZ bb = elem.get_BoundingBox(vista);

        //    // Crea las coordenadas de la linea
        //    XYZ puntoInicial = bb.Min;
        //    XYZ puntoFinal = puntoInicial.Add(vista.RightDirection);

        //    // Crea la linea temporal
        //    Line linea = Line.CreateBound(puntoInicial, puntoFinal);

        //    // Crea la cota temporal
        //    Dimension cota = doc.Create.NewDimension(vista, linea, ArregloRef, tipoCota);

        //    // Crea una caja de sección del elemento
        //    BoundingBoxXYZ bbCota = cota.get_BoundingBox(vista);

        //    // Obtiene la altura del texto
        //    double alturaTexto = Math.Abs(bbCota.Min.Z - cota.LeaderEndPosition.Z) + Math.Abs(cota.TextPosition.Z - cota.LeaderEndPosition.Z);
        //    double zCota = multiplicadorDistanciaCotasLinealesAbajo * alturaTexto;

        //    // Mueve la cota hacia abajo
        //    ElementTransformUtils.MoveElement(doc, cota.Id, new XYZ(0, 0, -zCota));

        //    return cota;
        //}

        ///<summary> Obtiene el vector para mover la etiqueta según la dirección dada </summary>
        //public static XYZ ObtenerVectorParaMoverEtiquetaOriginal(View vista, XYZ direccion, IndependentTag etiqueta, List<Dimension> cotas)
        //{
        //    // Crea el vector
        //    XYZ vector = new XYZ();

        //    // Obtiene la transformada de la vista
        //    Transform tra = vista.CropBox.Transform;

        //    // Obtiene la dirección en coordenadas de la vista
        //    XYZ direccionModificada = tra.Inverse.OfVector(direccion);

        //    // Crea los componentes del vector
        //    double x = 0;
        //    double y = 0;
        //    double z = 0;

        //    // Obtiene el recuadro de la etiqueta
        //    BoundingBoxXYZ bbEtiqueta = etiqueta.get_BoundingBox(vista);

        //    // Obtiene los bordes en coordenadas de la vista
        //    XYZ etiqueta1 = tra.Inverse.OfPoint(bbEtiqueta.Max);
        //    XYZ etiqueta2 = tra.Inverse.OfPoint(bbEtiqueta.Min);

        //    // Obtiene los puntos máximos y mínimos en coordenadas de la vista
        //    XYZ etiquetaMax = ObtenerPuntoMaximo(etiqueta1, etiqueta2);
        //    XYZ etiquetaMin = ObtenerPuntoMinimo(etiqueta1, etiqueta2);

        //    // Recorre todas las cotas
        //    foreach (Dimension cota in cotas)
        //    {
        //        // Verifica que la dirección no sea nula
        //        if (!direccion.IsZeroLength())
        //        {
        //            // Obtiene el recuadro de la cota
        //            BoundingBoxXYZ bbCota = cota.get_BoundingBox(vista);

        //            // Obtiene los bordes en coordenadas de la vista
        //            XYZ cota1 = tra.Inverse.OfPoint(bbCota.Max);
        //            XYZ cota2 = tra.Inverse.OfPoint(bbCota.Min);

        //            // Obtiene los puntos máximos y mínimos en coordenadas de la vista
        //            XYZ cotaMax = ObtenerPuntoMaximo(cota1, cota2);
        //            XYZ cotaMin = ObtenerPuntoMinimo(cota1, cota2);

        //            // Obtiene los componentes del vector
        //            double xCota = ComponenteDeVectorParaEtiqueta(direccionModificada.X, etiquetaMax.X, etiquetaMin.X, cotaMax.X, cotaMin.X);
        //            double yCota = ComponenteDeVectorParaEtiqueta(direccionModificada.Y, etiquetaMax.Y, etiquetaMin.Y, cotaMax.Y, cotaMin.Y);
        //            double zCota = ComponenteDeVectorParaEtiqueta(direccionModificada.Z, etiquetaMax.Z, etiquetaMin.Z, cotaMax.Z, cotaMin.Z);

        //            // Verifica si los componentes van en la misma dirección 
        //            x = ComponenteDeVectorMayorOMenorQuePunto(direccionModificada.X, xCota, x);
        //            y = ComponenteDeVectorMayorOMenorQuePunto(direccionModificada.Y, yCota, y);
        //            z = ComponenteDeVectorMayorOMenorQuePunto(direccionModificada.Z, zCota, z);
        //        }
        //    }

        //    // Transforma el vector de las coordenadas de la vista a coordenadas globales
        //    vector = tra.OfVector(new XYZ(x, y, z));

        //    return vector;
        //}

        #endregion

        #region Cota de profundidad

        ///<summary> Crea una cota de profundidad abajo a la izquierda </summary>
        //public static SpotDimension CrearCotaProfundidadAbajoIzquierdaOriginal(Document doc, View vista, Element elem, SpotDimensionType tipoCotaProfundidad)
        //{
        //    // Obtiene la referencia del elemento
        //    Reference referencia = ReferenciaCaraExtremaElementoEnVista(vista, -vista.UpDirection, elem).Reference;

        //    // Crea la bandera de la geometría
        //    bool banderaGeometria = false;

        //    // Obtiene el punto medio de la cara referenciada
        //    XYZ puntoMedioCara = ObtenerPuntoMedioCaraParaCotaProfundidad(doc, elem, referencia, ref banderaGeometria);

        //    // Crea la caja que contiene al elemento
        //    BoundingBoxXYZ bb = elem.get_BoundingBox(null);

        //    // Coordenadas del origen de la etiqueta
        //    XYZ puntoOrigen = puntoMedioCara;

        //    // Obtiene el cuadro de la vista
        //    BoundingBoxXYZ bbox = vista.get_BoundingBox(null);

        //    // Obtiene la transformación de la vista
        //    Transform tra = bbox.Transform;

        //    // Transforma las coordenadas máximas de la vista a coordenadas del proyecto
        //    XYZ vistaMin = tra.OfPoint(bbox.Min);

        //    // Coordenadas del pliegue de la etiqueta abajo a la derecha
        //    XYZ puntoPliegueTemp = new XYZ(vistaMin.X, vistaMin.Y, puntoMedioCara.Z);

        //    // Coordenadas después del pliegue
        //    XYZ puntoFinTemp = new XYZ(vistaMin.X, vistaMin.Y, puntoMedioCara.Z);

        //    // Coordenadas finales del pliegue
        //    XYZ puntoPliegue = new XYZ();

        //    // Coordenadas finales después del pliegue
        //    XYZ puntoFin = new XYZ();

        //    // Abre una subtransacción
        //    using (SubTransaction subT = new SubTransaction(doc))
        //    {
        //        subT.Start();

        //        // Crea la etiqueta temporal
        //        SpotDimension etiquetaTemporal = doc.Create.NewSpotElevation
        //                                            (vista, referencia, puntoOrigen, puntoPliegueTemp, puntoFinTemp, puntoOrigen, false);

        //        // Crea la caja que contiene a la etiqueta temporal
        //        BoundingBoxXYZ bbEtiqueta = etiquetaTemporal.get_BoundingBox(vista);

        //        // Obtiene el volumen tridimensional de la etiqueta temporal
        //        double xEtiMedio = (bbEtiqueta.Max.X - bbEtiqueta.Min.X);
        //        double yEtiMedio = (bbEtiqueta.Max.Y - bbEtiqueta.Min.Y);

        //        // Obtiene el punto del pliegue
        //        double xPliegue = puntoPliegueTemp.X - xEtiMedio * ObtenerSignoComponenteDeVector(vista.RightDirection.X);
        //        double yPliegue = puntoPliegueTemp.Y - yEtiMedio * ObtenerSignoComponenteDeVector(vista.RightDirection.Y);

        //        // Reemplaza las nuevas coordenadas del pliegue
        //        puntoPliegue = new XYZ(xPliegue, yPliegue, puntoPliegueTemp.Z);

        //        // Obtiene el punto final
        //        double xFin = puntoFinTemp.X - xEtiMedio * ObtenerSignoComponenteDeVector(vista.RightDirection.X);
        //        double yFin = puntoFinTemp.Y - yEtiMedio * ObtenerSignoComponenteDeVector(vista.RightDirection.Y);

        //        // Reemplaza las nuevas coordenadas después del pliegue
        //        puntoFin = new XYZ(xFin, yFin, puntoFinTemp.Z);

        //        subT.RollBack();
        //    }

        //    // Crea la etiqueta
        //    SpotDimension etiquetaProfundidad = doc.Create.NewSpotElevation
        //                                        (vista, referencia, puntoOrigen, puntoPliegue, puntoFin, puntoOrigen, true);

        //    // Cambia el estilo de etiqueta
        //    etiquetaProfundidad.DimensionType = tipoCotaProfundidad;

        //    return etiquetaProfundidad;
        //}

        ///<summary> Crea una cota de profundidad abajo a la derecha </summary>
        //public static SpotDimension CrearCotaProfundidadAbajoDerechaOriginal(Document doc, View vista, Element elem, SpotDimensionType tipoCotaProfundidad)
        //{
        //    // Obtiene la referencia del elemento
        //    Reference referencia = ReferenciaCaraExtremaElementoEnVista(vista, -vista.UpDirection, elem).Reference;

        //    // Crea la bandera de la geometría
        //    bool banderaGeometria = false;

        //    // Obtiene el punto medio de la cara referenciada
        //    XYZ puntoMedioCara = ObtenerPuntoMedioCaraParaCotaProfundidad(doc, elem, referencia, ref banderaGeometria);

        //    // Coordenadas del origen de la etiqueta
        //    XYZ puntoOrigen = puntoMedioCara;

        //    // Obtiene el cuadro de la vista
        //    BoundingBoxXYZ bbox = vista.get_BoundingBox(null);

        //    // Obtiene la transformación de la vista
        //    Transform tra = bbox.Transform;

        //    // Transforma las coordenadas máximas de la vista a coordenadas del proyecto
        //    XYZ vistaMax = tra.OfPoint(bbox.Max);

        //    // Coordenadas del pliegue de la etiqueta abajo a la derecha
        //    XYZ puntoPliegueTemp = new XYZ(vistaMax.X, vistaMax.Y, puntoMedioCara.Z);

        //    // Coordenadas después del pliegue
        //    XYZ puntoFinTemp = new XYZ(vistaMax.X, vistaMax.Y, puntoMedioCara.Z);

        //    // Coordenadas finales del pliegue
        //    XYZ puntoPliegue = new XYZ();

        //    // Coordenadas finales después del pliegue
        //    XYZ puntoFin = new XYZ();

        //    // Abre una subtransacción
        //    using (SubTransaction subT = new SubTransaction(doc))
        //    {
        //        subT.Start();

        //        // Crea la etiqueta temporal
        //        SpotDimension etiquetaTemporal = doc.Create.NewSpotElevation
        //                                            (vista, referencia, puntoOrigen, puntoPliegueTemp, puntoFinTemp, puntoOrigen, false);

        //        // Crea la caja que contiene a la etiqueta temporal
        //        BoundingBoxXYZ bbEtiqueta = etiquetaTemporal.get_BoundingBox(vista);

        //        // Obtiene el volumen tridimensional de la etiqueta temporal
        //        double xEtiMedio = (bbEtiqueta.Max.X - bbEtiqueta.Min.X) / 4;
        //        double yEtiMedio = (bbEtiqueta.Max.Y - bbEtiqueta.Min.Y) / 4;

        //        // Obtiene el punto del pliegue
        //        double xPliegue = puntoPliegueTemp.X + xEtiMedio * ObtenerSignoComponenteDeVector(vista.RightDirection.X);
        //        double yPliegue = puntoPliegueTemp.Y + yEtiMedio * ObtenerSignoComponenteDeVector(vista.RightDirection.Y);

        //        // Reemplaza las nuevas coordenadas del pliegue
        //        puntoPliegue = new XYZ(xPliegue, yPliegue, puntoPliegueTemp.Z);

        //        // Obtiene el punto final
        //        double xFin = puntoFinTemp.X + xEtiMedio * ObtenerSignoComponenteDeVector(vista.RightDirection.X);
        //        double yFin = puntoFinTemp.Y + yEtiMedio * ObtenerSignoComponenteDeVector(vista.RightDirection.Y);

        //        // Reemplaza las nuevas coordenadas después del pliegue
        //        puntoFin = new XYZ(xFin, yFin, puntoFinTemp.Z);

        //        subT.RollBack();
        //    }

        //    // Crea la etiqueta
        //    SpotDimension etiquetaProfundidad = doc.Create.NewSpotElevation
        //                                        (vista, referencia, puntoOrigen, puntoPliegue, puntoFin, puntoOrigen, true);

        //    // Cambia el estilo de etiqueta
        //    etiquetaProfundidad.DimensionType = tipoCotaProfundidad;

        //    return etiquetaProfundidad;
        //}

        #endregion

        #region Ordenar y mover Representación de armaduras

        ///<summary> Ordena y mueve las Represetaciones de Armaduras según las opciones </summary>
        //public void OrdenarYMoverRepresentacionArmaduraSegunDireccion(Document doc, Autodesk.Revit.DB.View vista, Element elem, List<ArmaduraRepresentacion> armaduras)
        //{
        //    // Crea las listas
        //    List<ArmaduraRepresentacion> listaArmadurasArriba = new List<ArmaduraRepresentacion>();
        //    List<ArmaduraRepresentacion> listaArmadurasAbajo = new List<ArmaduraRepresentacion>();
        //    List<ArmaduraRepresentacion> listaArmadurasIzquierda = new List<ArmaduraRepresentacion>();
        //    List<ArmaduraRepresentacion> listaArmadurasDerecha = new List<ArmaduraRepresentacion>();

        //    // Crea una transformada de la vista
        //    Transform tra = vista.CropBox.Transform;

        //    // Obtiene el recuadro del elemento
        //    BoundingBoxXYZ bbElem = Tools.ObtenerRecuadroElementoParaleloAVista(doc, vista, elem);

        //    // Obtiene el baricentro del recuadro del elemento
        //    XYZ puntoMedioElem = Tools.ObtenerBaricentroDeRecuadro(bbElem);

        //    // Recorre la lista de Representación de Armaduras
        //    foreach (ArmaduraRepresentacion bar in armaduras)
        //    {
        //        try
        //        {
        //            // Obtiene el recuadro de la barra
        //            BoundingBoxXYZ bbArmadura = Tools.ObtenerRecuadroElementoParaleloAVista(doc, vista, bar.Barra);

        //            // Obtiene el baricentro del recuadro de la barra
        //            XYZ puntoMedioArmadura = Tools.ObtenerBaricentroDeRecuadro(bbArmadura);

        //            // Obtiene la distancia en coordenadas de la vista
        //            XYZ distanciaRelativa = tra.Inverse.OfVector(puntoMedioArmadura - puntoMedioElem);

        //            OrganizarListaSegunDireccionDeBarra(vista, distanciaRelativa, bar,
        //                                                ref listaArmadurasArriba, ref listaArmadurasAbajo,
        //                                                ref listaArmadurasIzquierda, ref listaArmadurasDerecha);
        //        }
        //        catch (Exception) { }
        //    }

        //    OrdenarYMoverListaConArmadurasRepresentacion(doc, vista, tra, elem,
        //                                                 ref listaArmadurasArriba, ref listaArmadurasAbajo,
        //                                                 ref listaArmadurasIzquierda, ref listaArmadurasDerecha);
        //}

        ///<summary> Organiza una Representación de Armadura según una dirección </summary>
        //public void OrganizarListaSegunDireccionDeBarra(Autodesk.Revit.DB.View vista, XYZ distanciaRelativa, ArmaduraRepresentacion bar,
        //                                                       ref List<ArmaduraRepresentacion> listaArmadurasArriba,
        //                                                       ref List<ArmaduraRepresentacion> listaArmadurasAbajo,
        //                                                       ref List<ArmaduraRepresentacion> listaArmadurasIzquierda,
        //                                                       ref List<ArmaduraRepresentacion> listaArmadurasDerecha)
        //{
        //    // Obtiene la transformada inversa de la vista
        //    Transform traInv = vista.CropBox.Transform.Inverse;

        //    // Verifica si la distancia es cero
        //    if (distanciaRelativa.IsZeroLength())
        //    {
        //        // Proyecta y asigna la posición de la armadura en coordenadas relativas
        //        bar.Posicion = traInv.Inverse.OfVector(Tools.ProyectarVectorSobreDireccion(distanciaRelativa, traInv.OfVector(vista.RightDirection)));

        //        // Agrega la armadura a la lista
        //        listaArmadurasDerecha.Add(bar);
        //    }

        //    // Verifica si X es mayor a Y
        //    else if (Math.Abs(distanciaRelativa.X) >= Math.Abs(distanciaRelativa.Y))
        //    {
        //        // Verifica si X es positivo
        //        if (Tools.ObtenerSignoComponenteDeVector(distanciaRelativa.X) == 1)
        //        {
        //            // Proyecta y asigna la posición de la armadura
        //            bar.Posicion = traInv.Inverse.OfVector(Tools.ProyectarVectorSobreDireccion(distanciaRelativa, traInv.OfVector(vista.RightDirection)));

        //            // Agrega la armadura a la lista
        //            listaArmadurasDerecha.Add(bar);
        //        }

        //        else
        //        {
        //            // Proyecta y asigna la posición de la armadura
        //            bar.Posicion = traInv.Inverse.OfVector(Tools.ProyectarVectorSobreDireccion(distanciaRelativa, traInv.OfVector(vista.RightDirection.Negate())));

        //            // Agrega la armadura a la lista
        //            listaArmadurasIzquierda.Add(bar);
        //        }
        //    }

        //    // Y es mayor a X
        //    else
        //    {
        //        // Verifica si Y es positivo
        //        if (Tools.ObtenerSignoComponenteDeVector(distanciaRelativa.Y) == 1)
        //        {
        //            // Proyecta y asigna la posición de la armadura
        //            bar.Posicion = traInv.Inverse.OfVector(Tools.ProyectarVectorSobreDireccion(distanciaRelativa, traInv.OfVector(vista.UpDirection)));

        //            // Agrega la armadura a la lista
        //            listaArmadurasArriba.Add(bar);
        //        }

        //        else
        //        {
        //            // Proyecta y asigna la posición de la armadura
        //            bar.Posicion = traInv.Inverse.OfVector(Tools.ProyectarVectorSobreDireccion(distanciaRelativa, traInv.OfVector(vista.UpDirection.Negate())));

        //            // Agrega la armadura a la lista
        //            listaArmadurasAbajo.Add(bar);
        //        }
        //    }
        //}

        ///<summary> Ordena y mueve las listas de Representación de Armadura </summary>        
        //public void OrdenarYMoverListaConArmadurasRepresentacion(Document doc, Autodesk.Revit.DB.View vista, Transform tra, Element elem,
        //                                                                ref List<ArmaduraRepresentacion> listaArmadurasArriba,
        //                                                                ref List<ArmaduraRepresentacion> listaArmadurasAbajo,
        //                                                                ref List<ArmaduraRepresentacion> listaArmadurasIzquierda,
        //                                                                ref List<ArmaduraRepresentacion> listaArmadurasDerecha)
        //{
        //    // Verifica que existan elementos
        //    if (listaArmadurasArriba.Count > 0)
        //    {
        //        // Ordena la lista
        //        listaArmadurasArriba = listaArmadurasArriba.OrderBy(x => tra.Inverse.OfVector(x.Posicion).Y).ToList();

        //        // Mueve los elementos de la lista
        //        MoverListaConArmaduras(doc, vista, elem, vista.UpDirection, listaArmadurasArriba);
        //    }

        //    // Verifica que existan elementos
        //    if (listaArmadurasAbajo.Count > 0)
        //    {
        //        // Ordena la lista
        //        listaArmadurasAbajo = listaArmadurasAbajo.OrderByDescending(x => tra.Inverse.OfPoint(x.Posicion).Y).ToList();

        //        // Mueve los elementos de la lista
        //        MoverListaConArmaduras(doc, vista, elem, vista.UpDirection.Negate(), listaArmadurasAbajo);
        //    }

        //    // Verifica que existan elementos
        //    if (listaArmadurasDerecha.Count > 0)
        //    {
        //        // Ordena la lista
        //        listaArmadurasDerecha = listaArmadurasDerecha.OrderBy(x => tra.Inverse.OfVector(x.Posicion).X).ToList();

        //        // Mueve los elementos de la lista
        //        MoverListaConArmaduras(doc, vista, elem, vista.RightDirection, listaArmadurasDerecha);
        //    }

        //    // Verifica que existan elementos
        //    if (listaArmadurasIzquierda.Count > 0)
        //    {
        //        // Ordena la lista
        //        listaArmadurasIzquierda = listaArmadurasIzquierda.OrderByDescending(x => tra.Inverse.OfVector(x.Posicion).X).ToList();

        //        // Mueve los elementos de la lista
        //        MoverListaConArmaduras(doc, vista, elem, vista.RightDirection.Negate(), listaArmadurasIzquierda);
        //    }
        //}

        ///<summary> Mueve la lista de Representacion de Armaduras según una dirección </summary>
        //public void MoverListaConArmaduras(Document doc, Autodesk.Revit.DB.View vista, Element elem, XYZ direccion, List<ArmaduraRepresentacion> armaduras)
        //{
        //    // Crea las banderas de las direcciones
        //    bool banderaArriba = true;
        //    bool banderaAbajo = true;
        //    bool banderaDerecha = true;
        //    bool banderaIzquierda = true;

        //    // Crea una transformada de la vista
        //    Transform tra = vista.CropBox.Transform;

        //    // Recuadro del elemento
        //    BoundingBoxXYZ bbElem = Tools.ObtenerRecuadroElementoParaleloAVista(doc, vista, elem);

        //    // Distancia a mover
        //    XYZ distancia = new XYZ();

        //    // Dimensiones del elemento en coordenadas relativas
        //    XYZ elementoDimensiones = tra.Inverse.OfVector((bbElem.Max - bbElem.Min) / 2);
        //    XYZ elementoAncho = new XYZ(Math.Abs(elementoDimensiones.X), 0, 0);
        //    XYZ elementoAlto = new XYZ(0, Math.Abs(elementoDimensiones.Y), 0);

        //    foreach (ArmaduraRepresentacion bar in armaduras)
        //    {
        //        try
        //        {
        //            bar.DibujarArmaduraSegunDatagridview(this.dgvEstiloLinea);

        //            // Recuadro de la barra
        //            BoundingBoxXYZ bbBar = bar.ObtenerBoundingBoxDeArmadura();

        //            // Dimensiones de la Representación de Armadura en coordenadas relativas
        //            XYZ barDimensiones = tra.Inverse.OfVector(bbBar.Max - bbBar.Min);
        //            XYZ barAncho = new XYZ(Math.Abs(barDimensiones.X), 0, 0);
        //            XYZ barAlto = new XYZ(0, Math.Abs(barDimensiones.Y), 0);

        //            // Verifica si la dirección es arriba
        //            if (direccion.IsAlmostEqualTo(vista.UpDirection))
        //            {
        //                // Verifica si el la primera pasada
        //                if (banderaArriba)
        //                {
        //                    distancia = tra.Inverse.OfVector(Tools.ProyectarVectorSobreDireccion((bbElem.Max - bbBar.Min), vista.UpDirection)) + barAlto / 2;

        //                    // Cambia el estado de la bandera
        //                    banderaArriba = false;
        //                }
        //                else
        //                {
        //                    distancia += barAlto ;
        //                }
        //            }

        //            // Verifica si la dirección es abajo
        //            else if (direccion.IsAlmostEqualTo(vista.UpDirection.Negate()))
        //            {
        //                // Verifica si el la primera pasada
        //                if (banderaAbajo)
        //                {
        //                    // Obtiene la distancia a mover
        //                    distancia = tra.Inverse.OfVector(Tools.ProyectarVectorSobreDireccion((bbElem.Min - bbBar.Max), vista.UpDirection.Negate())) - barAlto / 2;

        //                    // Cambia el estado de la bandera
        //                    banderaAbajo = false;
        //                }
        //                else
        //                {
        //                    distancia -= barAlto;
        //                }
        //            }

        //            // Verifica si la dirección es derecha
        //            else if (direccion.IsAlmostEqualTo(vista.RightDirection))
        //            {
        //                // Verifica si el la primera pasada
        //                if (banderaDerecha)
        //                {
        //                    distancia = tra.Inverse.OfVector(Tools.ProyectarVectorSobreDireccion((bbElem.Max - bbBar.Min), vista.RightDirection)) + barAncho;

        //                    // Cambia el estado de la bandera
        //                    banderaDerecha = false;
        //                }
        //                else
        //                {
        //                    distancia += barAncho / 2;
        //                }
        //            }

        //            // Verifica si la dirección es izquierda
        //            else if (direccion.IsAlmostEqualTo(vista.RightDirection.Negate()))
        //            {
        //                // Verifica si el la primera pasada
        //                if (banderaIzquierda)
        //                {
        //                    distancia = tra.Inverse.OfVector(Tools.ProyectarVectorSobreDireccion((bbElem.Min - bbBar.Max), vista.RightDirection.Negate())) - barAncho;

        //                    // Cambia el estado de la bandera
        //                    banderaIzquierda = false;
        //                }
        //                else
        //                {
        //                    distancia -= barAncho / 2;
        //                }
        //            }

        //            else
        //            {
        //                // Verifica si el la primera pasada
        //                if (banderaDerecha)
        //                {
        //                    distancia = tra.Inverse.OfVector(Tools.ProyectarVectorSobreDireccion((bbElem.Max - bbBar.Min), vista.RightDirection)) + barAncho;

        //                    // Cambia el estado de la bandera
        //                    banderaDerecha = false;
        //                }
        //                else
        //                {
        //                    distancia += barAncho / 2;
        //                }
        //            }

        //            // Lo lleva a coordenadas globales
        //            bar.Posicion = tra.OfVector(distancia);

        //            bar.MoverArmaduraRepresentacionConEtiqueta(bar.Posicion);
        //        }
        //        catch (Exception) { }
        //    }
        //}

        #endregion

        #region Importar y exportar desde Excel

        /// <summary> Carga los diámetros y estilos de lineas de un archivo CSV </summary>
        //public static void CargarDiametrosYEstilos(System.Windows.Forms.DataGridView dgv,
        //                                           System.Windows.Forms.DataGridViewComboBoxColumn dgvCombo,
        //                                           Document doc, string rutaArchivo)
        //{
        //    using (SLDocument slDoc = new SLDocument(rutaArchivo))
        //    {
        //        // Crea un DataGridViewComboboxColumn
        //        System.Windows.Forms.DataGridViewComboBoxColumn dgvCombobox = new System.Windows.Forms.DataGridViewComboBoxColumn();

        //        // Crea los contadores
        //        int i = 0;
        //        int j = 0;

        //        // Recorre las filas
        //        while (!string.IsNullOrEmpty(slDoc.GetCellValueAsString(i + 1, j + 1)))
        //        {
        //            try
        //            {
        //                // Verifica que la celda 
        //                if (dgv.Rows[i].Cells[j].Value.ToString() == slDoc.GetCellValueAsString(i + 1, j + 1))
        //                {
        //                    // Limpia el DataGridViewComboboxColumn
        //                    dgvCombobox.Items.Clear();

        //                    // Asigna el valor del excel
        //                    dgvCombobox.Items.Add(slDoc.GetCellValueAsString(i + 1, j + 2));

        //                    // Asigna el valor al DataGridViewComboboxColumn
        //                    dgv.Rows[i].Cells[AboutJump.nombreColumnaEstilosLineas].Value = dgvCombobox.Items[0];
        //                }

        //                // Aumenta el contador
        //                i++;
        //            }
        //            catch (Exception) { }
        //        }
        //    }
        //}

        /// <summary> Guarda los diámetros y estilos de lineas en un archivo externo </summary>
        //public static void GuardarDiametrosYEstilos(System.Windows.Forms.DataGridView dgv, string rutaArchivo)
        //{
        //    using (SLDocument slDoc = new SLDocument())
        //    {
        //        // Crea una DataTable
        //        DataTable dt = new DataTable();

        //        // Recorre las columnas
        //        for (int i = 0; i < dgv.Columns.Count; i++)
        //        {
        //            // Agrega la columna
        //            dt.Columns.Add(dgv.Columns[i].Name);
        //        }

        //        // Recorre las filas
        //        for (int i = 0; i < dgv.Rows.Count; i++)
        //        {
        //            // Asigna el valor del DataGridView a la matriz
        //            dt.Rows.Add(dgv.Rows[i].Cells[AboutJump.nombreColumnaDiametros].Value.ToString(), dgv.Rows[i].Cells[AboutJump.nombreColumnaEstilosLineas].Value.ToString());
        //        }

        //        // Asigna la matriz al excel
        //        slDoc.ImportDataTable(celdaExcelInsertarDiametros, dt, celdaExcelEncabezadoDiametros);

        //        // Cambia el nombre de la pestaña
        //        slDoc.RenameWorksheet(SLDocument.DefaultFirstSheetName, pestanaExcelDiametros);

        //        // Ajusta el ancho de las columnas
        //        for (int i = 1; i <= dgv.Columns.Count; i++)
        //        {
        //            slDoc.AutoFitColumn(i);
        //        }

        //        // Guarda el archivo de excel
        //        slDoc.SaveAs(rutaArchivo);
        //    }
        //}

        #endregion
    }
}
