using BH.oM.Structure.Loads;
using Lusas.LPI;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        public IFLoadingConcentrated CreateConcentratedLoad(PointForce pointForce, IFPoint[] lusasPoints)
        {

            IFLoadingConcentrated lusasPointForce = null;
            IFLoadcase assignedLoadcase = (IFLoadcase)d_LusasData.getLoadset(
                "Lc" + pointForce.Loadcase.CustomData[AdapterId] + "/" + pointForce.Loadcase.Name);

            string lusasAttributeName = "Pl" + pointForce.CustomData[AdapterId] + "/" + pointForce.Name;
            NameSearch("Pl", pointForce.CustomData[AdapterId].ToString(), pointForce.Name, ref lusasAttributeName);

            if (d_LusasData.existsAttribute("Loading", lusasAttributeName))
            {
                lusasPointForce = (IFLoadingConcentrated)d_LusasData.getAttribute("Loading", lusasAttributeName);
            }
            else
            {
                lusasPointForce = d_LusasData.createLoadingConcentrated(lusasAttributeName);
                lusasPointForce.setConcentrated(
                    pointForce.Force.X, pointForce.Force.Y, pointForce.Force.Z,
                    pointForce.Moment.X, pointForce.Moment.Y, pointForce.Moment.Z);
            }

            IFAssignment lusasAssignment = m_LusasApplication.assignment();
            lusasAssignment.setLoadset(assignedLoadcase);
            lusasPointForce.assignTo(lusasPoints, lusasAssignment);

            return lusasPointForce;
        }
    }
}
