using System.Collections.Generic;
using System.Linq;
using BH.oM.Structure.Elements;
using BH.oM.Structure.Loads;
using Lusas.LPI;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        private List<ILoad> ReadBarUniformlyDistributedLoads(List<string> ids = null)
        {
            List<ILoad> bhomBarUniformlyDistributedLoads = new List<ILoad>();
            object[] lusasGlobalDistributedLoads = d_LusasData.getAttributes("Global Distributed Load");
            object[] lusasLocalDistributedLoads = d_LusasData.getAttributes("Distributed Load");

            object[] lusasDistributedLoads = lusasGlobalDistributedLoads.Concat(
                lusasLocalDistributedLoads).ToArray();

            List<Bar> bhomBars = ReadBars();
            Dictionary<string, Bar> barDictionary = bhomBars.ToDictionary(
                x => x.CustomData[AdapterId].ToString());

            List<IFLoadcase> allLoadcases = new List<IFLoadcase>();

            for (int i = 0; i < lusasDistributedLoads.Count(); i++)
            {
                IFLoading lusasDistributedLoad = (IFLoading)lusasDistributedLoads[i];

                IEnumerable<IGrouping<string, IFAssignment>> groupedByLoadcases = GetLoadAssignments(
                    lusasDistributedLoad);

                if (lusasDistributedLoad.getValue("type") == "Length")
                {
                    foreach (IEnumerable<IFAssignment> groupedAssignment in groupedByLoadcases)
                    {
                        BarUniformlyDistributedLoad bhomBarUniformlyDistributedLoad =
                            Engine.Lusas.Convert.ToBarUniformallyDistributed(
                                lusasDistributedLoad, groupedAssignment, barDictionary);

                        List<string> analysisName = new List<string> { lusasDistributedLoad.getAttributeType() };
                        bhomBarUniformlyDistributedLoad.Tags = new HashSet<string>(analysisName);
                        bhomBarUniformlyDistributedLoads.Add(bhomBarUniformlyDistributedLoad);
                    }
                }
            }

            return bhomBarUniformlyDistributedLoads;
        }
    }
}