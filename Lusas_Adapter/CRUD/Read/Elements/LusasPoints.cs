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
        private List<IFPoint> ReadLusasPoints(List<string> ids = null)
        {
            object[] lusasPoints = d_LusasData.getObjects("Point");

            List<IFPoint> lusasPointList = new List<IFPoint>();

            for (int i = 0; i < lusasPoints.Count(); i++)
            {
                IFPoint lusasPoint = (IFPoint)lusasPoints[i];
                lusasPointList.Add(lusasPoint);
            }
            return lusasPointList;
        }
    }
}