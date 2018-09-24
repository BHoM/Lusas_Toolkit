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
        public IFLoadingConcentrated CreateConcentratedLoad(PointForce pointForce, IFPoint[] lusasPoints)
        {

            IFLoadingConcentrated lusasPointForce = null;
            IFAssignment assignToNodes = m_LusasApplication.assignment();
            IFLoadcase assignedLoadcase = (IFLoadcase) d_LusasData.getLoadset("Lc" + pointForce.Loadcase.CustomData[AdapterId] + "/" + pointForce.Loadcase.Name);

            if (d_LusasData.existsAttribute("Loading","Pl"+ pointForce.CustomData[AdapterId] + "/" + pointForce.Name))
            {
                object[] attribute = d_LusasData.getAttributes("Loading", "Pl" + pointForce.CustomData[AdapterId] + "/" + pointForce.Name);
                lusasPointForce = (IFLoadingConcentrated)attribute[0];
            }
            else
            {
                lusasPointForce = d_LusasData.createLoadingConcentrated("Pl" + pointForce.CustomData[AdapterId] + "/" + pointForce.Name);
                lusasPointForce.setConcentrated(pointForce.Force.X, pointForce.Force.Y, pointForce.Force.Z, pointForce.Moment.X, pointForce.Moment.Y, pointForce.Moment.Z);
                assignToNodes.setLoadset(assignedLoadcase);
                lusasPointForce.assignTo(lusasPoints,assignToNodes);
            }
            return lusasPointForce;
        }
    }
}
