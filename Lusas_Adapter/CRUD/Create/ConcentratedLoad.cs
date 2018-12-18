using BH.oM.Structure.Loads;
using Lusas.LPI;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        public IFLoadingConcentrated CreateConcentratedLoad(PointForce pointForce, IFPoint[] lusasPoints)
        {
            if (!CheckIllegalCharacters(pointForce.Name))
            {
                return null;
            }

            IFLoadingConcentrated lusasPointForce = null;
            IFLoadcase assignedLoadcase = (IFLoadcase)d_LusasData.getLoadset(
                "Lc" + pointForce.Loadcase.CustomData[AdapterId] + "/" + pointForce.Loadcase.Name);

            string lusasName = "Pl" + pointForce.CustomData[AdapterId] + "/" + pointForce.Name;
            NameSearch("Pl", pointForce.CustomData[AdapterId].ToString(), pointForce.Name, ref lusasName);

            if (d_LusasData.existsAttribute("Loading", lusasName))
            {
                lusasPointForce = (IFLoadingConcentrated)d_LusasData.getAttribute("Loading", lusasName);
            }
            else
            {
                lusasPointForce = d_LusasData.createLoadingConcentrated(lusasName);
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
