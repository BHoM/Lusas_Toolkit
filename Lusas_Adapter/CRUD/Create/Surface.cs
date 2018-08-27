using System.Collections.Generic;
using System.Linq;
using System;
using BH.oM.Base;
using BH.oM.Structure.Elements;
using BH.oM.Geometry;
using BH.oM.Structure.Properties;
using BH.oM.Structure.Loads;
using BH.oM.Common.Materials;
using BH.Engine.Geometry;
using BH.Engine.Structure;
using Lusas.LPI;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {

        public IFSurface CreateSurface(PanelPlanar panel, List<IFLine> lusasLines)
        {
            IFObjectSet tempGroup = d_LusasData.createGroup("temp");

            List<List<double>> lusascoords = new List<List<double>>();

            for (int i = 0; i < lusasLines.Count; i++)
            {
                object[] coordinates = lusasLines[i].getInterpolatedPosition(0.5);
                lusascoords.Add(new List<double> { (double)coordinates[0], (double)coordinates[1], (double)coordinates[2] });
            }

            foreach (Edge edge in panel.ExternalEdges)
            {
                Point midwayPoint = edge.Curve.IPointAtParameter(0.5);

                int index = lusascoords.FindIndex(m => Math.Round(m[0], 3).Equals(Math.Round(midwayPoint.X, 3)) &&
                                                       Math.Round(m[1], 3).Equals(Math.Round(midwayPoint.Y, 3)) &&
                                                       Math.Round(m[2], 3).Equals(Math.Round(midwayPoint.Z, 3)));
                tempGroup.add(lusasLines[index]);
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

            if(!(panel.Property == null))
            {
                if (!(panel.Property.Material == null))
                {
                    String materialName = "M" + panel.Property.Material.CustomData[AdapterId] + "/" + panel.Property.Material.Name;
                    IFAttribute lusasMaterial = d_LusasData.getAttribute("Material", materialName);
                    lusasMaterial.assignTo(newSurface);
                }
            }


            return newSurface;
        }

    }
}