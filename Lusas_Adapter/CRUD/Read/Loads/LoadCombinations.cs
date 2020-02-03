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
using BH.oM.Structure.Loads;
using Lusas.LPI;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        private List<LoadCombination> ReadLoadCombinations(List<string> ids = null)
        {
            List<LoadCombination> bhomLoadCombintations = new List<LoadCombination>();
            object[] lusasCombinations = d_LusasData.getLoadsets("Combinations");

            if(!(lusasCombinations.Count()==0))
            {
                List<Loadcase> lusasLoadcases = ReadLoadcases();
                Dictionary<string, Loadcase> loadcaseDictionary = lusasLoadcases.ToDictionary(
                    x => x.Number.ToString());

                for (int i = 0; i < lusasCombinations.Count(); i++)
                {
                    IFBasicCombination lusasCombination = (IFBasicCombination)lusasCombinations[i];
                    LoadCombination bhomLoadCombination =
                        Engine.Lusas.Convert.ToBHoMLoadCombination(lusasCombination, loadcaseDictionary);
                    bhomLoadCombintations.Add(bhomLoadCombination);
                }
            }

            return bhomLoadCombintations;
        }
    }
}
