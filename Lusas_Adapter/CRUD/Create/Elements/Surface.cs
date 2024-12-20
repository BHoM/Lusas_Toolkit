/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2025, the respective contributors. All rights reserved.
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

using BH.oM.Adapters.Lusas;
using BH.Engine.Adapter;
using BH.oM.Structure.Elements;
using Lusas.LPI;
using BH.oM.Adapters.Lusas.Fragments;
using BH.Engine.Base;
using System.Linq;
using System.Collections.Generic;
using BH.Engine.Spatial;
using BH.oM.Geometry;
using BH.Engine.Geometry;

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
#elif Debug211 || Release211
    public partial class LusasV211Adapter
#else
    public partial class LusasV17Adapter
#endif
    {
        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        private IFSurface CreateSurface(Panel panel)
        {
            List<IFLine> edges = new List<IFLine>();

            foreach (Edge edge in panel.ExternalEdges)
            {
                string edgeId = GetAdapterId<string>(edge);

                if (string.IsNullOrEmpty(edgeId))
                {
                    Engine.Base.Compute.RecordError("Could not find the ids for at least one Edge, Panel not created.");
                    return null;
                }
                else
                {
                    edges.Add(d_LusasData.getLineByNumber(edgeId));
                }
            }

            IFSurface lusasSurface = d_LusasData.createSurfaceBy(edges.ToArray());

            List<IFSurface> openings = new List<IFSurface>();

            foreach (Opening opening in panel.Openings)
               {
                string openingID = GetAdapterId<string>(opening);

                if (string.IsNullOrEmpty(openingID))
                {
                    Engine.Base.Compute.RecordError($"Could not find the ids for at least one of the Openings on Panel {lusasSurface.getID()}, this Opening has not been created.");
                    continue;
                }


                if (EdgeIntersection(opening.Edges, panel.ExternalEdges))
                {
                    Engine.Base.Compute.RecordError($"At least one Edge defining Panel {lusasSurface.getID()} intersects with at least one Edge defining an Opening, this Opening has not been created.");
                    continue;
                }
                   

                if (!Engine.Geometry.Query.IsCoplanar(opening.FitPlane(), panel.FitPlane(), m_mergeTolerance))
                {
                    Engine.Base.Compute.RecordError($"The geometry defining Panel {lusasSurface.getID()} is not Coplanar with an Opening, this Opening has not been created.");
                    continue;
                }
                
                IFObjectSet lusasSelection = m_LusasApplication.newObjectSet();
                IFGeometryData lusasGeometryData = m_LusasApplication.newGeometryData();

                lusasGeometryData.setAllDefaults();
                lusasGeometryData.trimOuterBoundaryOff();
                lusasGeometryData.trimDeleteOuterBoundaryOff();
                lusasGeometryData.trimDeleteTrimmingLinesOn();

                lusasSelection.add(lusasSurface, "Surface");
                lusasSelection.add(d_LusasData.getSurfaceByNumber(openingID), "Surface");

                lusasSelection.trim(lusasGeometryData);
            }

                if (lusasSurface != null)
            {
                long adapterIdName = lusasSurface.getID();
                panel.SetAdapterId(typeof(LusasId), adapterIdName);

                if (!(panel.Tags.Count == 0))
                {
                    AssignObjectSet(lusasSurface, panel.Tags);
                }

                if (CheckPropertyWarning(panel, p => p.Property) && !Engine.Adapters.Lusas.Query.InvalidSurfaceProperty(panel.Property))
                {
                    IFAttribute lusasGeometricSurface = d_LusasData.getAttribute("Surface Geometric", panel.Property.AdapterId<int>(typeof(LusasId)));

                    lusasGeometricSurface.assignTo(lusasSurface);
                    if (CheckPropertyWarning(panel, p => p.Property.Material))
                    {
                        IFAttribute lusasMaterial = d_LusasData.getAttribute("Material", panel.Property.Material.AdapterId<int>(typeof(LusasId)));
                        lusasMaterial.assignTo(lusasSurface);
                    }
                }

                if (panel.Fragments.Contains(typeof(MeshSettings2D)))
                {
                    IFAssignment meshAssignment = m_LusasApplication.newAssignment();
                    meshAssignment.setAllDefaults();

                    MeshSettings2D meshSettings2D = panel.FindFragment<MeshSettings2D>();
                    IFMeshAttr mesh = d_LusasData.getMesh(meshSettings2D.Name);
                    mesh.assignTo(lusasSurface, meshAssignment);
                }
            }

            return lusasSurface;
        }

        private static bool EdgeIntersection(List<Edge> openingEdges, List<Edge> panelEdges)
        {
            foreach (Edge openingEdge in openingEdges)
            {
                foreach (Edge panelEdge in panelEdges)
                {
                    if (openingEdge.Curve.ICurveIntersections(panelEdge.Curve).Count != 0)
                        return true;
                }
            }
            return false;
        }

        private IFSurface CreateSurface(Opening opening)
        {
            List<IFLine> edges = new List<IFLine>();

            foreach (Edge edge in opening.Edges)
            {
                string edgeId = GetAdapterId<string>(edge);

                if (string.IsNullOrEmpty(edgeId))
                {
                    Engine.Base.Compute.RecordError("Could not find the ids for at least one Edge, Opening not created.");
                    return null;
                }
                else
                {
                    edges.Add(d_LusasData.getLineByNumber(edgeId));
                }
            }

            IFSurface lusasSurface = d_LusasData.createSurfaceBy(edges.ToArray());

            if (lusasSurface != null)
            {
                long adapterIdName = lusasSurface.getID();
                opening.SetAdapterId(typeof(LusasId), adapterIdName);

                if (!(opening.Tags.Count == 0))
                {
                    AssignObjectSet(lusasSurface, opening.Tags);
                }

            }

            return lusasSurface;
        }

        /***************************************************/

    }
}




