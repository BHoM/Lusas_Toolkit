using BH.oM.Structure.Elements;
using BH.oM.Geometry;
using Lusas.LPI;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {

        public IFPoint CreatePoint(Node node)
        {
            IFPoint lusasPoint;
            IFDatabaseOperations database_point = d_LusasData.createPoint(
                node.Position.X, node.Position.Y, node.Position.Z);

            lusasPoint = d_LusasData.getPointByNumber(d_LusasData.getLargestPointID());
            lusasPoint.setName("P" + node.CustomData[AdapterId].ToString());

            if (!(node.Tags.Count == 0))
            {
                AssignObjectSet(lusasPoint, node.Tags);
            }

            if (!(node.Constraint == null))
            {
                string constraintName = "Sp" + node.Constraint.CustomData[AdapterId] + "/" + node.Constraint.Name;
                IFAttribute lusasSupport = d_LusasData.getAttribute("Support", constraintName);
                lusasSupport.assignTo(lusasPoint);
            }

            return lusasPoint;
        }

        public IFPoint CreatePoint(Point point)
        {
            Node newNode = new Node { Position = point };

            int adapterID;
            if (newNode.CustomData.ContainsKey(AdapterId))
               adapterID= System.Convert.ToInt32(newNode.CustomData[AdapterId]);
            else
               adapterID= System.Convert.ToInt32(NextId(newNode.GetType()));

            newNode.CustomData[AdapterId] = adapterID;

            IFPoint newPoint = CreatePoint(newNode);

            return newPoint;
        }

    }
}
