using System.Collections.Generic;
using System.Linq;
using Lusas.LPI;

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

                for (int i = 0; i < lusasSupports.Count(); i++)
                {
                    IFSupportStructural lusasSupport = (IFSupportStructural)lusasSupports[i];
                    object[] lusasAssignments = lusasSupport.getAssignments();
                    for (int j = 0; j < lusasAssignments.Count(); j++)
                    {
                        IFAssignment lusasAssignment = (IFAssignment)lusasAssignments[j];
                        IFGeometry lusasGeometry = (IFGeometry)lusasAssignment.getDatabaseObject();
                        if (lusasGeometry is IFPoint)
                        {
                            d_LusasData.Delete(lusasSupport);
                            Engine.Reflection.Compute.RecordWarning(lusasSupport.getName() + " has been deleted because it was assigned to a point");
                        }
                        
                    }

                }
            }
            return success;
        }
    }
}