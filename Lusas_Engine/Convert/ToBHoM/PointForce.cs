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
        public static PointForce ToPointForce(
            IFLoading lusasPointForce, IEnumerable<IFAssignment> lusasAssignments, 
            Dictionary<string, Node> bhomNodeDictionary)
        {
            IFLoadcase assignedLoadcase = (IFLoadcase)lusasAssignments.First().getAssignmentLoadset();
            Loadcase bhomLoadcase = ToBHoMLoadcase(assignedLoadcase);

            IEnumerable<Node> bhomNodes = GetNodeAssignments(lusasAssignments, bhomNodeDictionary);

            Vector forceVector = new Vector
            {
                X = lusasPointForce.getValue("px"),
                Y = lusasPointForce.getValue("py"),
                Z = lusasPointForce.getValue("pz")
            };

            Vector momentVector = new Vector
            {
                X = lusasPointForce.getValue("mx"),
                Y = lusasPointForce.getValue("my"),
                Z = lusasPointForce.getValue("mz")
            };

            PointForce bhomPointForce = Structure.Create.PointForce(
                bhomLoadcase,
                bhomNodes,
                forceVector,
                momentVector,
                LoadAxis.Global,
                GetName(lusasPointForce));

            int adapterID = GetAdapterID(lusasPointForce, 'l');
            bhomPointForce.CustomData["Lusas_id"] = adapterID;

            return bhomPointForce;
        }
    }
}