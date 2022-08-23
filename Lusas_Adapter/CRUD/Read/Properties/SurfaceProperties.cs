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
using BH.oM.Structure.SurfaceProperties;
using Lusas.LPI;

namespace BH.Adapter.Lusas
{
#if Debug18 || Release18
    public partial class LusasV18Adapter
#elif Debug19 || Release19
    public partial class LusasV19Adapter
#elif Debug191 || Release191
    public partial class LusasV191Adapter
#else
    public partial class LusasV17Adapter
#endif
    {
        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        private List<ISurfaceProperty> Read2DProperties(List<string> ids = null)
        {
            object[] lusasGeometrics = d_LusasData.getAttributes("Surface Geometric");
            List<ISurfaceProperty> surfaceProperties = new List<ISurfaceProperty>();

            for (int i = 0; i < lusasGeometrics.Count(); i++)
            {
                IFAttribute lusasGeometric = (IFAttribute)lusasGeometrics[i];
                ISurfaceProperty surfaceProperty = Adapters.Lusas.Convert.ToSurfaceProperty(lusasGeometric);
                if (surfaceProperty != null)
                    surfaceProperties.Add(surfaceProperty);
            }

            return surfaceProperties;
        }

        /***************************************************/

    }
}



