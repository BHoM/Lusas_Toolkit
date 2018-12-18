using System.Linq;
using Lusas.LPI;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        public void DeleteSurfaceAssignments(object[] lusasAttributes)
        {
            for (int i = 0; i < lusasAttributes.Count(); i++)
            {
                IFAttribute lusasAttribute = (IFAttribute)lusasAttributes[i];
                object[] lusasAssignments = lusasAttribute.getAssignments();
                for (int j = 0; j < lusasAssignments.Count(); j++)
                {
                    IFAssignment lusasAssignment = (IFAssignment)lusasAssignments[j];
                    IFGeometry lusasGeometry = (IFGeometry)lusasAssignment.getDatabaseObject();
                    if (lusasGeometry is IFSurface)
                    {
                        d_LusasData.Delete(lusasGeometry);
                        Engine.Reflection.Compute.RecordWarning(lusasAttribute.getName() + " has been deleted because it was assigned to a surface");
                    }
                }
            }
        }
    }
}