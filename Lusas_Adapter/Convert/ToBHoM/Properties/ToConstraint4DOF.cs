/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2022, the respective contributors. All rights reserved.
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
using BH.oM.Adapters.Lusas;
using BH.Engine.Adapter;
using BH.oM.Structure.Constraints;
using Lusas.LPI;

namespace BH.Adapter.Adapters.Lusas
{
    public static partial class Convert
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static Constraint4DOF ToConstraint4DOF(this IFSupportStructural lusasAttribute)
        {
            List<string> releaseNames = new List<string> { "U", "V", "W", "THX" };

            List<DOFType> fixity = new List<DOFType>();
            List<double> stiffness = new List<double>();

            foreach (string releaseName in releaseNames)
            {
                string fixityValue = lusasAttribute.getValue(releaseName);

                if (fixityValue == "F")
                {
                    fixity.Add(DOFType.Free);
                    stiffness.Add(0.0);
                }
                else if (fixityValue == "R")
                {
                    fixity.Add(DOFType.Fixed);
                    stiffness.Add(0.0);
                }
                else if (fixityValue == "S")
                {
                    fixity.Add(DOFType.Spring);
                    double stiffnessValue = lusasAttribute.getValue(releaseName + "stiff");
                    stiffness.Add(stiffnessValue);
                }
            }

            string attributeName = GetName(lusasAttribute);

            Constraint4DOF constraint4DOF = new Constraint4DOF { Name = attributeName };

            constraint4DOF.TranslationX = fixity[0];
            constraint4DOF.TranslationY = fixity[1];
            constraint4DOF.TranslationZ = fixity[2];
            constraint4DOF.RotationX = fixity[3];

            constraint4DOF.RotationalStiffnessX = stiffness[0];
            constraint4DOF.TranslationalStiffnessX = stiffness[1];
            constraint4DOF.TranslationalStiffnessX = stiffness[2];
            constraint4DOF.TranslationalStiffnessX = stiffness[3];

            int adapterNameId = (int)lusasAttribute.getID();
            constraint4DOF.SetAdapterId(typeof(LusasId), adapterNameId);

            return constraint4DOF;
        }

        /***************************************************/

    }
}



