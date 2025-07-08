using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Jump.Models;

namespace Jump.ViewModels
{
    public class DetAutUserControlViewModelXX : DetAutUserControlViewModel
    {
        public DetAutUserControlViewModelXX(DetalleAutomaticoModel model) : base(model)
        {
            TiposDeVistas = Modelo.ObtenerTiposDeSecciones();
            TipoDeVista = TiposDeVistas.FirstOrDefault();

            Element elem = Familia.ObtenerElemento(VistaPrevia);

            View vista = Modelo.CrearVistaXX(elem, this);

            if (vista != null)
            {
                Modelo.CrearEtiquetasDeElemento(this, vista, elem);

                VistaActual = new PreviewControl(Modelo.Doc, vista.Id);
            }
        }
    }
}
