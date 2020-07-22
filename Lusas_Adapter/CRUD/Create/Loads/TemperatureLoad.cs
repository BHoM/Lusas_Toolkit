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

using BH.oM.Structure.Loads;
using Lusas.LPI;

namespace BH.Adapter.Lusas
{
#if Debug18 || Release18
    public partial class LusasV18Adapter
#else
    public partial class LusasV17Adapter
#endif
    {
        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        private IFLoadingTemperature CreateBarTemperatureLoad(BarTemperatureLoad temperatureLoad, object[] lusasLines)
        {
            IFLoadcase assignedLoadcase = (IFLoadcase)d_LusasData.getLoadset((int)temperatureLoad.Loadcase.CustomData[AdapterIdName]);

            IFLoadingTemperature lusasTemperatureLoad = CreateTemperatureLoad(
                temperatureLoad.Name,temperatureLoad.TemperatureChange, lusasLines, assignedLoadcase);

            temperatureLoad.CustomData[AdapterIdName] = lusasTemperatureLoad.getID().ToString();

            return lusasTemperatureLoad;
        }

        /***************************************************/

        private IFLoadingTemperature CreateAreaTemperatureLoad(AreaTemperatureLoad temperatureLoad,
            object[] lusasSurfaces)
        {
            IFLoadcase assignedLoadcase = (IFLoadcase)d_LusasData.getLoadset((int)temperatureLoad.Loadcase.CustomData[AdapterIdName]);

            IFLoadingTemperature lusasTemperatureLoad = CreateTemperatureLoad(temperatureLoad.Name,
                temperatureLoad.TemperatureChange, lusasSurfaces, assignedLoadcase);

            temperatureLoad.CustomData[AdapterIdName] = lusasTemperatureLoad.getID();

            return lusasTemperatureLoad;
        }

        private IFLoadingTemperature CreateTemperatureLoad(string name,
            double temperatureChange, object[] lusasGeometry, IFLoadcase assignedLoadcase)
        {
            IFLoadingTemperature lusasTemperatureLoad;
            if (d_LusasData.existsAttribute("Loading", name))
            {
                lusasTemperatureLoad = (IFLoadingTemperature)d_LusasData.getAttributes("Loading", name);
            }
            else
            {
                lusasTemperatureLoad = d_LusasData.createLoadingTemperature(name);
                lusasTemperatureLoad.setValue("T0", 0);
                lusasTemperatureLoad.setValue("T", temperatureChange);
            }

            IFAssignment lusasAssignment = m_LusasApplication.assignment();
            lusasAssignment.setLoadset(assignedLoadcase);
            lusasTemperatureLoad.assignTo(lusasGeometry, lusasAssignment);

            return lusasTemperatureLoad;
        }

        /***************************************************/

    }
}

