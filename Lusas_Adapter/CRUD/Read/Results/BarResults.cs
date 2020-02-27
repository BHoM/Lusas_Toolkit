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
using System;
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

                Dictionary<string, IFResultsComponentSet> maxResultsSets = GetResultsSets(entity, components, "Feature maximum", resultsContext);
                Dictionary<string, IFResultsComponentSet> minResultsSets = GetResultsSets(entity, components, "Feature minimum", resultsContext);

                foreach (int barId in ids)
                {
                    string lineName = "L" + barId;

                    Dictionary<string, double> maxFeatureResults = GetFeatureResults(components, maxResultsSets, unitSet, barId, "L");

                    double maxfX = 0; double maxfY = 0; double maxfZ = 0; double maxmX = 0; double maxmY = 0; double maxmZ = 0;
                    maxFeatureResults.TryGetValue("Fx", out maxfX); maxFeatureResults.TryGetValue("Fy", out maxfY); maxFeatureResults.TryGetValue("Fz", out maxfZ);
                    maxFeatureResults.TryGetValue("Mx", out maxmX); maxFeatureResults.TryGetValue("My", out maxmY); maxFeatureResults.TryGetValue("Mz", out maxmZ);

                    Dictionary<string, double> minFeatureResults = GetFeatureResults(components, minResultsSets, unitSet, barId, "L");

                    double minfX = 0; double minfY = 0; double minfZ = 0; double minmX = 0; double minmY = 0; double minmZ = 0;
                    minFeatureResults.TryGetValue("Fx", out minfX); minFeatureResults.TryGetValue("Fy", out minfY); minFeatureResults.TryGetValue("Fz", out minfZ);
                    minFeatureResults.TryGetValue("Mx", out minmX); minFeatureResults.TryGetValue("My", out minmY); minFeatureResults.TryGetValue("Mz", out minmZ);

                    BarForce barForce = new BarForce
                    {
                        ResultCase = Engine.Lusas.Query.GetName(loadset.getName()),
                        ObjectId = barId,
                        FX = Math.Max(Math.Abs(maxfX), Math.Abs(minfX)) * forceSIConversion,
                        FY = Math.Max(Math.Abs(maxfY), Math.Abs(minfY)) * forceSIConversion,
                        FZ = Math.Max(Math.Abs(maxfZ), Math.Abs(minfZ)) * forceSIConversion,
                        MX = Math.Max(Math.Abs(maxmX), Math.Abs(minmX)) * forceSIConversion * lengthSIConversion,
                        MY = Math.Max(Math.Abs(maxmY), Math.Abs(minmY)) * forceSIConversion * lengthSIConversion,
                        MZ = Math.Max(Math.Abs(maxmZ), Math.Abs(minmZ)) * forceSIConversion * lengthSIConversion
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

                Dictionary<string, IFResultsComponentSet> maxResultsSets = GetResultsSets(entity, components, "Feature maximum", resultsContext);
                Dictionary<string, IFResultsComponentSet> minResultsSets = GetResultsSets(entity, components, "Feature minimum", resultsContext);

                foreach (int barId in ids)
                {
                    Dictionary<string, double> maxFeatureResults = GetFeatureResults(components, maxResultsSets, unitSet, barId, "L");
                    double maxaxial = 0;
                    maxFeatureResults.TryGetValue("Sx(Fx)", out maxaxial);

                    Dictionary<string, double> minFeatureResults = GetFeatureResults(components, minResultsSets, unitSet, barId, "L");
                    double minaxial = 0;
                    minFeatureResults.TryGetValue("Sx(Fx)", out minaxial);

                    BarStress barStress = new BarStress
                    {
                        ResultCase = Engine.Lusas.Query.GetName(loadset.getName()),
                        ObjectId = barId,
                        Axial = Math.Max(Math.Abs(maxaxial), Math.Abs(minaxial)) * forceSIConversion * lengthSIConversion * lengthSIConversion
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

                Dictionary<string, IFResultsComponentSet> maxResultsSets = GetResultsSets(entity, components, "Feature maximum", resultsContext);
                Dictionary<string, IFResultsComponentSet> minResultsSets = GetResultsSets(entity, components, "Feature minimum", resultsContext);

                foreach (int barId in ids)
                {
                    Dictionary<string, double> maxFeatureResults = GetFeatureResults(components, maxResultsSets, unitSet, barId, "L");
                    double maxeX = 0; double maxeY = 0; double maxeZ = 0; double maxbX = 0; double maxbY = 0; double maxbZ = 0;
                    maxFeatureResults.TryGetValue("Ex", out maxeX); maxFeatureResults.TryGetValue("Ey", out maxeY); maxFeatureResults.TryGetValue("Ez", out maxeZ);
                    maxFeatureResults.TryGetValue("Bx", out maxbX); maxFeatureResults.TryGetValue("By", out maxbY); maxFeatureResults.TryGetValue("Bz", out maxbZ);

                    Dictionary<string, double> minFeatureResults = GetFeatureResults(components, minResultsSets, unitSet, barId, "L");
                    double mineX = 0; double mineY = 0; double mineZ = 0; double minbX = 0; double minbY = 0; double minbZ = 0;
                    minFeatureResults.TryGetValue("Ex", out mineX); minFeatureResults.TryGetValue("Ey", out mineY); minFeatureResults.TryGetValue("Ez", out mineZ);
                    minFeatureResults.TryGetValue("Bx", out minbX); minFeatureResults.TryGetValue("By", out minbY); minFeatureResults.TryGetValue("Bz", out minbZ);

                    BarStrain barStrain = new BarStrain
                    {
                        ResultCase = Engine.Lusas.Query.GetName(loadset.getName()),
                        ObjectId = barId,
                        Axial = Math.Max(Math.Abs(maxeX), Math.Abs(mineX)),
                        ShearY = Math.Max(Math.Abs(maxeY), Math.Abs(mineY)),
                        ShearZ = Math.Max(Math.Abs(maxeZ), Math.Abs(mineZ))
                    };

                    barStrains.Add(barStrain);

                }

                d_LusasData.stopUsingScriptedResults();
                d_LusasData.flushScriptedResults();
            }

            BH.Engine.Reflection.Compute.RecordWarning("Please note only axial and shear strains will be returned when pulling BarStress results.");

            return barStrains;
        }

        /***************************************************/

        private IEnumerable<IResult> ExtractBarDisplacement(List<int> ids, List<int> loadcaseIds)
        {
            List<BarDisplacement> barDisplacements = new List<BarDisplacement>();

            IFView view = m_LusasApplication.getCurrentView();
            IFResultsContext resultsContext = m_LusasApplication.newResultsContext(view);

            string entity = "Displacement";

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

                Dictionary<string, IFResultsComponentSet> maxResultsSets = GetResultsSets(entity, components, "Feature maximum", resultsContext);
                Dictionary<string, IFResultsComponentSet> minResultsSets = GetResultsSets(entity, components, "Feature minimum", resultsContext);

                foreach (int barId in ids)
                {
                    Dictionary<string, double> maxFeatureResults = GetFeatureResults(components, maxResultsSets, unitSet, barId, "L");
                    double maxuX = 0; double maxuY = 0; double maxuZ = 0; double maxrX = 0; double maxrY = 0; double maxrZ = 0;
                    maxFeatureResults.TryGetValue("DX", out maxuX); maxFeatureResults.TryGetValue("DY", out maxuY); maxFeatureResults.TryGetValue("DZ", out maxuZ);
                    maxFeatureResults.TryGetValue("THX", out maxrX); maxFeatureResults.TryGetValue("THY", out maxrY); maxFeatureResults.TryGetValue("THZ", out maxrZ);

                    Dictionary<string, double> minFeatureResults = GetFeatureResults(components, minResultsSets, unitSet, barId, "L");
                    double minuX = 0; double minuY = 0; double minuZ = 0; double minrX = 0; double minrY = 0; double minrZ = 0;
                    minFeatureResults.TryGetValue("DX", out minuX); minFeatureResults.TryGetValue("DY", out minuY); minFeatureResults.TryGetValue("DZ", out minuZ);
                    minFeatureResults.TryGetValue("THX", out minrX); minFeatureResults.TryGetValue("THY", out minrY); minFeatureResults.TryGetValue("THZ", out minrZ);


                    BarDisplacement barDisplacement = new BarDisplacement
                    {
                        ResultCase = Engine.Lusas.Query.GetName(loadset.getName()),
                        ObjectId = barId,
                        UX = Math.Max(Math.Abs(maxuX), Math.Abs(minuX)) * lengthSIConversion,
                        UY = Math.Max(Math.Abs(maxuY), Math.Abs(minuY)) * lengthSIConversion,
                        UZ = Math.Max(Math.Abs(maxuZ), Math.Abs(minuZ)) * lengthSIConversion,
                        RX = Math.Max(Math.Abs(maxrX), Math.Abs(minrX)),
                        RY = Math.Max(Math.Abs(maxrY), Math.Abs(minrY)),
                        RZ = Math.Max(Math.Abs(maxrZ), Math.Abs(minrZ))
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
