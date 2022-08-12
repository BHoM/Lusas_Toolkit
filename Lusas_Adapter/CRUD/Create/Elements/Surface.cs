/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2022, the respective contributors. All rights reserved.
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
        /**** Private Methods                           ****/
        /***************************************************/

        private IFSurface CreateSurface(Panel panel)
        {
            List<IFLine> edges = new List<IFLine>();

            foreach(Edge edge in panel.ExternalEdges)
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

            if (lusasSurface != null)
            {
                int adapterIdName = lusasSurface.getID();
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

        /***************************************************/

    }
}

