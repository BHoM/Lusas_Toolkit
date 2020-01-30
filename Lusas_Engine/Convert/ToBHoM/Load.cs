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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.oM.Base;
using BH.oM.Geometry;
using BH.oM.Structure.Elements;
using BH.oM.Structure.Properties;
using BH.oM.Structure.Loads;
using BH.oM.Common.Materials;
using BH.Engine.Lusas;
using Lusas.LPI;

namespace BH.Engine.Lusas
{
    public static partial class Convert
    {
        public static PointForce ToBHoMLoad(IFLoading lusasPointForce, IEnumerable<IFAssignment> assignmentList, Dictionary<string,Node> nodes)
        {
            IFLoadcase assignedLoadcase = (IFLoadcase)assignmentList.First().getAssignmentLoadset();
            Loadcase bhomLoadcase = BH.Engine.Lusas.Convert.ToBHoMLoadcase(assignedLoadcase);

            IEnumerable<Node> bhomNodes = GetNodeAssignments(assignmentList, nodes);

            Vector forceVector = new Vector { X = lusasPointForce.getValue("px"), Y = lusasPointForce.getValue("py"), Z = lusasPointForce.getValue("pz") };
            Vector momentVector = new Vector { X = lusasPointForce.getValue("mx"), Y = lusasPointForce.getValue("my"), Z = lusasPointForce.getValue("mz") };

            PointForce bhomPointForce = BH.Engine.Structure.Create.PointForce(bhomLoadcase, bhomNodes, forceVector, momentVector, LoadAxis.Global,GetName(lusasPointForce));

            int bhomID = GetBHoMID(lusasPointForce, 'l');
            bhomPointForce.CustomData["Lusas_id"] = bhomID;
            return bhomPointForce;
        }

        public static GravityLoad ToBHoMLoad(IFLoading lusasGravityLoad, IEnumerable<IFAssignment> assignmentList, string geometry, Dictionary<string, Bar> bars, Dictionary<string,PanelPlanar> surfaces)
        {
            IFLoadcase assignedLoadcase = (IFLoadcase)assignmentList.First().getAssignmentLoadset();
            Loadcase bhomLoadcase = BH.Engine.Lusas.Convert.ToBHoMLoadcase(assignedLoadcase);
            Vector gravityVector = new Vector { X = lusasGravityLoad.getValue("accX"), Y = lusasGravityLoad.getValue("accY"), Z = lusasGravityLoad.getValue("accZ") };
            GravityLoad bhomGravityLoad = new GravityLoad();

            if (geometry=="Bar")
            {
                IEnumerable<Bar> bhomBars = GetBarAssignments(assignmentList, bars);
                bhomGravityLoad = BH.Engine.Structure.Create.GravityLoad(bhomLoadcase, gravityVector, bhomBars,GetName(lusasGravityLoad));    
            }
            else
            {
                IEnumerable<PanelPlanar> bhomSurfs = GetSurfaceAssignments(assignmentList, surfaces);
                bhomGravityLoad = BH.Engine.Structure.Create.GravityLoad(bhomLoadcase, gravityVector, bhomSurfs, GetName(lusasGravityLoad));
            }

            int bhomID = GetBHoMID(lusasGravityLoad, 'l');
            bhomGravityLoad.CustomData["Lusas_id"] = bhomID;
            return bhomGravityLoad;
        }

        public static BarUniformlyDistributedLoad ToBHoMLoad(IFLoading lusasDistributed, IEnumerable<IFAssignment> assignmentList, Dictionary<string, Bar> bars)
        {
            IFLoadcase assignedLoadcase = (IFLoadcase)assignmentList.First().getAssignmentLoadset();
            Loadcase bhomLoadcase = BH.Engine.Lusas.Convert.ToBHoMLoadcase(assignedLoadcase);

            IEnumerable<Bar> bhomBars = GetBarAssignments(assignmentList, bars);

            Vector forceVector = new Vector { X = lusasDistributed.getValue("WX"), Y = lusasDistributed.getValue("WY"), Z = lusasDistributed.getValue("WZ") };

            BarUniformlyDistributedLoad bhomBarUniformlyDistributed = null;

            if (lusasDistributed.getAttributeType() == "Global Distributed Load")
            {
                Vector momentVector = new Vector { X = lusasDistributed.getValue("MX"), Y = lusasDistributed.getValue("MY"), Z = lusasDistributed.getValue("MZ") };

                bhomBarUniformlyDistributed = BH.Engine.Structure.Create.BarUniformlyDistributedLoad(
                    bhomLoadcase,
                    bhomBars,
                    forceVector,
                    momentVector,
                    LoadAxis.Global,
                    true,
                    GetName(lusasDistributed));
            }
            else if(lusasDistributed.getAttributeType() == "Distributed Load")
            {
                bhomBarUniformlyDistributed = BH.Engine.Structure.Create.BarUniformlyDistributedLoad(
                    bhomLoadcase,
                    bhomBars,
                    forceVector,
                    null,
                    LoadAxis.Local,
                    true,
                    GetName(lusasDistributed));
            }

            int bhomID = GetBHoMID(lusasDistributed, 'l');
            bhomBarUniformlyDistributed.CustomData["Lusas_id"] = bhomID;
            return bhomBarUniformlyDistributed;
        }

        public static AreaUniformalyDistributedLoad ToBHoMLoad(IFLoading lusasDistributed, IEnumerable<IFAssignment> assignmentList, Dictionary<string, PanelPlanar> surfs)
        {
            IFLoadcase assignedLoadcase = (IFLoadcase)assignmentList.First().getAssignmentLoadset();
            Loadcase bhomLoadcase = BH.Engine.Lusas.Convert.ToBHoMLoadcase(assignedLoadcase);

            IEnumerable<PanelPlanar> bhomSurfs = GetSurfaceAssignments(assignmentList, surfs);
            
            Vector pressureVector = new Vector { X = lusasDistributed.getValue("WX"), Y = lusasDistributed.getValue("WY"), Z = lusasDistributed.getValue("WZ") };

            AreaUniformalyDistributedLoad bhomSurfaceUniformlyDistributed = null;

            if (lusasDistributed.getAttributeType() == "Global Distributed Load")
            {
                bhomSurfaceUniformlyDistributed = BH.Engine.Structure.Create.AreaUniformalyDistributedLoad(
                    bhomLoadcase,
                    pressureVector,
                    bhomSurfs,
                    LoadAxis.Global,
                    true,
                    GetName(lusasDistributed));
            }
            else if (lusasDistributed.getAttributeType() == "Distributed Load")
            {
                bhomSurfaceUniformlyDistributed = BH.Engine.Structure.Create.AreaUniformalyDistributedLoad(
                    bhomLoadcase,
                    pressureVector,
                    bhomSurfs,
                    LoadAxis.Local,
                    true,
                    GetName(lusasDistributed));
            }

            int bhomID = GetBHoMID(lusasDistributed, 'l');
            bhomSurfaceUniformlyDistributed.CustomData["Lusas_id"] = bhomID;
            return bhomSurfaceUniformlyDistributed;
        }
    }
}

