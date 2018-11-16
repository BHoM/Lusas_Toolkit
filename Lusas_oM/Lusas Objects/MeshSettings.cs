using BH.oM.Base;
using BH.oM.Adapter.Lusas;

namespace BH.oM.Adapter.Lusas
{
    public class MeshSettings1D : BHoMObject
    {
        /***************************************************/
        /****            Public Properties              ****/
        /***************************************************/

        public Split SplitMethod { get; set; }
        public ElementType1D ElementType1D { get; set; }
        public double SplitParameter { get; set; }

        /***************************************************/
    }

    //public class MeshSettings2D : BHoMObject
    //{
    //    /***************************************************/
    //    /****            Public Properties              ****/
    //    /***************************************************/

    //    public MeshType MeshType { get; set; }
    //    public Split SplitMethod { get; set; }
    //    public double Value { get; set; }

    //    /***************************************************/
    //}
}