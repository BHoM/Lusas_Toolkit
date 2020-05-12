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
using BH.Engine.External.Lusas;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        private List<IFLoadingBeamDistributed> CreateBarDistributedLoad(
            BarVaryingDistributedLoad bhomBarDistributedLoad, object[] lusasLines)
        {
            if (!Engine.External.Lusas.Query.CheckIllegalCharacters(bhomBarDistributedLoad.Name))
            {
                return null;
            }

            List<IFLoadingBeamDistributed> lusasBarDistributedLoads = new List<IFLoadingBeamDistributed>();
            IFAssignment lusasAssignment = m_LusasApplication.assignment();
            IFLoadcase assignedLoadcase = (IFLoadcase)d_LusasData.getLoadset(
                "Lc" + bhomBarDistributedLoad.Loadcase.CustomData[AdapterIdName] +
                "/" + bhomBarDistributedLoad.Loadcase.Name);

            string lusasName =
                "BDl" + bhomBarDistributedLoad.CustomData[AdapterIdName] + "/" + bhomBarDistributedLoad.Name;

            NameSearch("BDl", bhomBarDistributedLoad.CustomData[AdapterIdName].ToString(),
                bhomBarDistributedLoad.Name, ref lusasName);

            if (d_LusasData.existsAttribute("Loading", lusasName))
            {
                IFLoadingBeamDistributed lusasBarDistributedLoad =
                    (IFLoadingBeamDistributed)d_LusasData.getAttribute("Loading", lusasName);

                lusasBarDistributedLoads.Add(lusasBarDistributedLoad);
            }
            else
            {
                Engine.Reflection.Compute.RecordWarning(
                    bhomBarDistributedLoad.GetType().ToString() + " uses parametric distances in the Lusas_Toolkit"
                    );

                List<double> valuesAtA = new List<double> {
                    bhomBarDistributedLoad.ForceA.X, bhomBarDistributedLoad.ForceA.Y,
                    bhomBarDistributedLoad.ForceA.Z,
                    bhomBarDistributedLoad.MomentA.X, bhomBarDistributedLoad.MomentA.Y,
                    bhomBarDistributedLoad.MomentA.Z
                };

                List<double> valuesAtB = new List<double> {
                    bhomBarDistributedLoad.ForceB.X, bhomBarDistributedLoad.ForceB.Y,
                    bhomBarDistributedLoad.ForceB.Z,
                    bhomBarDistributedLoad.MomentB.X, bhomBarDistributedLoad.MomentB.Y,
                    bhomBarDistributedLoad.MomentB.Z
                };

                List<string> keys = new List<string>() { "FX", "FY", "FZ", "MX", "MY", "MZ" };

                for (int i = 0; i < valuesAtA.Count(); i++)
                {
                    double valueAtA = valuesAtA[i];
                    double valueAtB = valuesAtB[i];
                    string key = keys[i];

                    if ((valueAtA != 0) || (valueAtB != 0))
                    {
                        IFLoadingBeamDistributed lusasBarDistributedLoad =
                            d_LusasData.createLoadingBeamDistributed(bhomBarDistributedLoad.Name + key);

                        if (bhomBarDistributedLoad.Axis.ToString() == "Global")
                            lusasBarDistributedLoad.setBeamDistributed("parametric", "global", "beam");
                        else
                            lusasBarDistributedLoad.setBeamDistributed("parametric", "local", "beam");

                        switch (key)
                        {
                            case "FX":
                                lusasBarDistributedLoad.addRow(
                                    bhomBarDistributedLoad.DistanceFromA, valueAtA, 0, 0, 0, 0, 0,
                                    bhomBarDistributedLoad.DistanceFromB, valueAtB, 0, 0, 0, 0, 0);

                                lusasBarDistributedLoads.Add(lusasBarDistributedLoad);
                                lusasAssignment.setLoadset(assignedLoadcase);
                                lusasBarDistributedLoad.assignTo(lusasLines, lusasAssignment);
                                break;

                            case "FY":
                                lusasBarDistributedLoad.addRow(
                                    bhomBarDistributedLoad.DistanceFromA, 0, valueAtA, 0, 0, 0, 0,
                                    bhomBarDistributedLoad.DistanceFromB, 0, valueAtB, 0, 0, 0, 0);

                                lusasBarDistributedLoads.Add(lusasBarDistributedLoad);
                                lusasAssignment.setLoadset(assignedLoadcase);
                                lusasBarDistributedLoad.assignTo(lusasLines, lusasAssignment);
                                break;

                            case "FZ":
                                lusasBarDistributedLoad.addRow(
                                    bhomBarDistributedLoad.DistanceFromA, 0, 0, valueAtA, 0, 0, 0,
                                    bhomBarDistributedLoad.DistanceFromB, 0, 0, valueAtB, 0, 0, 0);

                                lusasBarDistributedLoads.Add(lusasBarDistributedLoad);
                                lusasAssignment.setLoadset(assignedLoadcase);
                                lusasBarDistributedLoad.assignTo(lusasLines, lusasAssignment);
                                break;

                            case "MX":
                                lusasBarDistributedLoad.addRow(
                                    bhomBarDistributedLoad.DistanceFromA, 0, 0, 0, valueAtA, 0, 0,
                                    bhomBarDistributedLoad.DistanceFromB, 0, 0, 0, valueAtB, 0, 0);

                                lusasBarDistributedLoads.Add(lusasBarDistributedLoad);
                                lusasAssignment.setLoadset(assignedLoadcase);
                                lusasBarDistributedLoad.assignTo(lusasLines, lusasAssignment);
                                break;

                            case "MY":
                                lusasBarDistributedLoad.addRow(
                                    bhomBarDistributedLoad.DistanceFromA, 0, 0, 0, 0, valueAtA, 0,
                                    bhomBarDistributedLoad.DistanceFromB, 0, 0, 0, 0, valueAtB, 0);

                                lusasBarDistributedLoads.Add(lusasBarDistributedLoad);
                                lusasAssignment.setLoadset(assignedLoadcase);
                                lusasBarDistributedLoad.assignTo(lusasLines, lusasAssignment);
                                break;

                            case "MZ":
                                lusasBarDistributedLoad.addRow(
                                    bhomBarDistributedLoad.DistanceFromA, 0, 0, 0, 0, 0, valueAtA,
                                    bhomBarDistributedLoad.DistanceFromB, 0, 0, 0, 0, 0, valueAtB);

                                lusasBarDistributedLoads.Add(lusasBarDistributedLoad);
                                lusasAssignment.setLoadset(assignedLoadcase);
                                lusasBarDistributedLoad.assignTo(lusasLines, lusasAssignment);
                                break;
                        }

                    }
                }
            }
            return lusasBarDistributedLoads;
        }

        /***************************************************/

    }
}
