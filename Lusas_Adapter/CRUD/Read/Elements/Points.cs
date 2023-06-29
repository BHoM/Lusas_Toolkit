/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2023, the respective contributors. All rights reserved.
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
using BH.oM.Geometry;
using Lusas.LPI;

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
#else
    public partial class LusasV17Adapter
#endif
    {
        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        private List<Point> ReadPoints(List<string> ids = null)
        {
            object[] lusasPoints = d_LusasData.getObjects("Point");
            List<Point> points = new List<Point>();

            if (!(lusasPoints.Count() == 0))
            {
                HashSet<string> groupNames = ReadTags();

                for (int i = 0; i < lusasPoints.Count(); i++)
                {
                    IFPoint lusasPoint = (IFPoint)lusasPoints[i];
                    Point point = Adapters.Lusas.Convert.ToPoint(lusasPoint, groupNames);
                    points.Add(point);
                }
            }

            return points;
        }

        /***************************************************/

    }
}



