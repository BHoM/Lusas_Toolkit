using System.Collections.Generic;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        public int DeleteLoadCombinations(IEnumerable<object> ids)
        {
            int success = 1;

            if (ids != null)
            {
                Engine.Reflection.Compute.RecordError("The deleting of individual LoadCombinations is not supported in the Lusas_Toolkit");

                return 0;
            }
            else
            {
                d_LusasData.deleteLoadsets("Combinations");
            }
            return success;
        }
    }
}
