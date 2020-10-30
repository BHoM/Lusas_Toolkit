using BH.Adapter.Lusas;
using BH.oM.Geometry;
using BH.Engine.Reflection;
using BH.oM.Structure.Constraints;
using BH.oM.Structure.Elements;
using BH.Engine.Units;
using System.Collections.Generic;
using System.Linq;

namespace BH.Adapter.Adapters.Lusas
{
    public static partial class Convert
    {
        public static double ForceToSI(this double force, string forceUnit)
        {
            switch (forceUnit)
            {
                case "N":
                    break;
                case "KN":
                    return force.FromKilonewton();
                case "KGF":
                    return force.FromKilogramForce();
                case "TONF":
                    return force.FromTonneForce();
                case "LBF":
                    return force.FromPoundForce();
                case "KIPS":
                    return force.FromKilopoundForce();
                default:
                    Compute.RecordWarning("No firce unit detected, Lusas force unit assumed to be set to metres. Therefore no unit conversion will occur. ");
                    break;
            }

            return force;
        }

        /***************************************************/

    }
}
