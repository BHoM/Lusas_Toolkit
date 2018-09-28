using System.Collections.Generic;
using System.Linq;
using System;
using BH.oM.Base;
using BH.oM.Structure.Elements;
using BH.oM.Geometry;
using BH.oM.Structure.Properties;
using BH.oM.Structure.Loads;
using BH.oM.Common.Materials;
using BH.Engine.Geometry;
using BH.Engine.Structure;
using Lusas.LPI;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        public IFAttribute CreateMaterial(Material material)
        {
            IFAttribute lusasMaterial = null;
            string lusasAttributeName = "M" + material.CustomData[AdapterId] + "/" + material.Name;
            if (d_LusasData.existsAttribute("Material", lusasAttributeName))
            {
                lusasMaterial = d_LusasData.getAttribute("Material", lusasAttributeName);
            }
            else
            {
                lusasMaterial = d_LusasData.createIsotropicMaterial(material.Name,
                material.YoungsModulus, material.PoissonsRatio, material.Density, material.CoeffThermalExpansion);

                lusasMaterial.setName(lusasAttributeName);
            }
            return lusasMaterial;
        }
    }
}

