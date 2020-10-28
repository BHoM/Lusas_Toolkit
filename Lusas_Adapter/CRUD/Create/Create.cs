/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2020, the respective contributors. All rights reserved.
 *
 * Each contributor holds copyright over their respective contributions.
 * The project versioning (Git) records all such contribution source information.
 *                                           
 *                                                                              
 * The BHoM is free software: you can redistribute it and/or modify         
 * it under the terms of the GNU Lesser General Public License as published by  
 * the Free Software Foundation, either version 3.0 of the License, or          
 * (at your option) any later version.                                          
 *                                                                              
 * The BHoM is distributed in the hope that it will be useful,              
 * but WITHOUT ANY WARRANTY; without even the implied warranty of               
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the                 
 * GNU Lesser General Public License for more details.                          
 *                                                                            
 * You should have received a copy of the GNU Lesser General Public License     
 * along with this code. If not, see <https://www.gnu.org/licenses/lgpl-3.0.html>.      
 */

using BH.Engine.Geometry;
using BH.Engine.Adapters.Lusas.Object_Comparer.Equality_Comparer;
using BH.oM.Adapter;
using BH.oM.Adapters.Lusas;
using BH.oM.Structure.Elements;
using BH.oM.Geometry;
using BH.oM.Structure.Constraints;
using BH.oM.Structure.SectionProperties;
using BH.oM.Structure.SurfaceProperties;
using BH.oM.Structure.Loads;
using BH.oM.Structure.MaterialFragments;
using BH.Engine.Adapter;
using Lusas.LPI;
using System.Collections.Generic;
using System.Linq;
using BH.Engine.Base.Objects;
using System;
using System.Reflection;
using BH.Engine.Base;
using BH.oM.Adapters.Lusas.Fragments;

