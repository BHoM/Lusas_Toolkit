using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.oM.Base;
using BH.oM.Geometry;
using BH.oM.Structural.Elements;
using BH.oM.Structural.Properties;
using BH.oM.Common.Materials;
using LusasM15_2;

namespace BH.Engine.Lusas
{
    public static partial class Convert
    {
        public static Node ToBHoMObject(this IFPoint lusasPoint, HashSet<String> groupNames, Dictionary<string, Constraint6DOF> constraints6DOF)
        {
            HashSet<String> tags = new HashSet<string>(isMemberOf(lusasPoint, groupNames));

            List<String> supportAssignments = attributeAssignments(lusasPoint, "Support");

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

            String pointName = removePrefix(lusasPoint.getName(), "P");

            bhomNode.CustomData["Lusas_id"] = pointName;

            return bhomNode;
        }
    }
}

