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
using BH.oM.Structure.Loads;
using Lusas.LPI;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        private List<ILoad> ReadBarPointLoads(List<string> ids = null)
        {
            List<ILoad> bhomBarPointLoads = new List<ILoad>();

            object[] lusasInternalBeamPointLoads = d_LusasData.getAttributes("Beam Point Load");

            if (lusasInternalBeamPointLoads.Count() != 0)
            {
                List<Bar> bhomBars = ReadBars();
                Dictionary<string, Bar> barDictionary = bhomBars.ToDictionary(
                    x => x.CustomData[AdapterIdName].ToString());

                List<IFLoadcase> allLoadcases = new List<IFLoadcase>();

                for (int i = 0; i < lusasInternalBeamPointLoads.Count(); i++)
                {
                    IFLoading lusasInternalBeamPointLoad = (IFLoading)lusasInternalBeamPointLoads[i];
                    IEnumerable<IGrouping<string, IFAssignment>> groupedByLoadcases =
                        LusasAdapter.GetLoadAssignments(lusasInternalBeamPointLoad);

                    foreach (IEnumerable<IFAssignment> groupedAssignment in groupedByLoadcases)
                    {
                        BarPointLoad bhomBarPointLoad =
                            Adapter.External.Lusas.Convert.ToBarPointLoad(
                                lusasInternalBeamPointLoad, groupedAssignment, barDictionary);

                        List<string> analysisName = new List<string> { lusasInternalBeamPointLoad.getAttributeType() };
                        bhomBarPointLoad.Tags = new HashSet<string>(analysisName);
                        bhomBarPointLoads.Add(bhomBarPointLoad);
                    }
                }
            }

            return bhomBarPointLoads;
        }

        /***************************************************/

    }
}
