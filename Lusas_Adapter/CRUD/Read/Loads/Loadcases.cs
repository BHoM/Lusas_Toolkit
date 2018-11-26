using System.Collections.Generic;
using System.Linq;
using BH.oM.Structure.Loads;
using Lusas.LPI;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        private List<Loadcase> ReadLoadcases(List<string> ids = null)
        {
            List<Loadcase> bhomLoadcases = new List<Loadcase>();
            object[] allLoadcases = d_LusasData.getLoadsets("loadcase", "all");

            for (int i = 0; i < allLoadcases.Count(); i++)
            {
                IFLoadcase lusasLoadcase = (IFLoadcase)allLoadcases[i];
                Loadcase bhomLoadcase = Engine.Lusas.Convert.ToBHoMLoadcase(lusasLoadcase);
                List<string> analysisName = new List<string> { lusasLoadcase.getAnalysis().getName() };
                bhomLoadcase.Tags = new HashSet<string>(analysisName);
                bhomLoadcases.Add(bhomLoadcase);
            }

            return bhomLoadcases;
        }
    }
}