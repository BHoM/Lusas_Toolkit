using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.oM.Base;
using BH.oM.Geometry;
using BH.oM.Structural.Elements;
using BH.oM.Structural.Properties;
using BH.oM.Common.Materials;
using LusasM15_2;

namespace BH.Engine.Lusas
{
    public static partial class Convert
    {
        public static Material ToBHoMMaterial(this IFAttribute lusasAttribute)
        {
            string attributeName = getAttributeName(lusasAttribute);

            object[] valueNames = lusasAttribute.getValueNames();

            Material bhomMaterial = new Material
            {
                YoungsModulus = lusasAttribute.getValue("E"),
                PoissonsRatio = lusasAttribute.getValue("nu"),
                Density = lusasAttribute.getValue("rho"),
                CoeffThermalExpansion = lusasAttribute.getValue("alpha"),
            };

            //How to combine the mass Rayleigh and stiffness Rayleigh in to a single damping constant
            //https://www.orcina.com/SoftwareProducts/OrcaFlex/Documentation/Help/Content/html/RayleighDamping.htm

            int bhomID = getBHoMID(lusasAttribute, 'M');

            bhomMaterial.CustomData["Lusas_id"] = bhomID;

            return bhomMaterial;
        }


    }
}
