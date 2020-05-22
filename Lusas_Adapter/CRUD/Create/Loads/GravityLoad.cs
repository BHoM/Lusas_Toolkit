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

        private IFLoadingBody CreateGravityLoad(GravityLoad gravityLoad, IFGeometry[] lusasGeometry)
        {
            if (!Engine.Adapters.Lusas.Query.CheckIllegalCharacters(gravityLoad.Name))
            {
                return null;
            }

            IFLoadingBody lusasGravityLoad = null;
            IFLoadcase assignedLoadcase = (IFLoadcase)d_LusasData.getLoadset(
                "Lc" + gravityLoad.Loadcase.CustomData[AdapterIdName] + "/" + gravityLoad.Loadcase.Name);

            string lusasName = "Gl" + gravityLoad.CustomData[AdapterIdName] + "/" + gravityLoad.Name;
            NameSearch("Gl", gravityLoad.CustomData[AdapterIdName].ToString(), gravityLoad.Name, ref lusasName);

            if (d_LusasData.existsAttribute("Loading", lusasName))
            {
                lusasGravityLoad = (IFLoadingBody)d_LusasData.getAttributes("Loading", lusasName);
            }
            else
            {
                lusasGravityLoad = d_LusasData.createLoadingBody(lusasName);
                lusasGravityLoad.setBody(
                    gravityLoad.GravityDirection.X, gravityLoad.GravityDirection.Y, gravityLoad.GravityDirection.Z);
            }

            IFAssignment lusasAssignment = m_LusasApplication.assignment();
            lusasAssignment.setLoadset(assignedLoadcase);
            lusasGravityLoad.assignTo(lusasGeometry, lusasAssignment);

            return lusasGravityLoad;
        }

        /***************************************************/

    }
}

