using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.oM.Base;
using BH.oM.Geometry;
using BH.oM.Structural.Elements;
using BH.oM.Structural.Properties;
using BH.oM.Common.Materials;
using LusasM15_2;
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


        public static Bar ToBHoMObject(this IFLine lusasLine, Dictionary<string, Node> bhomNodes, HashSet<String> groupNames, Dictionary<string, Material> bhomMaterials)
        {

            Node startNode = getNode(lusasLine, 0, bhomNodes);

            Node endNode = getNode(lusasLine, 1, bhomNodes);

            HashSet<String> tags = new HashSet<string>(isMemberOf(lusasLine, groupNames));

            Bar bhomBar = new Bar { StartNode = startNode, EndNode = endNode, Tags = tags };

            List<String> materialAssignments = attributeAssignments(lusasLine, "Material");

            Material barMaterial = null;
            if (!(materialAssignments.Count() == 0))
            {
                bhomMaterials.TryGetValue(materialAssignments[0], out barMaterial);
                bhomBar.SectionProperty.Material = barMaterial;
            }

            String lineName = removePrefix(lusasLine.getName(), "L");

            bhomBar.CustomData["Lusas_id"] = lineName;

            return bhomBar;
        }
    }
}
