using System.Collections.Generic;
using BH.oM.Geometry;
using Lusas.LPI;

namespace BH.Engine.Lusas
{
    public static partial class Convert
    {
        public static Point ToBHoMPoint(this IFPoint lusasPoint, HashSet<string> groupNames)
        {
            HashSet<string> tags = new HashSet<string>(IsMemberOf(lusasPoint, groupNames));

            Point bhomPoint = new Point { X = lusasPoint.getX(), Y = lusasPoint.getY(), Z = lusasPoint.getZ() };

            //string pointName = removePrefix(lusasPoint.getName(), "P");

            //bhomPoint.CustomData["Lusas_id"] = pointName;

            //Read tags from objectsets

            return bhomPoint;
        }

        public static Point ToBHoMPoint(this IFPoint lusasPoint)
        {
            Point bhomPoint = new Point { X = lusasPoint.getX(), Y = lusasPoint.getY(), Z = lusasPoint.getZ() };

            return bhomPoint;
        }
    }
}
