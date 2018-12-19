using System.Linq;
using Lusas.LPI;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        public void DeletePointAssignments(object[] lusasAttributes)
        {
            for (int i = 0; i < lusasAttributes.Count(); i++)
            {
                IFAttribute lusasAttribute = (IFAttribute)lusasAttributes[i];
                object[] lusasAssignments = lusasAttribute.getAssignments();
                for (int j = 0; j < lusasAssignments.Count(); j++)
                {
                    IFAssignment lusasAssignment = (IFAssignment)lusasAssignments[j];
                    IFGeometry lusasGeometry = (IFGeometry)lusasAssignment.getDatabaseObject();
                    if (lusasGeometry is IFPoint)
                    {
                        Engine.Reflection.Compute.RecordWarning(lusasAttribute.getName() + " has been deleted because it was assigned to a point");
                        d_LusasData.Delete(lusasAttribute);
                        break;
                    }
                }
            }
        }
    }
}