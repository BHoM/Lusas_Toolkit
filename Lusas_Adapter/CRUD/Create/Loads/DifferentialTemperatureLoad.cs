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
using BH.oM.Structure.Loads;
using Lusas.LPI;

namespace BH.Adapter.Lusas
{
#if Debug18 || Release18
    public partial class LusasV18Adapter
#elif Debug19 || Release19
    public partial class LusasV19Adapter
#elif Debug191 || Release191
    public partial class LusasV191Adapter
#elif Debug200 || Release200
    public partial class LusasV200Adapter
#elif Debug210 || Release210
    public partial class LusasV210Adapter
#elif Debug211 || Release211
    public partial class LusasV211Adapter
#elif Debug220 || Release220
    public partial class LusasV220Adapter
#elif Debug230 || Release230
    public partial class LusasV230Adapter
#elif Debug240 || Release240
    public partial class LusasV240Adapter
#else
    public partial class LusasV17Adapter
#endif
    {
        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        private IFTemperatureProfileLoad CreateBarDifferentialTemperatureLoad(
            BarDifferentialTemperatureLoad temperatureLoad, object[] lusasLines)
        {
            IFLoadcase assignedLoadcase = (IFLoadcase)d_LusasData.getLoadset(
                temperatureLoad.Loadcase.AdapterId<int>(typeof(LusasId)));

            string loadDirection = temperatureLoad.LoadDirection == DifferentialTemperatureLoadDirection.LocalY
                ? "local y"
                : "local z";

            IFTemperatureProfileLoad lusasProfileLoad = CreateProfileTemperatureLoad(
                temperatureLoad.Name, temperatureLoad.TemperatureProfile, loadDirection, lusasLines, assignedLoadcase);
            if (lusasProfileLoad != null)
                temperatureLoad.SetAdapterId(typeof(LusasId), lusasProfileLoad.getID());

            return lusasProfileLoad;
        }

        /***************************************************/

        private IFTemperatureProfileLoad CreateAreaDifferentialTemperatureLoad(
            AreaDifferentialTemperatureLoad temperatureLoad, object[] lusasSurfaces)
        {
            IFLoadcase assignedLoadcase = (IFLoadcase)d_LusasData.getLoadset(
                temperatureLoad.Loadcase.AdapterId<int>(typeof(LusasId)));

            IFTemperatureProfileLoad lusasProfileLoad = CreateProfileTemperatureLoad(
                temperatureLoad.Name, temperatureLoad.TemperatureProfile, "local z", lusasSurfaces, assignedLoadcase);

            if (lusasProfileLoad != null)
                temperatureLoad.SetAdapterId(typeof(LusasId), lusasProfileLoad.getID());

            return lusasProfileLoad;
        }

        /***************************************************/

        private IFTemperatureProfileLoad CreateProfileTemperatureLoad(string name,
            Dictionary<double, double> temperatureProfile, string loadDirection,
            object[] lusasGeometry, IFLoadcase assignedLoadcase)
        {
            if (temperatureProfile.Count > 2)
            {
                Engine.Base.Compute.RecordError(
                    $"The TemperatureProfile for '{name}' contains {temperatureProfile.Count} positions. " +
                    "Multi-layer differential temperature profiles are not currently supported in the Lusas_Toolkit " +
                    "because BHoM uses normalised (parametric) positions whereas Lusas requires absolute thicknesses. " +
                    "Only profiles with exactly two positions (bottom = 0 and top = 1) can be pushed.");
                return null;
            }

            IFTemperatureProfileLoad lusasProfileLoad;

            if (d_LusasData.existsAttribute("Loading", name))
            {
                lusasProfileLoad = (IFTemperatureProfileLoad)d_LusasData.getAttributes("Loading", name);
            }
            else
            {
                List<KeyValuePair<double, double>> sortedProfile = temperatureProfile
                    .OrderBy(kv => kv.Key)
                    .ToList();

                lusasProfileLoad = d_LusasData.createLoadingTemperatureProfile(name);
                lusasProfileLoad.setTopTemperature(sortedProfile.Last().Value);
                lusasProfileLoad.setBottomTemperature(sortedProfile.First().Value);
                lusasProfileLoad.setForceType("Axial and Flexural");
                lusasProfileLoad.setLoadDirection(loadDirection);
                lusasProfileLoad.setAnalysisCategory("3D");
            }

            IFAssignment lusasAssignment = m_LusasApplication.assignment();
            lusasAssignment.setLoadset(assignedLoadcase);
            lusasProfileLoad.assignTo(lusasGeometry, lusasAssignment);

            return lusasProfileLoad;
        }

        /***************************************************/

    }
}
