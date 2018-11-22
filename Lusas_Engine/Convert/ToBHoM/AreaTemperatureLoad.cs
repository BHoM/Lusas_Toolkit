using System.Collections.Generic;
using System.Linq;
using BH.oM.Structure.Elements;
using BH.oM.Structure.Loads;
using Lusas.LPI;

namespace BH.Engine.Lusas
{
    public static partial class Convert
    {
        public static AreaTemperatureLoad ToAreaTempratureLoad(
            IFLoading lusasTemperatureLoad,
            IEnumerable<IFAssignment> lusasAssignments,
            Dictionary<string, PanelPlanar> panelPlanarDictionary)
        {
            IFLoadcase assignedLoadcase = (IFLoadcase)lusasAssignments.First().getAssignmentLoadset();
            Loadcase bhomLoadcase = ToBHoMLoadcase(assignedLoadcase);
            double temperatureChange = lusasTemperatureLoad.getValue("T")
                - lusasTemperatureLoad.getValue("T0");

            IEnumerable<IAreaElement> bhomPlanarPanels = GetSurfaceAssignments(lusasAssignments, panelPlanarDictionary);
            AreaTemperatureLoad bhomAreaTemperatureLoad = Structure.Create.AreaTemperatureLoad(
                bhomLoadcase,
                temperatureChange,
                bhomPlanarPanels,
                LoadAxis.Local,
                false,
                GetName(lusasTemperatureLoad));

            int adapterID = GetAdapterID(lusasTemperatureLoad, 'l');
            bhomAreaTemperatureLoad.CustomData["Lusas_id"] = adapterID;

            return bhomAreaTemperatureLoad;
        }
    }
}
