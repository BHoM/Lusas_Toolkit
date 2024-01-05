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
using System.Linq;
using BH.Engine.Adapter;
using BH.oM.Adapters.Lusas;
using BH.oM.Structure.Elements;
using BH.oM.Structure.Loads;
using Lusas.LPI;

namespace BH.Adapter.Lusas
{
#if Debug18 || Release18
    public partial class LusasV18Adapter
#elif Debug19 || Release19
    public partial class LusasV19Adapter
#elif Debug191 || Release191
    public partial class LusasV191Adapter
#elif Debug200 || Release200
    public partial class LusasV200Adapter
#elif Debug210 || Release210
    public partial class LusasV210Adapter
#else
    public partial class LusasV17Adapter
#endif
    {
        private List<ILoad> ReadAreaUniformlyDistributedLoads(List<string> ids = null)
        {
            /***************************************************/
            /**** Private Methods                           ****/
            /***************************************************/

            List<ILoad> areaUniformlyDistributedLoads = new List<ILoad>();

            object[] lusasGlobalDistributedLoads = d_LusasData.getAttributes("Global Distributed Load");
            object[] lusasLocalDistributedLoads = d_LusasData.getAttributes("Distributed Load");


            object[] lusasDistributedLoads = lusasGlobalDistributedLoads.Concat(
                lusasLocalDistributedLoads).ToArray();

            if (!(lusasDistributedLoads.Count() == 0))
            {
                List<Panel> panelsList = GetCachedOrRead<Panel>();
                Dictionary<string, Panel> panels = panelsList.ToDictionary(x => x.AdapterId<string>(typeof(LusasId)));

                List<IFLoadcase> allLoadcases = new List<IFLoadcase>();

                for (int i = 0; i < lusasDistributedLoads.Count(); i++)
                {
                    IFLoading lusasDistributedLoad = (IFLoading)lusasDistributedLoads[i];

                    if (lusasDistributedLoad.getValue("type") == "Area" || lusasDistributedLoad.getValue("type") == "all")
                    {
                        IEnumerable<IGrouping<string, IFAssignment>> groupedByLoadcases =
                            GetLoadAssignments(lusasDistributedLoad);

                        foreach (IEnumerable<IFAssignment> groupedAssignment in groupedByLoadcases)
                        {
                            AreaUniformlyDistributedLoad areaUniformlyDistributedLoad =
                                Adapters.Lusas.Convert.ToAreaUniformlyDistributed(
                                    lusasDistributedLoad, groupedAssignment, panels);

                            List<string> analysisName = new List<string> { lusasDistributedLoad.getAttributeType() };
                            areaUniformlyDistributedLoad.Tags = new HashSet<string>(analysisName);
                            areaUniformlyDistributedLoads.Add(areaUniformlyDistributedLoad);
                        }
                    }
                }
            }

            return areaUniformlyDistributedLoads;
        }

        /***************************************************/

    }
}




