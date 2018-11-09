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
using BH.Engine.Reflection;
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
                IFGeometricLine lusasGeometricLine = CreateSection(sectionProperty as dynamic, lusasAttributeName);
                lusasAttribute = lusasGeometricLine;
            }
            return lusasAttribute;
        }


        private IFAttribute CreateSection(SteelSection sectionProperty, string lusasAttributeName)
        {
            IFAttribute lusasGeometricLine = CreateProfile(sectionProperty.SectionProfile as dynamic, lusasAttributeName);
            return lusasGeometricLine;
        }

        private IFAttribute CreateSection(ConcreteSection sectionProperty, string lusasAttributeName)
        {
            Engine.Reflection.Compute.RecordError("ConcreteSection not supported in Lusas_Toolkit");
            return null;
        }

        private IFAttribute CreateSection(ExplicitSection sectionProperty, string lusasAttributeName)
        {
            Engine.Reflection.Compute.RecordError("ExplicitSection not supported in Lusas_Toolkit");
            return null;
        }

        private IFGeometricLine CreateProfile(RectangleProfile bhomProfile, string lusasAttributeName)
        {
            IFGeometricLine lusasGeometricLine = d_LusasData.createGeometricLine(lusasAttributeName);
            lusasGeometricLine.setValue("elementType", "3D Thick Beam");

            List<double> dimensionList = new List<double> { bhomProfile.Width, bhomProfile.Height };
            double[] dimensionArray = dimensionList.ToArray();

            List<string> valueList = new List<string> { "B", "D" };
            string[] valueArray = valueList.ToArray();

            int lusasType = 1;
            CreateLibrarySection(lusasGeometricLine, dimensionArray, valueArray, lusasType);

            return lusasGeometricLine;
        }

        private IFGeometricLine CreateProfile(BoxProfile bhomProfile, string lusasAttributeName)
        {
            IFGeometricLine lusasGeometricLine = d_LusasData.createGeometricLine(lusasAttributeName);
            lusasGeometricLine.setValue("elementType", "3D Thick Beam");

            List<double> dimensionList = new List<double> { bhomProfile.Width, bhomProfile.Height, bhomProfile.Thickness,
            bhomProfile.InnerRadius};
            double[] dimensionArray = dimensionList.ToArray();

            List<string> valueList = new List<string> { "B", "D", "t", "r" };
            string[] valueArray = valueList.ToArray();

            int lusasType = 2;
            CreateLibrarySection(lusasGeometricLine, dimensionArray, valueArray, lusasType);

            return lusasGeometricLine;
        }

        private IFGeometricLine CreateProfile(FabricatedBoxProfile bhomProfile, string lusasAttributeName)
        {
            IFGeometricLine lusasGeometricLine = d_LusasData.createGeometricLine(lusasAttributeName);
            lusasGeometricLine.setValue("elementType", "3D Thick Beam");

            Engine.Reflection.Compute.RecordWarning("Unequal flange thickness not supported in Lusas for FabricatedBoxProfile, top flange thickness used as flange thickness");
            Engine.Reflection.Compute.RecordWarning("Weld size assumed to be inner radius for FabricatedBoxProfile");

            List<double> dimensionList = new List<double> { bhomProfile.Width, bhomProfile.Height, bhomProfile.TopFlangeThickness,
            bhomProfile.WebThickness, bhomProfile.WeldSize};
            double[] dimensionArray = dimensionList.ToArray();

            List<string> valueList = new List<string> { "B", "D", "tf", "tw", "r"};
            string[] valueArray = valueList.ToArray();

            int lusasType = 15;
            CreateLibrarySection(lusasGeometricLine, dimensionArray, valueArray, lusasType);

            return lusasGeometricLine;
        }

        private IFGeometricLine CreateProfile(CircleProfile bhomProfile, string lusasAttributeName)
        {
            IFGeometricLine lusasGeometricLine = d_LusasData.createGeometricLine(lusasAttributeName);
            lusasGeometricLine.setValue("elementType", "3D Thick Beam");

            List<double> dimensionList = new List<double> { bhomProfile.Diameter };
            double[] dimensionArray = dimensionList.ToArray();

            List<string> valueList = new List<string> { "D" };
            string[] valueArray = valueList.ToArray();

            int lusasType = 3;
            CreateLibrarySection(lusasGeometricLine, dimensionArray, valueArray, lusasType);

            return lusasGeometricLine;
        }

        private IFGeometricLine CreateProfile(TubeProfile bhomProfile, string lusasAttributeName)
        {
            IFGeometricLine lusasGeometricLine = d_LusasData.createGeometricLine(lusasAttributeName);
            lusasGeometricLine.setValue("elementType", "3D Thick Beam");

            List<double> dimensionList = new List<double> { bhomProfile.Diameter, bhomProfile.Thickness };
            double[] dimensionArray = dimensionList.ToArray();

            List<string> valueList = new List<string> { "D", "t" };
            string[] valueArray = valueList.ToArray();

            int lusasType = 4;
            CreateLibrarySection(lusasGeometricLine, dimensionArray, valueArray, lusasType);

            return lusasGeometricLine;
        }

        private IFGeometricLine CreateProfile(ISectionProfile bhomProfile, string lusasAttributeName)
        {
            IFGeometricLine lusasGeometricLine = d_LusasData.createGeometricLine(lusasAttributeName);
            lusasGeometricLine.setValue("elementType", "3D Thick Beam");

            List<double> dimensionList = new List<double> { bhomProfile.Height, bhomProfile.Width,
            bhomProfile.WebThickness, bhomProfile.FlangeThickness, bhomProfile.RootRadius};
            double[] dimensionArray = dimensionList.ToArray();

            List<string> valueList = new List<string> { "D", "B", "tw", "tf", "r" };
            string[] valueArray = valueList.ToArray();

            int lusasType = 5;
            CreateLibrarySection(lusasGeometricLine, dimensionArray, valueArray, lusasType);

            return lusasGeometricLine;
        }

        private IFGeometricLine CreateProfile(TSectionProfile bhomProfile, string lusasAttributeName)
        {
            IFGeometricLine lusasGeometricLine = d_LusasData.createGeometricLine(lusasAttributeName);
            lusasGeometricLine.setValue("elementType", "3D Thick Beam");

            List<double> dimensionList = new List<double> { bhomProfile.Height, bhomProfile.Width,
            bhomProfile.FlangeThickness, bhomProfile.WebThickness, bhomProfile.RootRadius};
            double[] dimensionArray = dimensionList.ToArray();

            List<string> valueList = new List<string> { "D", "B", "tf", "tw", "r" };
            string[] valueArray = valueList.ToArray();

            int lusasType = 8;
            CreateLibrarySection(lusasGeometricLine, dimensionArray, valueArray, lusasType);

            return lusasGeometricLine;
        }

        private IFGeometricLine CreateProfile(FabricatedISectionProfile bhomProfile, string lusasAttributeName)
        {
            IFGeometricLine lusasGeometricLine = d_LusasData.createGeometricLine(lusasAttributeName);
            lusasGeometricLine.setValue("elementType", "3D Thick Beam");

            List<double> dimensionList = new List<double> { bhomProfile.Height, bhomProfile.TopFlangeWidth,
                bhomProfile.BotFlangeWidth, bhomProfile.TopFlangeWidth, bhomProfile.TopFlangeThickness,
            bhomProfile.WebThickness, bhomProfile.WeldSize};
            double[] dimensionArray = dimensionList.ToArray();

            List<string> valueList = new List<string> { "D", "Bt", "Bb", "tft", "bft", "tw", "r" };
            string[] valueArray = valueList.ToArray();

            int lusasType = 14;
            CreateLibrarySection(lusasGeometricLine, dimensionArray, valueArray, lusasType);

            Engine.Reflection.Compute.RecordWarning("Weld size assumed to be root radius for FabricatedISection");

            return lusasGeometricLine;
        }

        private IFGeometricLine CreateProfile(AngleProfile bhomProfile, string lusasAttributeName)
        {
            IFGeometricLine lusasGeometricLine = d_LusasData.createGeometricLine(lusasAttributeName);
            lusasGeometricLine.setValue("elementType", "3D Thick Beam");

            List<double> dimensionList = new List<double> { bhomProfile.Height, bhomProfile.Width, bhomProfile.FlangeThickness,
            bhomProfile.WebThickness, bhomProfile.RootRadius, bhomProfile.ToeRadius};
            double[] dimensionArray = dimensionList.ToArray();

            List<string> valueList = new List<string> { "D", "B","tf", "tw", "r1", "r2" };
            string[] valueArray = valueList.ToArray();

            int lusasType = 7;
            CreateLibrarySection(lusasGeometricLine, dimensionArray, valueArray, lusasType);

            return lusasGeometricLine;
        }

        private IFGeometricLine CreateProfile(ChannelProfile bhomProfile, string lusasAttributeName)
        {
            IFGeometricLine lusasGeometricLine = d_LusasData.createGeometricLine(lusasAttributeName);
            lusasGeometricLine.setValue("elementType", "3D Thick Beam");

            Engine.Reflection.Compute.RecordWarning("Toe radius not support for ChannelSection");
            List<double> dimensionList = new List<double> { bhomProfile.Height, bhomProfile.FlangeWidth,
            bhomProfile.FlangeThickness, bhomProfile.WebThickness, bhomProfile.RootRadius};
            double[] dimensionArray = dimensionList.ToArray();

            List<string> valueList = new List<string> { "D", "B", "tf", "tw", "r" };
            string[] valueArray = valueList.ToArray();

            int lusasType = 10;
            CreateLibrarySection(lusasGeometricLine, dimensionArray, valueArray, lusasType);

            return lusasGeometricLine;
        }

        private IFGeometricLine CreateProfile(ZSectionProfile bhomProfile, string lusasAttributeName)
        {
            IFGeometricLine lusasGeometricLine = d_LusasData.createGeometricLine(lusasAttributeName);
            lusasGeometricLine.setValue("elementType", "3D Thick Beam");

            Engine.Reflection.Compute.RecordWarning("Lusas only supports constant thickness Z sections, flange thickness used as thickness");
            Engine.Reflection.Compute.RecordWarning("Toe radius not supported for ZSection");
            List<double> dimensionList = new List<double> { bhomProfile.Height, bhomProfile.FlangeWidth, bhomProfile.FlangeWidth,
            bhomProfile.FlangeThickness, bhomProfile.RootRadius};
            double[] dimensionArray = dimensionList.ToArray();

            List<string> valueList = new List<string> { "D", "E", "F", "t", "r"};
            string[] valueArray = valueList.ToArray();

            int lusasType = 16;
            CreateLibrarySection(lusasGeometricLine, dimensionArray, valueArray, lusasType);

            return lusasGeometricLine;
        }

        private IFGeometricLine CreateProfile(GeneralisedFabricatedBoxProfile bhomProfile, string lusasAttributeName)
        {
            Engine.Reflection.Compute.RecordError("GeneralisedFabricatedBoxProfile not supported in Lusas_Toolkit");
            return null;
        }

        private IFGeometricLine CreateProfile(FreeFormProfile bhomProfile, string lusasAttributeName)
        {
            Engine.Reflection.Compute.RecordError("FreeFormProfile not supported in Lusas_Toolkit");
            return null;
        }

        private IFGeometricLine CreateProfile(KiteProfile bhomProfile, string lusasAttributeName)
        {
            Engine.Reflection.Compute.RecordError("KiteProfile not supported in Lusas_Toolkit");
            return null;
        }


        private void CreateLibrarySection(IFGeometricLine lusasGeometricLine, double[] dimensionArray, string[] valueArray, int lusasType)
        {
            string sectionName = lusasGeometricLine.getName();
            m_LusasApplication.addLibrarySection(true, sectionName, lusasType, dimensionArray, true);
            lusasGeometricLine.setFromLibrary("User Sections", "Local", sectionName, 0, 0);
        }
    }
}

