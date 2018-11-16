using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.oM.Adapter.Lusas;


namespace BH.Engine.Lusas.Create
{
    public static partial class Create
    {
        /***************************************************/
        /****           Public Constructors             ****/
        /***************************************************/

        public static MeshSettings1D MeshSettings1D(MeshType meshType, ElementType1D elementType1D, Split splitMethod, double divider)
        {
            return new MeshSettings1D
            {
                MeshType = meshType,
                ElementType1D = elementType1D,
                SplitMethod = splitMethod,
                NumberDivisions = divider
            };
        }


        public static MeshSettings2D MeshSettings2D(MeshType meshType, Split splitMethod, double divider)
        {
            return new MeshSettings2D
            {
                MeshType = meshType,
                SplitMethod = splitMethod,
                NumberDivisions = divider
            };
        }
    }
}
