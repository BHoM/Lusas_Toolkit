using System.Collections.Generic;
using System.Linq;
using System;
using BH.oM.Base;
using BH.oM.Structure.Elements;
using BH.oM.Geometry;
using BH.oM.Structure.Properties;
using BH.oM.Structure.Loads;
using BH.oM.Common.Materials;
using BH.Engine.Geometry;
using BH.Engine.Structure;
using Lusas.LPI;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {

        public IFPoint CreatePoint(Node node)
        {
                IFPoint newPoint;
                IFDatabaseOperations database_point = d_LusasData.createPoint(node.Position.X, node.Position.Y, node.Position.Z);
                newPoint = d_LusasData.getPointByNumber(d_LusasData.getLargestPointID());
                newPoint.setName("P" + node.CustomData[AdapterId].ToString());

            if (!(node.Tags.Count == 0))
            {
                AssignObjectSet(newPoint, node.Tags);
            }

            if (!(node.Constraint == null))
            {
                string constraintName = "Sp" + node.Constraint.CustomData[AdapterId] + "/" + node.Constraint.Name;
                IFAttribute lusasSupport = d_LusasData.getAttribute("Support", constraintName);
                lusasSupport.assignTo(newPoint);
            }

            return newPoint;
        }

        public IFPoint CreatePoint(Point point)
        {
            Node newNode = new Node { Position = point };

            int bhomID;
            if (newNode.CustomData.ContainsKey(AdapterId))
                bhomID = System.Convert.ToInt32(newNode.CustomData[AdapterId]);
            else
                bhomID = System.Convert.ToInt32(NextId(newNode.GetType()));

            newNode.CustomData[AdapterId] = bhomID;

            IFPoint newPoint = CreatePoint(newNode);

            return newPoint;
        }

    }
}
