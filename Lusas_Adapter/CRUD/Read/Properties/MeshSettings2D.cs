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
using System.Linq;
using Lusas.LPI;
using BH.oM.Adapters.Lusas;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        private List<MeshSettings2D> ReadMeshSettings2D(List<string> ids = null)
        {
            List<MeshSettings2D> bhomMeshSettings2Ds = new List<MeshSettings2D>();
            object[] lusasMesh2Ds = d_LusasData.getAttributes("Surface Mesh");

            for (int i = 0; i < lusasMesh2Ds.Count(); i++)
            {
                IFMeshSurface lusasMesh2D = (IFMeshSurface)lusasMesh2Ds[i];
                MeshSettings2D bhomMeshSettings2D = Engine.Lusas.Convert.ToBHoMMeshSettings2D(lusasMesh2D);
                List<string> analysisName = new List<string> { lusasMesh2D.getAttributeType() };
                bhomMeshSettings2D.Tags = new HashSet<string>(analysisName);
                bhomMeshSettings2Ds.Add(bhomMeshSettings2D);
            }
            return bhomMeshSettings2Ds;
        }
    }
}