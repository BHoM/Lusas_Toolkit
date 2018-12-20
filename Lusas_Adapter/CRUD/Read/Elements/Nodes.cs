/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2018, the respective contributors. All rights reserved.
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
using BH.oM.Structure.Properties.Constraint;
using Lusas.LPI;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        private List<Node> ReadNodes(List<string> ids = null)
        {
            object[] lusasPoints = d_LusasData.getObjects("Point");

            List<Node> bhomNodes = new List<Node>();
            HashSet<string> groupNames = ReadTags();

            IEnumerable<Constraint6DOF> constraints6DOFList = Read6DOFConstraints();
            Dictionary<string, Constraint6DOF> constraints6DOF = constraints6DOFList.ToDictionary(
                x => x.Name.ToString());

            for (int i = 0; i < lusasPoints.Count(); i++)
            {
                IFPoint lusasPoint = (IFPoint)lusasPoints[i];
                Node bhomNode = Engine.Lusas.Convert.ToBHoMNode(lusasPoint, groupNames, constraints6DOF);
                bhomNodes.Add(bhomNode);
            }
            return bhomNodes;
        }
    }
}