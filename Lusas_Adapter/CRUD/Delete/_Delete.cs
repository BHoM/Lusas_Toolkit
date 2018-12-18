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
            if (type == typeof(Bar))
                success = DeleteLines(ids);
            if (type == typeof(PanelPlanar))
                success = DeleteSurfaces(ids);
            if (type == typeof(ISectionProperty))
                success = DeleteSectionProperties(ids);
            if (type == typeof(ISurfaceProperty))
                success = DeleteSurfaceProperties(ids);
            if (type == typeof(MeshSettings1D))
                success = DeleteMeshSettings1D(ids);
            if (type == typeof(Constraint4DOF))
                success = DeleteMeshSettings2D(ids);
            if (type == typeof(Constraint6DOF))
                success = DeleteConstraint6DOF(ids);
            if (type == typeof(Material))
                success = DeleteMaterials(ids);
            if (type == typeof(AreaTemperatureLoad))
                success = DeleteAreaTemperatureLoad(ids);
            if (type == typeof(AreaUniformalyDistributedLoad))
                success = DeleteAreaUnformlyDistributedLoads(ids);
            if (type == typeof(BarPointLoad))
                success = DeleteBarPointLoad(ids);
            if (type == typeof(BarTemperatureLoad))
                success = DeleteBarTemperatureLoad(ids);
            if (type == typeof(BarUniformlyDistributedLoad))
                success = DeleteBarUniformlyDistributedLoads(ids);
            if (type == typeof(Loadcase))
                success = DeleteLoadcases(ids);
            if (type == typeof(LoadCombination))
                success = DeleteLoadCombinations(ids);
            if (type == typeof(PointDisplacement))
                success = DeletePointDisplacements(ids);
            if (type == typeof(PointForce))
                success = DeletePointForces(ids);

            return 0;
        }
    }
}
