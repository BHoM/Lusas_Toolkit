﻿using System.Collections.Generic;
using System.Linq;
using BH.oM.Geometry;
using BH.oM.Structure.Elements;
using BH.oM.Structure.Loads;
using Lusas.LPI;

namespace BH.Engine.Lusas
{
    public static partial class Convert
    {
        public static BarUniformlyDistributedLoad ToBarUniformallyDistributed(IFLoading lusasDistributed,
            IEnumerable<IFAssignment> lusasAssignments, Dictionary<string, Bar> bhomBarDictionary)
        {
            IFLoadcase assignedLoadcase = (IFLoadcase)lusasAssignments.First().getAssignmentLoadset();
            Loadcase bhomLoadcase = ToBHoMLoadcase(assignedLoadcase);

            IEnumerable<Bar> bhomBars = GetBarAssignments(lusasAssignments, bhomBarDictionary);

            Vector forceVector = new Vector
            {
                X = lusasDistributed.getValue("WX"),
                Y = lusasDistributed.getValue("WY"),
                Z = lusasDistributed.getValue("WZ")
            };

            BarUniformlyDistributedLoad bhomBarUniformlyDistributed = null;

            if (lusasDistributed.getAttributeType() == "Global Distributed Load")
            {
                Vector momentVector = new Vector
                {
                    X = lusasDistributed.getValue("MX"),
                    Y = lusasDistributed.getValue("MY"),
                    Z = lusasDistributed.getValue("MZ")
                };

                bhomBarUniformlyDistributed = Structure.Create.BarUniformlyDistributedLoad(
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
                bhomBarUniformlyDistributed = Structure.Create.BarUniformlyDistributedLoad(
                    bhomLoadcase,
                    bhomBars,
                    forceVector,
                    null,
                    LoadAxis.Local,
                    true,
                    GetName(lusasDistributed));
            }

            int adapterID = GetAdapterID(lusasDistributed, 'l');
            bhomBarUniformlyDistributed.CustomData["Lusas_id"] = adapterID;

            return bhomBarUniformlyDistributed;
        }
    }
}