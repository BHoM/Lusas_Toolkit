using BH.oM.Structure.Loads;
using Lusas.LPI;

namespace BH.Engine.Lusas
{
    public static partial class Convert
    {
        public static Loadcase ToBHoMLoadcase(this IFLoadcase lusasLoadcase)
        {
            Loadcase BHoMLoadcase = new Loadcase { Name = GetName(lusasLoadcase),
                Number = lusasLoadcase.getID()};

            int bhomID = GetBHoMID(lusasLoadcase, 'c');

            BHoMLoadcase.CustomData["Lusas_id"] = bhomID;
            return BHoMLoadcase;
        }
    }
}
