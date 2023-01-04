/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2023, the respective contributors. All rights reserved.
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

using BH.oM.Adapters.Lusas;
using BH.oM.Geometry;
using BH.Engine.Adapter;
using BH.oM.Structure.Loads;
using Lusas.LPI;

namespace BH.Adapter.Lusas
{
#if Debug18 || Release18
    public partial class LusasV18Adapter
#elif Debug19 || Release19
    public partial class LusasV19Adapter
#else
    public partial class LusasV17Adapter
#endif
    {
        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        private IFLoadingGlobalDistributed CreateGlobalDistributedLine(BarUniformlyDistributedLoad distributedLoad, object[] lusasLines)
        {
            IFLoadcase assignedLoadcase = (IFLoadcase)d_LusasData.getLoadset(distributedLoad.Loadcase.AdapterId<int>(typeof(LusasId)));
            IFLoadingGlobalDistributed lusasGlobalDistributed = CreateGlobalDistributed(distributedLoad.Name,
                "Length", assignedLoadcase, distributedLoad.Force, distributedLoad.Moment, lusasLines);

            int adapterIdName = lusasGlobalDistributed.getID();
            distributedLoad.SetAdapterId(typeof(LusasId), adapterIdName);

            return lusasGlobalDistributed;
        }

        /***************************************************/

        private IFLoadingGlobalDistributed CreateGlobalDistributedLoadSurface(AreaUniformlyDistributedLoad distributedLoad, object[] lusasSurfaces)
        {
            IFLoadcase assignedLoadcase = (IFLoadcase)d_LusasData.getLoadset(distributedLoad.Loadcase.AdapterId<int>(typeof(LusasId)));
            IFLoadingGlobalDistributed lusasGlobalDistributed = CreateGlobalDistributed(distributedLoad.Name,
                "Area", assignedLoadcase, distributedLoad.Pressure, null, lusasSurfaces);

            int adapterIdName = lusasGlobalDistributed.getID();
            distributedLoad.SetAdapterId(typeof(LusasId), adapterIdName);

            return lusasGlobalDistributed;
        }

        /***************************************************/

        private IFLoadingLocalDistributed CreateLocalDistributedLine(BarUniformlyDistributedLoad distributedLoad, object[] lusasLines)
        {
            IFLoadcase assignedLoadcase = (IFLoadcase)d_LusasData.getLoadset(distributedLoad.Loadcase.AdapterId<int>(typeof(LusasId)));
            IFLoadingLocalDistributed lusasLocalDistributed = CreateLocalDistributed(distributedLoad.Name,
                "Line", assignedLoadcase, distributedLoad.Force, lusasLines);

            int adapterIdName = lusasLocalDistributed.getID();
            distributedLoad.SetAdapterId(typeof(LusasId), adapterIdName);

            return lusasLocalDistributed;
        }

        /***************************************************/

        private IFLoadingLocalDistributed CreateLocalDistributedSurface(AreaUniformlyDistributedLoad distributedLoad, object[] lusasSurfaces)
        {
            IFLoadcase assignedLoadcase = (IFLoadcase)d_LusasData.getLoadset(distributedLoad.Loadcase.AdapterId<int>(typeof(LusasId)));

            IFLoadingLocalDistributed lusasLocalDistributed = CreateLocalDistributed(distributedLoad.Name,
                "Area", assignedLoadcase, distributedLoad.Pressure, lusasSurfaces);

            int adapterIdName = lusasLocalDistributed.getID();
            distributedLoad.SetAdapterId(typeof(LusasId), adapterIdName);

            return lusasLocalDistributed;
        }

        /***************************************************/

        private IFLoadingGlobalDistributed CreateGlobalDistributed(string lusasName,
            string type, IFLoadcase assignedLoadcase, Vector force, Vector moment, object[] lusasGeometry)
        {

            IFLoadingGlobalDistributed lusasGlobalDistributed;

            if (d_LusasData.existsAttribute("Loading", lusasName))
            {
                lusasGlobalDistributed = (IFLoadingGlobalDistributed)d_LusasData.getAttribute("Loading", lusasName);
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

            IFLoadingLocalDistributed lusasLocalDistributed;

            if (d_LusasData.existsAttribute("Loading", lusasName))
            {
                lusasLocalDistributed = (IFLoadingLocalDistributed)d_LusasData.getAttribute("Loading", lusasName);
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



