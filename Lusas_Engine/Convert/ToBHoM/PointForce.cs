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
        public static PointForce ToPointForce(IFLoading lusasPointForce, IEnumerable<IFAssignment> assignmentList, Dictionary<string, Node> nodes)
        {
            IFLoadcase assignedLoadcase = (IFLoadcase)assignmentList.First().getAssignmentLoadset();
            Loadcase bhomLoadcase = BH.Engine.Lusas.Convert.ToBHoMLoadcase(assignedLoadcase);

            IEnumerable<Node> bhomNodes = GetNodeAssignments(assignmentList, nodes);

            Vector forceVector = new Vector { X = lusasPointForce.getValue("px"), Y = lusasPointForce.getValue("py"), Z = lusasPointForce.getValue("pz") };
            Vector momentVector = new Vector { X = lusasPointForce.getValue("mx"), Y = lusasPointForce.getValue("my"), Z = lusasPointForce.getValue("mz") };

            PointForce bhomPointForce = BH.Engine.Structure.Create.PointForce(bhomLoadcase, bhomNodes, forceVector, momentVector, LoadAxis.Global, GetName(lusasPointForce));

            int bhomID = GetBHoMID(lusasPointForce, 'l');
            bhomPointForce.CustomData["Lusas_id"] = bhomID;
            return bhomPointForce;
        }
    }
}