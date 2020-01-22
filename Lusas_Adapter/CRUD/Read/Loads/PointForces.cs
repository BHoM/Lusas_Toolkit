/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2019, the respective contributors. All rights reserved.
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
using System.Diagnostics;
using BH.oM.Structure.Elements;
using BH.oM.Structure.Loads;
using Lusas.LPI;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        private List<ILoad> ReadPointLoads(List<string> ids = null)
        {
            List<ILoad> bhomPointLoads = new List<ILoad>();
            object[] lusasConcentratedLoads = d_LusasData.getAttributes("Concentrated Load");

            if(!(lusasConcentratedLoads.Count()==0))
            {
                List<Node> bhomNodes = ReadNodes();
                Dictionary<string, Node> nodeDictionary = bhomNodes.ToDictionary(
                    x => x.CustomData[AdapterIdName].ToString());

                List<IFLoadcase> allLoadcases = new List<IFLoadcase>();

                for (int i = 0; i < lusasConcentratedLoads.Count(); i++)
                {
                    IFLoading lusasConcentratedLoad = (IFLoading)lusasConcentratedLoads[i];

                    IEnumerable<IGrouping<string, IFAssignment>> groupedByLoadcases =
                        Engine.Lusas.Query.GetLoadAssignments(lusasConcentratedLoad);

                    foreach (IEnumerable<IFAssignment> groupedAssignment in groupedByLoadcases)
                    {
                        PointLoad bhomPointLoad = Engine.Lusas.Convert.ToPointLoad(
                            lusasConcentratedLoad, groupedAssignment, nodeDictionary
                            );
                        List<string> analysisName = new List<string> { lusasConcentratedLoad.getAttributeType() };
                        bhomPointLoad.Tags = new HashSet<string>(analysisName);
                        bhomPointLoads.Add(bhomPointLoad);
                    }
                }
            }

            return bhomPointLoads;
        }
    }
}