using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.oM.Base;
using BH.oM.Geometry;
using BH.oM.Structure.Elements;
using BH.oM.Structure.Properties;
using BH.oM.Structure.Loads;
using BH.oM.Common.Materials;
using BH.Engine.Lusas;
using Lusas.LPI;

namespace BH.Engine.Lusas
{
    public static partial class Convert
    {
        public static AreaTemperatureLoad ToAreaTempratureLoad(
            IFLoading lusasTemperatureLoad,
            IEnumerable<IFAssignment> assignmentList,
            Dictionary<string, PanelPlanar> panels)
        {
            IFLoadcase assignedLoadcase = (IFLoadcase)assignmentList.First().getAssignmentLoadset();
            Loadcase bhomLoadcase = BH.Engine.Lusas.Convert.ToBHoMLoadcase(assignedLoadcase);
            double temperatureChange = lusasTemperatureLoad.getValue("T")
                - lusasTemperatureLoad.getValue("T0");

            IEnumerable<IAreaElement> bhomPanels = GetSurfaceAssignments(assignmentList, panels);
            AreaTemperatureLoad bhomAreaTemperatureLoad = BH.Engine.Structure.Create.AreaTemperatureLoad(
                bhomLoadcase,
                temperatureChange,
                bhomPanels,
                LoadAxis.Local,
                false,
                GetName(lusasTemperatureLoad));
            int bhomID = GetBHoMID(lusasTemperatureLoad, 'l');
            bhomAreaTemperatureLoad.CustomData["Lusas_id"] = bhomID;
            return bhomAreaTemperatureLoad;
        }
    }
}
