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

using System.Linq;
using Lusas.LPI;
using BH.oM.Adapters.Lusas;
using BH.oM.Adapters.Lusas.Fragments;
using BH.Engine.Adapter;

namespace BH.Adapter.Adapters.Lusas
{
    public static partial class Convert
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static MeshSettings1D ToMeshSettings1D(this IFAttribute lusasAttrbute)
        {
            string attributeName = GetName(lusasAttrbute);

            IFMeshLine lusasMeshLine = (IFMeshLine)lusasAttrbute;

            double value = 0;
            Split1D splitMethod = Split1D.Automatic;

#if  Debug18 || Release18 || Debug19 || Release19
            int meshType = 0;
            lusasMeshLine.getMeshDivisions(ref meshType);
#else
            object meshTypeReference = 0;
            lusasMeshLine.getMeshDivisions(ref meshTypeReference);
            int meshType = (int)meshTypeReference;
#endif

            if (meshType == 0)
            {
                value = 0;
            }
            else if (meshType == 1)
            {
                splitMethod = Split1D.Divisions;
                object[] ratios = lusasMeshLine.getMeshDivisions(0);
                value = ratios.Count();
                if (value == 0)
                    value = 4;
            }
            else if (meshType == 2)
            {
                splitMethod = Split1D.Length;
                value = lusasMeshLine.getValue("size");
            }

            MeshSettings1D meshSettings1D = new MeshSettings1D
            {
                Name = attributeName,
                SplitMethod = splitMethod,
                SplitParameter = value
            };

            long adapterNameId = GetAdapterID(lusasMeshLine, 'e');
            meshSettings1D.SetAdapterId(typeof(LusasId), adapterNameId);

            return meshSettings1D;
        }

        /***************************************************/

    }
}






