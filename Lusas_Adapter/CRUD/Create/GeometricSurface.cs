using BH.oM.Structure.Properties;
using Lusas.LPI;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        public IFAttribute CreateGeometricSurface(IProperty2D property2D)
        {
            IFAttribute lusasAttribute = null;
            string lusasName = "G" + property2D.CustomData[AdapterId] + "/" + property2D.Name;

            if (d_LusasData.existsAttribute("Surface Geometric", lusasName))
            {
                lusasAttribute = d_LusasData.getAttribute("Surface Geometric", lusasName);
            }
            else
            {
                IFGeometricSurface lusasGeometricSurface = CreateSurfraceProfile(
                    property2D as dynamic, lusasName);

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

