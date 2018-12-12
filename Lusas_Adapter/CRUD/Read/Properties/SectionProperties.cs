using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BH.oM.Base;
using BH.oM.Geometry;
using BH.oM.Structure.Elements;
using BH.oM.Structure.Properties.Section;
using BH.oM.Structure.Loads;
using BH.oM.Common.Materials;
using Lusas.LPI;
using BH.oM.Adapters.Lusas;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        private List<ISectionProperty> ReadSectionProperties(List<string> ids = null)
        {
            object[] lusasSections = d_LusasData.getAttributes("Line Geometric");
            List<ISectionProperty> bhomSections = new List<ISectionProperty>();

            for (int i = 0; i < lusasSections.Count(); i++)
            {
                IFAttribute lusasSection = (IFAttribute)lusasSections[i];
                ISectionProperty bhomSection = Engine.Lusas.Convert.ToBHoMSection(lusasSection);
                bhomSections.Add(bhomSection);
            }
            return bhomSections;
        }
    }
}