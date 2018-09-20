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
        public static string removePrefix(string name, string forRemoval)
        {
            string geometryID = "";

            if (name.Contains(forRemoval))
            {
                geometryID = name.Replace(forRemoval, "");
            }
            else
            {
                geometryID = name;
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
                IFAssignment attributeAssignment = lusasGeometry.getAssignments(attributeType)[i];
                IFAttribute lusasAttribute = attributeAssignment.getAttribute();
                string attributeName = getName(lusasAttribute);
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

        public static int getBHoMID(IFLoadcase lusasLoadcase, char lastCharacter)
        {
            int bhomID = 0;

            if (lusasLoadcase.getName().Contains("/"))
            {
                bhomID = Int32.Parse(lusasLoadcase.getName().Split(lastCharacter, '/')[1]);
            }
            else
            {
                bhomID = lusasLoadcase.getID();
            }

            return bhomID;
        }

        public static string getName(IFAttribute lusasAttribute)
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

        public static string getName(IFLoadcase lusasLoadcase)
        {
            string loadcaseName = "";

            if (lusasLoadcase.getName().Contains("/"))
            {
                loadcaseName = lusasLoadcase.getName().Substring(
                    lusasLoadcase.getName().LastIndexOf("/") + 1);
            }
            else
            {
                loadcaseName = lusasLoadcase.getName();
            }

            return loadcaseName;
        }

        public static string getName(string loadname)
        {
            string bhomLoadName = "";

            if (loadname.Contains("/"))
            {
                bhomLoadName = loadname.Substring(
                    loadname.LastIndexOf("/") + 1);
            }
            else
            {
                bhomLoadName = loadname;
            }

            return bhomLoadName;
        }
    }
}
