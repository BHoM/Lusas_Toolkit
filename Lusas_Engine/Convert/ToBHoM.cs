using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.oM.Structural.Elements;
using BH.oM.Geometry;

namespace BH.Engine.Lusas
{
    public static partial class Convert
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        //Add methods for converting to BHoM from the specific software types, if possible to do without any BHoM calls
        //Example:
        //public static Node ToBHoM(this LusasNode node)
        //{

        //#region Geometry Converters

        public static BH.oM.Geometry.Point ToBHoMGeometry(double PX, double PY, double PZ)
        {
            return new oM.Geometry.Point { X = PX, Y = PY, Z = PZ };
        }

        public static Node ToBHoMObject(double PX, double PY, double PZ)
        {
            return new Node { Position = new Point { X = PX, Y = PY, Z = PZ } };
        }

        //}

        /***************************************************/
    }
}
