/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2025, the respective contributors. All rights reserved.
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using BH.Engine.Adapter;
using BH.oM.Adapters.Lusas;
using BH.oM.Structure.Loads;
using Lusas.LPI;

namespace BH.Adapter.Adapters.Lusas
{
    public static partial class Convert
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static LoadCombination ToLoadCombination(
            this IFBasicCombination lusasLoadCombination,
            Dictionary<string, Loadcase> loadcases)
        {
            object[] loadcaseIDs = lusasLoadCombination.getLoadcaseIDs();
            object[] loadcaseFactors = lusasLoadCombination.getFactors();

            List<Tuple<double, ICase>> factoredLoadcases = new List<Tuple<double, ICase>>();
            Loadcase loadcase = null;

            for (int i = 0; i < loadcaseIDs.Count(); i++)
            {
                int loadcaseID = (int)loadcaseIDs[i];
                double loadcaseFactor = (double)loadcaseFactors[i];
                loadcases.TryGetValue(loadcaseID.ToString(), out loadcase);
                Tuple<double, ICase> factoredLoadcase = new Tuple<double, ICase>(loadcaseFactor, loadcase);
                factoredLoadcases.Add(factoredLoadcase);
            }

            long adapterNameId = lusasLoadCombination.getID();

            LoadCombination loadCombination = new LoadCombination
            {
                Name = GetName(lusasLoadCombination),
                Number = System.Convert.ToInt32(Math.Max(Math.Min(adapterNameId, int.MaxValue), int.MinValue)),
                LoadCases = factoredLoadcases
            };

            if (adapterNameId > int.MaxValue)
                Engine.Base.Compute.RecordWarning($"The Number for {loadcase.Name} exceeds {int.MaxValue} and has been assigned {int.MaxValue}.");

            loadCombination.SetAdapterId(typeof(LusasId), adapterNameId);

            return loadCombination;
        }

        /***************************************************/

    }
}





