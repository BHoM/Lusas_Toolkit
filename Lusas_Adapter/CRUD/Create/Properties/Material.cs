/*
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

using BH.oM.Structure.MaterialFragments;
using BH.Engine.Structure;
using Lusas.LPI;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        public IFAttribute CreateMaterial(IMaterialFragment material)
        {
            if (!Engine.Lusas.Query.CheckIllegalCharacters(material.Name))
            {
                return null;
            }

            IFAttribute lusasMaterial = null;
            string lusasName = "M" + material.CustomData[AdapterId] + "/" + material.Name;

            if(material is IIsotropic)
            {
                IIsotropic isotropic = material as IIsotropic;

                if (d_LusasData.existsAttribute("Material", lusasName))
                {
                    lusasMaterial = d_LusasData.getAttribute("Material", lusasName);
                }
                else
                {
                    lusasMaterial = d_LusasData.createIsotropicMaterial(material.Name,
                    isotropic.YoungsModulus, isotropic.PoissonsRatio, isotropic.Density, isotropic.ThermalExpansionCoeff);

                    lusasMaterial.setName(lusasName);
                }
            }
            else
            {
                Engine.Reflection.Compute.RecordWarning("Lusas_Toolkit currently suports Isotropic materials only. No structural properties for material with name " + material.Name + " have been pushed");
                return null; ;
            }

            return lusasMaterial;
        }
    }
}

