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
        public IFAttribute ICreateGeometricLine(ISectionProperty sectionProperty)
        {

            return CreateGeometricLine(sectionProperty as dynamic);

            SteelSection section = (SteelSection)sectionProperty;



            int shape = (int)section.SectionProfile.Shape;

            int lusasShape = 0;

            if (shape == 0) //Rectangle
                lusasShape = 1;
            else if (shape == 1) //Box
                lusasShape = 2;
            else if (shape == 3) //ISection
                lusasShape = 5; //14 if unequal flanges
            else if (shape == 4) //TSection
                lusasShape = 8;
            else if (shape == 7) //Circular section
                lusasShape = 3;
            else if (shape == 6) //Tube
                lusasShape = 4;

            IFAttribute lusasGeometricLine = null;
            string lusasAttributeName = "G" + sectionProperty.CustomData[AdapterId] + "/" + sectionProperty.Name;
            if (d_LusasData.existsAttribute("Surface Geometric", lusasAttributeName))
            {
                lusasGeometricLine = d_LusasData.getAttribute("Surface Geometric", lusasAttributeName);
            }
            else
            {
                lusasGeometricLine = d_LusasData.createGeometricLine(lusasAttributeName);
                lusasGeometricLine.setValue("Type",);
                lusasGeometricLine.setValue("elementType", "3D Thick Beam");
            }
            return lusasGeometricLine;
        }


        private IFAttribute CreateGeometricLine(SteelSection sectionProperty)
        {
            return CreateGeometricLine(sectionProperty.SectionProfile as dynamic);
        }

        private IFAttribute CreateGeometricLine(ConcreteSection sectionProperty)
        {
            return CreateGeometricLine(sectionProperty.SectionProfile as dynamic);
        }

        private IFAttribute CreateGeometricLine(ExplicitSection sectionProperty)
        {
           
        }

        private IFAttribute CreateGeometricLine(RectangleProfile rectangelProfile)
        {
            rectangelProfile.
        }
    }
}

