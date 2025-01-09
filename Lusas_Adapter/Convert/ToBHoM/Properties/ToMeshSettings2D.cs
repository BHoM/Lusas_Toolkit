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

using Lusas.LPI;
using BH.oM.Adapters.Lusas;
using BH.oM.Adapters.Lusas.Fragments;
using BH.Engine.Adapter;

namespace BH.Adapter.Adapters.Lusas
{
    public static partial class Convert
    {
        public static MeshSettings2D ToMeshSettings2D(this IFAttribute lusasAttribute)
        {
            /***************************************************/
            /**** Public Methods                            ****/
            /***************************************************/

            string attributeName = GetName(lusasAttribute);

            int xDivisions = 0;
            int yDivisions = 0;
            double size = 0;

            Split2D splitMethod = Split2D.Automatic;


            if ((lusasAttribute.getValue("size") == 0) &&
                (lusasAttribute.getValue("xDivisions") == 0 &&
                lusasAttribute.getValue("yDivisions") == 0))
            {
            }
            else if (lusasAttribute.getValue("size") == 0)
            {
                splitMethod = Split2D.Divisions;
                xDivisions = lusasAttribute.getValue("xDivisions");
                yDivisions = lusasAttribute.getValue("yDivisions");

            }
            else
            {
                splitMethod = Split2D.Size;
                size = lusasAttribute.getValue("size");
            }

            MeshSettings2D meshSettings2D = new MeshSettings2D
            {
                Name = attributeName,
                SplitMethod = splitMethod,
                xDivisions = xDivisions,
                yDivisions = yDivisions,
                ElementSize = size
            };

            long adapterNameId = GetAdapterID(lusasAttribute, 'e');
            meshSettings2D.SetAdapterId(typeof(LusasId), adapterNameId);

            return meshSettings2D;
        }

        /***************************************************/

    }
}






