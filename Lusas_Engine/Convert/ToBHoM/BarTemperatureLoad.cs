using System.Collections.Generic;
using System.Linq;
using BH.oM.Structure.Elements;
using BH.oM.Structure.Loads;
using Lusas.LPI;

namespace BH.Engine.Lusas
{
    public static partial class Convert
    {
        public static BarTemperatureLoad ToBarTemperatureLoad(
            IFLoading lusasTemperatureLoad,
            IEnumerable<IFAssignment> lusasAssignments,
            Dictionary<string, Bar> bars)
        {
            IFLoadcase assignedLoadcase = (IFLoadcase)lusasAssignments.First().getAssignmentLoadset();
            Loadcase bhomLoadcase = ToBHoMLoadcase(assignedLoadcase);
            double temperatureChange = lusasTemperatureLoad.getValue("T")
                - lusasTemperatureLoad.getValue("T0");

            IEnumerable<Bar> bhomBars = GetBarAssignments(lusasAssignments, bars);
            BarTemperatureLoad bhomBarTemperatureLoad = Structure.Create.BarTemperatureLoad(
                bhomLoadcase,
                temperatureChange,
                bhomBars,
                LoadAxis.Local,
                false,
                GetName(lusasTemperatureLoad));

            int adapterID = GetAdapterID(lusasTemperatureLoad, 'l');
            bhomBarTemperatureLoad.CustomData["Lusas_id"] = adapterID;
            return bhomBarTemperatureLoad;
        }
    }
}
