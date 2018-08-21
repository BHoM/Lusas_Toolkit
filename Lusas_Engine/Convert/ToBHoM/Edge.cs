using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.oM.Structure.Elements;
using BH.oM.Geometry;
using BH.Engine.Geometry;
using Lusas.LPI;

namespace BH.Engine.Lusas
{
    public static partial class Convert
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        //Add methods for converting to BHoM from the specific software types, if possible to do without any BHoM calls
        //Example:
        //public static Node ToBHoM(this LusasNode node)
        //{

        //#region Geometry Converters


        public static Edge ToBHoMEdge(this IFLine lusasLine, Dictionary<string, Node> bhomNodes, HashSet<String> groupNames)
        {

            Node startNode = getNode(lusasLine, 0, bhomNodes);
            Node endNode = getNode(lusasLine, 1, bhomNodes);

            Point startPoint = new Point { X = startNode.Position.X, Y = startNode.Position.Y, Z = startNode.Position.Z };
            Point endPoint = new Point { X = endNode.Position.X, Y = endNode.Position.Y, Z = endNode.Position.Z };

            HashSet<String> tags = new HashSet<string>(isMemberOf(lusasLine, groupNames));

            Line bhomLine = new Line { Start = startPoint, End = endPoint };

            Edge bhomEdge = new Edge { Curve = bhomLine, Tags = tags };

            String lineName = removePrefix(lusasLine.getName(), "L");

            bhomEdge.CustomData["Lusas_id"] = lineName;


            return bhomEdge;
        }
    }
}
