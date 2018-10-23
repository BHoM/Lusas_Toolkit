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
using BH.oM.Common.Materials;
using Lusas.LPI;

namespace BH.Engine.Lusas
{
    public static partial class Convert
    {
        public static ISectionProperty ToBHoMSection(this IFAttribute lusasAttribute)
        {
            string attributeName = GetName(lusasAttribute);

            lusasAttribute.getValueNames();

            ISectionProperty bhomSection = null;

            bhomSection.Name = attributeName;

            int bhomID = GetBHoMID(lusasAttribute, 'M');

            bhomSection.CustomData["Lusas_id"] = bhomID;

            return bhomSection;
        }
    }
}