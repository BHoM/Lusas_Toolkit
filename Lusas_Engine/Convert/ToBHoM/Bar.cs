﻿using System;
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
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        //Add methods for converting to BHoM from the specific software types, if possible to do without any BHoM calls
        //Example:
        //public static Node ToBHoM(this LusasNode node)
        //{

        //#region Geometry Converters


        public static Bar ToBHoMObject(this IFLine lusasLine, Dictionary<string, Node> bhomNodes)
        {

            Node startNode = getNode(lusasLine, 0, bhomNodes);

            Node endNode = getNode(lusasLine, 1, bhomNodes);

            Bar bhomBar = new Bar { StartNode = startNode, EndNode = endNode };

            String lineName = removePrefix(lusasLine.getName(), "L");

            bhomBar.CustomData["Lusas_id"] = lineName;

            //Read tags from objectsets


            return bhomBar;
        }
    }
}
