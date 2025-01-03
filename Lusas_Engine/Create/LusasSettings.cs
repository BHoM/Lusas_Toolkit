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

using BH.oM.Adapters.Lusas;
using BH.oM.Base.Attributes;
using System.ComponentModel;

namespace BH.Engine.Adapters.Lusas
{
    public static partial class Create
    {
        /***************************************************/
        /****           Public Constructors             ****/
        /***************************************************/

        [Description("Lusas adapter settings.")]
        [Input("mergeTolerance", "Sets the merging tolerance used in Lusas.")]
        [Input("librarySettings", "Sets the library settings.")]
        [Input("g", "Sets the standard gravity i.e. the acceleration due to gravity. This is used when GravityLoads are pushed/pulled from Lusas as the BHoM uses a factor of g, whereas Lusas uses a specific acceleration.")]
        [Output("Lusas specific adapter settings to be used by the adapter.")]
        public static LusasSettings LusasSettings(double mergeTolerance, LibrarySettings librarySettings = null, double g = 9.80665)
        {
            LusasSettings lusasSettings = new LusasSettings();

            lusasSettings.MergeTolerance = mergeTolerance;

            if (librarySettings != null)
                lusasSettings.LibrarySettings = librarySettings;

            lusasSettings.StandardGravity = g;

            return lusasSettings;
        }
        /***************************************************/
    }
}






