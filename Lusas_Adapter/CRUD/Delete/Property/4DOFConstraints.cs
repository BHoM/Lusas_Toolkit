using System.Collections.Generic;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        public int DeleteConstraint4DOF(IEnumerable<object> ids)
        {
            int success = 1;

            if (ids != null)
            {
                Engine.Reflection.Compute.RecordError("The deleting of individual Constraint4DOF objects is not supported in the Lusas_Toolkit");

                return 0;
            }
            else
            {
                object[] lusasSupports = d_LusasData.getAttributes("Support");
                DeleteLineAssignments(lusasSupports);
            }
            return success;
        }
    }
}