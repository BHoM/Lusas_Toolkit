using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.oM.Structural.Elements;
using BH.oM.Structural.Properties;
using BH.oM.Common.Materials;
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
                Node startNode = bar.StartNode;
                IFPoint startPoint;

                if (d_LusasData.existsPointByName(startNode.Name))
                {
                    startPoint = d_LusasData.getPointByName(startNode.Name);
                }
                else
                {
                    IFGeometryData GeomData = m_LusasApplication.geometryData();
                    GeomData.setAllDefaults();
                    GeomData.addCoords(startNode.Position.X, startNode.Position.Y, startNode.Position.Z);
                    IFDatabaseOperations database_point = d_LusasData.createPoint(GeomData);
                    string nodeId = "ID" + startNode.Name;
                    startPoint = d_LusasData.getPointByNumber(d_LusasData.getLargestPointID());
                    startPoint.setName(nodeId);
                }

                Node endNode = bar.StartNode;
                IFPoint endPoint;

                if (d_LusasData.existsPointByName(endNode.Name))
                {
                    endPoint = d_LusasData.getPointByName(endNode.Name);
                }
                else
                {
                    IFGeometryData GeomData = m_LusasApplication.geometryData();
                    GeomData.setAllDefaults();
                    GeomData.addCoords(endNode.Position.X, endNode.Position.Y, endNode.Position.Z);
                    IFDatabaseOperations database_point = d_LusasData.createPoint(GeomData);
                    string nodeId = "ID" + endNode.Name;
                    endPoint = d_LusasData.getPointByNumber(d_LusasData.getLargestPointID());
                    endPoint.setName(nodeId);
                }

                IFLine newline = d_LusasData.createLineByPoints(startPoint, endPoint);
                newline.setName(bar.Name);
                //Tip: if the NextId method has been implemented you can get the id to be used for the creation out as (cast into applicable type used by the software):
                //object barId = bar.CustomData[AdapterId];
                //If also the default implmentation for the DependencyTypes is used,
                //one can from here get the id's of the subobjects by calling (cast into applicable type used by the software): 
                //object startNodeId = bar.StartNode.CustomData[AdapterId];
                //object endNodeId = bar.EndNode.CustomData[AdapterId];
                //object SecPropId = bar.SectionProperty.CustomData[AdapterId];

            }


             return true;
        }

        /***************************************************/

        private bool CreateCollection(IEnumerable<PanelPlanar> panels)
        {
            //Code for creating a collection of nodes in the software

            foreach (PanelPlanar panel in panels)
            {

            }

            return true; 
        }

        /***************************************************/


        private bool CreateCollection(IEnumerable<Node> nodes)
        {
            //Code for creating a collection of nodes in the software

            foreach (Node node in nodes)
            {
                IFGeometryData GeomData = m_LusasApplication.geometryData();
                GeomData.setAllDefaults();
                //Tip: if the NextId method has been implemented you can get the id to be used for the creation out as (cast into applicable type used by the software
                GeomData.addCoords(node.Position.X, node.Position.Y, node.Position.Z);
                IFDatabaseOperations database_point = d_LusasData.createPoint(GeomData);
                string nodeId = "ID" + node.Name;
                IFPoint newPoint = d_LusasData.getPointByNumber(d_LusasData.getLargestPointID());
                newPoint.setName(nodeId);
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
    }
}
