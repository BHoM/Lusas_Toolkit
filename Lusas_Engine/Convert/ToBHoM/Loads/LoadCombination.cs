using System;
using System.Collections.Generic;
using System.Linq;
using BH.oM.Structure.Loads;
using Lusas.LPI;

namespace BH.Engine.Lusas
{
    public static partial class Convert
    {
        public static LoadCombination ToBHoMLoadCombination(
            this IFBasicCombination lusasLoadCombination,
            Dictionary<string, Loadcase> bhomLoadcases)
        {
            object[] loadcaseIDs = lusasLoadCombination.getLoadcaseIDs();
            object[] loadcaseFactors = lusasLoadCombination.getFactors();

            List<Tuple<double, ICase>> factoredLoadcases = new List<Tuple<double, ICase>>();
            Loadcase bhomLoadcase = null;

            for (int i = 0; i < loadcaseIDs.Count(); i++)
            {
                int loadcaseID = (int)loadcaseIDs[i];
                double loadcaseFactor = (double)loadcaseFactors[i];
                bhomLoadcases.TryGetValue(loadcaseID.ToString(), out bhomLoadcase);
                ICase bhomICase = bhomLoadcase;
                Tuple<double, ICase> factoredLoadcase = new Tuple<double, ICase>(loadcaseFactor, bhomICase);
                factoredLoadcases.Add(factoredLoadcase);
            }

            LoadCombination BHoMLoadCombination = new LoadCombination
            {
                Name = GetName(lusasLoadCombination),
                Number = lusasLoadCombination.getID(),
                LoadCases = factoredLoadcases
            };

            int adapterID = GetAdapterID(lusasLoadCombination, 'c');
            BHoMLoadCombination.CustomData["Lusas_id"] = adapterID;

            return BHoMLoadCombination;
        }
    }
}