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

using LusasM15_2;

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

            if (objects.Count()>0)
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
            }

            //success = CreateCollection(objects as dynamic);
            m_LusasApplication.updateAllViews();

            return success;             //Finally return if the creation was successful or not

        }


        /***************************************************/
        /**** Private methods                           ****/
        /***************************************************/
        
        private bool CreateCollection(IEnumerable<Bar> bars)
        {
            //Code for creating a collection of bars in the software
  
            foreach (Bar bar in bars)
            {
                IFLine newline = createLine(bar);
            }
             return true;
        }

        //private bool CreateCollection(IEnumerable<Edge> edges)
        //{
        //    //Code for creating a collection of bars in the software

        //    foreach (Edge edge in edges)
        //    {
        //        IFLine newline = createEdge(edge);
        //    }
        //    return true;
        //}

        /***************************************************/

        private bool CreateCollection(IEnumerable<PanelPlanar> panels)
        {

            List<Point> allPoints = new List<Point>();
            List<ICurve> allEdges = new List<ICurve>();
            List<double> coordsum = new List<double>();
            List<IFPoint> LusasPoints = new List<IFPoint>();
            List<IFLine> LusasLines = new List<IFLine>();

            foreach (PanelPlanar panel in panels)
            {
                allPoints.AddRange(panel.ControlPoints());
                allEdges.AddRange(panel.AllEdgeCurves());
            }

            List<Point> distinctPoints = allPoints.GroupBy(m => new { X = Math.Round(m.X,3), Y=Math.Round(m.Y,3), Z=Math.Round(m.Z,3) })
                             .Select(x => x.First())
                             .ToList();

            List<ICurve> distinctEdges = allEdges.GroupBy(m => new { X = Math.Round(m.IPointAtParameter(0.5).X, 3), Y = Math.Round(m.IPointAtParameter(0.5).Y, 3), Z = Math.Round(m.IPointAtParameter(0.5).Z, 3) })
                            .Select(x => x.First())
                            .ToList();

            foreach (Point point in distinctPoints)
            {
                Node newnode = PointToNode(point);
                LusasPoints.Add(existsPoint(newnode));
            }

            foreach (ICurve edge in distinctEdges)
            {
                Point bhomStartPoint = edge.IStartPoint();
                Point bhomEndPoint = edge.IEndPoint();
                int startindex = distinctPoints.FindIndex(m => Math.Round(m.X,3).Equals(Math.Round(bhomStartPoint.X, 3)) &&
                        Math.Round(m.Y,3).Equals(Math.Round(bhomStartPoint.Y, 3)) &&
                        Math.Round(m.Z,3).Equals(Math.Round(bhomStartPoint.Z, 3)));
                int endindex = distinctPoints.FindIndex(m => Math.Round(m.X,3).Equals(Math.Round(bhomEndPoint.X, 3)) &&
        Math.Round(m.Y,3).Equals(Math.Round(bhomEndPoint.Y, 3)) &&
        Math.Round(m.Z,3).Equals(Math.Round(bhomEndPoint.Z, 3)));
                Bar bhomBar = LineToBar(PointToNode(bhomStartPoint), PointToNode(bhomEndPoint));
                LusasLines.Add(existsLine(bhomBar, LusasPoints[startindex], LusasPoints[endindex]));
            }

            foreach (PanelPlanar panel in panels)
            {
                IFSurface newsurface = createSurface(panel, distinctPoints, LusasPoints, distinctEdges, LusasLines);
            }

            return true; 
        }

        /***************************************************/

        private bool CreateCollection(IEnumerable<Node> nodes)
        {
            //Code for creating a collection of nodes in the software

            foreach (Node node in nodes)
            {
                IFPoint newpoint = createPoint(node);
            }

            return true;
        }

        /***************************************************/

        private bool CreateCollection(IEnumerable<ISectionProperty> sectionProperties)
        {
            //Code for creating a collection of section properties in the software

            foreach (ISectionProperty sectionProperty in sectionProperties)
            {
                //Tip: if the NextId method has been implemented you can get the id to be used for the creation out as (cast into applicable type used by the software):
                object secPropId = sectionProperty.CustomData[AdapterId];
                //If also the default implmentation for the DependencyTypes is used,
                //one can from here get the id's of the subobjects by calling (cast into applicable type used by the software): 
                object materialId = sectionProperty.Material.CustomData[AdapterId];
            }

            throw new NotImplementedException();
        }

        /***************************************************/

        private bool CreateCollection(IEnumerable<Material> materials)
        {
            //Code for creating a collection of materials in the software

            foreach (Material material in materials)
            {
                //Tip: if the NextId method has been implemented you can get the id to be used for the creation out as (cast into applicable type used by the software):
                object materialId = material.CustomData[AdapterId];

            }

            throw new NotImplementedException();
        }


        /***************************************************/
        
        public IFPoint createPoint(Node node)
        {
            IFGeometryData geomData = m_LusasApplication.geometryData();
            geomData.setAllDefaults();
            geomData.addCoords(node.Position.X, node.Position.Y, node.Position.Z);
            IFDatabaseOperations database_point = d_LusasData.createPoint(geomData);
            IFPoint newPoint = d_LusasData.getPointByNumber(d_LusasData.getLargestPointID());
            newPoint.setName("P"+ node.CustomData[AdapterId].ToString());
            return newPoint;
        }

        public IFLine createLine(Bar bar)
        {
            IFPoint startPoint = existsPoint(bar.StartNode);
            IFPoint endPoint = existsPoint(bar.EndNode);
            IFLine newLine = d_LusasData.createLineByPoints(startPoint, endPoint);
            newLine.setName("L" + bar.CustomData[AdapterId]);
            return newLine;
        }

        //public IFLine createEdge(Edge edge)
        //{
        //    IFPoint startPoint = createPoint(PointToNode(edge.Curve.IStartPoint()));
        //    IFPoint endPoint = createPoint(PointToNode(edge.Curve.IEndPoint()));
        //    IFLine newLine = d_LusasData.createLineByPoints(startPoint, endPoint);
        //    newLine.setName("L" + edge.CustomData[AdapterId]);
        //    return newLine;
        //}

        public IFSurface createSurface(PanelPlanar panel, List<Point> distinctPoints, List<IFPoint> LusasPoints, List<ICurve> distinctEdges, List<IFLine> LusasLines)
        {
            IFObjectSet LusasGroup = d_LusasData.createGroup("temp");

            List<ICurve> panelcurves = panel.AllEdgeCurves();

            foreach (ICurve edge in panel.AllEdgeCurves())
            {
                int edgeindex = distinctEdges.FindIndex(m => Math.Round(m.IPointAtParameter(0.5).X, 3).Equals(Math.Round(edge.IPointAtParameter(0.5).X, 3)) &&
                            Math.Round(m.IPointAtParameter(0.5).Y, 3).Equals(Math.Round(edge.IPointAtParameter(0.5).Y, 3)) &&
                           Math.Round(m.IPointAtParameter(0.5).Z, 3).Equals(Math.Round(edge.IPointAtParameter(0.5).Z, 3)));

                LusasGroup.add(LusasLines[edgeindex]);
            }

            IFSurface lusasSurface =existsSurface(panel,LusasGroup);
            d_LusasData.getGroupByName("temp").ungroup();
            return lusasSurface;
        }
       

        public IFPoint existsPoint(Node node)
        {
            IFPoint newPoint;

            int bhomID;
            if (node.CustomData.ContainsKey(AdapterId))
                bhomID = System.Convert.ToInt32(node.CustomData[AdapterId]);
            else
                bhomID = System.Convert.ToInt32(NextId(node.GetType()));

            node.CustomData[AdapterId] = bhomID;

            if (d_LusasData.existsPointByName("P"+node.CustomData[AdapterId]))
            {
                newPoint = d_LusasData.getPointByName("P" + node.CustomData[AdapterId]);
            }
            else
            {
                newPoint = createPoint(node);
            }

            return newPoint;
        }

        public IFLine existsLine(Bar bar, IFPoint startPoint, IFPoint endPoint)
        {
            IFLine newLine;

            int bhomID;
            if (bar.CustomData.ContainsKey(AdapterId))
                bhomID = System.Convert.ToInt32(bar.CustomData[AdapterId]);
            else
                bhomID = System.Convert.ToInt32(NextId(bar.GetType()));

            bar.CustomData[AdapterId] = bhomID;

            if (d_LusasData.existsLineByName("L" + bar.CustomData[AdapterId]))
            {
                newLine = d_LusasData.getLineByName("L" + bar.CustomData[AdapterId]);
            }
            else
            {
                newLine = d_LusasData.createLineByPoints(startPoint,endPoint);
                newLine.setName("L" + bar.CustomData[AdapterId]);
                return newLine;
            }

            return newLine;
        }

        public IFSurface existsSurface(PanelPlanar panel, IFObjectSet panelLines)
        {

            IFSurface newSurface;
            if (d_LusasData.existsSurfaceByName("S" + panel.CustomData[AdapterId]))
            {
                newSurface = d_LusasData.getSurfaceByName("S" + panel.CustomData[AdapterId]);
            }
            else
            {
                newSurface = d_LusasData.createSurfaceBy(panelLines);
                newSurface.setName("S" + panel.CustomData[AdapterId]);
                return newSurface;
            }

            return newSurface;
        }

        public Node PointToNode(Point point)
        {

            Node NewNode = new Node { Position = { X = point.X, Y = point.Y, Z = point.Z } };

            return NewNode;
        }

        public Bar LineToBar(Node startNode, Node endNode)
        {
            Bar NewBar = new Bar { StartNode = startNode, EndNode = endNode };

            return NewBar;
        }

    }
}

