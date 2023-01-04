/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2023, the respective contributors. All rights reserved.
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
using System.Linq;
using BH.oM.Adapters.Lusas;
using BH.oM.Structure.Constraints;
using BH.Engine.Structure;
using BH.Engine.Adapter;
using Lusas.LPI;

namespace BH.Adapter.Lusas
{
#if Debug18 || Release18
    public partial class LusasV18Adapter
#elif Debug19 || Release19
    public partial class LusasV19Adapter
#else
    public partial class LusasV17Adapter
#endif
    {
        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        private IFAttribute CreateSupport(Constraint6DOF constraint)
        {
            if (!Engine.Adapters.Lusas.Query.CheckIllegalCharacters(constraint.DescriptionOrName()))
            {
                return null;
            }

            IFAttribute lusasSupport = null;

            if (d_LusasData.existsAttribute("Support", constraint.DescriptionOrName()))
            {
                lusasSupport = d_LusasData.getAttribute("Support", constraint.DescriptionOrName());
            }
            else
            {
                lusasSupport = d_LusasData.createSupportStructural(constraint.DescriptionOrName());

                List<string> releaseNames = new List<string> { "U", "V", "W", "THX", "THY", "THZ" };
                List<double> stiffness = new List<double> {
                    constraint.TranslationalStiffnessX, constraint.TranslationalStiffnessY,
                    constraint.TranslationalStiffnessZ,
                    constraint.RotationalStiffnessX,constraint.RotationalStiffnessY,
                    constraint.RotationalStiffnessZ};

                bool[] fixities = constraint.Fixities();

                for (int i = 0; i < releaseNames.Count(); i++)
                {
                    if (fixities[i])
                    {
                        lusasSupport.setValue(releaseNames[i], "R");
                    }
                    else if (stiffness[i] == 0)
                    {
                        lusasSupport.setValue(releaseNames[i], "F");
                    }
                    else
                    {
                        lusasSupport.setValue(releaseNames[i], "S");
                        lusasSupport.setValue(releaseNames[i] + "stiff", stiffness[i]);
                    }
                }
            }

            if (lusasSupport != null)
            {
                int adapterIdName = lusasSupport.getID();
                constraint.SetAdapterId(typeof(LusasId), adapterIdName);

                return lusasSupport;
            }

            return null;
        }

        /***************************************************/

        private IFAttribute CreateSupport(Constraint4DOF constraint)
        {
            if (!Engine.Adapters.Lusas.Query.CheckIllegalCharacters(constraint.DescriptionOrName()))
            {
                return null;
            }

            IFAttribute lusasSupport = null;

            if (d_LusasData.existsAttribute("Support", constraint.DescriptionOrName()))
            {
                lusasSupport = d_LusasData.getAttribute("Support", constraint.DescriptionOrName());
            }
            else
            {
                lusasSupport = d_LusasData.createSupportStructural(constraint.DescriptionOrName());

                List<string> releaseNames = new List<string> { "U", "V", "W", "THX" };
                List<double> stiffness = new List<double> {
                    constraint.TranslationalStiffnessX,
                    constraint.TranslationalStiffnessY,
                    constraint.TranslationalStiffnessZ,
                    constraint.RotationalStiffnessX
                };
                List<DOFType> fixities = new List<DOFType> {
                    constraint.TranslationX,
                    constraint.TranslationY,
                    constraint.TranslationZ,
                    constraint.RotationX
                };

                for (int i = 0; i < releaseNames.Count(); i++)
                {
                    if (fixities[i] == DOFType.Fixed)
                    {
                        lusasSupport.setValue(releaseNames[i], "R");
                    }
                    else if (stiffness[i] == 0)
                    {
                        lusasSupport.setValue(releaseNames[i], "F");
                    }
                    else
                    {
                        lusasSupport.setValue(releaseNames[i], "S");
                        lusasSupport.setValue(releaseNames[i] + "stiff", stiffness[i]);
                    }
                }
            }

            if (lusasSupport != null)
            {
                int adapterIdName = lusasSupport.getID();
                constraint.SetAdapterId(typeof(LusasId), adapterIdName);

                return lusasSupport;
            }

            return null;

        }

        /***************************************************/

    }
}





