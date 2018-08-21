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

        public static Edge getEdge(IFSurface lusasSurf, int lineIndex, Dictionary<string, Edge> bhomBars)
        {
            Edge bhomEdge = null;
            IFLine lusasEdge = lusasSurf.getLOFs()[lineIndex];
            String lineName = removePrefix(lusasEdge.getName(), "L");
            bhomBars.TryGetValue(lineName, out bhomEdge);
            return bhomEdge;
        }

        public static HashSet<String> isMemberOf(IFGeometry lusasGeometry, HashSet<String> groupNames)
        {

            HashSet<String> memberGroups = new HashSet<string>();

            foreach (String groupName in groupNames)
            {
                if (lusasGeometry.isMemberOfGroup(groupName))
                {
                    memberGroups.Add(groupName);
                }
            }

            return memberGroups;
        }

        public static List<String> attributeAssignments(IFGeometry lusasGeometry, String attributeType)
        {
            Object[] attributeAssignments = lusasGeometry.getAssignments(attributeType);

            List<String> attributeNames = new List<String>();

            int n = attributeAssignments.Count();
            for (int i = 0; i < n; i++)
            {
                IFAssignment attributeAssignment = lusasGeometry.getAssignments()[i];
                IFAttribute lusasAttribute = attributeAssignment.getAttribute();
                string attributeName = getAttributeName(lusasAttribute);

                attributeNames.Add(attributeName);
            }

            return attributeNames;
        }

        public static int getBHoMID(IFAttribute lusasAttribute, char lastCharacter)
        {
            int bhomID = 0;

            if (lusasAttribute.getName().Contains("/"))
            {
                bhomID = Int32.Parse(lusasAttribute.getName().Split(lastCharacter, '/')[1]);
            }
            else
            {
                bhomID = lusasAttribute.getID();
            }

            return bhomID;
        }

        public static string getAttributeName(IFAttribute lusasAttribute)
        {
            string attributeName = "";

            if (lusasAttribute.getName().Contains("/"))
            {
                attributeName = lusasAttribute.getName().Substring(
                    lusasAttribute.getName().LastIndexOf("/") + 1);
            }
            else
            {
                attributeName = lusasAttribute.getName();
            }

            return attributeName;
        }
    }
}
