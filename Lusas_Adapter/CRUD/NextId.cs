﻿using System;
using System.Collections.Generic;
using BH.oM.Common.Materials;
using BH.oM.Structure.Properties;
using BH.oM.Structure.Elements;
using BH.oM.Structure.Loads;
using System.Linq;
using BH.oM.Geometry;
using Lusas.LPI;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        /***************************************************/
        /**** Adapter overload method                   ****/
        /***************************************************/

        protected override object NextId(Type type, bool refresh = false)
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
                               BH.Engine.Lusas.Convert.removePrefix(largestPoint.getName(), "P")) + 1;
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
                               BH.Engine.Lusas.Convert.removePrefix(largestLine.getName(), "L")) + 1;
                    }
                }
                if (type == typeof(PanelPlanar))
                {
                    if (d_LusasData.getLargestSurfaceID() == 0)
                    {
                        index = 1;
                    }
                    else
                    {

                        IFSurface largestSurface = d_LusasData.getSurfaceByNumber(d_LusasData.getLargestSurfaceID());
                        index = System.Convert.ToInt32(
                               BH.Engine.Lusas.Convert.removePrefix(largestSurface.getName(), "S")) + 1;
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
                               BH.Engine.Lusas.Convert.removePrefix(largestLine.getName(), "L")) + 1;
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
                               BH.Engine.Lusas.Convert.removePrefix(largestPoint.getName(), "P")) + 1;
                    }
                }
                if (type == typeof(Loadcase))
                {
                    int largestLoadcaseID = d_LusasData.getNextAvailableLoadcaseID() - 1;
                    if (largestLoadcaseID == 0)
                    {
                        index = 1;
                    }
                    else
                    {

                        IFLoadcase largestLoadcase = (IFLoadcase)d_LusasData.getLoadset(largestLoadcaseID);
                        index = System.Convert.ToInt32(BH.Engine.Lusas.Convert.GetBHoMID(largestLoadcase, 'c')) + 1;
                    }
                }
                if (type == typeof(Material))
                {
                    int largestMaterialID = d_LusasData.getLargestAttributeID("Material");
                    if (largestMaterialID == 0)
                    {
                        index = 1;
                    }
                    else
                    {

                        IFAttribute largestAttribute = d_LusasData.getAttribute("Material", largestMaterialID);
                        index = System.Convert.ToInt32(BH.Engine.Lusas.Convert.GetBHoMID(largestAttribute, 'M')) + 1;
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
                        index = System.Convert.ToInt32(BH.Engine.Lusas.Convert.GetBHoMID(largestAttribute, 'p')) + 1;
                    }
                }
                if (type == typeof(ConstantThickness) ||
                    type == typeof(SteelSection))
                {
                    int largestThicknessID = d_LusasData.getLargestAttributeID("Geometric");
                    if (largestThicknessID == 0)
                    {
                        index = 1;
                    }
                    else
                    {
                        IFAttribute largestAttribute = d_LusasData.getAttribute("Geometric", largestThicknessID);
                        index = System.Convert.ToInt32(BH.Engine.Lusas.Convert.GetBHoMID(largestAttribute, 'G')) + 1;
                    }
                }
                if (type == typeof(PointForce) ||
                    type == typeof(GravityLoad) ||
                    type == typeof(BarUniformlyDistributedLoad) ||
                    type == typeof(AreaUniformalyDistributedLoad) ||
                    type == typeof(BarTemperatureLoad) ||
                    type == typeof(AreaTemperatureLoad) ||
                    type == typeof(BarPointLoad) ||
                    type == typeof(BarVaryingDistributedLoad) ||
                    type == typeof(PointDisplacement))
                {
                    int largestLoadID = d_LusasData.getLargestAttributeID("Loading");
                    if (largestLoadID == 0)
                    {
                        index = 1;
                    }
                    else
                    {

                        IFAttribute largestAttribute = d_LusasData.getAttribute("Loading", largestLoadID);
                        index = System.Convert.ToInt32(BH.Engine.Lusas.Convert.GetBHoMID(largestAttribute, 'l')) + 1;
                    }
                }
                if (type == typeof(LoadCombination))
                {
                    object[] combinationObjects = d_LusasData.getLoadsets("Combinations");
                    if (combinationObjects.Count() == 0)
                    {
                       index = 1;
                    }
                    else
                    {
                        List<IFBasicCombination> loadCombinations = new List<IFBasicCombination>();
                        for (int i = 0; i < loadCombinations.Count(); i++)
                        {
                            IFBasicCombination loadCombination = (IFBasicCombination)combinationObjects[i];
                            loadCombinations.Add(loadCombination);
                        }

                        int largestLoadCombinationID = loadCombinations.Max(x => x.getID());

                        IFBasicCombination largestLoadCombination = (IFBasicCombination)d_LusasData.getLoadset("Combinations", largestLoadCombinationID);
                        index = System.Convert.ToInt32(BH.Engine.Lusas.Convert.GetBHoMID(largestLoadCombination, 'l')) + 1;
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

