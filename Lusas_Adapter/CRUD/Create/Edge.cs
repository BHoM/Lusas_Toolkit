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
        public IFLine CreateEdge(Edge edge, IFPoint startPoint, IFPoint endPoint)
        {
            IFLine newLine = d_LusasData.createLineByPoints(startPoint, endPoint);
            newLine.setName("L" + edge.CustomData[AdapterId].ToString());

            if (!(edge.Tags.Count == 0))
            {
                assignObjectSet(newLine, edge.Tags);
            }
            return newLine;
        }
    }


}
