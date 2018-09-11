using System.Collections.Generic;
using System.Linq;
using System;
using BH.oM.Base;
using BH.oM.Structure.Elements;
using BH.oM.Geometry;
using BH.oM.Structure.Properties;
using BH.oM.Structure.Loads;
using BH.oM.Common.Materials;
using BH.Engine.Geometry;
using BH.Engine.Structure;
using Lusas.LPI;

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
            if (System.Object.ReferenceEquals(point1, point2)) return true;

            //Check whether any of the compared objects is null.
            if (System.Object.ReferenceEquals(point1, null) || System.Object.ReferenceEquals(point2, null))
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
            if (System.Object.ReferenceEquals(point, null)) return 0;

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
