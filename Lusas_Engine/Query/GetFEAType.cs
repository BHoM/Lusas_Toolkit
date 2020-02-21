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

using BH.oM.Structure.Elements;

namespace BH.Engine.Lusas
{
    public static partial class Query
    {
        public static BarFEAType GetFEAType(object type)
        {
            BarFEAType barFEAType = BarFEAType.Flexural;

            if (
                                type.ToString() == "BMI21" ||
                                type.ToString() == "BMI31" ||
                                type.ToString() == "BMX21" ||
                                type.ToString() == "BMX31" ||
                                type.ToString() == "BMI21W" ||
                                type.ToString() == "BMI31W" ||
                                type.ToString() == "BMX21W" ||
                                type.ToString() == "BMX31W")
            {
                barFEAType = BarFEAType.Flexural;
            }
            else if (
                type.ToString() == "BRS2" ||
                type.ToString() == "BRS3")
                barFEAType = BarFEAType.Axial;
            else if (
                type.ToString() == "BS4" ||
                type.ToString() == "BSL4" ||
                type.ToString() == "BXL4" ||
                type.ToString() == "JSH4" ||
                type.ToString() == "JL43" ||
                type.ToString() == "JNT4" ||
                type.ToString() == "IPN4" ||
                type.ToString() == "IPN6" ||
                type.ToString() == "LMS3" ||
                type.ToString() == "LMS4")
            {
                Engine.Reflection.Compute.RecordWarning(
                    type.ToString() + " not supported, FEAType defaulted to Flexural");
            }

            return barFEAType;
        }
    }
}
