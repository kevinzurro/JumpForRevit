using Jump.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jump.ViewModels
{
    public class DetAutUserControlViewModelXX : DetAutUserControlViewModel
    {
        public DetAutUserControlViewModelXX(DetalleAutomaticoModel model) : base(model)
        {
            TiposDeVistas = Modelo.ObtenerTiposDeVistas();
            TipoDeVista = TiposDeVistas.FirstOrDefault();
        }
    }
}
