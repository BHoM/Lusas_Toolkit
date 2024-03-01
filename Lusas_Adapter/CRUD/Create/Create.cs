/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2024, the respective contributors. All rights reserved.
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

using System;
using System.Reflection;
using System.IO;
using System.Diagnostics;
using BH.Engine.Geometry;
using BH.Engine.Adapters.Lusas.Object_Comparer.Equality_Comparer;
using BH.oM.Adapter;
using BH.oM.Structure.Elements;
using BH.oM.Geometry;
using BH.oM.Structure.Constraints;
using BH.oM.Structure.SectionProperties;
using BH.oM.Structure.SurfaceProperties;
using BH.oM.Structure.Loads;
using BH.oM.Structure.MaterialFragments;
using Lusas.LPI;
using System.Collections.Generic;
using System.Linq;
using BH.Engine.Base.Objects;
using BH.Engine.Base;
using BH.oM.Adapters.Lusas.Fragments;
using System.Windows.Forms.VisualStyles;
using BH.Engine.Spatial;

namespace BH.Adapter.Lusas
{
#if Debug18 || Release18
    public partial class LusasV18Adapter
#elif Debug19 || Release19
    public partial class LusasV19Adapter
#elif Debug191 || Release191
    public partial class LusasV191Adapter
#elif Debug200 || Release200
    public partial class LusasV200Adapter
#elif Debug210 || Release210
    public partial class LusasV210Adapter
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
                else if (objects.First() is Bar)
                {
                    success = CreateCollection(objects as IEnumerable<Bar>);
                }
                else if (objects.First() is Panel)
                {
                    success = CreateCollection(objects as IEnumerable<Panel>);
                }
                else if (objects.First() is Edge)
                {
                    success = CreateCollection(objects as IEnumerable<Edge>);
                }
                else if (objects.First() is Opening)
                {
                    success = CreateCollection(objects as IEnumerable<Opening>);
                }
                else if (objects.First() is Point)
                {
                    success = CreateCollection(objects as IEnumerable<Point>);
                }
                else if (objects.First() is IMaterialFragment)
                {
                    success = CreateCollection(objects as IEnumerable<IMaterialFragment>);
                }
                else if (objects.First() is Constraint6DOF)
                {
                    success = CreateCollection(objects as IEnumerable<Constraint6DOF>);
                }
                else if (objects.First() is Constraint4DOF)
                {
                    success = CreateCollection(objects as IEnumerable<Constraint4DOF>);
                }
                else if (objects.First() is Loadcase)
                {
                    success = CreateCollection(objects as IEnumerable<Loadcase>);
                }
                else if (objects.First() is LoadCombination)
                {
                    success = CreateCollection(objects as IEnumerable<LoadCombination>);
                }
                else if (typeof(ILoad).IsAssignableFrom(objects.First().GetType()))
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
                        case "BH.oM.Structure.Loads.BarUniformTemperatureLoad":
                            success = CreateCollection(objects as IEnumerable<BarUniformTemperatureLoad>);
                            break;
                        case "BH.oM.Structure.Loads.AreaUniformTemperatureLoad":
                            success = CreateCollection(objects as IEnumerable<AreaUniformTemperatureLoad>);
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
                else if (typeof(ISurfaceProperty).IsAssignableFrom(objects.First().GetType()))
                {
                    success = CreateCollection(objects as IEnumerable<ISurfaceProperty>);
                }
                else if (typeof(ISectionProperty).IsAssignableFrom(objects.First().GetType()))
                {
                    success = CreateCollection(objects as IEnumerable<ISectionProperty>);
                }
                else if (objects.First() is MeshSettings1D)
                {
                    success = CreateCollection(objects as IEnumerable<MeshSettings1D>);
                }
                else if (objects.First() is MeshSettings2D)
                {
                    success = CreateCollection(objects as IEnumerable<MeshSettings2D>);
                }
                else
                {
                    Engine.Base.Compute.RecordError("Object is not supported in the Lusas_Toolkit.");
                }
            }

            return success;
        }

        /***************************************************/
        /**** Private methods                           ****/
        /***************************************************/

        private bool CreateCollection(IEnumerable<Node> nodes)
        {
            if (nodes != null)
            {
                CreateTags(nodes);

                foreach (Node node in nodes)
                {
                    IFPoint lusasPoint = CreatePoint(node);
                }

            }

            return true;
        }

        /***************************************************/

        private bool CreateCollection(IEnumerable<Point> points)
        {
            if (points != null)
            {
                List<Point> distinctPoints = Engine.Adapters.Lusas.Query.GetDistinctPoints(points);

                List<Point> existingPoints = ReadPoints();

                List<Point> lusasPoints = distinctPoints.Except(existingPoints).ToList();

                foreach (Point point in lusasPoints)
                {
                    IFPoint lusasPoint = CreatePoint(point);
                }
            }

            return true;
        }

        /***************************************************/

        private bool CreateCollection(IEnumerable<Bar> bars)
        {
            if (bars != null)
            {
                CreateTags(bars);

                if (bars.Any(x => x.Fragments.Contains(typeof(MeshSettings1D))))
                {
                    var barGroups = bars.GroupBy(m => new { m.FEAType, m.Release?.Name });

                    BHoMObjectNameComparer comparer = new BHoMObjectNameComparer();

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
                            bar.AddFragment(distinctMeshes.First(x => comparer.Equals(x, bar.FindFragment<MeshSettings1D>())), true);
                            IFLine lusasLine = CreateLine(bar);
                        }
                    }

                    d_LusasData.resetMesh();
                    d_LusasData.updateMesh();
                }
                else
                {
                    foreach (Bar bar in bars)
                    {
                        IFLine lusasLine = CreateLine(bar);
                    }
                }
            }

            return true;
        }

        /***************************************************/

        private bool CreateCollection(IEnumerable<Panel> panels)
        {
            if (panels != null)
            {
                CreateTags(panels);

                List<Panel> validPanels = new List<Panel>();

                foreach (Panel panel in panels)
                {
                    if (panel.ExternalEdges.Count > 0)
                    {
                        if (CheckPropertyError(panel, p => p.ExternalEdges))
                            if (CheckPropertyError(panel.ExternalEdges, e => e.Select(x => x.Curve)))
                                if (panel.ExternalEdges.All(x => x != null) && panel.ExternalEdges.Select(x => x.Curve).All(y => y != null))
                                {                                    
                                    if (panel.ExternalEdges.All(x => !Engine.Adapters.Lusas.Query.InvalidEdge(x)))
                                    {
                                        if (Engine.Spatial.Query.IsPlanar(panel, false, Tolerance.MacroDistance))
                                        {
                                            for (int i = 0; i < panel.ExternalEdges.Count; i++)
                                            {
                                                if (!CheckPropertyError(panel, p => panel.ExternalEdges[i]) && Engine.Adapters.Lusas.Query.InvalidEdge(panel.ExternalEdges[i]))
                                                    break;

                                                if (i == panel.ExternalEdges.Count - 1)
                                                    validPanels.Add(panel);
                                            }
                                        }
                                        else
                                            Engine.Base.Compute.RecordError("The geometry defining the Panel is not Planar, and therefore the Panel will not be created.");
                                    }
                                    else
                                        Engine.Base.Compute.RecordError("One or more of the External Edges of the Panel are invalid.");
                                }
                                else
                                    Engine.Base.Compute.RecordError("One of more of the External Edges of the Panel or Curves defining the External Edges are null.");
                    }
                    else
                        Engine.Base.Compute.RecordError($"An object of type {panel.GetType().Name} could not be created due to a property of type {typeof(Edge).Name} being null. Please check your input data!");
                }

                if (validPanels.Any(x => x.Fragments.Contains(typeof(MeshSettings2D))))
                {
                    BHoMObjectNameComparer comparer = new BHoMObjectNameComparer();
                    List<MeshSettings2D> distinctMeshes = panels.Select(x => x.FindFragment<MeshSettings2D>())
                        .Distinct<MeshSettings2D>(comparer)
                        .ToList();

                    foreach (MeshSettings2D mesh in distinctMeshes)
                        CreateMeshSettings2D(mesh);

                    foreach (Panel validPanel in validPanels)
                        validPanel.AddFragment(distinctMeshes.First(x => comparer.Equals(x, (validPanel.FindFragment<MeshSettings2D>()))), true);
                }

                IFSurface lusasSurface = null;

                foreach (Panel validPanel in validPanels)
                    lusasSurface = CreateSurface(validPanel);

            }

            return true;
        }

        /***************************************************/

        private bool CreateCollection(IEnumerable<Edge> edges)
        {
            if (edges != null)
            {
                //Check List<Curve> is not null and Curve is not invalid (i.e. not a Line)
                List<Edge> validEdges = edges.Where(x => CheckPropertyError(x, y => y.Curve))
                    .Where(x => !Engine.Adapters.Lusas.Query.InvalidEdge(x)).ToList();

                List<Edge> distinctEdges = new List<Edge>();

                //Check Curve is not a null
                foreach (Edge edge in validEdges)
                {
                    if (!(edge.Curve == null))
                        distinctEdges.Add(edge);
                }

                List<Point> distinctPoints = distinctEdges.Select(x => x.Curve.IStartPoint()).Union(edges.Select(x => x.Curve.IEndPoint())).ToList();

                List<Point> existingPoints = ReadPoints();
                List<Point> pointsToPush = distinctPoints.Except(existingPoints, new PointDistanceComparer()).ToList();

                foreach (Point point in pointsToPush)
                {
                    IFPoint lusasPoint = CreatePoint(point);
                }

                List<Point> points = ReadPoints();

                List<IFPoint> lusasPoints = ReadLusasPoints();

                CreateTags(distinctEdges);

                foreach (Edge edge in distinctEdges)
                {
                    IFPoint startPoint = lusasPoints[points.FindIndex(
                        m => m.Equals(edge.Curve.IStartPoint().ClosestPoint(points)))];
                    IFPoint endPoint = lusasPoints[points.FindIndex(
                        m => m.Equals(edge.Curve.IEndPoint().ClosestPoint(points)))];
                    IFLine lusasLine = CreateEdge(edge, startPoint, endPoint);
                }
            }

            return true;
        }

        /***************************************************/

        private bool CreateCollection(IEnumerable<Opening> openings)
        {
            if (openings != null)
            {
                CreateTags(openings);

                List<Opening> validOpenings = new List<Opening>();

                foreach (Opening opening in openings)
                {
                    if (CheckPropertyError(opening, p => p.Edges))
                        if (CheckPropertyError(opening.Edges, e => e.Select(x => x.Curve)))
                        {
                            if (opening.Edges.All(x => !Engine.Adapters.Lusas.Query.InvalidEdge(x)))
                            {
                                if (Engine.Spatial.Query.IsPlanar(opening, false, Tolerance.MacroDistance)) //Check if this works.
                                {
                                    //if (Engine.Geometry.Query.IsCoplanar(opening.FitPlane(), panel.FitPlane(), Tolerance.MacroDistance)) //Fix this check. Move this check.
                                    //{
                                        for (int i = 0; i < opening.Edges.Count; i++)
                                        {
                                            //Need to have a think which errors should give a warning and which should give an error... 
                                            //Not fun to have it structured like this. Surely there is a better way...

                                            if (!CheckPropertyError(opening, p => opening.Edges[i]) && Engine.Adapters.Lusas.Query.InvalidEdge(opening.Edges[i]))
                                                break;

                                            if (i == opening.Edges.Count - 1)
                                                validOpenings.Add(opening);
                                        }
                                    //}
                                    //else
                                        //Engine.Base.Compute.RecordError("The geometry defining the Panel is not Coplanar with at least one of the Openings, and therefore the Panel will not be created.");
                                }
                                else
                                    Engine.Base.Compute.RecordError("The geometry defining one of the Openings of the Panel is not Planar, and therefore the Panel will not be created.");
                            }
                            else
                                Engine.Base.Compute.RecordError("One or more of the Internal Edges of the Panel are invalid.");
                        }
                        else
                            Engine.Base.Compute.RecordError("One of more of the Internal Edges of the Panel or Curves defining the Opening are null.");
                }


                IFSurface lusasSurface = null;

                foreach (Opening validOpening in validOpenings)
                    lusasSurface = CreateSurface(validOpening);
            }
            return true;
        }

        /***************************************************/

        private bool CreateCollection(IEnumerable<ISectionProperty> sectionProperties)
        {
            foreach (ISectionProperty sectionProperty in sectionProperties)
            {
                if (sectionProperties != null)
                {
                    IFAttribute lusasGeometricLine = CreateGeometricLine(sectionProperty);
                }
            }

            return true;
        }

        /***************************************************/

        private bool CreateCollection(IEnumerable<IMaterialFragment> materials)
        {
            foreach (IMaterialFragment material in materials)
            {
                if (material != null)
                {
                    IFAttribute lusasMaterial = CreateMaterial(material);
                }
            }

            return true;
        }

        /***************************************************/

        private bool CreateCollection(IEnumerable<ISurfaceProperty> surfaceProperties)
        {
            foreach (ISurfaceProperty surfaceProperty in surfaceProperties)
            {
                if (surfaceProperty != null)
                {
                    IFAttribute lusasGeometricSurface = CreateGeometricSurface(surfaceProperty);
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

        private bool CreateCollection(IEnumerable<BarUniformTemperatureLoad> barUniformTemperatureLoads)
        {
            foreach (BarUniformTemperatureLoad barUniformTemperatureLoad in barUniformTemperatureLoads)
            {
                object[] arrayLines = GetAssignedLines(barUniformTemperatureLoad);
                IFLoadingTemperature lusasBarUniformTemperatureLoad =
                    CreateBarUniformTemperatureLoad(barUniformTemperatureLoad, arrayLines);

                if (lusasBarUniformTemperatureLoad == null)
                {
                    return false;
                }
            }

            return true;
        }

        /***************************************************/

        private bool CreateCollection(IEnumerable<AreaUniformTemperatureLoad> areaUniformTemperatureLoads)
        {
            foreach (AreaUniformTemperatureLoad areaUniformTemperatureLoad in areaUniformTemperatureLoads)
            {
                object[] assignedLines = GetAssignedSurfaces(areaUniformTemperatureLoad);
                IFLoadingTemperature lusasAreaUniformTemperatureLoad =
                    CreateAreaUniformTemperatureLoad(areaUniformTemperatureLoad, assignedLines);

                if (lusasAreaUniformTemperatureLoad == null)
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

            }

            return true;
        }

        /***************************************************/

        private bool CreateCollection(IEnumerable<Constraint6DOF> constraints)
        {
            foreach (Constraint6DOF constraint in constraints)
            {
                if (constraint != null)
                {
                    IFAttribute lusasSupport = CreateSupport(constraint);
                    if (lusasSupport == null)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private bool CreateCollection(IEnumerable<Constraint4DOF> constraints)
        {
            foreach (Constraint4DOF constraint in constraints)
            {
                if (constraint != null)
                {
                    IFAttribute lusasSupport = CreateSupport(constraint);
                    if (lusasSupport == null)
                    {
                        return false;
                    }
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
                if (meshSettings1D != null)
                {
                    IFMeshLine lusasLineMesh = CreateMeshSettings1D(meshSettings1D);
                }
            }

            return true;
        }

        /***************************************************/

        private bool CreateCollection(IEnumerable<MeshSettings2D> meshSettings2Ds)
        {

            foreach (MeshSettings2D meshSettings2D in meshSettings2Ds)
            {
                if (meshSettings2D != null)
                {
                    IFMeshSurface lusasSurfaceMesh = CreateMeshSettings2D(meshSettings2D);
                }
            }

            return true;
        }

        /***************************************************/

    }
}