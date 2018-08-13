using System;
using System.Collections.Generic;
using BH.oM.Common.Materials;
using BH.oM.Structural.Properties;
using BH.oM.Structural.Elements;
using BH.oM.Structural.Loads;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LusasM15_2;

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
            int index =1;

            if (!refresh && m_indexDict.TryGetValue(type, out index))
            {
                index++;
                m_indexDict[type] = index;
            }
            else
            {
                if (type == typeof(Node))
                {
                    index = d_LusasData.getLargestPointID()+1;
                }
                if (type == typeof(Bar))
                {
                    index = d_LusasData.getLargestLineID() + 1;
                }
                if (type == typeof(PanelPlanar))
                {
                    index = d_LusasData.getLargestSurfaceID() + 1;
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
