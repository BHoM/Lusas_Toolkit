using BH.oM.Structure.Properties.Surface;
using Lusas.LPI;

namespace BH.Engine.Lusas
{
    public static partial class Convert
    {
        public static ISurfaceProperty ToBHoMProperty2D(this IFAttribute lusasAttribute)
        {
            string attributeName = GetName(lusasAttribute);

            ISurfaceProperty bhomProperty2D = new ConstantThickness
            {
                Name = attributeName,
                Thickness = lusasAttribute.getValue("t")
            };

            int adapterID = GetAdapterID(lusasAttribute, 'G');

            bhomProperty2D.CustomData["Lusas_id"] = adapterID;

            return bhomProperty2D;
        }
    }
}
