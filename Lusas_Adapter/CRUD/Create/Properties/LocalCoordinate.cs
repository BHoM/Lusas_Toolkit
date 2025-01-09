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

using System.Linq;
using BH.Engine.Adapters.Lusas;
using BH.Engine.Structure;
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

        private IFLocalCoord CreateLocalCoordinate(IFLine lusasLine)
        {
            object lineXAxis = null;
            object lineYAxis = null;
            object lineZAxis = null;
            object origin = null;

            lusasLine.getAxesAtNrmCrds(0, ref origin, ref lineXAxis, ref lineYAxis, ref lineZAxis);

            double[] localXAxis = Adapters.Lusas.Convert.ToDouble(lineXAxis);
            double[] localYAxis = Adapters.Lusas.Convert.ToDouble(lineYAxis);
            double[] localZAxis = Adapters.Lusas.Convert.ToDouble(lineZAxis);

            IF3dCoords barStart = lusasLine.getStartPositionCoords();

            double[] worldXAxis = new double[] { 1, 0, 0 };
            double[] worldYAxis = new double[] { 0, 1, 0 };
            double[] worldZAxis = new double[] { 0, 0, 1 };

            double[] barorigin = new double[] { barStart.getX(), barStart.getY(), barStart.getZ() };

            double[] matrixCol0 = new double[]
            {
                worldXAxis.Zip(localXAxis, (d1,d2) => d1 * d2).Sum(),
                worldYAxis.Zip(localXAxis, (d1,d2) => d1 * d2).Sum(),
                worldZAxis.Zip(localXAxis, (d1,d2) => d1 * d2).Sum(),
            };

            double[] matrixCol1 = new double[]
            {
                worldXAxis.Zip(localYAxis, (d1,d2) => d1 * d2).Sum(),
                worldYAxis.Zip(localYAxis, (d1,d2) => d1 * d2).Sum(),
                worldZAxis.Zip(localYAxis, (d1,d2) => d1 * d2).Sum(),
            };

            double[] matrixCol2 = new double[]
            {
                worldXAxis.Zip(localZAxis, (d1,d2) => d1 * d2).Sum(),
                worldYAxis.Zip(localZAxis, (d1,d2) => d1 * d2).Sum(),
                worldZAxis.Zip(localZAxis, (d1,d2) => d1 * d2).Sum(),
            };

            string lusasName = "L" + lusasLine.getID().ToString() + "/ Local Axis";

            IFLocalCoord barLocalAxis = d_LusasData.createLocalCartesianAttr(
            lusasName, barorigin,
            matrixCol0, matrixCol1, matrixCol2);

            return barLocalAxis;
        }

        /***************************************************/

    }
}







