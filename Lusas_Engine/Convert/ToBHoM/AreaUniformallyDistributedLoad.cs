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
        public static AreaUniformalyDistributedLoad ToAreaUniformallyDistributed(IFLoading lusasDistributed, IEnumerable<IFAssignment> assignmentList, Dictionary<string, PanelPlanar> surfs)
        {
            IFLoadcase assignedLoadcase = (IFLoadcase)assignmentList.First().getAssignmentLoadset();
            Loadcase bhomLoadcase = BH.Engine.Lusas.Convert.ToBHoMLoadcase(assignedLoadcase);

            IEnumerable<IAreaElement> bhomSurfs = GetSurfaceAssignments(assignmentList, surfs);

            Vector pressureVector = new Vector { X = lusasDistributed.getValue("WX"), Y = lusasDistributed.getValue("WY"), Z = lusasDistributed.getValue("WZ") };

            AreaUniformalyDistributedLoad bhomSurfaceUniformlyDistributed = null;

            if (lusasDistributed.getAttributeType() == "Global Distributed Load")
            {
                bhomSurfaceUniformlyDistributed = BH.Engine.Structure.Create.AreaUniformalyDistributedLoad(
                bhomLoadcase,
                pressureVector,
                bhomSurfs,
                LoadAxis.Global,
                true,
                GetName(lusasDistributed));
            }
            else if (lusasDistributed.getAttributeType() == "Distributed Load")
            {
                bhomSurfaceUniformlyDistributed = BH.Engine.Structure.Create.AreaUniformalyDistributedLoad(
                bhomLoadcase,
                pressureVector,
                bhomSurfs,
                LoadAxis.Local,
                true,
                GetName(lusasDistributed));
            }

            int bhomID = GetBHoMID(lusasDistributed, 'l');
            bhomSurfaceUniformlyDistributed.CustomData["Lusas_id"] = bhomID;
            return bhomSurfaceUniformlyDistributed;
        }
    }
}