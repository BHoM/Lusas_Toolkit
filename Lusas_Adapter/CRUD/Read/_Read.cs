using System;
using System.Collections;
using System.Collections.Generic;
using BH.oM.Base;
using BH.oM.Geometry;
using BH.oM.Structure.Elements;
using BH.oM.Structure.Properties.Constraint;
using BH.oM.Structure.Properties.Surface;
using BH.oM.Structure.Properties.Section;
using BH.oM.Structure.Loads;
using BH.oM.Common.Materials;
using BH.oM.Adapters.Lusas;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        protected override IEnumerable<IBHoMObject> Read(Type type, IList ids = null)
        {
            if (type == typeof(Bar))
                return ReadBars(ids as dynamic);
            else if (type == typeof(Node))
                return ReadNodes(ids as dynamic);
            else if (type == typeof(Material))
                return ReadMaterials(ids as dynamic);
            else if (type == typeof(PanelPlanar))
                return ReadPlanarPanels(ids as dynamic);
            else if (type == typeof(Edge))
                return ReadEdges(ids as dynamic);
            else if (type == typeof(Point))
                return ReadPoints(ids as dynamic);
            else if (type == typeof(Constraint6DOF))
                return Read6DOFConstraints(ids as dynamic);
            else if (type == typeof(Constraint4DOF))
                return Read4DOFConstraints(ids as dynamic);
            else if (type == typeof(Loadcase))
                return ReadLoadcases(ids as dynamic);
            else if (typeof(ILoad).IsAssignableFrom(type))
                return ChooseLoad(type, ids as dynamic);
            else if (typeof(ISurfaceProperty).IsAssignableFrom(type))
                return Read2DProperties(ids as dynamic);
            else if (typeof(ISectionProperty).IsAssignableFrom(type))
                return ReadSectionProperties(ids as dynamic);
            else if (type == typeof(LoadCombination))
                return ReadLoadCombinations(ids as dynamic);
            else if (type == typeof(BHoMObject))
                return ReadAll(ids as dynamic);
            else if (type == typeof(MeshSettings1D))
                return Read1DMeshSettings(ids as dynamic);
            else if (type == typeof(MeshSettings2D))
                return Read2DMeshSettings(ids as dynamic);
            return null;
        }
    }
}
