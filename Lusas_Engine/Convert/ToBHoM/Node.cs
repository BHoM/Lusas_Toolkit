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
        public static Node ToBHoMObject(this IFPoint lusasPoint)
        {
            Node bhomNode = new Node { Position = { X = lusasPoint.getX(), Y = lusasPoint.getY(), Z = lusasPoint.getZ() } };

            String pointName = removePrefix(lusasPoint.getName(), "P");

            bhomNode.CustomData["Lusas_id"] = pointName;

            //Read tags from objectsets

            return bhomNode;
        }
    }
}

