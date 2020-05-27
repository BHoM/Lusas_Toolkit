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
using BH.oM.Structure.Constraints;
using BH.oM.Structure.SurfaceProperties;
using BH.oM.Structure.SectionProperties;
using BH.oM.Structure.MaterialFragments;
using BH.oM.Structure.Elements;
using BH.oM.Structure.Loads;
using BH.oM.Geometry;
using Lusas.LPI;
using BH.oM.Adapters.Lusas;
using BH.Engine.Adapters.Lusas;

namespace BH.Adapter.Lusas
{
#if Debug18 || Release18
    public partial class LusasV18Adapter
#else
    public partial class LusasV17Adapter
#endif
    {
        /***************************************************/
        /**** Adapter overload method                   ****/
        /***************************************************/

        protected override object NextFreeId(Type type, bool refresh = false)
        {
            //Method that returns the next free index for a specific object type. 
            //Software dependent which type of index to return. Could be int, string, Guid or whatever the specific software is using
            //At the point of index assignment, the objects have not yet been created in the target software. 
            //The if statement below is designed to grab the first free index for the first object being created and after that increment.

            //Change from object to what the specific software is using
            int index = 1;

            if (!refresh && m_indexDict.TryGetValue(type, out index))
            {
                index++;
                m_indexDict[type] = index;
            }
            else
            {
                if (type == typeof(Node))
                {
                    int largestPointID = d_LusasData.getLargestPointID();
                    if (largestPointID == 0)
                    {
                        index = 1;
                    }
                    else
                    {

                        IFPoint largestPoint = d_LusasData.getPointByNumber(largestPointID);
                        index = System.Convert.ToInt32(
                               largestPoint.getName().RemovePrefix("P")) + 1;
                    }
                }
                if (type == typeof(Bar))
                {
                    if (d_LusasData.getLargestLineID() == 0)
                    {
                        index = 1;
                    }
                    else
                    {

                        IFLine largestLine = d_LusasData.getLineByNumber(d_LusasData.getLargestLineID());
                        index = System.Convert.ToInt32(
                               largestLine.getName().RemovePrefix("L")) + 1;
                    }
                }
                if (type == typeof(Panel))
                {
                    if (d_LusasData.getLargestSurfaceID() == 0)
                    {
                        index = 1;
                    }
                    else
                    {

                        IFSurface largestSurface = d_LusasData.getSurfaceByNumber(d_LusasData.getLargestSurfaceID());
                        index = System.Convert.ToInt32(
                            largestSurface.getName().RemovePrefix("S")) + 1;
                    }
                }
                if (type == typeof(Edge))
                {
                    if (d_LusasData.getLargestLineID() == 0)
                    {
                        index = 1;
                    }
                    else
                    {

                        IFLine largestLine = d_LusasData.getLineByNumber(d_LusasData.getLargestLineID());
                        index = System.Convert.ToInt32(
                            largestLine.getName().RemovePrefix("L")) + 1;
                    }
                }
                if (type == typeof(Point))
                {
                    int largestPointID = d_LusasData.getLargestPointID();
                    if (largestPointID == 0)
                    {
                        index = 1;
                    }
                    else
                    {

                        IFPoint largestPoint = d_LusasData.getPointByNumber(largestPointID);
                        index = System.Convert.ToInt32(
                            largestPoint.getName().RemovePrefix("P")) + 1;
                    }
                }
                if (typeof(ICase).IsAssignableFrom(type))
                {
                    int largestLoadcaseID = d_LusasData.getNextAvailableLoadcaseID() - 1;
                    if (largestLoadcaseID == 0)
                    {
                        index = 1;
                    }
                    else
                    {
                        IFLoadset largestLoadset = d_LusasData.getLoadset(largestLoadcaseID);
                        if (largestLoadset is IFLoadcase)
                        {
                            IFLoadcase largestLoadcase = (IFLoadcase)largestLoadset;
                            index = System.Convert.ToInt32(
                                Adapters.Lusas.Convert.GetAdapterID(largestLoadcase, 'c')) + 1;
                        }
                        else if (largestLoadset is IFBasicCombination)
                        {
                            IFBasicCombination largestLoadCombination = (IFBasicCombination)largestLoadset;
                            index = System.Convert.ToInt32(
                                Adapters.Lusas.Convert.GetAdapterID(largestLoadCombination, 'c')) + 1;
                        }
                    }
                }
                if (typeof(IMaterialFragment).IsAssignableFrom(type))
                {
                    int largestMaterialID = d_LusasData.getLargestAttributeID("Material");
                    if (largestMaterialID == 0)
                    {
                        index = 1;
                    }
                    else
                    {
                        IFAttribute largestAttribute = d_LusasData.getAttribute("Material", largestMaterialID);
                        index = System.Convert.ToInt32(
                            Adapters.Lusas.Convert.GetAdapterID(largestAttribute, 'M')) + 1;
                    }
                }
                if (type == typeof(Constraint6DOF) ||
                    type == typeof(Constraint4DOF))
                {
                    int largestestSupportID = d_LusasData.getLargestAttributeID("Support");
                    if (largestestSupportID == 0)
                    {
                        index = 1;
                    }
                    else
                    {

                        IFAttribute largestAttribute = d_LusasData.getAttribute("Support", largestestSupportID);
                        index = System.Convert.ToInt32(
                            Adapters.Lusas.Convert.GetAdapterID(largestAttribute, 'p')) + 1;
                    }
                }
                if (typeof(ISectionProperty).IsAssignableFrom(type) || typeof(ISurfaceProperty).IsAssignableFrom(type))
                {
                    int largestThicknessID = d_LusasData.getLargestAttributeID("Geometric");
                    if (largestThicknessID == 0)
                    {
                        index = 1;
                    }
                    else
                    {
                        IFAttribute largestAttribute = d_LusasData.getAttribute("Geometric", largestThicknessID);
                        index = System.Convert.ToInt32(
                            Adapters.Lusas.Convert.GetAdapterID(largestAttribute, 'G')) + 1;
                    }
                }
                if (typeof(ILoad).IsAssignableFrom(type))
                {
                    int largestLoadID = d_LusasData.getLargestAttributeID("Loading");
                    if (largestLoadID == 0)
                    {
                        index = 1;
                    }
                    else
                    {
                        IFAttribute largestAttribute = d_LusasData.getAttribute("Loading", largestLoadID);
                        if (largestAttribute is IFPrescribedDisplacementLoad)
                        {
                            index = System.Convert.ToInt32(
                                Adapters.Lusas.Convert.GetAdapterID(largestAttribute, 'd')) + 1;
                        }
                        else
                        {
                            index = System.Convert.ToInt32(
                                Adapters.Lusas.Convert.GetAdapterID(largestAttribute, 'l')) + 1;
                        }
                    }
                }
                if (type == typeof(MeshSettings1D) ||
                    type == typeof(MeshSettings2D))
                {
                    int largestThicknessID = d_LusasData.getLargestAttributeID("Mesh");
                    if (largestThicknessID == 0)
                    {
                        index = 1;
                    }
                    else
                    {
                        IFAttribute largestAttribute = d_LusasData.getAttribute("Mesh", largestThicknessID);
                        index = System.Convert.ToInt32(
                            Adapters.Lusas.Convert.GetAdapterID(largestAttribute, 'e')) + 1;
                    }
                }
                m_indexDict[type] = index;
            }
            return index;
        }

        /***************************************************/
        /**** Private Fields                            ****/
        /***************************************************/

        //Change from object to the index type used by the specific software
        private Dictionary<Type, int> m_indexDict = new Dictionary<Type, int>();


        /***************************************************/

    }
}


