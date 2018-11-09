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
        public IFAttribute CreateGeometricSurface(IProperty2D property2D)
        {
            IFAttribute lusasAttribute = null;
            string lusasAttributeName = "G" + property2D.CustomData[AdapterId] + "/" + property2D.Name;

            if (d_LusasData.existsAttribute("Surface Geometric", lusasAttributeName))
            {
                lusasAttribute = d_LusasData.getAttribute("Surface Geometric", lusasAttributeName);
            }
            else
            {
                IFGeometricSurface lusasGeometricSurface = CreateSurfraceProfile(property2D as dynamic, lusasAttributeName);
                lusasAttribute = lusasGeometricSurface;
            }
            return lusasAttribute;
        }

        private IFAttribute CreateSurfraceProfile(ConstantThickness bhomThickness, string lusasAttributeName)
        {
            IFGeometricSurface lusasGeometricSurface = d_LusasData.createGeometricSurface(lusasAttributeName);
            lusasGeometricSurface.setValue("t", bhomThickness.Thickness);
            return lusasGeometricSurface;
        }

        private IFGeometricSurface CreateSurfraceProfile(LoadingPanelProperty bhomThickness, string lusasAttributeName)
        {
            Engine.Reflection.Compute.RecordError("LoadingPanelProperty not supported in Lusas_Toolkit");
            return null;
        }

        private IFGeometricSurface CreateSurfraceProfile(Ribbed bhomThickness, string lusasAttributeName)
        {
            Engine.Reflection.Compute.RecordError("Ribbed not supported in Lusas_Toolkit");
            return null;
        }

        private IFGeometricSurface CreateSurfraceProfile(Waffle bhomThickness, string lusasAttributeName)
        {
            Engine.Reflection.Compute.RecordError("Waffle not supported in Lusas_Toolkit");
            return null;
        }

    }
}

