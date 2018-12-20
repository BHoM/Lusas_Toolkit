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

using System.Collections.Generic;
using System;
using BH.oM.Geometry;

namespace BH.Engine.Lusas.Object_Comparer.Equality_Comparer
{
    public class PointDistanceComparer : IEqualityComparer<Point>
    {
        /***************************************************/
        /**** Constructors                              ****/
        /***************************************************/

        public PointDistanceComparer()
        {
            //TODO: Grab tolerance from global tolerance settings
            m_multiplier = 1000;
        }

        /***************************************************/

        public PointDistanceComparer(int decimals)
        {
            m_multiplier = Math.Pow(10, decimals);
        }


        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public bool Equals(Point point1, Point point2)
        {
            //Check whether the compared objects reference the same data.
            if (ReferenceEquals(point1, point2)) return true;

            //Check whether any of the compared objects is null.
            if (ReferenceEquals(point1, null) || System.Object.ReferenceEquals(point2, null))
                return false;

            if ((int)Math.Round(point1.X * m_multiplier) != (int)Math.Round(point2.X * m_multiplier))
                return false;

            if ((int)Math.Round(point1.Y * m_multiplier) != (int)Math.Round(point2.Y * m_multiplier))
                return false;

            if ((int)Math.Round(point1.Z * m_multiplier) != (int)Math.Round(point2.Z * m_multiplier))
                return false;

            return true;
        }

        /***************************************************/

        public int GetHashCode(Point point)
        {
            //Check whether the object is null
            if (ReferenceEquals(point, null)) return 0;

            int x = ((int)Math.Round(point.X * m_multiplier)).GetHashCode();
            int y = ((int)Math.Round(point.Y * m_multiplier)).GetHashCode();
            int z = ((int)Math.Round(point.Z * m_multiplier)).GetHashCode();
            return x ^ y ^ z;

        }


        /***************************************************/
        /**** Private Fields                            ****/
        /***************************************************/

        private double m_multiplier;


        /***************************************************/

    }
}
