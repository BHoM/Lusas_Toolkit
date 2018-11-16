using BH.oM.Structure.Loads;
using Lusas.LPI;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        public IFLoadingTemperature CreateBarTemperatureLoad(BarTemperatureLoad temperatureLoad, object[] lusasGeom)
        {
            IFLoadcase assignedLoadcase = (IFLoadcase)d_LusasData.getLoadset("Lc" + temperatureLoad.Loadcase.CustomData[AdapterId] + "/" + temperatureLoad.Loadcase.Name);
            string lusasAttributeName = "Tl" + temperatureLoad.CustomData[AdapterId] + "/" + temperatureLoad.Name;
            NameSearch("Tl", temperatureLoad.CustomData[AdapterId].ToString(), temperatureLoad.Name, ref lusasAttributeName);

            IFLoadingTemperature lusasTemperatureLoad = CreateTemperatureLoad(lusasAttributeName,
                temperatureLoad.TemperatureChange, lusasGeom, assignedLoadcase);

            return lusasTemperatureLoad;
        }

        public IFLoadingTemperature CreateAreaTemperatureLoad(AreaTemperatureLoad temperatureLoad, object[] lusasGeom)
        {

            IFLoadcase assignedLoadcase = (IFLoadcase)d_LusasData.getLoadset("Lc" + temperatureLoad.Loadcase.CustomData[AdapterId] + "/" + temperatureLoad.Loadcase.Name);
            string lusasAttributeName = "Tl" + temperatureLoad.CustomData[AdapterId] + "/" + temperatureLoad.Name;

            IFLoadingTemperature lusasTemperatureLoad = CreateTemperatureLoad(lusasAttributeName,
                temperatureLoad.TemperatureChange, lusasGeom, assignedLoadcase);

            return lusasTemperatureLoad;
        }

        public IFLoadingTemperature CreateTemperatureLoad(string lusasAttributeName, 
            double temperatureChange, object[] lusasGeometry, IFLoadcase assignedLoadcase)
        {
            IFLoadingTemperature lusasTemperatureLoad = null;
            if (d_LusasData.existsAttribute("Loading", lusasAttributeName))
            {
                lusasTemperatureLoad = (IFLoadingTemperature)d_LusasData.getAttributes("Loading", lusasAttributeName);
            }
            else
            {
                lusasTemperatureLoad = d_LusasData.createLoadingTemperature(lusasAttributeName);
                lusasTemperatureLoad.setValue("T0", 0);
                lusasTemperatureLoad.setValue("T", temperatureChange);
            }

            IFAssignment assignToGeom = m_LusasApplication.assignment();
            assignToGeom.setLoadset(assignedLoadcase);
            lusasTemperatureLoad.assignTo(lusasGeometry, assignToGeom);

            return lusasTemperatureLoad;
        }
    }
}
