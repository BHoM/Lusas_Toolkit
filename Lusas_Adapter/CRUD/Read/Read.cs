using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.oM.Base;
using BH.oM.Geometry;
using BH.oM.Structure.Elements;
using BH.oM.Structure.Properties;
using BH.oM.Structure.Loads;
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
            else if (type == typeof(Loadcase))
                return ReadLoadcases(ids as dynamic);
            else if (type ==typeof(PointForce))
                return ReadPointForce(ids as dynamic);
            else if (typeof(IProperty2D).IsAssignableFrom(type))
                return ReadProperty2D(ids as dynamic);
            return null;
        }

        /***************************************************/
        /**** Private specific read methods             ****/
        /***************************************************/

        //The List<string> in the methods below can be changed to a list of any type of identification more suitable for the toolkit

        private List<Bar> ReadBars(List<string> ids = null)
        {
            object[] lusasLines = d_LusasData.getObjects("Line");

            List<Bar> bhomBars = new List<Bar>();
            IEnumerable<Node> bhomNodesList = ReadNodes();
            Dictionary<string, Node> bhomNodes = bhomNodesList.ToDictionary(x => x.CustomData[AdapterId].ToString());
            IEnumerable<Material> materialList = ReadMaterials();
            Dictionary<string, Material> materials = materialList.ToDictionary(x => x.Name.ToString());
            HashSet<String> groupNames = ReadGroups();

            for (int i = 0; i < lusasLines.Count(); i++)
            {
                IFLine lusasLine = (IFLine)lusasLines[i];
                Bar bhomBar = BH.Engine.Lusas.Convert.ToBHoMBar(lusasLine, bhomNodes, groupNames, materials);

                bhomBars.Add(bhomBar);
            }
            return bhomBars;
        }

        /***************************************/


        private List<PanelPlanar> ReadSurfaces(List<string> ids = null)
        {
            object[] eleArray = d_LusasData.getObjects("Surface");
            List<PanelPlanar> bhomSurfaces = new List<PanelPlanar>();

            if (eleArray.Count() != 0)
            {
                IEnumerable<Edge> bhomEdgesList = ReadEdges();
                Dictionary<string, Edge> bhomEdges = bhomEdgesList.ToDictionary(x => x.CustomData[AdapterId].ToString());
                HashSet<String> groupNames = ReadGroups();
                IEnumerable<Material> materialList = ReadMaterials();
                Dictionary<string, Material> materials = materialList.ToDictionary(x => x.Name.ToString());
                IEnumerable<IProperty2D> geometricList = ReadProperty2D();
                Dictionary<string, IProperty2D> geometrics = geometricList.ToDictionary(x => x.Name.ToString());

                for (int i = 0; i < eleArray.Count(); i++)
                {
                    IFSurface lusasSurface = (IFSurface)eleArray[i];
                    PanelPlanar bhompanel = BH.Engine.Lusas.Convert.ToBHoMPanelPlanar(lusasSurface,
                        bhomEdges,
                        groupNames,
                        geometrics,
                        materials);

                    bhomSurfaces.Add(bhompanel);
                }
            }

            return bhomSurfaces;
        }

        private List<Node> ReadNodes(List<string> ids = null)
        {
            object[] lusasPoints = d_LusasData.getObjects("Point");

            List<Node> bhomNodes = new List<Node>();
            HashSet<String> groupNames = ReadGroups();

            IEnumerable<Constraint6DOF> constraints6DOFList = ReadConstraint6DOFs();
            Dictionary<string, Constraint6DOF> constraints6DOF = constraints6DOFList.ToDictionary(x => x.Name.ToString());

            for (int i = 0; i < lusasPoints.Count(); i++)
            {
                IFPoint lusasPoint = (IFPoint)lusasPoints[i];
                Node bhomNode = BH.Engine.Lusas.Convert.ToBHoMNode(lusasPoint, groupNames, constraints6DOF);
                bhomNodes.Add(bhomNode);
            }
            return bhomNodes;
        }

        private List<Point> ReadPoints(List<string> ids = null)
        {
            object[] lusasPoints = d_LusasData.getObjects("Point");

            List<Point> bhomPoints = new List<Point>();
            HashSet<String> groupNames = ReadGroups();

            for (int i = 0; i < lusasPoints.Count(); i++)
            {
                IFPoint lusasPoint = (IFPoint)lusasPoints[i];
                Point bhomPoint = BH.Engine.Lusas.Convert.ToBHoMPoint(lusasPoint, groupNames);
                bhomPoints.Add(bhomPoint);
            }
            return bhomPoints;
        }

        private List<IFPoint> ReadLusasPoints(List<string> ids = null)
        {
            object[] pointArray = d_LusasData.getObjects("Point");

            List<IFPoint> lusasPoints = new List<IFPoint>();

            for (int i = 0; i < pointArray.Count(); i++)
            {
                IFPoint lusasPoint = (IFPoint)pointArray[i];
                lusasPoints.Add(lusasPoint);
            }
            return lusasPoints;
        }

        private HashSet<String> ReadGroups(List<string> ids = null)
        {
            object[] eleArray = d_LusasData.getObjects("Groups");
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
            object[] lusasLines = d_LusasData.getObjects("Line");
            List<Edge> bhomEdges = new List<Edge>();

            if (lusasLines.Count() != 0)
            {
                List<Node> bhomNodesList = ReadNodes();
                Dictionary<string, Node> bhomNodes = bhomNodesList.ToDictionary(x => x.CustomData[AdapterId].ToString());
                HashSet<String> groupNames = ReadGroups();

                for (int i = 0; i < lusasLines.Count(); i++)
                {
                    IFLine lusasline = (IFLine)lusasLines[i];
                    Edge bhomEdge = BH.Engine.Lusas.Convert.ToBHoMEdge(lusasline, bhomNodes, groupNames);
                    bhomEdges.Add(bhomEdge);
                }
            }

            return bhomEdges;
        }

        private List<IFLine> ReadLusasEdges(List<string> ids = null)
        {
            object[] lusasLines = d_LusasData.getObjects("Line");
            List<IFLine> lusasEdges = new List<IFLine>();

            for (int i = 0; i < lusasLines.Count(); i++)
            {
                IFLine lusasline = d_LusasData.getLineByNumber(i);
                lusasEdges.Add(lusasline);
            }
            return lusasEdges;
        }

        /***************************************/

        private List<Constraint6DOF> ReadConstraint6DOFs(List<String> ids = null)
        {
            object[] lusasSupports = d_LusasData.getAttributes("Support");
            List<Constraint6DOF> bhomConstraints6DOF = new List<Constraint6DOF>();

            for (int i = 0; i < lusasSupports.Count(); i++)
            {
                IFSupportStructural lusasSupport = (IFSupportStructural)lusasSupports[i];
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
            object[] lusasMaterials = d_LusasData.getAttributes("Material");
            List<Material> bhomMaterials = new List<Material>();

            for (int i = 0; i < lusasMaterials.Count(); i++)
            {
                IFAttribute lusasMaterial = (IFAttribute)lusasMaterials[i];
                Material bhomMaterial = BH.Engine.Lusas.Convert.ToBHoMMaterial(lusasMaterial);
                bhomMaterials.Add(bhomMaterial);
            }

            return bhomMaterials;
        }

        /***************************************************/

        private List<Loadcase> ReadLoadcases(List<string> ids = null)
        {
            List<Loadcase> bhomLoadcases = new List<Loadcase>();
            object[] allLoadcases = d_LusasData.getLoadsets("loadcase", "all");

            for (int i = 0; i < allLoadcases.Count(); i++)
            {
                IFLoadcase lusasLoadcase = (IFLoadcase)allLoadcases[i];
                Loadcase bhomLoadcase = BH.Engine.Lusas.Convert.ToBHoMLoadcase(lusasLoadcase);
                List<string> analysisName = new List<string> { lusasLoadcase.getAnalysis().getName() };
                bhomLoadcase.Tags = new HashSet<string>(analysisName);
                bhomLoadcases.Add(bhomLoadcase);
            }

            return bhomLoadcases;
        }

        /***************************************************/
        private List<IProperty2D> ReadProperty2D(List<string> ids = null)
        {
            object[] lusasThicknesses = d_LusasData.getAttributes("Surface Geometric");
            List<IProperty2D> bhomProperties2D = new List<IProperty2D>();

            for (int i = 0; i < lusasThicknesses.Count(); i++)
            {
                IFAttribute lusasThickness = (IFAttribute)lusasThicknesses[i];
                string attributeType = lusasThickness.getAttributeType();
                string subType = lusasThickness.getSubType();
                Type type = lusasThickness.GetType();
                IProperty2D bhomProperty2D = BH.Engine.Lusas.Convert.ToBHoMProperty2D(lusasThickness);
                bhomProperties2D.Add(bhomProperty2D);
            }

            return bhomProperties2D;
        }

        /***************************************************/

        private List<PointForce> ReadPointForce(List<string> ids = null)
        {
            List<PointForce> bhomPointForces = new List<PointForce>();
            object[] lusasPointForces = d_LusasData.getAttributes("Concentrated Load");

            HashSet<String> groupNames = ReadGroups();
            IEnumerable<Constraint6DOF> constraints6DOFList = ReadConstraint6DOFs();
            Dictionary<string, Constraint6DOF> constraints6DOF = constraints6DOFList.ToDictionary(x => x.Name.ToString());
            List<IFLoadcase> allLoadcases = new List<IFLoadcase>();

            for (int i = 0; i < lusasPointForces.Count(); i++)
            {
                IFLoadingConcentrated lusasPointForce = (IFLoadingConcentrated)lusasPointForces[i];
                object[] assignmentObjects = lusasPointForce.getAssignments();
                List<IFAssignment> assignments = new List<IFAssignment>();

                for (int j = 0; j < assignmentObjects.Count(); j++)
                {
                    IFAssignment assignment = (IFAssignment)assignmentObjects[j];
                    assignments.Add(assignment);
                }

                IEnumerable<IGrouping<string, IFAssignment>> groupedByLoadcases = assignments.GroupBy(m => m.getAssignmentLoadset().getName());

                foreach (IEnumerable<IFAssignment> groupedAssignment in groupedByLoadcases)
                {
                    PointForce bhomPointForce = BH.Engine.Lusas.Convert.ToBHoMPointLoad(lusasPointForce, groupedAssignment, groupNames, constraints6DOF);
                    List<string> analysisName = new List<string> { lusasPointForce.getAttributeType() };
                    bhomPointForce.Tags = new HashSet<string>(analysisName);
                    bhomPointForces.Add(bhomPointForce);
                }
            }

            return bhomPointForces;

            /***************************************************/
        }
    }
}
