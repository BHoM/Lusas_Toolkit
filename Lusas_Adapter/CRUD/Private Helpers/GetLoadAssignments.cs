using System.Collections.Generic;
using System.Linq;
using Lusas.LPI;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        public static IEnumerable<IGrouping<string, IFAssignment>> GetLoadAssignments(IFLoading lusasForce)
        {
            object[] assignmentObjects = lusasForce.getAssignments();
            List<IFAssignment> assignments = new List<IFAssignment>();

            for (int j = 0; j < assignmentObjects.Count(); j++)
            {
                IFAssignment assignment = (IFAssignment)assignmentObjects[j];
                assignments.Add(assignment);
            }

            IEnumerable<IGrouping<string, IFAssignment>> groupedByLoadcases =
                assignments.GroupBy(m => m.getAssignmentLoadset().getName());

            return groupedByLoadcases;
        }
    }
}