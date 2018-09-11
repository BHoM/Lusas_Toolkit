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
using Lusas.LPI;

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
            else if (type == typeof(Edge))
                return ReadEdges(ids as dynamic);
            else if (type == typeof(Point))
                return ReadPoints(ids as dynamic);
            else if (type == typeof(Constraint6DOF))
                return ReadConstraint6DOFs(ids as dynamic);
            return null;
        }

        /***************************************************/
        /**** Private specific read methods             ****/
        /***************************************************/

        //The List<string> in the methods below can be changed to a list of any type of identification more suitable for the toolkit

        private List<Bar> ReadBars(List<string> ids = null)
        {
            IFObjectSet selection = m_LusasApplication.getVisibleSet();
            object[] eleArray = selection.getObjects("Line");

            List<Bar> bhomBars = new List<Bar>();
            IEnumerable<Node> bhomNodesList = ReadNodes();
            Dictionary<string, Node> bhomNodes = bhomNodesList.ToDictionary(x => x.CustomData[AdapterId].ToString());
            IEnumerable<Material> materialList = ReadMaterials();
            Dictionary<string, Material> materials = materialList.ToDictionary(x => x.Name.ToString());
            HashSet<String> groupNames = ReadGroups();

            for (int i = 0; i < eleArray.Count(); i++)
            {
                    IFLine lusasLine = (IFLine) eleArray[i];
                    Bar bhomBar = BH.Engine.Lusas.Convert.ToBHoMBar(lusasLine, bhomNodes, groupNames,materials);

                    bhomBars.Add(bhomBar);
            }
            return bhomBars;
        }

        /***************************************/


        private List<PanelPlanar> ReadSurfaces(List<string> ids = null)
        {
            IFObjectSet selection = m_LusasApplication.getVisibleSet();
            object[] eleArray = selection.getObjects("Surface");
            List<PanelPlanar> bhomSurfaces = new List<PanelPlanar>();

            if (eleArray.Count()!=0)
            {
                IEnumerable<Edge> bhomEdgesList = ReadEdges();
                Dictionary<string, Edge> bhomEdges = bhomEdgesList.ToDictionary(x => x.CustomData[AdapterId].ToString());
                HashSet<String> groupNames = ReadGroups();
                IEnumerable<Material> materialList = ReadMaterials();
                Dictionary<string, Material> materials = materialList.ToDictionary(x => x.Name.ToString());

                for (int i = 0; i < eleArray.Count(); i++)
                {
                    IFSurface lusasSurface = (IFSurface)eleArray[i];
                    PanelPlanar bhompanel = BH.Engine.Lusas.Convert.ToBHoMPanelPlanar(lusasSurface,
                        bhomEdges,
                        groupNames,
                        materials);

                    bhomSurfaces.Add(bhompanel);
                }
            }

            return bhomSurfaces;
        }

        private List<Node> ReadNodes(List<string> ids = null)
        {
            IFObjectSet selection = m_LusasApplication.getVisibleSet();
            object[] eleArray = selection.getObjects("Point");

            List<Node> bhomNodes = new List<Node>();
            HashSet<String> groupNames = ReadGroups();

            IEnumerable<Constraint6DOF> constraints6DOFList = ReadConstraint6DOFs();
            Dictionary<string, Constraint6DOF> constraints6DOF = constraints6DOFList.ToDictionary(x => x.Name.ToString());

            for (int i = 0; i < eleArray.Count(); i++)
            {
                IFPoint lusasPoint = (IFPoint)eleArray[i];
                Node bhomNode = BH.Engine.Lusas.Convert.ToBHoMNode(lusasPoint, groupNames, constraints6DOF);
                bhomNodes.Add(bhomNode);
            }
            return bhomNodes;
        }

        private List<Point> ReadPoints(List<string> ids = null)
        {
            IFObjectSet selection = m_LusasApplication.getVisibleSet();
            object[] eleArray = selection.getObjects("Point");

            List<Point> bhomPoints = new List<Point>();
            HashSet<String> groupNames = ReadGroups();

            for (int i = 0; i < eleArray.Count(); i++)
            {
                IFPoint lusasPoint = (IFPoint)eleArray[i];
                Point bhomNode = BH.Engine.Lusas.Convert.ToBHoMPoint(lusasPoint, groupNames);
                bhomPoints.Add(bhomNode);
            }
            return bhomPoints;
        }

        private List<IFPoint> ReadLusasPoints(List<string> ids = null)
        {
            IFObjectSet selection = m_LusasApplication.getVisibleSet();
            object[] eleArray = selection.getObjects("Point");

            List<IFPoint> lusasPoints = new List<IFPoint>();

            for (int i = 0; i <= eleArray.Count(); i++)
            {
                IFPoint lusasPoint = (IFPoint)eleArray[i];
                lusasPoints.Add(lusasPoint);
            }
            return lusasPoints;
        }

        private HashSet<String> ReadGroups(List<string> ids = null)
        {
            IFObjectSet selection = m_LusasApplication.getVisibleSet();
            object[] eleArray = selection.getObjects("Groups");
            HashSet<String> bhomTags = new HashSet<string>();

            for (int i = 0; i < eleArray.Count(); i++)
            {
                IFGroup lusasGroup = (IFGroup)eleArray[i];
                bhomTags.Add(lusasGroup.getName());
            }

            return bhomTags;
        }

        /***************************************/

        private List<Edge> ReadEdges(List<string> ids = null)
        {
            IFObjectSet selection = m_LusasApplication.getVisibleSet();
            object[] eleArray = selection.getObjects("Line");
            List<Edge> bhomEdges = new List<Edge>();

            if(eleArray.Count()!=0)
            {
                List<Node> bhomNodesList = ReadNodes();
                Dictionary<string, Node> bhomNodes = bhomNodesList.ToDictionary(x => x.CustomData[AdapterId].ToString());
                HashSet<String> groupNames = ReadGroups();

                for (int i = 0; i < eleArray.Count(); i++)
                {
                    IFLine lusasline = (IFLine)eleArray[i];
                    Edge bhomEdge = BH.Engine.Lusas.Convert.ToBHoMEdge(lusasline, bhomNodes, groupNames);
                    bhomEdges.Add(bhomEdge);
                }
            }

            return bhomEdges;
        }


        private List<IFLine> ReadLusasEdges(List<string> ids = null)
        {
            IFObjectSet selection = m_LusasApplication.getVisibleSet();
            object[] eleArray = selection.getObjects("Line");
            List<IFLine> lusasEdges = new List<IFLine>();

            for (int i = 0; i < eleArray.Count(); i++)
            {
                    IFLine lusasline = d_LusasData.getLineByNumber(i);
                    lusasEdges.Add(lusasline);
            }

            return lusasEdges;
        }

        /***************************************/

        private List<Constraint6DOF> ReadConstraint6DOFs(List<String> ids = null)
        {
            IFObjectSet selection = m_LusasApplication.getVisibleSet();
            object[] eleArray = selection.getObjects("Support");
            List<Constraint6DOF> bhomConstraints6DOF = new List<Constraint6DOF>();

            for (int i = 0; i < eleArray.Count(); i++)
            {
                    IFAttribute lusasSupport = (IFAttribute) eleArray[i];
                    Constraint6DOF bhomConstraint6DOF = BH.Engine.Lusas.Convert.ToBHoMConstraint6DOF(lusasSupport);
                    bhomConstraints6DOF.Add(bhomConstraint6DOF);
            }

            return bhomConstraints6DOF;
        }

        /***************************************/

        private List<ISectionProperty> ReadSectionProperties(List<string> ids = null)
        {
            //Implement code for reading section properties
            int largestSecID = d_LusasData.getLargestAttributeID("Geometric");

            for (int i = 0; i < largestSecID; i++)
            {
                IFAttribute lusasSecProp = d_LusasData.getAttribute("Geometric", i + 1);
                object[] secPropNames1 = lusasSecProp.getValueNames();
                string[] secPropNames = ((IEnumerable)secPropNames1).Cast<object>()
                                 .Select(x => x.ToString())
                                 .ToArray();
                string lusasSecType = lusasSecProp.getValue("elementType", 0);

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
            IFObjectSet selection = m_LusasApplication.getVisibleSet();
            object[] eleArray = selection.getObjects("Material");
            List<Material> bhomMaterials = new List<Material>();

            for (int i = 0; i < eleArray.Count(); i++)
            {
                    IFAttribute lusasMaterial = (IFAttribute)eleArray[i];
                    Material bhomMaterial = BH.Engine.Lusas.Convert.ToBHoMMaterial(lusasMaterial);
                    bhomMaterials.Add(bhomMaterial);
            }

            return bhomMaterials;
        }

        /***************************************************/

        /***************************************************/
    }
}
