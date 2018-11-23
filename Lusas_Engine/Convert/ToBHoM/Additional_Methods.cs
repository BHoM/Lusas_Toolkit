using System;
using System.Collections.Generic;
using System.Linq;
using BH.oM.Structure.Elements;
using Lusas.LPI;
using BH.oM.Base;

namespace BH.Engine.Lusas
{
    public static partial class Convert
    {
        public static string removePrefix(string name, string forRemoval)
        {
            string adapterID = "";

            if (name.Contains(forRemoval))
            {
                adapterID = name.Replace(forRemoval, "");
            }
            else
            {
                adapterID = name;
            }
            return adapterID;
        }

        public static Node GetNode(IFLine lusasLine, int nodeIndex, Dictionary<string, Node> bhomNodes)
        {
            Node bhomNode = null;
            IFPoint lusasPoint = lusasLine.getLOFs()[nodeIndex];
            string pointName = removePrefix(lusasPoint.getName(), "P");
            bhomNodes.TryGetValue(pointName, out bhomNode);

            return bhomNode;
        }

        public static Bar GetBar(IFSurface lusasSurf, int lineIndex, Dictionary<string, Bar> bhomBars)
        {
            Bar bhomBar = null;
            IFLine lusasEdge = lusasSurf.getLOFs()[lineIndex];
            string lineName = removePrefix(lusasEdge.getName(), "L");
            bhomBars.TryGetValue(lineName, out bhomBar);
            return bhomBar;
        }

        public static Edge GetEdge(IFSurface lusasSurf, int lineIndex, Dictionary<string, Edge> bhomBars)
        {
            Edge bhomEdge = null;
            IFLine lusasEdge = lusasSurf.getLOFs()[lineIndex];
            string lineName = removePrefix(lusasEdge.getName(), "L");
            bhomBars.TryGetValue(lineName, out bhomEdge);
            return bhomEdge;
        }

        public static HashSet<string> IsMemberOf(IFGeometry lusasGeometry, HashSet<string> bhomTags)
        {

            HashSet<string> geometryTag = new HashSet<string>();

            foreach (string tag in bhomTags)
            {
                if (lusasGeometry.isMemberOfGroup(tag))
                {
                    geometryTag.Add(tag);
                }
            }

            return geometryTag;
        }

        public static List<string> AttributeAssignments(IFGeometry lusasGeometry, string attributeType)
        {
            object[] lusasAssignments = lusasGeometry.getAssignments(attributeType);

            List<string> attributeNames = new List<string>();

            int n = lusasAssignments.Count();
            for (int i = 0; i < n; i++)
            {
                IFAssignment lusasAssignment = lusasGeometry.getAssignments(attributeType)[i];
                IFAttribute lusasAttribute = lusasAssignment.getAttribute();
                string attributeName = GetName(lusasAttribute);
                attributeNames.Add(attributeName);
            }
            return attributeNames;
        }

        public static int GetAdapterID(IFAttribute lusasAttribute, char lastCharacter)
        {
            int adapterID = 0;

            lusasAttribute.getName();

            if (lusasAttribute.getName().Contains("/"))
            {
                adapterID = int.Parse(lusasAttribute.getName().Split(lastCharacter, '/')[1]);
            }
            else
            {
                adapterID = lusasAttribute.getID();
            }

            return adapterID;
        }

        public static int GetAdapterID(IFLoadcase lusasLoadcase, char lastCharacter)
        {
            int adapterID = 0;

            lusasLoadcase.getName();

            if (lusasLoadcase.getName().Contains("/"))
            {
                adapterID = int.Parse(lusasLoadcase.getName().Split(lastCharacter, '/')[1]);
            }
            else
            {
                adapterID = lusasLoadcase.getID();
            }

            return adapterID;
        }

        public static int GetAdapterID(IFBasicCombination lusasLoadCombination, char lastCharacter)
        {
            int adapterID = 0;

            lusasLoadCombination.getName();

            if (lusasLoadCombination.getName().Contains("/"))
            {
                adapterID = int.Parse(lusasLoadCombination.getName().Split(lastCharacter, '/')[1]);
            }
            else
            {
                adapterID = lusasLoadCombination.getID();
            }

            return adapterID;
        }

        public static string GetName(IFAttribute lusasAttribute)
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

