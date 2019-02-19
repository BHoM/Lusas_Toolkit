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

using BH.oM.Common.Materials;
using Lusas.LPI;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        public IFAttribute CreateMaterial(Material material)
        {
            if (!Engine.Lusas.Query.CheckIllegalCharacters(material.Name))
            {
                return null;
            }

            IFAttribute lusasMaterial = null;
            string lusasName = "M" + material.CustomData[AdapterId] + "/" + material.Name;
            if (d_LusasData.existsAttribute("Material", lusasName))
            {
                lusasMaterial = d_LusasData.getAttribute("Material", lusasName);
            }
            else
            {
                lusasMaterial = d_LusasData.createIsotropicMaterial(material.Name,
                material.YoungsModulus, material.PoissonsRatio, material.Density, material.CoeffThermalExpansion);

                lusasMaterial.setName(lusasName);
            }
            return lusasMaterial;
        }
    }
}

