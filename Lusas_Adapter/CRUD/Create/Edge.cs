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
        public IFLine CreateEdge(Edge edge, List<Edge> existingEdges)
        {
            IFLine newLine;

            int bhomID;
            if (edge.CustomData.ContainsKey(AdapterId))
                bhomID = System.Convert.ToInt32(edge.CustomData[AdapterId]);
            else
                bhomID = System.Convert.ToInt32(NextId(edge.GetType()));

            edge.CustomData[AdapterId] = bhomID;

            int position = existingEdges.FindIndex(m =>
                Math.Round(m.Curve.IPointAtParameter(0.5).X, 3).Equals(Math.Round(edge.Curve.IPointAtParameter(0.5).X, 3)) &&
                Math.Round(m.Curve.IPointAtParameter(0.5).Y, 3).Equals(Math.Round(edge.Curve.IPointAtParameter(0.5).Y, 3)) &&
                Math.Round(m.Curve.IPointAtParameter(0.5).Z, 3).Equals(Math.Round(edge.Curve.IPointAtParameter(0.5).Z, 3)));

            if (position == -1)
            {
                IFPoint startPoint = CreatePoint(edge.Curve.IStartPoint());
                IFPoint endPoint = CreatePoint(edge.Curve.IEndPoint());
                newLine = d_LusasData.createLineByPoints(startPoint, endPoint);
                newLine.setName("L" + edge.CustomData[AdapterId]);
            }
            else
            {
                newLine = d_LusasData.getLineByName("L"+existingEdges[position].CustomData[AdapterId].ToString());
            }

            if (!(edge.Tags.Count == 0))
            {
                assignObjectSet(newLine, edge.Tags);
            }
            return newLine;
        }
    }

}
