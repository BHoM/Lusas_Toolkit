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

using BH.oM.Spatial.ShapeProfiles;
using Lusas.LPI;

namespace BH.Adapter.Adapters.Lusas
{
    public static partial class Convert
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static IProfile ToProfile(IFAttribute lusasAttribute, int i)
        {
            int sectionType = lusasAttribute.getValue("Type");

            IProfile profile = null;

            switch (sectionType)
            {
                case 1:
                    {
                        profile = BH.Engine.Spatial.Create.RectangleProfile(
                            lusasAttribute.getValue("D",i),
                            lusasAttribute.getValue("B",i),
                            0
                        );
                        break;
                    }
                case 2:
                    {
                        profile = BH.Engine.Spatial.Create.BoxProfile(
                            lusasAttribute.getValue("D",i),
                            lusasAttribute.getValue("B",i),
                            lusasAttribute.getValue("t",i),
                            lusasAttribute.getValue("r",i),
                            lusasAttribute.getValue("r",i)
                        );
                        break;
                    }
                case 3:
                    {
                        profile = BH.Engine.Spatial.Create.CircleProfile(
                            lusasAttribute.getValue("D",i)
                        );
                        break;
                    }
                case 4:
                    {
                        profile = BH.Engine.Spatial.Create.TubeProfile(
                            lusasAttribute.getValue("D",i),
                            lusasAttribute.getValue("t",i)
                        );
                        break;
                    }
                case 5:
                    {
                        profile = BH.Engine.Spatial.Create.ISectionProfile(
                            lusasAttribute.getValue("D",i),
                            lusasAttribute.getValue("B",i),
                            lusasAttribute.getValue("tw",i),
                            lusasAttribute.getValue("tf",i),
                            lusasAttribute.getValue("r",i),
                            lusasAttribute.getValue("r",i)
                        );
                        break;
                    }
                case 7:
                    {
                        profile = BH.Engine.Spatial.Create.AngleProfile(
                            lusasAttribute.getValue("D",i),
                            lusasAttribute.getValue("B",i),
                            lusasAttribute.getValue("tw",i),
                            lusasAttribute.getValue("tf",i),
                            lusasAttribute.getValue("r1",i),
                            lusasAttribute.getValue("r2",i)
                        );
                        break;
                    }
                case 8:
                    {
                        profile = BH.Engine.Spatial.Create.TSectionProfile(
                            lusasAttribute.getValue("D",i),
                            lusasAttribute.getValue("B",i),
                            lusasAttribute.getValue("tw",i),
                            lusasAttribute.getValue("tf",i),
                            lusasAttribute.getValue("r", i),
                            lusasAttribute.getValue("r", i)
                        );
                        break;
                    }
                case 10:
                    {
                        profile = BH.Engine.Spatial.Create.ChannelProfile(
                            lusasAttribute.getValue("D", i),
                            lusasAttribute.getValue("B", i),
                            lusasAttribute.getValue("tw", i),
                            lusasAttribute.getValue("tf", i),
                            lusasAttribute.getValue("r", i),
                            lusasAttribute.getValue("r", i)
                        );
                        break;
                    }
                case 14:
                    {
                        profile = BH.Engine.Spatial.Create.FabricatedISectionProfile(
                            lusasAttribute.getValue("D", i),
                            lusasAttribute.getValue("Bt", i),
                            lusasAttribute.getValue("Bb", i),
                            lusasAttribute.getValue("tw", i),
                            lusasAttribute.getValue("tft", i),
                            lusasAttribute.getValue("tfb", i),
                            lusasAttribute.getValue("r", i)
                        );
                        break;
                    }
                case 15:
                    {
                        profile = BH.Engine.Spatial.Create.FabricatedBoxProfile(
                            lusasAttribute.getValue("D", i),
                            lusasAttribute.getValue("B", i),
                            lusasAttribute.getValue("tw", i),
                            lusasAttribute.getValue("tf", i),
                            lusasAttribute.getValue("tf", i),
                            0
                        );
                        break;
                    }
                case 16:
                    {
                        profile = BH.Engine.Spatial.Create.ZSectionProfile(
                            lusasAttribute.getValue("D", i),
                            lusasAttribute.getValue("B", i),
                            lusasAttribute.getValue("tw", i),
                            lusasAttribute.getValue("tf", i),
                            lusasAttribute.getValue("r", i),
                            lusasAttribute.getValue("r", i)
                        );
                        break;
                    }
            }

            return profile;
        }

        /***************************************************/

    }
}



