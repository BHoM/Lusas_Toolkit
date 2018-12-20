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
using BH.oM.Structure.Elements;
using BH.oM.Adapters.Lusas;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        public void SetSplitMethod(IFMeshLine lusasLineMesh, MeshSettings1D meshSettings1D, BarFEAType barFEAType)
        {
            if (meshSettings1D.SplitMethod == Split1D.Length)
            {
                if (barFEAType == BarFEAType.Axial)
                    lusasLineMesh.setSize("BRS2", meshSettings1D.SplitParameter);
                else if (barFEAType == BarFEAType.Flexural)
                    lusasLineMesh.setSize("BMX21", meshSettings1D.SplitParameter);
            }
            else if (meshSettings1D.SplitMethod == Split1D.Automatic)
            {
                lusasLineMesh.setValue("uiSpacing", "uniform");
                SetElementType(lusasLineMesh, barFEAType);
            }
            else if (meshSettings1D.SplitMethod == Split1D.Divisions)
            {
                lusasLineMesh.addSpacing(System.Convert.ToInt32(meshSettings1D.SplitParameter), 1);
                SetElementType(lusasLineMesh, barFEAType);
            }
        }
    }
}
