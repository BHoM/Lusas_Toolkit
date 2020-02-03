﻿/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2019, the respective contributors. All rights reserved.
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

        public IEnumerable<IResult> ReadResults(MeshResultRequest request)
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
                    results = ExtractMeshStress(objectIds, loadCases).ToList();
                    break;
                case MeshResultType.VonMises:
                    results = ExtractMeshVonMises(objectIds, loadCases).ToList();
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

        private IEnumerable<IResult> ExtractMeshDisplacement(List<int> ids, List<int> loadcaseIds)
        {
            return null;
        }

        /***************************************************/

        private IEnumerable<IResult> ExtractMeshForce(List<int> ids, List<int> loadcaseIds)
        {
            return null;
        }

        /***************************************************/

        private IEnumerable<IResult> ExtractMeshStress(List<int> ids, List<int> loadcaseIds)
        {
            return null;
        }

        private IEnumerable<IResult> ExtractMeshVonMises(List<int> ids, List<int> loadcaseIds)
        {
            return null;
        }

        /***************************************************/

    }
}
