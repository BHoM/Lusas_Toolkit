using System.Collections.Generic;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        public int DeletePointDisplacements (IEnumerable<object> ids)
        {
            int success = 1;

            if (ids != null)
            {
                //Engine.Reflection.Compute.RecordError("The deleting of individual PointDisplacement objects is not supported in the Lusas_Toolkit");

                return 0;
            }
            else
            {
                d_LusasData.deleteAttributes("Prescribed Load");
                object[] lusasPrescribedDisplacements = d_LusasData.getAttributes("Prescribed Load");
                DeletePointAssignments(lusasPrescribedDisplacements);
            }
            return success;
        }
    }
}
