using BH.oM.Adapters.Lusas;


namespace BH.Engine.Adapters.Lusas
{
    public static partial class Create
    {
        /***************************************************/
        /****           Public Constructors             ****/
        /***************************************************/

        public static MeshSettings1D MeshSettings1D(Split1D SplitMethod, double SplitParameter =4, string name = null)
        {
            return new MeshSettings1D
            {
                SplitMethod = SplitMethod,
                SplitParameter = SplitParameter,
                Name = name
            };
        }


        public static MeshSettings2D MeshSettings2D(Split2D splitMethod, int xDivisions = 0, int yDivisions = 0, double size = 1, string name = null)
        {
            return new MeshSettings2D
            {
                SplitMethod = splitMethod,
                xDivisions = xDivisions,
                yDivisions = yDivisions,
                ElementSize = size,
                Name = name
            };
        }
    }
}
