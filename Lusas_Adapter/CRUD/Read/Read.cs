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
            else if (type == typeof(ISectionProperty) || type.GetInterfaces().Contains(typeof(ISectionProperty)))
                return ReadSectionProperties(ids as dynamic);
            else if (type == typeof(Material))
                return ReadMaterials(ids as dynamic);
            else if (type == typeof(PanelPlanar))
                return ReadSurfaces(ids as dynamic);
            else if (type == typeof(Constraint6DOF))
                return ReadConstraint6DOF(ids as dynamic);
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
            HashSet<String> groupNames = ReadTags();
            IEnumerable<Constraint6DOF> constraints6DOFList = ReadConstraint6DOF();
            Dictionary<string, Constraint6DOF> constraints6DOF = constraints6DOFList.ToDictionary(x => x.Name.ToString());

            for (int i = 1; i <= maxlineid; i++)
            {
                if (d_LusasData.existsLineByID(i))
                {
                    IFLine lusasline = d_LusasData.getLineByNumber(i);
                    Bar bhomBar = BH.Engine.Lusas.Convert.ToBHoMObject(lusasline, bhomNodes, groupNames, constraints6DOF);
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
            HashSet<String> groupNames = ReadTags();
            IEnumerable<Constraint6DOF> constraints6DOFList = ReadConstraint6DOF();
            Dictionary<string, Constraint6DOF> constraints6DOF = constraints6DOFList.ToDictionary(x => x.Name.ToString());


            for (int i = 1; i <= maxSurfID; i++)
            {
                if (d_LusasData.existsSurfaceByID(i))
                {
                    IFSurface lusasSurface = d_LusasData.getSurfaceByNumber(i);
                    PanelPlanar bhompanel = BH.Engine.Lusas.Convert.ToBHoMObject(lusasSurface, bhomBars, bhomNodes, groupNames, constraints6DOF);
                    bhomSurfaces.Add(bhompanel);
                }
            }
            return bhomSurfaces;
        }

        private List<Node> ReadNodes(List<string> ids = null)
        {
            int maxPointID = d_LusasData.getLargestPointID();
            List<Node> bhomNodes = new List<Node>();
            HashSet<String> groupNames = ReadTags();

            IEnumerable<Constraint6DOF> constraints6DOFList = ReadConstraint6DOF();
            Dictionary<string, Constraint6DOF> constraints6DOF = constraints6DOFList.ToDictionary(x => x.Name.ToString());

            for (int i = 1; i <= maxPointID; i++)
            {
                if (d_LusasData.existsPointByID(i))
                {
                    IFPoint lusasPoint = d_LusasData.getPointByNumber(i);
                    Node bhomNode = BH.Engine.Lusas.Convert.ToBHoMObject(lusasPoint, groupNames,constraints6DOF);
                    bhomNodes.Add(bhomNode);
                }
            }
            return bhomNodes;
        }

        private HashSet<String> ReadTags(List<string> ids = null)
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

        private List<Constraint6DOF> ReadConstraint6DOF(List<String> ids = null)
        {
            List<Constraint6DOF> bhomConstraints6DOF = new List<Constraint6DOF>();

            int largestAttributeID = d_LusasData.getLargestAttributeID("Support");

            for(int i = 1; i <= largestAttributeID; i++)
            {
                if(d_LusasData.existsAttribute("Support",i))
                {
                    IFAttribute lusasSupport = d_LusasData.getAttribute("Support", i);
                    Constraint6DOF bhomConstraint6DOF = BH.Engine.Lusas.Convert.ToBHoMObject(lusasSupport);
                    bhomConstraints6DOF.Add(bhomConstraint6DOF);
                }
            }
            return bhomConstraints6DOF;
        }

        /***************************************/

        private List<ISectionProperty> ReadSectionProperties(List<string> ids = null)
        {
            //Implement code for reading section properties
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
