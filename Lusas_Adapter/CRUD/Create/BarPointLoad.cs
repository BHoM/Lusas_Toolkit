using BH.oM.Structure.Loads;
using Lusas.LPI;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        public IFLoadingBeamPoint CreateBarPointLoad(BarPointLoad bhomBarPointLoad, IFLine[] lusasLines)
        {
            IFLoadingBeamPoint lusasBarPointLoad = null;
            IFLoadcase assignedLoadcase = (IFLoadcase)d_LusasData.getLoadset("Lc" + bhomBarPointLoad.Loadcase.CustomData[AdapterId] + "/" + bhomBarPointLoad.Loadcase.Name);
            string lusasAttributeName = "BPl" + bhomBarPointLoad.CustomData[AdapterId] + "/" + bhomBarPointLoad.Name;
            NameSearch("BPl", bhomBarPointLoad.CustomData[AdapterId].ToString(), bhomBarPointLoad.Name, ref lusasAttributeName);

            if (d_LusasData.existsAttribute("Loading", lusasAttributeName))
            {
                lusasBarPointLoad = (IFLoadingBeamPoint)d_LusasData.getAttribute("Loading", lusasAttributeName);
            }
            else
            {
                lusasBarPointLoad = d_LusasData.createLoadingBeamPoint(bhomBarPointLoad.Name);
                if (bhomBarPointLoad.Axis.ToString() == "Global")
                    lusasBarPointLoad.setBeamPoint("parametric", "global", "beam");
                else
                    lusasBarPointLoad.setBeamPoint("parametric", "local", "beam");
                lusasBarPointLoad.addRow(bhomBarPointLoad.DistanceFromA, bhomBarPointLoad.Force.X, bhomBarPointLoad.Force.Y, bhomBarPointLoad.Force.Z, bhomBarPointLoad.Moment.X, bhomBarPointLoad.Moment.Y, bhomBarPointLoad.Moment.Z);
            }

            IFAssignment assignToBars = m_LusasApplication.assignment();
            assignToBars.setLoadset(assignedLoadcase);
            lusasBarPointLoad.assignTo(lusasLines, assignToBars);

            return lusasBarPointLoad;
        }
    }
}