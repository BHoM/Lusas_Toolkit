using System.Collections.Generic;
using System;
using BH.oM.Structure.Loads;
using Lusas.LPI;
using BH.Engine.Reflection;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        public IFBasicCombination CreateLoadCombination(LoadCombination loadCombination)
        {
            if (!CheckIllegalCharacters(loadCombination.Name))
            {
                return null;
            }

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
                if(loadCombination.Number == 0)
                {
                    lusasLoadcombination = d_LusasData.createCombinationBasic(lusasLoadCombinationName);
                    Compute.RecordWarning("0 used for LoadCombination number,"
                        + "therefore LoadCombination number will not be forced");
                }
                else
                {
                    lusasLoadcombination = d_LusasData.createCombinationBasic(lusasLoadCombinationName, "",
                        loadCombination.Number);
                }
                foreach (Tuple<double, ICase> factoredLoad in loadCombination.LoadCases)
                {
                    string lusasName = "Lc" + factoredLoad.Item2.CustomData[AdapterId] + "/"
                        + factoredLoad.Item2.Name;
                    double factor = factoredLoad.Item1;
                    IFLoadset lusasLoadcase = d_LusasData.getLoadset(lusasName);
                    lusasLoadcombination.addEntry(factor, lusasLoadcase);
                }
            }

            return lusasLoadcombination;
        }
    }
}