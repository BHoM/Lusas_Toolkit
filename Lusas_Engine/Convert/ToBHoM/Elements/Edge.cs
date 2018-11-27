using System.Collections.Generic;
using BH.oM.Structure.Elements;
using BH.oM.Geometry;
using Lusas.LPI;

namespace BH.Engine.Lusas
{
    public static partial class Convert
    {
        public static Edge ToBHoMEdge(this IFLine lusasLine,
            Dictionary<string, Node> bhomNodes, HashSet<string> groupNames)
        {
            Node startNode = GetNode(lusasLine, 0, bhomNodes);
            Node endNode = GetNode(lusasLine, 1, bhomNodes);

            Point startPoint = new Point
            {
                X = startNode.Position.X,
                Y = startNode.Position.Y,
                Z = startNode.Position.Z
            };

            Point endPoint = new Point
            {
                X = endNode.Position.X,
                Y = endNode.Position.Y,
                Z = endNode.Position.Z
            };

            HashSet<string> tags = new HashSet<string>(IsMemberOf(lusasLine, groupNames));

            Line bhomLine = new Line { Start = startPoint, End = endPoint };

            Edge bhomEdge = new Edge { Curve = bhomLine, Tags = tags };

            string adapterID = RemovePrefix(lusasLine.getName(), "L");

            bhomEdge.CustomData["Lusas_id"] = adapterID;

            return bhomEdge;
        }
    }
}
