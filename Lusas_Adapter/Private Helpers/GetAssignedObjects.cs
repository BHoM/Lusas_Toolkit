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
using BH.oM.Structure.Elements;
using System.Linq;
using Lusas.LPI;
using BH.oM.Base;
using BH.oM.Structure.Loads;

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
        /**** Private Methods                           ****/
        /***************************************************/

        private object[] GetAssignedPoints(Load<Node> loads)
        {
            int[] lusasIDs = loads.Objects.Elements.Select(x => System.Convert.ToInt32(x.CustomData[AdapterIdName])).ToArray();

            object[] arrayGeometry = d_LusasData.getObjects("Point", lusasIDs);

            return arrayGeometry;
        }

        /***************************************************/

        private object[] GetAssignedLines(Load<Bar> loads)
        {
            int[] lusasIDs = loads.Objects.Elements.Select(x => System.Convert.ToInt32(x.CustomData[AdapterIdName])).ToArray();

            object[] arrayGeometry = d_LusasData.getObjects("Line", lusasIDs);

            return arrayGeometry;
        }

        /***************************************************/

        private object[] GetAssignedSurfaces(Load<IAreaElement> loads)
        {
            int[] lusasIDs = loads.Objects.Elements.Select(x => System.Convert.ToInt32(x.CustomData[AdapterIdName])).ToArray();

            object[] arrayGeometry = d_LusasData.getObjects("Surface", lusasIDs);

            return arrayGeometry;
        }

        /***************************************************/

        private IFGeometry[] GetAssignedObjects(Load<BHoMObject> loads)
        {
            List<IFGeometry> assignedGeometry = new List<IFGeometry>();

            foreach (BHoMObject element in loads.Objects.Elements)
            {
                if (element is Node)
                {
                    IFGeometry lusasPoint = d_LusasData.getPointByNumber(
                        System.Convert.ToInt32(element.CustomData[AdapterIdName]));

                    assignedGeometry.Add(lusasPoint);
                }
                else if (element is Bar)
                {
                    IFGeometry lusasBar = d_LusasData.getLineByNumber(
                        System.Convert.ToInt32(element.CustomData[AdapterIdName]));

                    assignedGeometry.Add(lusasBar);
                }
                else if (element is Panel)
                {
                    IFGeometry lusasSurface = d_LusasData.getSurfaceByNumber(
                        System.Convert.ToInt32(element.CustomData[AdapterIdName]));

                    assignedGeometry.Add(lusasSurface);
                }
            }

            IFGeometry[] arrayGeometry = assignedGeometry.ToArray();

            return arrayGeometry;
        }

        /***************************************************/

    }
}

