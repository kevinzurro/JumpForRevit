﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI.Selection;

namespace Jump
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    internal class cmdArmaduraRepresentacion : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // Variables necesarias
            UIApplication uiApp = commandData.Application;
            UIDocument uiDoc = uiApp.ActiveUIDocument;
            Autodesk.Revit.ApplicationServices.Application app = uiApp.Application;
            Document doc = uiDoc.Document;

            string IdiomaDelPrograma = Tools.ObtenerIdiomaDelPrograma();

            // Muestra el formulario de Detalle de armadura
            frmDetalleArmadura inicioDetalleArmadura = new frmDetalleArmadura(doc);

            inicioDetalleArmadura.ShowDialog();

            DataGridView dgvEstiloLinea = inicioDetalleArmadura.dgvEstiloLinea;

            // Abre una transacción
            using (Transaction t = new Transaction(doc, Language.ObtenerTexto(IdiomaDelPrograma, "DetArm1")))
            {
                t.Start();

                try
                {
                    bool bandera = true;

                    Autodesk.Revit.DB.View vista = doc.ActiveView;

                    if (vista.SketchPlane == null)
                    {
                        using (SubTransaction st = new SubTransaction(doc))
                        {
                            st.Start();

                            try
                            {
                                Plane plano = Plane.CreateByNormalAndOrigin(vista.ViewDirection, vista.Origin);

                                SketchPlane sketch = SketchPlane.Create(doc, plano);

                                vista.SketchPlane = sketch;
                            }
                            catch (Exception) { }

                            st.Commit();
                        }
                    }

                    do
                    {
                        try
                        {
                            // Selecciona la barra
                            Reference referencia = uiDoc.Selection.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Element, Language.ObtenerTexto(IdiomaDelPrograma, "DetArm2"));

                            // Obtiene el elementos
                            Element elem = doc.GetElement(referencia);

                            // Verifica que sea una barra
                            if (elem is Rebar)
                            {
                                // Castea el elemento a barra
                                Rebar barra = elem as Rebar;

                                // Longitud parcial de barra
                                if (inicioDetalleArmadura.Longitud)
                                {
                                    try
                                    {
                                        // Crea una representación de barra
                                        ArmaduraRepresentacion armadura = new ArmaduraRepresentacion(doc, vista, barra);

                                        // Asigna el tipo de texto a la representación de la barra
                                        armadura.TipoDeTexto = inicioDetalleArmadura.tipoTexto;

                                        // Dibuja las armaduras y asigna los estilos de líneas en función de cada diámetro
                                        armadura.DibujarArmaduraSegunDatagridview(dgvEstiloLinea);

                                        // Verifica que la opción de etiqueta esté activo
                                        if (inicioDetalleArmadura.Armadura)
                                        {
                                            // Posición de la etiqueta
                                            int posicion = Jump.Properties.Settings.Default.EtiquetaIndependienteArmadura;

                                            // Asigna el tipo de etiqueta
                                            armadura.TipoEtiquetaArmadura = inicioDetalleArmadura.tipoEtiqueta;

                                            // Asigna la etiqueta
                                            armadura.EtiquetaArmadura = Tools.CrearEtiquetaSegunConfiguracion(doc, vista, barra, inicioDetalleArmadura.tipoEtiqueta, posicion);
                                        }

                                        // Punto donde se coloca el despiece
                                        XYZ puntoSeleccionado = uiDoc.Selection.PickPoint(Language.ObtenerTexto(IdiomaDelPrograma, "DetArm3"));
                                        
                                        // Asigna la posición de la Representación
                                        armadura.Posicion = puntoSeleccionado - armadura.PuntoMedio;

                                        // Mueve la representación de la armadura
                                        armadura.MoverArmaduraRepresentacionConEtiqueta(armadura.Posicion);

                                        // Agrega la armadura a la lista de despieces
                                        Inicio.listaArmaduraRepresentacion.Add(armadura);
                                    }
                                    catch (Exception) { }
                                }
                            }
                        }
                        catch (Exception) 
                        { 
                            bandera = false; 
                        }

                    } while (bandera);
                }
                catch (Exception) { }

                t.Commit();
            }

            return Result.Succeeded;
        }
    }
}
