/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2020, the respective contributors. All rights reserved.
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
using BH.oM.Structure.SectionProperties;
using BH.oM.Geometry.ShapeProfiles;
using Lusas.LPI;
using BH.Engine.Structure;

namespace BH.Adapter.Lusas
{
#if Debug18 || Release18
    public partial class LusasV18Adapter
#elif Debug19 || Release19
    public partial class LusasV19Adapter
#else
    public partial class LusasV17Adapter
#endif
    {
        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        private IFAttribute CreateGeometricLine(ISectionProperty sectionProperty)
        {
            IFAttribute lusasAttribute;

            if (d_LusasData.existsAttribute("Line Geometric", sectionProperty.DescriptionOrName()))
            {
                lusasAttribute = d_LusasData.getAttribute("Line Geometric", sectionProperty.DescriptionOrName());
            }
            else
            {
                IFGeometricLine lusasGeometricLine = CreateSection(sectionProperty as dynamic, sectionProperty.DescriptionOrName());
                lusasAttribute = lusasGeometricLine;
            }

            sectionProperty.CustomData[AdapterIdName] = lusasAttribute.getID().ToString();

            return lusasAttribute;
        }

        /***************************************************/

        private IFAttribute CreateSection(SteelSection sectionProperty, string lusasName)
        {
            IFAttribute lusasGeometricLine = CreateProfile(
                sectionProperty.SectionProfile as dynamic, lusasName
                );
            return lusasGeometricLine;
        }

        /***************************************************/

        private IFAttribute CreateSection(ConcreteSection sectionProperty, string lusasName)
        {
            IFAttribute lusasGeometricLine = CreateProfile(
             sectionProperty.SectionProfile as dynamic, lusasName
             );
            return lusasGeometricLine;
        }

        /***************************************************/

        private IFAttribute CreateSection(ExplicitSection sectionProperty, string lusasName)
        {
            Engine.Reflection.Compute.RecordError("ExplicitSection not supported in Lusas_Toolkit");
            return null;
        }

        /***************************************************/

        private IFAttribute CreateSection(TimberSection sectionProperty, string lusasName)
        {
            IFAttribute lusasGeometricLine = CreateProfile(
                sectionProperty.SectionProfile as dynamic, lusasName
                );
            return lusasGeometricLine;
        }

        /***************************************************/

        private IFAttribute CreateSection(AluminiumSection sectionProperty, string lusasName)
        {
            IFAttribute lusasGeometricLine = CreateProfile(
                sectionProperty.SectionProfile as dynamic, lusasName
                );
            return lusasGeometricLine;
        }

        /***************************************************/

        private IFAttribute CreateSection(GenericSection sectionProperty, string lusasName)
        {
            IFAttribute lusasGeometricLine = CreateProfile(
                sectionProperty.SectionProfile as dynamic, lusasName
                );
            return lusasGeometricLine;
        }

        /***************************************************/

        private IFGeometricLine CreateProfile(RectangleProfile profile, string lusasName)
        {
            IFGeometricLine lusasGeometricLine = d_LusasData.createGeometricLine(lusasName);
            lusasGeometricLine.setValue("elementType", "3D Thick Beam");

            List<double> dimensionList = new List<double> { profile.Width, profile.Height };
            double[] dimensionArray = dimensionList.ToArray();

            List<string> valueList = new List<string> { "B", "D" };
            string[] valueArray = valueList.ToArray();

            int lusasType = 1;
            CreateLibrarySection(lusasGeometricLine, dimensionArray, valueArray, lusasType);

            return lusasGeometricLine;
        }

        /***************************************************/

        private IFGeometricLine CreateProfile(BoxProfile profile, string lusasName)
        {
            IFGeometricLine lusasGeometricLine = d_LusasData.createGeometricLine(lusasName);
            lusasGeometricLine.setValue("elementType", "3D Thick Beam");

            List<double> dimensionList = new List<double> {
                profile.Width, profile.Height, profile.Thickness,profile.InnerRadius
            };
            double[] dimensionArray = dimensionList.ToArray();

            List<string> valueList = new List<string> { "B", "D", "t", "r" };
            string[] valueArray = valueList.ToArray();

            int lusasType = 2;
            CreateLibrarySection(lusasGeometricLine, dimensionArray, valueArray, lusasType);

            return lusasGeometricLine;
        }

        /***************************************************/

        private IFGeometricLine CreateProfile(FabricatedBoxProfile profile, string lusasName)
        {
            IFGeometricLine lusasGeometricLine = d_LusasData.createGeometricLine(lusasName);
            lusasGeometricLine.setValue("elementType", "3D Thick Beam");

            Engine.Reflection.Compute.RecordWarning(
                "Unequal flange thickness not supported in Lusas for " + profile.GetType().ToString()
                + ", top flange thickness used as flange thickness");
            Engine.Reflection.Compute.RecordWarning(
                "Weld size assumed to be inner radius for " + profile.GetType().ToString());

            List<double> dimensionList = new List<double> {
                profile.Width, profile.Height, profile.TopFlangeThickness,
            profile.WebThickness, profile.WeldSize
            };
            double[] dimensionArray = dimensionList.ToArray();

            List<string> valueList = new List<string> { "B", "D", "tf", "tw", "r" };
            string[] valueArray = valueList.ToArray();

            int lusasType = 15;
            CreateLibrarySection(lusasGeometricLine, dimensionArray, valueArray, lusasType);

            return lusasGeometricLine;
        }

        /***************************************************/

        private IFGeometricLine CreateProfile(CircleProfile profile, string lusasName)
        {
            IFGeometricLine lusasGeometricLine = d_LusasData.createGeometricLine(lusasName);
            lusasGeometricLine.setValue("elementType", "3D Thick Beam");

            List<double> dimensionList = new List<double> { profile.Diameter };
            double[] dimensionArray = dimensionList.ToArray();

            List<string> valueList = new List<string> { "D" };
            string[] valueArray = valueList.ToArray();

            int lusasType = 3;
            CreateLibrarySection(lusasGeometricLine, dimensionArray, valueArray, lusasType);

            return lusasGeometricLine;
        }

        /***************************************************/

        private IFGeometricLine CreateProfile(TubeProfile profile, string lusasName)
        {
            IFGeometricLine lusasGeometricLine = d_LusasData.createGeometricLine(lusasName);
            lusasGeometricLine.setValue("elementType", "3D Thick Beam");

            List<double> dimensionList = new List<double> { profile.Diameter, profile.Thickness };
            double[] dimensionArray = dimensionList.ToArray();

            List<string> valueList = new List<string> { "D", "t" };
            string[] valueArray = valueList.ToArray();

            int lusasType = 4;
            CreateLibrarySection(lusasGeometricLine, dimensionArray, valueArray, lusasType);

            return lusasGeometricLine;
        }

        /***************************************************/

        private IFGeometricLine CreateProfile(ISectionProfile profile, string lusasName)
        {
            IFGeometricLine lusasGeometricLine = d_LusasData.createGeometricLine(lusasName);
            lusasGeometricLine.setValue("elementType", "3D Thick Beam");

            List<double> dimensionList = new List<double> { profile.Width, profile.Height,
            profile.FlangeThickness, profile.WebThickness, profile.RootRadius};
            double[] dimensionArray = dimensionList.ToArray();

            List<string> valueList = new List<string> { "B", "D", "tf", "tw", "r" };
            string[] valueArray = valueList.ToArray();

            int lusasType = 5;
            CreateLibrarySection(lusasGeometricLine, dimensionArray, valueArray, lusasType);

            return lusasGeometricLine;
        }

        /***************************************************/

        private IFGeometricLine CreateProfile(TSectionProfile profile, string lusasName)
        {
            IFGeometricLine lusasGeometricLine = d_LusasData.createGeometricLine(lusasName);
            lusasGeometricLine.setValue("elementType", "3D Thick Beam");

            List<double> dimensionList = new List<double> { profile.Width, profile.Height,
            profile.FlangeThickness, profile.WebThickness, profile.RootRadius};
            double[] dimensionArray = dimensionList.ToArray();

            List<string> valueList = new List<string> { "B", "D", "tf", "tw", "r" };
            string[] valueArray = valueList.ToArray();

            int lusasType = 8;
            CreateLibrarySection(lusasGeometricLine, dimensionArray, valueArray, lusasType);

            return lusasGeometricLine;
        }

        /***************************************************/

        private IFGeometricLine CreateProfile(FabricatedISectionProfile profile, string lusasName)
        {
            IFGeometricLine lusasGeometricLine = d_LusasData.createGeometricLine(lusasName);
            lusasGeometricLine.setValue("elementType", "3D Thick Beam");

            List<double> dimensionList = new List<double> { profile.TopFlangeWidth,
                profile.BotFlangeWidth, profile.Height, profile.TopFlangeThickness, profile.BotFlangeThickness,
            profile.WebThickness, profile.WeldSize};
            double[] dimensionArray = dimensionList.ToArray();

            List<string> valueList = new List<string> { "Bt", "Bb", "D", "tft", "bft", "tw", "r" };
            string[] valueArray = valueList.ToArray();

            int lusasType = 14;
            CreateLibrarySection(lusasGeometricLine, dimensionArray, valueArray, lusasType);

            Engine.Reflection.Compute.RecordWarning("Weld size assumed to be root radius for " +
                profile.GetType().ToString());

            return lusasGeometricLine;
        }

        /***************************************************/

        private IFGeometricLine CreateProfile(AngleProfile profile, string lusasName)
        {
            IFGeometricLine lusasGeometricLine = d_LusasData.createGeometricLine(lusasName);
            lusasGeometricLine.setValue("elementType", "3D Thick Beam");

            List<double> dimensionList = new List<double> {
                profile.Height, profile.Width, profile.WebThickness,
            profile.FlangeThickness, profile.RootRadius, profile.ToeRadius
            };
            double[] dimensionArray = dimensionList.ToArray();

            List<string> valueList = new List<string> { "D", "B", "tw", "tf", "r1", "r2" };
            string[] valueArray = valueList.ToArray();

            int lusasType = 7;
            CreateLibrarySection(lusasGeometricLine, dimensionArray, valueArray, lusasType);

            return lusasGeometricLine;
        }

        /***************************************************/

        private IFGeometricLine CreateProfile(ChannelProfile profile, string lusasName)
        {
            IFGeometricLine lusasGeometricLine = d_LusasData.createGeometricLine(lusasName);
            lusasGeometricLine.setValue("elementType", "3D Thick Beam");

            Engine.Reflection.Compute.RecordWarning("Toe radius not support for ChannelSection");
            List<double> dimensionList = new List<double> { profile.FlangeWidth, profile.Height,
            profile.FlangeThickness, profile.WebThickness, profile.RootRadius};
            double[] dimensionArray = dimensionList.ToArray();

            List<string> valueList = new List<string> { "B", "D", "tf", "tw", "r" };
            string[] valueArray = valueList.ToArray();

            int lusasType = 10;
            CreateLibrarySection(lusasGeometricLine, dimensionArray, valueArray, lusasType);

            return lusasGeometricLine;
        }

        /***************************************************/

        private IFGeometricLine CreateProfile(ZSectionProfile profile, string lusasName)
        {
            IFGeometricLine lusasGeometricLine = d_LusasData.createGeometricLine(lusasName);
            lusasGeometricLine.setValue("elementType", "3D Thick Beam");

            Engine.Reflection.Compute.RecordWarning(
                "Lusas only supports constant thickness Z sections, flange thickness used as thickness");
            Engine.Reflection.Compute.RecordWarning("Toe radius not supported for ZSection");
            List<double> dimensionList = new List<double> {
                profile.Height, profile.FlangeWidth, profile.FlangeWidth,
            profile.FlangeThickness, profile.RootRadius};
            double[] dimensionArray = dimensionList.ToArray();

            List<string> valueList = new List<string> { "D", "E", "F", "t", "r" };
            string[] valueArray = valueList.ToArray();

            int lusasType = 16;
            CreateLibrarySection(lusasGeometricLine, dimensionArray, valueArray, lusasType);

            return lusasGeometricLine;
        }

        /***************************************************/

        private IFGeometricLine CreateProfile(GeneralisedFabricatedBoxProfile profile,
            string lusasName)
        {
            Engine.Reflection.Compute.RecordError(
                "GeneralisedFabricatedBoxProfile not supported in Lusas_Toolkit"
                );
            return null;
        }

        /***************************************************/

        private IFGeometricLine CreateProfile(GeneralisedTSectionProfile profile,
           string lusasName)
        {
            Engine.Reflection.Compute.RecordError(
                "GeneralisedTSectionProfile not supported in Lusas_Toolkit"
                );
            return null;
        }

        /***************************************************/

        private IFGeometricLine CreateProfile(FreeFormProfile profile, string lusasName)
        {
            Engine.Reflection.Compute.RecordError(
                "FreeFormProfile not supported in Lusas_Toolkit"
                );
            return null;
        }

        /***************************************************/

        private IFGeometricLine CreateProfile(KiteProfile profile, string lusasName)
        {
            Engine.Reflection.Compute.RecordError("KiteProfile not supported in Lusas_Toolkit");
            return null;
        }

        /***************************************************/


        private void CreateLibrarySection(IFGeometricLine lusasGeometricLine,
            double[] dimensionArray, string[] valueArray, int lusasType)
        {
            string sectionName = lusasGeometricLine.getName();
            m_LusasApplication.addLibrarySection(true, sectionName, lusasType, dimensionArray, true);
            lusasGeometricLine.setFromLibrary("User Sections", "Local", sectionName, 0, 0);
        }

        /***************************************************/

    }
}


