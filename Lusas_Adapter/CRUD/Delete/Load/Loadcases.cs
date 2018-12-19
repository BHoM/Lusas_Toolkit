using System.Collections.Generic;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        public int DeleteLoadcases(IEnumerable<object> ids)
        {
            int success = 1;

            if (ids != null)
            {
                //Engine.Reflection.Compute.RecordError("The deleting of individual Loadcases is not supported in the Lusas_Toolkit");

                return 0;
            }
            else
            {
                d_LusasData.deleteLoadsets("loadcase","all");
            }
            return success;
        }
    }
}
