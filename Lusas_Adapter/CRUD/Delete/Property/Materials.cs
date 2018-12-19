using System.Collections.Generic;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        public int DeleteMaterials(IEnumerable<object> ids)
        {
            int success = 1;

            if (ids != null)
            {
                //Engine.Reflection.Compute.RecordError("The deleting of individual Material objects is not supported in the Lusas_Toolkit");

                return 0;
            }
            else
            {
                d_LusasData.deleteAttributes("Material");
            }
            return success;
        }
    }
}
