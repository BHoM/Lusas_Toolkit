using System.Collections.Generic;
using System.Linq;
using System;
using BH.oM.Structure.Elements;
using Lusas.LPI;
using BH.Engine.Geometry;
using BH.oM.Geometry;
using BH.oM.Base;
using BH.oM.Structure.Loads;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        public void AssignObjectSet(IFGeometry newGeometry, HashSet<string> tags)
        {
            foreach (string tag in tags)
            {
                IFObjectSet objectSet = d_LusasData.getGroupByName(tag);
                objectSet.add(newGeometry);
            }
        }

        public static IEnumerable<IGrouping<string, IFAssignment>> GetLoadAssignments(IFLoading lusasForce)
        {
            object[] assignmentObjects = lusasForce.getAssignments();
            List<IFAssignment> assignments = new List<IFAssignment>();

            for (int j = 0; j < assignmentObjects.Count(); j++)
            {
                IFAssignment assignment = (IFAssignment)assignmentObjects[j];
                assignments.Add(assignment);
            }

            IEnumerable<IGrouping<string, IFAssignment>> groupedByLoadcases =
                assignments.GroupBy(m => m.getAssignmentLoadset().getName());

            return groupedByLoadcases;
        }

        public void NameSearch(string prefix, string customID, string suffix, ref string name)
        {
            for (int i = 1; i < Int32.Parse(customID); i++)
            {
                name = prefix + i.ToString() + "/" + suffix;
                if (d_LusasData.existsAttribute("Loading", name))
                {
                    break;
                }
            }
        }

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

        public void CreateTags(IEnumerable<IBHoMObject> bhomObject)
        {
            List<string> objectTags = bhomObject.SelectMany(x => x.Tags).Distinct().ToList();

            foreach (string tag in objectTags)
            {
                if (!d_LusasData.existsGroupByName(tag))
                {
                    d_LusasData.createGroup(tag);
                }
            }
        }

        public IFPoint[] GetAssignedPoints(Load<Node> bhomLoads)
        {
            List<IFPoint> assignedGeometry = new List<IFPoint>();
            foreach (BHoMObject bhomObject in bhomLoads.Objects.Elements)
            {
                IFPoint lusasPoint = d_LusasData.getPointByName(
                    "P" + bhomObject.CustomData[AdapterId].ToString());

                assignedGeometry.Add(lusasPoint);
            }

            IFPoint[] arrayGeometry = assignedGeometry.ToArray();

            return arrayGeometry;
        }

        public IFLine[] GetAssignedLines(Load<Bar> bhomLoads)
        {
            List<IFLine> assignedGeometry = new List<IFLine>();
            foreach (BHoMObject bhomObject in bhomLoads.Objects.Elements)
            {
                IFLine lusasLine = d_LusasData.getLineByName(
                    "L" + bhomObject.CustomData[AdapterId].ToString());

                assignedGeometry.Add(lusasLine);
            }

            IFLine[] arrayGeometry = assignedGeometry.ToArray();

            return arrayGeometry;
        }

        public IFSurface[] GetAssignedSurfaces(Load<IAreaElement> bhomLoads)
        {
            List<IFSurface> assignedGeometry = new List<IFSurface>();
            foreach (BHoMObject bhomObject in bhomLoads.Objects.Elements)
            {
                IFSurface lusasSurface = d_LusasData.getSurfaceByName(
                    "S" + bhomObject.CustomData[AdapterId].ToString());

                assignedGeometry.Add(lusasSurface);
            }

            IFSurface[] arrayGeometry = assignedGeometry.ToArray();

            return arrayGeometry;
        }

        public IFGeometry[] GetAssignedObjects(Load<BHoMObject> bhomLoads)
        {
            List<IFGeometry> assignedGeometry = new List<IFGeometry>();

            foreach (BHoMObject bhomObject in bhomLoads.Objects.Elements)
            {
                if (bhomObject is Node)
                {
                    IFGeometry lusasPoint = d_LusasData.getPointByName(
                        "P" + bhomObject.CustomData[AdapterId].ToString());

                    assignedGeometry.Add(lusasPoint);
                }
                else if (bhomObject is Bar)
                {
                    IFGeometry lusasBar = d_LusasData.getLineByName(
                        "L" + bhomObject.CustomData[AdapterId].ToString());

                    assignedGeometry.Add(lusasBar);
                }
                else if (bhomObject is PanelPlanar)
                {
                    IFGeometry lusasSurface = d_LusasData.getSurfaceByName(
                        "S" + bhomObject.CustomData[AdapterId].ToString());

                    assignedGeometry.Add(lusasSurface);
                }
            }

            IFGeometry[] arrayGeometry = assignedGeometry.ToArray();

            return arrayGeometry;
        }
    }
}
