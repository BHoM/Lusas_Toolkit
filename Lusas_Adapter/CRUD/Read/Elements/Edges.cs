using System.Collections.Generic;
using System.Linq;
using BH.oM.Structure.Elements;
using Lusas.LPI;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        private List<Edge> ReadEdges(List<string> ids = null)
        {
            object[] lusasLines = d_LusasData.getObjects("Line");
            List<Edge> bhomEdges = new List<Edge>();

            if (lusasLines.Count() != 0)
            {
                List<Node> bhomNodesList = ReadNodes();
                Dictionary<string, Node> bhomNodes = bhomNodesList.ToDictionary(
                    x => x.CustomData[AdapterId].ToString());

                HashSet<string> groupNames = ReadTags();

                for (int i = 0; i < lusasLines.Count(); i++)
                {
                    IFLine lusasLine = (IFLine)lusasLines[i];
                    Edge bhomEdge = Engine.Lusas.Convert.ToBHoMEdge(lusasLine, bhomNodes, groupNames);
                    bhomEdges.Add(bhomEdge);
                }
            }

            return bhomEdges;
        }
    }
}