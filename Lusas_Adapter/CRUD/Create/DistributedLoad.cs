using BH.oM.Geometry;
using BH.oM.Structure.Loads;
using Lusas.LPI;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        public IFLoadingGlobalDistributed CreateGlobalDistributedLine(BarUniformlyDistributedLoad distributedLoad, IFLine[] lusasLines)
        {
            IFLoadcase assignedLoadcase = (IFLoadcase)d_LusasData.getLoadset("Lc" + distributedLoad.Loadcase.CustomData[AdapterId] + "/" + distributedLoad.Loadcase.Name);
            string lusasAttributeName = "Dl" + distributedLoad.CustomData[AdapterId] + "/" + distributedLoad.Name;
            NameSearch("Dl", distributedLoad.CustomData[AdapterId].ToString(), distributedLoad.Name, ref lusasAttributeName);

            IFLoadingGlobalDistributed lusasGlobalDistributed = CreateGlobalDistributed(lusasAttributeName,
                "Length", assignedLoadcase, distributedLoad.Force, distributedLoad.Moment, lusasLines);

            return lusasGlobalDistributed;
        }

        public IFLoadingGlobalDistributed CreateGlobalDistributedLoad(AreaUniformalyDistributedLoad distributedLoad, IFSurface[] lusasSurfaces)
        {
            IFLoadcase assignedLoadcase = (IFLoadcase)d_LusasData.getLoadset("Lc" + distributedLoad.Loadcase.CustomData[AdapterId] + "/" + distributedLoad.Loadcase.Name);
            string lusasAttributeName = "Dl" + distributedLoad.CustomData[AdapterId] + "/" + distributedLoad.Name;

            IFLoadingGlobalDistributed lusasGlobalDistributed = CreateGlobalDistributed(lusasAttributeName,
                "Area", assignedLoadcase, distributedLoad.Pressure, null, lusasSurfaces);

            return lusasGlobalDistributed;
        }

        public IFLoadingLocalDistributed CreateLocalDistributedBar(BarUniformlyDistributedLoad distributedLoad, object[] lusasLines)
        {
            IFLoadcase assignedLoadcase = (IFLoadcase)d_LusasData.getLoadset("Lc" + distributedLoad.Loadcase.CustomData[AdapterId] + "/" + distributedLoad.Loadcase.Name);
            string lusasAttributeName = "Dl" + distributedLoad.CustomData[AdapterId] + "/" + distributedLoad.Name;

            IFLoadingLocalDistributed lusasLocalDistributed = CreateLocalDistributed(lusasAttributeName,
                "Line", assignedLoadcase, distributedLoad.Force, lusasLines);

            return lusasLocalDistributed;
        }

        public IFLoadingLocalDistributed CreateLocalDistributedSurface(AreaUniformalyDistributedLoad distributedLoad, IFSurface[] lusasSurfaces)
        {
            IFLoadcase assignedLoadcase = (IFLoadcase)d_LusasData.getLoadset("Lc" + distributedLoad.Loadcase.CustomData[AdapterId] + "/" + distributedLoad.Loadcase.Name);
            string lusasAttributeName = "Dl" + distributedLoad.CustomData[AdapterId] + "/" + distributedLoad.Name;

            IFLoadingLocalDistributed lusasLocalDistributed = CreateLocalDistributed(lusasAttributeName,
                "Area", assignedLoadcase, distributedLoad.Pressure, lusasSurfaces);

            return lusasLocalDistributed;
        }

        public IFLoadingGlobalDistributed CreateGlobalDistributed(string lusasAttributeName,
            string type, IFLoadcase assignedLoadcase, Vector force, Vector moment, object[] lusasGeometry)
        {
            IFLoadingGlobalDistributed lusasGlobalDistributed = null;

            if (d_LusasData.existsAttribute("Loading", lusasAttributeName))
            {
                lusasGlobalDistributed = (IFLoadingGlobalDistributed)d_LusasData.getAttribute("Loading",
                    lusasAttributeName);
            }
            else
            {
                lusasGlobalDistributed = d_LusasData.createLoadingGlobalDistributed(lusasAttributeName);
                if (type == "Length")
                {
                    lusasGlobalDistributed.setGlobalDistributed(type,
                        force.X, force.Y, force.Z, moment.X, moment.Y, moment.Z);
                }
                else if (type == "Area")
                {
                    lusasGlobalDistributed.setGlobalDistributed(type,
                        force.X, force.Y, force.Z);
                }
            }

            IFAssignment assignToLines = m_LusasApplication.assignment();
            assignToLines.setLoadset(assignedLoadcase);
            lusasGlobalDistributed.assignTo(lusasGeometry, assignToLines);

            return lusasGlobalDistributed;
        }

        public IFLoadingLocalDistributed CreateLocalDistributed(string lusasAttributeName,
            string type, IFLoadcase assignedLoadcase, Vector force, object[] lusasGeometry)
        {
            IFLoadingLocalDistributed lusasLocalDistributed = null;

            if (d_LusasData.existsAttribute("Loading", lusasAttributeName))
            {
                lusasLocalDistributed = (IFLoadingLocalDistributed)d_LusasData.getAttribute("Loading",
                    lusasAttributeName);
            }
            else
            {
                lusasLocalDistributed = d_LusasData.createLoadingLocalDistributed(lusasAttributeName);
                lusasLocalDistributed.setLocalDistributed(force.X, force.Y, force.Z,type);
            }

            IFAssignment assignToLines = m_LusasApplication.assignment();
            assignToLines.setLoadset(assignedLoadcase);
            lusasLocalDistributed.assignTo(lusasGeometry, assignToLines);

            return lusasLocalDistributed;
        }
    }
}