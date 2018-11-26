using System.Collections.Generic;
using System.Linq;
using BH.oM.Geometry;
using BH.oM.Structure.Elements;
using BH.oM.Structure.Loads;
using Lusas.LPI;

namespace BH.Engine.Lusas
{
    public static partial class Convert
    {
        public static BarPointLoad ToBHoMBarPointLoad(IFLoading lusasBarPointLoad,
            IEnumerable<IFAssignment> lusasAssignments, Dictionary<string, Bar> bhomBarDictionary)
        {
            IFLoadcase assignedLoadcase = (IFLoadcase)lusasAssignments.First().getAssignmentLoadset();
            Loadcase bhomLoadcase = ToBHoMLoadcase(assignedLoadcase);

            IEnumerable<Bar> bhomBars = GetBarAssignments(lusasAssignments, bhomBarDictionary);

            Vector forceVector = new Vector
            {
                X = lusasBarPointLoad.getValue("PX"),
                Y = lusasBarPointLoad.getValue("PY"),
                Z = lusasBarPointLoad.getValue("PZ")
            };

            Vector momentVector = new Vector
            {
                X = lusasBarPointLoad.getValue("MX"),
                Y = lusasBarPointLoad.getValue("MY"),
                Z = lusasBarPointLoad.getValue("MZ")
            };

            double forcePosition = lusasBarPointLoad.getValue("Distance");

            BarPointLoad bhomBarPointLoad = null;

            bhomBarPointLoad = Structure.Create.BarPointLoad(
                bhomLoadcase,
                forcePosition,
                bhomBars,
                forceVector,
                momentVector,
                LoadAxis.Global,
                GetName(lusasBarPointLoad));

            int adapterID = GetAdapterID(lusasBarPointLoad, 'l');
            bhomBarPointLoad.CustomData["Lusas_id"] = adapterID;

            return bhomBarPointLoad;
        }
    }
}