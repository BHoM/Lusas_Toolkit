/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2026, the respective contributors. All rights reserved.
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
using System.Collections.Generic;
using Lusas.LPI;

namespace BH.Adapter.Adapters.Lusas
{
    public static partial class Convert
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        // NOTE: The field names "TTop", "TBot", "thickness", and "temperature" are the Lusas LPI
        // internal parameter names for IFTemperatureProfileLoad. These should be verified against
        // the Lusas LPI documentation or confirmed via getValue inspection during integration testing.
        public static Dictionary<double, double> ExtractTemperatureProfile(IFTemperatureProfileLoad profileLoad)
        {
            double topTemperature = (double)profileLoad.getValue("TTop");
            long rowCount = profileLoad.countRows("thickness");

            if (rowCount == 0)
            {
                // Simple linear profile defined by top and bottom boundary temperatures only.
                double bottomTemperature = (double)profileLoad.getValue("TBot");
                return new Dictionary<double, double>
                {
                    { 0.0, bottomTemperature },
                    { 1.0, topTemperature }
                };
            }

            // Multi-layer profile defined entirely by upper rows. Reconstruct normalised positions
            // by subtracting each row's fractional thickness from the top (position 1) downward.
            Dictionary<double, double> profile = new Dictionary<double, double>
            {
                { 1.0, topTemperature }
            };

            double cumulativePosition = 1.0;
#if Debug220 || Release220 || Debug230 || Release230
                for (long i = 0; i < rowCount; i++)

#else
            for (int i = 0; i < rowCount; i++)
#endif
            {
                double thickness = (double)profileLoad.getRowValue("thickness", i, Type.Missing);
                double temperature = (double)profileLoad.getRowValue("temperature", i, Type.Missing);
                cumulativePosition -= thickness;
                profile[Math.Round(cumulativePosition, 10)] = temperature;
            }

            return profile;
        }

        /***************************************************/

    }
}
