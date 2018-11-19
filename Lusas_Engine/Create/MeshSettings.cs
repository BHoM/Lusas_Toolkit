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

        public static MeshSettings1D MeshSettings1D(ElementType1D ElementType, Split SplitMethod, double SplitParameter =4, List<int> StartReleases = null, List<int> EndReleases = null, string name = null)
        {
            return new MeshSettings1D
            {
                SplitMethod = SplitMethod,
                ElementType1D = ElementType,
                SplitParameter = SplitParameter,
                StartReleases = StartReleases,
                EndReleases = EndReleases,
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
