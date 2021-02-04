/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2021, the respective contributors. All rights reserved.
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
using BH.oM.Structure.Elements;
using BH.Engine.Geometry;
using BH.Engine.Adapters.Lusas;


namespace BH.Engine.Adapters.Lusas.Object_Comparer.Equality_Comparer
{
    public class EdgeMidPointComparer : IEqualityComparer<Edge>
    {
        /***************************************************/
        /**** Constructors                              ****/
        /***************************************************/

        public EdgeMidPointComparer()
        {
            m_pointComparer = new PointDistanceComparer();
        }

        /***************************************************/

        public EdgeMidPointComparer(int decimals)
        {
            m_pointComparer = new PointDistanceComparer(decimals);
        }


        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public bool Equals(Edge edge1, Edge edge2)
        {
            //Check whether the compared objects reference the same data.
            if (ReferenceEquals(edge1, edge2)) return true;

            //Check whether any of the compared objects is null.
            if (ReferenceEquals(edge1, null) || ReferenceEquals(edge2, null))
                return false;

            if (ReferenceEquals(edge1.Curve, null) || ReferenceEquals(edge2.Curve, null))
                return false;

            if (!edge1.Curve.IsNurbsCurve() && !edge2.Curve.IsNurbsCurve())
                if (Query.InvalidEdgeCheck(edge1) || Query.InvalidEdgeCheck(edge2))
                    return false;

            //Check if the GUIDs are the same
            if (edge1.BHoM_Guid == edge2.BHoM_Guid)
                return true;

            if (m_pointComparer.Equals(edge1.Curve.IPointAtParameter(0.5), edge2.Curve.IPointAtParameter(0.5)))
            {
                return m_pointComparer.Equals(edge1.Curve.IPointAtParameter(0.5), edge2.Curve.IPointAtParameter(0.5));
            }

            return false;
        }

        /***************************************************/

        public int GetHashCode(Edge edge)
        {
            //Check whether the object is null
            if (ReferenceEquals(edge, null)) return 0;

            if (edge.Curve != null)
            {
                if (!edge.Curve.IsNurbsCurve())
                    if (!Query.InvalidEdgeCheck(edge))
                        return edge.Curve.IPointAtParameter(0.5).GetHashCode();
            }

            return 0;
        }


        /***************************************************/
        /**** Private Fields                            ****/
        /***************************************************/

        private PointDistanceComparer m_pointComparer;

        /***************************************************/

    }
}


