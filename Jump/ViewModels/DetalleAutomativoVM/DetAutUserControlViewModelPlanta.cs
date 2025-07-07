using Jump.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jump.ViewModels
{
    public class DetAutUserControlViewModelPlanta : DetAutUserControlViewModel
    {
        public DetAutUserControlViewModelPlanta(DetalleAutomaticoModel model) : base(model)
        {
            TiposDeVistas = Modelo.ObtenerTiposDePlanoEstructural();
            TipoDeVista = TiposDeVistas.FirstOrDefault();
        }
    }
}
