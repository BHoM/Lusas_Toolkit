using System.Collections.Generic;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        public int DeleteSectionProperties(IEnumerable<object> ids)
        {
            int success = 1;

            if (ids != null)
            {
                Engine.Reflection.Compute.RecordError("The deleting of individual Section Properties is not supported in the Lusas_Toolkit" );

                return 0;
            }
            else
            {
                d_LusasData.deleteAttributes("Line Geometric");
            }
            return success;
        }
    }
}
