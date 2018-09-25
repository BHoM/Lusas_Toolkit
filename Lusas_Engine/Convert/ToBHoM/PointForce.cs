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
        public static PointForce ToBHoMLoad(IFLoading lusasPointForce, IEnumerable<IFAssignment> assignmentList, Dictionary<string,Node> nodes)
        {
            IFLoadcase assignedLoadcase = (IFLoadcase)assignmentList.First().getAssignmentLoadset();
            Loadcase bhomLoadcase = BH.Engine.Lusas.Convert.ToBHoMLoadcase(assignedLoadcase);

            BHoMGroup<Node> bhomNodes = GetNodeAssignments(assignmentList, nodes);

            Vector forceVector = new Vector { X = lusasPointForce.getValue("px"), Y = lusasPointForce.getValue("py"), Z = lusasPointForce.getValue("pz") };
            Vector momentVector = new Vector { X = lusasPointForce.getValue("mx"), Y = lusasPointForce.getValue("my"), Z = lusasPointForce.getValue("mz") };

            PointForce bhomPointForce = new PointForce { Loadcase=bhomLoadcase, Name = GetName(lusasPointForce), Objects = bhomNodes, Force = forceVector, Moment = momentVector };

            int bhomID = GetBHoMID(lusasPointForce, 'l');
            bhomPointForce.CustomData["Lusas_id"] = bhomID;
            return bhomPointForce;
        }
    }
}
