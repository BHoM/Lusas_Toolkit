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
using System.Linq;
using Lusas.LPI;
using BH.oM.Adapters.Lusas;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        private List<MeshSettings1D> ReadMeshSettings1D(List<string> ids = null)
        {
            List<MeshSettings1D> bhomMeshSettings1Ds = new List<MeshSettings1D>();
            object[] lusasMesh1Ds = d_LusasData.getAttributes("Line Mesh");

            for (int i = 0; i < lusasMesh1Ds.Count(); i++)
            {
                IFMeshLine lusasMesh1D = (IFMeshLine)lusasMesh1Ds[i];
                MeshSettings1D bhomMeshSettings1D = Engine.Lusas.Convert.ToBHoMMeshSettings1D(lusasMesh1D);
                List<string> analysisName = new List<string> { lusasMesh1D.getAttributeType() };
                bhomMeshSettings1D.Tags = new HashSet<string>(analysisName);
                bhomMeshSettings1Ds.Add(bhomMeshSettings1D);
            }

            return bhomMeshSettings1Ds;
        }
    }
}
