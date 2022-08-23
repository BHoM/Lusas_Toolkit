/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2023, the respective contributors. All rights reserved.
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
using BH.oM.Structure.SectionProperties;
using BH.oM.Spatial.ShapeProfiles;
using Lusas.LPI;
using BH.oM.Adapters.Lusas;
using BH.Engine.Adapter;
using System.Collections.Generic;
using System.Linq;
using System.Dynamic;

namespace BH.Adapter.Adapters.Lusas
{
    public static partial class Convert
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static ISectionProperty ToSection(this IFAttribute lusasAttribute)
        {
            string attributeName = GetName(lusasAttribute);
            string attributeType = lusasAttribute.getAttributeType();

            int rows = (int)lusasAttribute.countRows("A");

            IProfile profile;
            GenericSection section = null;
            List<int> interpolationOrders = new List<int>();
            List<double> positions = new List<double>();
            List<IProfile> profiles = new List<IProfile>();

            for (int i = 0; i < rows; i++)
            {
                profile = ToProfile(lusasAttribute, i);
                double area = lusasAttribute.getValue("A", i);
                double rgy = lusasAttribute.getValue("ky", i);
                double rgz = lusasAttribute.getValue("kz", i);
                double j = lusasAttribute.getValue("J", i);
                double iy = lusasAttribute.getValue("Iyy", i);
                double iz = lusasAttribute.getValue("Izz", i);
                double iw = lusasAttribute.getValue("Cw", i);
                double wely = Math.Min(Math.Abs(lusasAttribute.getValue("Syt", i)), Math.Abs(lusasAttribute.getValue("Syb", i)));
                double welz = Math.Min(Math.Abs(lusasAttribute.getValue("Szt", i)), Math.Abs(lusasAttribute.getValue("Szb", i)));
                double wply = lusasAttribute.getValue("Zpy", i);
                double wplz = lusasAttribute.getValue("Zpz", i);
                double centreZ = 0; //Eccentricity is handeled in the Bar not at the section
                double centreY = 0; //Eccentricity is handeled in the Bar not at the section
                double zt = lusasAttribute.getValue("zt", i);
                double zb = Math.Abs(lusasAttribute.getValue("zb", i));
                double yt = Math.Abs(lusasAttribute.getValue("yb", i)); //Lusas Y-Axis is opposite to the BHoM Y-axis
                double yb = lusasAttribute.getValue("yt", i);
                double asy = lusasAttribute.getValue("Asy", i);
                double asz = lusasAttribute.getValue("Asz", i);

                if (profile == null)
                {
                    return null;
                }

                profile = Engine.Structure.Compute.Integrate(profile, oM.Geometry.Tolerance.MicroDistance).Item1;

                if (attributeType == "Multiple Varying Geometric")
                {
                    double position = lusasAttribute.getValue("distanceAlongBeam", i);
                    positions.Add(position);
                    profiles.Add(profile);
                }
                else
                {
                    section = new GenericSection(profile, area, rgy, rgz, j, iy, iz, iw, wely, welz, wply, wplz, centreZ, centreY, zt, zb, yt, yb, asy, asz);
                }

            }

            if (lusasAttribute.getAttributeType() == "Multiple Varying Geometric")
            {
                for (int i = 0; i < profiles.Count - 1; i++)
                    interpolationOrders.Add(1);
                TaperedProfile taperedProfile = Engine.Spatial.Create.TaperedProfile(positions, profiles, interpolationOrders);
                if (taperedProfile == null)
                    return null;
                section = Engine.Structure.Create.GenericSectionFromProfile(taperedProfile);
            }

            section.Name = attributeName;

            int adapterNameId = (int)lusasAttribute.getID();
            section.SetAdapterId(typeof(LusasId), adapterNameId);

            return section;
        }

        /***************************************************/

    }
}



