using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BH.oM.Base;
using BH.oM.Geometry;
using BH.oM.Structure.Elements;
using BH.oM.Structure.Properties;
using BH.oM.Structure.Loads;
using BH.oM.Common.Materials;
using Lusas.LPI;
using BH.oM.Adapters.Lusas;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        private List<BHoMObject> ReadElements(List<string> ids = null)
        {
            List<BHoMObject> objects = new List<BHoMObject>();

            objects.AddRange(ReadNodes());
            objects.AddRange(ReadBars());
            objects.AddRange(ReadPlanarPanels());

            return objects;
        }
    }
}