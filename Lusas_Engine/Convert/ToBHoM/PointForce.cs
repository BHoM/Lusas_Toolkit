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

            IEnumerable<Node> bhomNodes = GetNodeAssignments(assignmentList, nodes);

            Vector forceVector = new Vector { X = lusasPointForce.getValue("px"), Y = lusasPointForce.getValue("py"), Z = lusasPointForce.getValue("pz") };
            Vector momentVector = new Vector { X = lusasPointForce.getValue("mx"), Y = lusasPointForce.getValue("my"), Z = lusasPointForce.getValue("mz") };

            PointForce bhomPointForce = BH.Engine.Structure.Create.PointForce(bhomLoadcase, bhomNodes, forceVector, momentVector, LoadAxis.Global,GetName(lusasPointForce));

            int bhomID = GetBHoMID(lusasPointForce, 'l');
            bhomPointForce.CustomData["Lusas_id"] = bhomID;
            return bhomPointForce;
        }

        public static GravityLoad ToBHoMLoad(IFLoading lusasGravityLoad, IEnumerable<IFAssignment> assignmentList, string geometry, Dictionary<string, Bar> bars, Dictionary<string,PanelPlanar> surfaces)
        {
            IFLoadcase assignedLoadcase = (IFLoadcase)assignmentList.First().getAssignmentLoadset();
            Loadcase bhomLoadcase = BH.Engine.Lusas.Convert.ToBHoMLoadcase(assignedLoadcase);
            Vector gravityVector = new Vector { X = lusasGravityLoad.getValue("accX"), Y = lusasGravityLoad.getValue("accY"), Z = lusasGravityLoad.getValue("accZ") };
            GravityLoad bhomGravityLoad = new GravityLoad();

            if (geometry=="Bar")
            {
                IEnumerable<Bar> bhomBars = GetBarAssignments(assignmentList, bars);
                bhomGravityLoad = BH.Engine.Structure.Create.GravityLoad(bhomLoadcase, gravityVector, bhomBars,GetName(lusasGravityLoad));    
            }
            else
            {
                IEnumerable<PanelPlanar> bhomSurfs = GetSurfaceAssignments(assignmentList, surfaces);
                bhomGravityLoad = BH.Engine.Structure.Create.GravityLoad(bhomLoadcase, gravityVector, bhomSurfs, GetName(lusasGravityLoad));
            }

            int bhomID = GetBHoMID(lusasGravityLoad, 'l');
            bhomGravityLoad.CustomData["Lusas_id"] = bhomID;
            return bhomGravityLoad;
        }
    }
}
