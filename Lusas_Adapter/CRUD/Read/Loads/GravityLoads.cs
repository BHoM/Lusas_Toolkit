using System.Collections.Generic;
using System.Linq;
using BH.oM.Structure.Elements;
using BH.oM.Structure.Loads;
using Lusas.LPI;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        private List<ILoad> ReadGravityLoads(List<string> ids = null)
        {
            List<ILoad> bhomGravityLoads = new List<ILoad>();
            object[] lusasBodyForces = d_LusasData.getAttributes("Body Force Load");

            List<Bar> bhomBars = ReadBars();
            List<PanelPlanar> bhomPlanarPanels = ReadPlanarPanels();
            Dictionary<string, Bar> barDictionary = bhomBars.ToDictionary(
                x => x.CustomData[AdapterId].ToString());

            Dictionary<string, PanelPlanar> panelPlanarDictionary = bhomPlanarPanels.ToDictionary(
                x => x.CustomData[AdapterId].ToString());

            List<IFLoadcase> allLoadcases = new List<IFLoadcase>();

            for (int i = 0; i < lusasBodyForces.Count(); i++)
            {
                IFLoading lusasBodyForce = (IFLoading)lusasBodyForces[i];

                IEnumerable<IGrouping<string, IFAssignment>> groupedByLoadcases =
                    GetLoadAssignments(lusasBodyForce);

                foreach (IEnumerable<IFAssignment> groupedAssignment in groupedByLoadcases)
                {
                    List<string> analysisName = new List<string> { lusasBodyForce.getAttributeType() };

                    GravityLoad bhomGravityLoad = Engine.Lusas.Convert.ToGravityLoad(
                        lusasBodyForce, groupedAssignment, barDictionary, panelPlanarDictionary);
                    bhomGravityLoad.Tags = new HashSet<string>(analysisName);
                    bhomGravityLoads.Add(bhomGravityLoad);
                }
            }

            return bhomGravityLoads;
        }
    }
}