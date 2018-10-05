using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.oM.Base;
using BH.oM.Geometry;
using BH.oM.Structure.Elements;
using BH.oM.Structure.Properties;
using BH.oM.Common.Materials;
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
        //ToBHoMObject(thise ToBHoM(this LusasNode node)
        //{

        //#region Geometry Converters


        public static Bar ToBHoMBar(this IFLine lusasLine, 
            Dictionary<string, Node> bhomNodes, 
            HashSet<String> groupNames, 
            Dictionary<string, Material> bhomMaterials)

        {

            Node startNode = GetNode(lusasLine, 0, bhomNodes);

            Node endNode = GetNode(lusasLine, 1, bhomNodes);

            HashSet<String> tags = new HashSet<string>(IsMemberOf(lusasLine, groupNames));

            Bar bhomBar = new Bar { StartNode = startNode, EndNode = endNode, Tags = tags };

            List<String> materialAssignments = AttributeAssignments(lusasLine, "Material");

            //This will be wrapped in with the SectionProperties when they are defined
            //Material barMaterial = null;
            //if (!(materialAssignments.Count() == 0))
            //{
            //    bhomMaterials.TryGetValue(materialAssignments[0], out barMaterial);
            //    bhomBar.SectionProperty.Material = barMaterial;
            //}

            String lineName = removePrefix(lusasLine.getName(), "L");

            bhomBar.CustomData["Lusas_id"] = lineName;

            return bhomBar;
        }
    }
}
