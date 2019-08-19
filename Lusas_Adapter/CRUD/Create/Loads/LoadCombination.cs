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
using System;
using BH.oM.Structure.Loads;
using Lusas.LPI;
using BH.Engine.Reflection;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        private IFBasicCombination CreateLoadCombination(LoadCombination loadCombination)
        {
            if (!Engine.Lusas.Query.CheckIllegalCharacters(loadCombination.Name))
            {
                return null;
            }

            IFBasicCombination lusasLoadcombination = null;
            string lusasLoadCombinationName = "Lc" + loadCombination.CustomData[AdapterId] +
                "/" + loadCombination.Name;

            List<double> loadFactors = new List<double>();
            List<int> loadcases = new List<int>();

            if (d_LusasData.existsLoadset(lusasLoadCombinationName))
            {
                lusasLoadcombination = (IFBasicCombination)d_LusasData.getLoadset(lusasLoadCombinationName);
            }
            else
            {
                if(loadCombination.Number == 0)
                {
                    lusasLoadcombination = d_LusasData.createCombinationBasic(lusasLoadCombinationName);
                    Compute.RecordWarning("0 used for LoadCombination number,"
                        + "therefore LoadCombination number will not be forced");
                }
                else
                {
                    lusasLoadcombination = d_LusasData.createCombinationBasic(lusasLoadCombinationName, "",
                        loadCombination.Number);
                }
                foreach (Tuple<double, ICase> factoredLoad in loadCombination.LoadCases)
                {
                    string lusasName = "Lc" + factoredLoad.Item2.CustomData[AdapterId] + "/"
                        + factoredLoad.Item2.Name;
                    double factor = factoredLoad.Item1;
                    IFLoadset lusasLoadcase = d_LusasData.getLoadset(lusasName);
                    lusasLoadcombination.addEntry(factor, lusasLoadcase);
                }
            }

            return lusasLoadcombination;
        }
    }
}