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
        public static Loadcase ToBHoMLoadcase(this IFLoadcase lusasLoadcase)
        {
            Loadcase BHoMLoadcase = new Loadcase { Name = getName(lusasLoadcase),
                Number = lusasLoadcase.getID()};

            int bhomID = getBHoMID(lusasLoadcase, 'c');

            BHoMLoadcase.CustomData["Lusas_id"] = bhomID;
            return BHoMLoadcase;
        }
    }
}
