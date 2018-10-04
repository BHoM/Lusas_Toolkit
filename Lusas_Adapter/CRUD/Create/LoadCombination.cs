using System.Collections.Generic;
using System.Linq;
using System;
using BH.oM.Base;
using BH.oM.Structure.Elements;
using BH.oM.Geometry;
using BH.oM.Structure.Properties;
using BH.oM.Structure.Loads;
using BH.oM.Common.Materials;
using BH.Engine.Geometry;
using BH.Engine.Structure;
using Lusas.LPI;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        public IFBasicCombination CreateLoadCombination(LoadCombination loadCombination)
        {
            IFBasicCombination lusasLoadcombination = null;
            string lusasLoadCombinationName = "Lc" + loadCombination.CustomData[AdapterId] + "/" + loadCombination.Name;

            List<double> loadFactors = new List<double>();
            List<ICase> loadcases = new List<ICase>();

            if (d_LusasData.existsLoadset(lusasLoadCombinationName))
            {
                lusasLoadcombination = (IFBasicCombination)d_LusasData.getLoadset(lusasLoadCombinationName);
            }
            else
            {
                lusasLoadcombination = d_LusasData.createCombinationBasic(lusasLoadCombinationName,"", loadCombination.Number);
                foreach(Tuple<double,ICase> factoredLoad in loadCombination.LoadCases)
                {
                    loadFactors.Add(factoredLoad.Item1);
                    loadcases.Add(factoredLoad.Item2);
                }
                lusasLoadcombination.setValue("loadsetArray", loadcases);
                lusasLoadcombination.setValue("factorArray", loadFactors);
            }
            return lusasLoadcombination;
        }
    }
}