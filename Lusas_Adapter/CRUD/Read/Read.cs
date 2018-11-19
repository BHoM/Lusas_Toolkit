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
using BH.Engine.Lusas;
using Lusas.LPI;
using BH.oM.Adapter.Lusas;

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
            else if(type == typeof(Constraint4DOF))
                return ReadConstraint4DOFs(ids as dynamic);
            else if (type == typeof(Loadcase))
                return ReadLoadcases(ids as dynamic);
            else if (typeof(ILoad).IsAssignableFrom(type))
                return chooseLoad(type, ids as dynamic);
            else if (typeof(IProperty2D).IsAssignableFrom(type))
                return ReadProperty2D(ids as dynamic);
            else if (typeof(ISectionProperty).IsAssignableFrom(type))
                return ReadSectionProperties(ids as dynamic);
            else if (type == typeof(LoadCombination))
                return ReadLoadCombination(ids as dynamic);
            else if(type == typeof(BHoMObject))
                return ReadAll(ids as dynamic);
            else if (type == typeof(MeshSettings1D))
                return ReadMeshSettings1D(ids as dynamic);
            else if (type == typeof(MeshSettings2D))
                return ReadMeshSettings2D(ids as dynamic);
            return null;
        }

        /***************************************************/
        /**** Private specific read methods             ****/
        /***************************************************/

        //The List<string> in the methods below can be changed to a list of any type of identification more suitable for the toolkit

        private List<IBHoMObject> ReadAll(List<string> ids = null)
        {
            List<IBHoMObject> objects = new List<IBHoMObject>();

            objects.AddRange(ReadNodes());
            objects.AddRange(ReadBars());
            objects.AddRange(ReadSurfaces());
            objects.AddRange(ReadProperty2D());
            objects.AddRange(ReadMaterials());
            objects.AddRange(ReadConstraint4DOFs());
            objects.AddRange(ReadConstraint6DOFs());
            objects.AddRange(ReadLoadcases());
            objects.AddRange(ReadLoadCombination());
            objects.AddRange(ReadPointForce());
            objects.AddRange(ReadPointDisplacement());
            objects.AddRange(ReadBarUniformlyDistributedLoad());
            objects.AddRange(ReadBarPointLoad());
            objects.AddRange(ReadBarVaryingDistributedLoad());
            objects.AddRange(ReadAreaUniformlyDistributedLoad());
            objects.AddRange(ReadBarTemperatureLoad());
            objects.AddRange(ReadAreaTemperatureLoad());
            objects.AddRange(ReadGravityLoad());
            return objects;
        }

        private List<Bar> ReadBars(List<string> ids = null)
        {
            object[] lusasLines = d_LusasData.getObjects("Line");

            List<Bar> bhomBars = new List<Bar>();
            IEnumerable<Node> bhomNodesList = ReadNodes();
            Dictionary<string, Node> bhomNodes = bhomNodesList.ToDictionary(x => x.CustomData[AdapterId].ToString());
            IEnumerable<Constraint4DOF> bhomSupportList = ReadConstraint4DOFs();
            Dictionary<string, Constraint4DOF> bhomSupports = bhomSupportList.ToDictionary(x => x.Name);
            IEnumerable<Material> materialList = ReadMaterials();
            Dictionary<string, Material> materials = materialList.ToDictionary(x => x.Name.ToString());
            IEnumerable<ISectionProperty> geometricList = ReadSectionProperties();
            Dictionary<string, ISectionProperty> geometrics = geometricList.ToDictionary(x => x.Name.ToString());
            HashSet<string> groupNames = ReadGroups();

            for (int i = 0; i < lusasLines.Count(); i++)
            {
                IFLine lusasLine = (IFLine)lusasLines[i];
                Bar bhomBar = BH.Engine.Lusas.Convert.ToBHoMBar(lusasLine, bhomNodes, bhomSupports, groupNames,materials,geometrics);

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
                HashSet<string> groupNames = ReadGroups();
                IEnumerable<Material> materialList = ReadMaterials();
                Dictionary<string, Material> materials = materialList.ToDictionary(x => x.Name.ToString());
                IEnumerable<IProperty2D> geometricList = ReadProperty2D();
                Dictionary<string, IProperty2D> geometrics = geometricList.ToDictionary(x => x.Name.ToString());
                IEnumerable<Constraint4DOF> bhomSupportList = ReadConstraint4DOFs();
                Dictionary<string, Constraint4DOF> bhomSupports = bhomSupportList.ToDictionary(x => x.Name);

                for (int i = 0; i < eleArray.Count(); i++)
                {
                    IFSurface lusasSurface = (IFSurface)eleArray[i];
                    PanelPlanar bhompanel = BH.Engine.Lusas.Convert.ToBHoMPanelPlanar(lusasSurface,
                        bhomEdges,
                        groupNames,
                        geometrics,
                        materials,
                        bhomSupports);

                    bhomSurfaces.Add(bhompanel);
                }
            }

            return bhomSurfaces;
        }

        /***************************************************/

        private List<Node> ReadNodes(List<string> ids = null)
        {
            object[] lusasPoints = d_LusasData.getObjects("Point");

            List<Node> bhomNodes = new List<Node>();
            HashSet<string> groupNames = ReadGroups();

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

        /***************************************************/

        private List<Point> ReadPoints(List<string> ids = null)
        {
            object[] lusasPoints = d_LusasData.getObjects("Point");

            List<Point> bhomPoints = new List<Point>();
            HashSet<string> groupNames = ReadGroups();

            for (int i = 0; i < lusasPoints.Count(); i++)
            {
                IFPoint lusasPoint = (IFPoint)lusasPoints[i];
                Point bhomPoint = BH.Engine.Lusas.Convert.ToBHoMPoint(lusasPoint, groupNames);
                bhomPoints.Add(bhomPoint);
            }
            return bhomPoints;
        }

        /***************************************************/

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

        /***************************************************/

        private HashSet<string> ReadGroups(List<string> ids = null)
        {
            object[] eleArray = d_LusasData.getObjects("Groups");
            HashSet<string> bhomTags = new HashSet<string>();

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
                HashSet<string> groupNames = ReadGroups();

                for (int i = 0; i < lusasLines.Count(); i++)
                {
                    IFLine lusasline = (IFLine)lusasLines[i];
                    Edge bhomEdge = BH.Engine.Lusas.Convert.ToBHoMEdge(lusasline, bhomNodes, groupNames);
                    bhomEdges.Add(bhomEdge);
                }
            }

            return bhomEdges;
        }

        /***************************************************/

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

        private List<Constraint6DOF> ReadConstraint6DOFs(List<string> ids = null)
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

        private List<Constraint4DOF> ReadConstraint4DOFs(List<string> ids = null)
        {
            object[] lusasSupports = d_LusasData.getAttributes("Support");
            List<Constraint4DOF> bhomConstraints4DOFs = new List<Constraint4DOF>();

            for (int i = 0; i < lusasSupports.Count(); i++)
            {
                IFSupportStructural lusasSupport = (IFSupportStructural)lusasSupports[i];
                Constraint4DOF bhomConstraint4DOF = BH.Engine.Lusas.Convert.ToBHoMConstraint4DOF(lusasSupport);
                bhomConstraints4DOFs.Add(bhomConstraint4DOF);
            }
            return bhomConstraints4DOFs;
        }

        /***************************************/

        private List<ISectionProperty> ReadSectionProperties(List<string> ids = null)
        {
            object[] lusasSections = d_LusasData.getAttributes("Line Geometric");
            List<ISectionProperty> bhomSections = new List<ISectionProperty>();

            for (int i = 0; i < lusasSections.Count(); i++)
            {
                IFAttribute lusasSection = (IFAttribute)lusasSections[i];
                ISectionProperty bhomSection = BH.Engine.Lusas.Convert.ToBHoMSection(lusasSection);
                bhomSections.Add(bhomSection);
            }
            return bhomSections;
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
                IProperty2D bhomProperty2D = BH.Engine.Lusas.Convert.ToBHoMProperty2D(lusasThickness);
                bhomProperties2D.Add(bhomProperty2D);
            }

            return bhomProperties2D;
        }

        /***************************************************/

        private List<ILoad> chooseLoad(Type type, List<string> ids = null)
        {
            List<ILoad> readLoads = null;
            string typeName = type.Name;
            switch(typeName)
            {
                case "PointForce":
                    readLoads = ReadPointForce(ids as dynamic);
                    break;
                case "GravityLoad":
                    readLoads = ReadGravityLoad(ids as dynamic);
                    break;
                case "BarUniformlyDistributedLoad":
                    readLoads = ReadBarUniformlyDistributedLoad(ids as dynamic);
                    break;
                case "AreaUniformalyDistributedLoad":
                    readLoads = ReadAreaUniformlyDistributedLoad(ids as dynamic);
                    break;
                case "BarTemperatureLoad":
                    readLoads = ReadBarTemperatureLoad(ids as dynamic);
                    break;
                case "AreaTemperatureLoad":
                    readLoads = ReadAreaTemperatureLoad(ids as dynamic);
                    break;
                case "PointDisplacement":
                    readLoads = ReadPointDisplacement(ids as dynamic);
                    break;
                case "BarPointLoad":
                    readLoads = ReadBarPointLoad(ids as dynamic);
                    break;
                case "BarVaryingDistributedLoad":
                    readLoads = ReadBarVaryingDistributedLoad(ids as dynamic);
                    break;
            }
            return readLoads;

        }

        /***************************************************/

        private List<ILoad> ReadPointForce(List<string> ids = null)
        {
            List<ILoad> bhomPointForces = new List<ILoad>();
            object[] lusasPointForces = d_LusasData.getAttributes("Concentrated Load");

            List<Node> bhomNodes = ReadNodes();
            Dictionary<string, Node> nodeDict = bhomNodes.ToDictionary(x => x.CustomData[AdapterId].ToString());
            List<IFLoadcase> allLoadcases = new List<IFLoadcase>();

            for (int i = 0; i < lusasPointForces.Count(); i++)
            {
                IFLoading lusasPointForce = (IFLoading)lusasPointForces[i];

                IEnumerable<IGrouping<string, IFAssignment>> groupedByLoadcases = GetLoadAssignments(lusasPointForce);

                foreach (IEnumerable<IFAssignment> groupedAssignment in groupedByLoadcases)
                {
                    PointForce bhomPointForce = BH.Engine.Lusas.Convert.ToPointForce(lusasPointForce, groupedAssignment, nodeDict);
                    List<string> analysisName = new List<string> { lusasPointForce.getAttributeType() };
                    bhomPointForce.Tags = new HashSet<string>(analysisName);
                    bhomPointForces.Add(bhomPointForce);
                }
            }

            return bhomPointForces;
        }

        /***************************************************/

        private List<ILoad> ReadGravityLoad(List<string> ids = null)
        {
            List<ILoad> bhomGravityLoads = new List<ILoad>();
            object[] lusasGravityLoads = d_LusasData.getAttributes("Body Force Load");

            List<Bar> bhomBars = ReadBars();
            List<PanelPlanar> bhomPanels = ReadSurfaces();
            Dictionary<string, Bar> barDictionary = bhomBars.ToDictionary(x => x.CustomData[AdapterId].ToString());
            Dictionary<string, PanelPlanar> panelDictionary = bhomPanels.ToDictionary(x => x.CustomData[AdapterId].ToString());
            List<IFLoadcase> allLoadcases = new List<IFLoadcase>();

            for (int i = 0; i < lusasGravityLoads.Count(); i++)
            {
                IFLoading lusasGravityLoad = (IFLoading) lusasGravityLoads[i];

                IEnumerable<IGrouping<string, IFAssignment>> groupedByLoadcases = GetLoadAssignments(lusasGravityLoad);

                foreach (IEnumerable<IFAssignment> groupedAssignment in groupedByLoadcases)
                {
                    List<IFAssignment> barAssignments = new List<IFAssignment>();
                    List<IFAssignment> surfaceAssignments = new List<IFAssignment>();

                    foreach (IFAssignment assignment in groupedAssignment)
                    {
                        IFLine tryLine = assignment.getDatabaseObject() as IFLine;
                        IFSurface trySurf = assignment.getDatabaseObject() as IFSurface;

                        if (tryLine != null)
                        {
                            barAssignments.Add(assignment);
                        }
                        else
                        {
                            surfaceAssignments.Add(assignment);
                        }
                    }

                    List<string> analysisName = new List<string> { lusasGravityLoad.getAttributeType() };

                    if (barAssignments.Count!=0)
                    {
                        GravityLoad bhomBarGravityLoad = BH.Engine.Lusas.Convert.ToGravityLoad(lusasGravityLoad, barAssignments, "Bar", barDictionary, panelDictionary);
                        bhomBarGravityLoad.Tags = new HashSet<string>(analysisName);
                        bhomGravityLoads.Add(bhomBarGravityLoad);
                    }

                    if (surfaceAssignments.Count != 0)
                    {
                        GravityLoad bhomSurfGravityLoad = BH.Engine.Lusas.Convert.ToGravityLoad(lusasGravityLoad, surfaceAssignments, "Surface", barDictionary, panelDictionary);
                        bhomSurfGravityLoad.Tags = new HashSet<string>(analysisName);
                        bhomGravityLoads.Add(bhomSurfGravityLoad);
                    }
                }
            }

            return bhomGravityLoads;
        }

        /***************************************************/

        private List<ILoad> ReadBarUniformlyDistributedLoad(List<string> ids = null)
        {
            List<ILoad> bhomBarUniformlyDistributedLoads = new List<ILoad>();
            object[] lusasGlobalDistributedLoads = d_LusasData.getAttributes("Global Distributed Load");
            object[] lusasLocalDistributedLoads = d_LusasData.getAttributes("Distributed Load");

            object[] lusasDistributedLoads = lusasGlobalDistributedLoads.Concat(
                lusasLocalDistributedLoads).ToArray();

            List<Bar> bhomBars = ReadBars();
            Dictionary<string, Bar> barDictionary = bhomBars.ToDictionary(x => x.CustomData[AdapterId].ToString());
            List<IFLoadcase> allLoadcases = new List<IFLoadcase>();

            for (int i = 0; i < lusasDistributedLoads.Count(); i++)
            {
                IFLoading lusasDistributedLoad = (IFLoading)lusasDistributedLoads[i];

                IEnumerable<IGrouping<string, IFAssignment>> groupedByLoadcases = GetLoadAssignments(lusasDistributedLoad);

                if(lusasDistributedLoad.getValue("type")=="Length")
                {
                    foreach (IEnumerable<IFAssignment> groupedAssignment in groupedByLoadcases)
                    {
                        BarUniformlyDistributedLoad bhomBarUniformlyDistributedLoad = BH.Engine.Lusas.Convert.ToBarUniformallyDistributed(lusasDistributedLoad, groupedAssignment, barDictionary);
                        List<string> analysisName = new List<string> { lusasDistributedLoad.getAttributeType() };
                        bhomBarUniformlyDistributedLoad.Tags = new HashSet<string>(analysisName);
                        bhomBarUniformlyDistributedLoads.Add(bhomBarUniformlyDistributedLoad);
                    }
                }
            }

            return bhomBarUniformlyDistributedLoads;
        }

        private List<ILoad> ReadBarTemperatureLoad(List<string> ids = null)
        {
            List<ILoad> bhomBarTemperatureLoads = new List<ILoad>();
            object[] lusasTemperatureLoads = d_LusasData.getAttributes("Temperature");

            List<Bar> bhomBars = ReadBars();
            Dictionary<string, Bar> barDictionary = bhomBars.ToDictionary(x => x.CustomData[AdapterId].ToString());
            List<IFLoadcase> allLoadcases = new List<IFLoadcase>();

            for (int i = 0; i < lusasTemperatureLoads.Count(); i++)
            {
                IFLoading lusasTemperatureLoad = (IFLoading)lusasTemperatureLoads[i];

                IEnumerable<IGrouping<string, IFAssignment>> groupedByLoadcases = GetLoadAssignments(lusasTemperatureLoad);

                foreach (IEnumerable<IFAssignment> groupedAssignment in groupedByLoadcases)
                {
                    List<IFAssignment> barAssignments = new List<IFAssignment>();

                    foreach (IFAssignment assignment in groupedAssignment)
                    {
                        IFLine tryLine = assignment.getDatabaseObject() as IFLine;

                        if (tryLine != null)
                        {
                            barAssignments.Add(assignment);
                        }
                    }

                    List<string> analysisName = new List<string> { lusasTemperatureLoad.getAttributeType() };

                    if (barAssignments.Count != 0)
                    {
                        BarTemperatureLoad bhomBarTemperatureLoad = BH.Engine.Lusas.Convert.ToBarTemperatureLoad(lusasTemperatureLoad, groupedAssignment, barDictionary);
                        bhomBarTemperatureLoad.Tags = new HashSet<string>(analysisName);
                        bhomBarTemperatureLoads.Add(bhomBarTemperatureLoad);
                    }
                }
            }

            return bhomBarTemperatureLoads;
        }

        private List<ILoad> ReadAreaTemperatureLoad(List<string> ids = null)
        {
            List<ILoad> bhomAreaTemperatureLoads = new List<ILoad>();
            object[] lusasTemperatureLoads = d_LusasData.getAttributes("Temperature");

            List<PanelPlanar> bhomPanelPlanar = ReadSurfaces();
            Dictionary<string, PanelPlanar> surfaceDictionary = bhomPanelPlanar.ToDictionary(x => x.CustomData[AdapterId].ToString());
            List<IFLoadcase> allLoadcases = new List<IFLoadcase>();

            for (int i = 0; i < lusasTemperatureLoads.Count(); i++)
            {
                IFLoading lusasTemperatureLoad = (IFLoading)lusasTemperatureLoads[i];

                IEnumerable<IGrouping<string, IFAssignment>> groupedByLoadcases = GetLoadAssignments(lusasTemperatureLoad);

                foreach (IEnumerable<IFAssignment> groupedAssignment in groupedByLoadcases)
                {
                    List<IFAssignment> surfaceAssignments = new List<IFAssignment>();

                    foreach (IFAssignment assignment in groupedAssignment)
                    {
                        IFSurface trySurf = assignment.getDatabaseObject() as IFSurface;

                        if(trySurf != null)
                        {
                            surfaceAssignments.Add(assignment);
                        }
                    }

                    List<string> analysisName = new List<string> { lusasTemperatureLoad.getAttributeType() };

                    if (surfaceAssignments.Count != 0)
                    {
                        AreaTemperatureLoad bhomAreaTemperatureLoad = BH.Engine.Lusas.Convert.ToAreaTempratureLoad(lusasTemperatureLoad, groupedAssignment, surfaceDictionary);
                        bhomAreaTemperatureLoad.Tags = new HashSet<string>(analysisName);
                        bhomAreaTemperatureLoads.Add(bhomAreaTemperatureLoad);
                    }
                }

            }

            return bhomAreaTemperatureLoads;
        }

        /***************************************************/

        private List<ILoad> ReadAreaUniformlyDistributedLoad(List<string> ids = null)
        {
            List<ILoad> bhomPanelUniformlyDistributedLoads = new List<ILoad>();

            object[] lusasGlobalDistributedLoads = d_LusasData.getAttributes("Global Distributed Load");
            object[] lusasLocalDistributedLoads = d_LusasData.getAttributes("Distributed Load");


            object[] lusasDistributedLoads = lusasGlobalDistributedLoads.Concat(
                lusasLocalDistributedLoads).ToArray();

            List<PanelPlanar> bhomSurfaces = ReadSurfaces();
            Dictionary<string, PanelPlanar> surfaceDictionary = bhomSurfaces.ToDictionary(x => x.CustomData[AdapterId].ToString());
            List<IFLoadcase> allLoadcases = new List<IFLoadcase>();

            for (int i = 0; i < lusasDistributedLoads.Count(); i++)
            {
                IFLoading lusasDistributedLoad = (IFLoading)lusasDistributedLoads[i];

                if (lusasDistributedLoad.getValue("type")=="Area")
                {
                    IEnumerable<IGrouping<string, IFAssignment>> groupedByLoadcases = GetLoadAssignments(lusasDistributedLoad);

                    foreach (IEnumerable<IFAssignment> groupedAssignment in groupedByLoadcases)
                    {
                        AreaUniformalyDistributedLoad bhomBarUniformlyDistributedLoad = BH.Engine.Lusas.Convert.ToAreaUniformallyDistributed(lusasDistributedLoad, groupedAssignment, surfaceDictionary);
                        List<string> analysisName = new List<string> { lusasDistributedLoad.getAttributeType() };
                        bhomBarUniformlyDistributedLoad.Tags = new HashSet<string>(analysisName);
                        bhomPanelUniformlyDistributedLoads.Add(bhomBarUniformlyDistributedLoad);
                    }
                }
            }

            return bhomPanelUniformlyDistributedLoads;
        }

        private List<LoadCombination> ReadLoadCombination(List<string> ids = null)
        {
            List<LoadCombination> bhomLoadCombintations = new List<LoadCombination>();

            object[] lusasCombinations = d_LusasData.getLoadsets("Combinations");

            List<Loadcase> lusasLoadcases = ReadLoadcases();
            Dictionary<string, Loadcase> loadcaseDictionary = lusasLoadcases.ToDictionary(x => x.Number.ToString());

            for (int i = 0; i < lusasCombinations.Count(); i++)
            {
                IFBasicCombination lusasCombination = (IFBasicCombination)lusasCombinations[i];
                LoadCombination bhomLoadCombination = BH.Engine.Lusas.Convert.ToBHoMLoadCombination(lusasCombination, loadcaseDictionary);
                bhomLoadCombintations.Add(bhomLoadCombination);
            }

            return bhomLoadCombintations;
        }

        private List<ILoad> ReadPointDisplacement(List<string> ids = null)
        {
            List<ILoad> bhomPointDisplacements = new List<ILoad>();
            object[] lusasPointDisplacements = d_LusasData.getAttributes("Prescribed Load");

            List<Node> bhomNodes = ReadNodes();
            Dictionary<string, Node> nodeDict = bhomNodes.ToDictionary(x => x.CustomData[AdapterId].ToString());
            List<IFLoadcase> allLoadcases = new List<IFLoadcase>();

            for (int i = 0; i < lusasPointDisplacements.Count(); i++)
            {
                IFLoading lusasPointForce = (IFLoading)lusasPointDisplacements[i];

                IEnumerable<IGrouping<string, IFAssignment>> groupedByLoadcases = GetLoadAssignments(lusasPointForce);

                foreach (IEnumerable<IFAssignment> groupedAssignment in groupedByLoadcases)
                {
                    PointDisplacement bhomPointDisplacement = BH.Engine.Lusas.Convert.ToPointDisplacement(lusasPointForce, groupedAssignment, nodeDict);
                    List<string> analysisName = new List<string> { lusasPointForce.getAttributeType() };
                    bhomPointDisplacement.Tags = new HashSet<string>(analysisName);
                    bhomPointDisplacements.Add(bhomPointDisplacement);
                }
            }

            return bhomPointDisplacements;
        }

        /***************************************************/

        private List<ILoad> ReadBarPointLoad(List<string> ids = null)
        {
            List<ILoad> bhomBarPointLoads = new List<ILoad>();

            object[] lusasBarPointLoads = d_LusasData.getAttributes("Beam Point Load");

            if (lusasBarPointLoads.Count() != 0)
            {
                List<Bar> bhomBars = ReadBars();
                Dictionary<string, Bar> barDictionary = bhomBars.ToDictionary(x => x.CustomData[AdapterId].ToString());
                List<IFLoadcase> allLoadcases = new List<IFLoadcase>();

                for (int i = 0; i < lusasBarPointLoads.Count(); i++)
                {
                    IFLoading lusasBarPointLoad = (IFLoading)lusasBarPointLoads[i];
                    IEnumerable<IGrouping<string, IFAssignment>> groupedByLoadcases = GetLoadAssignments(lusasBarPointLoad);

                    foreach (IEnumerable<IFAssignment> groupedAssignment in groupedByLoadcases)
                    {
                        BarPointLoad bhomBarPointLoad = BH.Engine.Lusas.Convert.ToBHoMBarPointLoad(lusasBarPointLoad, groupedAssignment, barDictionary);
                        List<string> analysisName = new List<string> { lusasBarPointLoad.getAttributeType() };
                        bhomBarPointLoad.Tags = new HashSet<string>(analysisName);
                        bhomBarPointLoads.Add(bhomBarPointLoad);
                    }
                }
            }

            return bhomBarPointLoads;
        }

        /***************************************************/

        private List<ILoad> ReadBarVaryingDistributedLoad(List<string> ids = null)
        {
            List<ILoad> bhomBarDistributedLoads = new List<ILoad>();

            object[] lusasBarDistributedLoads = d_LusasData.getAttributes("Beam Distributed Load");

            if (lusasBarDistributedLoads.Count() != 0)
            {
                List<Bar> bhomBars = ReadBars();
                Dictionary<string, Bar> barDictionary = bhomBars.ToDictionary(x => x.CustomData[AdapterId].ToString());
                List<IFLoadcase> allLoadcases = new List<IFLoadcase>();

                for (int i = 0; i < lusasBarDistributedLoads.Count(); i++)
                {
                    IFLoading lusasBarDistributedLoad = (IFLoading)lusasBarDistributedLoads[i];
                    IEnumerable<IGrouping<string, IFAssignment>> groupedByLoadcases = GetLoadAssignments(lusasBarDistributedLoad);

                    foreach (IEnumerable<IFAssignment> groupedAssignment in groupedByLoadcases)
                    {
                        BarVaryingDistributedLoad bhomBarDistributedLoad = BH.Engine.Lusas.Convert.ToBHoMBarDistributedLoad(lusasBarDistributedLoad, groupedAssignment, barDictionary);
                        List<string> analysisName = new List<string> { lusasBarDistributedLoad.getAttributeType() };
                        bhomBarDistributedLoad.Tags = new HashSet<string>(analysisName);
                        bhomBarDistributedLoads.Add(bhomBarDistributedLoad);
                    }
                }
            }

            return bhomBarDistributedLoads;
        }

        /***************************************************/

        private List<MeshSettings1D> ReadMeshSettings1D(List<string> ids = null)
        {
            List<MeshSettings1D> bhomMeshSettings1Ds = new List<MeshSettings1D>();

            object[] lusasMesh1Ds = d_LusasData.getAttributes("Mesh");

            for (int i = 0; i < lusasMesh1Ds.Count(); i++)
            {
                IFAttribute meshAttribute = (IFAttribute)lusasMesh1Ds[i];
                if (meshAttribute.getAttributeType() == "Line Mesh")
                {
                    IFMeshLine lusasMesh1D = (IFMeshLine)lusasMesh1Ds[i];
                    MeshSettings1D bhomMeshSettings1D = BH.Engine.Lusas.Convert.ToBHoMMeshSettings1D(lusasMesh1D);
                    List<string> analysisName = new List<string> { lusasMesh1D.getAttributeType() };
                    bhomMeshSettings1D.Tags = new HashSet<string>(analysisName);
                    bhomMeshSettings1Ds.Add(bhomMeshSettings1D);
                }
            }

            return bhomMeshSettings1Ds;
        }

        /***************************************************/

        private List<MeshSettings2D> ReadMeshSettings2D(List<string> ids = null)
        {
            List<MeshSettings2D> bhomMeshSettings2Ds = new List<MeshSettings2D>();

            object[] lusasMesh2Ds = d_LusasData.getAttributes("Mesh");

            for (int i = 0; i < lusasMesh2Ds.Count(); i++)
            {
                IFAttribute meshAttribute = (IFAttribute)lusasMesh2Ds[i];
                if (meshAttribute.getAttributeType() == "Surface Mesh")
                {
                    IFMeshSurface lusasMesh2D = (IFMeshSurface)lusasMesh2Ds[i];
                    MeshSettings2D bhomMeshSettings2D = BH.Engine.Lusas.Convert.ToBHoMMeshSettings2D(lusasMesh2D);
                    List<string> analysisName = new List<string> { lusasMesh2D.getAttributeType() };
                    bhomMeshSettings2D.Tags = new HashSet<string>(analysisName);
                    bhomMeshSettings2Ds.Add(bhomMeshSettings2D);
                }
            }

            return bhomMeshSettings2Ds;
        }

        /***************************************************/
    }
}
