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
using BH.Engine.Adapter;
using BH.oM.Adapters.Lusas;

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

        private object[] GetAssignedPoints(IElementLoad<Node> loads)
        {
            int[] lusasIDs = loads.Objects.Elements.Select(x => x.AdapterId<int>(typeof(LusasId))).ToArray();

            object[] arrayGeometry = d_LusasData.getObjects("Point", lusasIDs);

            return arrayGeometry;
        }

        /***************************************************/

        private object[] GetAssignedLines(IElementLoad<Bar> loads)
        {
            int[] lusasIDs = loads.Objects.Elements.Select(x => x.AdapterId<int>(typeof(LusasId))).ToArray();

            object[] arrayGeometry = d_LusasData.getObjects("Line", lusasIDs);

            return arrayGeometry;
        }

        /***************************************************/

        private object[] GetAssignedSurfaces(IElementLoad<IAreaElement> loads)
        {
            int[] lusasIDs = loads.Objects.Elements.Select(x => x.AdapterId<int>(typeof(LusasId))).ToArray();

            object[] arrayGeometry = d_LusasData.getObjects("Surface", lusasIDs);

            return arrayGeometry;
        }

        /***************************************************/

        private IFGeometry[] GetAssignedObjects(IElementLoad<BHoMObject> loads)
        {
            List<IFGeometry> assignedGeometry = new List<IFGeometry>();

            foreach (BHoMObject element in loads.Objects.Elements)
            {
                if (element is Node)
                {
                    IFGeometry lusasPoint = d_LusasData.getPointByNumber(
                        element.AdapterId<int>(typeof(LusasId)));

                    assignedGeometry.Add(lusasPoint);
                }
                else if (element is Bar)
                {
                    IFGeometry lusasBar = d_LusasData.getLineByNumber(
                        element.AdapterId<int>(typeof(LusasId)));

                    assignedGeometry.Add(lusasBar);
                }
                else if (element is Panel)
                {
                    IFGeometry lusasSurface = d_LusasData.getSurfaceByNumber(
                        element.AdapterId<int>(typeof(LusasId)));

                    assignedGeometry.Add(lusasSurface);
                }
            }

            IFGeometry[] arrayGeometry = assignedGeometry.ToArray();

            return arrayGeometry;
        }

        /***************************************************/

    }
}

