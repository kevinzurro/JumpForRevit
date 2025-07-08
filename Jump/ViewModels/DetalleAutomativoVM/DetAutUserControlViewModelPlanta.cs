using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Jump.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jump.ViewModels
{
    public class DetAutUserControlViewModelPlanta : DetAutUserControlViewModel
    {
        public DetAutUserControlViewModelPlanta(DetalleAutomaticoModel model) : base(model)
        {
            TiposDeVistas = Modelo.ObtenerTiposDePlanosEstructurales();
            TipoDeVista = TiposDeVistas.FirstOrDefault();

            Element elem = Familia.ObtenerElemento(VistaPrevia);

            View vista = Modelo.CrearVistaEnPlanta(elem, this);

            if (vista != null)
            {
                Modelo.CrearEtiquetasDeElemento(this, vista, elem);
                
                VistaActual = new PreviewControl(Modelo.Doc, vista.Id);
            }
        }
    }
}
