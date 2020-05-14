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

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        private List<Loadcase> ReadLoadcases(List<string> ids = null)
        {
            List<Loadcase> bhomLoadcases = new List<Loadcase>();
            object[] allLoadcases = d_LusasData.getLoadsets("loadcase", "all");

            for (int i = 0; i < allLoadcases.Count(); i++)
            {
                IFLoadcase lusasLoadcase = (IFLoadcase)allLoadcases[i];
                Loadcase bhomLoadcase = Adapter.Adapters.Lusas.Convert.ToLoadcase(lusasLoadcase);
                List<string> analysisName = new List<string> { lusasLoadcase.getAnalysis().getName() };
                bhomLoadcase.Tags = new HashSet<string>(analysisName);
                bhomLoadcases.Add(bhomLoadcase);
            }

            return bhomLoadcases;
        }

        /***************************************************/

    }
}
