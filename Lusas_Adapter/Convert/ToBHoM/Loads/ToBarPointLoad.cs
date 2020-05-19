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

using System.Collections.Generic;
using System.Linq;
using BH.Adapter.Lusas;
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
            IEnumerable<IFAssignment> lusasAssignments, Dictionary<string, Bar> bhomBarDictionary)
        {
            IFLoadcase assignedLoadcase = (IFLoadcase)lusasAssignments.First().getAssignmentLoadset();
            Loadcase bhomLoadcase = ToLoadcase(assignedLoadcase);

            IEnumerable<Bar> bhomBars = GetLineAssignments(lusasAssignments, bhomBarDictionary);

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

            BarPointLoad bhomBarPointLoad = null;

            bhomBarPointLoad = Engine.Structure.Create.BarPointLoad(
                bhomLoadcase,
                forcePosition,
                bhomBars,
                forceVector,
                momentVector,
                LoadAxis.Global,
                GetName(lusasBarPointLoad));

            int adapterID = GetAdapterID(lusasBarPointLoad, 'l');
            bhomBarPointLoad.CustomData[AdapterIdName] = adapterID;

            return bhomBarPointLoad;
        }

        /***************************************************/

    }
}
