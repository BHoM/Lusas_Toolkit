using System.Collections.Generic;
using System.Linq;
using System;
using Lusas.LPI;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        public void AssignObjectSet(IFGeometry newGeometry, HashSet<string> tags)
        {
            foreach (string tag in tags)
            {
                IFObjectSet objectSet = d_LusasData.getGroupByName(tag);
                objectSet.add(newGeometry);
            }
        }

        public static IEnumerable<IGrouping<string, IFAssignment>> GetLoadAssignments(IFLoading lusasForce)
        {
            object[] assignmentObjects = lusasForce.getAssignments();
            List<IFAssignment> assignments = new List<IFAssignment>();

            for (int j = 0; j < assignmentObjects.Count(); j++)
            {
                IFAssignment assignment = (IFAssignment)assignmentObjects[j];
                assignments.Add(assignment);
            }

            IEnumerable<IGrouping<string, IFAssignment>> groupedByLoadcases = assignments.GroupBy(m => m.getAssignmentLoadset().getName());

            return groupedByLoadcases;
        }

        public void NameSearch(string prefix, string customID, string suffix, ref string name)
        {
            for (int i = 1; i < Int32.Parse(customID); i++)
            {
                name = prefix + i.ToString() + "/" + suffix;
                if (d_LusasData.existsAttribute("Loading", name))
                {
                    break;
                }
            }
        }

    }
}
