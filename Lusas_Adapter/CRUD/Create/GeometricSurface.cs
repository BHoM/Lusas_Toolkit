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
        public IFAttribute CreateGeometricSurface(ConstantThickness thickness)
        {
            IFAttribute lusasGeometricSurface = null;

            if (d_LusasData.existsAttribute("Surface Geometric", "G" + thickness.CustomData[AdapterId] + "/" + thickness.Name))
            {
                lusasGeometricSurface = d_LusasData.getAttribute("Surface Geometric", "G" + thickness.CustomData[AdapterId] + "/" + thickness.Name);
            }
            else
            {
                lusasGeometricSurface = d_LusasData.createGeometricSurface("G" + thickness.CustomData[AdapterId] + "/" + thickness.Name);
                lusasGeometricSurface.setValue("t", thickness.Thickness);
            }

            return lusasGeometricSurface;
        }
    }
}

