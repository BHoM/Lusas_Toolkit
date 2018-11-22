using BH.Engine.Base.Objects;
using BH.oM.Common.Materials;
using BH.oM.Structure.Elements;
using BH.oM.Structure.Properties;
using System;
using BH.oM.Geometry;
using BH.oM.Structure.Loads;
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
            {typeof(Node), new Engine.Structure.NodeDistanceComparer(3) },
            {typeof(Bar), new Engine.Lusas.Object_Comparer.Equality_Comparer.BarMidPointComparer(3)},
            {typeof(Edge), new Engine.Lusas.Object_Comparer.Equality_Comparer.EdgeMidPointComparer(3) },
            { typeof(Point), new Engine.Lusas.Object_Comparer.Equality_Comparer.PointDistanceComparer(3) },
            {typeof(Material), new BHoMObjectNameComparer() },
            {typeof(LinkConstraint), new BHoMObjectNameComparer() },
            {typeof(IProperty2D), new BHoMObjectNameComparer() },
            {typeof(ISectionProperty), new BHoMObjectNameComparer() },
            {typeof(ILoad), new BHoMObjectNameComparer() },
            {typeof(Loadcase), new BHoMObjectNameComparer()},
            {typeof(LoadCombination), new BHoMObjectNameComparer()}
        };
        /***************************************************/
    }
}

