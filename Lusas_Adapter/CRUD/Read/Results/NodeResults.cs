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

        public IEnumerable<IResult> ReadResults(NodeResultRequest request)
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
                    Engine.Reflection.Compute.RecordError("Result of type " + request.ResultType + " is not yet supported");
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
            List<NodeReaction> bhomNodeReactions = new List<NodeReaction>();

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

                    Dictionary<string, double> featureResults = GetFeatureResults(components, resultsSets, unitSet, nodeId, "P");

                    double Fx = 0; double Fy = 0; double Fz = 0; double Mx = 0; double My = 0; double Mz = 0;
                    featureResults.TryGetValue("Fx", out Fx); featureResults.TryGetValue("Fy", out Fy); featureResults.TryGetValue("Fz", out Fz);
                    featureResults.TryGetValue("Mx", out Mx); featureResults.TryGetValue("My", out My); featureResults.TryGetValue("Mz", out Mz);

                    NodeReaction nodeReaction = new NodeReaction
                    {
                        ResultCase = Engine.Lusas.Query.GetName(loadset.getName()),
                        ObjectId = nodeId,
                        FX = Fx * forceSIConversion,
                        FY = Fy * forceSIConversion,
                        FZ = Fz * forceSIConversion,
                        MX = Mx * forceSIConversion * lengthSIConversion,
                        MY = My * forceSIConversion * lengthSIConversion,
                        MZ = Mz * forceSIConversion * lengthSIConversion,
                    };

                    bhomNodeReactions.Add(nodeReaction);

                }

                d_LusasData.stopUsingScriptedResults();
                d_LusasData.flushScriptedResults();
            }

            return bhomNodeReactions;
        }

        private IEnumerable<IResult> ExtractNodeDisplacement(List<int> ids, List<int> loadcaseIds)
        {
            List<NodeDisplacement> bhomNodeDisplacements = new List<NodeDisplacement>();

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

                    Dictionary<string, double> featureResults = GetFeatureResults(components, resultsSets, unitSet, nodeId, "P");

                    double Fx = 0; double Fy = 0; double Fz = 0; double Mx = 0; double My = 0; double Mz = 0;
                    featureResults.TryGetValue("DX", out Fx); featureResults.TryGetValue("DY", out Fy); featureResults.TryGetValue("DZ", out Fz);
                    featureResults.TryGetValue("THX", out Mx); featureResults.TryGetValue("THY", out My); featureResults.TryGetValue("THZ", out Mz);

                    NodeDisplacement bhomNodeDisplacement = new NodeDisplacement
                    {
                        ResultCase = Engine.Lusas.Query.GetName(loadset.getName()),
                        ObjectId = nodeId,
                        UX = Fx * forceSIConversion,
                        UY = Fy * forceSIConversion,
                        UZ = Fz * forceSIConversion,
                        RX = Mx * forceSIConversion * lengthSIConversion,
                        RY = My * forceSIConversion * lengthSIConversion,
                        RZ = Mz * forceSIConversion * lengthSIConversion,
                    };

                    bhomNodeDisplacements.Add(bhomNodeDisplacement);

                }

                d_LusasData.stopUsingScriptedResults();
                d_LusasData.flushScriptedResults();
            }

            return bhomNodeDisplacements;
        }

            /***************************************************/

        }
}