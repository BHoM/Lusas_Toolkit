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
using BH.oM.Structure.Loads;
using Lusas.LPI;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        private List<ILoad> ReadPointForces(List<string> ids = null)
        {
            List<ILoad> bhomPointForces = new List<ILoad>();
            object[] lusasConcentratedLoads = d_LusasData.getAttributes("Concentrated Load");

            List<Node> bhomNodes = ReadNodes();
            Dictionary<string, Node> nodeDictionary = bhomNodes.ToDictionary(
                x => x.CustomData[AdapterId].ToString());

            List<IFLoadcase> allLoadcases = new List<IFLoadcase>();

            for (int i = 0; i < lusasConcentratedLoads.Count(); i++)
            {
                IFLoading lusasConcentratedLoad = (IFLoading)lusasConcentratedLoads[i];

                IEnumerable<IGrouping<string, IFAssignment>> groupedByLoadcases =
                    GetLoadAssignments(lusasConcentratedLoad);

                foreach (IEnumerable<IFAssignment> groupedAssignment in groupedByLoadcases)
                {
                    PointForce bhomPointForce = Engine.Lusas.Convert.ToPointForce(
                        lusasConcentratedLoad, groupedAssignment, nodeDictionary
                        );
                    List<string> analysisName = new List<string> { lusasConcentratedLoad.getAttributeType() };
                    bhomPointForce.Tags = new HashSet<string>(analysisName);
                    bhomPointForces.Add(bhomPointForce);
                }
            }

            return bhomPointForces;
        }
    }
}