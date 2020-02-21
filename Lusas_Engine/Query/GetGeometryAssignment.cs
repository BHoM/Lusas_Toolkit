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
using BH.oM.Structure.Elements;
using Lusas.LPI;
using BH.oM.Base;

namespace BH.Engine.Lusas
{
    public static partial class Query
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static IEnumerable<BHoMObject> GetGeometryAssignments(IEnumerable<IFAssignment> lusasAssignments,
            Dictionary<string, Node> bhomNodes, Dictionary<string, Bar> bhomBars,
            Dictionary<string, Panel> bhomPanels)
        {
            List<BHoMObject> assignedObjects = new List<BHoMObject>();

            Node bhomNode = new Node();
            Bar bhomBar = new Bar();
            Panel bhomPanel = new Panel();

            foreach (IFAssignment lusasAssignment in lusasAssignments)
            {
                IFGeometry lusasGeometry = (IFGeometry)lusasAssignment.getDatabaseObject();

                if (lusasGeometry is IFPoint)
                {
                    bhomNodes.TryGetValue(Engine.Lusas.Modify.RemovePrefix(lusasGeometry.getName(), "P"), out bhomNode);
                    assignedObjects.Add(bhomNode);
                }
                else if (lusasGeometry is IFLine)
                {
                    bhomBars.TryGetValue(Engine.Lusas.Modify.RemovePrefix(lusasGeometry.getName(), "L"), out bhomBar);
                    assignedObjects.Add(bhomBar);
                }
                else if (lusasGeometry is IFSurface)
                {
                    bhomPanels.TryGetValue(Engine.Lusas.Modify.RemovePrefix(lusasGeometry.getName(), "S"), out bhomPanel);
                    assignedObjects.Add(bhomPanel);
                }
            }

            return assignedObjects;
        }

        /***************************************************/

    }
}
