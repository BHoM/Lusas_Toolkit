/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2023, the respective contributors. All rights reserved.
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
using System;
using BH.oM.Adapters.Lusas;
using BH.oM.Structure.Loads;
using BH.Engine.Adapter;
using Lusas.LPI;
using BH.Engine.Base;

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

        private IFBasicCombination CreateLoadCombination(LoadCombination loadCombination)
        {
            IFBasicCombination lusasLoadcombination;

            List<double> loadFactors = new List<double>();
            List<int> loadcases = new List<int>();

            if (d_LusasData.existsLoadset(loadCombination.Name))
            {
                lusasLoadcombination = (IFBasicCombination)d_LusasData.getLoadset(loadCombination.Name);
            }
            else
            {
                if (loadCombination.Number == 0)
                {
                    lusasLoadcombination = d_LusasData.createCombinationBasic(loadCombination.Name);
                    Compute.RecordWarning($"LoadCombination {loadCombination.Name} ID will be autogenerated by Lusas.");
                }
                else
                {
                    lusasLoadcombination = d_LusasData.createCombinationBasic(loadCombination.Name, "", loadCombination.Number);
                }
                foreach (Tuple<double, ICase> factoredLoad in loadCombination.LoadCases)
                {
                    double factor = factoredLoad.Item1;
                    IFLoadset lusasLoadcase = d_LusasData.getLoadset(factoredLoad.Item2.AdapterId<int>(typeof(LusasId)));
                    lusasLoadcombination.addEntry(factor, lusasLoadcase);
                }
            }

            return lusasLoadcombination;
        }

        /***************************************************/

    }
}



