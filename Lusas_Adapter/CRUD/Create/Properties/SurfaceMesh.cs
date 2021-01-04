/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2021, the respective contributors. All rights reserved.
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
using BH.Engine.Adapter;
using BH.oM.Adapters.Lusas;
using BH.oM.Adapters.Lusas.Fragments;

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
        private IFMeshSurface CreateMeshSettings2D(MeshSettings2D meshSettings2D)
        {
            /***************************************************/
            /**** Private Methods                           ****/
            /***************************************************/

            if (!Engine.Adapters.Lusas.Query.CheckIllegalCharacters(meshSettings2D.Name))
            {
                return null;
            }

            IFMeshSurface lusasSurfaceMesh = null;

            if (d_LusasData.existsAttribute("Mesh", meshSettings2D.Name))
            {
                lusasSurfaceMesh = (IFMeshSurface)d_LusasData.getAttribute("Mesh", meshSettings2D.Name);
            }
            else
            {
                lusasSurfaceMesh = d_LusasData.createMeshSurface(meshSettings2D.Name);
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

            int adapterIdName = lusasSurfaceMesh.getID();
            meshSettings2D.SetAdapterId(typeof(LusasId), adapterIdName);

            return lusasSurfaceMesh;
        }

        /***************************************************/

    }
}

