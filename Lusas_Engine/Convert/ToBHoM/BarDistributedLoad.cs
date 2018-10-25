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
        public static BarVaryingDistributedLoad ToBHoMBarDistributedLoad(IFLoading lusasBarDistributedLoad, IEnumerable<IFAssignment> assignmentList, Dictionary<string, Bar> bars)
        {
            IFLoadcase assignedLoadcase = (IFLoadcase)assignmentList.First().getAssignmentLoadset();
            Loadcase bhomLoadcase = BH.Engine.Lusas.Convert.ToBHoMLoadcase(assignedLoadcase);

            IEnumerable<Bar> bhomBars = GetBarAssignments(assignmentList, bars);

            Vector startForceVector = new Vector { X = lusasBarDistributedLoad.getValue("startpx"), Y = lusasBarDistributedLoad.getValue("startpy"), Z = lusasBarDistributedLoad.getValue("startpz") };
            Vector endForceVector = new Vector { X = lusasBarDistributedLoad.getValue("endpx"), Y = lusasBarDistributedLoad.getValue("endpy"), Z = lusasBarDistributedLoad.getValue("endpz") };
            Vector startMomentVector = new Vector { X = lusasBarDistributedLoad.getValue("startmx"), Y = lusasBarDistributedLoad.getValue("startmy"), Z = lusasBarDistributedLoad.getValue("startmz") };
            Vector endMomentVector = new Vector { X = lusasBarDistributedLoad.getValue("endmx"), Y = lusasBarDistributedLoad.getValue("endmy"), Z = lusasBarDistributedLoad.getValue("endmz") };
            double startPosition = lusasBarDistributedLoad.getValue("startDistance");
            double endPosition = lusasBarDistributedLoad.getValue("endDistance");

            BarVaryingDistributedLoad bhomBarPointLoad = null;

            bhomBarPointLoad = BH.Engine.Structure.Create.BarVaryingDistributedLoad(
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
                
            int bhomID = GetBHoMID(lusasBarDistributedLoad, 'l');
            bhomBarPointLoad.CustomData["Lusas_id"] = bhomID;
            return bhomBarPointLoad;
        }
    }
}