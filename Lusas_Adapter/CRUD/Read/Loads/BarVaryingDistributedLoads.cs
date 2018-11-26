using System.Collections.Generic;
using System.Linq;
using BH.oM.Structure.Elements;
using BH.oM.Structure.Loads;
using Lusas.LPI;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        private List<ILoad> ReadBarVaryingDistributedLoads(List<string> ids = null)
        {
            List<ILoad> bhomBarDistributedLoads = new List<ILoad>();

            object[] lusasInternalBeamDistributedLoads = d_LusasData.getAttributes("Beam Distributed Load");

            if (lusasInternalBeamDistributedLoads.Count() != 0)
            {
                List<Bar> bhomBars = ReadBars();
                Dictionary<string, Bar> barDictionary = bhomBars.ToDictionary(
                    x => x.CustomData[AdapterId].ToString());

                List<IFLoadcase> allLoadcases = new List<IFLoadcase>();

                for (int i = 0; i < lusasInternalBeamDistributedLoads.Count(); i++)
                {
                    IFLoading lusasInternalBeamDistributedLoad = (IFLoading)lusasInternalBeamDistributedLoads[i];
                    IEnumerable<IGrouping<string, IFAssignment>> groupedByLoadcases =
                        GetLoadAssignments(lusasInternalBeamDistributedLoad);

                    foreach (IEnumerable<IFAssignment> groupedAssignment in groupedByLoadcases)
                    {
                        BarVaryingDistributedLoad bhomBarDistributedLoad =
                            Engine.Lusas.Convert.ToBHoMBarDistributedLoad(
                                lusasInternalBeamDistributedLoad, groupedAssignment, barDictionary);

                        List<string> analysisName = new List<string> {
                            lusasInternalBeamDistributedLoad.getAttributeType() };

                        bhomBarDistributedLoad.Tags = new HashSet<string>(analysisName);
                        bhomBarDistributedLoads.Add(bhomBarDistributedLoad);
                    }
                }
            }

            return bhomBarDistributedLoads;
        }
    }
}