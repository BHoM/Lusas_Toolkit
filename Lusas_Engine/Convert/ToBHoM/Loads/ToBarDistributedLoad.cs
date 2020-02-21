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
using BH.oM.Geometry;
using BH.oM.Structure.Elements;
using BH.oM.Structure.Loads;
using Lusas.LPI;

namespace BH.Engine.Lusas
{
    public static partial class Convert
    {
        public static BarVaryingDistributedLoad ToBarDistributedLoad(IFLoading lusasBarDistributedLoad,
            IEnumerable<IFAssignment> lusasAssignments, Dictionary<string, Bar> bhomBarDictionary)
        {
            IFLoadcase assignedLoadcase = (IFLoadcase)lusasAssignments.First().getAssignmentLoadset();
            Loadcase bhomLoadcase = ToLoadcase(assignedLoadcase);

            IEnumerable<Bar> bhomBars = Lusas.Query.GetLineAssignments(lusasAssignments, bhomBarDictionary);

            Vector startForceVector = new Vector
            {
                X = lusasBarDistributedLoad.getValue("startpx"),
                Y = lusasBarDistributedLoad.getValue("startpy"),
                Z = lusasBarDistributedLoad.getValue("startpz")
            };

            Vector endForceVector = new Vector
            {
                X = lusasBarDistributedLoad.getValue("endpx"),
                Y = lusasBarDistributedLoad.getValue("endpy"),
                Z = lusasBarDistributedLoad.getValue("endpz")
            };

            Vector startMomentVector = new Vector
            {
                X = lusasBarDistributedLoad.getValue("startmx"),
                Y = lusasBarDistributedLoad.getValue("startmy"),
                Z = lusasBarDistributedLoad.getValue("startmz")
            };

            Vector endMomentVector = new Vector
            {
                X = lusasBarDistributedLoad.getValue("endmx"),
                Y = lusasBarDistributedLoad.getValue("endmy"),
                Z = lusasBarDistributedLoad.getValue("endmz")
            };

            double startPosition = lusasBarDistributedLoad.getValue("startDistance");
            double endPosition = lusasBarDistributedLoad.getValue("endDistance");

            BarVaryingDistributedLoad bhomBarPointLoad = null;

            bhomBarPointLoad = Structure.Create.BarVaryingDistributedLoad(
                bhomLoadcase,
                bhomBars,
                startPosition,
                startForceVector,
                startMomentVector,
                endPosition,
                endForceVector,
                endMomentVector,
                LoadAxis.Local,
                false,
                Lusas.Query.GetName(lusasBarDistributedLoad));

            int adapterID = Lusas.Query.GetAdapterID(lusasBarDistributedLoad, 'l');
            bhomBarPointLoad.CustomData["Lusas_id"] = adapterID;

            return bhomBarPointLoad;
        }
    }
}
