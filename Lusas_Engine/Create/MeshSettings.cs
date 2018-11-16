using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.oM.Adapter.Lusas;


namespace BH.Engine.Adapters.Lusas
{
    public static partial class Create
    {
        /***************************************************/
        /****           Public Constructors             ****/
        /***************************************************/

        public static MeshSettings1D MeshSettings1D(ElementType1D elementType1D, Split splitMethod, double divider =0, string name = null)
        {
            return new MeshSettings1D
            {
                SplitMethod = splitMethod,
                ElementType1D = elementType1D,
                SplitParameter = divider,
                Name = name
            };


            //};
        }


        //public static MeshSettings2D MeshSettings2D(ElementType2D elementType1D = ElementType1D.BarL, Split splitMethod = Split.Divisions, double divider = 0)
        //{
        //    return new MeshSettings2D
        //    {
        //        ElementType2D = elementType1D,
        //        SplitMethod = splitMethod,
        //        Value = divider
        //    };
        //}
    }
}
