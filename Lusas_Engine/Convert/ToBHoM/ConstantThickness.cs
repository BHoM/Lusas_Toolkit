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
        public static ConstantThickness ToBHoMConstantThickness(this IFAttribute lusasAttribute)
        {
            string attributeName = getName(lusasAttribute);

            ConstantThickness bhomThickness = new ConstantThickness
            {
                Name = attributeName,
                Thickness = lusasAttribute.getValue("t")
            };

            
            int bhomID = getBHoMID(lusasAttribute, 'T');

            bhomThickness.CustomData["Lusas_id"] = bhomID;

            return bhomThickness;
        }


    }
}
