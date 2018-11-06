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
        public IFAttribute CreateGeometricLine(ISectionProperty sectionProperty)
        {
            IFAttribute lusasAttribute = null;
            string lusasAttributeName = "G" + sectionProperty.CustomData[AdapterId] + "/" + sectionProperty.Name;

            if (d_LusasData.existsAttribute("Line Geometric", lusasAttributeName))
            {
                lusasAttribute = d_LusasData.getAttribute("Line Geometric", lusasAttributeName);
            }
            else
            {
                IFGeometricLine lusasGeometricLine = d_LusasData.createGeometricLine(lusasAttributeName);
                lusasGeometricLine.setValue("elementType", "3D Thick Beam");
                CreateSection(sectionProperty as dynamic, lusasGeometricLine);
                lusasAttribute = lusasGeometricLine;
            }
            return lusasAttribute;
        }


        private void CreateSection(SteelSection sectionProperty, IFGeometricLine lusasGeometricLine)
        {
            CreateProfile(sectionProperty.SectionProfile as dynamic, lusasGeometricLine);
        }

        private void CreateSection(ConcreteSection sectionProperty, string attributeName)
        {
            throw new NotImplementedException();
        }

        private void CreateSection(ExplicitSection sectionProperty)
        {
            throw new NotImplementedException();
        }

        private void CreateProfile(RectangleProfile bhomProfile, IFGeometricLine lusasGeometricLine)
        {
            List<double> dimensionList = new List<double> { bhomProfile.Width, bhomProfile.Height };
            double[] dimensionArray = dimensionList.ToArray();

            List<string> valueList = new List<string> { "B", "D" };
            string[] valueArray = valueList.ToArray();

            int lusasType = 1;
            CreateLibrarySection(lusasGeometricLine, dimensionArray, valueArray, lusasType);
        }

        private void CreateProfile(BoxProfile bhomProfile, IFGeometricLine lusasGeometricLine)
        {
            List<double> dimensionList = new List<double> { bhomProfile.Width, bhomProfile.Height, bhomProfile.Thickness };
            double[] dimensionArray = dimensionList.ToArray();

            List<string> valueList = new List<string> { "B", "D", "t" };
            string[] valueArray = valueList.ToArray();

            int lusasType = 2;
            CreateLibrarySection(lusasGeometricLine, dimensionArray, valueArray, lusasType);
        }

        private void CreateProfile(CircleProfile bhomProfile, IFGeometricLine lusasGeometricLine)
        {
            List<double> dimensionList = new List<double> { bhomProfile.Diameter };
            double[] dimensionArray = dimensionList.ToArray();

            List<string> valueList = new List<string> { "D" };
            string[] valueArray = valueList.ToArray();

            int lusasType = 3;
            CreateLibrarySection(lusasGeometricLine, dimensionArray, valueArray, lusasType);
        }

        private void CreateProfile(TubeProfile bhomProfile, IFGeometricLine lusasGeometricLine)
        {
            List<double> dimensionList = new List<double> { bhomProfile.Diameter, bhomProfile.Thickness };
            double[] dimensionArray = dimensionList.ToArray();

            List<string> valueList = new List<string> { "D", "t" };
            string[] valueArray = valueList.ToArray();

            int lusasType = 4;
            CreateLibrarySection(lusasGeometricLine, dimensionArray, valueArray, lusasType);
        }

        private void CreateProfile(ISectionProfile bhomProfile, IFGeometricLine lusasGeometricLine)
        {
            List<double> dimensionList = new List<double> { bhomProfile.Height, bhomProfile.Width,
            bhomProfile.WebThickness, bhomProfile.FlangeThickness};
            double[] dimensionArray = dimensionList.ToArray();

            List<string> valueList = new List<string> { "D", "B", "tw", "tf" };
            string[] valueArray = valueList.ToArray();

            int lusasType = 5;
            CreateLibrarySection(lusasGeometricLine, dimensionArray, valueArray, lusasType);
        }

        private void CreateProfile(TSectionProfile bhomProfile, IFGeometricLine lusasGeometricLine)
        {
            List<double> dimensionList = new List<double> { bhomProfile.Height, bhomProfile.Width,
            bhomProfile.FlangeThickness, bhomProfile.WebThickness};
            double[] dimensionArray = dimensionList.ToArray();

            List<string> valueList = new List<string> { "D", "B", "tf", "tw" };
            string[] valueArray = valueList.ToArray();

            int lusasType = 8;
            CreateLibrarySection(lusasGeometricLine, dimensionArray, valueArray, lusasType);
        }

        private void CreateProfile(FabricatedISectionProfile bhomProfile, IFGeometricLine lusasGeometricLine)
        {
            List<double> dimensionList = new List<double> { bhomProfile.Height, bhomProfile.TopFlangeWidth,
                bhomProfile.BotFlangeWidth, bhomProfile.TopFlangeWidth, bhomProfile.TopFlangeThickness,
            bhomProfile.WebThickness};
            double[] dimensionArray = dimensionList.ToArray();

            List<string> valueList = new List<string> { "D", "Bt", "Bb", "tft", "bft", "tw" };
            string[] valueArray = valueList.ToArray();

            int lusasType = 14;
            CreateLibrarySection(lusasGeometricLine, dimensionArray, valueArray, lusasType);
        }

        private void CreateLibrarySection(IFGeometricLine lusasGeometricLine, double[] dimensionArray, string[] valueArray, int lusasType)
        {
            string sectionName = lusasGeometricLine.getName();
            m_LusasApplication.addLibrarySection(true, sectionName, lusasType, dimensionArray, true);
            lusasGeometricLine.setFromLibrary("User Sections", "Local", sectionName, 0, 0);
        }
    }
}

