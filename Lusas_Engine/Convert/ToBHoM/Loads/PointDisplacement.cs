/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2019, the respective contributors. All rights reserved.
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
        public static PointDisplacement ToPointDisplacement(IFLoading lusasPrescribedDisplacement,
            IEnumerable<IFAssignment> lusasAssignments, Dictionary<string, Node> bhomNodeDictionary)
        {
            IFLoadcase assignedLoadcase = (IFLoadcase)lusasAssignments.First().getAssignmentLoadset();
            Loadcase bhomLoadcase = ToBHoMLoadcase(assignedLoadcase);

            IEnumerable<Node> bhomNodes = Lusas.Query.GetNodeAssignments(lusasAssignments, bhomNodeDictionary);

            lusasPrescribedDisplacement.getValueNames();

            Vector translationVector = new Vector
            {
                X = lusasPrescribedDisplacement.getValue("U"),
                Y = lusasPrescribedDisplacement.getValue("V"),
                Z = lusasPrescribedDisplacement.getValue("W")
            };

            Vector rotationVector = new Vector
            {
                X = lusasPrescribedDisplacement.getValue("THX"),
                Y = lusasPrescribedDisplacement.getValue("THY"),
                Z = lusasPrescribedDisplacement.getValue("THZ")
            };

            PointDisplacement bhomPointDisplacement = Structure.Create.PointDisplacement(
                bhomLoadcase, bhomNodes, translationVector, rotationVector, LoadAxis.Global, 
                Lusas.Query.GetName(lusasPrescribedDisplacement));

            int adapterID = Lusas.Query.GetAdapterID(lusasPrescribedDisplacement, 'd');
            bhomPointDisplacement.CustomData["Lusas_id"] = adapterID;
            // Needs to be a bit here that determines whether it is global or local - actually this cannot be done as the 
            //attribute is applied to a group, and within the group the axis could local or global

            return bhomPointDisplacement;
        }
    }
}