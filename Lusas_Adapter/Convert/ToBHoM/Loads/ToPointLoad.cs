/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2022, the respective contributors. All rights reserved.
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

        public static PointLoad ToPointLoad(
            IFLoading lusasPointLoad, IEnumerable<IFAssignment> lusasAssignments,
            Dictionary<string, Node> nodes)
        {
            IFLoadcase assignedLoadcase = (IFLoadcase)lusasAssignments.First().getAssignmentLoadset();
            Loadcase loadcase = ToLoadcase(assignedLoadcase);

            IEnumerable<Node> assignedNodes = GetPointAssignments(lusasAssignments, nodes);

            Vector forceVector = new Vector
            {
                X = lusasPointLoad.getValue("px"),
                Y = lusasPointLoad.getValue("py"),
                Z = lusasPointLoad.getValue("pz")
            };

            Vector momentVector = new Vector
            {
                X = lusasPointLoad.getValue("mx"),
                Y = lusasPointLoad.getValue("my"),
                Z = lusasPointLoad.getValue("mz")
            };

            PointLoad pointLoad = Engine.Structure.Create.PointLoad(
                loadcase,
                assignedNodes,
                forceVector,
                momentVector,
                LoadAxis.Global,
                GetName(lusasPointLoad));

            int adapterNameId = (int)lusasPointLoad.getID();
            pointLoad.SetAdapterId(typeof(LusasId), adapterNameId);

            return pointLoad;
        }

        /***************************************************/

    }
}


