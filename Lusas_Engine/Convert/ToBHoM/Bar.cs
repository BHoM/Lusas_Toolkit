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
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        //Add methods for converting to BHoM from the specific software types, if possible to do without any BHoM calls
        //Example:
        //public static Node ToBHoM(this LusasNode node)
        //{

        //#region Geometry Converters


        public static Bar ToBHoMBar(this IFLine lusasLine, Dictionary<string, Node> bhomNodes, HashSet<String> groupNames)
        {

            Node startNode = getNode(lusasLine, 0, bhomNodes);

            Node endNode = getNode(lusasLine, 1, bhomNodes);

            HashSet<String> tags = new HashSet<string>(isMemberOf(lusasLine, groupNames));

            Bar bhomBar = new Bar { StartNode = startNode, EndNode = endNode, Tags = tags };

            String lineName = removePrefix(lusasLine.getName(), "L");

            bhomBar.CustomData["Lusas_id"] = lineName;


            return bhomBar;
        }
    }
}
