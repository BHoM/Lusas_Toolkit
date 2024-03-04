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

using System.Collections.Generic;
using System.Linq;
using BH.oM.Adapters.Lusas.Fragments;
using BH.oM.Structure.Elements;
using BH.oM.Structure.SurfaceProperties;
using BH.oM.Structure.Constraints;
using BH.oM.Geometry;
using BH.oM.Structure.MaterialFragments;
using Lusas.LPI;
using BH.oM.Adapters.Lusas;
using BH.Engine.Adapter;
using BH.Engine.Base;

namespace BH.Adapter.Adapters.Lusas
{
    public static partial class Convert
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static Panel ToPanel(this IFSurface lusasSurface,
            Dictionary<string, Edge> edges,
            Dictionary<string, Opening> openings,
            HashSet<string> groupNames,
            Dictionary<string, ISurfaceProperty> surfaceProperties,
            Dictionary<string, IMaterialFragment> materials,
            Dictionary<string, Constraint4DOF> supports,
            Dictionary<string, MeshSettings2D> meshes)

        {
            object[] lusasSurfaceLines = lusasSurface.getBoundaryLOFs(0);

            int n = lusasSurfaceLines.Length;
            HashSet<string> tags = new HashSet<string>(IsMemberOf(lusasSurface, groupNames));

            List<Edge> surfaceEdges = new List<Edge>();

            for (int i = 0; i < n; i++)
            {
                Edge edge = GetOuterEdge(lusasSurface, i, edges);
                surfaceEdges.Add(edge);
            }

            Panel panel = Engine.Structure.Create.Panel(surfaceEdges);

            List<Opening> panelOpenings = new List<Opening>();

            for (int i = 1; i < lusasSurface.countBoundaries(); i++)
            {
                Opening opening = GetOpening(lusasSurface.getID(), i, openings);
                panelOpenings.Add(opening);
            }

            panel.Openings = panelOpenings;

            panel.Tags = tags;

            long adapterID = lusasSurface.getID();
            panel.SetAdapterId(typeof(LusasId), adapterID);

            List<string> geometricAssignments = GetAttributeAssignments(lusasSurface, "Geometric");
            List<string> materialAssignments = GetAttributeAssignments(lusasSurface, "Material");

            IMaterialFragment panelMaterial;
            ISurfaceProperty surfaceProperty;

            if (!(geometricAssignments.Count() == 0))
            {
                surfaceProperties.TryGetValue(geometricAssignments[0], out surfaceProperty);
                if (!(materialAssignments.Count() == 0))
                {
                    materials.TryGetValue(materialAssignments[0], out panelMaterial);
                    surfaceProperty.Material = panelMaterial;
                }

                panel.Property = surfaceProperty;
            }

            MeshSettings2D surfaceMesh;
            List<string> meshSettings = GetAttributeAssignments(lusasSurface, "Mesh");

            if (!(meshSettings.Count() == 0))
            {
                meshes.TryGetValue(meshSettings[0], out surfaceMesh);
                panel.Fragments.Add(surfaceMesh);
            }

            return panel;
        }

        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        private static Edge GetOuterEdge(IFSurface lusasSurf, int lineIndex, Dictionary<string, Edge> bars)
        {
            Edge edge;
            IFLine lusasEdge = lusasSurf.getBoundaryLOFs(0)[lineIndex];
            bars.TryGetValue(lusasEdge.getID().ToString(), out edge);
            return edge;
        }

        private static Opening GetOpening(int surfaceID, int boundaryIndex, Dictionary<string, Opening> openings)
        {
            Opening opening;
            openings.TryGetValue(surfaceID.ToString()+"_"+boundaryIndex.ToString(), out opening);
            return opening;
        }

        /***************************************************/

    }
}




