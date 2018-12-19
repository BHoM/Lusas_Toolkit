using System.Collections.Generic;
using System;
using BH.oM.Structure.Elements;
using BH.oM.Structure.Properties.Constraint;
using BH.oM.Structure.Properties.Section;
using BH.oM.Structure.Properties.Surface;
using BH.oM.Structure.Loads;
using BH.oM.Common.Materials;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        /***************************************************/
        /**** BHoM Adapter Interface                    ****/
        /***************************************************/

        //Standard implementation for dependency types (change the dictionary below to override):

        protected override List<Type> DependencyTypes<T>()
        {
            Type type = typeof(T);

            if (m_DependencyTypes.ContainsKey(type))
                return m_DependencyTypes[type];

            else if (type.BaseType != null && m_DependencyTypes.ContainsKey(type.BaseType))
                return m_DependencyTypes[type.BaseType];

            else
            {
                foreach (Type interType in type.GetInterfaces())
                {
                    if (m_DependencyTypes.ContainsKey(interType))
                        return m_DependencyTypes[interType];
                }
            }

            return new List<Type>();
        }

        /***************************************************/
        /**** Private Fields                            ****/
        /***************************************************/

        private static Dictionary<Type, List<Type>> m_DependencyTypes = new Dictionary<Type, List<Type>>
        {
            {typeof(PanelPlanar), new List<Type> { typeof(ISurfaceProperty), typeof(Edge)} },
            {typeof(Bar), new List<Type> { typeof(Node) , typeof(ISectionProperty), typeof(Constraint4DOF) } },
            {typeof(Node), new List<Type> { typeof(Constraint6DOF) } },
            {typeof(ISectionProperty), new List<Type> { typeof(Material) } },
            {typeof(RigidLink), new List<Type> { typeof(LinkConstraint), typeof(Node) } },
            {typeof(MeshFace), new List<Type> { typeof(ISurfaceProperty), typeof(Node) } },
            {typeof(ISurfaceProperty), new List<Type> { typeof(Material) } },
            //{typeof(LoadCombination), new List<Type> { typeof(Loadcase) } },
            {typeof(ILoad), new List<Type> {typeof(Loadcase) } },
        };


        /***************************************************/
    }
}
