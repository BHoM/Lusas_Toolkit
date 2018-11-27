using BH.oM.Base;

namespace BH.oM.Adapters.Lusas
{
    public class MeshSettings2D : BHoMObject
    {
        /***************************************************/
        /****            Public Properties              ****/
        /***************************************************/

        public Split2D SplitMethod { get; set; }
        public int xDivisions { get; set; }
        public int yDivisions { get; set; }
        public double ElementSize { get; set; }

        /***************************************************/
    }
}