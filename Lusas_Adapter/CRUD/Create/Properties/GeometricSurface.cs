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

using BH.Engine.Structure;
using BH.oM.Structure.SurfaceProperties;
using Lusas.LPI;

namespace BH.Adapter.Lusas
{
#if Debug18 || Release18
    public partial class LusasV18Adapter
#else
    public partial class LusasV17Adapter
#endif
    {
        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        private IFAttribute CreateGeometricSurface(ISurfaceProperty surfaceProperty)
        {
            IFAttribute lusasAttribute;

            if (d_LusasData.existsAttribute("Surface Geometric", surfaceProperty.DescriptionOrName()))
            {
                lusasAttribute = d_LusasData.getAttribute("Surface Geometric", surfaceProperty.DescriptionOrName());
            }
            else
            {
                IFGeometricSurface lusasGeometricSurface = CreateSurfraceProfile(surfaceProperty as dynamic, surfaceProperty.DescriptionOrName());
                lusasAttribute = lusasGeometricSurface;
            }

            surfaceProperty.CustomData[AdapterIdName] = lusasAttribute.getID().ToString();

            return lusasAttribute;
        }

        /***************************************************/

        private IFAttribute CreateSurfraceProfile(ConstantThickness bhomThickness, string lusasName)
        {
            IFGeometricSurface lusasGeometricSurface = d_LusasData.createGeometricSurface(lusasName);
            lusasGeometricSurface.setValue("t", bhomThickness.Thickness);
            return lusasGeometricSurface;
        }

        /***************************************************/

        private IFGeometricSurface CreateSurfraceProfile(LoadingPanelProperty bhomThickness, string lusasName)
        {
            Engine.Reflection.Compute.RecordError("LoadingPanelProperty not supported in Lusas_Toolkit");
            return null;
        }

        /***************************************************/

        private IFGeometricSurface CreateSurfraceProfile(Ribbed bhomThickness, string lusasName)
        {
            Engine.Reflection.Compute.RecordError("Ribbed not supported in Lusas_Toolkit");
            return null;
        }

        /***************************************************/

        private IFGeometricSurface CreateSurfraceProfile(Waffle bhomThickness, string lusasName)
        {
            Engine.Reflection.Compute.RecordError("Waffle not supported in Lusas_Toolkit");
            return null;
        }

        /***************************************************/

    }
}


