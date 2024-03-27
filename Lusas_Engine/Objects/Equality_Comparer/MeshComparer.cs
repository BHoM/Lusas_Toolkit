/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2024, the respective contributors. All rights reserved.
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
using BH.oM.Structure.Elements;
using BH.Engine.Geometry;
using BH.Engine.Structure;
using BH.Engine.Base;
using BH.oM.Adapters.Lusas.Fragments;

namespace BH.Engine.Adapters.Lusas.Object_Comparer.Equality_Comparer
{
    public class LineMeshComparer : IEqualityComparer<Bar>
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public bool Equals(Bar bar1, Bar bar2)
        {
            //Check whether the compared objects reference the same data.
            if (ReferenceEquals(bar1, bar2)) return true;

            //Check whether any of the compared objects is null.
            if (ReferenceEquals(bar1, null) || ReferenceEquals(bar2, null))
                return false;

            //Check if the GUIDs are the same
            if (bar1.BHoM_Guid == bar2.BHoM_Guid)
                return true;

            if (bar1.FEAType.Equals(bar2.FEAType) && bar1.OrientationAngle.Equals(bar2.OrientationAngle) && bar1.Release.Equals(bar2.Release) && bar1.FindFragment<MeshSettings1D>().Equals(bar2.FindFragment<MeshSettings2D>()))
            {
                return true;
            }

            return false;
        }

        /***************************************************/

        public int GetHashCode(Bar bar)
        {
            //Check whether the object is null
            if (ReferenceEquals(bar, null)) return 0;

            return bar.Start.GetHashCode() ^ bar.End.GetHashCode();
        }

    }
}





