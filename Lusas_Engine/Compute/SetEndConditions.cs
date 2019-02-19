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

using BH.oM.Structure.Properties.Constraint;
using Lusas.LPI;

namespace BH.Engine.Lusas
{
    public partial class Compute
    {
        public static void SetEndConditions(IFMeshLine lusasLineMesh, BarRelease barReleases)
        {
            if (barReleases.StartRelease.TranslationX == DOFType.Free)
                lusasLineMesh.setEndRelease("Start", "u", "free");
            if (barReleases.StartRelease.TranslationY == DOFType.Free)
                lusasLineMesh.setEndRelease("Start", "v", "free");
            if (barReleases.StartRelease.TranslationY == DOFType.Free)
                lusasLineMesh.setEndRelease("Start", "w", "free");
            if (barReleases.StartRelease.RotationX == DOFType.Free)
                lusasLineMesh.setEndRelease("Start", "thx", "free");
            if (barReleases.StartRelease.RotationY == DOFType.Free)
                lusasLineMesh.setEndRelease("Start", "thy", "free");
            if (barReleases.StartRelease.RotationZ == DOFType.Free)
                lusasLineMesh.setEndRelease("Start", "thz", "free");

            if (barReleases.EndRelease.TranslationX == DOFType.Free)
                lusasLineMesh.setEndRelease("End", "u", "free");
            if (barReleases.EndRelease.TranslationY == DOFType.Free)
                lusasLineMesh.setEndRelease("End", "v", "free");
            if (barReleases.EndRelease.TranslationY == DOFType.Free)
                lusasLineMesh.setEndRelease("End", "w", "free");
            if (barReleases.EndRelease.RotationX == DOFType.Free)
                lusasLineMesh.setEndRelease("End", "thx", "free");
            if (barReleases.EndRelease.RotationY == DOFType.Free)
                lusasLineMesh.setEndRelease("End", "thy", "free");
            if (barReleases.EndRelease.RotationZ == DOFType.Free)
                lusasLineMesh.setEndRelease("End", "thz", "free");
        }
    }
}