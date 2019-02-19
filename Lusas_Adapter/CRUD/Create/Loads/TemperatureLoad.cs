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

using BH.oM.Structure.Loads;
using Lusas.LPI;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        public IFLoadingTemperature CreateBarTemperatureLoad(BarTemperatureLoad temperatureLoad, object[] lusasLines)
        {
            if (!Engine.Lusas.Query.CheckIllegalCharacters(temperatureLoad.Name))
            {
                return null;
            }

            IFLoadcase assignedLoadcase = (IFLoadcase)d_LusasData.getLoadset(
                "Lc" + temperatureLoad.Loadcase.CustomData[AdapterId] + "/" + temperatureLoad.Loadcase.Name);

            string lusasName = "Tl" + temperatureLoad.CustomData[AdapterId] + "/" + temperatureLoad.Name;
            NameSearch("Tl", temperatureLoad.CustomData[AdapterId].ToString(), 
                temperatureLoad.Name, ref lusasName);

            IFLoadingTemperature lusasTemperatureLoad = CreateTemperatureLoad(lusasName,
                temperatureLoad.TemperatureChange, lusasLines, assignedLoadcase);

            return lusasTemperatureLoad;
        }

        public IFLoadingTemperature CreateAreaTemperatureLoad(AreaTemperatureLoad temperatureLoad, 
            object[] lusasSurfaces)
        {
            if (!Engine.Lusas.Query.CheckIllegalCharacters(temperatureLoad.Name))
            {
                return null;
            }

            IFLoadcase assignedLoadcase = (IFLoadcase)d_LusasData.getLoadset(
                "Lc" + temperatureLoad.Loadcase.CustomData[AdapterId] + "/" + temperatureLoad.Loadcase.Name);

            string lusasName = "Tl" + temperatureLoad.CustomData[AdapterId] + "/" + temperatureLoad.Name;

            IFLoadingTemperature lusasTemperatureLoad = CreateTemperatureLoad(lusasName,
                temperatureLoad.TemperatureChange, lusasSurfaces, assignedLoadcase);

            return lusasTemperatureLoad;
        }

        public IFLoadingTemperature CreateTemperatureLoad(string lusasName, 
            double temperatureChange, object[] lusasGeometry, IFLoadcase assignedLoadcase)
        {

            IFLoadingTemperature lusasTemperatureLoad = null;
            if (d_LusasData.existsAttribute("Loading", lusasName))
            {
                lusasTemperatureLoad = (IFLoadingTemperature)d_LusasData.getAttributes(
                    "Loading", lusasName);
            }
            else
            {
                lusasTemperatureLoad = d_LusasData.createLoadingTemperature(lusasName);
                lusasTemperatureLoad.setValue("T0", 0);
                lusasTemperatureLoad.setValue("T", temperatureChange);
            }

            IFAssignment lusasAssignment = m_LusasApplication.assignment();
            lusasAssignment.setLoadset(assignedLoadcase);
            lusasTemperatureLoad.assignTo(lusasGeometry, lusasAssignment);

            return lusasTemperatureLoad;
        }
    }
}
