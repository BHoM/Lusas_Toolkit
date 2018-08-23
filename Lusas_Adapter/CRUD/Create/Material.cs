using System.Collections.Generic;
using System.Linq;
using System;
using BH.oM.Base;
using BH.oM.Structural.Elements;
using BH.oM.Geometry;
using BH.oM.Structural.Properties;
using BH.oM.Structural.Loads;
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
            IFAttribute lusasSupport = d_LusasData.createIsotropicMaterial(material.Name,
                material.YoungsModulus,material.PoissonsRatio,material.Density,material.CoeffThermalExpansion);

            int bhomID;
            if (material.CustomData.ContainsKey(AdapterId))
                bhomID = System.Convert.ToInt32(material.CustomData[AdapterId]);
            else
                bhomID = System.Convert.ToInt32(NextId(material.GetType()));

            material.CustomData[AdapterId] = bhomID;

            lusasSupport.setName("M" + bhomID + "/" + material.Name);

            return lusasSupport;
        }
    }
}

