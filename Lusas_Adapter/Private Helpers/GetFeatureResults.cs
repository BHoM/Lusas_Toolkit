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
using Lusas.LPI;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        internal Dictionary<string, double> GetFeatureResults(List<string> components, Dictionary<string, IFResultsComponentSet> resultsSets, IFUnitSet unitSet, int id, string suffix)
        {
            Dictionary<string, double> featureResults = new Dictionary<string, double>();
            IFResultsComponentSet resultsSet = null;

            foreach (string component in components)
            {
                resultsSets.TryGetValue(component, out resultsSet);

                int componentNumber = resultsSet.getComponentNumber(component);
                int nodeID = 0;
                int nullID = 0; //For Nodal results the pLocnIndex2 will return 2 from getFeatureResults
                double featureResult = 0;

                if(suffix == "P")
                {
                    try
                    {
                        featureResult = resultsSet.getFeatureResults(resultsSet.getComponentNumber(component), d_LusasData.getPointByName(suffix + id), 3, unitSet, nodeID, nullID);
                    }
                    catch (System.ArgumentException)
                    {
                        featureResult = resultsSet.getFeatureResults(resultsSet.getComponentNumber(component), d_LusasData.getPointByName(id.ToString()), 3, unitSet, nodeID, nullID);
                    }
                }
                else if(suffix == "L")
                {
                    try
                    {
                        featureResult = resultsSet.getFeatureResults(resultsSet.getComponentNumber(component), d_LusasData.getLineByName(suffix + id), 3, unitSet, nodeID, nullID);
                    }
                    catch (System.ArgumentException)
                    {
                        featureResult = resultsSet.getFeatureResults(resultsSet.getComponentNumber(component), d_LusasData.getLineByName(id.ToString()), 3, unitSet, nodeID, nullID);
                    }
                }
                else if (suffix == "S")
                {
                    try
                    {
                        featureResult = resultsSet.getFeatureResults(resultsSet.getComponentNumber(component), d_LusasData.getSurfaceByName(suffix + id), 3, unitSet, nodeID, nullID);
                    }
                    catch (System.ArgumentException)
                    {
                        featureResult = resultsSet.getFeatureResults(resultsSet.getComponentNumber(component), d_LusasData.getSurfaceByName(id.ToString()), 3, unitSet, nodeID, nullID);
                    }
                }

                if (featureResult == double.MinValue || featureResult == double.MaxValue || featureResult == double.NaN)
                {
                    featureResult = 0;
                }
                
                if (!(resultsSet.isValidValue(featureResult)))
                {
                    featureResult = 0;
                    Engine.Reflection.Compute.RecordWarning($"{suffix}{id} {component} is an invalid result and will be set to zero" );
                }

                featureResults.Add(component, featureResult);
            }

            return featureResults;
        }

    }
}

