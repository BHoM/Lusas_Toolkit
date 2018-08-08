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
                if (objects.First() is Edge)
                {
                    success = CreateCollection(objects as IEnumerable<Edge>);
                }
                if (objects.First() is Point)
                {
                    success = CreateCollection(objects as IEnumerable<Point>);
                }
                if (objects.First().GetType().GetInterfaces().Contains(typeof(ISectionProperty)))
                {
                    success = CreateCollection(objects as IEnumerable<ISectionProperty>);
                }
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

            List<String> barTags = nodes.SelectMany(x => x.Tags).Distinct().ToList();

            foreach (String tag in barTags)
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
            //Code for creating a collection of nodes in the software

            //List<String> barTags = points.SelectMany(x => x.Tags).Distinct().ToList();

            //foreach (String tag in barTags)
            //{
            //    if (!d_LusasData.existsGroupByName(tag))
            //    {
            //        d_LusasData.createGroup(tag);
            //    }
            //}

            List<Point> distinctPoints = points.GroupBy(m => new {
                X = Math.Round(m.X, 3),
                Y = Math.Round(m.Y, 3),
                Z = Math.Round(m.Z, 3)
            })
                 .Select(x => x.First())
                 .ToList();

            foreach (Point point in distinctPoints)
            {
                IFPoint newpoint = CreatePoint(point);
            }

            return true;
        }

        private bool CreateCollection(IEnumerable<Bar> bars)
        {
            //Code for creating a collection of bars in the software

            List<String> barTags = bars.SelectMany(x => x.Tags).Distinct().ToList();

            foreach(String tag in barTags)
            {
                if(!d_LusasData.existsGroupByName(tag))
                {
                    d_LusasData.createGroup(tag);
                }
            }
 
            List<Bar> existingLines = ReadBars();

            foreach (Bar bar in bars)
            {
                IFLine newline = CreateLine(bar, existingLines);
            }
             return true;
        }

        /***************************************************/

        private bool CreateCollection(IEnumerable<PanelPlanar> panels)
        {

            List<Node> existingNodes = ReadNodes();
            List<Edge> existingEdges = ReadEdges();

            //foreach (PanelPlanar panel in panels)
            //{
            //    allPoints.AddRange(panel.ControlPoints());
            //    allEdges.AddRange(panel.AllEdgeCurves());
            //}

            //List<Point> distinctPoints = allPoints.GroupBy(m => new {
            //    X = Math.Round(m.X,3),
            //    Y =Math.Round(m.Y,3),
            //    Z =Math.Round(m.Z,3) })
            //                 .Select(x => x.First())
            //                 .ToList();

            //List<ICurve> distinctEdges = allEdges.GroupBy(m => new {
            //    X = Math.Round(m.IPointAtParameter(0.5).X, 3),
            //    Y = Math.Round(m.IPointAtParameter(0.5).Y, 3),
            //    Z = Math.Round(m.IPointAtParameter(0.5).Z, 3) })
            //                .Select(x => x.First())
            //                .ToList();


            //List<IFPoint> lusasPoints = new List<IFPoint>();
            //foreach (Point point in distinctPoints)
            //{
            //    IFPoint newPoint = CreatePoint(point);
            //    lusasPoints.Add(newPoint);
            //}

            //List<IFLine> lusasLines = new List<IFLine>();

            //foreach (ICurve edge in distinctEdges)
            //{
            //        Point bhomStartPoint = edge.IStartPoint();
            //        Point bhomEndPoint = edge.IEndPoint();
            //        int startindex = distinctPoints.FindIndex(m =>
            //                Math.Round(m.X, 3).Equals(Math.Round(bhomStartPoint.X, 3)) &&
            //                Math.Round(m.Y, 3).Equals(Math.Round(bhomStartPoint.Y, 3)) &&
            //                Math.Round(m.Z, 3).Equals(Math.Round(bhomStartPoint.Z, 3)));
            //        int endindex = distinctPoints.FindIndex(m =>
            //                Math.Round(m.X, 3).Equals(Math.Round(bhomEndPoint.X, 3)) &&
            //                Math.Round(m.Y, 3).Equals(Math.Round(bhomEndPoint.Y, 3)) &&
            //                Math.Round(m.Z, 3).Equals(Math.Round(bhomEndPoint.Z, 3)));

            //        lusasLines.Add(CreateLine(edge, lusasPoints[startindex], lusasPoints[endindex]));
            //}

            List<String> barTags = panels.SelectMany(x => x.Tags).Distinct().ToList();

            foreach (String tag in barTags)
            {
                if (!d_LusasData.existsGroupByName(tag))
                {
                    d_LusasData.createGroup(tag);
                }
            }

            foreach (PanelPlanar panel in panels)
            {
                IFSurface newsurface = CreateSurface(panel, existingNodes, existingEdges);
            }

            return true; 
        }

        /***************************************************/

        private bool CreateCollection(IEnumerable<Edge> edges)
        {
            //Code for creating a collection of bars in the software

            List<Edge> distinctEdges = edges.GroupBy(m => new {
                X = Math.Round(m.Curve.IPointAtParameter(0.5).X, 3),
                Y = Math.Round(m.Curve.IPointAtParameter(0.5).Y, 3),
                Z = Math.Round(m.Curve.IPointAtParameter(0.5).Z, 3)
            })
                .Select(x => x.First())
                .ToList();

            List<String> edgesTags = edges.SelectMany(x => x.Tags).Distinct().ToList();

            foreach (String tag in edgesTags)
            {
                if (!d_LusasData.existsGroupByName(tag))
                {
                    d_LusasData.createGroup(tag);
                }
            }

            List<Edge> existingLines = ReadEdges();

            foreach (Edge edge in distinctEdges)
            {
                IFLine newline = CreateEdge(edge, existingLines);
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
            //Code for creating a collection of materials in the software

            foreach (Material material in materials)
            {
                //Tip: if the NextId method has been implemented you can get the id to be used for the creation out as (cast into applicable type used by the software):
                object materialId = material.CustomData[AdapterId];

            }

            throw new NotImplementedException();
        }

        /***************************************************/

        /***************************************************/
    }
}

