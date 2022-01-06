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

using BH.oM.Adapters.Lusas;
using BH.oM.Structure.MaterialFragments;
using BH.oM.Geometry;
using Lusas.LPI;
using BH.Engine.Adapter;

namespace BH.Adapter.Adapters.Lusas
{
    public static partial class Convert
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static IMaterialFragment ToMaterial(this IFAttribute lusasAttribute)
        {
            string attributeName = GetName(lusasAttribute);

            IMaterialFragment material = null;
            if (lusasAttribute is IFMaterialIsotropic)
            {
                material = new GenericIsotropicMaterial()
                {
                    Name = attributeName,
                    YoungsModulus = lusasAttribute.getValue("E"),
                    PoissonsRatio = lusasAttribute.getValue("nu"),
                    ThermalExpansionCoeff = lusasAttribute.getValue("alpha"),
                    Density = lusasAttribute.getValue("rho")
                };
            }
            else if (lusasAttribute is IFMaterialOrthotropic)
            {
                material = Engine.Structure.Create.Timber(
                attributeName,
                    new Vector() { X = lusasAttribute.getValue("Ex"), Y = lusasAttribute.getValue("Ey"), Z = lusasAttribute.getValue("Ez") },
                    new Vector() { X = lusasAttribute.getValue("nuxy"), Y = lusasAttribute.getValue("nuyz"), Z = lusasAttribute.getValue("nuzx") },
                    new Vector() { X = lusasAttribute.getValue("Gxy"), Y = 0.0, Z = 0.0 },
                    new Vector() { X = lusasAttribute.getValue("ax"), Y = lusasAttribute.getValue("ay"), Z = lusasAttribute.getValue("az") },
                    lusasAttribute.getValue("rho"), 0);
            }

            int adapterNameId = lusasAttribute.getID();
            material.SetAdapterId(typeof(LusasId), adapterNameId);


            return material;
        }

        /***************************************************/

    }
}



