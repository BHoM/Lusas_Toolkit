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
        public IFLine CreateEdge(Edge edge, IFPoint startPoint, IFPoint endPoint)
        {
            IFLine newLine = d_LusasData.createLineByPoints(startPoint, endPoint);
            newLine.setName("L" + edge.CustomData[AdapterId].ToString());

            if (!(edge.Tags.Count == 0))
            {
                AssignObjectSet(newLine, edge.Tags);
            }

            if (!(edge.Constraint == null))
            {
                string supportName  = "Sp" + edge.Constraint.CustomData[AdapterId] + "/" + edge.Constraint.Name;
                IFAttribute lusasSupport = d_LusasData.getAttribute("Support", supportName);
                lusasSupport.assignTo(newLine);
            }

            return newLine;
        }
    }


}
