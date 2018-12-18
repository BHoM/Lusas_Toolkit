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
                success = DeleteMeshSettings1D(ids);
            if (type == typeof(Constraint6DOF))
                success = DeleteMeshSettings1D(ids);
            if (type == typeof(Material))
                success = DeleteMeshSettings1D(ids);
            if (type == typeof(AreaTemperatureLoad))
                success = DeleteMeshSettings1D(ids);

            return 0;
        }
    }
}
