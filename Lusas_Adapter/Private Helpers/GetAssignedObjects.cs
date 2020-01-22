/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2019, the respective contributors. All rights reserved.
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
    public partial class LusasAdapter
    {
        internal object[] GetAssignedPoints(Load<Node> bhomLoads)
        {
            string[] lusasIDs = bhomLoads.Objects.Elements.Select(x => "P" + x.CustomData[AdapterIdName].ToString()).ToArray();

            object[] arrayGeometry = d_LusasData.getObjects("Point", lusasIDs);

            return arrayGeometry;
        }

        public object[] GetAssignedLines(Load<Bar> bhomLoads)
        {
            string[] lusasIDs = bhomLoads.Objects.Elements.Select(x => "L" + x.CustomData[AdapterIdName].ToString()).ToArray();

            object[] arrayGeometry = d_LusasData.getObjects("Line", lusasIDs);

            return arrayGeometry;
        }

        public object[] GetAssignedSurfaces(Load<IAreaElement> bhomLoads)
        {
            string[] lusasIDs = bhomLoads.Objects.Elements.Select(x => "S" + x.CustomData[AdapterIdName].ToString()).ToArray();

            object[] arrayGeometry = d_LusasData.getObjects("Surface", lusasIDs);

            return arrayGeometry;
        }

        public IFGeometry[] GetAssignedObjects(Load<BHoMObject> bhomLoads)
        {
            List<IFGeometry> assignedGeometry = new List<IFGeometry>();

            foreach (BHoMObject bhomObject in bhomLoads.Objects.Elements)
            {
                if (bhomObject is Node)
                {
                    IFGeometry lusasPoint = d_LusasData.getPointByName(
                        "P" + bhomObject.CustomData[AdapterIdName].ToString());

                    assignedGeometry.Add(lusasPoint);
                }
                else if (bhomObject is Bar)
                {
                    IFGeometry lusasBar = d_LusasData.getLineByName(
                        "L" + bhomObject.CustomData[AdapterIdName].ToString());

                    assignedGeometry.Add(lusasBar);
                }
                else if (bhomObject is Panel)
                {
                    IFGeometry lusasSurface = d_LusasData.getSurfaceByName(
                        "S" + bhomObject.CustomData[AdapterIdName].ToString());

                    assignedGeometry.Add(lusasSurface);
                }
            }

            IFGeometry[] arrayGeometry = assignedGeometry.ToArray();

            return arrayGeometry;
        }
    }
}
