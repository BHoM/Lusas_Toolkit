using System;
using System.Collections.Generic;
using BH.oM.Structure.Elements;
using BH.oM.Structure.Properties.Section;

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
            //if (type == typeof(ISectionProperty))
            //    success = DeleteSectionProperties(ids);

            return 0;
        }
    }
}
