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
using System;
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

        private Dictionary<string, double> GetFeatureResults(List<string> components, Dictionary<string, IFResultsComponentSet> resultsSets, IFUnitSet unitSet, int id, string suffix, int resultType = 6)
        {
            Dictionary<string, double> featureResults = new Dictionary<string, double>();
            bool invalidResult = false;
            IFResultsComponentSet resultsSet = null;

            foreach (string component in components)
            {
                resultsSets.TryGetValue(component, out resultsSet);

                int componentNumber = resultsSet.getComponentNumber(component);
                int nodeID = 0; //Returned: The node id for nodal results or the element id (for Gauss, internal and elementNodal results) from which the returned value has been calculated
                int nullID = 0; //Returned: 0 for nodal results or the index within the element (for Gauss, internal and elementNodal results) at which the returned value has been calculated
                double featureResult = 0;

                //resultType 3 = maximum, 4 = minimum, 5 = absolute 6 = extreme
                if (suffix == "P")
                {
                    try
                    {
                        featureResult = resultsSet.getFeatureResults(resultsSet.getComponentNumber(component), d_LusasData.getPointByNumber(id), resultType, unitSet, nodeID, nullID);
                    }
                    catch (System.ArgumentException)
                    {
                        featureResult = resultsSet.getFeatureResults(resultsSet.getComponentNumber(component), d_LusasData.getPointByNumber(id), resultType, unitSet, nodeID, nullID);
                    }
                }
                else if (suffix == "L")
                {
                    try
                    {
                        featureResult = resultsSet.getFeatureResults(resultsSet.getComponentNumber(component), d_LusasData.getLineByNumber(id), resultType, unitSet, nodeID, nullID);
                    }
                    catch (System.ArgumentException)
                    {
                        featureResult = resultsSet.getFeatureResults(resultsSet.getComponentNumber(component), d_LusasData.getLineByNumber(id), resultType, unitSet, nodeID, nullID);
                    }
                }
                else if (suffix == "S")
                {
                    try
                    {
                        featureResult = resultsSet.getFeatureResults(resultsSet.getComponentNumber(component), d_LusasData.getSurfaceByNumber(id), resultType, unitSet, nodeID, nullID);
                    }
                    catch (System.ArgumentException)
                    {
                        featureResult = resultsSet.getFeatureResults(resultsSet.getComponentNumber(component), d_LusasData.getSurfaceByNumber(id), resultType, unitSet, nodeID, nullID);
                    }
                }

                if (double.IsInfinity(featureResult) || double.IsNaN(featureResult) || featureResult == double.MaxValue || featureResult == double.MinValue)
                {
                    featureResult = 0;
                }

                if (!(resultsSet.isValidValue(featureResult)))
                {
                    featureResult = 0;
                    invalidResult = true;
                }

                featureResults.Add(component, featureResult);
            }

            if (invalidResult)
            { 
                Engine.Base.Compute.RecordWarning($"Invalid results (i.e. where DOF is released) will be set to zero.");
            }

            return featureResults;
        }

        /***************************************************/

    }
}





