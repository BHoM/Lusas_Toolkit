using System;
using System.Collections.Generic;
using BH.oM.Common.Materials;
using BH.oM.Structure.Properties;
using BH.oM.Structure.Elements;
using BH.oM.Structure.Loads;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.oM.Geometry;
using BH.Engine.Lusas;
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
                        index = System.Convert.ToInt32(BH.Engine.Lusas.Convert.getBHoMID(largestLoadcase,'c')) + 1;
                    }
                }
                if (type == typeof(Material))
                {
                    int largestMaterialID = d_LusasData.getLargestAttributeID("Material") ;
                    if (largestMaterialID == 0)
                    {
                        index = 1;
                    }
                    else
                    {

                        IFAttribute largestAttribute = d_LusasData.getAttribute("Material",largestMaterialID);
                        index = System.Convert.ToInt32(BH.Engine.Lusas.Convert.getBHoMID(largestAttribute, 'M')) + 1;
                    }
                }
                if (type == typeof(ConstantThickness))
                {
                    int largestThicknessID = d_LusasData.getLargestAttributeID("Surface Geometric");
                    if (largestThicknessID == 0)
                    {
                        index = 1;
                    }
                    else
                    {

                        IFAttribute largestAttribute = d_LusasData.getAttribute("Surface Geometric", largestThicknessID);
                        index = System.Convert.ToInt32(BH.Engine.Lusas.Convert.getBHoMID(largestAttribute, 'G')) + 1;
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

