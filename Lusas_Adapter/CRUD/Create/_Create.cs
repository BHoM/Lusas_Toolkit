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
using Lusas.LPI;
using BH.Engine.Lusas.Object_Comparer.Equality_Comparer;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        /***************************************************/
        /**** Adapter overload method                   ****/
        /***************************************************/

        protected override bool Create<T>(IEnumerable<T> objects, bool replaceAll = false)
        {
            bool success = true;        //boolean returning if the creation was successfull or not

            if (objects.Count() > 0)
            {
                if (objects.First() is Node)
                {
                    success = CreateCollection(objects as IEnumerable<Node>);
                }
                if (objects.First() is Bar)
                {
                    success = CreateCollection(objects as IEnumerable<Bar>);
                }
                if (objects.First() is PanelPlanar)
                {
                    success = CreateCollection(objects as IEnumerable<PanelPlanar>);
                }
                if (objects.First() is Edge)
                {
                    success = CreateCollection(objects as IEnumerable<Edge>);
                }
                if (objects.First() is Point)
                {
                    success = CreateCollection(objects as IEnumerable<Point>);
                }
                if (objects.First() is Material)
                {
                    success = CreateCollection(objects as IEnumerable<Material>);
                }
                if (objects.First() is Constraint6DOF)
                {
                    success = CreateCollection(objects as IEnumerable<Constraint6DOF>);
                }
                //if (objects.First().GetType().GetInterfaces().Contains(typeof(ISectionProperty)))
                //{
                //    success = CreateCollection(objects as IEnumerable<ISectionProperty>);
                //}
            }

            //success = CreateCollection(objects as dynamic);
            m_LusasApplication.updateAllViews();

            return success;             //Finally return if the creation was successful or not

        }


        /***************************************************/
        /**** Private methods                           ****/
        /***************************************************/

        private bool CreateCollection(IEnumerable<Node> nodes)
        {
            //Code for creating a collection of nodes in the software

            List<String> nodeTags = nodes.SelectMany(x => x.Tags).Distinct().ToList();

            foreach (String tag in nodeTags)
            {
                if (!d_LusasData.existsGroupByName(tag))
                {
                    d_LusasData.createGroup(tag);
                }
            }

            foreach (Node node in nodes)
            {
                IFPoint newpoint = CreatePoint(node);
            }

            return true;
        }

        private bool CreateCollection(IEnumerable<Point> points)
        {

            List<Point> distinctPoints = points.GroupBy(m => new
            {
                X = Math.Round(m.X, 3),
                Y = Math.Round(m.Y, 3),
                Z = Math.Round(m.Z, 3)
            })
                 .Select(x => x.First())
                 .ToList();

            List<Point> existingPoints = ReadPoints();

            List<Point> pointsToCreate = distinctPoints.Except(existingPoints).ToList();


            foreach (Point point in pointsToCreate)
            {
                IFPoint newpoint = CreatePoint(point);
            }

            return true;
        }

        private bool CreateCollection(IEnumerable<Bar> bars)
        {
            //List<Bar> distinctBars = bars.GroupBy(m => new {
            //    X = Math.Round(m.Geometry().IPointAtParameter(0.5).X, 3),
            //    Y = Math.Round(m.Geometry().IPointAtParameter(0.5).Y, 3),
            //    Z = Math.Round(m.Geometry().IPointAtParameter(0.5).Z, 3)
            //})
            //.Select(x => x.First())
            //.ToList();


            List<String> barTags = bars.SelectMany(x => x.Tags).Distinct().ToList();

            foreach (String tag in barTags)
            {
                if (!d_LusasData.existsGroupByName(tag))
                {
                    d_LusasData.createGroup(tag);
                }
            }

            foreach (Bar bar in bars)
            {
                IFLine newline = CreateLine(bar);
            }
            return true;
        }

        /***************************************************/

        private bool CreateCollection(IEnumerable<PanelPlanar> panels)
        {

            List<String> barTags = panels.SelectMany(x => x.Tags).Distinct().ToList();

            foreach (String tag in barTags)
            {
                if (!d_LusasData.existsGroupByName(tag))
                {
                    d_LusasData.createGroup(tag);
                }
            }

            List<Edge> allEdges = new List<Edge>();

            foreach (PanelPlanar panel in panels)
            {
                allEdges.AddRange(panel.ExternalEdges);
            }

            List<Edge> distinctEdges = allEdges.GroupBy(m => new
            {
                X = Math.Round(m.Curve.IPointAtParameter(0.5).X, 3),
                Y = Math.Round(m.Curve.IPointAtParameter(0.5).Y, 3),
                Z = Math.Round(m.Curve.IPointAtParameter(0.5).Z, 3)
            })
                .Select(x => x.First())
                .ToList();

            List<Point> midpoints = new List<Point>();

            foreach (Edge edge in distinctEdges)
            {
                midpoints.Add(edge.Curve.IPointAtParameter(0.5));
            }

            foreach (PanelPlanar panel in panels)
            {
                IFLine[] lusasLines = new IFLine[panel.ExternalEdges.Count];
                List<Edge> edges = panel.ExternalEdges;

                for (int i = 0; i < panel.ExternalEdges.Count; i++)
                {
                    Edge correctEdge = distinctEdges[midpoints.FindIndex(m => m.Equals(edges[i].Curve.IPointAtParameter(0.5).ClosestPoint(midpoints)))];
                    lusasLines[i] = d_LusasData.getLineByName("L" + correctEdge.CustomData[AdapterId].ToString());
                }

                IFSurface newsurface = CreateSurface(panel, lusasLines);
            }

            return true;
        }

        /***************************************************/

        private bool CreateCollection(IEnumerable<Edge> edges)
        {
            List<Point> allPoints = new List<Point>();

            List<Edge> distinctEdges = edges.GroupBy(m => new
            {
                X = Math.Round(m.Curve.IPointAtParameter(0.5).X, 3),
                Y = Math.Round(m.Curve.IPointAtParameter(0.5).Y, 3),
                Z = Math.Round(m.Curve.IPointAtParameter(0.5).Z, 3)
            })
        .Select(x => x.First())
        .ToList();

            foreach (Edge edge in distinctEdges)
            {
                allPoints.Add(edge.Curve.IStartPoint());
                allPoints.Add(edge.Curve.IEndPoint());
            }

            List<Point> distinctPoints = allPoints.GroupBy(m => new
            {
                X = Math.Round(m.X, 3),
                Y = Math.Round(m.Y, 3),
                Z = Math.Round(m.Z, 3)
            })
                .Select(x => x.First())
                .ToList();

            List<Point> existingPoints = ReadPoints();
            List<Point> pointsToPush = distinctPoints.Except(existingPoints, new PointDistanceComparer()).ToList();

            foreach (Point point in pointsToPush)
            {
                IFPoint lusasPoint = CreatePoint(point);
            }

            List<IFPoint> lusasPoints = ReadLusasPoints();
            List<Point> bhomPoints = new List<Point>();
            
            foreach (IFPoint point in lusasPoints)
            {
                bhomPoints.Add(BH.Engine.Lusas.Convert.ToBHoMPoint(point));
            }

            List<String> edgesTags = distinctEdges.SelectMany(x => x.Tags).Distinct().ToList();

            foreach (String tag in edgesTags)
            {
                if (!d_LusasData.existsGroupByName(tag))
                {
                    d_LusasData.createGroup(tag);
                }
            }

            foreach (Edge edge in distinctEdges)
            {
                IFPoint startPoint = lusasPoints[bhomPoints.FindIndex(m => m.Equals(edge.Curve.IStartPoint().ClosestPoint(bhomPoints)))];
                IFPoint endPoint = lusasPoints[bhomPoints.FindIndex(m => m.Equals(edge.Curve.IEndPoint().ClosestPoint(bhomPoints)))];
                IFLine newline = CreateEdge(edge, startPoint, endPoint);
            }
            return true;
        }

        /***************************************************/

        private bool CreateCollection(IEnumerable<ISectionProperty> sectionProperties)
        {
            List<ISectionProperty> secPropList = sectionProperties.ToList();

            foreach (ISectionProperty secProp in secPropList)
            {
                IFGeometricLine attribute = d_LusasData.createGeometricLine("beam");
                attribute.setValue("elementType", "3D Thick Beam");
                attribute.setBeam(secProp.Area, secProp.Iy, secProp.Iz, 0, secProp.J, secProp.Asz, secProp.Asy, secProp.CentreY, secProp.CentreZ);
            }

            throw new NotImplementedException();
        }

        /***************************************************/

        private bool CreateCollection(IEnumerable<Material> materials)
        {
            foreach (Material material in materials)
            {
                IFAttribute newMaterial = CreateMaterial(material);
            }

            return true;
        }

        private bool CreateCollection(IEnumerable<Constraint6DOF> constraints)
        {
            foreach (Constraint6DOF constraint in constraints)
            {
                IFAttribute newSupport = CreateSupport(constraint);
            }

            return true;
        }
    }
}


    /***************************************************/
