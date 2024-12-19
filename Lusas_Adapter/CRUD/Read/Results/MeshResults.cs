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

using BH.oM.Analytical.Results;
using BH.oM.Adapter;
using BH.oM.Structure.Requests;
using BH.oM.Structure.Results;
using System.Collections.Generic;
using System.Linq;
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
                    Engine.Base.Compute.RecordError($"Result of type {request.ResultType} is not yet supported in the Lusas_Toolkit.");
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
            List<MeshDisplacement> meshDisplacements = new List<MeshDisplacement>();

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

                foreach (int meshId in ids)
                {
                    Dictionary<string, double> featureResults = GetFeatureResults(components, resultsSets, unitSet, meshId, "S", 6);

                    double uX = 0; double uY = 0; double uZ = 0; double rX = 0; double rY = 0; double rZ = 0;
                    featureResults.TryGetValue("DX", out uX); featureResults.TryGetValue("DY", out uY); featureResults.TryGetValue("DZ", out uZ);
                    featureResults.TryGetValue("THX", out rX); featureResults.TryGetValue("THY", out rY); featureResults.TryGetValue("THZ", out rZ);
                    int mode = -1;
                    MeshDisplacement meshDisplacement = new MeshDisplacement
                        (
                        meshId, 0, 0, loadcaseId, mode, 0, MeshResultLayer.Middle, 0.5, MeshResultSmoothingType.ByPanel, null,
                        uX * lengthSIConversion,
                        uY * lengthSIConversion,
                        uZ * lengthSIConversion,
                        rX,
                        rY,
                        rZ
                        );

                    meshDisplacements.Add(meshDisplacement);

                }

                d_LusasData.stopUsingScriptedResults();
                d_LusasData.flushScriptedResults();
            }

            return meshDisplacements;
        }

        /***************************************************/

        private IEnumerable<IResult> ExtractMeshForce(List<int> ids, List<int> loadcaseIds)
        {
            List<MeshForce> meshForces = new List<MeshForce>();

            IFView view = m_LusasApplication.getCurrentView();
            IFResultsContext resultsContext = m_LusasApplication.newResultsContext(view);

            string entity = "Force/Moment - Thick Shell";
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

                List<string> components = new List<string>() { "NX", "NY", "NXY", "MX", "MY", "MXY", "SX", "SY" };
                d_LusasData.startUsingScriptedResults();

                Dictionary<string, IFResultsComponentSet> resultsSets = GetResultsSets(entity, components, location, resultsContext);

                foreach (int meshId in ids)
                {
                    Dictionary<string, double> featureResults = GetFeatureResults(components, resultsSets, unitSet, meshId, "S", 6);

                    double nX = 0; double nY = 0; double nXY = 0; double mX = 0; double mY = 0; double mXY = 0; double sX = 0; double sY = 0;
                    featureResults.TryGetValue("NX", out nX); featureResults.TryGetValue("NY", out nY); featureResults.TryGetValue("NXY", out nXY);
                    featureResults.TryGetValue("MX", out mX); featureResults.TryGetValue("MY", out mY); featureResults.TryGetValue("MXY", out mXY);
                    featureResults.TryGetValue("SX", out sX); featureResults.TryGetValue("SY", out sY);
                    int mode = -1;
                    MeshForce meshForce = new MeshForce(
                        meshId, 0, 0, loadcaseId, mode, 0, MeshResultLayer.Middle, 0.5, MeshResultSmoothingType.ByPanel, null,
                        nX * forceSIConversion,
                        nY * forceSIConversion,
                        nXY * forceSIConversion,
                        mX * forceSIConversion,
                        mY * forceSIConversion,
                        mXY * forceSIConversion,
                        sX * forceSIConversion,
                        sY * forceSIConversion);

                    meshForces.Add(meshForce);

                }

                d_LusasData.stopUsingScriptedResults();
                d_LusasData.flushScriptedResults();
            }

            return meshForces;
        }

        /***************************************************/

        private IEnumerable<IResult> ExtractMeshStress(List<int> ids, List<int> loadcaseIds, MeshResultLayer meshResultLayer)
        {
            List<MeshStress> meshStresses = new List<MeshStress>();

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
                    Engine.Base.Compute.RecordWarning("No valid MeshLayerPosition provided, therefore it has defaulted to middle (i.e. 0.5).");
                    break;
            }

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

                List<string> components = new List<string>() { "SX", "SY", "SZ", "SYZ", "SZX", "S1", "S3", "S2" };
                d_LusasData.startUsingScriptedResults();

                Dictionary<string, IFResultsComponentSet> resultsSets = GetResultsSets(entity, components, location, resultsContext);

                foreach (int meshId in ids)
                {
                    Dictionary<string, double> featureResults = GetFeatureResults(components, resultsSets, unitSet, meshId, "S", 6);

                    double sX = 0; double sY = 0; double sZ = 0; double sYZ = 0; double sXZ = 0; double s1 = 0; double s3 = 0; double s2 = 0;
                    featureResults.TryGetValue("SX", out sX); featureResults.TryGetValue("SY", out sY); featureResults.TryGetValue("SZ", out sZ);
                    featureResults.TryGetValue("SYZ", out sYZ); featureResults.TryGetValue("SZX", out sXZ);
                    featureResults.TryGetValue("S1", out s1); featureResults.TryGetValue("S3", out s3); featureResults.TryGetValue("S2", out s2);
                    int mode = -1;
                    MeshStress meshStress = new MeshStress(
                        meshId, 0, 0, loadcaseId, mode, 0, MeshResultLayer.Middle, 0.5, MeshResultSmoothingType.ByPanel, null,
                        sX * forceSIConversion / (lengthSIConversion * lengthSIConversion),
                        sY * forceSIConversion / (lengthSIConversion * lengthSIConversion),
                        sZ * forceSIConversion / (lengthSIConversion * lengthSIConversion),
                        sYZ * forceSIConversion / (lengthSIConversion * lengthSIConversion),
                        sXZ * forceSIConversion / (lengthSIConversion * lengthSIConversion),
                        s1 * forceSIConversion / (lengthSIConversion * lengthSIConversion),
                        s3 * forceSIConversion / (lengthSIConversion * lengthSIConversion),
                        s2 * forceSIConversion / (lengthSIConversion * lengthSIConversion)
                        );

                    meshStresses.Add(meshStress);

                }

                d_LusasData.stopUsingScriptedResults();
                d_LusasData.flushScriptedResults();
            }

            return meshStresses;
        }

        private IEnumerable<IResult> ExtractMeshVonMises(List<int> ids, List<int> loadcaseIds, MeshResultLayer meshResultLayer)
        {
            List<MeshVonMises> meshStresses = new List<MeshVonMises>();

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
                    Engine.Base.Compute.RecordWarning("No valid MeshLayerPosition provided, therefore it has defaulted to middle (i.e. 0.5).");
                    break;
            }
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

                List<string> components = new List<string>() { "SE" };
                d_LusasData.startUsingScriptedResults();

                Dictionary<string, IFResultsComponentSet> resultsSets = GetResultsSets(entity, components, location, resultsContext);

                foreach (int meshId in ids)
                {
                    Dictionary<string, double> featureResults = GetFeatureResults(components, resultsSets, unitSet, meshId, "S", 6);

                    double sE = 0;
                    featureResults.TryGetValue("SE", out sE);
                    int mode = -1;
                    MeshVonMises meshStress = new MeshVonMises(
                        meshId, 0, 0, loadcaseId, mode, 0, MeshResultLayer.Middle, 0.5, MeshResultSmoothingType.ByPanel, null,
                        sE * forceSIConversion / (lengthSIConversion * lengthSIConversion), 0, 0);

                    meshStresses.Add(meshStress);

                }

                d_LusasData.stopUsingScriptedResults();
                d_LusasData.flushScriptedResults();
            }

            return meshStresses;
        }

        /***************************************************/

    }
}





