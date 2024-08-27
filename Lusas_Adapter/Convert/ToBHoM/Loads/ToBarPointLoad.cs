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

using System.Collections.Generic;
using System.Linq;
using BH.Engine.Adapter;
using BH.oM.Adapters.Lusas;
using BH.oM.Geometry;
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

        public static BarPointLoad ToBarPointLoad(IFLoading lusasBarPointLoad,
            IEnumerable<IFAssignment> lusasAssignments, Dictionary<string, Bar> bars)
        {
            IFLoadcase assignedLoadcase = (IFLoadcase)lusasAssignments.First().getAssignmentLoadset();
            Loadcase loadcase = ToLoadcase(assignedLoadcase);

            IEnumerable<Bar> assignedBars = GetLineAssignments(lusasAssignments, bars);

            Vector forceVector = new Vector
            {
                X = lusasBarPointLoad.getValue("PX"),
                Y = lusasBarPointLoad.getValue("PY"),
                Z = lusasBarPointLoad.getValue("PZ")
            };

            Vector momentVector = new Vector
            {
                X = lusasBarPointLoad.getValue("MX"),
                Y = lusasBarPointLoad.getValue("MY"),
                Z = lusasBarPointLoad.getValue("MZ")
            };

            double forcePosition = lusasBarPointLoad.getValue("Distance");

            BarPointLoad barPointLoad;

#if Debug17 || Release17 || Debug18 || Release18 || Debug19
            LoadAxis loadAxis = lusasBarPointLoad.getValue("LoadDirection") == 4 ? LoadAxis.Local : LoadAxis.Global; //4 for local, 3 for global (only two possible values)
#else
            LoadAxis loadAxis = lusasBarPointLoad.getValue("LoadDirection") == "Local(beam)" ? LoadAxis.Local : LoadAxis.Global;   
#endif

            barPointLoad = Engine.Structure.Create.BarPointLoad(
                loadcase,
                forcePosition,
                assignedBars,
                forceVector,
                momentVector,
                loadAxis,
                GetName(lusasBarPointLoad));

            long adapterNameId = lusasBarPointLoad.getID();
            barPointLoad.SetAdapterId(typeof(LusasId), adapterNameId);

            return barPointLoad;
        }

        /***************************************************/

    }
}
