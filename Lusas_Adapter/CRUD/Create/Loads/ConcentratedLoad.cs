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
    public partial class LusasAdapter
    {
        private IFLoadingConcentrated CreateConcentratedLoad(PointLoad pointLoad, object[] lusasPoints)
        {
            if (!Engine.External.Lusas.Query.CheckIllegalCharacters(pointLoad.Name))
            {
                return null;
            }

            IFLoadingConcentrated lusasPointLoad = null;
            IFLoadcase assignedLoadcase = (IFLoadcase)d_LusasData.getLoadset(
                "Lc" + pointLoad.Loadcase.CustomData[AdapterIdName] + "/" + pointLoad.Loadcase.Name);

            string lusasName = "Pl" + pointLoad.CustomData[AdapterIdName] + "/" + pointLoad.Name;
            NameSearch("Pl", pointLoad.CustomData[AdapterIdName].ToString(), pointLoad.Name, ref lusasName);

            if (d_LusasData.existsAttribute("Loading", lusasName))
            {
                lusasPointLoad = (IFLoadingConcentrated)d_LusasData.getAttribute("Loading", lusasName);
            }
            else
            {
                lusasPointLoad = d_LusasData.createLoadingConcentrated(lusasName);
                lusasPointLoad.setConcentrated(
                    pointLoad.Force.X, pointLoad.Force.Y, pointLoad.Force.Z,
                    pointLoad.Moment.X, pointLoad.Moment.Y, pointLoad.Moment.Z);
            }

            IFAssignment lusasAssignment = m_LusasApplication.assignment();
            lusasAssignment.setLoadset(assignedLoadcase);
            lusasPointLoad.assignTo(lusasPoints, lusasAssignment);

            return lusasPointLoad;
        }
    }
}

