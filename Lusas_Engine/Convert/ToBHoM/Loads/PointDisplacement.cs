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
        public static PointDisplacement ToPointDisplacement(IFLoading lusasPrescribedDisplacement,
            IEnumerable<IFAssignment> lusasAssignments, Dictionary<string, Node> bhomNodeDictionary)
        {
            IFLoadcase assignedLoadcase = (IFLoadcase)lusasAssignments.First().getAssignmentLoadset();
            Loadcase bhomLoadcase = ToBHoMLoadcase(assignedLoadcase);

            IEnumerable<Node> bhomNodes = GetNodeAssignments(lusasAssignments, bhomNodeDictionary);

            lusasPrescribedDisplacement.getValueNames();

            Vector translationVector = new Vector
            {
                X = lusasPrescribedDisplacement.getValue("U"),
                Y = lusasPrescribedDisplacement.getValue("V"),
                Z = lusasPrescribedDisplacement.getValue("W")
            };

            Vector rotationVector = new Vector
            {
                X = lusasPrescribedDisplacement.getValue("THX"),
                Y = lusasPrescribedDisplacement.getValue("THY"),
                Z = lusasPrescribedDisplacement.getValue("THZ")
            };

            PointDisplacement bhomPointDisplacement = Structure.Create.PointDisplacement(bhomLoadcase, bhomNodes, translationVector, rotationVector, LoadAxis.Global, GetName(lusasPrescribedDisplacement));

            int adapterID = GetAdapterID(lusasPrescribedDisplacement, 'd');
            bhomPointDisplacement.CustomData["Lusas_id"] = adapterID;
            // Needs to be a bit here that determines whether it is global or local - actually this cannot be done as the 
            //attribute is applied to a group, and within the group the axis could local or global

            return bhomPointDisplacement;
        }
    }
}