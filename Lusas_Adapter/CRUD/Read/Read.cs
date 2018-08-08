using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.oM.Base;
using BH.oM.Geometry;
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
        protected override IEnumerable<IBHoMObject> Read(Type type, IList ids = null)
        {
            //Choose what to pull out depending on the type. Also see example methods below for pulling out bars and dependencies
            if (type == typeof(Bar))
                return ReadBars(ids as dynamic);
            else if (type == typeof(Node))
                return ReadNodes(ids as dynamic);
            //else if (type == typeof(ISectionProperty) || type.GetInterfaces().Contains(typeof(ISectionProperty)))
            //    return ReadSectionProperties(ids as dynamic);
            else if (type == typeof(Material))
                return ReadMaterials(ids as dynamic);
            else if (type == typeof(PanelPlanar))
                return ReadSurfaces(ids as dynamic);
            else if (type == typeof(PanelPlanar))
                return ReadSurfaces(ids as dynamic);
            else if (type == typeof(Edge))
                return ReadEdges(ids as dynamic);
            else if (type == typeof(Point))
                return ReadPoints(ids as dynamic);
            return null;
        }

        /***************************************************/
        /**** Private specific read methods             ****/
        /***************************************************/

        //The List<string> in the methods below can be changed to a list of any type of identification more suitable for the toolkit

        private List<Bar> ReadBars(List<string> ids = null)
        {
            int maxlineid = d_LusasData.getLargestLineID();
            List<Bar> bhomBars = new List<Bar>();
            IEnumerable<Node> bhomNodesList = ReadNodes();
            Dictionary<string, Node> bhomNodes = bhomNodesList.ToDictionary(x => x.CustomData[AdapterId].ToString());

            HashSet<String> groupNames = ReadGroups();

            for (int i = 1; i <= maxlineid; i++)
            {
                if (d_LusasData.existsLineByID(i))
                {
                    IFLine lusasline = d_LusasData.getLineByNumber(i);
                    Bar bhomBar = BH.Engine.Lusas.Convert.ToBHoMObject(lusasline, bhomNodes, groupNames);
                    bhomBars.Add(bhomBar);
                }
            }
            return bhomBars;
        }

        /***************************************/


        private List<PanelPlanar> ReadSurfaces(List<string> ids = null)
        {
            int maxSurfID = d_LusasData.getLargestSurfaceID();
            List<PanelPlanar> bhomSurfaces = new List<PanelPlanar>();

            IEnumerable<Node> bhomNodesList = ReadNodes();
            Dictionary<string, Node> bhomNodes = bhomNodesList.ToDictionary(x => x.CustomData[AdapterId].ToString());
            IEnumerable<Bar> bhomBarsList = ReadBars();
            Dictionary<string, Bar> bhomBars = bhomBarsList.ToDictionary(x => x.CustomData[AdapterId].ToString());
            HashSet<String> groupNames = ReadGroups();

            for (int i = 1; i <= maxSurfID; i++)
            {
                if (d_LusasData.existsSurfaceByID(i))
                {
                    IFSurface lusasSurface = d_LusasData.getSurfaceByNumber(i);
                    PanelPlanar bhompanel = BH.Engine.Lusas.Convert.ToBHoMObject(lusasSurface, bhomBars, bhomNodes, groupNames);
                    bhomSurfaces.Add(bhompanel);
                }
            }
            return bhomSurfaces;
        }

        private List<Node> ReadNodes(List<string> ids = null)
        {
            int maxPointID = d_LusasData.getLargestPointID();
            List<Node> bhomNodes = new List<Node>();
            HashSet<String> groupNames = ReadGroups();

            for (int i = 1; i <= maxPointID; i++)
            {
                if (d_LusasData.existsPointByID(i))
                {
                    IFPoint lusasPoint = d_LusasData.getPointByNumber(i);
                    Node bhomNode = BH.Engine.Lusas.Convert.ToBHoMObject(lusasPoint, groupNames);
                    bhomNodes.Add(bhomNode);
                }
            }
            return bhomNodes;
        }

        private List<Point> ReadPoints(List<string> ids = null)
        {
            int maxPointID = d_LusasData.getLargestPointID();
            List<Point> bhomPoints = new List<Point>();
            HashSet<String> groupNames = ReadGroups();

            for (int i = 1; i <= maxPointID; i++)
            {
                if (d_LusasData.existsPointByID(i))
                {
                    IFPoint lusasPoint = d_LusasData.getPointByNumber(i);
                    Point bhomNode = BH.Engine.Lusas.Convert.ToBHoMGeom(lusasPoint, groupNames);
                    bhomPoints.Add(bhomNode);
                }
            }
            return bhomPoints;
        }

        private HashSet<String> ReadGroups(List<string> ids = null)
        {
            int numGroups = d_LusasData.countGroups();
            IFGroup lusasGroup = null;
            HashSet<String> bhomTags = new HashSet<string>();

            for (int i=0; i<numGroups; i++)
            {
                lusasGroup = d_LusasData.getObjects("Groups")[i];
                bhomTags.Add(lusasGroup.getName());
            }

            return bhomTags;
        }

        /***************************************/

        private List<Edge> ReadEdges(List<string> ids = null)
        {
            int maxlineid = d_LusasData.getLargestLineID();
            List<Edge> bhomEdges = new List<Edge>();
            List<Node> bhomNodesList = ReadNodes();
            Dictionary<string, Node> bhomNodes = bhomNodesList.ToDictionary(x => x.CustomData[AdapterId].ToString());
            HashSet<String> groupNames = ReadGroups();

            for (int i = 1; i <= maxlineid; i++)
            {
                if (d_LusasData.existsLineByID(i))
                {
                    IFLine lusasline = d_LusasData.getLineByNumber(i);
                    Edge bhomEdge = BH.Engine.Lusas.Convert.ToBHoMGeom(lusasline, bhomNodes, groupNames);
                    bhomEdges.Add(bhomEdge);

                }
            }
            return bhomEdges;
        }

        /***************************************/

        private List<ISectionProperty> ReadSectionProperties(List<string> ids = null)
        {
            //Implement code for reading section properties
            int largestSecID = d_LusasData.getLargestAttributeID("Geometric");

            for (int i = 0; i < largestSecID; i++)
            {
                IFAttribute lusasSecProp = d_LusasData.getAttribute("Geometric", i+1);
                object[] secPropNames1 = lusasSecProp.getValueNames();
                string[] secPropNames = ((IEnumerable)secPropNames1).Cast<object>()
                                 .Select(x => x.ToString())
                                 .ToArray();
                string lusasSecType = lusasSecProp.getValue("elementType",0);

                //if (lusasSecType == "I beam")
                //{
                //    ISectionProperty section = 
                //}

                //ISectionProperty bhomSection;
                //bhomSection.
                //bhomSection.Area = lusasSecProp.getValue()

                //foreach (string value in secPropValues)
                //{

                //}
            }

            throw new NotImplementedException();
        }

        /***************************************/

        private List<Material> ReadMaterials(List<string> ids = null)
        {
            //Implement code for reading materials
            throw new NotImplementedException();
        }

        /***************************************************/

            /***************************************************/
        }
    }
