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

using BH.oM.Structure.Elements;
using Lusas.LPI;
using System;
using BH.oM.Structure.Constraints;

namespace BH.Engine.Lusas
{
    public partial class Query
    {
        public static Tuple<bool, double, BarRelease, BarFEAType> GetMeshProperties(IFLine lusasLine)
        {
            bool meshAssigned = true;
            double betaAngle = 0;
            BarRelease barRelease = null;
            BarFEAType barType = BarFEAType.Flexural;

            object[] meshAssignments = lusasLine.getAssignments("Mesh");

            if (meshAssignments.Length > 0)
            {
                foreach (object assignment in meshAssignments)
                {
                    IFAssignment lusasAssignment = (IFAssignment)assignment;
                    IFAttribute lusasMesh = lusasAssignment.getAttribute();
                    IFMeshLine lusasLineMesh = (IFMeshLine)lusasMesh;
                    betaAngle = lusasAssignment.getBetaAngle();

                    barRelease = Lusas.Query.GetBarRelease(lusasLineMesh);

                    object[] barMeshName = lusasLineMesh.getElementNames();

                    foreach (object type in barMeshName)
                    {
                        barType = Lusas.Query.GetFEAType(type);
                    }
                }
            }
            else
                meshAssigned = false;

            Tuple<bool, double, BarRelease, BarFEAType> lineMeshProperties =
                new Tuple<bool, double, BarRelease, BarFEAType>(meshAssigned, betaAngle, barRelease, barType);

            return lineMeshProperties;
        }
    }
}

