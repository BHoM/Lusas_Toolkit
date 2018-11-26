using System.Collections.Generic;
using System.Linq;
using BH.oM.Structure.Loads;
using Lusas.LPI;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        private List<LoadCombination> ReadLoadCombinations(List<string> ids = null)
        {
            List<LoadCombination> bhomLoadCombintations = new List<LoadCombination>();

            object[] lusasCombinations = d_LusasData.getLoadsets("Combinations");

            List<Loadcase> lusasLoadcases = ReadLoadcases();
            Dictionary<string, Loadcase> loadcaseDictionary = lusasLoadcases.ToDictionary(
                x => x.Number.ToString());

            for (int i = 0; i < lusasCombinations.Count(); i++)
            {
                IFBasicCombination lusasCombination = (IFBasicCombination)lusasCombinations[i];
                LoadCombination bhomLoadCombination =
                    Engine.Lusas.Convert.ToBHoMLoadCombination(lusasCombination, loadcaseDictionary);
                bhomLoadCombintations.Add(bhomLoadCombination);
            }

            return bhomLoadCombintations;
        }
    }
}