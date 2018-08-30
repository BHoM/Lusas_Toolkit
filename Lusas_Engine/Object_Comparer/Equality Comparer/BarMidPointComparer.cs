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
            if (System.Object.ReferenceEquals(bar1, bar2)) return true;

            //Check whether any of the compared objects is null.
            if (System.Object.ReferenceEquals(bar1, null) || System.Object.ReferenceEquals(bar2, null))
                return false;

            //Check if the GUIDs are the same
            if (bar1.BHoM_Guid == bar2.BHoM_Guid)
                return true;

            if (m_pointComparer.Equals(bar1.Geometry().IPointAtParameter(0.5), bar2.Geometry().IPointAtParameter(0.5)))
            {
                return m_pointComparer.Equals(bar1.Geometry().IPointAtParameter(0.5), bar2.Geometry().IPointAtParameter(0.5));
            }

            return false;
        }

        /***************************************************/

        public int GetHashCode(Bar bar)
        {
            //Check whether the object is null
            if (System.Object.ReferenceEquals(bar, null)) return 0;

            return bar.StartNode.GetHashCode() ^ bar.EndNode.GetHashCode();
        }


        /***************************************************/
        /**** Private Fields                            ****/
        /***************************************************/

        private PointDistanceComparer m_pointComparer;


        /***************************************************/

    }
}
