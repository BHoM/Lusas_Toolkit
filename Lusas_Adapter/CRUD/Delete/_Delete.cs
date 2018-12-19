using System;
using System.Collections.Generic;
using BH.oM.Common.Materials;
using BH.oM.Structure.Elements;
using BH.oM.Structure.Properties.Section;
using BH.oM.Structure.Properties.Surface;
using BH.oM.Structure.Properties.Constraint;
using BH.oM.Structure.Loads;
using BH.oM.Adapters.Lusas;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        protected override int Delete(Type type, IEnumerable<object> ids)
        {
            int success = 0;

            if (type == typeof(Node))
                success = DeletePoints(ids);
            else if (type == typeof(Bar))
                success = DeleteLines(ids);
            else if (type == typeof(PanelPlanar))
                success = DeleteSurfaces(ids);
            else if (typeof(ISectionProperty).IsAssignableFrom(type))
                success = DeleteSectionProperties(ids);
            else if (typeof(ISurfaceProperty).IsAssignableFrom(type))
                success = DeleteSurfaceProperties(ids);
            else if (type == typeof(MeshSettings1D))
                success = DeleteMeshSettings1D(ids);
            else if (type == typeof(Constraint4DOF))
                success = DeleteMeshSettings2D(ids);
            else if (type == typeof(Constraint6DOF))
                success = DeleteConstraint6DOF(ids);
            else if (type == typeof(Material))
                success = DeleteMaterials(ids);
            else if (type == typeof(AreaTemperatureLoad))
                success = DeleteAreaTemperatureLoad(ids);
            else if (type == typeof(AreaUniformalyDistributedLoad))
                success = DeleteAreaUnformlyDistributedLoads(ids);
            else if (type == typeof(BarPointLoad))
                success = DeleteBarPointLoad(ids);
            else if (type == typeof(BarTemperatureLoad))
                success = DeleteBarTemperatureLoad(ids);
            else if (type == typeof(BarUniformlyDistributedLoad))
                success = DeleteBarUniformlyDistributedLoads(ids);
            else if (type == typeof(Loadcase))
                success = DeleteLoadcases(ids);
            else if (type == typeof(LoadCombination))
                success = DeleteLoadCombinations(ids);
            else if (type == typeof(PointDisplacement))
                success = DeletePointDisplacements(ids);
            else if (type == typeof(PointForce))
                success = DeletePointForces(ids);

            return 0;
        }
    }
}
