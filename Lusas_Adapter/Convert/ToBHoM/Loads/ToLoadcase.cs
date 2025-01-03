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

using System;
using BH.Engine.Adapter;
using BH.oM.Adapters.Lusas;
using BH.oM.Structure.Loads;
using Lusas.LPI;

namespace BH.Adapter.Adapters.Lusas
{
    public static partial class Convert
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static Loadcase ToLoadcase(this IFLoadcase lusasLoadcase)
        {
            long adapterNameId = lusasLoadcase.getID();

            Loadcase loadcase = new Loadcase
            {
                Name = GetName(lusasLoadcase),
                Number = System.Convert.ToInt32(Math.Max(Math.Min(adapterNameId, int.MaxValue), int.MinValue)),
            };

            if (adapterNameId > int.MaxValue)
                Engine.Base.Compute.RecordWarning($"The Number for {loadcase.Name} exceeds {int.MaxValue} and has been assigned {int.MaxValue}. Please verify the Load Combinations are still valid.");

            loadcase.SetAdapterId(typeof(LusasId), adapterNameId);

            return loadcase;
        }

        /***************************************************/

    }
}






