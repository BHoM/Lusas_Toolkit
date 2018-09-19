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
using Lusas.LPI;

namespace BH.Engine.Lusas
{
    public static partial class Convert
    {
        public static PointForce ToBHoMPointLoad(IFLoadingConcentrated lusasPointForce, IEnumerable<IFAssignment> assignmentList, HashSet<string> groupNames, Dictionary<string, Constraint6DOF> constraints6DOF)
        {
            IFLoadcase assignedLoadcase = (IFLoadcase)assignmentList.First().getAssignmentLoadset();
            Loadcase bhomLoadcase = BH.Engine.Lusas.Convert.ToBHoMLoadcase(assignedLoadcase);
            List<Node> assignedNodes = new List<Node>();

            foreach (var assignment in assignmentList)
            {
                IFPoint lusasPoint = (IFPoint) assignment.getDatabaseObject();
                Node bhomNode = BH.Engine.Lusas.Convert.ToBHoMNode(lusasPoint, groupNames, constraints6DOF);
                assignedNodes.Add(bhomNode);
            }

            BHoMGroup<Node> bhomNodes = new BHoMGroup<Node> { Elements = assignedNodes };

            Vector forceVector = new Vector { X = lusasPointForce.getValue("px"), Y = lusasPointForce.getValue("py"), Z = lusasPointForce.getValue("pz") };
            Vector momentVector = new Vector { X = lusasPointForce.getValue("mx"), Y = lusasPointForce.getValue("my"), Z = lusasPointForce.getValue("mz") };

            PointForce bhomPointForce = new PointForce { Loadcase=bhomLoadcase, Name = getName(lusasPointForce), Objects = bhomNodes, Force = forceVector, Moment = momentVector };

            int bhomID = getBHoMID(lusasPointForce, 'l');
            bhomPointForce.CustomData["Lusas_id"] = bhomID;
            return bhomPointForce;
        }
    }
}
