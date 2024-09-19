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
#elif Debug200 || Release200
    public partial class LusasV200Adapter
#elif Debug210 || Release210
    public partial class LusasV210Adapter
#elif Debug211 || Release211
    public partial class LusasV211Adapter
#else
    public partial class LusasV17Adapter
#endif
    {
        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        private List<ILoad> ReadGravityLoads(List<string> ids = null)
        {
            List<ILoad> gravityLoads = new List<ILoad>();
            object[] lusasBodyForces = d_LusasData.getAttributes("Body Force Load");

            if (!(lusasBodyForces.Count() == 0))
            {
                List<Node> nodeList = GetCachedOrRead<Node>();
                Dictionary<string, Node> nodes = nodeList.ToDictionary(x => x.AdapterId<string>(typeof(LusasId)));
                List<Bar> barsList = GetCachedOrRead<Bar>();
                Dictionary<string, Bar> bars = barsList.ToDictionary(x => x.AdapterId<string>(typeof(LusasId)));
                List<Panel> panelsList = GetCachedOrRead<Panel>();
                Dictionary<string, Panel> panels = panelsList.ToDictionary(x => x.AdapterId<string>(typeof(LusasId)));

                List<IFLoadcase> allLoadcases = new List<IFLoadcase>();

                for (int i = 0; i < lusasBodyForces.Count(); i++)
                {
                    IFLoading lusasBodyForce = (IFLoading)lusasBodyForces[i];

                    IEnumerable<IGrouping<string, IFAssignment>> groupedByLoadcases =
                        GetLoadAssignments(lusasBodyForce);

                    foreach (IEnumerable<IFAssignment> groupedAssignment in groupedByLoadcases)
                    {
                        List<string> analysisName = new List<string> { lusasBodyForce.getAttributeType() };
                        
                        GravityLoad gravityLoad = Adapters.Lusas.Convert.ToGravityLoad(
                            lusasBodyForce, groupedAssignment, nodes, bars, panels, m_g);
                        gravityLoad.Tags = new HashSet<string>(analysisName);
                        gravityLoads.Add(gravityLoad);
                    }
                }
            }

            return gravityLoads;
        }

        /***************************************************/

    }
}




