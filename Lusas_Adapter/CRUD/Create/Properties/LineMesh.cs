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
using BH.oM.Adapters.Lusas;
using BH.oM.Structure.Elements;
using BH.oM.Structure.Properties.Constraint;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        public IFMeshLine CreateMeshSettings1D(MeshSettings1D meshSettings1D, BarFEAType barFEAType = BarFEAType.Flexural, BarRelease barRelease = null)
        {
            if (!CheckIllegalCharacters(meshSettings1D.Name))
            {
                return null;
            }

            if (barRelease != null && barFEAType == BarFEAType.Axial)
            {
                Engine.Reflection.Compute.RecordWarning(
                    barFEAType + " used with barReleases, this information will be lost when pushed to Lusas");
            }
            else if(barRelease == null)
            {
                barRelease = Engine.Structure.Create.BarReleaseFixFix();
            }


            int adapterID;
            if (meshSettings1D.CustomData.ContainsKey(AdapterId))
                adapterID = System.Convert.ToInt32(meshSettings1D.CustomData[AdapterId]);
            else
                adapterID = System.Convert.ToInt32(NextId(meshSettings1D.GetType()));

            string releaseString = CreateReleaseString(barRelease);

            IFMeshLine lusasLineMesh = null;
            string lusasName = 
                "Me" + adapterID + "/" + meshSettings1D.Name + "\\" + barFEAType.ToString() + "|" + releaseString;

            if (d_LusasData.existsAttribute("Mesh", lusasName))
            {
                lusasLineMesh = (IFMeshLine)d_LusasData.getAttribute("Mesh", lusasName);
            }
            else
            {
                lusasLineMesh = d_LusasData.createMeshLine(lusasName);
                SetSplitMethod(lusasLineMesh, meshSettings1D, barFEAType);
                SetEndConditions(lusasLineMesh, barRelease);
            }
            return lusasLineMesh;
        }
    }
}