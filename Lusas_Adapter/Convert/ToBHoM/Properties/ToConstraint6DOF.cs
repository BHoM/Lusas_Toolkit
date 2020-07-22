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

using System.Collections.Generic;
using BH.Adapter.Lusas;
using BH.oM.Structure.Constraints;
using Lusas.LPI;

namespace BH.Adapter.Adapters.Lusas
{
    public static partial class Convert
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static Constraint6DOF ToConstraint6DOF(this IFSupportStructural lusasAttribute)
        {
            List<string> releaseNames = new List<string> { "U", "V", "W", "THX", "THY", "THZ" };

            List<bool> fixity = new List<bool>();
            List<double> stiffness = new List<double>();

            foreach (string releaseName in releaseNames)
            {
                string fixityValue = lusasAttribute.getValue(releaseName);

                if (fixityValue == "F")
                {
                    fixity.Add(false);
                    stiffness.Add(0.0);
                }
                else if (fixityValue == "R")
                {
                    fixity.Add(true);
                    stiffness.Add(0.0);
                }
                else if (fixityValue == "S")
                {
                    fixity.Add(false);
                    double stiffnessValue = lusasAttribute.getValue(releaseName + "stiff");
                    stiffness.Add(stiffnessValue);
                }
            }

            string attributeName = GetName(lusasAttribute);

            Constraint6DOF constraint6DOF = BH.Engine.Structure.Create.Constraint6DOF(
               attributeName, fixity, stiffness);

            constraint6DOF.CustomData[AdapterIdName] = lusasAttribute.getID();

            return constraint6DOF;
        }

        /***************************************************/

    }
}

