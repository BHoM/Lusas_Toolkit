using System.Collections.Generic;
using System.Linq;
using BH.oM.Geometry;
using BH.oM.Structure.Elements;
using BH.oM.Structure.Loads;
using Lusas.LPI;

namespace BH.Engine.Lusas
{
    public static partial class Convert
    {
        public static PointDisplacement ToPointDisplacement(IFLoading lusasPrescribedDisplacement, IEnumerable<IFAssignment> assignmentList, Dictionary<string, Node> nodes)
        {
            IFLoadcase assignedLoadcase = (IFLoadcase)assignmentList.First().getAssignmentLoadset();
            Loadcase bhomLoadcase = BH.Engine.Lusas.Convert.ToBHoMLoadcase(assignedLoadcase);

            IEnumerable<Node> bhomNodes = GetNodeAssignments(assignmentList, nodes);

            lusasPrescribedDisplacement.getValueNames();

            Vector translationVector = new Vector { X = lusasPrescribedDisplacement.getValue("U"), Y = lusasPrescribedDisplacement.getValue("V"), Z = lusasPrescribedDisplacement.getValue("W") };
            Vector rotationVector = new Vector { X = lusasPrescribedDisplacement.getValue("THX"), Y = lusasPrescribedDisplacement.getValue("THY"), Z = lusasPrescribedDisplacement.getValue("THZ") };

            PointDisplacement bhomPointDisplacement = BH.Engine.Structure.Create.PointDisplacement(bhomLoadcase, bhomNodes, translationVector, rotationVector, LoadAxis.Global, GetName(lusasPrescribedDisplacement));

            int bhomID = GetBHoMID(lusasPrescribedDisplacement, 'd');
            bhomPointDisplacement.CustomData["Lusas_id"] = bhomID;
            return bhomPointDisplacement;

            // Needs to be a bit here that determines whether it is global or local - actually this cannot be done as the 
            //attribute is applied to a group, and within the group the axis could local or global

        }
    }
}