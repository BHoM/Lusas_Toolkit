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

using Lusas.LPI;
using BH.oM.Adapters.Lusas;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        private IFMeshSurface CreateMeshSettings2D(MeshSettings2D meshSettings2D)
        {
            if (!Engine.Lusas.Query.CheckIllegalCharacters(meshSettings2D.Name))
            {
                return null;
            }

            IFMeshSurface lusasSurfaceMesh = null;
            string lusasName = "Me" + meshSettings2D.CustomData[AdapterIdName] + "/" + meshSettings2D.Name;
            if (d_LusasData.existsAttribute("Mesh", lusasName))
            {
                lusasSurfaceMesh = (IFMeshSurface)d_LusasData.getAttribute("Mesh", lusasName);
            }
            else
            {
                lusasSurfaceMesh = d_LusasData.createMeshSurface(lusasName);
                if (meshSettings2D.SplitMethod == Split2D.Automatic)
                {
                    lusasSurfaceMesh.addElementName("QTS4");
                }
                else if (meshSettings2D.SplitMethod == Split2D.Divisions)
                {
                    lusasSurfaceMesh.setRegular("QTS4", meshSettings2D.xDivisions, meshSettings2D.yDivisions);
                }
                else if (meshSettings2D.SplitMethod == Split2D.Size)
                {
                    lusasSurfaceMesh.setRegularSize("QTS4", meshSettings2D.ElementSize);
                }
            }
            return lusasSurfaceMesh;
        }
    }
}