        public static string GetName(IFLoadcase lusasLoadcase)
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

        public static string GetName(IFBasicCombination lusasLoadCombination)
        {
            string loadcaseName = "";

            if (lusasLoadCombination.getName().Contains("/"))
            {
                loadcaseName = lusasLoadCombination.getName().Substring(
                    lusasLoadCombination.getName().LastIndexOf("/") + 1);
            }
            else
            {
                loadcaseName = lusasLoadCombination.getName();
            }

            return loadcaseName;
        }

        public static string GetName(string loadName)
        {
            string bhomLoadName = "";

            if (loadName.Contains("/"))
            {
                bhomLoadName = loadName.Substring(
                    loadName.LastIndexOf("/") + 1);
            }
            else
            {
                bhomLoadName = loadName;
            }

            return bhomLoadName;
        }

        public static IEnumerable<Node> GetNodeAssignments(IEnumerable<IFAssignment> lusasAssignments,
            Dictionary<string, Node> bhomNodes)
        {
            List<Node> assignedNodes = new List<Node>();
            Node bhomNode = new Node();

            foreach (IFAssignment lusasAssignment in lusasAssignments)
            {
                IFPoint lusasPoint = (IFPoint)lusasAssignment.getDatabaseObject();
                bhomNodes.TryGetValue(removePrefix(lusasPoint.getName(), "P"), out bhomNode);
                assignedNodes.Add(bhomNode);
            }
            return assignedNodes;
        }

        public static IEnumerable<Bar> GetBarAssignments(IEnumerable<IFAssignment> lusasAssignments,
            Dictionary<string, Bar> bhomBars)
        {
            List<Bar> assignedBars = new List<Bar>();
            Bar bhomBar = new Bar();

            foreach (IFAssignment lusasAssignment in lusasAssignments)
            {
                IFLine lusasLine = (IFLine)lusasAssignment.getDatabaseObject();
                bhomBars.TryGetValue(removePrefix(lusasLine.getName(), "L"), out bhomBar);
                assignedBars.Add(bhomBar);
            }

            return assignedBars;
        }

        public static IEnumerable<IAreaElement> GetSurfaceAssignments(IEnumerable<IFAssignment> lusasAssignments,
            Dictionary<string, PanelPlanar> bhomPlanarPanels)
        {
            List<IAreaElement> assignedSurfs = new List<IAreaElement>();
            PanelPlanar bhomPanelPlanar = new PanelPlanar();

            foreach (IFAssignment lusasAssignment in lusasAssignments)
            {
                IFSurface lusasSurface = (IFSurface)lusasAssignment.getDatabaseObject();
                bhomPlanarPanels.TryGetValue(removePrefix(lusasSurface.getName(), "S"), out bhomPanelPlanar);
                assignedSurfs.Add(bhomPanelPlanar);
            }

            return assignedSurfs;
        }

        public static IEnumerable<BHoMObject> GetGeometryAssignments(IEnumerable<IFAssignment> lusasAssignments,
            Dictionary<string, Node> bhomNodes, Dictionary<string, Bar> bhomBars,
            Dictionary<string, PanelPlanar> bhomPlanarPanels)
        {
            List<BHoMObject> assignedObjects = new List<BHoMObject>();

            Node bhomNode = new Node();
            Bar bhomBar = new Bar();
            PanelPlanar bhomPanelPlanar = new PanelPlanar();

            foreach (IFAssignment lusasAssignment in lusasAssignments)
            {
                IFGeometry lusasGeometry = (IFGeometry)lusasAssignment.getDatabaseObject();

                if (lusasGeometry is IFPoint)
                {
                    bhomNodes.TryGetValue(removePrefix(lusasGeometry.getName(), "P"), out bhomNode);
                    assignedObjects.Add(bhomNode);
                }
                else if (lusasGeometry is IFLine)
                {
                    bhomBars.TryGetValue(removePrefix(lusasGeometry.getName(), "L"), out bhomBar);
                    assignedObjects.Add(bhomBar);
                }
                else if (lusasGeometry is IFSurface)
                {
                    bhomPlanarPanels.TryGetValue(removePrefix(lusasGeometry.getName(), "S"), out bhomPanelPlanar);
                    assignedObjects.Add(bhomPanelPlanar);
                }
            }

            return assignedObjects;
        }

    }
}
