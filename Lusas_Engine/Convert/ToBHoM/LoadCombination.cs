using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.oM.Base;
using BH.oM.Geometry;
using BH.oM.Structure.Elements;
using BH.oM.Structure.Properties;
using BH.oM.Structure.Loads;
using BH.oM.Common.Materials;
using Lusas.LPI;

namespace BH.Engine.Lusas
{
    public static partial class Convert
    {
        public static LoadCombination ToBHoMLoadCombination(this IFBasicCombination lusasLoadCombination, Dictionary<string, Loadcase> loadcases)
        {

            object[] loadcaseIDs = lusasLoadCombination.getLoadcaseIDs();
            object[] loadcaseFactors = lusasLoadCombination.getFactors();

            List<Tuple<double, ICase>> factoredLoadcases = new List<Tuple<double, ICase>>();
            Loadcase bhomLoadcase = null;

            for (int i = 0; i < loadcaseIDs.Count(); i++)
            {
                int loadcaseID = (int)loadcaseIDs[i];
                double loadcaseFactor = (double)loadcaseFactors[i];
                loadcases.TryGetValue(loadcaseID.ToString(), out bhomLoadcase);
                ICase bhomICase = (ICase)bhomLoadcase;
                Tuple<double, ICase> factoredLoadcase = new Tuple<double, ICase>(loadcaseFactor, bhomICase);
                factoredLoadcases.Add(factoredLoadcase);
            }

            LoadCombination BHoMLoadCombination = new LoadCombination
            {
                Name = GetName(lusasLoadCombination),
                Number = lusasLoadCombination.getID(),
                LoadCases = factoredLoadcases
            };

            int bhomID = GetBHoMID(lusasLoadCombination, 'c');
            BHoMLoadCombination.CustomData["Lusas_id"] = bhomID;
            return BHoMLoadCombination;
        }
    }
}