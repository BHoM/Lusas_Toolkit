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

using BH.oM.Geometry.ShapeProfiles;
using Lusas.LPI;

namespace BH.Adapter.Adapters.Lusas
{
    public static partial class Convert
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static IProfile ToProfile(IFAttribute lusasAttribute)
        {
            int sectionType = lusasAttribute.getValue("Type");

            IProfile sectionProfile = null;

            switch (sectionType)
            {
                case 1:
                    {
                        sectionProfile = BH.Engine.Geometry.Create.RectangleProfile(
                            lusasAttribute.getValue("D"),
                            lusasAttribute.getValue("B"),
                            0
                        );
                        break;
                    }
                case 2:
                    {
                        sectionProfile = BH.Engine.Geometry.Create.BoxProfile(
                            lusasAttribute.getValue("D"),
                            lusasAttribute.getValue("B"),
                            lusasAttribute.getValue("t"),
                            lusasAttribute.getValue("r"),
                            lusasAttribute.getValue("r")
                        );
                        break;
                    }
                case 3:
                    {
                        sectionProfile = BH.Engine.Geometry.Create.CircleProfile(
                            lusasAttribute.getValue("D")
                        );
                        break;
                    }
                case 4:
                    {
                        sectionProfile = BH.Engine.Geometry.Create.TubeProfile(
                            lusasAttribute.getValue("D"),
                            lusasAttribute.getValue("t")
                        );
                        break;
                    }
                case 5:
                    {
                        sectionProfile = BH.Engine.Geometry.Create.ISectionProfile(
                            lusasAttribute.getValue("D"),
                            lusasAttribute.getValue("B"),
                            lusasAttribute.getValue("tw"),
                            lusasAttribute.getValue("tf"),
                            lusasAttribute.getValue("r"),
                            lusasAttribute.getValue("r")
                        );
                        break;
                    }
                case 7:
                    {
                        sectionProfile = BH.Engine.Geometry.Create.AngleProfile(
                            lusasAttribute.getValue("D"),
                            lusasAttribute.getValue("B"),
                            lusasAttribute.getValue("tw"),
                            lusasAttribute.getValue("tf"),
                            lusasAttribute.getValue("r1"),
                            lusasAttribute.getValue("r2")
                        );
                        break;
                    }
                case 8:
                    {
                        sectionProfile = BH.Engine.Geometry.Create.TSectionProfile(
                            lusasAttribute.getValue("D"),
                            lusasAttribute.getValue("B"),
                            lusasAttribute.getValue("tw"),
                            lusasAttribute.getValue("tf"),
                            lusasAttribute.getValue("r"),
                            lusasAttribute.getValue("r")
                        );
                        break;
                    }
                case 10:
                    {
                        sectionProfile = BH.Engine.Geometry.Create.ChannelProfile(
                            lusasAttribute.getValue("D"),
                            lusasAttribute.getValue("B"),
                            lusasAttribute.getValue("tw"),
                            lusasAttribute.getValue("tf"),
                            lusasAttribute.getValue("r"),
                            lusasAttribute.getValue("r")
                        );
                        break;
                    }
                case 14:
                    {
                        sectionProfile = BH.Engine.Geometry.Create.FabricatedISectionProfile(
                            lusasAttribute.getValue("D"),
                            lusasAttribute.getValue("Bt"),
                            lusasAttribute.getValue("Bb"),
                            lusasAttribute.getValue("tw"),
                            lusasAttribute.getValue("tft"),
                            lusasAttribute.getValue("tfb"),
                            lusasAttribute.getValue("r")
                        );
                        break;
                    }
                case 15:
                    {
                        sectionProfile = BH.Engine.Geometry.Create.FabricatedBoxProfile(
                            lusasAttribute.getValue("D"),
                            lusasAttribute.getValue("B"),
                            lusasAttribute.getValue("tw"),
                            lusasAttribute.getValue("tf"),
                            lusasAttribute.getValue("tf"),
                            0
                        );
                        break;
                    }
                case 16:
                    {
                        sectionProfile = BH.Engine.Geometry.Create.ZSectionProfile(
                            lusasAttribute.getValue("D"),
                            lusasAttribute.getValue("B"),
                            lusasAttribute.getValue("tw"),
                            lusasAttribute.getValue("tf"),
                            lusasAttribute.getValue("r"),
                            lusasAttribute.getValue("r")
                        );
                        break;
                    }
            }

            return sectionProfile;
        }

        /***************************************************/

    }
}

