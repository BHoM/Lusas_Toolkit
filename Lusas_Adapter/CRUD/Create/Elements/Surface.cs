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

using BH.oM.Structure.Elements;
using Lusas.LPI;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        private IFSurface CreateSurface(Panel panel, IFLine[] lusasLines)
        {
            IFSurface lusasSurface;
            if (d_LusasData.existsSurfaceByName("S" + panel.CustomData[AdapterId]))
            {
                lusasSurface = d_LusasData.getSurfaceByName("S" + panel.CustomData[AdapterId]);
            }
            else
            {
                lusasSurface = d_LusasData.createSurfaceBy(lusasLines);
                lusasSurface.setName("S" + panel.CustomData[AdapterId]);
            }

            if (!(panel.Tags.Count == 0))
            {
                AssignObjectSet(lusasSurface, panel.Tags);
            }

            if(!(panel.Property == null))
            {
                string geometricSurfaceName = "G" + 
                    panel.Property.CustomData[AdapterId] + "/" + panel.Property.Name;

                IFAttribute lusasGeometricSurface = d_LusasData.getAttribute(
                    "Surface Geometric", geometricSurfaceName);

                lusasGeometricSurface.assignTo(lusasSurface);
                if (!(panel.Property.Material == null))
                {
                    string materialName = "M" + panel.Property.Material.CustomData[AdapterId] + 
                        "/" + panel.Property.Material.Name;

                    IFAttribute lusasMaterial = d_LusasData.getAttribute("Material", materialName);
                    lusasMaterial.assignTo(lusasSurface);
                }
            }

            return lusasSurface;
        }

    }
}