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
using BH.Engine.Analytical;

namespace BH.Engine.Adapters.Lusas.Object_Comparer.Equality_Comparer
{
    public class BarMidPointComparer : IEqualityComparer<Bar>
    {
        /***************************************************/
        /**** Constructors                              ****/
        /***************************************************/

        public BarMidPointComparer()
        {
            m_pointComparer = new PointDistanceComparer();
        }

        /***************************************************/

        public BarMidPointComparer(int decimals)
        {
            m_pointComparer = new PointDistanceComparer(decimals);
        }


        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public bool Equals(Bar bar1, Bar bar2)
        {
            //Check whether the compared objects reference the same data.
            if (ReferenceEquals(bar1, bar2)) return true;

            //Check if the GUIDs are the same
            if (bar1.BHoM_Guid == bar2.BHoM_Guid)
                return true;

            //Check whether any of the compared objects is null.
            if (ReferenceEquals(bar1, null) || ReferenceEquals(bar2, null))
                return false;

            if (ReferenceEquals(bar1.StartNode, null) || ReferenceEquals(bar1.EndNode, null) ||
                ReferenceEquals(bar2.StartNode, null) || ReferenceEquals(bar2.EndNode, null))
                return false;

            if (ReferenceEquals(bar1.StartNode.Position, null) || ReferenceEquals(bar1.EndNode.Position, null) ||
                ReferenceEquals(bar2.StartNode.Position, null) || ReferenceEquals(bar2.EndNode.Position, null))
                return false;

            if (m_pointComparer.Equals(
                bar1.Geometry().IPointAtParameter(0.5),
                bar2.Geometry().IPointAtParameter(0.5)))
            {
                return m_pointComparer.Equals(
                    bar1.Geometry().IPointAtParameter(0.5),
                    bar2.Geometry().IPointAtParameter(0.5));
            }

            return false;
        }

        /***************************************************/

        public int GetHashCode(Bar bar)
        {
            //Check whether the object is null
            if (!ReferenceEquals(bar, null))
                if (!ReferenceEquals(bar.StartNode, null) || !ReferenceEquals(bar.EndNode, null))
                    return bar.StartNode.GetHashCode() ^ bar.EndNode.GetHashCode();

            return 0;
        }


        /***************************************************/
        /**** Private Fields                            ****/
        /***************************************************/

        private PointDistanceComparer m_pointComparer;

        /***************************************************/

    }
}