namespace BH.Adapter.Lusas
{
#if Debug18 || Release18
    public partial class LusasV18Adapter
#elif Debug19 || Release19
    public partial class LusasV19Adapter
#else
    public partial class LusasV17Adapter
#endif
    {
        /***************************************************/
        /**** Adapter overload method                   ****/
        /***************************************************/
        protected override bool ICreate<T>(IEnumerable<T> objects, ActionConfig actionConfig = null)
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
                if (objects.First() is Panel)
                {
                    success = CreateCollection(objects as IEnumerable<Panel>);
                }
                if (objects.First() is Edge)
                {
                    success = CreateCollection(objects as IEnumerable<Edge>);
                }
                if (objects.First() is Point)
                {
                    success = CreateCollection(objects as IEnumerable<Point>);
                }
                if (objects.First() is IMaterialFragment)
                {
                    success = CreateCollection(objects as IEnumerable<IMaterialFragment>);
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
                        case "BH.oM.Structure.Loads.PointLoad":
                            success = CreateCollection(objects as IEnumerable<PointLoad>);
                            break;
                        case "BH.oM.Structure.Loads.GravityLoad":
                            success = CreateCollection(objects as IEnumerable<GravityLoad>);
                            break;
                        case "BH.oM.Structure.Loads.BarUniformlyDistributedLoad":
                            success = CreateCollection(objects as IEnumerable<BarUniformlyDistributedLoad>);
                            break;
                        case "BH.oM.Structure.Loads.AreaUniformlyDistributedLoad":
                            success = CreateCollection(objects as IEnumerable<AreaUniformlyDistributedLoad>);
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

            return success;
        }

        /***************************************************/
        /**** Private methods                           ****/
        /***************************************************/

        private bool CreateCollection(IEnumerable<Node> nodes)
        {
            CreateTags(nodes);

            ReduceRuntime(true);

            foreach (Node node in nodes)
            {
                IFPoint lusasPoint = CreatePoint(node);
            }

            ReduceRuntime(false);

            return true;
        }

        /***************************************************/

        private bool CreateCollection(IEnumerable<Point> points)
        {

            List<Point> distinctPoints = Engine.Adapters.Lusas.Query.GetDistinctPoints(points);

            List<Point> existingPoints = ReadPoints();

            List<Point> lusasPoints = distinctPoints.Except(existingPoints).ToList();

            ReduceRuntime(true);

            foreach (Point point in lusasPoints)
            {
                IFPoint lusasPoint = CreatePoint(point);
            }

            ReduceRuntime(false);

            return true;
        }

        /***************************************************/

        private bool CreateCollection(IEnumerable<Bar> bars)
        {
            List<Bar> barList = bars.ToList();

            CreateTags(bars);

            if (bars.Any(x => x.Fragments.Contains(typeof(MeshSettings1D))))
            {
                var barGroups = bars.GroupBy(m => new { m.FEAType, m.Release.Name });

                BHoMObjectNameComparer comparer = new BHoMObjectNameComparer();

                ReduceRuntime(true);

                foreach (var barGroup in barGroups)
                {
                    List<MeshSettings1D> distinctMeshes = barGroup.Select(x => x.FindFragment<MeshSettings1D>())
                        .Distinct<MeshSettings1D>(comparer)
                        .ToList();

                    foreach (MeshSettings1D mesh in distinctMeshes)
                    {
                        CreateMeshSettings1D(mesh, barGroup.First().FEAType, barGroup.First().Release);
                    }

                    foreach (Bar bar in barGroup)
                    {
                        bar.Fragments.Add(distinctMeshes.First(x => comparer.Equals(x, bar.FindFragment<MeshSettings1D>())));
                        IFLine lusasLine = CreateLine(bar);

                        if (lusasLine == null)
                        {
                            return false;
                        }
                    }
                }
                ReduceRuntime(false);
                d_LusasData.resetMesh();
                d_LusasData.updateMesh();

                return true;
            }
            else
            {
                ReduceRuntime(true);

                foreach (Bar bar in bars)
                {
                    IFLine lusasLine = CreateLine(bar);

                    if (lusasLine == null)
                    {
                        return false;
                    }
                }

                ReduceRuntime(false);

                return true;
            }
        }

        /***************************************************/

        private bool CreateCollection(IEnumerable<Panel> panels)
        {

            CreateTags(panels);

            if (panels.Any(x => x.Fragments.Contains(typeof(MeshSettings2D))))
            {
                BHoMObjectNameComparer comparer = new BHoMObjectNameComparer();
                List<MeshSettings2D> distinctMeshes = panels.Select(x => x.FindFragment<MeshSettings2D>())
                    .Distinct<MeshSettings2D>(comparer)
                    .ToList();

                foreach (MeshSettings2D mesh in distinctMeshes)
                {
                    CreateMeshSettings2D(mesh);
                }

                foreach (Panel panel in panels)
                {
                    panel.AddFragment(distinctMeshes.First(x => comparer.Equals(x, (panel.FindFragment<MeshSettings2D>()))));
                }

            }

            List<Edge> PanelEdges = new List<Edge>();

            foreach (Panel Panel in panels)
            {
                PanelEdges.AddRange(Panel.ExternalEdges);
            }

            List<Edge> distinctEdges = Engine.Adapters.Lusas.Query.GetDistinctEdges(PanelEdges);

            List<Point> midPoints = new List<Point>();

            foreach (Edge edge in distinctEdges)
            {
                midPoints.Add(edge.Curve.IPointAtParameter(0.5));
            }

            ReduceRuntime(true);

            foreach (Panel panel in panels)
            {
                IFLine[] lusasLines = new IFLine[panel.ExternalEdges.Count];
                List<Edge> edges = panel.ExternalEdges;

                for (int i = 0; i < panel.ExternalEdges.Count; i++)
                {
                    Edge edge = distinctEdges[midPoints.FindIndex(
                        m => m.Equals(edges[i].Curve.IPointAtParameter(0.5).ClosestPoint(midPoints)))];

                    lusasLines[i] = d_LusasData.getLineByNumber(edge.AdapterId<int>(typeof(LusasId)));
                }

                IFSurface lusasSurface = CreateSurface(panel, lusasLines);
            }

            ReduceRuntime(false);

            return true;
        }

        /***************************************************/

        private bool CreateCollection(IEnumerable<Edge> edges)
        {
            List<Point> allPoints = new List<Point>();

            List<Edge> distinctEdges = Engine.Adapters.Lusas.Query.GetDistinctEdges(edges);

            foreach (Edge edge in distinctEdges)
            {
                allPoints.Add(edge.Curve.IStartPoint());
                allPoints.Add(edge.Curve.IEndPoint());
            }

            List<Point> distinctPoints = Engine.Adapters.Lusas.Query.GetDistinctPoints(allPoints);

            List<Point> existingPoints = ReadPoints();
            List<Point> pointsToPush = distinctPoints.Except(
                existingPoints, new PointDistanceComparer()).ToList();

            ReduceRuntime(true);

            foreach (Point point in pointsToPush)
            {
                IFPoint lusasPoint = CreatePoint(point);
            }

            ReduceRuntime(false);

            List<IFPoint> lusasPoints = ReadLusasPoints();
            List<Point> points = new List<Point>();

            foreach (IFPoint point in lusasPoints)
            {
                points.Add(Adapters.Lusas.Convert.ToPoint(point));
            }

            CreateTags(distinctEdges);

            ReduceRuntime(true);

            foreach (Edge edge in distinctEdges)
            {
                IFPoint startPoint = lusasPoints[points.FindIndex(
                    m => m.Equals(edge.Curve.IStartPoint().ClosestPoint(points)))];
                IFPoint endPoint = lusasPoints[points.FindIndex(
                    m => m.Equals(edge.Curve.IEndPoint().ClosestPoint(points)))];
                IFLine lusasLine = CreateEdge(edge, startPoint, endPoint);
            }

            ReduceRuntime(false);

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

        private bool CreateCollection(IEnumerable<IMaterialFragment> materials)
        {
            foreach (IMaterialFragment material in materials)
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

        private bool CreateCollection(IEnumerable<PointLoad> pointLoads)
        {

            foreach (PointLoad PointLoad in pointLoads)
            {
                object[] assignedPoints = GetAssignedPoints(PointLoad);
                IFLoadingConcentrated lusasPointLoad = CreateConcentratedLoad(PointLoad, assignedPoints);

                if (lusasPointLoad == null)
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
                object[] assignedLines = GetAssignedLines(barUniformlyDistributedLoad);

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

        private bool CreateCollection(IEnumerable<AreaUniformlyDistributedLoad> areaUniformlyDistributedLoads)
        {
            foreach (AreaUniformlyDistributedLoad areaUniformlyDistributedLoad in areaUniformlyDistributedLoads)
            {
                object[] assignedSurfaces = GetAssignedSurfaces(areaUniformlyDistributedLoad);
                if (areaUniformlyDistributedLoad.Axis == LoadAxis.Global)
                {
                    IFLoadingGlobalDistributed lusasGlobalDistributed =
                        CreateGlobalDistributedLoadSurface(areaUniformlyDistributedLoad, assignedSurfaces);

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
                object[] arrayLines = GetAssignedLines(barTemperatureLoad);
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
                object[] assignedLines = GetAssignedSurfaces(areaTemperatureLoad);
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
                object[] assignedPoints = GetAssignedPoints(pointDisplacement);
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
                object[] assignedLines = GetAssignedLines(barPointLoad);
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
                object[] assignedBars = GetAssignedLines(barDistributedLoad);
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


