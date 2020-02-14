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

using System.Collections;
using System.Collections.Generic;
using BH.oM.Structure.Results;
using Lusas.LPI;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        private List<BarForce> ReadBarForce(IList ids = null, IList cases = null)
        {
            List<BarForce> bhombarForces = new List<BarForce>();

            List<int> barIds = new List<int>();

            if (ids == null || ids.Count == 0)
                Engine.Reflection.Compute.RecordWarning("Please provide ids");
            else
                barIds = Engine.Lusas.Query.GetObjectIDs(ids);

            List<int> loadcaseIds = new List<int>();

            if (cases == null || cases.Count == 0)
                Engine.Reflection.Compute.RecordWarning("Please provide loadcase ids");
            else
                loadcaseIds = Engine.Lusas.Query.GetLoadcaseIDs(cases);

            IFView view = m_LusasApplication.getCurrentView();
            IFResultsContext resultsContext = m_LusasApplication.newResultsContext(view);

            string entity = "Force/Moment - Thick 3D Beam";
            string location = "Feature extreme";

            foreach (int loadcaseId in loadcaseIds)
            {
                IFLoadset loadset = d_LusasData.getLoadset(loadcaseId);

                if (!loadset.needsAssociatedValues())
                {
                    resultsContext.setActiveLoadset(loadset);
                }

                IFUnitSet unitSet = d_LusasData.getModelUnits();
                double forceSIConversion = 1 / unitSet.getForceFactor();
                double lengthSIConversion = 1 / unitSet.getLengthFactor();

                List<string> components = new List<string>() { "Fx", "Fy", "Fz", "Mx", "My", "Mz" };
                d_LusasData.startUsingScriptedResults();

                Dictionary<string, IFResultsComponentSet> resultsSets = GetResultsSets(entity, components, location, resultsContext);

                foreach (int barId in barIds)
                {
                    string lineName = "L" + barId;

                    Dictionary<string, double> featureResults = GetFeatureResults(components, resultsSets, unitSet, barId, "L");

                    double Fx = 0; double Fy = 0; double Fz = 0; double Mx = 0; double My = 0; double Mz = 0;
                    featureResults.TryGetValue("Fx", out Fx); featureResults.TryGetValue("Fy", out Fy); featureResults.TryGetValue("Fz", out Fz);
                    featureResults.TryGetValue("Mx", out Mx); featureResults.TryGetValue("My", out My); featureResults.TryGetValue("Mz", out Mz);

                    BarForce barForce = new BarForce
                    {
                        ResultCase = Engine.Lusas.Query.GetName(loadset.getName()),
                        ObjectId = barId,
                        FX = Fx * forceSIConversion,
                        FY = Fy * forceSIConversion,
                        FZ = Fz * forceSIConversion,
                        MX = Mx * forceSIConversion * lengthSIConversion,
                        MY = My * forceSIConversion * lengthSIConversion,
                        MZ = Mz * forceSIConversion * lengthSIConversion,
                    };

                    bhombarForces.Add(barForce);

                }

                d_LusasData.stopUsingScriptedResults();
                d_LusasData.flushScriptedResults();
            }

            return bhombarForces;
        }

    }
}