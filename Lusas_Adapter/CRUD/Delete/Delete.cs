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

using System;
using System.Collections.Generic;
using BH.oM.Adapter;
using BH.oM.External.Lusas;
using BH.oM.Structure.Elements;
using BH.oM.Structure.SectionProperties;
using BH.oM.Structure.SurfaceProperties;
using BH.oM.Structure.MaterialFragments;
using BH.oM.Structure.Constraints;
using BH.oM.Structure.Loads;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        /***************************************************/
        /**** Adapter overload method                   ****/
        /***************************************************/

        protected override int IDelete(Type type, IEnumerable<object> ids, ActionConfig actionConfig = null)
        {
            int success = 0;

            if (type == typeof(Node))
                success = DeletePoints(ids);
            else if (type == typeof(Bar))
                success = DeleteLines(ids);
            else if (type == typeof(Panel))
                success = DeleteSurfaces(ids);
            else if (typeof(ISectionProperty).IsAssignableFrom(type))
                success = DeleteSectionProperties(ids);
            else if (typeof(ISurfaceProperty).IsAssignableFrom(type))
                success = DeleteSurfaceProperties(ids);
            else if (type == typeof(MeshSettings1D))
                success = DeleteMeshSettings1D(ids);
            else if (type == typeof(Constraint4DOF))
                success = DeleteMeshSettings2D(ids);
            else if (type == typeof(Constraint6DOF))
                success = DeleteConstraint6DOF(ids);
            else if (type == typeof(IMaterialFragment))
                success = DeleteMaterials(ids);
            else if (type == typeof(AreaTemperatureLoad))
                success = DeleteAreaTemperatureLoad(ids);
            else if (type == typeof(AreaUniformlyDistributedLoad))
                success = DeleteAreaUnformlyDistributedLoads(ids);
            else if (type == typeof(BarPointLoad))
                success = DeleteBarPointLoad(ids);
            else if (type == typeof(BarTemperatureLoad))
                success = DeleteBarTemperatureLoad(ids);
            else if (type == typeof(BarUniformlyDistributedLoad))
                success = DeleteBarUniformlyDistributedLoads(ids);
            else if (type == typeof(Loadcase))
                success = DeleteLoadcases(ids);
            else if (type == typeof(LoadCombination))
                success = DeleteLoadCombinations(ids);
            else if (type == typeof(PointDisplacement))
                success = DeletePointDisplacements(ids);
            else if (type == typeof(PointLoad))
                success = DeletePointLoads(ids);

            return 0;
        }

        /***************************************************/

    }
}

