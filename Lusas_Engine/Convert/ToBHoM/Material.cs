using BH.oM.Common.Materials;
using Lusas.LPI;

namespace BH.Engine.Lusas
{
    public static partial class Convert
    {
        public static Material ToBHoMMaterial(this IFAttribute lusasAttribute)
        {
            string attributeName = GetName(lusasAttribute);

            Material bhomMaterial = new Material
            {
                YoungsModulus = lusasAttribute.getValue("E"),
                PoissonsRatio = lusasAttribute.getValue("nu"),
                Density = lusasAttribute.getValue("rho"),
                CoeffThermalExpansion = lusasAttribute.getValue("alpha"),
                Name = attributeName
            };

            //How to combine the mass Rayleigh and stiffness Rayleigh in to a single damping constant
            //https://www.orcina.com/SoftwareProducts/OrcaFlex/Documentation/Help/Content/html/RayleighDamping.htm

            int bhomID = GetBHoMID(lusasAttribute, 'M');

            bhomMaterial.CustomData["Lusas_id"] = bhomID;

            return bhomMaterial;
        }


    }
}
