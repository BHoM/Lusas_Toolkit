using BH.oM.Base;
using BH.oM.Adapters.Lusas;
using System;
using System.Collections.Generic;

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