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
using BH.oM.Structure.Loads;
using Lusas.LPI;
using BH.Engine.Adapters.Lusas;

namespace BH.Adapter.Lusas
{
#if Debug18 || Release18
    public partial class LusasV18Adapter
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
            IFLoadcase assignedLoadcase = (IFLoadcase)d_LusasData.getLoadset((int)barDistributedLoad.Loadcase.CustomData[AdapterIdName]);

            Engine.Reflection.Compute.RecordWarning(
                barDistributedLoad.GetType().ToString() + " uses parametric distances in the Lusas_Toolkit"
                );

            List<double> valuesAtA = new List<double> {
                    barDistributedLoad.ForceA.X, barDistributedLoad.ForceA.Y,barDistributedLoad.ForceA.Z,
                    barDistributedLoad.MomentA.X, barDistributedLoad.MomentA.Y,barDistributedLoad.MomentA.Z
                };

            List<double> valuesAtB = new List<double> {
                    barDistributedLoad.ForceB.X, barDistributedLoad.ForceB.Y,barDistributedLoad.ForceB.Z,
                    barDistributedLoad.MomentB.X, barDistributedLoad.MomentB.Y,barDistributedLoad.MomentB.Z
                };

            List<string> keys = new List<string>() { "FX", "FY", "FZ", "MX", "MY", "MZ" };

            List<int> ids = new List<int>();

            for (int i = 0; i < valuesAtA.Count(); i++)
            {
                double valueAtA = valuesAtA[i];
                double valueAtB = valuesAtB[i];

                if ((valueAtA != 0) || (valueAtB != 0))
                {
                    IFLoadingBeamDistributed lusasBarDistributedLoad;
                    if (d_LusasData.existsAttribute("Loading", barDistributedLoad.Name+keys[i]))
                    {
                        lusasBarDistributedLoad = (IFLoadingBeamDistributed)d_LusasData.getAttribute("Loading", barDistributedLoad.Name+keys[i]);
                        lusasBarDistributedLoads.Add(lusasBarDistributedLoad);
                    }
                    else
                    {
                        lusasBarDistributedLoad = d_LusasData.createLoadingBeamDistributed(barDistributedLoad.Name + keys[i]);
                        if (barDistributedLoad.Axis.ToString() == "Global")
                            lusasBarDistributedLoad.setBeamDistributed("parametric", "global", "beam");
                        else
                            lusasBarDistributedLoad.setBeamDistributed("parametric", "local", "beam");

                        switch (keys[i])
                        {
                            case "FX":
                                lusasBarDistributedLoad.addRow(
                                    barDistributedLoad.DistanceFromA, valueAtA, 0, 0, 0, 0, 0,
                                    barDistributedLoad.DistanceFromB, valueAtB, 0, 0, 0, 0, 0);

                                lusasBarDistributedLoads.Add(lusasBarDistributedLoad);
                                lusasAssignment.setLoadset(assignedLoadcase);
                                lusasBarDistributedLoad.assignTo(lusasLines, lusasAssignment);
                                break;

                            case "FY":
                                lusasBarDistributedLoad.addRow(
                                    barDistributedLoad.DistanceFromA, 0, valueAtA, 0, 0, 0, 0,
                                    barDistributedLoad.DistanceFromB, 0, valueAtB, 0, 0, 0, 0);

                                lusasBarDistributedLoads.Add(lusasBarDistributedLoad);
                                lusasAssignment.setLoadset(assignedLoadcase);
                                lusasBarDistributedLoad.assignTo(lusasLines, lusasAssignment);
                                break;

                            case "FZ":
                                lusasBarDistributedLoad.addRow(
                                    barDistributedLoad.DistanceFromA, 0, 0, valueAtA, 0, 0, 0,
                                    barDistributedLoad.DistanceFromB, 0, 0, valueAtB, 0, 0, 0);

                                lusasBarDistributedLoads.Add(lusasBarDistributedLoad);
                                lusasAssignment.setLoadset(assignedLoadcase);
                                lusasBarDistributedLoad.assignTo(lusasLines, lusasAssignment);
                                break;

                            case "MX":
                                lusasBarDistributedLoad.addRow(
                                    barDistributedLoad.DistanceFromA, 0, 0, 0, valueAtA, 0, 0,
                                    barDistributedLoad.DistanceFromB, 0, 0, 0, valueAtB, 0, 0);

                                lusasBarDistributedLoads.Add(lusasBarDistributedLoad);
                                lusasAssignment.setLoadset(assignedLoadcase);
                                lusasBarDistributedLoad.assignTo(lusasLines, lusasAssignment);
                                break;

                            case "MY":
                                lusasBarDistributedLoad.addRow(
                                    barDistributedLoad.DistanceFromA, 0, 0, 0, 0, valueAtA, 0,
                                    barDistributedLoad.DistanceFromB, 0, 0, 0, 0, valueAtB, 0);

                                lusasBarDistributedLoads.Add(lusasBarDistributedLoad);
                                lusasAssignment.setLoadset(assignedLoadcase);
                                lusasBarDistributedLoad.assignTo(lusasLines, lusasAssignment);
                                break;

                            case "MZ":
                                lusasBarDistributedLoad.addRow(
                                    barDistributedLoad.DistanceFromA, 0, 0, 0, 0, 0, valueAtA,
                                    barDistributedLoad.DistanceFromB, 0, 0, 0, 0, 0, valueAtB);

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

            barDistributedLoad.CustomData[AdapterIdName] = ids;

            return lusasBarDistributedLoads;
        }

        /***************************************************/

    }
}
