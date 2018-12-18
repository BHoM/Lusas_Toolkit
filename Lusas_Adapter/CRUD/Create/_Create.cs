using System.Collections.Generic;
using System.Linq;
using BH.oM.Structure.Elements;
using BH.oM.Geometry;
using BH.oM.Structure.Properties.Constraint;
using BH.oM.Structure.Properties.Section;
using BH.oM.Structure.Properties.Surface;
using BH.oM.Structure.Loads;
using BH.oM.Common.Materials;
using BH.Engine.Geometry;
using Lusas.LPI;
using BH.Engine.Lusas.Object_Comparer.Equality_Comparer;
using BH.oM.Adapters.Lusas;

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

            //m_LusasApplication.setManualRefresh(true);
            //m_LusasApplication.suppressMessages(1);
            //m_LusasApplication.enableTrees(false);

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
                if (objects.First() is Material)
                {
                    success = CreateCollection(objects as IEnumerable<Material>);
                }
                if (objects.First() is Constraint6DOF)
                {
                    success = CreateCollection(objects as IEnumerable<Constraint6DOF>);
                }
                if (objects.First() is Constraint4DOF)
                {
                    success = CreateCollection(objects as IEnumerable<Constraint4DOF>);
                }
                if (objects.First() is Loadcase)
                {
                    success = CreateCollection(objects as IEnumerable<Loadcase>);
                }
                if (objects.First() is LoadCombination)
                {
                    success = CreateCollection(objects as IEnumerable<LoadCombination>);
                }
                if (typeof(ILoad).IsAssignableFrom(objects.First().GetType()))
                {
                    string loadType = objects.First().GetType().ToString();

                    switch (loadType)
                    {
                        case "BH.oM.Structure.Loads.PointForce":
                            success = CreateCollection(objects as IEnumerable<PointForce>);
                            break;
                        case "BH.oM.Structure.Loads.GravityLoad":
                            success = CreateCollection(objects as IEnumerable<GravityLoad>);
                            break;
                        case "BH.oM.Structure.Loads.BarUniformlyDistributedLoad":
                            success = CreateCollection(objects as IEnumerable<BarUniformlyDistributedLoad>);
                            break;
                        case "BH.oM.Structure.Loads.AreaUniformalyDistributedLoad":
                            success = CreateCollection(objects as IEnumerable<AreaUniformalyDistributedLoad>);
                            break;
                        case "BH.oM.Structure.Loads.BarTemperatureLoad":
                            success = CreateCollection(objects as IEnumerable<BarTemperatureLoad>);
                            break;
                        case "BH.oM.Structure.Loads.AreaTemperatureLoad":
                            success = CreateCollection(objects as IEnumerable<AreaTemperatureLoad>);
                            break;
                        case "BH.oM.Structure.Loads.PointDisplacement":
                            success = CreateCollection(objects as IEnumerable<PointDisplacement>);
                            break;
                        case "BH.oM.Structure.Loads.BarPointLoad":
                            success = CreateCollection(objects as IEnumerable<BarPointLoad>);
                            break;
                        case "BH.oM.Structure.Loads.BarVaryingDistributedLoad":
                            success = CreateCollection(objects as IEnumerable<BarVaryingDistributedLoad>);
                            break;
                    }
                }
                if (typeof(ISurfaceProperty).IsAssignableFrom(objects.First().GetType()))
                {
                    success = CreateCollection(objects as IEnumerable<ISurfaceProperty>);
                }
                if (typeof(ISectionProperty).IsAssignableFrom(objects.First().GetType()))
                {
                    success = CreateCollection(objects as IEnumerable<ISectionProperty>);
                }
                if (objects.First() is MeshSettings1D)
                {
                    success = CreateCollection(objects as IEnumerable<MeshSettings1D>);
                }
                if (objects.First() is MeshSettings2D)
                {
                    success = CreateCollection(objects as IEnumerable<MeshSettings2D>);
                }
            }

            //m_LusasApplication.setManualRefresh(false);
            //m_LusasApplication.suppressMessages(0);
            //m_LusasApplication.enableTrees(true);
            ////success = CreateCollection(objects as dynamic);
            //m_LusasApplication.updateAllViews();

            return success;
        }

        /***************************************************/
        /**** Private methods                           ****/
        /***************************************************/

        private bool CreateCollection(IEnumerable<Node> nodes)
        {
            CreateTags(nodes);

            foreach (Node node in nodes)
            {
                IFPoint lusasPoint = CreatePoint(node);
            }
            return true;
        }

        /***************************************************/

        private bool CreateCollection(IEnumerable<Point> points)
        {

            List<Point> distinctPoints = GetDistinctPoints(points);

            List<Point> existingPoints = ReadPoints();

            List<Point> lusasPoints = distinctPoints.Except(existingPoints).ToList();

            foreach (Point point in lusasPoints)
            {
                IFPoint lusasPoint = CreatePoint(point);
            }

            return true;
        }

        /***************************************************/

        private bool CreateCollection(IEnumerable<Bar> bars)
        {
            List<Bar> barList = bars.ToList();
            CreateTags(bars);

            if (bars.Any(x => x.CustomData.ContainsKey("Mesh")))
            {
                var groupedBars = bars.GroupBy(m => new { m.Release, m.FEAType, MeshSettings1D = m.CustomData["Mesh"] });


                List<Bar> distinctMeshBars = groupedBars.Select(m => m.First()).ToList();
                List<IFMeshLine> lusasLineMesh = new List<IFMeshLine>();

                foreach (Bar bar in distinctMeshBars)
                {
                    if (bar.CustomData["Mesh"] != null)
                    {
                        lusasLineMesh.Add(CreateMeshSettings1D((MeshSettings1D)bar.CustomData["Mesh"], bar.FEAType, bar.Release));
                    }
                }

                int count = 0;

                foreach (var group in groupedBars)
                {
                    List<Bar> barGroup = group.ToList();
                    foreach (Bar bar in barGroup)
                    {
                        IFLine lusasLine = CreateLine(bar, lusasLineMesh[count]);

                        if (lusasLine == null)
                        {
                            return false;
                        }

                    }
                    count++;
                }
                d_LusasData.resetMesh();
                d_LusasData.updateMesh();

                return true;
            }
            else
            {
                foreach (Bar bar in bars)
                {
                    IFLine lusasLine = CreateLine(bar, null);

                    if (lusasLine == null)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        /***************************************************/

        private bool CreateCollection(IEnumerable<PanelPlanar> planarPanels)
        {

            CreateTags(planarPanels);

            List<Edge> panelPlanarEdges = new List<Edge>();

            foreach (PanelPlanar panelPlanar in planarPanels)
            {
                panelPlanarEdges.AddRange(panelPlanar.ExternalEdges);
            }

            List<Edge> distinctEdges = GetDistinctEdges(panelPlanarEdges);

            List<Point> midPoints = new List<Point>();

            foreach (Edge edge in distinctEdges)
            {
                midPoints.Add(edge.Curve.IPointAtParameter(0.5));
            }

            foreach (PanelPlanar panelPlanar in planarPanels)
            {
                IFLine[] lusasLines = new IFLine[panelPlanar.ExternalEdges.Count];
                List<Edge> edges = panelPlanar.ExternalEdges;

                for (int i = 0; i < panelPlanar.ExternalEdges.Count; i++)
                {
                    Edge edge = distinctEdges[midPoints.FindIndex(
                        m => m.Equals(edges[i].Curve.IPointAtParameter(0.5).ClosestPoint(midPoints)))];

                    lusasLines[i] = d_LusasData.getLineByName("L" + edge.CustomData[AdapterId].ToString());
                }

                IFSurface lusasSurface = CreateSurface(panelPlanar, lusasLines);
            }
            return true;
        }

        /***************************************************/

        private bool CreateCollection(IEnumerable<Edge> edges)
        {
            List<Point> allPoints = new List<Point>();

            List<Edge> distinctEdges = GetDistinctEdges(edges);

            foreach (Edge edge in distinctEdges)
            {
                allPoints.Add(edge.Curve.IStartPoint());
                allPoints.Add(edge.Curve.IEndPoint());
            }

            List<Point> distinctPoints = GetDistinctPoints(allPoints);

            List<Point> existingPoints = ReadPoints();
            List<Point> pointsToPush = distinctPoints.Except(
                existingPoints, new PointDistanceComparer()).ToList();

            foreach (Point point in pointsToPush)
            {
                IFPoint lusasPoint = CreatePoint(point);
            }

            List<IFPoint> lusasPoints = ReadLusasPoints();
            List<Point> bhomPoints = new List<Point>();

            foreach (IFPoint point in lusasPoints)
            {
                bhomPoints.Add(Engine.Lusas.Convert.ToBHoMPoint(point));
            }

            CreateTags(distinctEdges);

            foreach (Edge edge in distinctEdges)
            {
                IFPoint startPoint = lusasPoints[bhomPoints.FindIndex(
                    m => m.Equals(edge.Curve.IStartPoint().ClosestPoint(bhomPoints)))];
                IFPoint endPoint = lusasPoints[bhomPoints.FindIndex(
                    m => m.Equals(edge.Curve.IEndPoint().ClosestPoint(bhomPoints)))];
                IFLine lusasLine = CreateEdge(edge, startPoint, endPoint);
            }

            return true;
        }

        /***************************************************/

        private bool CreateCollection(IEnumerable<ISectionProperty> sectionProperties)
        {
            foreach (ISectionProperty sectionProperty in sectionProperties)
            {
                IFAttribute lusasGeometricLine = CreateGeometricLine(sectionProperty);

                if (lusasGeometricLine == null)
                {
                    return false;
                }
            }

            return true;
        }

        /***************************************************/

        private bool CreateCollection(IEnumerable<Material> materials)
        {
            foreach (Material material in materials)
            {
                IFAttribute lusasMaterial = CreateMaterial(material);

                if (lusasMaterial == null)
                {
                    return false;
                }
            }

            return true;
        }

        /***************************************************/

        private bool CreateCollection(IEnumerable<ISurfaceProperty> properties2D)
        {
            foreach (ISurfaceProperty property2D in properties2D)
            {
                IFAttribute lusasGeometricSurface = CreateGeometricSurface(property2D);

                if (lusasGeometricSurface == null)
                {
                    return false;
                }
            }

            return true;
        }

        /***************************************************/

        private bool CreateCollection(IEnumerable<Loadcase> loadcases)
        {
            foreach (Loadcase loadcase in loadcases)
            {
                IFLoadcase lusasLoadcase = CreateLoadcase(loadcase);

                if (lusasLoadcase == null)
                {
                    return false;
                }
            }

            return true;
        }

        /***************************************************/

        private bool CreateCollection(IEnumerable<PointForce> pointForces)
        {

            foreach (PointForce pointForce in pointForces)
            {
                IFPoint[] assignedPoints = GetAssignedPoints(pointForce);
                IFLoadingConcentrated lusasPointForce = CreateConcentratedLoad(pointForce, assignedPoints);

                if (lusasPointForce == null)
                {
                    return false;
                }
            }

            return true;
        }

        /***************************************************/

        private bool CreateCollection(IEnumerable<GravityLoad> gravityLoads)
        {
            foreach (GravityLoad gravityLoad in gravityLoads)
            {
                IFGeometry[] assignedGeometry = GetAssignedObjects(gravityLoad);
                IFLoadingBody lusasGravityLoad = CreateGravityLoad(gravityLoad, assignedGeometry);

                if (lusasGravityLoad == null)
                {
                    return false;
                }
            }

            return true;
        }

        /***************************************************/

        private bool CreateCollection(IEnumerable<BarUniformlyDistributedLoad> barUniformlyDistributedLoads)
        {
            foreach (BarUniformlyDistributedLoad barUniformlyDistributedLoad in barUniformlyDistributedLoads)
            {
                IFLine[] assignedLines = GetAssignedLines(barUniformlyDistributedLoad);

                if (barUniformlyDistributedLoad.Axis == LoadAxis.Global)
                {
                    IFLoadingGlobalDistributed lusasGlobalDistributed =
                        CreateGlobalDistributedLine(barUniformlyDistributedLoad, assignedLines);

                    if (lusasGlobalDistributed == null)
                    {
                        return false;
                    }
                }
                else if (barUniformlyDistributedLoad.Axis == LoadAxis.Local)
                {
                    IFLoadingLocalDistributed lusasLocalDistributed =
                        CreateLocalDistributedLine(barUniformlyDistributedLoad, assignedLines);

                    if (lusasLocalDistributed == null)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /***************************************************/

        private bool CreateCollection(IEnumerable<AreaUniformalyDistributedLoad> areaUniformlyDistributedLoads)
        {
            foreach (AreaUniformalyDistributedLoad areaUniformlyDistributedLoad in areaUniformlyDistributedLoads)
            {
                IFSurface[] assignedSurfaces = GetAssignedSurfaces(areaUniformlyDistributedLoad);
                if (areaUniformlyDistributedLoad.Axis == LoadAxis.Global)
                {
                    IFLoadingGlobalDistributed lusasGlobalDistributed =
                        CreateGlobalDistributedLoad(areaUniformlyDistributedLoad, assignedSurfaces);

                    if (lusasGlobalDistributed == null)
                    {
                        return false;
                    }
                }
                else if (areaUniformlyDistributedLoad.Axis == LoadAxis.Local)
                {
                    IFLoadingLocalDistributed lusasLocalDistributed =
                        CreateLocalDistributedSurface(areaUniformlyDistributedLoad, assignedSurfaces);

                    if (lusasLocalDistributed == null)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /***************************************************/

        private bool CreateCollection(IEnumerable<BarTemperatureLoad> barTemperatureLoads)
        {
            foreach (BarTemperatureLoad barTemperatureLoad in barTemperatureLoads)
            {
                IFLine[] arrayLines = GetAssignedLines(barTemperatureLoad);
                IFLoadingTemperature lusasBarTemperatureLoad =
                    CreateBarTemperatureLoad(barTemperatureLoad, arrayLines);

                if (lusasBarTemperatureLoad == null)
                {
                    return false;
                }
            }

            return true;
        }

        /***************************************************/

        private bool CreateCollection(IEnumerable<AreaTemperatureLoad> areaTemperatureLoads)
        {
            foreach (AreaTemperatureLoad areaTemperatureLoad in areaTemperatureLoads)
            {
                IFSurface[] assignedLines = GetAssignedSurfaces(areaTemperatureLoad);
                IFLoadingTemperature lusasAreaTemperatureLoad =
                    CreateAreaTemperatureLoad(areaTemperatureLoad, assignedLines);

                if (lusasAreaTemperatureLoad == null)
                {
                    return false;
                }
            }

            return true;
        }

        /***************************************************/

        private bool CreateCollection(IEnumerable<PointDisplacement> pointDisplacements)
        {
            foreach (PointDisplacement pointDisplacement in pointDisplacements)
            {
                IFPoint[] assignedPoints = GetAssignedPoints(pointDisplacement);
                IFPrescribedDisplacementLoad lusasPrescribedDisplacement =
                    CreatePrescribedDisplacement(pointDisplacement, assignedPoints);

                if (lusasPrescribedDisplacement == null)
                {
                    return false;
                }
            }

            return true;
        }

        /***************************************************/

        private bool CreateCollection(IEnumerable<BarPointLoad> barPointLoads)
        {

            foreach (BarPointLoad barPointLoad in barPointLoads)
            {
                IFLine[] assignedLines = GetAssignedLines(barPointLoad);
                IFLoadingBeamPoint lusasGlobalDistributed =
                    CreateBarPointLoad(barPointLoad, assignedLines);

                if (lusasGlobalDistributed == null)
                {
                    return false;
                }
            }

            return true;
        }

        /***************************************************/

        private bool CreateCollection(IEnumerable<BarVaryingDistributedLoad> barDistributedLoads)
        {

            foreach (BarVaryingDistributedLoad barDistributedLoad in barDistributedLoads)
            {
                IFLine[] assignedBars = GetAssignedLines(barDistributedLoad);
                List<IFLoadingBeamDistributed> lusasGlobalDistributed =
                    CreateBarDistributedLoad(barDistributedLoad, assignedBars);

                if (lusasGlobalDistributed == null)
                {
                    return false;
                }
            }

            return true;
        }

        /***************************************************/

        private bool CreateCollection(IEnumerable<Constraint6DOF> constraints)
        {
            foreach (Constraint6DOF constraint in constraints)
            {
                IFAttribute lusasSupport = CreateSupport(constraint);

                if (lusasSupport == null)
                {
                    return false;
                }
            }

            return true;
        }

        private bool CreateCollection(IEnumerable<Constraint4DOF> constraints)
        {
            foreach (Constraint4DOF constraint in constraints)
            {
                IFAttribute lusasSupport = CreateSupport(constraint);

                if (lusasSupport == null)
                {
                    return false;
                }
            }

            return true;
        }

        private bool CreateCollection(IEnumerable<LoadCombination> loadcombinations)
        {
            foreach (LoadCombination loadcombination in loadcombinations)
            {
                IFBasicCombination lusasLoadCombination = CreateLoadCombination(loadcombination);

                if (lusasLoadCombination == null)
                {
                    return false;
                }
            }

            return true;
        }

        /***************************************************/

        private bool CreateCollection(IEnumerable<MeshSettings1D> meshSettings1Ds)
        {

            foreach (MeshSettings1D meshSettings1D in meshSettings1Ds)
            {
                IFMeshLine lusasLineMesh = CreateMeshSettings1D(meshSettings1D);

                if (lusasLineMesh == null)
                {
                    return false;
                }
            }

            return true;
        }

        /***************************************************/

        private bool CreateCollection(IEnumerable<MeshSettings2D> meshSettings2Ds)
        {

            foreach (MeshSettings2D meshSettings2D in meshSettings2Ds)
            {
                IFMeshSurface lusasSurfaceMesh = CreateMeshSettings2D(meshSettings2D);

                if (lusasSurfaceMesh == null)
                {
                    return false;
                }
            }

            return true;
        }

        /***************************************************/

    }
}

