using System.Collections.Generic;
using BH.oM.Structure.Elements;
using Lusas.LPI;
using BH.oM.Base;

namespace BH.Engine.Lusas
{
    public partial class Convert
    {
        public static IEnumerable<Node> GetNodeAssignments(IEnumerable<IFAssignment> lusasAssignments,
    Dictionary<string, Node> bhomNodes)
        {
            List<Node> assignedNodes = new List<Node>();
            Node bhomNode = new Node();

            foreach (IFAssignment lusasAssignment in lusasAssignments)
            {
                if (lusasAssignment.getDatabaseObject() is IFPoint)
                {
                    IFPoint lusasPoint = (IFPoint)lusasAssignment.getDatabaseObject();
                    bhomNodes.TryGetValue(RemovePrefix(lusasPoint.getName(), "P"), out bhomNode);
                    assignedNodes.Add(bhomNode);
                }
                else
                {
                    WarningLineAssignment(lusasAssignment);
                    WarningSurfaceAssignment(lusasAssignment);
                }
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
                if (lusasAssignment.getDatabaseObject() is IFLine)
                {
                    IFLine lusasLine = (IFLine)lusasAssignment.getDatabaseObject();
                    bhomBars.TryGetValue(RemovePrefix(lusasLine.getName(), "L"), out bhomBar);
                    assignedBars.Add(bhomBar);
                }
                else
                {
                    WarningPointAssignment(lusasAssignment);
                    WarningSurfaceAssignment(lusasAssignment);
                }

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
                if (lusasAssignment.getDatabaseObject() is IFSurface)
                {
                    IFSurface lusasSurface = (IFSurface)lusasAssignment.getDatabaseObject();
                    bhomPlanarPanels.TryGetValue(RemovePrefix(lusasSurface.getName(), "S"), out bhomPanelPlanar);
                    assignedSurfs.Add(bhomPanelPlanar);
                }
                else
                {
                    WarningPointAssignment(lusasAssignment);
                    WarningLineAssignment(lusasAssignment);
                }
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
                    bhomNodes.TryGetValue(RemovePrefix(lusasGeometry.getName(), "P"), out bhomNode);
                    assignedObjects.Add(bhomNode);
                }
                else if (lusasGeometry is IFLine)
                {
                    bhomBars.TryGetValue(RemovePrefix(lusasGeometry.getName(), "L"), out bhomBar);
                    assignedObjects.Add(bhomBar);
                }
                else if (lusasGeometry is IFSurface)
                {
                    bhomPlanarPanels.TryGetValue(RemovePrefix(lusasGeometry.getName(), "S"), out bhomPanelPlanar);
                    assignedObjects.Add(bhomPanelPlanar);
                }
            }

            return assignedObjects;
        }
    }
}