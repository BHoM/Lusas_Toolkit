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

using BH.oM.Geometry;
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

        private IFLoadingGlobalDistributed CreateGlobalDistributedLine(BarUniformlyDistributedLoad distributedLoad, object[] lusasLines)
        {
            if (!Engine.Adapters.Lusas.Query.CheckIllegalCharacters(distributedLoad.Name))
            {
                return null;
            }

            IFLoadcase assignedLoadcase = (IFLoadcase)d_LusasData.getLoadset("Lc" + distributedLoad.Loadcase.CustomData[AdapterIdName] + "/" + distributedLoad.Loadcase.Name);
            string lusasName = "Dl" + distributedLoad.CustomData[AdapterIdName] + "/" + distributedLoad.Name;
            NameSearch("Dl", distributedLoad.CustomData[AdapterIdName].ToString(), distributedLoad.Name, ref lusasName);

            IFLoadingGlobalDistributed lusasGlobalDistributed = CreateGlobalDistributed(lusasName,
                "Length", assignedLoadcase, distributedLoad.Force, distributedLoad.Moment, lusasLines);

            return lusasGlobalDistributed;
        }

        /***************************************************/

        private IFLoadingGlobalDistributed CreateGlobalDistributedLoadSurface(AreaUniformlyDistributedLoad distributedLoad, object[] lusasSurfaces)
        {
            if (!Engine.Adapters.Lusas.Query.CheckIllegalCharacters(distributedLoad.Name))
            {
                return null;
            }

            IFLoadcase assignedLoadcase = (IFLoadcase)d_LusasData.getLoadset(
                "Lc" + distributedLoad.Loadcase.CustomData[AdapterIdName] + "/" + distributedLoad.Loadcase.Name);

            string lusasName = "Dl" + distributedLoad.CustomData[AdapterIdName] + "/" + distributedLoad.Name;

            IFLoadingGlobalDistributed lusasGlobalDistributed = CreateGlobalDistributed(lusasName,
                "Area", assignedLoadcase, distributedLoad.Pressure, null, lusasSurfaces);

            return lusasGlobalDistributed;
        }

        /***************************************************/

        private IFLoadingLocalDistributed CreateLocalDistributedLine(BarUniformlyDistributedLoad distributedLoad, object[] lusasLines)
        {
            if (!Engine.Adapters.Lusas.Query.CheckIllegalCharacters(distributedLoad.Name))
            {
                return null;
            }

            IFLoadcase assignedLoadcase = (IFLoadcase)d_LusasData.getLoadset(
                "Lc" + distributedLoad.Loadcase.CustomData[AdapterIdName] + "/" + distributedLoad.Loadcase.Name);

            string lusasName = "Dl" + distributedLoad.CustomData[AdapterIdName] + "/" + distributedLoad.Name;

            IFLoadingLocalDistributed lusasLocalDistributed = CreateLocalDistributed(lusasName,
                "Line", assignedLoadcase, distributedLoad.Force, lusasLines);

            return lusasLocalDistributed;
        }

        /***************************************************/

        private IFLoadingLocalDistributed CreateLocalDistributedSurface(AreaUniformlyDistributedLoad distributedLoad, object[] lusasSurfaces)
        {
            if (!Engine.Adapters.Lusas.Query.CheckIllegalCharacters(distributedLoad.Name))
            {
                return null;
            }

            IFLoadcase assignedLoadcase = (IFLoadcase)d_LusasData.getLoadset(
                "Lc" + distributedLoad.Loadcase.CustomData[AdapterIdName] + "/" + distributedLoad.Loadcase.Name);

            string lusasName = "Dl" + distributedLoad.CustomData[AdapterIdName] + "/" + distributedLoad.Name;

            IFLoadingLocalDistributed lusasLocalDistributed = CreateLocalDistributed(lusasName,
                "Area", assignedLoadcase, distributedLoad.Pressure, lusasSurfaces);

            return lusasLocalDistributed;
        }

        /***************************************************/

        private IFLoadingGlobalDistributed CreateGlobalDistributed(string lusasName,
            string type, IFLoadcase assignedLoadcase, Vector force, Vector moment, object[] lusasGeometry)
        {

            IFLoadingGlobalDistributed lusasGlobalDistributed = null;

            if (d_LusasData.existsAttribute("Loading", lusasName))
            {
                lusasGlobalDistributed = (IFLoadingGlobalDistributed)d_LusasData.getAttribute("Loading",
                    lusasName);
            }
            else
            {
                lusasGlobalDistributed = d_LusasData.createLoadingGlobalDistributed(lusasName);
                if (type == "Length")
                {
                    lusasGlobalDistributed.setGlobalDistributed(type,
                        force.X, force.Y, force.Z, moment.X, moment.Y, moment.Z);
                }
                else if (type == "Area")
                {
                    lusasGlobalDistributed.setGlobalDistributed(type,
                        force.X, force.Y, force.Z);
                }
            }

            IFAssignment lusasAssignment = m_LusasApplication.assignment();
            lusasAssignment.setLoadset(assignedLoadcase);
            lusasGlobalDistributed.assignTo(lusasGeometry, lusasAssignment);

            return lusasGlobalDistributed;
        }

        /***************************************************/

        private IFLoadingLocalDistributed CreateLocalDistributed(string lusasName,
            string type, IFLoadcase assignedLoadcase, Vector force, object[] lusasGeometry)
        {

            IFLoadingLocalDistributed lusasLocalDistributed = null;

            if (d_LusasData.existsAttribute("Loading", lusasName))
            {
                lusasLocalDistributed = (IFLoadingLocalDistributed)d_LusasData.getAttribute("Loading",
                    lusasName);
            }
            else
            {
                lusasLocalDistributed = d_LusasData.createLoadingLocalDistributed(lusasName);
                lusasLocalDistributed.setLocalDistributed(force.X, force.Y, force.Z, type);
            }

            IFAssignment lusasAssignment = m_LusasApplication.assignment();
            lusasAssignment.setLoadset(assignedLoadcase);
            lusasLocalDistributed.assignTo(lusasGeometry, lusasAssignment);

            return lusasLocalDistributed;
        }

        /***************************************************/

    }
}
