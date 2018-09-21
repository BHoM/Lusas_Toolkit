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

        public IFSurface CreateSurface(PanelPlanar panel, IFLine[] lusasLines)
        {

            IFSurface newSurface;

            if (d_LusasData.existsSurfaceByName("S" + panel.CustomData[AdapterId]))
            {
                newSurface = d_LusasData.getSurfaceByName("S" + panel.CustomData[AdapterId]);
            }
            else
            {
                newSurface = d_LusasData.createSurfaceBy(lusasLines);
                newSurface.setName("S" + panel.CustomData[AdapterId]);
            }

            if (!(panel.Tags.Count == 0))
            {
                assignObjectSet(newSurface, panel.Tags);
            }

            if(!(panel.Property == null))
            {
                String geometricSurfaceName = "G" + panel.Property.CustomData[AdapterId] + "/" + panel.Property.Name;
                IFAttribute lusasGeometricSurface = d_LusasData.getAttribute("Surface Geometric", geometricSurfaceName);
                lusasGeometricSurface.assignTo(newSurface);
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