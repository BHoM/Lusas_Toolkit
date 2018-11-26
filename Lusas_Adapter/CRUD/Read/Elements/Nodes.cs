using System.Collections.Generic;
using System.Linq;
using BH.oM.Structure.Elements;
using BH.oM.Structure.Properties;
using Lusas.LPI;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        private List<Node> ReadNodes(List<string> ids = null)
        {
            object[] lusasPoints = d_LusasData.getObjects("Point");

            List<Node> bhomNodes = new List<Node>();
            HashSet<string> groupNames = ReadTags();

            IEnumerable<Constraint6DOF> constraints6DOFList = Read6DOFConstraints();
            Dictionary<string, Constraint6DOF> constraints6DOF = constraints6DOFList.ToDictionary(
                x => x.Name.ToString());

            for (int i = 0; i < lusasPoints.Count(); i++)
            {
                IFPoint lusasPoint = (IFPoint)lusasPoints[i];
                Node bhomNode = Engine.Lusas.Convert.ToBHoMNode(lusasPoint, groupNames, constraints6DOF);
                bhomNodes.Add(bhomNode);
            }
            return bhomNodes;
        }
    }
}