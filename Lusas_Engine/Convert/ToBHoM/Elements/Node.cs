using System.Collections.Generic;
using System.Linq;
using BH.oM.Structure.Elements;
using BH.oM.Structure.Properties.Constraint;
using Lusas.LPI;

namespace BH.Engine.Lusas
{
    public static partial class Convert
    {
        public static Node ToBHoMNode(this IFPoint lusasPoint,
            HashSet<string> groupNames, Dictionary<string, Constraint6DOF> bhom6DOFConstraints)
        {
            HashSet<string> tags = new HashSet<string>(IsMemberOf(lusasPoint, groupNames));
            List<string> supportAssignments = AttributeAssignments(lusasPoint, "Support");

            Constraint6DOF nodeConstraint = null;
            if (!(supportAssignments.Count() == 0))
            {
                bhom6DOFConstraints.TryGetValue(supportAssignments[0], out nodeConstraint);
            }

            Node bhomNode = new Node
            {
                Position = { X = lusasPoint.getX(), Y = lusasPoint.getY(), Z = lusasPoint.getZ() },
                Tags = tags,
                Constraint = nodeConstraint
            };

            string adapterID = RemovePrefix(lusasPoint.getName(), "P");
            bhomNode.CustomData["Lusas_id"] = adapterID;

            return bhomNode;
        }

    }
}

