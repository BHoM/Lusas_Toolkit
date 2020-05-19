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

        private IFLoadingBeamPoint CreateBarPointLoad(BarPointLoad bhomBarPointLoad, object[] lusasLines)
        {
            if (!Engine.Adapters.Lusas.Query.CheckIllegalCharacters(bhomBarPointLoad.Name))
            {
                return null;
            }

            IFLoadingBeamPoint lusasBarPointLoad = null;
            IFLoadcase assignedLoadcase = (IFLoadcase)d_LusasData.getLoadset(
                "Lc" + bhomBarPointLoad.Loadcase.CustomData[AdapterIdName] + "/" + bhomBarPointLoad.Loadcase.Name);

            string lusasName =
                "BPl" + bhomBarPointLoad.CustomData[AdapterIdName] + "/" + bhomBarPointLoad.Name;

            NameSearch("BPl", bhomBarPointLoad.CustomData[AdapterIdName].ToString(),
                bhomBarPointLoad.Name, ref lusasName);

            if (d_LusasData.existsAttribute("Loading", lusasName))
            {
                lusasBarPointLoad = (IFLoadingBeamPoint)d_LusasData.getAttribute("Loading", lusasName);
            }
            else
            {
                lusasBarPointLoad = d_LusasData.createLoadingBeamPoint(bhomBarPointLoad.Name);
                if (bhomBarPointLoad.Axis.ToString() == "Global")
                    lusasBarPointLoad.setBeamPoint("parametric", "global", "beam");
                else
                    lusasBarPointLoad.setBeamPoint("parametric", "local", "beam");
                lusasBarPointLoad.addRow(
                    bhomBarPointLoad.DistanceFromA,
                    bhomBarPointLoad.Force.X, bhomBarPointLoad.Force.Y, bhomBarPointLoad.Force.Z,
                    bhomBarPointLoad.Moment.X, bhomBarPointLoad.Moment.Y, bhomBarPointLoad.Moment.Z);
            }

            IFAssignment lusasAssignment = m_LusasApplication.assignment();
            lusasAssignment.setLoadset(assignedLoadcase);
            lusasBarPointLoad.assignTo(lusasLines, lusasAssignment);

            return lusasBarPointLoad;
        }

        /***************************************************/

    }
}
