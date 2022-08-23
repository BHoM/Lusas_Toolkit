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
using System.IO;
using System.Linq;
using BH.Engine.Adapter;
using BH.oM.Adapters.Lusas;
using BH.oM.Adapter;
using BH.oM.Base;
using BH.oM.Adapter.Commands;
using BH.oM.Structure.Loads;
using Lusas.LPI;

namespace BH.Adapter.Lusas
{
#if Debug18 || Release18
    public partial class LusasV18Adapter
#elif Debug19 || Release19
    public partial class LusasV19Adapter
#elif Debug191 || Release191
    public partial class LusasV191Adapter
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
            m_LusasApplication.fileOpen("%PerMachineAppDataPlatform%\\config\\AfterNewModel");
            d_LusasData.setAnalysisCategory("3D");

            string fileName = $"{m_directory}\\sections.csv";
            File.Delete(fileName);

            return true;
        }

        /***************************************************/

        public bool RunCommand(Save command)
        {
            if (d_LusasData.getDBFilename() == "")
            {
                Engine.Base.Compute.RecordError("The model file does not have a filename, please SaveAs before attempting to Save.");
                return false;
            }

            d_LusasData.save();

            return true;
        }

        /***************************************************/

        public bool RunCommand(SaveAs command)
        {
            d_LusasData.saveAs(command.FileName);
            m_directory = new FileInfo(command.FileName).Directory.FullName;

            return true;
        }

        /***************************************************/

        public bool RunCommand(Open command)
        {
            if (System.IO.File.Exists(command.FileName))
            {
                m_LusasApplication.openDatabase(command.FileName);
                d_LusasData = m_LusasApplication.getDatabase();
                m_directory = command.FileName;

                return true;
            }
            else
            {
                Engine.Base.Compute.RecordError("File does not exist");
                return false;
            }
        }

        /***************************************************/

        public bool RunCommand(Analyse command)
        {
            return Analyse();
        }

        /***************************************************/

        public bool RunCommand(AnalyseLoadCases command)
        {
            if (command.LoadCases == null || command.LoadCases.Count() == 0)
                Engine.Base.Compute.RecordNote("No cases provided, all cases will be run");

            return Analyse(command.LoadCases);
        }

        /***************************************************/

        public bool RunCommand(ClearResults command)
        {
            d_LusasData.closeAllResults();

            return true;
        }

        /***************************************************/

        public bool RunCommand(Close command)
        {
            if (command.SaveBeforeClose)
            {
                if (d_LusasData.getDBFilename() == "")
                {
                    Engine.Base.Compute.RecordError("The model file does not have a filename, please SaveAs before attempting to Save.");
                    return false;
                }
                else
                    d_LusasData.save();
            }

            d_LusasData.close();

            return true;
        }

        /***************************************************/

        public bool RunCommand(Exit command)
        {
            if (command.SaveBeforeClose)
            {
                if (d_LusasData.getDBFilename() == "")
                {
                    Engine.Base.Compute.RecordError("The model file does not have a filename, please SaveAs before attempting to Save.");
                    return false;
                }
                else
                    d_LusasData.save();
            }

            d_LusasData.close();
            m_LusasApplication.quit();
            m_LusasApplication = null;

            return true;
        }

        /***************************************************/

        public bool RunCommand(IExecuteCommand command)
        {
            Engine.Base.Compute.RecordWarning($"The command {command.GetType().Name} is not supported by this Adapter.");
            return false;
        }

        /***************************************************/
        /**** Private helper methods                    ****/
        /***************************************************/

        private bool Analyse(IEnumerable<object> cases = null)
        {
            d_LusasData.closeAllResults();
            d_LusasData.updateMesh();
            string filename = d_LusasData.getDBFilename();
            if (filename == "")
            {
                Engine.Base.Compute.RecordError("Model has not been saved with a filename, please SaveAs with a filename");
                return false;
            }
            d_LusasData.save();

            IFLusasRunOptionsObj solverOptions = m_LusasApplication.solverOptions();
            IFTabulateDataObj solverExport = m_LusasApplication.solverExport();

            solverOptions.setAllDefaults();
            solverExport.setFilename("%DBFolder%\\%ModelName%~Analysis 1.dat");
            solverExport.setAnalysis("Analysis 1");

            if (cases == null || cases.Count() == 0)
            {
                solverExport.setSolveAllLoadcases(true);
            }
            else
            {
                //Disable setSolveAllLoadcases, this overrides the setDoSolve on individual loadcases
                solverExport.setSolveAllLoadcases(false);

                List<string> names = new List<string>();

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
                        Engine.Base.Compute.RecordWarning("Can not set up cases for running of type " + item.GetType().Name + ". Item " + item.ToString() + " will be ignored. Please provide case names or BHoM cases to be run");
                        continue;
                    }
                    names.Add(name);
                }

                object[] loadcases = d_LusasData.getLoadsets("loadcase", "all");

                for (int i = 0; i < loadcases.Count(); i++)
                {
                    IFLoadcase loadcase = (IFLoadcase)loadcases[i];
                    if (names.Contains(loadcase.getName()))
                        loadcase.setDoSolve(true);
                    else
                        loadcase.setDoSolve(false);
                }
            }

            int exportError = d_LusasData.exportSolver(solverExport, solverOptions);

            //Did any calls to exportSolver and solve produce errors?
            bool exportErrors = false;
            bool solveErrors = false;

            //Any non-zero value for solveError or ExportError indicates an error
            int solveError;

            if (exportError != 0)
                exportErrors = true;
            else
            {
                solveError = m_LusasApplication.solve("%DBFolder%\\%ModelName%~Analysis 1.dat", solverOptions);
                if (solveError != 0)
                    solveErrors = true;
                m_LusasApplication.fileOpen("%PerMachineAppDataPlatform%\\config\\AfterSolve");
                m_LusasApplication.scanout("%DBFolder%\\%ModelName%~Analysis 1.out");
            }
            
            m_LusasApplication.processSolveErrors(exportErrors, solveErrors);
            d_LusasData.openResults("%DBFolder%\\%ModelName%~Analysis 1.mys", "Analysis 1", false, 0, false, false);

            return !(exportErrors & solveErrors);
        }

        /***************************************************/

    }
}

