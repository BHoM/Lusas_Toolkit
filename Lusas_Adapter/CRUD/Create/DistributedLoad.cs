using System.Collections.Generic;
using System.Linq;
using System;
using BH.oM.Base;
using BH.oM.Structure.Elements;
using BH.oM.Geometry;
using BH.oM.Structure.Properties;
using BH.oM.Structure.Loads;
using BH.oM.Common.Materials;
using BH.Engine.Geometry;
using BH.Engine.Structure;
using Lusas.LPI;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        public IFLoadingGlobalDistributed CreateGlobalDistributedLoad(BarUniformlyDistributedLoad distributedLoad, IFLine[] lusasLines)
        {
            IFLoadingGlobalDistributed lusasGlobalDistributed = null;
            IFLoadcase assignedLoadcase = (IFLoadcase)d_LusasData.getLoadset("Lc" + distributedLoad.Loadcase.CustomData[AdapterId] + "/" + distributedLoad.Loadcase.Name);
            string lusasAttributeName = "Dl" + distributedLoad.CustomData[AdapterId] + "/" + distributedLoad.Name;

            if (d_LusasData.existsAttribute("Loading",lusasAttributeName))
            {
                lusasGlobalDistributed = (IFLoadingGlobalDistributed)d_LusasData.getAttribute("Loading",
                    lusasAttributeName);
            }
            else
            {
                lusasGlobalDistributed = d_LusasData.createLoadingGlobalDistributed(lusasAttributeName);
                lusasGlobalDistributed.setGlobalDistributed("Length",
                    distributedLoad.Force.X, distributedLoad.Force.Y, distributedLoad.Force.Z,
                    distributedLoad.Moment.X, distributedLoad.Moment.Y, distributedLoad.Moment.Z);

            }

            IFAssignment assignToLines = m_LusasApplication.assignment();
            assignToLines.setLoadset(assignedLoadcase);
            lusasGlobalDistributed.assignTo(lusasLines, assignToLines);

            return lusasGlobalDistributed;
        }

        public IFLoadingLocalDistributed CreateLocalDistributedLoad(BarUniformlyDistributedLoad distributedLoad, IFLine[] lusasLines)
        {
            IFLoadingLocalDistributed lusasLocalDistributed = null;
            IFLoadcase assignedLoadcase = (IFLoadcase)d_LusasData.getLoadset("Lc" + distributedLoad.Loadcase.CustomData[AdapterId] + "/" + distributedLoad.Loadcase.Name);
            string lusasAttributeName = "Dl" + distributedLoad.CustomData[AdapterId] + "/" + distributedLoad.Name;

            if (d_LusasData.existsAttribute("Loading", lusasAttributeName))
            {
                lusasLocalDistributed = (IFLoadingLocalDistributed)d_LusasData.getAttribute("Loading",
                    lusasAttributeName);
            }
            else
            {
                lusasLocalDistributed = d_LusasData.createLoadingLocalDistributed(lusasAttributeName);
                lusasLocalDistributed.setLocalDistributed(
                    distributedLoad.Force.X, distributedLoad.Force.Y, distributedLoad.Force.Z,
                    "Line");

            }

            IFAssignment assignToLines = m_LusasApplication.assignment();
            assignToLines.setLoadset(assignedLoadcase);
            lusasLocalDistributed.assignTo(lusasLines, assignToLines);

            return lusasLocalDistributed;
        }
    }
}