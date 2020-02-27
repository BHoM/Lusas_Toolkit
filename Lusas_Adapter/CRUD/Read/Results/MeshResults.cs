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

        public IEnumerable<IResult> ReadResults(MeshResultRequest request, ActionConfig actionConfig)
        {
            List<IResult> results;
            List<int> objectIds = GetObjectIDs(request);
            List<int> loadCases = GetLoadcaseIDs(request);

            switch (request.ResultType)
            {
                case MeshResultType.Displacements:
                    results = ExtractMeshDisplacement(objectIds, loadCases).ToList();
                    break;
                case MeshResultType.Forces:
                    results = ExtractMeshForce(objectIds, loadCases).ToList();
                    break;
                case MeshResultType.Stresses:
                    results = ExtractMeshStress(objectIds, loadCases, request.Layer).ToList();
                    break;
                case MeshResultType.VonMises:
                    results = ExtractMeshVonMises(objectIds, loadCases, request.Layer).ToList();
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

        private IEnumerable<IResult> ExtractMeshDisplacement(List<int> ids, List<int> loadcaseIds)
        {
            List<MeshDisplacement> bhomMeshDisplacements = new List<MeshDisplacement>();

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

                foreach (int meshId in ids)
                {
                    Dictionary<string, double> maxFeatureResults = GetFeatureResults(components, maxResultsSets, unitSet, meshId, "S");
                    double maxuX = 0; double maxuY = 0; double maxuZ = 0; double maxrX = 0; double maxrY = 0; double maxrZ = 0;
                    maxFeatureResults.TryGetValue("DX", out maxuX); maxFeatureResults.TryGetValue("DY", out maxuY); maxFeatureResults.TryGetValue("DZ", out maxuZ);
                    maxFeatureResults.TryGetValue("THX", out maxrX); maxFeatureResults.TryGetValue("THY", out maxrY); maxFeatureResults.TryGetValue("THZ", out maxrZ);

                    Dictionary<string, double> minFeatureResults = GetFeatureResults(components, minResultsSets, unitSet, meshId, "S");
                    double minuX = 0; double minuY = 0; double minuZ = 0; double minrX = 0; double minrY = 0; double minrZ = 0;
                    minFeatureResults.TryGetValue("DX", out minuX); minFeatureResults.TryGetValue("DY", out minuY); minFeatureResults.TryGetValue("DZ", out minuZ);
                    minFeatureResults.TryGetValue("THX", out minrX); minFeatureResults.TryGetValue("THY", out minrY); minFeatureResults.TryGetValue("THZ", out minrZ);

                    MeshDisplacement bhomMeshDisplacement = new MeshDisplacement
                        (
                        meshId, 0, 0, loadcaseId, 0, MeshResultLayer.Middle, 0.5, MeshResultSmoothingType.ByPanel, null,
                        Math.Max(Math.Abs(maxuX), Math.Abs(minuX)) * lengthSIConversion,
                        Math.Max(Math.Abs(maxuY), Math.Abs(minuY)) * lengthSIConversion,
                        Math.Max(Math.Abs(maxuZ), Math.Abs(minuZ)) * lengthSIConversion,
                        Math.Max(Math.Abs(maxrX), Math.Abs(minrX)),
                        Math.Max(Math.Abs(maxrY), Math.Abs(minrY)),
                        Math.Max(Math.Abs(maxrZ), Math.Abs(minrZ))
                        );

                    bhomMeshDisplacements.Add(bhomMeshDisplacement);

                }

                d_LusasData.stopUsingScriptedResults();
                d_LusasData.flushScriptedResults();
            }

            return bhomMeshDisplacements;
        }

        /***************************************************/

        private IEnumerable<IResult> ExtractMeshForce(List<int> ids, List<int> loadcaseIds)
        {
            List<MeshForce> bhomMeshForces = new List<MeshForce>();

            IFView view = m_LusasApplication.getCurrentView();
            IFResultsContext resultsContext = m_LusasApplication.newResultsContext(view);

            string entity = "Force/Moment - Thick Shell";

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

                List<string> components = new List<string>() { "NX", "NY", "NXY", "MX", "MY", "MXY", "SX", "SY" };
                d_LusasData.startUsingScriptedResults();

                Dictionary<string, IFResultsComponentSet> maxResultsSets = GetResultsSets(entity, components, "Feature maximum", resultsContext);
                Dictionary<string, IFResultsComponentSet> minResultsSets = GetResultsSets(entity, components, "Feature minimum", resultsContext);

                foreach (int meshId in ids)
                {
                    Dictionary<string, double> maxFeatureResults = GetFeatureResults(components, maxResultsSets, unitSet, meshId, "S");
                    double maxnX = 0; double maxnY = 0; double maxnXY = 0; double maxmX = 0; double maxmY = 0; double maxmXY = 0; double maxsX = 0; double maxsY = 0;
                    maxFeatureResults.TryGetValue("NX", out maxnX); maxFeatureResults.TryGetValue("NY", out maxnY); maxFeatureResults.TryGetValue("NXY", out maxnXY);
                    maxFeatureResults.TryGetValue("MX", out maxmX); maxFeatureResults.TryGetValue("MY", out maxmY); maxFeatureResults.TryGetValue("MXY", out maxmXY);
                    maxFeatureResults.TryGetValue("SX", out maxsX); maxFeatureResults.TryGetValue("SY", out maxsY);

                    Dictionary<string, double> minFeatureResults = GetFeatureResults(components, minResultsSets, unitSet, meshId, "S");
                    double minnX = 0; double minnY = 0; double minnXY = 0; double minmX = 0; double minmY = 0; double minmXY = 0; double minsX = 0; double minsY = 0;
                    minFeatureResults.TryGetValue("NX", out minnX); minFeatureResults.TryGetValue("NY", out minnY); minFeatureResults.TryGetValue("NXY", out minnXY);
                    minFeatureResults.TryGetValue("MX", out minmX); minFeatureResults.TryGetValue("MY", out minmY); minFeatureResults.TryGetValue("MXY", out minmXY);
                    minFeatureResults.TryGetValue("SX", out minsX); minFeatureResults.TryGetValue("SY", out minsY);

                    MeshForce meshForce = new MeshForce(
                        meshId, 0, 0, loadcaseId, 0, MeshResultLayer.Middle, 0.5, MeshResultSmoothingType.ByPanel, null,
                        Math.Max(Math.Abs(maxnX), Math.Abs(minnX)) * forceSIConversion,
                        Math.Max(Math.Abs(maxnY), Math.Abs(minnY)) * forceSIConversion,
                        Math.Max(Math.Abs(maxnXY), Math.Abs(minnXY)) * forceSIConversion,
                        Math.Max(Math.Abs(maxmX), Math.Abs(minmX)) * forceSIConversion,
                        Math.Max(Math.Abs(maxmY), Math.Abs(minmY)) * forceSIConversion,
                        Math.Max(Math.Abs(maxmXY), Math.Abs(minmXY)) * forceSIConversion,
                        Math.Max(Math.Abs(maxsX), Math.Abs(minsX)) * forceSIConversion,
                        Math.Max(Math.Abs(maxsY), Math.Abs(minsY)) * forceSIConversion);

                    bhomMeshForces.Add(meshForce);

                }

                d_LusasData.stopUsingScriptedResults();
                d_LusasData.flushScriptedResults();
            }

            return bhomMeshForces;
        }

        /***************************************************/

        private IEnumerable<IResult> ExtractMeshStress(List<int> ids, List<int> loadcaseIds, MeshResultLayer meshResultLayer)
        {
            List<MeshStress> bhomMeshStresses = new List<MeshStress>();

            IFView view = m_LusasApplication.getCurrentView();
            IFResultsContext resultsContext = m_LusasApplication.newResultsContext(view);

            string entity;
            switch (meshResultLayer)
            {
                case MeshResultLayer.Lower:
                    entity = "Stress (bottom) - Thick Shell";
                    break;
                case MeshResultLayer.Middle:
                    entity = "Stress (middle) - Thick Shell";
                    break;
                case MeshResultLayer.Upper:
                    entity = "Stress (top) - Thick Shell";
                    break;
                default:
                    entity = "Stress (middle) - Thick Shell";
                    Engine.Reflection.Compute.RecordWarning("No valid MeshLayerPosition provided, therefore it has defaulted to middle (i.e. 0.5).");
                    break;
            }

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

                List<string> components = new List<string>() { "SX", "SY", "SZ", "SYZ", "SZX","S1","S3","S2"};
                d_LusasData.startUsingScriptedResults();

                Dictionary<string, IFResultsComponentSet> maxResultsSets = GetResultsSets(entity, components, "Feature maximum", resultsContext);
                Dictionary<string, IFResultsComponentSet> minResultsSets = GetResultsSets(entity, components, "Feature minimum", resultsContext);

                foreach (int meshId in ids)
                {
                    Dictionary<string, double> maxFeatureResults = GetFeatureResults(components, maxResultsSets, unitSet, meshId, "S");
                    double maxsX = 0; double maxsY = 0; double maxsZ = 0; double maxsYZ = 0; double maxsZX = 0; double maxs1 = 0; double maxs3 = 0; double maxs2 = 0;
                    maxFeatureResults.TryGetValue("SX", out maxsX); maxFeatureResults.TryGetValue("SY", out maxsY); maxFeatureResults.TryGetValue("SZ", out maxsZ);
                    maxFeatureResults.TryGetValue("SYZ", out maxsYZ); maxFeatureResults.TryGetValue("SZX", out maxsZX);
                    maxFeatureResults.TryGetValue("S1", out maxs1); maxFeatureResults.TryGetValue("S3", out maxs3); maxFeatureResults.TryGetValue("S2", out maxs2);

                    Dictionary<string, double> minFeatureResults = GetFeatureResults(components, minResultsSets, unitSet, meshId, "S");
                    double minsX = 0; double minsY = 0; double minsZ = 0; double minsYZ = 0; double minsZX = 0; double mins1 = 0; double mins3 = 0; double mins2 = 0;
                    minFeatureResults.TryGetValue("SX", out minsX); minFeatureResults.TryGetValue("SY", out minsY); minFeatureResults.TryGetValue("SZ", out minsZ);
                    minFeatureResults.TryGetValue("SYZ", out minsYZ); minFeatureResults.TryGetValue("SZX", out minsZX);
                    minFeatureResults.TryGetValue("S1", out mins1); minFeatureResults.TryGetValue("S3", out mins3); minFeatureResults.TryGetValue("S2", out mins2);

                    MeshStress meshStress = new MeshStress(
                        meshId, 0, 0, loadcaseId, 0, MeshResultLayer.Middle, 0.5, MeshResultSmoothingType.ByPanel, null,
                        Math.Max(Math.Abs(maxsX), Math.Abs(minsX)) * forceSIConversion/(lengthSIConversion*lengthSIConversion),
                        Math.Max(Math.Abs(maxsY), Math.Abs(minsY)) * forceSIConversion / (lengthSIConversion * lengthSIConversion),
                        Math.Max(Math.Abs(maxsZ), Math.Abs(minsZ)) * forceSIConversion / (lengthSIConversion * lengthSIConversion),
                        Math.Max(Math.Abs(maxsYZ), Math.Abs(minsYZ)) * forceSIConversion / (lengthSIConversion * lengthSIConversion),
                        Math.Max(Math.Abs(maxsZX), Math.Abs(minsZX)) * forceSIConversion / (lengthSIConversion * lengthSIConversion),
                        Math.Max(Math.Abs(maxs1), Math.Abs(mins1)) * forceSIConversion / (lengthSIConversion * lengthSIConversion),
                        Math.Max(Math.Abs(maxs3), Math.Abs(mins3)) * forceSIConversion / (lengthSIConversion * lengthSIConversion),
                        Math.Max(Math.Abs(maxs2), Math.Abs(mins2)) * forceSIConversion / (lengthSIConversion * lengthSIConversion)
                        );

                    bhomMeshStresses.Add(meshStress);

                }

                d_LusasData.stopUsingScriptedResults();
                d_LusasData.flushScriptedResults();
            }

            return bhomMeshStresses;
        }

        private IEnumerable<IResult> ExtractMeshVonMises(List<int> ids, List<int> loadcaseIds, MeshResultLayer meshResultLayer)
        {
            List<MeshVonMises> bhomMeshStresses = new List<MeshVonMises>();

            IFView view = m_LusasApplication.getCurrentView();
            IFResultsContext resultsContext = m_LusasApplication.newResultsContext(view);

            string entity;

            switch (meshResultLayer)
            {
                case MeshResultLayer.Lower:
                    entity = "Stress (bottom) - Thick Shell";
                    break;
                case MeshResultLayer.Middle:
                    entity = "Stress (middle) - Thick Shell";
                    break;
                case MeshResultLayer.Upper:
                    entity = "Stress (top) - Thick Shell";
                    break;
                default:
                    entity = "Stress (middle) - Thick Shell";
                    Engine.Reflection.Compute.RecordWarning("No valid MeshLayerPosition provided, therefore it has defaulted to middle (i.e. 0.5).");
                    break;
            }

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

                List<string> components = new List<string>() { "SE" };
                d_LusasData.startUsingScriptedResults();

                Dictionary<string, IFResultsComponentSet> maxResultsSets = GetResultsSets(entity, components, "Feature maximum", resultsContext);
                Dictionary<string, IFResultsComponentSet> minResultsSets = GetResultsSets(entity, components, "Feature minimum", resultsContext);

                foreach (int meshId in ids)
                {
                    Dictionary<string, double> maxFeatureResults = GetFeatureResults(components, maxResultsSets, unitSet, meshId, "S");
                    double maxsE = 0;
                    maxFeatureResults.TryGetValue("SE", out maxsE);

                    Dictionary<string, double> minFeatureResults = GetFeatureResults(components, minResultsSets, unitSet, meshId, "S");
                    double minsE = 0;
                    minFeatureResults.TryGetValue("SE", out minsE);

                    MeshVonMises meshStress = new MeshVonMises(
                        meshId, 0, 0, loadcaseId, 0, MeshResultLayer.Middle, 0.5, MeshResultSmoothingType.ByPanel, null,
                        Math.Max(Math.Abs(maxsE), Math.Abs(minsE)) * forceSIConversion / (lengthSIConversion * lengthSIConversion), 0,0);

                    bhomMeshStresses.Add(meshStress);
                }

                d_LusasData.stopUsingScriptedResults();
                d_LusasData.flushScriptedResults();
            }

            return bhomMeshStresses;
        }

        /***************************************************/

    }
}
