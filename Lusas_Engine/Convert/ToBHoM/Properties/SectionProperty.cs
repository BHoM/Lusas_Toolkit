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

using System;
using BH.oM.Structure.Properties.Section;
using BH.oM.Structure.Properties.Section.ShapeProfiles;
using Lusas.LPI;

namespace BH.Engine.Lusas
{
    public static partial class Convert
    {
        public static ISectionProperty ToBHoMSection(this IFAttribute lusasAttribute)
        {
            string attributeName = GetName(lusasAttribute);

            IProfile sectionProfile = CreateProfile(lusasAttribute);

            double area = lusasAttribute.getValue("A");
            double rgy = lusasAttribute.getValue("ky");
            double rgz = lusasAttribute.getValue("kz");
            double j = lusasAttribute.getValue("J");
            double iz = lusasAttribute.getValue("Iyy");
            double iy = lusasAttribute.getValue("Izz");
            double iw = lusasAttribute.getValue("Cw");
            double wely = Math.Min(Math.Abs(lusasAttribute.getValue("Syt")), Math.Abs(lusasAttribute.getValue("Syb")));
            double welz = Math.Min(Math.Abs(lusasAttribute.getValue("Szt")), Math.Abs(lusasAttribute.getValue("Szb")));
            double wply = lusasAttribute.getValue("Zpy");
            double wplz = lusasAttribute.getValue("Zpz");
            double centreZ = lusasAttribute.getValue("zo");
            double centreY = lusasAttribute.getValue("yo");
            double zt = lusasAttribute.getValue("zt");
            double zb = Math.Abs(lusasAttribute.getValue("zb"));
            double yt = Math.Abs(lusasAttribute.getValue("yb"));
            double yb = lusasAttribute.getValue("yt");
            double asy = lusasAttribute.getValue("Asy");
            double asz = lusasAttribute.getValue("Asz");

            SteelSection bhomSection = new SteelSection(
                sectionProfile, area, rgy, rgz, j, iy, iz, iw,
                wely, welz, wply, wplz, centreZ, centreY, zt, zb, yt, yb, asy, asz);

            bhomSection.Name = attributeName;

            int adapterID = GetAdapterID(lusasAttribute, 'G');

            bhomSection.CustomData["Lusas_id"] = adapterID;

            return bhomSection;
        }

        public static IProfile CreateProfile(IFAttribute lusasAttribute)
        {
            int sectionType = lusasAttribute.getValue("Type");

            IProfile sectionProfile = null;

            if (sectionType == 1)
            {
                sectionProfile = BH.Engine.Structure.Create.RectangleProfile(
                    lusasAttribute.getValue("D"),
                    lusasAttribute.getValue("B"),
                    0
                );
            }
            else if (sectionType == 2)
            {
                sectionProfile = BH.Engine.Structure.Create.BoxProfile(
                    lusasAttribute.getValue("D"),
                    lusasAttribute.getValue("B"),
                    lusasAttribute.getValue("t"),
                    lusasAttribute.getValue("r"),
                    lusasAttribute.getValue("r")
                );
            }
            else if (sectionType == 3)
            {
                sectionProfile = BH.Engine.Structure.Create.CircleProfile(
                    lusasAttribute.getValue("D")
                );
            }
            else if (sectionType == 4)
            {
                sectionProfile = BH.Engine.Structure.Create.TubeProfile(
                    lusasAttribute.getValue("D"),
                    lusasAttribute.getValue("t")
                );
            }
            else if (sectionType == 5)
            {
                sectionProfile = BH.Engine.Structure.Create.ISectionProfile(
                    lusasAttribute.getValue("D"),
                    lusasAttribute.getValue("B"),
                    lusasAttribute.getValue("tw"),
                    lusasAttribute.getValue("tf"),
                    lusasAttribute.getValue("r"),
                    lusasAttribute.getValue("r")
                );
            }
            else if (sectionType == 7)
            {
                sectionProfile = BH.Engine.Structure.Create.AngleProfile(
                    lusasAttribute.getValue("D"),
                    lusasAttribute.getValue("B"),
                    lusasAttribute.getValue("tw"),
                    lusasAttribute.getValue("tf"),
                    lusasAttribute.getValue("r1"),
                    lusasAttribute.getValue("r2")
                );
            }
            else if (sectionType == 8)
            {
                sectionProfile = BH.Engine.Structure.Create.TSectionProfile(
                    lusasAttribute.getValue("D"),
                    lusasAttribute.getValue("B"),
                    lusasAttribute.getValue("tw"),
                    lusasAttribute.getValue("tf"),
                    lusasAttribute.getValue("r"),
                    lusasAttribute.getValue("r")
                );
            }
            else if (sectionType == 10)
            {
                sectionProfile = BH.Engine.Structure.Create.ChannelProfile(
                    lusasAttribute.getValue("D"),
                    lusasAttribute.getValue("B"),
                    lusasAttribute.getValue("tw"),
                    lusasAttribute.getValue("tf"),
                    lusasAttribute.getValue("r"),
                    lusasAttribute.getValue("r")
                );
            }
            else if (sectionType == 14)
            {
                sectionProfile = BH.Engine.Structure.Create.FabricatedISectionProfile(
                    lusasAttribute.getValue("D"),
                    lusasAttribute.getValue("Bt"),
                    lusasAttribute.getValue("Bb"),
                    lusasAttribute.getValue("tw"),
                    lusasAttribute.getValue("tft"),
                    lusasAttribute.getValue("tfb"),
                    lusasAttribute.getValue("r")
                );
            }
            else if (sectionType == 15)
            {
                sectionProfile = BH.Engine.Structure.Create.FabricatedBoxProfile(
                    lusasAttribute.getValue("D"),
                    lusasAttribute.getValue("B"),
                    lusasAttribute.getValue("tw"),
                    lusasAttribute.getValue("tf"),
                    lusasAttribute.getValue("tf"),
                    0
                );
            }
            else if (sectionType == 16)
            {
                sectionProfile = BH.Engine.Structure.Create.ZSectionProfile(
                    lusasAttribute.getValue("D"),
                    lusasAttribute.getValue("B"),
                    lusasAttribute.getValue("tw"),
                    lusasAttribute.getValue("tf"),
                    lusasAttribute.getValue("r"),
                    lusasAttribute.getValue("r")
                );
            }
            return sectionProfile;
        }
    }
}