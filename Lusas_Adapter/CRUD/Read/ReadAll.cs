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
using BH.oM.Base;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        private List<IBHoMObject> ReadAll(List<string> ids = null)
        {
            List<IBHoMObject> objects = new List<IBHoMObject>();

            objects.AddRange(ReadNodes());
            objects.AddRange(ReadBars());
            objects.AddRange(ReadPanels());
            objects.AddRange(Read2DProperties());
            objects.AddRange(ReadMaterials());
            objects.AddRange(Read4DOFConstraints());
            objects.AddRange(Read6DOFConstraints());
            objects.AddRange(ReadLoadcases());
            objects.AddRange(ReadLoadCombinations());
            objects.AddRange(ReadPointLoads());
            objects.AddRange(ReadPointDisplacements());
            objects.AddRange(ReadBarUniformlyDistributedLoads());
            objects.AddRange(ReadBarPointLoads());
            objects.AddRange(ReadBarVaryingDistributedLoads());
            objects.AddRange(ReadAreaUniformlyDistributedLoads());
            objects.AddRange(ReadBarTemperatureLoads());
            objects.AddRange(ReadAreaTemperatureLoads());
            objects.AddRange(ReadGravityLoads());
            return objects;
        }
    }
}
