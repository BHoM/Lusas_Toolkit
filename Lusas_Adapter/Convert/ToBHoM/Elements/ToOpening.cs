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

        public static Opening ToOpening(this IFSurface lusasSurface,
            int boundaryIndex,
            Dictionary<string, Edge> edges,
            HashSet<string> groupNames
            )

        {
            object[] lusasSurfaceLines = lusasSurface.getBoundaryLOFs(boundaryIndex);

            HashSet<string> tags = new HashSet<string>(IsMemberOf(lusasSurface, groupNames));

            List<ICurve> openingEdges = new List<ICurve>();

            for (int i = 0; i < lusasSurfaceLines.Length; i++)
            {
                Edge edge = GetInnerEdge(lusasSurface, boundaryIndex, i, edges);
                openingEdges.Insert(0, edge.Curve);
            }
            
            Opening opening = Engine.Structure.Create.Opening(openingEdges);

            opening.Tags = tags;

            //Takes both the ID of the surface and the index of the "hole" from Lusas.
            string adapterID = lusasSurface.getID().ToString() + "_" + boundaryIndex.ToString();
            opening.SetAdapterId(typeof(LusasId), adapterID);

            return opening;
        }

        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

       private static Edge GetInnerEdge(IFSurface lusasSurf, int boundaryIndex, int lineIndex, Dictionary<string, Edge> edges)
        {
            Edge edge;
            IFLine lusasEdge = lusasSurf.getBoundaryLOFs(boundaryIndex)[lineIndex];
            edges.TryGetValue(lusasEdge.getID().ToString(), out edge);
            return edge;
        }
 
        /***************************************************/

    }
}