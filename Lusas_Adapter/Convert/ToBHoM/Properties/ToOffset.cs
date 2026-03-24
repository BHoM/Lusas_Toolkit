/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2026, the respective contributors. All rights reserved.
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

using BH.oM.Structure.Offsets;
using BH.oM.Geometry;
using BH.oM.Adapters.Lusas;
using BH.Engine.Adapter;
using Lusas.LPI;

namespace BH.Adapter.Adapters.Lusas
{
    public static partial class Convert
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static Offset ToOffset(this IFGeometricLine lusasAttribute)
        {
            double ey = lusasAttribute.getValue("ey0", 0);
            double ez = lusasAttribute.getValue("ez0", 0);

            object yOriginType = "";
            object zOriginType = "";
            object yFibreLabel = "";
            object zFibreLabel = "";
#if !Debug18 && !Release18 && !Debug19 && !Release19 && !Debug191 && !Release191
            lusasAttribute.getEccentricityOrigin(ref yOriginType, ref zOriginType, ref yFibreLabel, ref zFibreLabel);
#endif

            if ((int)yOriginType == 1)
            {
                object fibreZ = 0.0, fibreY = 0.0;
                lusasAttribute.getFibrePosition((string)yFibreLabel, ref fibreZ, ref fibreY);
                ey += (double)fibreY; 
            }

            if ((int)zOriginType == 1)
            {
                object fibreZ = 0.0, fibreY = 0.0;
                lusasAttribute.getFibrePosition((string)zFibreLabel, ref fibreZ, ref fibreY);
                ez += (double)fibreZ;
            }

            if (ey == 0 && ez == 0)
                return null;

            Vector offsetVector = new Vector { X = 0, Y = -ey, Z = ez };

            Offset offset = new Offset
            {
                Start = offsetVector,
                End = new Vector { X = offsetVector.X, Y = offsetVector.Y, Z = offsetVector.Z }
            };

            offset.Name = GetName(lusasAttribute);

            long adapterNameId = lusasAttribute.getID();
            offset.SetAdapterId(typeof(LusasId), adapterNameId);

            return offset;
        }

        /***************************************************/

    }
}
