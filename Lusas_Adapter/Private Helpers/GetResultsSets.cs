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

using System;
using System.Collections.Generic;
using Lusas.LPI;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        internal Dictionary<string, IFResultsComponentSet> GetResultsSets(string entity, List<string> components, string location, IFResultsContext resultsContext)
        {
            Dictionary<string, IFResultsComponentSet> resultsSet = new Dictionary<string, IFResultsComponentSet>();

            foreach (string component in components)
            {
                try
                {
                    resultsSet.Add(component, d_LusasData.getResultsComponentSet(entity, component, location, resultsContext));
                }
                catch(System.Runtime.InteropServices.COMException)
                {
                    try
                    {
                        d_LusasData.openResults(@"%DBFolder%\%ModelName%~Analysis 1.mys", "Analysis 1", false, 0, false, false);
                        resultsSet.Add(component, d_LusasData.getResultsComponentSet(entity, component, location, resultsContext));
                    }
                    catch(System.Runtime.InteropServices.COMException)
                    {
                        Engine.Reflection.Compute.RecordError("No results file exists for this model, the model needs to be solved before pulling results.");
                    }

                }
            }

            return resultsSet;
        }
    }
}

