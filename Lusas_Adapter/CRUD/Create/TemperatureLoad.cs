using BH.oM.Structure.Loads;
using Lusas.LPI;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        public IFLoadingTemperature CreateBarTemperatureLoad(BarTemperatureLoad temperatureLoad, IFLine[] lusasLines)
        {
            IFLoadcase assignedLoadcase = (IFLoadcase)d_LusasData.getLoadset(
                "Lc" + temperatureLoad.Loadcase.CustomData[AdapterId] + "/" + temperatureLoad.Loadcase.Name);

            string lusasAttributeName = "Tl" + temperatureLoad.CustomData[AdapterId] + "/" + temperatureLoad.Name;
            NameSearch("Tl", temperatureLoad.CustomData[AdapterId].ToString(), 
                temperatureLoad.Name, ref lusasAttributeName);

            IFLoadingTemperature lusasTemperatureLoad = CreateTemperatureLoad(lusasAttributeName,
                temperatureLoad.TemperatureChange, lusasLines, assignedLoadcase);

            return lusasTemperatureLoad;
        }

        public IFLoadingTemperature CreateAreaTemperatureLoad(AreaTemperatureLoad temperatureLoad, 
            IFSurface[] lusasSurfaces)
        {

            IFLoadcase assignedLoadcase = (IFLoadcase)d_LusasData.getLoadset(
                "Lc" + temperatureLoad.Loadcase.CustomData[AdapterId] + "/" + temperatureLoad.Loadcase.Name);

            string lusasAttributeName = "Tl" + temperatureLoad.CustomData[AdapterId] + "/" + temperatureLoad.Name;

            IFLoadingTemperature lusasTemperatureLoad = CreateTemperatureLoad(lusasAttributeName,
                temperatureLoad.TemperatureChange, lusasSurfaces, assignedLoadcase);

            return lusasTemperatureLoad;
        }

        public IFLoadingTemperature CreateTemperatureLoad(string lusasAttributeName, 
            double temperatureChange, IFGeometry[] lusasGeometry, IFLoadcase assignedLoadcase)
        {
            IFLoadingTemperature lusasTemperatureLoad = null;
            if (d_LusasData.existsAttribute("Loading", lusasAttributeName))
            {
                lusasTemperatureLoad = (IFLoadingTemperature)d_LusasData.getAttributes(
                    "Loading", lusasAttributeName);
            }
            else
            {
                lusasTemperatureLoad = d_LusasData.createLoadingTemperature(lusasAttributeName);
                lusasTemperatureLoad.setValue("T0", 0);
                lusasTemperatureLoad.setValue("T", temperatureChange);
            }

            IFAssignment lusasAssignment = m_LusasApplication.assignment();
            lusasAssignment.setLoadset(assignedLoadcase);
            lusasTemperatureLoad.assignTo(lusasGeometry, lusasAssignment);

            return lusasTemperatureLoad;
        }
    }
}
