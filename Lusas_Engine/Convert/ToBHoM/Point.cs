using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.oM.Structural.Elements;
using BH.oM.Geometry;
using BH.Engine.Geometry;
using LusasM15_2;

namespace BH.Engine.Lusas
{
    public static partial class Convert
    {
        public static Point ToBHoMGeom(this IFPoint lusasPoint, HashSet<String> groupNames)
        {
            HashSet<String> tags = new HashSet<string>(isMemberOf(lusasPoint, groupNames));

            Point bhomPoint = new Point { X = lusasPoint.getX(), Y = lusasPoint.getY(), Z = lusasPoint.getZ() }; 

            //String pointName = removePrefix(lusasPoint.getName(), "P");

            //bhomPoint.CustomData["Lusas_id"] = pointName;

            //Read tags from objectsets

            return bhomPoint;
        }
    }
}
