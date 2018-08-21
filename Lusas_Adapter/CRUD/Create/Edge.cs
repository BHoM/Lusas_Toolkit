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
        public IFLine CreateEdge(Edge edge, List<IFPoint> lusasPoints)
        {
            int bhomID;
            if (edge.CustomData.ContainsKey(AdapterId))
                bhomID = System.Convert.ToInt32(edge.CustomData[AdapterId]);
            else
                bhomID = System.Convert.ToInt32(NextId(edge.GetType()));

            Point startPoint = edge.Curve.IStartPoint();
            Point endPoint = edge.Curve.IEndPoint();

            edge.CustomData[AdapterId] = bhomID;

            int startIndex = lusasPoints.FindIndex(m => Math.Round(m.getX(), 3).Equals(Math.Round(startPoint.X, 3)) &&
                                                        Math.Round(m.getY(), 3).Equals(Math.Round(startPoint.Y, 3)) &&
                                                        Math.Round(m.getZ(), 3).Equals(Math.Round(startPoint.Z, 3)));

            int endIndex = lusasPoints.FindIndex(m => Math.Round(m.getX(), 3).Equals(Math.Round(endPoint.X, 3)) &&
                                                      Math.Round(m.getY(), 3).Equals(Math.Round(endPoint.Y, 3)) &&
                                                      Math.Round(m.getZ(), 3).Equals(Math.Round(endPoint.Z, 3)));

            IFLine newLine = d_LusasData.createLineByPoints(lusasPoints[startIndex], lusasPoints[endIndex]);
            newLine.setName("L" + edge.CustomData[AdapterId]);

            if (!(edge.Tags.Count == 0))
            {
                assignObjectSet(newLine, edge.Tags);
            }
            return newLine;
        }
    }


}
