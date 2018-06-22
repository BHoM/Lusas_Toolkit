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
                IFLine newline = createline(bar);
            }
             return true;
        }

        /***************************************************/

        private bool CreateCollection(IEnumerable<PanelPlanar> panels)
        {
            //Code for creating a collection of nodes in the software

            foreach (PanelPlanar panel in panels)
            {
                IFSurface newsurface = createsurface(panel);
            }

            return true; 
        }

        /***************************************************/

        private bool CreateCollection(IEnumerable<Node> nodes)
        {
            //Code for creating a collection of nodes in the software

            foreach (Node node in nodes)
            {
                IFPoint newpoint = createpoint(node);
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
        
        public IFPoint createpoint(Node node)
        {
            IFGeometryData GeomData = m_LusasApplication.geometryData();
            GeomData.setAllDefaults();
            GeomData.addCoords(node.Position.X, node.Position.Y, node.Position.Z);
            IFDatabaseOperations database_point = d_LusasData.createPoint(GeomData);
            IFPoint newPoint = d_LusasData.getPointByNumber(d_LusasData.getLargestPointID());
            int bhid = System.Convert.ToInt32(node.CustomData[AdapterId]);
            newPoint.setName("P"+bhid.ToString());
            return newPoint;
        }

        public IFLine createline(Bar bar)
        {

            IFPoint startPoint = existsPoint(bar.StartNode);
            IFPoint endPoint = existsPoint(bar.EndNode);
            IFLine newline = d_LusasData.createLineByPoints(startPoint, endPoint);
            newline.setName("L" + bar.CustomData[AdapterId]);
            return newline;
        }

        public IFSurface createsurface(PanelPlanar panel)
        {
            IFGeometryData bhomedges = m_LusasApplication.geometryData();
            List<ICurve> panellines = new List<ICurve>();

            foreach (Edge edge in panel.ExternalEdges)
            {
                panellines.AddRange(Query.ISubParts(edge.Curve).ToList());
            }

            IFSurface lusassurface = d_LusasData.createSurfaceBy(panellines);
            int bhid = System.Convert.ToInt32(panel.CustomData[AdapterId]);
            lusassurface.setName("S" + bhid.ToString());

           return lusassurface;
        }

        public IFPoint existsPoint(Node node)
        {
            IFPoint newPoint;
            if (d_LusasData.existsPointByName("P"+node.CustomData[AdapterId]))
            {
                newPoint = d_LusasData.getPointByName("P" + node.CustomData[AdapterId]);
            }
            else
            {
                newPoint = createpoint(node);
            }

            return newPoint;
        }

        //public IFPoint existsSurface(PanelPlanar panel)
        //{
        //    IFPoint newPoint;
        //    if (d_LusasData.existsPointByName(node.Name))
        //    {
        //        newPoint = d_LusasData.getPointByName(node.Name);
        //    }
        //    else
        //    {
        //        newPoint = createpoint(node);
        //    }

        //    return newPoint;
        //}
    }
}

