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

using System;
using System.Collections.Generic;
using BH.oM.Structure.Constraints;
using BH.oM.Structure.SurfaceProperties;
using BH.oM.Structure.SectionProperties;
using BH.oM.Structure.MaterialFragments;
using BH.oM.Structure.Elements;
using BH.oM.Structure.Loads;
using BH.oM.Geometry;
using Lusas.LPI;
using BH.oM.Adapters.Lusas;
using BH.oM.Adapters.Lusas.Fragments;
using BH.Engine.Adapters.Lusas;

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
        /**** Adapter overload method                   ****/
        /***************************************************/

        //protected override object NextFreeId(Type type, bool refresh = false)
        //{
        //    //Method that returns the next free index for a specific object type. 
        //    //Software dependent which type of index to return. Could be int, string, Guid or whatever the specific software is using
        //    //At the point of index assignment, the objects have not yet been created in the target software. 
        //    //The if statement below is designed to grab the first free index for the first object being created and after that increment.

        //    //Change from object to what the specific software is using
        //    int index = 1;

        protected override object NextFreeId(Type objectType, bool refresh)
        {
            return -1;
        }

    }
}





