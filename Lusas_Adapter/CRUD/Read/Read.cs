/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2024, the respective contributors. All rights reserved.
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
using System.Collections;
using System.Collections.Generic;
using BH.oM.Adapter;
using BH.oM.Adapters.Lusas;
using BH.oM.Adapters.Lusas.Fragments;
using BH.oM.Analytical.Results;
using BH.oM.Base;
using BH.oM.Geometry;
using BH.oM.Structure.Elements;
using BH.oM.Structure.Constraints;
using BH.oM.Structure.SurfaceProperties;
using BH.oM.Structure.SectionProperties;
using BH.oM.Structure.Loads;
using BH.oM.Structure.MaterialFragments;
using BH.oM.Structure.Results;

namespace BH.Adapter.Lusas
{
#if Debug18 || Release18
    public partial class LusasV18Adapter
#elif Debug19 || Release19
    public partial class LusasV19Adapter
#elif Debug191 || Release191
    public partial class LusasV191Adapter
#elif Debug200 || Release200
    public partial class LusasV200Adapter
#elif Debug210 || Release210
    public partial class LusasV210Adapter
#elif Debug211 || Release211
    public partial class LusasV211Adapter
#else
    public partial class LusasV17Adapter
#endif
    {
        /***************************************************/
        /**** Adapter overload method                   ****/
        /***************************************************/

        protected override IEnumerable<IBHoMObject> IRead(Type type, IList ids = null, ActionConfig actionConfig = null)
        {
            if (type == typeof(Bar))
                return ReadBars(ids as dynamic);
            else if (type == typeof(Node))
                return ReadNodes(ids as dynamic);
            else if (type == typeof(IMaterialFragment))
                return ReadMaterials(ids as dynamic);
            else if (type == typeof(Panel))
                return ReadPanels(ids as dynamic);
            else if (type == typeof(Edge))
                return ReadEdges(ids as dynamic);
            else if (type == typeof(Point))
                return ReadPoints(ids as dynamic);
            else if (type == typeof(Opening))
                return ReadOpenings(ids as dynamic);
            else if (type == typeof(Constraint6DOF))
                return Read6DOFConstraints(ids as dynamic);
            else if (type == typeof(Constraint4DOF))
                return Read4DOFConstraints(ids as dynamic);
            else if (type == typeof(Loadcase))
                return ReadLoadcases(ids as dynamic);
            else if (typeof(ILoad).IsAssignableFrom(type))
                return ChooseLoad(type, ids as dynamic);
            else if (typeof(ISurfaceProperty).IsAssignableFrom(type))
                return Read2DProperties(ids as dynamic);
            else if (typeof(ISectionProperty).IsAssignableFrom(type))
                return ReadSectionProperties(ids as dynamic);
            else if (type == typeof(LoadCombination))
                return ReadLoadCombinations(ids as dynamic);
            else if (type == typeof(BHoMObject))
                return ReadAll(ids as dynamic);
            else if (type == typeof(MeshSettings1D))
                return ReadMeshSettings1D(ids as dynamic);
            else if (type == typeof(MeshSettings2D))
                return ReadMeshSettings2D(ids as dynamic);
            else if (typeof(IResult).IsAssignableFrom(type))
                Modules.Structure.ErrorMessages.ReadResultsError(type);

            return null;


        }

        /***************************************************/

    }
}





