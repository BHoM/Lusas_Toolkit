using System.Collections.Generic;
using System.Linq;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        public int DeleteAreaUnformlyDistributedLoads(IEnumerable<object> ids)
        {
            int success = 1;

            if (ids != null)
            {
                //Engine.Reflection.Compute.RecordError("The deleting of individual AreaUniformlyDistributedLoad objects is not supported in the Lusas_Toolkit");

                return 0;
            }
            else
            {
                object[] lusasGlobalDistributedLoads = d_LusasData.getAttributes("Global Distributed Load");
                object[] lusasLocalDistributedLoads = d_LusasData.getAttributes("Distributed Load");

                object[] lusasDistributedLoads = lusasGlobalDistributedLoads.Concat(
                    lusasLocalDistributedLoads).ToArray();

                DeleteSurfaceAssignments(lusasDistributedLoads);
            }
            return success;
        }
    }
}