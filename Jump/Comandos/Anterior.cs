using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Jump
{
    internal class Anterior
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

        /////<summary> Crea una cota horizontal abajo de un elemento en una vista particular </summary>
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

        #region Etiquetas

        ///<summary> Crea una etiqueta horizontal en la parte superior izquierda sin guía en una vista particular </summary>
        //public static IndependentTag CrearEtiquetaIndependienteArribaIzquierdaOriginal(Document doc, View vista, Element elem, FamilySymbol tipoEtiqueta)
        //{
        //    // Obtiene la referencia del elemento
        //    Reference referencia = new Reference(elem);

        //    // Crea la caja que contiene al elemento
        //    BoundingBoxXYZ bb = ObtenerRecuadroElementoParaleloAVista(doc, vista, elem);

        //    // Obtiene la ubicación del punto superior izquierdo
        //    double x = bb.Min.X;
        //    double y = bb.Min.Y;
        //    double z = bb.Max.Z;

        //    // Asigna donde se colocará la etiqueta
        //    XYZ punto = new XYZ(x, y, z);

        //    // Crea la etiqueta
        //    IndependentTag etiqueta = IndependentTag.Create(doc, tipoEtiqueta.Id, vista.Id, referencia, false, TagOrientation.Horizontal, punto);

        //    // Obtiene la caja que contiene a la etiqueta
        //    BoundingBoxXYZ bbetiqueta = etiqueta.get_BoundingBox(vista);

        //    // Obtiene mitad de la altura de la etiqueta
        //    double altura = (bbetiqueta.Max.Z - bbetiqueta.Min.Z) / 2;

        //    // Asigna la nueva ubicación a la etiqueta
        //    XYZ puntoFinal = new XYZ(x, y, (z + altura));

        //    // Coloca la etiqueta correctamente
        //    etiqueta.TagHeadPosition = puntoFinal;

        //    return etiqueta;
        //}

        ///<summary> Crea una etiqueta horizontal en la parte superior central sin guía en una vista particular </summary>
        //public static IndependentTag CrearEtiquetaIndependienteArribaMedioOriginal(Document doc, View vista, Element elem, FamilySymbol tipoEtiqueta)
        //{
        //    // Obtiene la referencia del elemento
        //    Reference referencia = new Reference(elem);

        //    // Crea la caja que contiene al elemento
        //    BoundingBoxXYZ bb = ObtenerRecuadroElementoParaleloAVista(doc, vista, elem);

        //    // Obtiene el volumen tridimensional del elemento
        //    double xMedio = (bb.Max.X - bb.Min.X) / 2;
        //    double yMedio = (bb.Max.Y - bb.Min.Y) / 2;

        //    // Obtiene la ubicación del punto medio superior
        //    double x = (bb.Min.X + xMedio);
        //    double y = (bb.Min.Y + yMedio);
        //    double z = bb.Max.Z;

        //    // Asigna donde se colocará la etiqueta
        //    XYZ punto = new XYZ(x, y, z);

        //    // Crea la etiqueta
        //    IndependentTag etiqueta = IndependentTag.Create(doc, tipoEtiqueta.Id, vista.Id, referencia, false, TagOrientation.Horizontal, punto);

        //    // Obtiene la caja que contiene a la etiqueta
        //    BoundingBoxXYZ bbetiqueta = etiqueta.get_BoundingBox(vista);

        //    // Obtiene mitad de la altura de la etiqueta
        //    double altura = (bbetiqueta.Max.Z - bbetiqueta.Min.Z) / 2;

        //    // Asigna la nueva ubicación a la etiqueta
        //    XYZ puntoFinal = new XYZ(x, y, (z + altura));

        //    // Coloca la etiqueta correctamente
        //    etiqueta.TagHeadPosition = puntoFinal;

        //    return etiqueta;
        //}

        ///<summary> Crea una etiqueta horizontal en la parte superior derecha sin guía en una vista particular </summary>
        //public static IndependentTag CrearEtiquetaIndependienteArribaDerechaOriginal(Document doc, View vista, Element elem, FamilySymbol tipoEtiqueta)
        //{
        //    // Obtiene la referencia del elemento
        //    Reference referencia = new Reference(elem);

        //    // Crea la caja que contiene al elemento
        //    BoundingBoxXYZ bb = ObtenerRecuadroElementoParaleloAVista(doc, vista, elem);

        //    // Obtiene la ubicación del punto superior derecho
        //    double x = bb.Max.X;
        //    double y = bb.Max.Y;
        //    double z = bb.Max.Z;

        //    // Asigna donde se colocará la etiqueta
        //    XYZ punto = new XYZ(x, y, z);

        //    // Crea la etiqueta
        //    IndependentTag etiqueta = IndependentTag.Create(doc, tipoEtiqueta.Id, vista.Id, referencia, false, TagOrientation.Horizontal, punto);

        //    // Obtiene la caja que contiene a la etiqueta
        //    BoundingBoxXYZ bbetiqueta = etiqueta.get_BoundingBox(vista);

        //    // Obtiene mitad de la altura de la etiqueta
        //    double altura = (bbetiqueta.Max.Z - bbetiqueta.Min.Z) / 2;

        //    // Asigna la nueva ubicación a la etiqueta
        //    XYZ puntoFinal = new XYZ(x, y, (z + altura));

        //    // Coloca la etiqueta correctamente
        //    etiqueta.TagHeadPosition = puntoFinal;

        //    return etiqueta;
        //}

        ///<summary> Crea una etiqueta horizontal en la parte media izquierda sin guía en una vista particular </summary>
        //public static IndependentTag CrearEtiquetaIndependienteCentroIzquierdaOriginal(Document doc, View vista, Element elem, FamilySymbol tipoEtiqueta)
        //{
        //    // Obtiene la referencia del elemento
        //    Reference referencia = new Reference(elem);

        //    // Crea la caja que contiene al elemento
        //    BoundingBoxXYZ bb = ObtenerRecuadroElementoParaleloAVista(doc, vista, elem);

        //    // Obtiene el volumen tridimensional del elemento
        //    double zMedio = (bb.Max.Z - bb.Min.Z) / 2;

        //    // Obtiene la ubicación del punto central izquierdo
        //    double x = bb.Min.X;
        //    double y = bb.Min.Y;
        //    double z = (bb.Min.Z + zMedio);

        //    // Asigna donde se colocará la etiqueta
        //    XYZ punto = new XYZ(x, y, z);

        //    // Crea la etiqueta
        //    IndependentTag etiqueta = IndependentTag.Create(doc, tipoEtiqueta.Id, vista.Id, referencia, false, TagOrientation.Horizontal, punto);

        //    return etiqueta;
        //}

        ///<summary> Crea una etiqueta horizontal en la parte media central sin guía en una vista particular </summary>
        //public static IndependentTag CrearEtiquetaIndependienteCentroMedioOriginal(Document doc, View vista, Element elem, FamilySymbol tipoEtiqueta)
        //{
        //    // Obtiene la referencia del elemento
        //    Reference referencia = new Reference(elem);

        //    // Crea la caja que contiene al elemento
        //    BoundingBoxXYZ bb = ObtenerRecuadroElementoParaleloAVista(doc, vista, elem);

        //    // Obtiene el volumen tridimensional del elemento
        //    double xMedio = (bb.Max.X - bb.Min.X) / 2;
        //    double yMedio = (bb.Max.Y - bb.Min.Y) / 2;
        //    double zMedio = (bb.Max.Z - bb.Min.Z) / 2;

        //    // Obtiene la ubicación del punto central medio
        //    double x = (bb.Min.X + xMedio);
        //    double y = (bb.Min.Y + yMedio);
        //    double z = (bb.Min.Z + zMedio);

        //    // Asigna donde se colocará la etiqueta
        //    XYZ punto = new XYZ(x, y, z);

        //    // Crea la etiqueta
        //    IndependentTag etiqueta = IndependentTag.Create(doc, tipoEtiqueta.Id, vista.Id, referencia, false, TagOrientation.Horizontal, punto);

        //    return etiqueta;
        //}

        ///<summary> Crea una etiqueta horizontal en la parte media derecha sin guía en una vista particular </summary>
        //public static IndependentTag CrearEtiquetaIndependienteCentroDerechaOriginal(Document doc, View vista, Element elem, FamilySymbol tipoEtiqueta)
        //{
        //    // Obtiene la referencia del elemento
        //    Reference referencia = new Reference(elem);

        //    // Crea la caja que contiene al elemento
        //    BoundingBoxXYZ bb = ObtenerRecuadroElementoParaleloAVista(doc, vista, elem);

        //    // Obtiene el volumen tridimensional del elemento
        //    double zMedio = (bb.Max.Z - bb.Min.Z) / 2;

        //    // Obtiene la ubicación del punto central derecho
        //    double x = bb.Max.X;
        //    double y = bb.Max.Y;
        //    double z = (bb.Min.Z + zMedio);

        //    // Asigna donde se colocará la etiqueta
        //    XYZ punto = new XYZ(x, y, z);

        //    // Crea la etiqueta
        //    IndependentTag etiqueta = IndependentTag.Create(doc, tipoEtiqueta.Id, vista.Id, referencia, false, TagOrientation.Horizontal, punto);

        //    return etiqueta;
        //}

        ///<summary> Crea una etiqueta horizontal en la parte inferior izquierda sin guía en una vista particular </summary>
        //public static IndependentTag CrearEtiquetaIndependienteAbajoIzquierdaOriginal(Document doc, View vista, Element elem, FamilySymbol tipoEtiqueta)
        //{
        //    // Obtiene la referencia del elemento
        //    Reference referencia = new Reference(elem);

        //    // Crea la caja que contiene al elemento
        //    BoundingBoxXYZ bb = ObtenerRecuadroElementoParaleloAVista(doc, vista, elem);

        //    // Obtiene la ubicación del punto inferior izquierdo
        //    double x = bb.Min.X;
        //    double y = bb.Min.Y;
        //    double z = bb.Min.Z;

        //    // Asigna donde se colocará la etiqueta
        //    XYZ punto = new XYZ(x, y, z);

        //    // Crea la etiqueta
        //    IndependentTag etiqueta = IndependentTag.Create(doc, tipoEtiqueta.Id, vista.Id, referencia, false, TagOrientation.Horizontal, punto);

        //    // Obtiene la caja que contiene a la etiqueta
        //    BoundingBoxXYZ bbetiqueta = etiqueta.get_BoundingBox(vista);

        //    // Obtiene mitad de la altura de la etiqueta
        //    double altura = (bbetiqueta.Max.Z - bbetiqueta.Min.Z) / 2;

        //    // Asigna la nueva ubicación a la etiqueta
        //    XYZ puntoFinal = new XYZ(x, y, (z - altura));

        //    // Coloca la etiqueta correctamente
        //    etiqueta.TagHeadPosition = puntoFinal;

        //    return etiqueta;
        //}

        ///<summary> Crea una etiqueta horizontal en la parte inferior central sin guía en una vista particular </summary>
        //public static IndependentTag CrearEtiquetaIndependienteAbajoMedioOriginal(Document doc, View vista, Element elem, FamilySymbol tipoEtiqueta)
        //{
        //    // Obtiene la referencia del elemento
        //    Reference referencia = new Reference(elem);

        //    // Crea la caja que contiene al elemento
        //    BoundingBoxXYZ bb = ObtenerRecuadroElementoParaleloAVista(doc, vista, elem);

        //    // Obtiene el volumen tridimensional del elemento
        //    double xMedio = (bb.Max.X - bb.Min.X) / 2;
        //    double yMedio = (bb.Max.Y - bb.Min.Y) / 2;

        //    // Obtiene la ubicación del punto medio inferior
        //    double x = (bb.Min.X + xMedio);
        //    double y = (bb.Min.Y + yMedio);
        //    double z = bb.Min.Z;

        //    // Asigna donde se colocará la etiqueta
        //    XYZ punto = new XYZ(x, y, z);

        //    // Crea la etiqueta
        //    IndependentTag etiqueta = IndependentTag.Create(doc, tipoEtiqueta.Id, vista.Id, referencia, false, TagOrientation.Horizontal, punto);

        //    // Obtiene la caja que contiene a la etiqueta
        //    BoundingBoxXYZ bbetiqueta = etiqueta.get_BoundingBox(vista);

        //    // Obtiene mitad de la altura de la etiqueta
        //    double altura = bbetiqueta.Max.Z - bbetiqueta.Min.Z;

        //    // Asigna la nueva ubicación a la etiqueta
        //    XYZ puntoFinal = new XYZ(x, y, (z - altura));

        //    // Coloca la etiqueta correctamente
        //    etiqueta.TagHeadPosition = puntoFinal;

        //    return etiqueta;
        //}

        ///<summary> Crea una etiqueta horizontal en la parte inferior derecha sin guía en una vista particular </summary>
        //public static IndependentTag CrearEtiquetaIndependienteAbajoDerechaOriginal(Document doc, View vista, Element elem, FamilySymbol tipoEtiqueta)
        //{
        //    // Obtiene la referencia del elemento
        //    Reference referencia = new Reference(elem);

        //    // Crea la caja que contiene al elemento
        //    BoundingBoxXYZ bb = ObtenerRecuadroElementoParaleloAVista(doc, vista, elem);

        //    // Obtiene la ubicación del punto inferior derecho
        //    double x = bb.Max.X;
        //    double y = bb.Max.Y;
        //    double z = bb.Min.Z;

        //    // Asigna donde se colocará la etiqueta
        //    XYZ punto = new XYZ(x, y, z);

        //    // Crea la etiqueta
        //    IndependentTag etiqueta = IndependentTag.Create(doc, tipoEtiqueta.Id, vista.Id, referencia, false, TagOrientation.Horizontal, punto);

        //    // Obtiene la caja que contiene a la etiqueta
        //    BoundingBoxXYZ bbetiqueta = etiqueta.get_BoundingBox(vista);

        //    // Obtiene mitad de la altura de la etiqueta
        //    double altura = (bbetiqueta.Max.Z - bbetiqueta.Min.Z) / 2;

        //    // Asigna la nueva ubicación a la etiqueta
        //    XYZ puntoFinal = new XYZ(x, y, (z - altura));

        //    // Coloca la etiqueta correctamente
        //    etiqueta.TagHeadPosition = puntoFinal;

        //    return etiqueta;
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
