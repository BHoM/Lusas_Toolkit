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
using BH.oM.Geometry;
using BH.oM.Structure.Elements;
using BH.oM.Structure.Constraints;
using Lusas.LPI;
using BH.Adapter.Lusas;
using BH.Engine.Adapters.Lusas;

namespace BH.Adapter.Adapters.Lusas
{
    public static partial class Convert
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static Node ToNode(this IFPoint lusasPoint,
            HashSet<string> groupNames, Dictionary<string, Constraint6DOF> constraint6DOFs)
        {
            HashSet<string> tags = new HashSet<string>(IsMemberOf(lusasPoint, groupNames));
            List<string> supportAssignments = GetAttributeAssignments(lusasPoint, "Support");

            Constraint6DOF nodeConstraint = null;
            if (!(supportAssignments.Count() == 0))
            {
                constraint6DOFs.TryGetValue(supportAssignments[0], out nodeConstraint);
            }

            Node node = Engine.Structure.Create.Node(
                new Point { X = lusasPoint.getX(), Y = lusasPoint.getY(), Z = lusasPoint.getZ() },
                "",
                nodeConstraint);

            node.Tags = tags;

            node.CustomData[AdapterIdName] = lusasPoint.getID();

            return node;
        }

        /***************************************************/

    }
}


