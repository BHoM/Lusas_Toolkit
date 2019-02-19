/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2018, the respective contributors. All rights reserved.
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
        public static PointForce ToPointForce(
            IFLoading lusasPointForce, IEnumerable<IFAssignment> lusasAssignments, 
            Dictionary<string, Node> bhomNodeDictionary)
        {
            IFLoadcase assignedLoadcase = (IFLoadcase)lusasAssignments.First().getAssignmentLoadset();
            Loadcase bhomLoadcase = ToBHoMLoadcase(assignedLoadcase);

            IEnumerable<Node> bhomNodes = Lusas.Query.GetNodeAssignments(lusasAssignments, bhomNodeDictionary);

            Vector forceVector = new Vector
            {
                X = lusasPointForce.getValue("px"),
                Y = lusasPointForce.getValue("py"),
                Z = lusasPointForce.getValue("pz")
            };

            Vector momentVector = new Vector
            {
                X = lusasPointForce.getValue("mx"),
                Y = lusasPointForce.getValue("my"),
                Z = lusasPointForce.getValue("mz")
            };

            PointForce bhomPointForce = Structure.Create.PointForce(
                bhomLoadcase,
                bhomNodes,
                forceVector,
                momentVector,
                LoadAxis.Global,
                Lusas.Query.GetName(lusasPointForce));

            int adapterID = Lusas.Query.GetAdapterID(lusasPointForce, 'l');
            bhomPointForce.CustomData["Lusas_id"] = adapterID;

            return bhomPointForce;
        }
    }
}