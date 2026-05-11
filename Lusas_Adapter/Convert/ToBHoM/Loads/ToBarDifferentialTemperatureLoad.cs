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

using System.Collections.Generic;
using System.Linq;
using BH.Engine.Adapter;
using BH.oM.Adapters.Lusas;
using BH.oM.Structure.Elements;
using BH.oM.Structure.Loads;
using Lusas.LPI;

namespace BH.Adapter.Adapters.Lusas
{
    public static partial class Convert
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static BarDifferentialTemperatureLoad ToBarDifferentialTemperatureLoad(
            IFLoading lusasProfileLoad,
            IEnumerable<IFAssignment> lusasAssignments,
            Dictionary<string, Bar> bars)
        {
            IFTemperatureProfileLoad profileLoad = (IFTemperatureProfileLoad)lusasProfileLoad;

            IFLoadcase assignedLoadcase = (IFLoadcase)lusasAssignments.First().getAssignmentLoadset();
            Loadcase loadcase = ToLoadcase(assignedLoadcase);

            Dictionary<double, double> temperatureProfile = ExtractTemperatureProfile(profileLoad);

            string loadDirectionString = (string)profileLoad.getValue("LoadDir");
            DifferentialTemperatureLoadDirection loadDirection = loadDirectionString == "local y"
                ? DifferentialTemperatureLoadDirection.LocalY
                : DifferentialTemperatureLoadDirection.LocalZ;

            IEnumerable<Bar> assignedBars = GetLineAssignments(lusasAssignments, bars);

            BarDifferentialTemperatureLoad barDifferentialTemperatureLoad =
                Engine.Structure.Create.BarDifferentialTemperatureLoad(
                    loadcase,
                    temperatureProfile.Keys.ToList(),
                    temperatureProfile.Values.ToList(),
                    loadDirection,
                    assignedBars,
                    GetName(lusasProfileLoad));

            long adapterNameId = lusasProfileLoad.getID();
            barDifferentialTemperatureLoad.SetAdapterId(typeof(LusasId), adapterNameId);

            return barDifferentialTemperatureLoad;
        }

        /***************************************************/

    }
}
