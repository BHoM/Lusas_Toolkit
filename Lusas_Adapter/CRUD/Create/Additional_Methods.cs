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
using LusasM15_2;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        public void assignObjectSet(IFPoint newPoint, HashSet<String> tags)
        {
            foreach(string tag in tags)
            {
                IFObjectSet objectSet = d_LusasData.getGroupByName(tag);
                objectSet.add(newPoint);
            }
        }

        public void assignObjectSet(IFLine newLine, HashSet<String> tags)
        {
            foreach (string tag in tags)
            {
                IFObjectSet objectSet = d_LusasData.getGroupByName(tag);
                objectSet.add(newLine);
            }
        }

        public void assignObjectSet(IFSurface newSurface, HashSet<String> tags)
        {
            foreach (string tag in tags)
            {
                IFObjectSet objectSet = d_LusasData.getGroupByName(tag);
                objectSet.add(newSurface);
            }
        }
    }
}
