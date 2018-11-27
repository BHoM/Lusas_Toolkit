using System.Collections.Generic;
using System.Linq;
using Lusas.LPI;
namespace BH.Engine.Lusas
{
    public partial class Convert
    {
        public static List<string> AttributeAssignments(IFGeometry lusasGeometry, string attributeType)
        {
            object[] lusasAssignments = lusasGeometry.getAssignments(attributeType);

            List<string> attributeNames = new List<string>();

            int n = lusasAssignments.Count();
            for (int i = 0; i < n; i++)
            {
                IFAssignment lusasAssignment = lusasGeometry.getAssignments(attributeType)[i];
                IFAttribute lusasAttribute = lusasAssignment.getAttribute();
                string attributeName = GetName(lusasAttribute);
                attributeNames.Add(attributeName);
            }
            return attributeNames;
        }
    }
}
