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

        public static MeshSettings1D MeshSettings1D(Split1D SplitMethod, ElementType1D ElementType, double SplitParameter =4, List<int> StartReleases = null, List<int> EndReleases = null, string name = null)
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
        }


        public static MeshSettings2D MeshSettings2D(Split2D splitMethod, ElementType2D elementType2D, int xDivisions = 0, int yDivisions = 0, double size = 1, string name = null)
        {
            return new MeshSettings2D
            {
                SplitMethod = splitMethod,
                ElementType2D = elementType2D,
                xDivisions = xDivisions,
                yDivisions = yDivisions,
                ElementSize = size,
                Name = name
            };
        }
    }
}
