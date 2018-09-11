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
        public IFLoadcase CreateLoadcase(Loadcase loadcase)
        {

            IFLoadcase lusasLoadcase = null;

            if (d_LusasData.existsLoadset("Lc" + loadcase.CustomData[AdapterId] + "/" + loadcase.Name))
            {
                lusasLoadcase = (IFLoadcase)d_LusasData.getLoadset("Lc" + loadcase.CustomData[AdapterId] + "/" + loadcase.Name);
            }
            else
            {
                lusasLoadcase = d_LusasData.createLoadcase("Lc" + loadcase.CustomData[AdapterId] + "/" + loadcase.Name,"",loadcase.Number);
            }
            return lusasLoadcase;
        }
    }
}