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
            IFAttribute lusasGeometricSurface = null;
            ConstantThickness constantThickness = property2D as ConstantThickness;
            string lusasAttributeName = "G" + constantThickness.CustomData[AdapterId] + "/" + constantThickness.Name;

            if (d_LusasData.existsAttribute("Surface Geometric", lusasAttributeName))
            {
                lusasGeometricSurface = d_LusasData.getAttribute("Surface Geometric", lusasAttributeName);
            }
            else
            {
                lusasGeometricSurface = d_LusasData.createGeometricSurface("G" + constantThickness.CustomData[AdapterId] + "/" + constantThickness.Name);
                lusasGeometricSurface.setValue("t", constantThickness.Thickness);
            }

            return lusasGeometricSurface;
        }
    }
}

