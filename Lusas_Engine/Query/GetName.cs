/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2018, the respective contributors. All rights reserved.
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

namespace BH.Engine.Lusas
{
    public partial class Query
    {
        public static string GetName(IFAttribute lusasAttribute)
        {
            string attributeName = "";

            if (lusasAttribute.getName().Contains("/"))
            {
                if(lusasAttribute.getName().Contains("\\"))
                {
                    attributeName = lusasAttribute.getName().Split('/', '\\')[0];
                }
                else
                {
                    attributeName = lusasAttribute.getName().Substring(
                        lusasAttribute.getName().LastIndexOf("/") + 1);
                }
            }
            else
            {
                attributeName = lusasAttribute.getName();
            }

            return attributeName;
        }

        public static string GetName(IFLoadcase lusasLoadcase)
        {
            string loadcaseName = "";

            if (lusasLoadcase.getName().Contains("/"))
            {
                loadcaseName = lusasLoadcase.getName().Substring(
                    lusasLoadcase.getName().LastIndexOf("/") + 1);
            }
            else
            {
                loadcaseName = lusasLoadcase.getName();
            }

            return loadcaseName;
        }

        public static string GetName(IFBasicCombination lusasLoadCombination)
        {
            string loadcaseName = "";

            if (lusasLoadCombination.getName().Contains("/"))
            {
                loadcaseName = lusasLoadCombination.getName().Substring(
                    lusasLoadCombination.getName().LastIndexOf("/") + 1);
            }
            else
            {
                loadcaseName = lusasLoadCombination.getName();
            }

            return loadcaseName;
        }

        public static string GetName(string loadName)
        {
            string bhomLoadName = "";

            if (loadName.Contains("/"))
            {
                bhomLoadName = loadName.Substring(
                    loadName.LastIndexOf("/") + 1);
            }
            else
            {
                bhomLoadName = loadName;
            }

            return bhomLoadName;
        }
    }
}