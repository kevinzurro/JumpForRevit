using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.UI.Events;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Diagnostics;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;

namespace Jump.Models
{
    public class ConfiguracionesModel : ModelBase
    {
        ForgeTypeId tipoUnidad;

        public ConfiguracionesModel(UIApplication UIApp)
        {
            this.UIApp = UIApp;
            this.UIDoc = UIApp.ActiveUIDocument;
            this.Appli = UIApp.Application;
            this.Doc = UIDoc.Document;

            FormatOptions forOpt = Doc.GetUnits().GetFormatOptions(SpecTypeId.Length);
            this.tipoUnidad = forOpt.GetUnitTypeId();
        }

        public double ConvertirDesdeUnidadesInternas(double distancia)
        {
            double valor;

            valor = UnitUtils.ConvertFromInternalUnits(distancia, this.tipoUnidad);

            return valor;
        }

        public double ConvertirAUnidadesInternas(double distancia)
        {
            double valor;

            valor = UnitUtils.ConvertToInternalUnits(distancia, this.tipoUnidad);

            return valor;
        }

        public string ObtenerUnidadDistancia()
        {
            string unidad = LabelUtils.GetLabelForUnit(this.tipoUnidad);

            return unidad;
        }
    }
}
