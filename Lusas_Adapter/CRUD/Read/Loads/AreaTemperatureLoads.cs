using System.Collections.Generic;
using System.Linq;
using BH.oM.Structure.Elements;
using BH.oM.Structure.Loads;
using Lusas.LPI;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        private List<ILoad> ReadAreaTemperatureLoads(List<string> ids = null)
        {
            List<ILoad> bhomAreaTemperatureLoads = new List<ILoad>();
            object[] lusasTemperatureLoads = d_LusasData.getAttributes("Temperature");

            List<PanelPlanar> bhomPanelPlanar = ReadPlanarPanels();
            Dictionary<string, PanelPlanar> surfaceDictionary = bhomPanelPlanar.ToDictionary(
                x => x.CustomData[AdapterId].ToString());

            List<IFLoadcase> allLoadcases = new List<IFLoadcase>();

            for (int i = 0; i < lusasTemperatureLoads.Count(); i++)
            {
                IFLoading lusasTemperatureLoad = (IFLoading)lusasTemperatureLoads[i];

                IEnumerable<IGrouping<string, IFAssignment>> groupedByLoadcases =
                    GetLoadAssignments(lusasTemperatureLoad);

                foreach (IEnumerable<IFAssignment> groupedAssignment in groupedByLoadcases)
                {
                    List<IFAssignment> surfaceAssignments = new List<IFAssignment>();

                    foreach (IFAssignment assignment in groupedAssignment)
                    {
                        IFSurface trySurf = assignment.getDatabaseObject() as IFSurface;

                        if (trySurf != null)
                        {
                            surfaceAssignments.Add(assignment);
                        }
                    }

                    List<string> analysisName = new List<string> { lusasTemperatureLoad.getAttributeType() };

                    if (surfaceAssignments.Count != 0)
                    {
                        AreaTemperatureLoad bhomAreaTemperatureLoad =
                            Engine.Lusas.Convert.ToAreaTempratureLoad(
                                lusasTemperatureLoad, groupedAssignment, surfaceDictionary);

                        bhomAreaTemperatureLoad.Tags = new HashSet<string>(analysisName);
                        bhomAreaTemperatureLoads.Add(bhomAreaTemperatureLoad);
                    }
                }

            }

            return bhomAreaTemperatureLoads;
        }
    }
}