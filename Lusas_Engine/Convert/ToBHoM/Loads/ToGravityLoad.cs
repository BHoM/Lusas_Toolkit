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
using BH.oM.Base;
using Lusas.LPI;

namespace BH.Engine.Lusas
{
    public static partial class Convert
    {
        public static GravityLoad ToGravityLoad(IFLoading lusasGravityLoad,
            IEnumerable<IFAssignment> lusasAssignments,
            Dictionary<string, Bar> bhomBars,
            Dictionary<string, Panel> bhomPanels)
        {
            IFLoadcase assignedLoadcase = (IFLoadcase)lusasAssignments.First().getAssignmentLoadset();
            Loadcase bhomLoadcase = ToLoadcase(assignedLoadcase);
            Vector gravityVector = new Vector
            {
                X = lusasGravityLoad.getValue("accX"),
                Y = lusasGravityLoad.getValue("accY"),
                Z = lusasGravityLoad.getValue("accZ")
            };

            IEnumerable<BHoMObject> bhomObjects = Lusas.Query.GetGeometryAssignments(
                lusasAssignments, null, bhomBars, bhomPanels);
            GravityLoad bhomGravityLoad = Structure.Create.GravityLoad(
                bhomLoadcase, gravityVector, bhomObjects, Lusas.Query.GetName(lusasGravityLoad));

            int adapterID = Lusas.Query.GetAdapterID(lusasGravityLoad, 'l');
            bhomGravityLoad.CustomData[AdapterIdName] = adapterID;

            return bhomGravityLoad;
        }
    }
}
