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

        public IFSurface CreateSurface(PanelPlanar panel, List<Node> existingNodes, List<Edge> existingEdges)
        {
            IFObjectSet tempGroup = d_LusasData.createGroup("temp");

            foreach (Edge edge in panel.ExternalEdges)
            {
                int edgeindex = existingEdges.FindIndex(m => Math.Round(m.Curve.IPointAtParameter(0.5).X, 3).Equals(Math.Round(edge.Curve.IPointAtParameter(0.5).X, 3)) &&
                            Math.Round(m.Curve.IPointAtParameter(0.5).Y, 3).Equals(Math.Round(edge.Curve.IPointAtParameter(0.5).Y, 3)) &&
                           Math.Round(m.Curve.IPointAtParameter(0.5).Z, 3).Equals(Math.Round(edge.Curve.IPointAtParameter(0.5).Z, 3)));

                tempGroup.add(d_LusasData.getLineByName(existingEdges[edgeindex].CustomData[AdapterId].ToString()));
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

            if (!(panel.Tags.Count == 0))
            {
                assignObjectSet(newSurface, panel.Tags);
            }

            return newSurface;
        }

    }
}