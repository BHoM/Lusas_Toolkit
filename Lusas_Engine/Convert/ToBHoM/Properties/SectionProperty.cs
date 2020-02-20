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

using System;
using BH.oM.Structure.SectionProperties;
using BH.oM.Geometry.ShapeProfiles;
using Lusas.LPI;

namespace BH.Engine.Lusas
{
    public static partial class Convert
    {
      
        public static ISectionProperty ToSection(this IFAttribute lusasAttribute)
        {
            string attributeName = Lusas.Query.GetName(lusasAttribute);
         
            IProfile bhomProfile = Lusas.Convert.ToProfile(lusasAttribute);


                double area = lusasAttribute.getValue("A");
                double rgy = lusasAttribute.getValue("ky");
                double rgz = lusasAttribute.getValue("kz");
                double j = lusasAttribute.getValue("J");
                double iy = lusasAttribute.getValue("Iyy");
                double iz = lusasAttribute.getValue("Izz");
                double iw = lusasAttribute.getValue("Cw");
                double wely = Math.Min(Math.Abs(lusasAttribute.getValue("Syt")), Math.Abs(lusasAttribute.getValue("Syb")));
                double welz = Math.Min(Math.Abs(lusasAttribute.getValue("Szt")), Math.Abs(lusasAttribute.getValue("Szb")));
                double wply = lusasAttribute.getValue("Zpy");
                double wplz = lusasAttribute.getValue("Zpz");
                double centreZ = 0; //Eccentricity is handeled in the Bar not at the section
                double centreY = 0; //Eccentricity is handeled in the Bar not at the section
                double zt = lusasAttribute.getValue("zt");
                double zb = Math.Abs(lusasAttribute.getValue("zb")); 
                double yt = Math.Abs(lusasAttribute.getValue("yb")); //Lusas Y-Axis is opposite to the BHoM Y-axis
                double yb = lusasAttribute.getValue("yt");
                double asy = lusasAttribute.getValue("Asy");
                double asz = lusasAttribute.getValue("Asz");

                bhomProfile = Engine.Structure.Compute.Integrate(bhomProfile, oM.Geometry.Tolerance.MicroDistance).Item1;

                GenericSection bhomSection = new GenericSection(bhomProfile, area, rgy, rgz, j, iy, iz, iw,
                    wely, welz, wply, wplz, centreZ, centreY, zt, zb, yt, yb, asy, asz);

                bhomSection.Name = attributeName;

                int adapterID = Lusas.Query.GetAdapterID(lusasAttribute, 'G');

                bhomSection.CustomData["Lusas_id"] = adapterID;

                return bhomSection;

        }
    }
}
