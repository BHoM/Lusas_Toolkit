using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.oM.Structure.Elements;
using BH.oM.Geometry;
using BH.Engine.Geometry;
using Lusas.LPI;

namespace BH.Engine.Lusas
{
    public static partial class Convert
    {
        public static Point ToBHoMPoint(this IFPoint lusasPoint, HashSet<String> groupNames)
        {
            HashSet<String> tags = new HashSet<string>(IsMemberOf(lusasPoint, groupNames));

            Point bhomPoint = new Point { X = lusasPoint.getX(), Y = lusasPoint.getY(), Z = lusasPoint.getZ() };

            //String pointName = removePrefix(lusasPoint.getName(), "P");

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
