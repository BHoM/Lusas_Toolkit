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
            if (System.Object.ReferenceEquals(edge1, edge2)) return true;

            //Check whether any of the compared objects is null.
            if (System.Object.ReferenceEquals(edge1, null) || System.Object.ReferenceEquals(edge2, null))
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
            if (System.Object.ReferenceEquals(edge, null)) return 0;

            return edge.Curve.IPointAtParameter(0.5).GetHashCode();
        }


        /***************************************************/
        /**** Private Fields                            ****/
        /***************************************************/

        private PointDistanceComparer m_pointComparer;


        /***************************************************/

    }
}
