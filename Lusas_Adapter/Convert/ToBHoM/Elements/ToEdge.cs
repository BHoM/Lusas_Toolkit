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
using BH.oM.Adapters.Lusas;
using BH.oM.Structure.Elements;
using BH.oM.Structure.Constraints;
using BH.oM.Geometry;
using Lusas.LPI;
using BH.Engine.Adapter;
using System.Linq;


namespace BH.Adapter.Adapters.Lusas
{
    public static partial class Convert
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static Edge ToEdge(this IFLine lusasLine,
            Dictionary<string, Node> nodes, Dictionary<string, Constraint6DOF> supports, HashSet<string> groupNames)
        {
            Node startNode = GetNode(lusasLine, 0, nodes);
            Node endNode = GetNode(lusasLine, 1, nodes);

            Point startPoint = startNode.Position;
            Point endPoint = endNode.Position;

            HashSet<string> tags = new HashSet<string>(IsMemberOf(lusasLine, groupNames));

            List<string> supportAssignments = GetAttributeAssignments(lusasLine, "Support");

            Constraint6DOF barConstraint = null;
            if (!(supportAssignments.Count() == 0))
            {
                supports.TryGetValue(supportAssignments[0], out barConstraint);
            }

            Line line = new Line { Start = startPoint, End = endPoint };
            Edge edge = new Edge { Curve = line, Tags = tags, Support = barConstraint };

            string adapterID = lusasLine.getID().ToString();
            edge.SetAdapterId(typeof(LusasId), adapterID);

            return edge;
        }

        /***************************************************/

    }
}