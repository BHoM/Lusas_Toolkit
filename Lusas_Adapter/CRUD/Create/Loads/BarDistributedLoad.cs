/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2025, the respective contributors. All rights reserved.
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
using BH.oM.Adapters.Lusas;
using BH.oM.Structure.Loads;
using Lusas.LPI;
using BH.Engine.Adapter;

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
#elif Debug211 || Release211
    public partial class LusasV211Adapter
#else
    public partial class LusasV17Adapter
#endif
    {
        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        private List<IFLoadingBeamDistributed> CreateBarDistributedLoad(
            BarVaryingDistributedLoad barDistributedLoad, object[] lusasLines)
        {
            List<IFLoadingBeamDistributed> lusasBarDistributedLoads = new List<IFLoadingBeamDistributed>();
            IFAssignment lusasAssignment = m_LusasApplication.assignment();
            IFLoadcase assignedLoadcase = (IFLoadcase)d_LusasData.getLoadset(barDistributedLoad.Loadcase.AdapterId<int>(typeof(LusasId)));

            if(!barDistributedLoad.RelativePositions)
                Engine.Base.Compute.RecordWarning(barDistributedLoad.GetType().ToString() + " uses parametric distances in the Lusas_Toolkit");

            List<double> valuesAtA = new List<double> {
                    barDistributedLoad.ForceAtStart.X, barDistributedLoad.ForceAtStart.Y,barDistributedLoad.ForceAtStart.Z,
                    barDistributedLoad.MomentAtStart.X, barDistributedLoad.MomentAtStart.Y,barDistributedLoad.MomentAtStart.Z
                };

            List<double> valuesAtB = new List<double> {
                    barDistributedLoad.ForceAtEnd.X, barDistributedLoad.ForceAtEnd.Y,barDistributedLoad.ForceAtEnd.Z,
                    barDistributedLoad.MomentAtEnd.X, barDistributedLoad.MomentAtEnd.Y,barDistributedLoad.MomentAtEnd.Z
                };

            List<string> keys = new List<string>() { "FX", "FY", "FZ", "MX", "MY", "MZ" };

            List<long> ids = new List<long>();

            string positioning = barDistributedLoad.RelativePositions ? "parametric" : "actual";
            string axis;
            if (barDistributedLoad.Projected)
            {
                axis = "projected";
                if (barDistributedLoad.RelativePositions)
                {
                    Engine.Base.Compute.RecordError(barDistributedLoad.Name + " has not been pushed because the positions are relative and the loads " +
                        "are projected (this combination is not supported in Lusas).");
                    return null;
                }
            }
            else
                axis = barDistributedLoad.Axis == LoadAxis.Global ? "global" : "local";

            for (int i = 0; i < valuesAtA.Count(); i++)
            {
                double valueAtA = valuesAtA[i];
                double valueAtB = valuesAtB[i];

                if ((valueAtA != 0) || (valueAtB != 0))
                {
                    IFLoadingBeamDistributed lusasBarDistributedLoad;
                    if (d_LusasData.existsAttribute("Loading", barDistributedLoad.Name + keys[i]))
                    {
                        lusasBarDistributedLoad = (IFLoadingBeamDistributed)d_LusasData.getAttribute("Loading", barDistributedLoad.Name + keys[i]);
                        lusasBarDistributedLoads.Add(lusasBarDistributedLoad);
                    }
                    else
                    {
                        lusasBarDistributedLoad = d_LusasData.createLoadingBeamDistributed(barDistributedLoad.Name + keys[i]);

                        lusasBarDistributedLoad.setBeamDistributed(positioning, axis, "beam");

                        switch (keys[i])
                        {
                            case "FX":
                                lusasBarDistributedLoad.addRow(
                                    barDistributedLoad.StartPosition, valueAtA, 0, 0, 0, 0, 0,
                                    barDistributedLoad.EndPosition, valueAtB, 0, 0, 0, 0, 0);

                                lusasBarDistributedLoads.Add(lusasBarDistributedLoad);
                                lusasAssignment.setLoadset(assignedLoadcase);
                                lusasBarDistributedLoad.assignTo(lusasLines, lusasAssignment);
                                break;

                            case "FY":
                                lusasBarDistributedLoad.addRow(
                                    barDistributedLoad.StartPosition, 0, valueAtA, 0, 0, 0, 0,
                                    barDistributedLoad.EndPosition, 0, valueAtB, 0, 0, 0, 0);

                                lusasBarDistributedLoads.Add(lusasBarDistributedLoad);
                                lusasAssignment.setLoadset(assignedLoadcase);
                                lusasBarDistributedLoad.assignTo(lusasLines, lusasAssignment);
                                break;

                            case "FZ":
                                lusasBarDistributedLoad.addRow(
                                    barDistributedLoad.StartPosition, 0, 0, valueAtA, 0, 0, 0,
                                    barDistributedLoad.EndPosition, 0, 0, valueAtB, 0, 0, 0);

                                lusasBarDistributedLoads.Add(lusasBarDistributedLoad);
                                lusasAssignment.setLoadset(assignedLoadcase);
                                lusasBarDistributedLoad.assignTo(lusasLines, lusasAssignment);
                                break;

                            case "MX":
                                lusasBarDistributedLoad.addRow(
                                    barDistributedLoad.StartPosition, 0, 0, 0, valueAtA, 0, 0,
                                    barDistributedLoad.EndPosition, 0, 0, 0, valueAtB, 0, 0);

                                lusasBarDistributedLoads.Add(lusasBarDistributedLoad);
                                lusasAssignment.setLoadset(assignedLoadcase);
                                if(barDistributedLoad.Projected || barDistributedLoad.Axis == LoadAxis.Global)
                                        Engine.Base.Compute.RecordWarning("Lusas does not support internal distributed moments in the global axis or as projected loads.");
                                lusasBarDistributedLoad.assignTo(lusasLines, lusasAssignment);
                                break;

                            case "MY":
                                lusasBarDistributedLoad.addRow(
                                    barDistributedLoad.StartPosition, 0, 0, 0, 0, valueAtA, 0,
                                    barDistributedLoad.EndPosition, 0, 0, 0, 0, valueAtB, 0);

                                lusasBarDistributedLoads.Add(lusasBarDistributedLoad);
                                lusasAssignment.setLoadset(assignedLoadcase);
                                lusasBarDistributedLoad.assignTo(lusasLines, lusasAssignment);
                                break;

                            case "MZ":
                                lusasBarDistributedLoad.addRow(
                                    barDistributedLoad.StartPosition, 0, 0, 0, 0, 0, valueAtA,
                                    barDistributedLoad.EndPosition, 0, 0, 0, 0, 0, valueAtB);

                                lusasBarDistributedLoads.Add(lusasBarDistributedLoad);
                                lusasAssignment.setLoadset(assignedLoadcase);
                                lusasBarDistributedLoad.assignTo(lusasLines, lusasAssignment);
                                break;
                        }
                        lusasBarDistributedLoads.Add(lusasBarDistributedLoad);
                        ids.Add(lusasBarDistributedLoad.getID());
                    }
                }
            }

            barDistributedLoad.SetAdapterId(typeof(LusasId), ids);

            return lusasBarDistributedLoads;
        }

        /***************************************************/

    }
}





