/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2018, the respective contributors. All rights reserved.
 *
 * Each contributor holds copyright over their respective contributions.
 * The project versioning (Git) records all such contribution source information.
 *                                           
 *                                                                              
 * The BHoM is free software: you can redistribute it and/or modify         
 * it under the terms of the GNU Lesser General Public License as published by  
 * the Free Software Foundation, either version 3.0 of the License, or          
 * (at your option) any later version.                                          
 *                                                                              
 * The BHoM is distributed in the hope that it will be useful,              
 * but WITHOUT ANY WARRANTY; without even the implied warranty of               
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the                 
 * GNU Lesser General Public License for more details.                          
 *                                                                            
 * You should have received a copy of the GNU Lesser General Public License     
 * along with this code. If not, see <https://www.gnu.org/licenses/lgpl-3.0.html>.      
 */

using System.Collections.Generic;
using BH.oM.Structure.Properties.Section;
using BH.oM.Structure.Properties.Section.ShapeProfiles;
using Lusas.LPI;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        public IFAttribute CreateGeometricLine(ISectionProperty sectionProperty)
        {
            if (!Engine.Lusas.Query.CheckIllegalCharacters(sectionProperty.Name))
            {
                return null;
            }

            IFAttribute lusasAttribute = null;
            string lusasName = "G" + sectionProperty.CustomData[AdapterId] + "/" + sectionProperty.Name;

            if (d_LusasData.existsAttribute("Line Geometric", lusasName))
            {
                lusasAttribute = d_LusasData.getAttribute("Line Geometric", lusasName);
            }
            else
            {
                IFGeometricLine lusasGeometricLine = CreateSection(sectionProperty as dynamic, lusasName);
                lusasAttribute = lusasGeometricLine;
            }
            return lusasAttribute;
        }


        private IFAttribute CreateSection(SteelSection sectionProperty, string lusasName)
        {
            IFAttribute lusasGeometricLine = CreateProfile(
                sectionProperty.SectionProfile as dynamic, lusasName
                );
            return lusasGeometricLine;
        }

        private IFAttribute CreateSection(ConcreteSection sectionProperty, string lusasName)
        {
            Engine.Reflection.Compute.RecordError("ConcreteSection not supported in Lusas_Toolkit");
            return null;
        }

        private IFAttribute CreateSection(ExplicitSection sectionProperty, string lusasName)
        {
            Engine.Reflection.Compute.RecordError("ExplicitSection not supported in Lusas_Toolkit");
            return null;
        }

        private IFGeometricLine CreateProfile(RectangleProfile bhomProfile, string lusasName)
        {
            IFGeometricLine lusasGeometricLine = d_LusasData.createGeometricLine(lusasName);
            lusasGeometricLine.setValue("elementType", "3D Thick Beam");

            List<double> dimensionList = new List<double> { bhomProfile.Width, bhomProfile.Height };
            double[] dimensionArray = dimensionList.ToArray();

            List<string> valueList = new List<string> { "B", "D" };
            string[] valueArray = valueList.ToArray();

            int lusasType = 1;
            CreateLibrarySection(lusasGeometricLine, dimensionArray, valueArray, lusasType);

            return lusasGeometricLine;
        }

        private IFGeometricLine CreateProfile(BoxProfile bhomProfile, string lusasName)
        {
            IFGeometricLine lusasGeometricLine = d_LusasData.createGeometricLine(lusasName);
            lusasGeometricLine.setValue("elementType", "3D Thick Beam");

            List<double> dimensionList = new List<double> {
                bhomProfile.Width, bhomProfile.Height, bhomProfile.Thickness,bhomProfile.InnerRadius
            };
            double[] dimensionArray = dimensionList.ToArray();

            List<string> valueList = new List<string> { "B", "D", "t", "r" };
            string[] valueArray = valueList.ToArray();

            int lusasType = 2;
            CreateLibrarySection(lusasGeometricLine, dimensionArray, valueArray, lusasType);

            return lusasGeometricLine;
        }

        private IFGeometricLine CreateProfile(FabricatedBoxProfile bhomProfile, string lusasName)
        {
            IFGeometricLine lusasGeometricLine = d_LusasData.createGeometricLine(lusasName);
            lusasGeometricLine.setValue("elementType", "3D Thick Beam");

            Engine.Reflection.Compute.RecordWarning(
                "Unequal flange thickness not supported in Lusas for " + bhomProfile.GetType().ToString() 
                + ", top flange thickness used as flange thickness");
            Engine.Reflection.Compute.RecordWarning(
                "Weld size assumed to be inner radius for " + bhomProfile.GetType().ToString());

            List<double> dimensionList = new List<double> {
                bhomProfile.Width, bhomProfile.Height, bhomProfile.TopFlangeThickness,
            bhomProfile.WebThickness, bhomProfile.WeldSize
            };
            double[] dimensionArray = dimensionList.ToArray();

            List<string> valueList = new List<string> { "B", "D", "tf", "tw", "r" };
            string[] valueArray = valueList.ToArray();

            int lusasType = 15;
            CreateLibrarySection(lusasGeometricLine, dimensionArray, valueArray, lusasType);

            return lusasGeometricLine;
        }

        private IFGeometricLine CreateProfile(CircleProfile bhomProfile, string lusasName)
        {
            IFGeometricLine lusasGeometricLine = d_LusasData.createGeometricLine(lusasName);
            lusasGeometricLine.setValue("elementType", "3D Thick Beam");

            List<double> dimensionList = new List<double> { bhomProfile.Diameter };
            double[] dimensionArray = dimensionList.ToArray();

            List<string> valueList = new List<string> { "D" };
            string[] valueArray = valueList.ToArray();

            int lusasType = 3;
            CreateLibrarySection(lusasGeometricLine, dimensionArray, valueArray, lusasType);

            return lusasGeometricLine;
        }

        private IFGeometricLine CreateProfile(TubeProfile bhomProfile, string lusasName)
        {
            IFGeometricLine lusasGeometricLine = d_LusasData.createGeometricLine(lusasName);
            lusasGeometricLine.setValue("elementType", "3D Thick Beam");

            List<double> dimensionList = new List<double> { bhomProfile.Diameter, bhomProfile.Thickness };
            double[] dimensionArray = dimensionList.ToArray();

            List<string> valueList = new List<string> { "D", "t" };
            string[] valueArray = valueList.ToArray();

            int lusasType = 4;
            CreateLibrarySection(lusasGeometricLine, dimensionArray, valueArray, lusasType);

            return lusasGeometricLine;
        }

        private IFGeometricLine CreateProfile(ISectionProfile bhomProfile, string lusasName)
        {
            IFGeometricLine lusasGeometricLine = d_LusasData.createGeometricLine(lusasName);
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

        private IFGeometricLine CreateProfile(TSectionProfile bhomProfile, string lusasName)
        {
            IFGeometricLine lusasGeometricLine = d_LusasData.createGeometricLine(lusasName);
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

        private IFGeometricLine CreateProfile(FabricatedISectionProfile bhomProfile, string lusasName)
        {
            IFGeometricLine lusasGeometricLine = d_LusasData.createGeometricLine(lusasName);
            lusasGeometricLine.setValue("elementType", "3D Thick Beam");

            List<double> dimensionList = new List<double> { bhomProfile.Height, bhomProfile.TopFlangeWidth,
                bhomProfile.BotFlangeWidth, bhomProfile.TopFlangeThickness, bhomProfile.BotFlangeThickness,
            bhomProfile.WebThickness, bhomProfile.WeldSize};
            double[] dimensionArray = dimensionList.ToArray();

            List<string> valueList = new List<string> { "D", "Bt", "Bb", "tft", "bft", "tw", "r" };
            string[] valueArray = valueList.ToArray();

            int lusasType = 14;
            CreateLibrarySection(lusasGeometricLine, dimensionArray, valueArray, lusasType);

            Engine.Reflection.Compute.RecordWarning("Weld size assumed to be root radius for " +
                bhomProfile.GetType().ToString());

            return lusasGeometricLine;
        }

        private IFGeometricLine CreateProfile(AngleProfile bhomProfile, string lusasName)
        {
            IFGeometricLine lusasGeometricLine = d_LusasData.createGeometricLine(lusasName);
            lusasGeometricLine.setValue("elementType", "3D Thick Beam");

            List<double> dimensionList = new List<double> {
                bhomProfile.Height, bhomProfile.Width, bhomProfile.FlangeThickness,
            bhomProfile.WebThickness, bhomProfile.RootRadius, bhomProfile.ToeRadius
            };
            double[] dimensionArray = dimensionList.ToArray();

            List<string> valueList = new List<string> { "D", "B", "tf", "tw", "r1", "r2" };
            string[] valueArray = valueList.ToArray();

            int lusasType = 7;
            CreateLibrarySection(lusasGeometricLine, dimensionArray, valueArray, lusasType);

            return lusasGeometricLine;
        }

        private IFGeometricLine CreateProfile(ChannelProfile bhomProfile, string lusasName)
        {
            IFGeometricLine lusasGeometricLine = d_LusasData.createGeometricLine(lusasName);
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

        private IFGeometricLine CreateProfile(ZSectionProfile bhomProfile, string lusasName)
        {
            IFGeometricLine lusasGeometricLine = d_LusasData.createGeometricLine(lusasName);
            lusasGeometricLine.setValue("elementType", "3D Thick Beam");

            Engine.Reflection.Compute.RecordWarning(
                "Lusas only supports constant thickness Z sections, flange thickness used as thickness");
            Engine.Reflection.Compute.RecordWarning("Toe radius not supported for ZSection");
            List<double> dimensionList = new List<double> {
                bhomProfile.Height, bhomProfile.FlangeWidth, bhomProfile.FlangeWidth,
            bhomProfile.FlangeThickness, bhomProfile.RootRadius};
            double[] dimensionArray = dimensionList.ToArray();

            List<string> valueList = new List<string> { "D", "E", "F", "t", "r" };
            string[] valueArray = valueList.ToArray();

            int lusasType = 16;
            CreateLibrarySection(lusasGeometricLine, dimensionArray, valueArray, lusasType);

            return lusasGeometricLine;
        }

        private IFGeometricLine CreateProfile(GeneralisedFabricatedBoxProfile bhomProfile,
            string lusasName)
        {
            Engine.Reflection.Compute.RecordError(
                "GeneralisedFabricatedBoxProfile not supported in Lusas_Toolkit"
                );
            return null;
        }

        private IFGeometricLine CreateProfile(FreeFormProfile bhomProfile, string lusasName)
        {
            Engine.Reflection.Compute.RecordError(
                "FreeFormProfile not supported in Lusas_Toolkit"
                );
            return null;
        }

        private IFGeometricLine CreateProfile(KiteProfile bhomProfile, string lusasName)
        {
            Engine.Reflection.Compute.RecordError("KiteProfile not supported in Lusas_Toolkit");
            return null;
        }


        private void CreateLibrarySection(IFGeometricLine lusasGeometricLine,
            double[] dimensionArray, string[] valueArray, int lusasType)
        {
            string sectionName = lusasGeometricLine.getName();
            m_LusasApplication.addLibrarySection(true, sectionName, lusasType, dimensionArray, true);
            lusasGeometricLine.setFromLibrary("User Sections", "Local", sectionName, 0, 0);
        }
    }
}

