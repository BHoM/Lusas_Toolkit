/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2022, the respective contributors. All rights reserved.
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
using BH.oM.Adapters.Lusas;
using BH.oM.Structure.SectionProperties;
using BH.oM.Spatial.ShapeProfiles;
using Lusas.LPI;
using BH.Engine.Spatial;
using BH.Engine.Structure;
using BH.Engine.Adapter;

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
                IFGeometricLine lusasGeometricLine = CreateSection(sectionProperty as dynamic);
                lusasAttribute = lusasGeometricLine;
            }

            if (lusasAttribute != null)
            {
                int adapterIdName = lusasAttribute.getID();
                sectionProperty.SetAdapterId(typeof(LusasId), adapterIdName);

                return lusasAttribute;
            }

            return null;
        }

        /***************************************************/

        private IFAttribute CreateSection(SteelSection sectionProperty)
        {
            IFGeometricLine lusasGeometricLine = null;
            if (sectionProperty.SectionProfile != null)
            {
                lusasGeometricLine = d_LusasData.createGeometricLine(sectionProperty.DescriptionOrName());
                lusasGeometricLine.setValue("elementType", "3D Thick Beam");
                if (CreateProfile(sectionProperty.DescriptionOrName(), sectionProperty.SectionProfile as dynamic))
                    if (!(sectionProperty.SectionProfile is TaperedProfile))
                        lusasGeometricLine.setFromLibrary("User Sections", "Local", sectionProperty.DescriptionOrName(), 0, 0);
            }


            return lusasGeometricLine;
        }

        /***************************************************/

        private IFAttribute CreateSection(ConcreteSection sectionProperty)
        {
            IFGeometricLine lusasGeometricLine = null;
            if (sectionProperty.SectionProfile != null)
            {
                lusasGeometricLine = d_LusasData.createGeometricLine(sectionProperty.DescriptionOrName());
                lusasGeometricLine.setValue("elementType", "3D Thick Beam");
                if (CreateProfile(sectionProperty.DescriptionOrName(), sectionProperty.SectionProfile as dynamic))
                    if (!(sectionProperty.SectionProfile is TaperedProfile))
                        lusasGeometricLine.setFromLibrary("User Sections", "Local", sectionProperty.DescriptionOrName(), 0, 0);
            }
            return lusasGeometricLine;
        }

        /***************************************************/

        private IFAttribute CreateSection(ExplicitSection sectionProperty)
        {
            Engine.Base.Compute.RecordError("ExplicitSection is not supported in Lusas_Toolkit.");
            return null;
        }

        /***************************************************/

        private IFAttribute CreateSection(TimberSection sectionProperty)
        {
            IFGeometricLine lusasGeometricLine = null;
            if (sectionProperty.SectionProfile != null)
            {
                lusasGeometricLine = d_LusasData.createGeometricLine(sectionProperty.DescriptionOrName());
                lusasGeometricLine.setValue("elementType", "3D Thick Beam");
                if (CreateProfile(sectionProperty.DescriptionOrName(), sectionProperty.SectionProfile as dynamic))
                    if (!(sectionProperty.SectionProfile is TaperedProfile))
                        lusasGeometricLine.setFromLibrary("User Sections", "Local", sectionProperty.DescriptionOrName(), 0, 0);
            }
            return lusasGeometricLine;
        }

        /***************************************************/

        private IFAttribute CreateSection(AluminiumSection sectionProperty)
        {
            IFGeometricLine lusasGeometricLine = null;
            if (sectionProperty.SectionProfile != null)
            {
                lusasGeometricLine = d_LusasData.createGeometricLine(sectionProperty.DescriptionOrName());
                lusasGeometricLine.setValue("elementType", "3D Thick Beam");
                if (CreateProfile(sectionProperty.DescriptionOrName(), sectionProperty.SectionProfile as dynamic))
                    if (!(sectionProperty.SectionProfile is TaperedProfile))
                        lusasGeometricLine.setFromLibrary("User Sections", "Local", sectionProperty.DescriptionOrName(), 0, 0);
            }
            return lusasGeometricLine;
        }

        /***************************************************/

        private IFAttribute CreateSection(GenericSection sectionProperty)
        {
            IFGeometricLine lusasGeometricLine = null;
            if (sectionProperty.SectionProfile != null)
            {
                lusasGeometricLine = d_LusasData.createGeometricLine(sectionProperty.DescriptionOrName());
                lusasGeometricLine.setValue("elementType", "3D Thick Beam");
                if (CreateProfile(sectionProperty.DescriptionOrName(), sectionProperty.SectionProfile as dynamic))
                    if (!(sectionProperty.SectionProfile is TaperedProfile))
                        lusasGeometricLine.setFromLibrary("User Sections", "Local", sectionProperty.DescriptionOrName(), 0, 0);
            }
            return lusasGeometricLine;
        }

        /***************************************************/

        private bool CreateProfile(string name, RectangleProfile profile)
        {
            List<double> dimensionList = new List<double> { profile.Width, profile.Height };
            double[] dimensionArray = dimensionList.ToArray();

            //List<string> valueList = new List<string> { "B", "D" };

            int lusasType = 1;
            CreateLibrarySection(name, dimensionArray, lusasType);

            return true;
        }

        /***************************************************/

        private bool CreateProfile(string name, BoxProfile profile)
        {
            List<double> dimensionList = new List<double> { profile.Width, profile.Height, profile.Thickness, profile.InnerRadius };
            double[] dimensionArray = dimensionList.ToArray();

            //List<string> valueList = new List<string> { "B", "D", "t", "r" };

            int lusasType = 2;
            CreateLibrarySection(name, dimensionArray, lusasType);

            return true;
        }

        /***************************************************/

        private bool CreateProfile(string name, FabricatedBoxProfile profile)
        {
            Engine.Base.Compute.RecordWarning(
                "Unequal flange thickness not supported in Lusas for " + profile.GetType().ToString()
                + ", top flange thickness used as flange thickness");
            Engine.Base.Compute.RecordWarning(
                "Weld size assumed to be inner radius for " + profile.GetType().ToString());

            List<double> dimensionList = new List<double> {
                profile.Width, profile.Height, profile.TopFlangeThickness,profile.WebThickness, profile.WeldSize};
            double[] dimensionArray = dimensionList.ToArray();

            //List<string> valueList = new List<string> { "B", "D", "tf", "tw", "r" };

            int lusasType = 15;
            CreateLibrarySection(name, dimensionArray, lusasType);

            return true;
        }

        /***************************************************/

        private bool CreateProfile(string name, CircleProfile profile)
        {
            List<double> dimensionList = new List<double> { profile.Diameter };
            double[] dimensionArray = dimensionList.ToArray();

            //List<string> valueList = new List<string> { "D" };

            int lusasType = 3;
            CreateLibrarySection(name, dimensionArray, lusasType);

            return true;
        }

        /***************************************************/

        private bool CreateProfile(string name, TubeProfile profile)
        {
            List<double> dimensionList = new List<double> { profile.Diameter, profile.Thickness };
            double[] dimensionArray = dimensionList.ToArray();

            //List<string> valueList = new List<string> { "D", "t" };

            int lusasType = 4;
            CreateLibrarySection(name, dimensionArray, lusasType);

            return true;
        }

        /***************************************************/

        private bool CreateProfile(string name, ISectionProfile profile)
        {
            List<double> dimensionList = new List<double> { profile.Width, profile.Height,
            profile.FlangeThickness, profile.WebThickness, profile.RootRadius};
            double[] dimensionArray = dimensionList.ToArray();

            //List<string> valueList = new List<string> { "B", "D", "tf", "tw", "r" };

            int lusasType = 5;
            CreateLibrarySection(name, dimensionArray, lusasType);

            return true;
        }

        /***************************************************/

        private bool CreateProfile(string name, TSectionProfile profile)
        {
            List<double> dimensionList = new List<double> { profile.Height, profile.Width,
            profile.FlangeThickness, profile.WebThickness, profile.RootRadius};
            double[] dimensionArray = dimensionList.ToArray();

            //List<string> valueList = new List<string> { "D", "B", "tf", "tw", "r" };

            int lusasType = 8;
            CreateLibrarySection(name, dimensionArray, lusasType);

            return true;
        }

        /***************************************************/

        private bool CreateProfile(string name, FabricatedISectionProfile profile)
        {
            List<double> dimensionList = new List<double> { profile.TopFlangeWidth,
                profile.BotFlangeWidth, profile.Height, profile.TopFlangeThickness, profile.BotFlangeThickness,
            profile.WebThickness, profile.WeldSize};
            double[] dimensionArray = dimensionList.ToArray();

            //List<string> valueList = new List<string> { "Bt", "Bb", "D", "tft", "bft", "tw", "r" };

            int lusasType = 14;
            CreateLibrarySection(name, dimensionArray, lusasType);

            Engine.Base.Compute.RecordWarning("Weld size assumed to be root radius for " + profile.GetType().ToString());

            return true;
        }

        /***************************************************/

        private bool CreateProfile(string name, AngleProfile profile)
        {
            List<double> dimensionList = new List<double> {profile.Height, profile.Width, profile.WebThickness,
                profile.FlangeThickness, profile.RootRadius, profile.ToeRadius};

            double[] dimensionArray = dimensionList.ToArray();

            //List<string> valueList = new List<string> { "D", "B", "tw", "tf", "r1", "r2" };

            int lusasType = 7;
            CreateLibrarySection(name, dimensionArray, lusasType);

            return true;
        }

        /***************************************************/

        private bool CreateProfile(string name, ChannelProfile profile)
        {
            Engine.Base.Compute.RecordWarning("Toe radius not support for ChannelSection");
            List<double> dimensionList = new List<double> { profile.FlangeWidth, profile.Height,
            profile.FlangeThickness, profile.WebThickness, profile.RootRadius};
            double[] dimensionArray = dimensionList.ToArray();

            //List<string> valueList = new List<string> { "B", "D", "tf", "tw", "r" };

            int lusasType = 10;
            CreateLibrarySection(name, dimensionArray, lusasType);

            return true;
        }


        /***************************************************/

        private bool CreateProfile(string name, ZSectionProfile profile)
        {
            Engine.Base.Compute.RecordWarning("Lusas only supports constant thickness Z sections, flange thickness used as thickness");
            Engine.Base.Compute.RecordWarning("Toe radius not supported for ZSection");

            List<double> dimensionList = new List<double> {
                profile.Height, profile.FlangeWidth, profile.FlangeWidth,
            profile.FlangeThickness, profile.RootRadius};
            double[] dimensionArray = dimensionList.ToArray();

            //List<string> valueList = new List<string> { "D", "E", "F", "t", "r" };

            int lusasType = 16;
            CreateLibrarySection(name, dimensionArray, lusasType);

            return true;
        }

        /***************************************************/

        private bool CreateProfile(string name, GeneralisedFabricatedBoxProfile profile)
        {
            Engine.Base.Compute.RecordError("GeneralisedFabricatedBoxProfile not supported in Lusas_Toolkit");
            return false;
        }

        /***************************************************/

        private bool CreateProfile(string name, GeneralisedTSectionProfile profile)
        {
            Engine.Base.Compute.RecordError("GeneralisedTSectionProfile not supported in Lusas_Toolkit");
            return false;
        }

        /***************************************************/

        private bool CreateProfile(string name, FreeFormProfile profile)
        {
            Engine.Base.Compute.RecordError("FreeFormProfile not supported in Lusas_Toolkit");
            return false;
        }

        /***************************************************/

        private bool CreateProfile(string name, KiteProfile profile)
        {
            Engine.Base.Compute.RecordError("KiteProfile not supported in Lusas_Toolkit");
            return false;
        }

        /***************************************************/

        private bool CreateProfile(string name, TaperedProfile profile)
        {
            profile.MapPositionDomain();

            IFGeometricLine lusasGeometricLine = (IFGeometricLine)d_LusasData.getAttribute("Line Geometric", name);
            lusasGeometricLine.setMultipleVarying(true);
            lusasGeometricLine.setNumberOfSections(profile.Profiles.Count);
            lusasGeometricLine.setValue("interpMethod", "Enhanced");
            lusasGeometricLine.setSpecifyInterp(true);
            lusasGeometricLine.setEqualSpacing(false);
            lusasGeometricLine.setSymmetry(false);
            lusasGeometricLine.setDistanceType("Parametric");

            List<double> keys = new List<double>(profile.Profiles.Keys);
            IProfile iProfile;
            for (int i = 0; i < keys.Count; i++)
            {
                profile.Profiles.TryGetValue(keys[i], out iProfile);
                string profileName;
                if (i == 0)
                    profileName = $"{name}-0";
                else
                    profileName = $"{name}-{keys[i]:G3}";

                CreateProfile(profileName, iProfile as dynamic);
                lusasGeometricLine.setFromLibrary("User Sections", "Local", profileName, 0, 0, i);
                if (i == 0)
                {
                    lusasGeometricLine.setInterpolation("Constant", (double)keys[i], i);
                }
                else
                {
                    lusasGeometricLine.setInterpolation("Function", (double)keys[i], i, profile.InterpolationOrder[i - 1]);
                }

            }

            lusasGeometricLine.setVerticalAlignment("CenterToCenter");
            lusasGeometricLine.setHorizontalAlignment("CenterToCenter");
            lusasGeometricLine.setAlignmentSection(1);

            return true;
        }

        /***************************************************/

        private void CreateLibrarySection(string name, double[] dimensionArray, int lusasType)
        {
            m_LusasApplication.addLibrarySection(true, name, lusasType, dimensionArray, true);
        }

        /***************************************************/

    }
}




