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
        public static AreaUniformalyDistributedLoad ToAreaUniformallyDistributed(
            IFLoading lusasDistributed, IEnumerable<IFAssignment> lusasAssignments,
            Dictionary<string, PanelPlanar> panelPlanarDictionary)
        {
            IFLoadcase assignedLoadcase = (IFLoadcase)lusasAssignments.First().getAssignmentLoadset();
            Loadcase bhomLoadcase = ToBHoMLoadcase(assignedLoadcase);

            IEnumerable<IAreaElement> bhomPlanarPanels = GetSurfaceAssignments(
                lusasAssignments, panelPlanarDictionary);

            Vector pressureVector = new Vector
            {
                X = lusasDistributed.getValue("WX"),
                Y = lusasDistributed.getValue("WY"),
                Z = lusasDistributed.getValue("WZ")
            };

            AreaUniformalyDistributedLoad bhomSurfaceUniformlyDistributed = null;

            if (lusasDistributed.getAttributeType() == "Global Distributed Load")
            {
                bhomSurfaceUniformlyDistributed = Structure.Create.AreaUniformalyDistributedLoad(
                bhomLoadcase,
                pressureVector,
                bhomPlanarPanels,
                LoadAxis.Global,
                true,
                GetName(lusasDistributed));
            }
            else if (lusasDistributed.getAttributeType() == "Distributed Load")
            {
                bhomSurfaceUniformlyDistributed = Structure.Create.AreaUniformalyDistributedLoad(
                bhomLoadcase,
                pressureVector,
                bhomPlanarPanels,
                LoadAxis.Local,
                true,
                GetName(lusasDistributed));
            }

            int adapterID = GetAdapterID(lusasDistributed, 'l');
            bhomSurfaceUniformlyDistributed.CustomData["Lusas_id"] = adapterID;

            return bhomSurfaceUniformlyDistributed;
        }
    }
}