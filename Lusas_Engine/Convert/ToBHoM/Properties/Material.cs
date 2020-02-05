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

using BH.oM.Structure.MaterialFragments;
using BH.Engine.Structure;
using BH.oM.Geometry;
using BH.Engine.Geometry;
using Lusas.LPI;

namespace BH.Engine.Lusas
{
    public static partial class Convert
    {
        public static IMaterialFragment ToBHoMMaterial(this IFAttribute lusasAttribute)
        {
            string attributeName = Lusas.Query.GetName(lusasAttribute);

            IMaterialFragment bhomMaterial = null;
            if (lusasAttribute is IFMaterialIsotropic)
            {

                IMaterialFragment bhomMaterials = Engine.Structure.Create.Steel(
                attributeName,
                lusasAttribute.getValue("E"),
                lusasAttribute.getValue("nu"),
                lusasAttribute.getValue("alpha"),
                lusasAttribute.getValue("rho"),
                0, 0, 0);

                Engine.Reflection.Compute.RecordWarning("Isotropic materials in Lusas will default to a SteelMaterial. Properties for " + attributeName + " are stored in a SteelMaterial");

                int adapterID = Lusas.Query.GetAdapterID(lusasAttribute, 'M');
                bhomMaterials.CustomData["Lusas_id"] = adapterID;
                bhomMaterial = bhomMaterials;
            }
            else if (lusasAttribute is IFMaterialOrthotropic)
               
                    {

                Vector YE = new Vector () { X= lusasAttribute.getValue("Ex"), Y= lusasAttribute.getValue("Ey"), Z= lusasAttribute.getValue("Ez") };
                Vector V = new Vector() { X = lusasAttribute.getValue("nuxy"), Y = lusasAttribute.getValue("nuyz"), Z = lusasAttribute.getValue("nuzx") };
                Vector Z = new Vector() { X = lusasAttribute.getValue("Gxy"),Y= 0.0, Z= 0.0 };
                Vector tc = new Vector() { X = lusasAttribute.getValue("ax"), Y = lusasAttribute.getValue("ay"), Z = lusasAttribute.getValue("az") };
                IMaterialFragment bhomMaterials = Engine.Structure.Create.Timber(
                attributeName, YE, V, Z, tc, lusasAttribute.getValue("rho"), 0, 0);


                Engine.Reflection.Compute.RecordWarning("orthotropic materials in Lusas will default to a timberMaterial. Properties for " + attributeName + " are stored in a TimberMaterial");

                int adapterID = Lusas.Query.GetAdapterID(lusasAttribute, 'M');
                bhomMaterials.CustomData["Lusas_id"] = adapterID;
                bhomMaterial = bhomMaterials;

            }
                return bhomMaterial;
            }
                       }
        
    }

