using BH.oM.Structure.Properties.Surface;
using Lusas.LPI;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        public IFAttribute CreateGeometricSurface(ISurfaceProperty surfaceProperty)
        {
            if (!CheckIllegalCharacters(surfaceProperty.Name))
            {
                return null;
            }

            IFAttribute lusasAttribute = null;
            string lusasName = "G" + surfaceProperty.CustomData[AdapterId] + "/" + surfaceProperty.Name;

            if (d_LusasData.existsAttribute("Surface Geometric", lusasName))
            {
                lusasAttribute = d_LusasData.getAttribute("Surface Geometric", lusasName);
            }
            else
            {
                IFGeometricSurface lusasGeometricSurface = CreateSurfraceProfile(
                    surfaceProperty as dynamic, lusasName);

                lusasAttribute = lusasGeometricSurface;
            }
            return lusasAttribute;
        }

        private IFAttribute CreateSurfraceProfile(ConstantThickness bhomThickness, string lusasName)
        {
            IFGeometricSurface lusasGeometricSurface = d_LusasData.createGeometricSurface(lusasName);
            lusasGeometricSurface.setValue("t", bhomThickness.Thickness);
            return lusasGeometricSurface;
        }

        private IFGeometricSurface CreateSurfraceProfile(LoadingPanelProperty bhomThickness, string lusasName)
        {
            Engine.Reflection.Compute.RecordError("LoadingPanelProperty not supported in Lusas_Toolkit");
            return null;
        }

        private IFGeometricSurface CreateSurfraceProfile(Ribbed bhomThickness, string lusasName)
        {
            Engine.Reflection.Compute.RecordError("Ribbed not supported in Lusas_Toolkit");
            return null;
        }

        private IFGeometricSurface CreateSurfraceProfile(Waffle bhomThickness, string lusasName)
        {
            Engine.Reflection.Compute.RecordError("Waffle not supported in Lusas_Toolkit");
            return null;
        }

    }
}

