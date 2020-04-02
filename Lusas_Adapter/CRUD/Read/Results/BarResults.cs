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

using BH.oM.Common;
using BH.oM.Adapter;
using BH.oM.Structure.Requests;
using BH.oM.Structure.Results;
using System.Collections.Generic;
using System.Linq;
using Lusas.LPI;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {

        /***************************************************/
        /**** Public method - Read override             ****/
        /***************************************************/

        public IEnumerable<IResult> ReadResults(BarResultRequest request, ActionConfig actionConfig)
        {
            List<IResult> results;
            List<int> objectIds = GetObjectIDs(request);
            List<int> loadCases = GetLoadcaseIDs(request);

            switch (request.ResultType)
            {
                case BarResultType.BarForce:
                    results = ExtractBarForce(objectIds, loadCases).ToList();
                    break;
                case BarResultType.BarStrain:
                    results = ExtractBarStrain(objectIds, loadCases).ToList();
                    break;
                case BarResultType.BarStress:
                    results = ExtractBarStress(objectIds, loadCases).ToList();
                    break;
                case BarResultType.BarDisplacement:
                    results = ExtractBarDisplacement(objectIds, loadCases).ToList();
                    break;
                default:
                    Engine.Reflection.Compute.RecordError($"Result of type {request.ResultType} is not yet supported in the Lusas_Toolkit.");
                    results = new List<IResult>();
                    break;
            }
            results.Sort();
            return results;
        }

        /***************************************************/
        /**** Private  Methods                          ****/
        /***************************************************/

        private IEnumerable<IResult> ExtractBarForce(List<int> ids, List<int> loadcaseIds)
        {
            List<BarForce> barForces = new List<BarForce>();

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

                foreach (int barId in ids)
                {
                    string lineName = "L" + barId;

                    Dictionary<string, double> featureResults = GetFeatureResults(components, resultsSets, unitSet, barId, "L", 6);

                    double fX = 0; double fY = 0; double fZ = 0; double mX = 0; double mY = 0; double mZ = 0;
                    featureResults.TryGetValue("Fx", out fX); featureResults.TryGetValue("Fy", out fY); featureResults.TryGetValue("Fz", out fZ);
                    featureResults.TryGetValue("Mx", out mX); featureResults.TryGetValue("My", out mY); featureResults.TryGetValue("Mz", out mZ);

                    BarForce barForce = new BarForce
                    {
                        ResultCase = Engine.External.Lusas.Query.GetName(loadset.getName()),
                        ObjectId = barId,
                        FX = fX * forceSIConversion,
                        FY = fY * forceSIConversion,
                        FZ = fZ * forceSIConversion,
                        MX = mX * forceSIConversion * lengthSIConversion,
                        MY = mY * forceSIConversion * lengthSIConversion,
                        MZ = mZ * forceSIConversion * lengthSIConversion,
                    };

                    barForces.Add(barForce);

                }

                d_LusasData.stopUsingScriptedResults();
                d_LusasData.flushScriptedResults();
            }

            return barForces;
        }

        /***************************************************/

        private IEnumerable<IResult> ExtractBarStress(List<int> ids, List<int> loadcaseIds)
        {
            List<BarStress> barStresses = new List<BarStress>();

            IFView view = m_LusasApplication.getCurrentView();
            IFResultsContext resultsContext = m_LusasApplication.newResultsContext(view);

            string entity = "Stress - Thick 3D Beam";
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

                List<string> components = new List<string>() { "Sx(Fx)" };
                d_LusasData.startUsingScriptedResults();

                Dictionary<string, IFResultsComponentSet> resultsSets = GetResultsSets(entity, components, location, resultsContext);

                foreach (int barId in ids)
                {
                    string lineName = "L" + barId;

                    Dictionary<string, double> featureResults = GetFeatureResults(components, resultsSets, unitSet, barId, "L", 6);

                    double axial = 0;
                    featureResults.TryGetValue("Sx(Fx)", out axial);

                    BarStress barStress = new BarStress
                    {
                        ResultCase = Engine.External.Lusas.Query.GetName(loadset.getName()),
                        ObjectId = barId,
                        Axial = axial * forceSIConversion * lengthSIConversion * lengthSIConversion
                    };

                    barStresses.Add(barStress);

                }

                d_LusasData.stopUsingScriptedResults();
                d_LusasData.flushScriptedResults();
            }

            BH.Engine.Reflection.Compute.RecordWarning("Please note only axial strains will be returned when pulling BarStress results.");

            return barStresses;
        }

        /***************************************************/

        private IEnumerable<IResult> ExtractBarStrain(List<int> ids, List<int> loadcaseIds)
        {
            List<BarStrain> barStrains = new List<BarStrain>();

            IFView view = m_LusasApplication.getCurrentView();
            IFResultsContext resultsContext = m_LusasApplication.newResultsContext(view);

            string entity = "Strain - Thick 3D Beam";
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

                List<string> components = new List<string>() { "Ex", "Ey", "Ez", "Bx", "By", "Bz" };
                d_LusasData.startUsingScriptedResults();

                Dictionary<string, IFResultsComponentSet> resultsSets = GetResultsSets(entity, components, location, resultsContext);

                foreach (int barId in ids)
                {
                    string lineName = "L" + barId;

                    Dictionary<string, double> featureResults = GetFeatureResults(components, resultsSets, unitSet, barId, "L", 6);
                    List<string> keys = featureResults.Keys.ToList();

                    double eX = 0; double eY = 0; double eZ = 0; double bX = 0; double bY = 0; double bZ = 0;
                    featureResults.TryGetValue("Ex", out eX); featureResults.TryGetValue("Ey", out eY); featureResults.TryGetValue("Ez", out eZ);
                    featureResults.TryGetValue("Bx", out bX); featureResults.TryGetValue("By", out bY); featureResults.TryGetValue("Bz", out bZ);

                    BarStrain barStrain = new BarStrain
                    {
                        ResultCase = Engine.External.Lusas.Query.GetName(loadset.getName()),
                        ObjectId = barId,
                        Axial = eX,
                        ShearY = eY,
                        ShearZ = eZ,
                    };

                    BH.Engine.Reflection.Compute.RecordWarning("Please note only axial and shear strains will be returned when pulling BarStrain results.");

                    barStrains.Add(barStrain);

                }

                d_LusasData.stopUsingScriptedResults();
                d_LusasData.flushScriptedResults();
            }

            return barStrains;
        }

        /***************************************************/

        private IEnumerable<IResult> ExtractBarDisplacement(List<int> ids, List<int> loadcaseIds)
        {
            List<BarDisplacement> barDisplacements = new List<BarDisplacement>();

            IFView view = m_LusasApplication.getCurrentView();
            IFResultsContext resultsContext = m_LusasApplication.newResultsContext(view);

            string entity = "Displacement";
            string location = "Feature extreme";

            foreach (int loadcaseId in loadcaseIds)
            {
                IFLoadset loadset = d_LusasData.getLoadset(loadcaseId);

                if (!loadset.needsAssociatedValues())
                {
                    resultsContext.setActiveLoadset(loadset);
                }

                IFUnitSet unitSet = d_LusasData.getModelUnits();
                double lengthSIConversion = 1 / unitSet.getLengthFactor();

                List<string> components = new List<string>() { "DX", "DY", "DZ", "THX", "THY", "THZ" };
                d_LusasData.startUsingScriptedResults();

                Dictionary<string, IFResultsComponentSet> resultsSets = GetResultsSets(entity, components, location, resultsContext);

                foreach (int barId in ids)
                {
                    Dictionary<string, double> featureResults = GetFeatureResults(components, resultsSets, unitSet, barId, "L", 6);

                    double uX = 0; double uY = 0; double uZ = 0; double rX = 0; double rY = 0; double rZ = 0;
                    featureResults.TryGetValue("DX", out uX); featureResults.TryGetValue("DY", out uY); featureResults.TryGetValue("DZ", out uZ);
                    featureResults.TryGetValue("THX", out rX); featureResults.TryGetValue("THY", out rY); featureResults.TryGetValue("THZ", out rZ);

                    BarDisplacement barDisplacement = new BarDisplacement
                    {
                        ResultCase = Engine.External.Lusas.Query.GetName(loadset.getName()),
                        ObjectId = barId,
                        UX = uX * lengthSIConversion,
                        UY = uY * lengthSIConversion,
                        UZ = uZ * lengthSIConversion,
                        RX = rX,
                        RY = rY,
                        RZ = rZ,
                    };

                    barDisplacements.Add(barDisplacement);

                }

                d_LusasData.stopUsingScriptedResults();
                d_LusasData.flushScriptedResults();
            }

            return barDisplacements;
        }

        /***************************************************/

    }
}
