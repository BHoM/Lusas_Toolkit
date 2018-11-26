using System.Collections.Generic;
using System.Linq;
using BH.oM.Structure.Elements;
using BH.oM.Structure.Loads;
using Lusas.LPI;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        private List<ILoad> ReadBarPointLoads(List<string> ids = null)
        {
            List<ILoad> bhomBarPointLoads = new List<ILoad>();

            object[] lusasInternalBeamPointLoads = d_LusasData.getAttributes("Beam Point Load");

            if (lusasInternalBeamPointLoads.Count() != 0)
            {
                List<Bar> bhomBars = ReadBars();
                Dictionary<string, Bar> barDictionary = bhomBars.ToDictionary(
                    x => x.CustomData[AdapterId].ToString());

                List<IFLoadcase> allLoadcases = new List<IFLoadcase>();

                for (int i = 0; i < lusasInternalBeamPointLoads.Count(); i++)
                {
                    IFLoading lusasInternalBeamPointLoad = (IFLoading)lusasInternalBeamPointLoads[i];
                    IEnumerable<IGrouping<string, IFAssignment>> groupedByLoadcases =
                        GetLoadAssignments(lusasInternalBeamPointLoad);

                    foreach (IEnumerable<IFAssignment> groupedAssignment in groupedByLoadcases)
                    {
                        BarPointLoad bhomBarPointLoad =
                            Engine.Lusas.Convert.ToBHoMBarPointLoad(
                                lusasInternalBeamPointLoad, groupedAssignment, barDictionary);

                        List<string> analysisName = new List<string> { lusasInternalBeamPointLoad.getAttributeType() };
                        bhomBarPointLoad.Tags = new HashSet<string>(analysisName);
                        bhomBarPointLoads.Add(bhomBarPointLoad);
                    }
                }
            }

            return bhomBarPointLoads;
        }
    }
}