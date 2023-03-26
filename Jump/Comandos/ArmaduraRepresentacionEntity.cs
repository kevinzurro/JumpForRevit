using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Revit.ES.Extension;
using Revit.ES.Extension.Attributes;

namespace Jump
{
    [Schema("ed2d5175-56e3-42a2-a433-aedfda1ccedd", AboutJump.nombreEsquema)]
    public class ArmaduraRepresentacionEntity : IRevitEntity
    {
        [Field]
        public ElementId Vista { get; set; }

        [Field]
        public List<ElementId> ListaCurvas { get; set; }

        [Field]
        public List<ElementId> ListaTextos { get; set; }

        [Field]
        public ElementId TipoDeTexto { get; set; }

        [Field]
        public ElementId Etiqueta { get; set; }

        [Field(Documentation = "XYZ Position", UnitType = UnitType.UT_Length)]
        public XYZ Posicion { get; set; }
    }

    [Schema("b8e4e02b-3886-4918-84f0-d8df893fc5a1", AboutJump.nombreEsquemaArmaduras)]
    public class RepresentacionesEntity : IRevitEntity
    {
        [Field]
        public List<ArmaduraRepresentacionEntity> ListaRepresentaciones { get; set; }
    }
}
