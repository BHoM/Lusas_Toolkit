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

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Lusas.LPI;
using BH.oM.Adapters.Lusas;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter : BHoMAdapter
    {

        /***************************************************/
        /**** Constructors                              ****/
        /***************************************************/

        public LusasAdapter(string filePath, LusasConfig lusasConfig = null, bool active = false)
        {
            if(active)
            {
                AdapterId = "Lusas_id";   //Set the "AdapterId" to "SoftwareName_id". Generally stored as a constant string in the convert class in the SoftwareName_Engine

                Config.SeparateProperties = true;   //Set to true to push dependant properties of objects before the main objects are being pushed. Example: push nodes before pushing bars
                Config.MergeWithComparer = true;    //Set to true to use EqualityComparers to merge objects. Example: merge nodes in the same location
                Config.ProcessInMemory = false;     //Set to false to to update objects in the toolkit during the push
                Config.CloneBeforePush = true;      //Set to true to clone the objects before they are being pushed through the software. Required if any modifications at all, as adding a software ID is done to the objects
                Config.UseAdapterId = true;         //Tag objects with a software specific id in the CustomData. Requires the NextID method to be overridden and implemented

                if (string.IsNullOrWhiteSpace(filePath))
                {
                    throw new ArgumentException("No file path given");
                }
                else if (IsApplicationRunning())
                {
                    throw new Exception("Lusas process already running");
                }
                else
                {
                    m_LusasApplication = new LusasWinApp();
                    m_LusasApplication.enableUI(true);
                    m_LusasApplication.setVisible(true);
                    d_LusasData = m_LusasApplication.openDatabase(filePath);
                }
            }
        }

        /***************************************************/
        /**** Public  Fields                           ****/
        /***************************************************/

        public static bool IsApplicationRunning()
        {
            return (Process.GetProcessesByName("lusas_m").Length > 0) ? true : false;
        }

        /***************************************************/
        /**** Private  Fields                           ****/
        /***************************************************/

        //Add any comlink object as a private field here, example named:

        public LusasWinApp m_LusasApplication;
        public IFDatabase d_LusasData;
        private Dictionary<Type, Dictionary<int, HashSet<string>>> m_tags = new Dictionary<Type, Dictionary<int, HashSet<string>>>();
        public LusasConfig lusasConfig;


        /***************************************************/


    }
}
