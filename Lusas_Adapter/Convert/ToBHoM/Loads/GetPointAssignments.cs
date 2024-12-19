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

        private static IEnumerable<Node> GetPointAssignments(IEnumerable<IFAssignment> lusasAssignments,
               Dictionary<string, Node> nodes)
        {
            List<Node> assignedNodes = new List<Node>();
            Node node;

            foreach (IFAssignment lusasAssignment in lusasAssignments)
            {
                if (lusasAssignment.getDatabaseObject() is IFPoint)
                {
                    IFPoint lusasPoint = (IFPoint)lusasAssignment.getDatabaseObject();
                    nodes.TryGetValue(lusasPoint.getID().ToString(), out node);
                    assignedNodes.Add(node);
                }
                else
                {
                    AssignmentWarning(lusasAssignment);
                }
            }

            return assignedNodes;
        }

        /***************************************************/

    }
}





