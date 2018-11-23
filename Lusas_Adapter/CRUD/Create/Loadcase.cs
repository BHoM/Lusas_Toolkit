using BH.oM.Structure.Loads;
using Lusas.LPI;
using BH.Engine.Reflection;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        public IFLoadcase CreateLoadcase(Loadcase loadcase)
        {
            IFLoadcase lusasLoadcase = null;
            string lusasAttributeName = "Lc" + loadcase.CustomData[AdapterId] + "/" + loadcase.Name;

            if (d_LusasData.existsLoadset(lusasAttributeName))
            {
                lusasLoadcase = (IFLoadcase)d_LusasData.getLoadset(lusasAttributeName);
            }
            else
            {
                if (loadcase.Number == 0)
                {
                    lusasLoadcase = d_LusasData.createLoadcase(lusasAttributeName);
                    Compute.RecordWarning("0 used for LoadCombination number,"
                        + "therefore LoadCombination number will not be forced");
                }
                else
                {
                    lusasLoadcase = d_LusasData.createLoadcase(lusasAttributeName, "",
                        loadcase.Number);
                }
            }
            return lusasLoadcase;
        }
    }
}