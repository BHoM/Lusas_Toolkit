using System.Collections.Generic;
using BH.oM.Structure.Elements;
using Lusas.LPI;

namespace BH.Engine.Lusas
{
    public partial class Convert
    {
        public static Node GetNode(IFLine lusasLine, int nodeIndex, Dictionary<string, Node> bhomNodes)
        {
            Node bhomNode = null;
            IFPoint lusasPoint = lusasLine.getLOFs()[nodeIndex];
            string pointName = RemovePrefix(lusasPoint.getName(), "P");
            bhomNodes.TryGetValue(pointName, out bhomNode);

            return bhomNode;
        }

        public static Bar GetBar(IFSurface lusasSurf, int lineIndex, Dictionary<string, Bar> bhomBars)
        {
            Bar bhomBar = null;
            IFLine lusasEdge = lusasSurf.getLOFs()[lineIndex];
            string lineName = RemovePrefix(lusasEdge.getName(), "L");
            bhomBars.TryGetValue(lineName, out bhomBar);
            return bhomBar;
        }

        public static Edge GetEdge(IFSurface lusasSurf, int lineIndex, Dictionary<string, Edge> bhomBars)
        {
            Edge bhomEdge = null;
            IFLine lusasEdge = lusasSurf.getLOFs()[lineIndex];
            string lineName = RemovePrefix(lusasEdge.getName(), "L");
            bhomBars.TryGetValue(lineName, out bhomEdge);
            return bhomEdge;
        }
    }
}