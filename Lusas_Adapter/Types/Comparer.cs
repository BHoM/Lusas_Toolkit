using BH.Engine.Base.Objects;
using BH.oM.Common.Materials;
using BH.oM.Structural.Elements;
using BH.oM.Structural.Properties;
using System;
using BH.oM.Geometry;
using System.Collections.Generic;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        /***************************************************/
        /**** BHoM Adapter Interface                    ****/
        /***************************************************/

        //Standard implementation of the comparer class.
        //Compares nodes by distance (down to 3 decimal places -> mm)
        //Compares Materials, SectionProprties, LinkConstraints, and Property2D by name
        //Add/remove any type in the dictionary below that you want (or not) a specific comparison method for

        protected override IEqualityComparer<T> Comparer<T>()
        {
            Type type = typeof(T);

            if (m_Comparers.ContainsKey(type))
            {
                return m_Comparers[type] as IEqualityComparer<T>;
            }
            else
            {
                return EqualityComparer<T>.Default;
            }

        }


        /***************************************************/
        /**** Private Fields                            ****/
        /***************************************************/

        private static Dictionary<Type, object> m_Comparers = new Dictionary<Type, object>
        {
            {typeof(Node), new BH.Engine.Structure.NodeDistanceComparer(3) },   //The 3 in here sets how many decimal places to look at for node merging. 3 decimal places gives mm precision
<<<<<<< HEAD
            {typeof(Edge), new BH.Engine.Structure.BarEndNodesDistanceComparer(3) },
            {typeof(Point), new BH.Engine.Structure.NodeDistanceComparer(3) },
            {typeof(ISectionProperty), new BHoMObjectNameOrToStringComparer() },
=======
            //{typeof(Edge), new BH.Engine.Structure.BarEndNodesDistanceComparer(3) },
            { typeof(Point), new BH.Engine.Structure.NodeDistanceComparer(3) },
            { typeof(ISectionProperty), new BHoMObjectNameOrToStringComparer() },
>>>>>>> CS-Test
            {typeof(Material), new BHoMObjectNameComparer() },
            {typeof(LinkConstraint), new BHoMObjectNameComparer() },
            {typeof(IProperty2D), new BHoMObjectNameComparer() },
        };
        /***************************************************/
    }
}

