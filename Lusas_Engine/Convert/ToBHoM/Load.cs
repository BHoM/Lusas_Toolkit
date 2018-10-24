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

        public static BarUniformlyDistributedLoad ToBHoMLoad(IFLoading lusasDistributed, IEnumerable<IFAssignment> assignmentList, Dictionary<string, Bar> bars)
        {
            IFLoadcase assignedLoadcase = (IFLoadcase)assignmentList.First().getAssignmentLoadset();
            Loadcase bhomLoadcase = BH.Engine.Lusas.Convert.ToBHoMLoadcase(assignedLoadcase);

            IEnumerable<Bar> bhomBars = GetBarAssignments(assignmentList, bars);

            Vector forceVector = new Vector { X = lusasDistributed.getValue("WX"), Y = lusasDistributed.getValue("WY"), Z = lusasDistributed.getValue("WZ") };

            BarUniformlyDistributedLoad bhomBarUniformlyDistributed = null;

            if (lusasDistributed.getAttributeType() == "Global Distributed Load")
            {
                Vector momentVector = new Vector { X = lusasDistributed.getValue("MX"), Y = lusasDistributed.getValue("MY"), Z = lusasDistributed.getValue("MZ") };

                bhomBarUniformlyDistributed = BH.Engine.Structure.Create.BarUniformlyDistributedLoad(
                    bhomLoadcase,
                    bhomBars,
                    forceVector,
                    momentVector,
                    LoadAxis.Global,
                    true,
                    GetName(lusasDistributed));
            }
            else if(lusasDistributed.getAttributeType() == "Distributed Load")
            {
                bhomBarUniformlyDistributed = BH.Engine.Structure.Create.BarUniformlyDistributedLoad(
                    bhomLoadcase,
                    bhomBars,
                    forceVector,
                    null,
                    LoadAxis.Local,
                    true,
                    GetName(lusasDistributed));
            }

            int bhomID = GetBHoMID(lusasDistributed, 'l');
            bhomBarUniformlyDistributed.CustomData["Lusas_id"] = bhomID;
            return bhomBarUniformlyDistributed;
        }

        public static AreaUniformalyDistributedLoad ToBHoMLoad(IFLoading lusasDistributed, IEnumerable<IFAssignment> assignmentList, Dictionary<string, PanelPlanar> surfs)
        {
            IFLoadcase assignedLoadcase = (IFLoadcase)assignmentList.First().getAssignmentLoadset();
            Loadcase bhomLoadcase = BH.Engine.Lusas.Convert.ToBHoMLoadcase(assignedLoadcase);

            IEnumerable<PanelPlanar> bhomSurfs = GetSurfaceAssignments(assignmentList, surfs);
            
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

        public static BarPointLoad ToBHoMBarPointLoad(IFLoading lusasBarPointLoad, IEnumerable<IFAssignment> assignmentList, Dictionary<string, Bar> bars)
        {
            IFLoadcase assignedLoadcase = (IFLoadcase)assignmentList.First().getAssignmentLoadset();
            Loadcase bhomLoadcase = BH.Engine.Lusas.Convert.ToBHoMLoadcase(assignedLoadcase);

            IEnumerable<Bar> bhomBars = GetBarAssignments(assignmentList, bars);

            Vector forceVector = new Vector { X = lusasBarPointLoad.getValue("PX"), Y = lusasBarPointLoad.getValue("PY"), Z = lusasBarPointLoad.getValue("PZ") };
            Vector momentVector = new Vector { X = lusasBarPointLoad.getValue("MX"), Y = lusasBarPointLoad.getValue("MY"), Z = lusasBarPointLoad.getValue("MZ") };
            double forcePosition = lusasBarPointLoad.getValue("Distance");

            BarPointLoad bhomBarPointLoad = null;

                bhomBarPointLoad = BH.Engine.Structure.Create.BarPointLoad(
                    bhomLoadcase,
                    forcePosition,
                    bhomBars,
                    forceVector,
                    momentVector,
                    LoadAxis.Global,
                    GetName(lusasBarPointLoad));

            int bhomID = GetBHoMID(lusasBarPointLoad, 'l');
            bhomBarPointLoad.CustomData["Lusas_id"] = bhomID;
            return bhomBarPointLoad;
        }

    }
}
