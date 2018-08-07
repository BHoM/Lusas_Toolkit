﻿using System.Collections.Generic;
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
                if (!(d_LusasData.existsAttribute("Support",constraint.Name)))
                {
                    IFAttribute lusasAttribute = CreateAttribute(constraint);
                }
            }

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


            foreach (Bar bar in bars)
            {
                IFLine newline = CreateLine(bar);
            }
             return true;
        }

        /***************************************************/

        private bool CreateCollection(IEnumerable<PanelPlanar> panels)
        {

            List<Point> allPoints = new List<Point>();
            List<ICurve> allEdges = new List<ICurve>();

            foreach (PanelPlanar panel in panels)
            {
                allPoints.AddRange(panel.ControlPoints());
                allEdges.AddRange(panel.AllEdgeCurves());
            }

            List<Point> distinctPoints = allPoints.GroupBy(m => new {
                X = Math.Round(m.X,3),
                Y =Math.Round(m.Y,3),
                Z =Math.Round(m.Z,3) })
                             .Select(x => x.First())
                             .ToList();

            List<ICurve> distinctEdges = allEdges.GroupBy(m => new {
                X = Math.Round(m.IPointAtParameter(0.5).X, 3),
                Y = Math.Round(m.IPointAtParameter(0.5).Y, 3),
                Z = Math.Round(m.IPointAtParameter(0.5).Z, 3) })
                            .Select(x => x.First())
                            .ToList();


            List<IFPoint> lusasPoints = new List<IFPoint>();
            foreach (Point point in distinctPoints)
            {
                IFPoint newPoint = CreatePoint(point);
                lusasPoints.Add(newPoint);
            }

            List<IFLine> lusasLines = new List<IFLine>();
            foreach (ICurve edge in distinctEdges)
            {
                Point bhomStartPoint = edge.IStartPoint();
                Point bhomEndPoint = edge.IEndPoint();
                int startindex = distinctPoints.FindIndex(m => 
                        Math.Round(m.X,3).Equals(Math.Round(bhomStartPoint.X, 3)) &&
                        Math.Round(m.Y,3).Equals(Math.Round(bhomStartPoint.Y, 3)) &&
                        Math.Round(m.Z,3).Equals(Math.Round(bhomStartPoint.Z, 3)));
                int endindex = distinctPoints.FindIndex(m => 
                        Math.Round(m.X,3).Equals(Math.Round(bhomEndPoint.X, 3)) &&
                        Math.Round(m.Y,3).Equals(Math.Round(bhomEndPoint.Y, 3)) &&
                        Math.Round(m.Z,3).Equals(Math.Round(bhomEndPoint.Z, 3)));

                lusasLines.Add(CreateLine(edge, lusasPoints[startindex], lusasPoints[endindex]));
            }

            List<String> panelPlanarTags = panels.SelectMany(x => x.Tags).Distinct().ToList();

            foreach (String tag in panelPlanarTags)
            {
                if (!d_LusasData.existsGroupByName(tag))
                {
                    d_LusasData.createGroup(tag);
                }
            }

            foreach (PanelPlanar panel in panels)
            {
                IFSurface newsurface = CreateSurface(panel, distinctPoints, lusasPoints, distinctEdges, lusasLines);
            }

            return true; 
        }

        /***************************************************/


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
    }
}

