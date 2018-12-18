using BH.oM.Structure.Elements;
using Lusas.LPI;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {

        public IFSurface CreateSurface(PanelPlanar panel, IFLine[] lusasLines)
        {
            IFSurface lusasSurface;
            if (d_LusasData.existsSurfaceByName("S" + panel.CustomData[AdapterId]))
            {
                lusasSurface = d_LusasData.getSurfaceByName("S" + panel.CustomData[AdapterId]);
            }
            else
            {
                lusasSurface = d_LusasData.createSurfaceBy(lusasLines);
                lusasSurface.setName("S" + panel.CustomData[AdapterId]);
            }

            if (!(panel.Tags.Count == 0))
            {
                AssignObjectSet(lusasSurface, panel.Tags);
            }

            if(!(panel.Property == null))
            {
                string geometricSurfaceName = "G" + 
                    panel.Property.CustomData[AdapterId] + "/" + panel.Property.Name;

                IFAttribute lusasGeometricSurface = d_LusasData.getAttribute(
                    "Surface Geometric", geometricSurfaceName);

                lusasGeometricSurface.assignTo(lusasSurface);
                if (!(panel.Property.Material == null))
                {
                    string materialName = "M" + panel.Property.Material.CustomData[AdapterId] + 
                        "/" + panel.Property.Material.Name;

                    IFAttribute lusasMaterial = d_LusasData.getAttribute("Material", materialName);
                    lusasMaterial.assignTo(lusasSurface);
                }
            }

            return lusasSurface;
        }

    }
}