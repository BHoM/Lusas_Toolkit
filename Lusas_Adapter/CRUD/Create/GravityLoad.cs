using BH.oM.Structure.Loads;
using Lusas.LPI;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        public IFLoadingBody CreateGravityLoad(GravityLoad gravityLoad, IFGeometry[] lusasGeometry)
        {
            if (!CheckIllegalCharacters(gravityLoad.Name))
            {
                return null;
            }

            IFLoadingBody lusasGravityLoad = null;
            IFLoadcase assignedLoadcase = (IFLoadcase)d_LusasData.getLoadset(
                "Lc" + gravityLoad.Loadcase.CustomData[AdapterId] + "/" + gravityLoad.Loadcase.Name);

            string lusasName = "Gl" + gravityLoad.CustomData[AdapterId] + "/" + gravityLoad.Name;
            NameSearch("Gl", gravityLoad.CustomData[AdapterId].ToString(), gravityLoad.Name, ref lusasName);

            if (d_LusasData.existsAttribute("Loading", lusasName))
            {
                lusasGravityLoad = (IFLoadingBody)d_LusasData.getAttributes("Loading", lusasName);
            }
            else
            {
                lusasGravityLoad = d_LusasData.createLoadingBody(lusasName);
                lusasGravityLoad.setBody(
                    gravityLoad.GravityDirection.X, gravityLoad.GravityDirection.Y, gravityLoad.GravityDirection.Z);
            }

            IFAssignment lusasAssignment = m_LusasApplication.assignment();
            lusasAssignment.setLoadset(assignedLoadcase);
            lusasGravityLoad.assignTo(lusasGeometry, lusasAssignment);

            return lusasGravityLoad;
        }
    }
}
