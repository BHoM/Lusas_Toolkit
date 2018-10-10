using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.oM.Base;
using BH.oM.Geometry;
using BH.oM.Structure.Elements;
using BH.oM.Structure.Properties;
using BH.oM.Common.Materials;
using Lusas.LPI;

namespace BH.Engine.Lusas
{
    public static partial class Convert
    {
        public static Node ToBHoMNode(this IFPoint lusasPoint, HashSet<string> groupNames, Dictionary<string, Constraint6DOF> constraints6DOF)
        {
            HashSet<string> tags = new HashSet<string>(IsMemberOf(lusasPoint, groupNames));

            List<string> supportAssignments = AttributeAssignments(lusasPoint, "Support");

            Constraint6DOF nodeConstraint = null;
            if (!(supportAssignments.Count() == 0))
            {
                constraints6DOF.TryGetValue(supportAssignments[0], out nodeConstraint);
            }

            Node bhomNode = new Node
            {
                Position = { X = lusasPoint.getX(),
                Y = lusasPoint.getY(),
                Z = lusasPoint.getZ() },
                Tags = tags,
                Constraint = nodeConstraint
            };

            string pointName = removePrefix(lusasPoint.getName(), "P");

            bhomNode.CustomData["Lusas_id"] = pointName;

            return bhomNode;
        }

    }
}

