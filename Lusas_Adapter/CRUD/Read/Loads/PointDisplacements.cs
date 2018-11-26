using System.Collections.Generic;
using System.Linq;
using BH.oM.Structure.Elements;
using BH.oM.Structure.Loads;
using Lusas.LPI;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        private List<ILoad> ReadPointDisplacements(List<string> ids = null)
        {
            List<ILoad> bhomPointDisplacements = new List<ILoad>();
            object[] lusasPrescribedDisplacements = d_LusasData.getAttributes("Prescribed Load");

            List<Node> bhomNodes = ReadNodes();
            Dictionary<string, Node> nodeDictionary = bhomNodes.ToDictionary(
                x => x.CustomData[AdapterId].ToString());

            List<IFLoadcase> allLoadcases = new List<IFLoadcase>();

            for (int i = 0; i < lusasPrescribedDisplacements.Count(); i++)
            {
                IFLoading lusasPrescribedDisplacement = (IFLoading)lusasPrescribedDisplacements[i];

                IEnumerable<IGrouping<string, IFAssignment>> groupedByLoadcases =
                    GetLoadAssignments(lusasPrescribedDisplacement);

                foreach (IEnumerable<IFAssignment> groupedAssignment in groupedByLoadcases)
                {
                    PointDisplacement bhomPointDisplacement =
                        Engine.Lusas.Convert.ToPointDisplacement(
                            lusasPrescribedDisplacement, groupedAssignment, nodeDictionary);

                    List<string> analysisName = new List<string> { lusasPrescribedDisplacement.getAttributeType() };
                    bhomPointDisplacement.Tags = new HashSet<string>(analysisName);
                    bhomPointDisplacements.Add(bhomPointDisplacement);
                }
            }

            return bhomPointDisplacements;
        }
    }
}