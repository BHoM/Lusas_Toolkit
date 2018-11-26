using System.Collections.Generic;
using System.Linq;
using BH.oM.Structure.Elements;
using BH.oM.Structure.Loads;
using Lusas.LPI;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        private List<ILoad> ReadBarTemperatureLoads(List<string> ids = null)
        {
            List<ILoad> bhomBarTemperatureLoads = new List<ILoad>();
            object[] lusasTemperatureLoads = d_LusasData.getAttributes("Temperature");

            List<Bar> bhomBars = ReadBars();
            Dictionary<string, Bar> barDictionary = bhomBars.ToDictionary(
                x => x.CustomData[AdapterId].ToString());

            List<IFLoadcase> allLoadcases = new List<IFLoadcase>();

            for (int i = 0; i < lusasTemperatureLoads.Count(); i++)
            {
                IFLoading lusasTemperatureLoad = (IFLoading)lusasTemperatureLoads[i];

                IEnumerable<IGrouping<string, IFAssignment>> groupedByLoadcases =
                    GetLoadAssignments(lusasTemperatureLoad);

                foreach (IEnumerable<IFAssignment> groupedAssignment in groupedByLoadcases)
                {
                    List<IFAssignment> assignments = new List<IFAssignment>();

                    foreach (IFAssignment assignment in groupedAssignment)
                    {
                        IFLine tryLine = assignment.getDatabaseObject() as IFLine;

                        if (tryLine != null)
                        {
                            assignments.Add(assignment);
                        }
                    }

                    List<string> analysisName = new List<string> { lusasTemperatureLoad.getAttributeType() };

                    if (assignments.Count != 0)
                    {
                        BarTemperatureLoad bhomBarTemperatureLoad =
                            Engine.Lusas.Convert.ToBarTemperatureLoad(
                                lusasTemperatureLoad, groupedAssignment, barDictionary);

                        bhomBarTemperatureLoad.Tags = new HashSet<string>(analysisName);
                        bhomBarTemperatureLoads.Add(bhomBarTemperatureLoad);
                    }
                }
            }

            return bhomBarTemperatureLoads;
        }
    }
}
