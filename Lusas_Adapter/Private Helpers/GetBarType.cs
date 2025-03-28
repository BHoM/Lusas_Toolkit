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
using System;
using Lusas.LPI;
using BH.oM.Structure.Constraints;
using BH.oM.Structure.Elements;
using BH.oM.Base.Attributes;
using System.ComponentModel;

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
        [Description("Sorts a list of Lusas Bar IDs based on if they have the properties of a thick beam or of a bar. This is returned as a Tuple with two lists (Bar IDs, Beam IDs)")]
        [Input("IDs", "The list of IDs to be split")]

        private Tuple<List<int>, List<int>> GetBarType(List<int> ids)
        {
            List<int> barIds = new List<int>();
            List<int> beamIds = new List<int>();

            foreach (int id in ids)
            {
                IFLine lusasLine = d_LusasData.getLineByNumber(id);
                object[] meshAssignments = lusasLine.getAssignments("Mesh");

                if (meshAssignments.Length > 0)
                {
                    foreach (object assignment in meshAssignments)
                    {
                        IFAssignment lusasAssignment = (IFAssignment)assignment;
                        IFAttribute lusasMesh = lusasAssignment.getAttribute();
                        IFMeshLine lusasLineMesh = (IFMeshLine)lusasMesh;

                        object[] barMeshName = lusasLineMesh.getElementNames();

                        foreach (object type in barMeshName)
                        {
                            if (
                                    type.ToString() == "BMI21" ||
                                    type.ToString() == "BMI31" ||
                                    type.ToString() == "BMX21" ||
                                    type.ToString() == "BMX31" ||
                                    type.ToString() == "BMI21W" ||
                                    type.ToString() == "BMI31W" ||
                                    type.ToString() == "BMX21W" ||
                                    type.ToString() == "BMX31W")
                            {
                                beamIds.Add(id);
                            }
                            else if (
                                type.ToString() == "BRS2" ||
                                type.ToString() == "BRS3")
                                barIds.Add(id);
                        }
                    }
                }
            }

            Tuple<List<int>, List<int>> sortedIds =
            new Tuple<List<int>, List<int>>(barIds, beamIds);
            return sortedIds;

        }
        /***************************************************/

    }
}
