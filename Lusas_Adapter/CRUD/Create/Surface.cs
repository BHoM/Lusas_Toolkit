using System.Collections.Generic;
using System.Linq;
using System;
using BH.oM.Base;
using BH.oM.Structural.Elements;
using BH.oM.Geometry;
using BH.oM.Structural.Properties;
using BH.oM.Structural.Loads;
using BH.oM.Common.Materials;
using BH.Engine.Geometry;
using BH.Engine.Structure;
using LusasM15_2;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {

        public IFSurface CreateSurface(PanelPlanar panel, List<Point> distinctPoints, List<IFPoint> LusasPoints, List<ICurve> distinctEdges, List<IFLine> LusasLines)
        {
            IFObjectSet tempGroup = d_LusasData.createGroup("temp");

            foreach (ICurve edge in panel.AllEdgeCurves())
            {
                int edgeindex = distinctEdges.FindIndex(m => Math.Round(m.IPointAtParameter(0.5).X, 3).Equals(Math.Round(edge.IPointAtParameter(0.5).X, 3)) &&
                            Math.Round(m.IPointAtParameter(0.5).Y, 3).Equals(Math.Round(edge.IPointAtParameter(0.5).Y, 3)) &&
                           Math.Round(m.IPointAtParameter(0.5).Z, 3).Equals(Math.Round(edge.IPointAtParameter(0.5).Z, 3)));

                tempGroup.add(LusasLines[edgeindex]);
            }

            IFSurface newSurface;

            if (d_LusasData.existsSurfaceByName("S" + panel.CustomData[AdapterId]))
            {
                newSurface = d_LusasData.getSurfaceByName("S" + panel.CustomData[AdapterId]);
            }
            else
            {
                newSurface = d_LusasData.createSurfaceBy(tempGroup);
                newSurface.setName("S" + panel.CustomData[AdapterId]);
            }
            d_LusasData.getGroupByName("temp").ungroup();

            return newSurface;
        }

    }
}