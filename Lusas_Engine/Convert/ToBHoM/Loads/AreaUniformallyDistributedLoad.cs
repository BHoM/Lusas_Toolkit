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
        public static AreaUniformalyDistributedLoad ToAreaUniformallyDistributed(
            IFLoading lusasDistributed, IEnumerable<IFAssignment> lusasAssignments,
            Dictionary<string, PanelPlanar> panelPlanarDictionary)
        {
            IFLoadcase assignedLoadcase = (IFLoadcase)lusasAssignments.First().getAssignmentLoadset();
            Loadcase bhomLoadcase = ToBHoMLoadcase(assignedLoadcase);

            IEnumerable<IAreaElement> bhomPlanarPanels = Lusas.Query.GetSurfaceAssignments(
                lusasAssignments, panelPlanarDictionary);

            Vector pressureVector = new Vector
            {
                X = lusasDistributed.getValue("WX"),
                Y = lusasDistributed.getValue("WY"),
                Z = lusasDistributed.getValue("WZ")
            };

            AreaUniformalyDistributedLoad bhomSurfaceUniformlyDistributed = null;

            if (lusasDistributed.getAttributeType() == "Global Distributed Load")
            {
                bhomSurfaceUniformlyDistributed = Structure.Create.AreaUniformalyDistributedLoad(
                bhomLoadcase,
                pressureVector,
                bhomPlanarPanels,
                LoadAxis.Global,
                true,
                Lusas.Query.GetName(lusasDistributed));
            }
            else if (lusasDistributed.getAttributeType() == "Distributed Load")
            {
                bhomSurfaceUniformlyDistributed = Structure.Create.AreaUniformalyDistributedLoad(
                bhomLoadcase,
                pressureVector,
                bhomPlanarPanels,
                LoadAxis.Local,
                true,
                Lusas.Query.GetName(lusasDistributed));
            }

            int adapterID = Lusas.Query.GetAdapterID(lusasDistributed, 'l');
            bhomSurfaceUniformlyDistributed.CustomData["Lusas_id"] = adapterID;

            return bhomSurfaceUniformlyDistributed;
        }
    }
}