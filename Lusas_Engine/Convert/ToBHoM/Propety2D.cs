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
        public static IProperty2D ToBHoMProperty2D(this IFAttribute lusasAttribute)
        {
            string attributeName = GetName(lusasAttribute);

            IProperty2D bhomProperty2D = new ConstantThickness
            {
                Name = attributeName,
                Thickness = lusasAttribute.getValue("t")
            };

            
            int bhomID = GetBHoMID(lusasAttribute, 'G');

            bhomProperty2D.CustomData["Lusas_id"] = bhomID;

            return bhomProperty2D;
        }


    }
}
