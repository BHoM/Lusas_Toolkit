using System.Collections.Generic;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        public int DeleteAreaTemperatureLoad(IEnumerable<object> ids)
        {
            int success = 1;

            if (ids != null)
            {
                Engine.Reflection.Compute.RecordError("The deleting of individual AreaTemperatureLoad objects is not supported in the Lusas_Toolkit");

                return 0;
            }
            else
            {
                object[] lusasTemperatureLoads = d_LusasData.getAttributes("Temperature");
                DeleteSurfaceAssignments(lusasTemperatureLoads);
            }
            return success;
        }
    }
}