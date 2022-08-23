/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2022, the respective contributors. All rights reserved.
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
using BH.oM.Adapters.Lusas;
using BH.oM.Structure.Elements;
using BH.oM.Structure.Loads;
using BH.Engine.Adapter;
using Lusas.LPI;

namespace BH.Adapter.Lusas
{
#if Debug18 || Release18
    public partial class LusasV18Adapter
#elif Debug19 || Release19
    public partial class LusasV19Adapter
#elif Debug191 || Release191
    public partial class LusasV191Adapter
#else
    public partial class LusasV17Adapter
#endif
    {
        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        private List<ILoad> ReadBarVaryingDistributedLoads(List<string> ids = null)
        {
            List<ILoad> barDistributedLoads = new List<ILoad>();

            object[] lusasInternalBeamDistributedLoads = d_LusasData.getAttributes("Beam Distributed Load");

            if (lusasInternalBeamDistributedLoads.Count() != 0)
            {
                List<Bar> barsList = ReadBars();
                Dictionary<string, Bar> bars = barsList.ToDictionary(
                    x => x.AdapterId<string>(typeof(LusasId)));

                List<IFLoadcase> allLoadcases = new List<IFLoadcase>();

                for (int i = 0; i < lusasInternalBeamDistributedLoads.Count(); i++)
                {
                    IFLoading lusasInternalBeamDistributedLoad = (IFLoading)lusasInternalBeamDistributedLoads[i];
                    IEnumerable<IGrouping<string, IFAssignment>> groupedByLoadcases =
                        GetLoadAssignments(lusasInternalBeamDistributedLoad);

                    foreach (IEnumerable<IFAssignment> groupedAssignment in groupedByLoadcases)
                    {
                        BarVaryingDistributedLoad barDistributedLoad =
                            Adapters.Lusas.Convert.ToBarDistributedLoad(
                                lusasInternalBeamDistributedLoad, groupedAssignment, bars);

                        if (barDistributedLoad != null)
                        {
                            List<string> analysisName = new List<string> {
                            lusasInternalBeamDistributedLoad.getAttributeType() };

                            barDistributedLoad.Tags = new HashSet<string>(analysisName);
                            barDistributedLoads.Add(barDistributedLoad);
                        }
                    }
                }
            }

            return barDistributedLoads;
        }

        /***************************************************/

    }
}


