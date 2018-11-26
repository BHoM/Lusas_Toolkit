using System.Collections.Generic;
using System.Linq;
using BH.oM.Structure.Elements;
using BH.oM.Structure.Loads;
using Lusas.LPI;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        private List<ILoad> ReadAreaUniformlyDistributedLoads(List<string> ids = null)
        {
            List<ILoad> bhomPanelUniformlyDistributedLoads = new List<ILoad>();

            object[] lusasGlobalDistributedLoads = d_LusasData.getAttributes("Global Distributed Load");
            object[] lusasLocalDistributedLoads = d_LusasData.getAttributes("Distributed Load");


            object[] lusasDistributedLoads = lusasGlobalDistributedLoads.Concat(
                lusasLocalDistributedLoads).ToArray();

            List<PanelPlanar> bhomSurfaces = ReadPlanarPanels();
            Dictionary<string, PanelPlanar> surfaceDictionary = bhomSurfaces.ToDictionary(
                x => x.CustomData[AdapterId].ToString());

            List<IFLoadcase> allLoadcases = new List<IFLoadcase>();

            for (int i = 0; i < lusasDistributedLoads.Count(); i++)
            {
                IFLoading lusasDistributedLoad = (IFLoading)lusasDistributedLoads[i];

                if (lusasDistributedLoad.getValue("type") == "Area")
                {
                    IEnumerable<IGrouping<string, IFAssignment>> groupedByLoadcases =
                        GetLoadAssignments(lusasDistributedLoad);

                    foreach (IEnumerable<IFAssignment> groupedAssignment in groupedByLoadcases)
                    {
                        AreaUniformalyDistributedLoad bhomBarUniformlyDistributedLoad =
                            Engine.Lusas.Convert.ToAreaUniformallyDistributed(
                                lusasDistributedLoad, groupedAssignment, surfaceDictionary);

                        List<string> analysisName = new List<string> { lusasDistributedLoad.getAttributeType() };
                        bhomBarUniformlyDistributedLoad.Tags = new HashSet<string>(analysisName);
                        bhomPanelUniformlyDistributedLoads.Add(bhomBarUniformlyDistributedLoad);
                    }
                }
            }

            return bhomPanelUniformlyDistributedLoads;
        }
    }
}