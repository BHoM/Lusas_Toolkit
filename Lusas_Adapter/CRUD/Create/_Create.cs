using System.Collections.Generic;
using System.Linq;
using System;
using BH.oM.Base;
using BH.oM.Structure.Elements;
using BH.oM.Geometry;
using BH.oM.Structure.Properties;
using BH.oM.Structure.Loads;
using BH.oM.Common.Materials;
using BH.Engine.Geometry;
using BH.Engine.Structure;
using Lusas.LPI;

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

            List<String> nodeTags = nodes.SelectMany(x => x.Tags).Distinct().ToList();

            foreach (String tag in nodeTags)
            {
                if (!d_LusasData.existsGroupByName(tag))
                {
                    d_LusasData.createGroup(tag);
                }
            }

            List<Constraint6DOF> nodeConstraints = nodes.Select(x => x.Constraint).Distinct().ToList();

            foreach (Constraint6DOF constraint in nodeConstraints)
            {
                if (!(constraint == null))
                {
                    if (!(d_LusasData.existsAttribute("Support", "Sp" + constraint.CustomData[AdapterId] + "/" + constraint.Name)))
                    {
                        IFAttribute lusasAttribute = CreateAttribute(constraint);
                    }
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

            List<Point> distinctPoints = points.GroupBy(m => new {
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

            List<String> barTags = bars.SelectMany(x => x.Tags).Distinct().ToList();

            foreach (String tag in barTags)
            {
                if (!d_LusasData.existsGroupByName(tag))
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

            List<String> barTags = panels.SelectMany(x => x.Tags).Distinct().ToList();

            foreach (String tag in barTags)
            {
                if (!d_LusasData.existsGroupByName(tag))
                {
                    d_LusasData.createGroup(tag);
                }
            }

            List<IFLine> lusasLines = ReadLusasEdges();

            foreach (PanelPlanar panel in panels)
            {
                IFSurface newsurface = CreateSurface(panel, lusasLines);
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

            List<Point> allPoints = new List<Point>();
            List<IFPoint> lusasPoints = new List<IFPoint>();

            foreach (Edge edge in distinctEdges)
            {
                allPoints.Add(edge.Curve.IStartPoint());
                allPoints.Add(edge.Curve.IEndPoint());
            }

            List<Point> distinctPoints = allPoints.GroupBy(m => new {
                X = Math.Round(m.X, 3),
                Y = Math.Round(m.Y, 3),
                Z = Math.Round(m.Z, 3)
            })
                .Select(x => x.First())
                .ToList();

            List<Point> existingPoints = ReadPoints();
            List<Edge> existingLines = ReadEdges();

            List<Edge> edgesToCreate = distinctEdges.Except(existingLines, new MidpointComparer()).ToList();
            List<Point> pointsToCreate = distinctPoints.Except(existingPoints).ToList();

            List<String> edgesTags = edges.SelectMany(x => x.Tags).Distinct().ToList();

            foreach (String tag in edgesTags)
            {
                if (!d_LusasData.existsGroupByName(tag))
                {
                    d_LusasData.createGroup(tag);
                }
            }

            foreach (Point point in pointsToCreate)
            {
                IFPoint lusasPoint = CreatePoint(point);
            }

            lusasPoints.AddRange(ReadLusasPoints());

            foreach (Edge edge in edgesToCreate)
            {
                IFLine newline = CreateEdge(edge, lusasPoints);
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

        public class MidpointComparer : IEqualityComparer<Edge>
        {

            #region IEqualityComparer<ThisClass> Members


            public bool Equals(Edge x, Edge y)
            {
                //no null check here, you might want to do that, or correct that to compare just one part of your object
                return x.Curve.IPointAtParameter(0.5) == y.Curve.IPointAtParameter(0.5);
            }


            public int GetHashCode(Edge obj)
            {
                unchecked
                {
                    var hash = 17;
                    //same here, if you only want to get a hashcode on a, remove the line with b
                    hash = hash * 23 + obj.Curve.IPointAtParameter(0.5).GetHashCode();
                    hash = hash * 23 + obj.Curve.IPointAtParameter(0.5).GetHashCode();

                    return hash;
                }
            }
        }

        #endregion
    }

    /***************************************************/

}