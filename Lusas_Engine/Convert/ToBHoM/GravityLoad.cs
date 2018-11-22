using System.Collections.Generic;
using System.Linq;
using BH.oM.Geometry;
using BH.oM.Structure.Elements;
using BH.oM.Structure.Loads;
using BH.oM.Base;
using Lusas.LPI;

namespace BH.Engine.Lusas
{
    public static partial class Convert
    {
        public static GravityLoad ToGravityLoad(IFLoading lusasGravityLoad,
            IEnumerable<IFAssignment> lusasAssignments,
            Dictionary<string, Bar> bhomBars,
            Dictionary<string, PanelPlanar> bhomPlanarPanels)
        {
            IFLoadcase assignedLoadcase = (IFLoadcase)lusasAssignments.First().getAssignmentLoadset();
            Loadcase bhomLoadcase = ToBHoMLoadcase(assignedLoadcase);
            Vector gravityVector = new Vector
            {
                X = lusasGravityLoad.getValue("accX"),
                Y = lusasGravityLoad.getValue("accY"),
                Z = lusasGravityLoad.getValue("accZ")
            };

            GravityLoad bhomGravityLoad = new GravityLoad();

            IEnumerable<BHoMObject> bhomObjects = GetGeometryAssignments(
                lusasAssignments, null, bhomBars, bhomPlanarPanels);
            bhomGravityLoad = Structure.Create.GravityLoad(
                bhomLoadcase, gravityVector, bhomObjects, GetName(lusasGravityLoad));

            int adapterID = GetAdapterID(lusasGravityLoad, 'l');
            bhomGravityLoad.CustomData["Lusas_id"] = adapterID;

            return bhomGravityLoad;
        }
    }
}