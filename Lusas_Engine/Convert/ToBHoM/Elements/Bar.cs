using System.Collections.Generic;
using System.Linq;
using BH.oM.Structure.Elements;
using BH.oM.Structure.Properties;
using BH.oM.Common.Materials;
using Lusas.LPI;

namespace BH.Engine.Lusas
{
    public static partial class Convert
    {
        public static Bar ToBHoMBar(this IFLine lusasLine, 
            Dictionary<string, Node> bhomNodes, 
            Dictionary<string, Constraint4DOF> bhomSupports,
            HashSet<string> lusasGroups,
            Dictionary<string, Material> bhomMaterials,
            Dictionary<string, ISectionProperty> bhomSections
            )

        {

            Node startNode = GetNode(lusasLine, 0, bhomNodes);

            Node endNode = GetNode(lusasLine, 1, bhomNodes);

            HashSet<string> tags = new HashSet<string>(IsMemberOf(lusasLine, lusasGroups));

            List<string> supportAssignments = AttributeAssignments(lusasLine, "Support");

            Constraint4DOF barConstraint = null;
            if (!(supportAssignments.Count() == 0))
            {
                bhomSupports.TryGetValue(supportAssignments[0], out barConstraint);
            }

            Bar bhomBar = new Bar { StartNode = startNode,
                EndNode = endNode,
                Tags = tags,
                Spring = barConstraint};

            List<string> geometricAssignments = AttributeAssignments(lusasLine, "Geometric");
            List<string> materialAssignments = AttributeAssignments(lusasLine, "Material");

            Material lineMaterial = null;
            ISectionProperty lineSection = null;

            if (!(geometricAssignments.Count() == 0))
            {
                bhomSections.TryGetValue(geometricAssignments[0], out lineSection);
                if (!(materialAssignments.Count() == 0))
                {
                    bhomMaterials.TryGetValue(materialAssignments[0], out lineMaterial);
                    lineSection.Material = lineMaterial;
                }
                bhomBar.SectionProperty = lineSection;
            }

            string adapterID = RemovePrefix(lusasLine.getName(), "L");

            bhomBar.CustomData["Lusas_id"] = adapterID;

            return bhomBar;
        }
    }
}
