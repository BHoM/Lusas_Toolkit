using BH.oM.Structure.Properties;
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
