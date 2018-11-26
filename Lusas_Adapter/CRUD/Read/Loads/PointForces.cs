using System.Collections.Generic;
using System.Linq;
using BH.oM.Structure.Elements;
using BH.oM.Structure.Loads;
using Lusas.LPI;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        private List<ILoad> ReadPointForces(List<string> ids = null)
        {
            List<ILoad> bhomPointForces = new List<ILoad>();
            object[] lusasConcentratedLoads = d_LusasData.getAttributes("Concentrated Load");

            List<Node> bhomNodes = ReadNodes();
            Dictionary<string, Node> nodeDictionary = bhomNodes.ToDictionary(
                x => x.CustomData[AdapterId].ToString());

            List<IFLoadcase> allLoadcases = new List<IFLoadcase>();

            for (int i = 0; i < lusasConcentratedLoads.Count(); i++)
            {
                IFLoading lusasConcentratedLoad = (IFLoading)lusasConcentratedLoads[i];

                IEnumerable<IGrouping<string, IFAssignment>> groupedByLoadcases =
                    GetLoadAssignments(lusasConcentratedLoad);

                foreach (IEnumerable<IFAssignment> groupedAssignment in groupedByLoadcases)
                {
                    PointForce bhomPointForce = Engine.Lusas.Convert.ToPointForce(
                        lusasConcentratedLoad, groupedAssignment, nodeDictionary
                        );
                    List<string> analysisName = new List<string> { lusasConcentratedLoad.getAttributeType() };
                    bhomPointForce.Tags = new HashSet<string>(analysisName);
                    bhomPointForces.Add(bhomPointForce);
                }
            }

            return bhomPointForces;
        }
    }
}