using BH.oM.Structure.Elements;
using Lusas.LPI;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        public IFLine CreateEdge(Edge edge, IFPoint startPoint, IFPoint endPoint)
        {
            IFLine lusasLine = d_LusasData.createLineByPoints(startPoint, endPoint);
            lusasLine.setName("L" + edge.CustomData[AdapterId].ToString());

            if (!(edge.Tags.Count == 0))
            {
                AssignObjectSet(lusasLine, edge.Tags);
            }

            if (!(edge.Constraint == null))
            {
                string supportName = "Sp" + edge.Constraint.CustomData[AdapterId] + "/" + edge.Constraint.Name;
                IFAttribute lusasSupport = d_LusasData.getAttribute("Support", supportName);
                lusasSupport.assignTo(lusasLine);
            }

            return lusasLine;
        }
    }


}
