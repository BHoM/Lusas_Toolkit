using BH.oM.Structure.Loads;
using Lusas.LPI;

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
                lusasLoadcase = d_LusasData.createLoadcase(lusasAttributeName, "", loadcase.Number);
            }
            return lusasLoadcase;
        }
    }
}