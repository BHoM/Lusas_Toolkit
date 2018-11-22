using System.Collections.Generic;
using System;
using BH.oM.Structure.Loads;
using Lusas.LPI;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        public IFBasicCombination CreateLoadCombination(LoadCombination loadCombination)
        {
            IFBasicCombination lusasLoadcombination = null;
            string lusasLoadCombinationName = "Lc" + loadCombination.CustomData[AdapterId] +
                "/" + loadCombination.Name;

            List<double> loadFactors = new List<double>();
            List<int> loadcases = new List<int>();

            if (d_LusasData.existsLoadset(lusasLoadCombinationName))
            {
                lusasLoadcombination = (IFBasicCombination)d_LusasData.getLoadset(lusasLoadCombinationName);
            }
            else
            {
                lusasLoadcombination = d_LusasData.createCombinationBasic(lusasLoadCombinationName, "",
                    loadCombination.Number);
                foreach (Tuple<double, ICase> factoredLoad in loadCombination.LoadCases)
                {
                    string lusasAttributeName = "Lc" + factoredLoad.Item2.CustomData[AdapterId] + "/"
                        + factoredLoad.Item2.Name;
                    double factor = factoredLoad.Item1;
                    IFLoadset lusasLoadcase = d_LusasData.getLoadset(lusasAttributeName);
                    lusasLoadcombination.addEntry(factor, lusasLoadcase);
                }
            }

            return lusasLoadcombination;
        }
    }
}