﻿/*
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
using BH.Engine.Adapter;
using BH.oM.Adapters.Lusas;
using BH.oM.Adapter;
using BH.oM.Reflection;
using BH.oM.Adapter.Commands;
using BH.oM.Structure.Loads;
using Lusas.LPI;

namespace BH.Adapter.Lusas
{
#if Debug18 || Release18
    public partial class LusasV18Adapter
#elif Debug19 || Release19
    public partial class LusasV19Adapter
#else
    public partial class LusasV17Adapter
#endif
    {
        /***************************************************/
        /**** IAdapter Interface                        ****/
        /***************************************************/

        public override Output<List<object>, bool> Execute(IExecuteCommand command, ActionConfig actionConfig = null)
        {
            var output = new Output<List<object>, bool>() { Item1 = null, Item2 = false };

            output.Item2 = RunCommand(command as dynamic);

            return output;
        }

        /***************************************************/
        /**** Commands                                  ****/
        /***************************************************/

        public bool RunCommand(NewModel command)
        {
            m_LusasApplication.newDatabase("Structural");
            d_LusasData = m_LusasApplication.getDatabase();
            d_LusasData.setModelUnits("N,m,kg,s,C");
            d_LusasData.setTimescaleUnits("Seconds");
            d_LusasData.setVerticalDir("Z");
            //m_LusasApplication.fileOpen("%PerMachineAppDataPlatform%\\config\\AfterNewModel");
            d_LusasData.setAnalysisCategory("3D");

            return true;
        }

        /***************************************************/

        public bool RunCommand(Save command)
        {
            d_LusasData.save();

            return true;
        }

        /***************************************************/

        public bool RunCommand(SaveAs command)
        {
            d_LusasData.saveAs(command.FileName);

            return true;
        }

        /***************************************************/

        public bool RunCommand(Open command)
        {
            if (System.IO.File.Exists(command.FileName))
            {
                m_LusasApplication.openDatabase(command.FileName);

                return true;
            }
            else
            {
                Engine.Reflection.Compute.RecordError("File does not exist");
                return false;
            }
        }

        /***************************************************/

        public bool RunCommand(Analyse command)
        {
            IFTabulateDataObj tabulateDataObj = m_LusasApplication.getSolverExportData();
            tabulateDataObj.setSolveAllLoadcases(true);
            m_LusasApplication.solve(d_LusasData.getDBFilename());

            return true;
        }

        /***************************************************/

        public bool RunCommand(AnalyseLoadCases command)
        {
            if (command.LoadCases == null || command.LoadCases.Count() == 0)
                Engine.Reflection.Compute.RecordNote("No cases provided, all cases will be run");

            return Analyse(command.LoadCases);
        }

        /***************************************************/

        public bool RunCommand(ClearResults command)
        {
            d_LusasData.closeAllResults();
            
            return true;
        }

        /***************************************************/

        public bool RunCommand(IExecuteCommand command)
        {
            Engine.Reflection.Compute.RecordWarning($"The command {command.GetType().Name} is not supported by this Adapter.");
            return false;
        }

        /***************************************************/
        /**** Private helper methods                    ****/
        /***************************************************/

        private bool Analyse(IEnumerable<object> cases = null)
        {
            //Check if the model has been saved
            if (d_LusasData.getDBFilename() == "Untitled")
            {
                Engine.Reflection.Compute.RecordWarning("Lusas requires the model to be saved before being analysed. Please save the model and try running again.");
                return false;
            }

            IFTabulateDataObj tabulateDataObj = m_LusasApplication.getSolverExportData();

            if (cases == null || cases.Count() == 0)
            {
                tabulateDataObj.setSolveAllLoadcases(true);
            }
            else
            {
                //Unselect all cases
                tabulateDataObj.setSolveAllLoadcases(false);

                //Select provided cases
                foreach (object item in cases)
                {
                    string name;
                    if (item == null)
                        continue;
                    if (item is string)
                        name = item as string;
                    else if (item is ICase)
                        name = (item as ICase).Name;
                    else
                    {
                        Engine.Reflection.Compute.RecordWarning("Can not set up cases for running of type " + item.GetType().Name + ". Item " + item.ToString() + " will be ignored. Please provide case names or BHoM cases to be run");
                        continue;
                    }

                    IFLoadcase loadcase = (IFLoadcase)d_LusasData.getLoadset(name);
                    if (loadcase == null)
                        Engine.Reflection.Compute.RecordWarning("Failed to set case " + name + "for running. Please check that the case exists in the model");
                    else
                        loadcase.setDoSolve(true);
                }

            }

            return true;
        }

        /***************************************************/

    }
}