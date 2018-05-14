using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.oM.Structural.Elements;
using BH.oM.Geometry;
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
            Node startNode = null;
            
            IFPoint startPoint = lusasLine.getLOFs()[0];
            bhomNodes.TryGetValue(startPoint.getID().ToString(), out startNode);
            Node endNode = null;
            IFPoint endPoint = lusasLine.getLOFs()[1];
            bhomNodes.TryGetValue(endPoint.getID().ToString(), out endNode);
            Bar bhomBar = new Bar { StartNode = startNode, EndNode = endNode, Name = lusasLine.getName() };

            bhomBar.CustomData[AdapterId] = lusasLine.getID();

            return bhomBar;
        }

        public static Node ToBHoMObject(this IFPoint lusasPoint)
        {
            Node newNode = new Node { Position = { X = lusasPoint.getX(), Y = lusasPoint.getY(), Z = lusasPoint.getZ() }, Name = lusasPoint.getName() };
            newNode.CustomData[AdapterId] = lusasPoint.getID();

            return newNode;
        }


        //

        /***************************************************/
    }
}
