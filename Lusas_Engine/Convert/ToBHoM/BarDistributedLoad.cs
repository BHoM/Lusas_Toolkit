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
        public static BarVaryingDistributedLoad ToBHoMBarDistributedLoad(IFLoading lusasBarDistributedLoad,
            IEnumerable<IFAssignment> lusasAssignments, Dictionary<string, Bar> bhomBarDictionary)
        {
            IFLoadcase assignedLoadcase = (IFLoadcase)lusasAssignments.First().getAssignmentLoadset();
            Loadcase bhomLoadcase = ToBHoMLoadcase(assignedLoadcase);

            IEnumerable<Bar> bhomBars = GetBarAssignments(lusasAssignments, bhomBarDictionary);

            Vector startForceVector = new Vector
            {
                X = lusasBarDistributedLoad.getValue("startpx"),
                Y = lusasBarDistributedLoad.getValue("startpy"),
                Z = lusasBarDistributedLoad.getValue("startpz")
            };

            Vector endForceVector = new Vector
            {
                X = lusasBarDistributedLoad.getValue("endpx"),
                Y = lusasBarDistributedLoad.getValue("endpy"),
                Z = lusasBarDistributedLoad.getValue("endpz")
            };

            Vector startMomentVector = new Vector
            {
                X = lusasBarDistributedLoad.getValue("startmx"),
                Y = lusasBarDistributedLoad.getValue("startmy"),
                Z = lusasBarDistributedLoad.getValue("startmz")
            };

            Vector endMomentVector = new Vector
            {
                X = lusasBarDistributedLoad.getValue("endmx"),
                Y = lusasBarDistributedLoad.getValue("endmy"),
                Z = lusasBarDistributedLoad.getValue("endmz")
            };

            double startPosition = lusasBarDistributedLoad.getValue("startDistance");
            double endPosition = lusasBarDistributedLoad.getValue("endDistance");

            BarVaryingDistributedLoad bhomBarPointLoad = null;

            bhomBarPointLoad = Structure.Create.BarVaryingDistributedLoad(
                bhomLoadcase,
                bhomBars,
                startPosition,
                startForceVector,
                startMomentVector,
                endPosition,
                endForceVector,
                endMomentVector,
                LoadAxis.Local,
                false,
                GetName(lusasBarDistributedLoad));

            int adapterID = GetAdapterID(lusasBarDistributedLoad, 'l');
            bhomBarPointLoad.CustomData["Lusas_id"] = adapterID;

            return bhomBarPointLoad;
        }
    }
}