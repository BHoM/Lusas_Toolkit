/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2024, the respective contributors. All rights reserved.
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
using BH.oM.Structure.Loads;
using BH.Engine.Adapter;
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
#else
    public partial class LusasV17Adapter
#endif
    {
        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        private IFLoadingBeamPoint CreateBarPointLoad(BarPointLoad barPointLoad, object[] lusasLines)
        {
            IFLoadingBeamPoint lusasBarPointLoad;
            IFLoadcase assignedLoadcase = (IFLoadcase)d_LusasData.getLoadset(barPointLoad.Loadcase.AdapterId<int>(typeof(LusasId)));

            if (d_LusasData.existsAttribute("Loading", barPointLoad.Name))
            {
                lusasBarPointLoad = (IFLoadingBeamPoint)d_LusasData.getAttribute("Loading", barPointLoad.Name);
            }
            else
            {
                lusasBarPointLoad = d_LusasData.createLoadingBeamPoint(barPointLoad.Name);
                if (barPointLoad.Axis.ToString() == "Global")
                    lusasBarPointLoad.setBeamPoint("parametric", "global", "beam");
                else
                    lusasBarPointLoad.setBeamPoint("parametric", "local", "beam");
                lusasBarPointLoad.addRow(
                    barPointLoad.DistanceFromA,
                    barPointLoad.Force.X, barPointLoad.Force.Y, barPointLoad.Force.Z,
                    barPointLoad.Moment.X, barPointLoad.Moment.Y, barPointLoad.Moment.Z);
            }

            IFAssignment lusasAssignment = m_LusasApplication.assignment();
            lusasAssignment.setLoadset(assignedLoadcase);
            lusasBarPointLoad.assignTo(lusasLines, lusasAssignment);

            long adapterIdName = lusasBarPointLoad.getID();
            barPointLoad.SetAdapterId(typeof(LusasId), adapterIdName);

            return lusasBarPointLoad;
        }

        /***************************************************/

    }
}




