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

using BH.oM.Physical.Materials;
using BH.Engine.Physical;
using Lusas.LPI;
using System.Collections.Generic;

namespace BH.Engine.Lusas
{
    public static partial class Convert
    {
        public static Material ToBHoMMaterial(this IFAttribute lusasAttribute)
        {
            string attributeName = Lusas.Query.GetName(lusasAttribute);

            Material bhomMaterial = Engine.Physical.Create.Material(attributeName,
                lusasAttribute.getValue("rho"),
                new List<IMaterialProperties>()
                {
                     lusasAttribute.getValue("E"),lusasAttribute.getValue("nu"),lusasAttribute.getValue("alpha")
                });

            //How to combine the mass Rayleigh and stiffness Rayleigh in to a single damping constant
            //https://www.orcina.com/SoftwareProducts/OrcaFlex/Documentation/Help/Content/html/RayleighDamping.htm

            int adapterID = Lusas.Query.GetAdapterID(lusasAttribute, 'M');
            bhomMaterial.CustomData["Lusas_id"] = adapterID;

            return bhomMaterial;
        }


    }
}
