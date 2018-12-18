using BH.oM.Common.Materials;
using Lusas.LPI;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        public IFAttribute CreateMaterial(Material material)
        {
            if (!CheckIllegalCharacters(material.Name))
            {
                return null;
            }

            IFAttribute lusasMaterial = null;
            string lusasName = "M" + material.CustomData[AdapterId] + "/" + material.Name;
            if (d_LusasData.existsAttribute("Material", lusasName))
            {
                lusasMaterial = d_LusasData.getAttribute("Material", lusasName);
            }
            else
            {
                lusasMaterial = d_LusasData.createIsotropicMaterial(material.Name,
                material.YoungsModulus, material.PoissonsRatio, material.Density, material.CoeffThermalExpansion);

                lusasMaterial.setName(lusasName);
            }
            return lusasMaterial;
        }
    }
}

