using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.oM.Structural.Elements;
using BH.oM.Geometry;
using BH.Engine.Geometry;
using LusasM15_2;

namespace BH.Engine.Lusas
{
    public static partial class Convert
    {
        public static string removePrefix(string geometryName, string forRemoval)
        {
            string geometryID = "";

            if (geometryName.Contains(forRemoval))
            {
                geometryID = geometryName.Replace(forRemoval, "");
            }
            else
            {
                geometryID = geometryName;
            }
            return geometryID;
        }

        public static Node getNode(IFLine lusasLine, int nodeIndex, Dictionary<string, Node> bhomNodes)
        {
            Node bhomNode = null;
            IFPoint lusasPoint = lusasLine.getLOFs()[nodeIndex];
            String pointName = removePrefix(lusasPoint.getName(), "P");
            bhomNodes.TryGetValue(pointName, out bhomNode);

            return bhomNode;
        }

        public static Bar getBar(IFSurface lusasSurf, int lineIndex, Dictionary<string, Bar> bhomBars)
        {
            Bar bhomBar = null;
            IFLine lusasEdge = lusasSurf.getLOFs()[lineIndex];
            String lineName = removePrefix(lusasEdge.getName(), "L");
            bhomBars.TryGetValue(lineName, out bhomBar);

            return bhomBar;
        }

        public static List<String> isMemberOf(IFLine lusasLine, List<String> groupNames)
        {

            List<String> memberGroups = new List<string>();

            foreach (String groupName in groupNames)
            {
                if (lusasLine.isMemberOfGroup(groupName))
                {
                    memberGroups.Add(groupName);
                }
            }

            return memberGroups;
        }
    }
}
