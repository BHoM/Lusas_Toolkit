using System.Collections.Generic;
using System.Linq;
using System;
using BH.oM.Structure.Elements;
using BH.Engine.Geometry;
using BH.oM.Geometry;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        public static List<Edge> GetDistinctEdges(IEnumerable<Edge> edges)
        {
            List<Edge> distinctEdges = edges.GroupBy(m => new
            {
                X = Math.Round(m.Curve.IPointAtParameter(0.5).X, 3),
                Y = Math.Round(m.Curve.IPointAtParameter(0.5).Y, 3),
                Z = Math.Round(m.Curve.IPointAtParameter(0.5).Z, 3)
            })
        .Select(x => x.First())
        .ToList();

            return distinctEdges;
        }

        public static List<Point> GetDistinctPoints(IEnumerable<Point> points)
        {
            List<Point> distinctPoints = points.GroupBy(m => new
            {
                X = Math.Round(m.X, 3),
                Y = Math.Round(m.Y, 3),
                Z = Math.Round(m.Z, 3)
            })
                 .Select(x => x.First())
                 .ToList();

            return distinctPoints;
        }
    }
}