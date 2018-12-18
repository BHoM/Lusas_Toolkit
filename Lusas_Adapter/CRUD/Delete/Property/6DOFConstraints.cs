using System.Collections.Generic;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        public int DeleteConstraint6DOF(IEnumerable<object> ids)
        {
            int success = 1;

            if (ids != null)
            {
                Engine.Reflection.Compute.RecordError("The deleting of individual Constraint6DOF objects is not supported in the Lusas_Toolkit");

                return 0;
            }
            else
            {
                object[] lusasSupports = d_LusasData.getAttributes("Support");
                DeletePointAssignments(lusasSupports);
            }
            return success;
        }
    }
}