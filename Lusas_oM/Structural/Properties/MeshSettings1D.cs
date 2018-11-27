using BH.oM.Base;

namespace BH.oM.Adapters.Lusas
{
    public class MeshSettings1D : BHoMObject
    {
        /***************************************************/
        /****            Public Properties              ****/
        /***************************************************/

        public Split1D SplitMethod { get; set; }
        public double SplitParameter { get; set; }

        /***************************************************/
    }
}