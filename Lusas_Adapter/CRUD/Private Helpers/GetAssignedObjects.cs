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

using System.Collections.Generic;
using BH.oM.Structure.Elements;
using Lusas.LPI;
using BH.oM.Base;
using BH.oM.Structure.Loads;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        public IFPoint[] GetAssignedPoints(Load<Node> bhomLoads)
        {
            List<IFPoint> assignedGeometry = new List<IFPoint>();
            foreach (BHoMObject bhomObject in bhomLoads.Objects.Elements)
            {
                IFPoint lusasPoint = d_LusasData.getPointByName(
                    "P" + bhomObject.CustomData[AdapterId].ToString());

                assignedGeometry.Add(lusasPoint);
            }

            IFPoint[] arrayGeometry = assignedGeometry.ToArray();

            return arrayGeometry;
        }

        public IFLine[] GetAssignedLines(Load<Bar> bhomLoads)
        {
            List<IFLine> assignedGeometry = new List<IFLine>();
            foreach (BHoMObject bhomObject in bhomLoads.Objects.Elements)
            {
                IFLine lusasLine = d_LusasData.getLineByName(
                    "L" + bhomObject.CustomData[AdapterId].ToString());

                assignedGeometry.Add(lusasLine);
            }

            IFLine[] arrayGeometry = assignedGeometry.ToArray();

            return arrayGeometry;
        }

        public IFSurface[] GetAssignedSurfaces(Load<IAreaElement> bhomLoads)
        {
            List<IFSurface> assignedGeometry = new List<IFSurface>();
            foreach (BHoMObject bhomObject in bhomLoads.Objects.Elements)
            {
                IFSurface lusasSurface = d_LusasData.getSurfaceByName(
                    "S" + bhomObject.CustomData[AdapterId].ToString());

                assignedGeometry.Add(lusasSurface);
            }

            IFSurface[] arrayGeometry = assignedGeometry.ToArray();

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
                        "P" + bhomObject.CustomData[AdapterId].ToString());

                    assignedGeometry.Add(lusasPoint);
                }
                else if (bhomObject is Bar)
                {
                    IFGeometry lusasBar = d_LusasData.getLineByName(
                        "L" + bhomObject.CustomData[AdapterId].ToString());

                    assignedGeometry.Add(lusasBar);
                }
                else if (bhomObject is PanelPlanar)
                {
                    IFGeometry lusasSurface = d_LusasData.getSurfaceByName(
                        "S" + bhomObject.CustomData[AdapterId].ToString());

                    assignedGeometry.Add(lusasSurface);
                }
            }

            IFGeometry[] arrayGeometry = assignedGeometry.ToArray();

            return arrayGeometry;
        }
    }
}
