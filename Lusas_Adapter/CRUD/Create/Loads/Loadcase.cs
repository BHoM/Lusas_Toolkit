using BH.oM.Structure.Loads;
using Lusas.LPI;
using BH.Engine.Reflection;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        public IFLoadcase CreateLoadcase(Loadcase loadcase)
        {
            if (!CheckIllegalCharacters(loadcase.Name))
            {
                return null;
            }

            IFLoadcase lusasLoadcase = null;
            string lusasName = "Lc" + loadcase.CustomData[AdapterId] + "/" + loadcase.Name;

            if (d_LusasData.existsLoadset(lusasName))
            {
                lusasLoadcase = (IFLoadcase)d_LusasData.getLoadset(lusasName);
            }
            else
            {
                if (loadcase.Number == 0)
                {
                    lusasLoadcase = d_LusasData.createLoadcase(lusasName);
                    Compute.RecordWarning("0 used for LoadCombination number,"
                        + "therefore LoadCombination number will not be forced");
                }
                else
                {
                    lusasLoadcase = d_LusasData.createLoadcase(lusasName, "",
                        loadcase.Number);
                }
            }
            return lusasLoadcase;
        }
    }
}