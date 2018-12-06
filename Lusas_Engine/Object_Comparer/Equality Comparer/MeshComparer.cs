using System.Collections.Generic;
using BH.oM.Structure.Elements;
using BH.Engine.Geometry;
using BH.Engine.Structure;

namespace BH.Engine.Lusas.Object_Comparer.Equality_Comparer
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

            if (bar1.FEAType.Equals(bar2.FEAType) && bar1.OrientationAngle.Equals(bar2.OrientationAngle) && bar1.Release.Equals(bar2.Release) && bar1.CustomData["Mesh"].Equals(bar2.CustomData["Mesh"]))
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

            return bar.StartNode.GetHashCode() ^ bar.EndNode.GetHashCode();
        }

    }
}
