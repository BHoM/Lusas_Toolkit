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

using System.Collections.Generic;
using System.Linq;
using BH.oM.Structure.Elements;
using BH.oM.Structure.SurfaceProperties;
using BH.oM.Structure.Constraints;
using BH.oM.Geometry;
using BH.oM.Structure.MaterialFragments;
using Lusas.LPI;
using BH.Adapter.Lusas;

namespace BH.Adapter.External.Lusas
{
    public static partial class Convert
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static Panel ToPanel(this IFSurface lusasSurface,
            Dictionary<string, Edge> bhomEdges,
            HashSet<string> groupNames,
            Dictionary<string, ISurfaceProperty> bhom2DProperties,
            Dictionary<string, IMaterialFragment> bhomMaterials,
            Dictionary<string, Constraint4DOF> bhomSupports)

        {
            object[] lusasSurfaceLines = lusasSurface.getLOFs();
            List<ICurve> dummyCurve = new List<ICurve>();

            int n = lusasSurfaceLines.Length;
            HashSet<string> tags = new HashSet<string>(LusasAdapter.IsMemberOf(lusasSurface, groupNames));

            List<Edge> surfaceEdges = new List<Edge>();

            for (int i = 0; i < n; i++)
            {
                Edge bhomEdge = GetEdge(lusasSurface, i, bhomEdges);
                surfaceEdges.Add(bhomEdge);
            }

            Panel bhomPanel =Engine.Structure.Create.Panel(surfaceEdges, dummyCurve);

            bhomPanel.Tags = tags;
            bhomPanel.CustomData[AdapterIdName] = lusasSurface.getName();
            
            List<string> geometricAssignments = LusasAdapter.GetAttributeAssignments(lusasSurface, "Geometric");
            List<string> materialAssignments = LusasAdapter.GetAttributeAssignments(lusasSurface, "Material");

            IMaterialFragment panelMaterial = null;
            ISurfaceProperty bhomProperty2D = null;

            if (!(geometricAssignments.Count() == 0))
            {
                bhom2DProperties.TryGetValue(geometricAssignments[0], out bhomProperty2D);
                if (!(materialAssignments.Count() == 0))
                {
                    bhomMaterials.TryGetValue(materialAssignments[0], out panelMaterial);
                    bhomProperty2D.Material = panelMaterial;
                }

                bhomPanel.Property = bhomProperty2D;
            }

            return bhomPanel;
        }

        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        private static Edge GetEdge(IFSurface lusasSurf, int lineIndex, Dictionary<string, Edge> bhomBars)
        {
            Edge bhomEdge = null;
            IFLine lusasEdge = lusasSurf.getLOFs()[lineIndex];
            string lineName = Engine.External.Lusas.Modify.RemovePrefix(lusasEdge.getName(), "L");
            bhomBars.TryGetValue(lineName, out bhomEdge);
            return bhomEdge;
        }

        /***************************************************/

    }
}
