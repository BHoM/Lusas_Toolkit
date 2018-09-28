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
        public static BarUniformlyDistributedLoad ToBarUniformlyDistributed(IFLoading lusasDistributed, IEnumerable<IFAssignment> assignmentList, Dictionary<string, Bar> bars)
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
            else if (lusasDistributed.getAttributeType() == "Distributed Load")
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
    }
}
