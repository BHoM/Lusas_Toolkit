using BH.oM.Structure.Loads;
using Lusas.LPI;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        public IFLoadingTemperature CreateBarTemperatureLoad(BarTemperatureLoad temperatureLoad, IFLine[] lusasLines)
        {
            if (!CheckIllegalCharacters(temperatureLoad.Name))
            {
                return null;
            }

            IFLoadcase assignedLoadcase = (IFLoadcase)d_LusasData.getLoadset(
                "Lc" + temperatureLoad.Loadcase.CustomData[AdapterId] + "/" + temperatureLoad.Loadcase.Name);

            string lusasName = "Tl" + temperatureLoad.CustomData[AdapterId] + "/" + temperatureLoad.Name;
            NameSearch("Tl", temperatureLoad.CustomData[AdapterId].ToString(), 
                temperatureLoad.Name, ref lusasName);

            IFLoadingTemperature lusasTemperatureLoad = CreateTemperatureLoad(lusasName,
                temperatureLoad.TemperatureChange, lusasLines, assignedLoadcase);

            return lusasTemperatureLoad;
        }

        public IFLoadingTemperature CreateAreaTemperatureLoad(AreaTemperatureLoad temperatureLoad, 
            IFSurface[] lusasSurfaces)
        {
            if (!CheckIllegalCharacters(temperatureLoad.Name))
            {
                return null;
            }

            IFLoadcase assignedLoadcase = (IFLoadcase)d_LusasData.getLoadset(
                "Lc" + temperatureLoad.Loadcase.CustomData[AdapterId] + "/" + temperatureLoad.Loadcase.Name);

            string lusasName = "Tl" + temperatureLoad.CustomData[AdapterId] + "/" + temperatureLoad.Name;

            IFLoadingTemperature lusasTemperatureLoad = CreateTemperatureLoad(lusasName,
                temperatureLoad.TemperatureChange, lusasSurfaces, assignedLoadcase);

            return lusasTemperatureLoad;
        }

        public IFLoadingTemperature CreateTemperatureLoad(string lusasName, 
            double temperatureChange, IFGeometry[] lusasGeometry, IFLoadcase assignedLoadcase)
        {
            if (!CheckIllegalCharacters(lusasName))
            {
                return null;
            }

            IFLoadingTemperature lusasTemperatureLoad = null;
            if (d_LusasData.existsAttribute("Loading", lusasName))
            {
                lusasTemperatureLoad = (IFLoadingTemperature)d_LusasData.getAttributes(
                    "Loading", lusasName);
            }
            else
            {
                lusasTemperatureLoad = d_LusasData.createLoadingTemperature(lusasName);
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
