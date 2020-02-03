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

using BH.oM.Adapters.Lusas;


namespace BH.Engine.Adapters.Lusas
{
    public static partial class Create
    {
        /***************************************************/
        /****           Public Constructors             ****/
        /***************************************************/

        public static MeshSettings1D MeshSettings1D(Split1D SplitMethod,
            double SplitParameter = 4, string name = null)
        {
            return new MeshSettings1D
            {
                SplitMethod = SplitMethod,
                SplitParameter = SplitParameter,
                Name = name
            };
        }


        public static MeshSettings2D MeshSettings2D(Split2D splitMethod,
            int xDivisions = 0, int yDivisions = 0, double size = 1, string name = null)
        {
            return new MeshSettings2D
            {
                SplitMethod = splitMethod,
                xDivisions = xDivisions,
                yDivisions = yDivisions,
                ElementSize = size,
                Name = name
            };
        }
    }
}

