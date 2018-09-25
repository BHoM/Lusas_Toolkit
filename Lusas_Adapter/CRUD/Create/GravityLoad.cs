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
        public IFLoadingBody CreateGravityLoad(GravityLoad gravityLoad, object[] lusasGeom,string assignedType)
        {
            IFLoadingBody lusasGravityLoad = null;
            IFAssignment assignToGeom = m_LusasApplication.assignment();
            IFLoadcase assignedLoadcase = (IFLoadcase)d_LusasData.getLoadset("Lc" + gravityLoad.Loadcase.CustomData[AdapterId] + "/" + gravityLoad.Loadcase.Name);

            if (d_LusasData.existsAttribute("Loading", "Gl" + gravityLoad.CustomData[AdapterId] + "/" + gravityLoad.Name))
            {
                object[] attribute = d_LusasData.getAttributes("Loading", "Gl" + gravityLoad.CustomData[AdapterId] + "/" + gravityLoad.Name);
                lusasGravityLoad = (IFLoadingBody)attribute[0];
            }
            else
            {
                lusasGravityLoad = d_LusasData.createLoadingBody("Gl" + gravityLoad.CustomData[AdapterId] + "/" + gravityLoad.Name);
                lusasGravityLoad.setBody(gravityLoad.GravityDirection.X, gravityLoad.GravityDirection.Y, gravityLoad.GravityDirection.Z);
                assignToGeom.setLoadset(assignedLoadcase);
                lusasGravityLoad.assignTo(lusasGeom, assignToGeom);
            }
            return lusasGravityLoad;
        }
    }
}
