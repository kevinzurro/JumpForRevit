using Jump.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jump.ViewModels
{
    public class DetAutUserControlViewModelYY : DetAutUserControlViewModel
    {
        public DetAutUserControlViewModelYY(DetalleAutomaticoModel model) : base(model)
        {
            TiposDeVistas = Modelo.ObtenerTiposDeVistas();
            TipoDeVista = TiposDeVistas.FirstOrDefault();
        }
    }
}
