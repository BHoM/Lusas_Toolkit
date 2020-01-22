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

using System.Collections.Generic;
using System.Linq;
using BH.oM.Structure.Loads;
using Lusas.LPI;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        private IFPrescribedDisplacementLoad CreatePrescribedDisplacement(PointDisplacement pointDisplacement, object[] lusasPoints)
        {
            if (!Engine.Lusas.Query.CheckIllegalCharacters(pointDisplacement.Name))
            {
                return null;
            }

            IFPrescribedDisplacementLoad lusasPrescribedDisplacement = null;
            IFLoadcase assignedLoadcase = (IFLoadcase)d_LusasData.getLoadset(
                "Lc" + pointDisplacement.Loadcase.CustomData[AdapterIdName] + "/" + pointDisplacement.Loadcase.Name);

            string lusasName = "Pd" + 
                pointDisplacement.CustomData[AdapterIdName] + "/" + pointDisplacement.Name;

            NameSearch("Pd", pointDisplacement.CustomData[AdapterIdName].ToString(), 
                pointDisplacement.Name, ref lusasName);

            if (d_LusasData.existsAttribute("Loading", lusasName))
            {
                lusasPrescribedDisplacement = (IFPrescribedDisplacementLoad)d_LusasData.getAttribute(
                    "Loading", lusasName);
            }
            else
            {
                List<string> valueNames = new List<string> { "u", "v", "w", "thx", "thy", "thz" };
                List<string> boolCheck = new List<string> { "haveDispX", "haveDispY", "haveDispZ",
                    "haveRotX", "haveRotY", "haveRotZ" };
                List<double> displacements = new List<double>
                {
                    pointDisplacement.Translation.X, pointDisplacement.Translation.Y,
                    pointDisplacement.Translation.Z,
                    pointDisplacement.Rotation.X,pointDisplacement.Rotation.Y,
                    pointDisplacement.Rotation.Z
                };

                lusasPrescribedDisplacement = d_LusasData.createPrescribedDisplacementLoad(
                    lusasName, "Total");

                for(int i=0; i < valueNames.Count(); i++)
                {
                    if(!(displacements[i] == 0))
                    {
                        lusasPrescribedDisplacement.setValue(boolCheck[i], true);
                        lusasPrescribedDisplacement.setValue(valueNames[i], displacements[i]);
                    }
                    else
                    {
                        lusasPrescribedDisplacement.setValue(boolCheck[i], false);
                    }
                }

            }

            IFAssignment lusasAssignment = m_LusasApplication.assignment();
            lusasAssignment.setLoadset(assignedLoadcase);
            lusasPrescribedDisplacement.assignTo(lusasPoints, lusasAssignment);

            return lusasPrescribedDisplacement;
        }
    }
}
