/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2023, the respective contributors. All rights reserved.
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
using BH.oM.Structure.Elements;
using Lusas.LPI;
using BH.oM.Base;
using BH.Engine.Adapters.Lusas;

namespace BH.Adapter.Adapters.Lusas
{
    public static partial class Convert
    {
        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        private static IEnumerable<IAreaElement> GetSurfaceAssignments(IEnumerable<IFAssignment> lusasAssignments,
            Dictionary<string, Panel> panels)
        {
            List<IAreaElement> assignedSurfs = new List<IAreaElement>();
            Panel panel;

            foreach (IFAssignment lusasAssignment in lusasAssignments)
            {
                if (lusasAssignment.getDatabaseObject() is IFSurface)
                {
                    IFSurface lusasSurface = (IFSurface)lusasAssignment.getDatabaseObject();
                    panels.TryGetValue(lusasSurface.getID().ToString(), out panel);
                    assignedSurfs.Add(panel);
                }
                else
                {
                    AssignmentWarning(lusasAssignment);
                }
            }

            return assignedSurfs;
        }

        /***************************************************/

    }
}



