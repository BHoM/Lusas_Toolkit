﻿/*
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
#else
    public partial class LusasV17Adapter
#endif
    {

        /***************************************************/
        /**** Public method - Read override             ****/
        /***************************************************/

        public IEnumerable<IResult> ReadResults(NodeResultRequest request, ActionConfig actionConfig)
        {
            List<IResult> results;
            List<int> objectIds = GetObjectIDs(request);
            List<int> loadCases = GetLoadcaseIDs(request);


            switch (request.ResultType)
            {
                case NodeResultType.NodeReaction:
                    results = ExtractNodeReaction(objectIds, loadCases).ToList();
                    break;
                case NodeResultType.NodeDisplacement:
                    results = ExtractNodeDisplacement(objectIds, loadCases).ToList();
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

        private IEnumerable<IResult> ExtractNodeReaction(List<int> ids, List<int> loadcaseIds)
        {
            List<NodeReaction> nodeReactions = new List<NodeReaction>();

            IFView view = m_LusasApplication.getCurrentView();
            IFResultsContext resultsContext = m_LusasApplication.newResultsContext(view);

            string entity = "Reaction";
            string location = "Nodal";

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

                foreach (int nodeId in ids)
                {
                    string pointName = "P" + nodeId;

                    Dictionary<string, double> featureResults = GetFeatureResults(components, resultsSets, unitSet, nodeId, "P", 6);

                    double fX = 0; double fY = 0; double fZ = 0; double mX = 0; double mY = 0; double mZ = 0;
                    featureResults.TryGetValue("Fx", out fX); featureResults.TryGetValue("Fy", out fY); featureResults.TryGetValue("Fz", out fZ);
                    featureResults.TryGetValue("Mx", out mX); featureResults.TryGetValue("My", out mY); featureResults.TryGetValue("Mz", out mZ);

                    List<double> results = new List<double>();

                    NodeReaction nodeReaction = new NodeReaction
                    {
                        ResultCase = Adapters.Lusas.Convert.GetName(loadset.getName()),
                        ObjectId = nodeId,
                        FX = fX * forceSIConversion,
                        FY = fY * forceSIConversion,
                        FZ = fZ * forceSIConversion,
                        MX = mX * forceSIConversion * lengthSIConversion,
                        MY = mY * forceSIConversion * lengthSIConversion,
                        MZ = mZ * forceSIConversion * lengthSIConversion,
                    };

                    nodeReactions.Add(nodeReaction);

                }

                d_LusasData.stopUsingScriptedResults();
                d_LusasData.flushScriptedResults();
            }

            return nodeReactions;
        }

        private IEnumerable<IResult> ExtractNodeDisplacement(List<int> ids, List<int> loadcaseIds)
        {
            List<NodeDisplacement> nodeDisplacements = new List<NodeDisplacement>();

            IFView view = m_LusasApplication.getCurrentView();
            IFResultsContext resultsContext = m_LusasApplication.newResultsContext(view);

            string entity = "Displacement";
            string location = "Nodal";

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

                List<string> components = new List<string>() { "DX", "DY", "DZ", "THX", "THY", "THZ" };
                d_LusasData.startUsingScriptedResults();

                Dictionary<string, IFResultsComponentSet> resultsSets = GetResultsSets(entity, components, location, resultsContext);

                foreach (int nodeId in ids)
                {
                    string pointName = "P" + nodeId;

                    Dictionary<string, double> featureResults = GetFeatureResults(components, resultsSets, unitSet, nodeId, "P", 6);

                    double uX = 0; double uY = 0; double uZ = 0; double rX = 0; double rY = 0; double rZ = 0;
                    featureResults.TryGetValue("DX", out uX); featureResults.TryGetValue("DY", out uY); featureResults.TryGetValue("DZ", out uZ);
                    featureResults.TryGetValue("THX", out rX); featureResults.TryGetValue("THY", out rY); featureResults.TryGetValue("THZ", out rZ);

                    NodeDisplacement bhomNodeDisplacement = new NodeDisplacement
                    {
                        ResultCase = Adapters.Lusas.Convert.GetName(loadset.getName()),
                        ObjectId = nodeId,
                        UX = uX * lengthSIConversion,
                        UY = uY * lengthSIConversion,
                        UZ = uZ * lengthSIConversion,
                        RX = rX,
                        RY = rY,
                        RZ = rZ,
                    };

                    nodeDisplacements.Add(bhomNodeDisplacement);

                }

                d_LusasData.stopUsingScriptedResults();
                d_LusasData.flushScriptedResults();
            }

            return nodeDisplacements;
        }

        /***************************************************/

    }
}