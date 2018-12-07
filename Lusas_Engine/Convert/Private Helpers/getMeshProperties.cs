using BH.oM.Structure.Elements;
using Lusas.LPI;
using System;
using BH.oM.Structure.Properties;

namespace BH.Engine.Lusas
{
    public partial class Convert
    {
        public static Tuple<bool, double, BarRelease, BarFEAType> GetMeshProperties(IFLine lusasLine)
        {
            bool meshAssigned = true;
            double betaAngle = 0;
            BarRelease barRelease = null;
            BarFEAType barType = BarFEAType.Flexural;

            object[] meshAssignments = lusasLine.getAssignments("Mesh");

            if (meshAssignments.Length > 0)
            {
                foreach (object assignment in meshAssignments)
                {
                    IFAssignment lusasAssignment = (IFAssignment)assignment;
                    IFAttribute lusasMesh = lusasAssignment.getAttribute();
                    IFMeshLine lusasLineMesh = (IFMeshLine)lusasMesh;
                    betaAngle = lusasAssignment.getBetaAngle();

                    barRelease = GetBarRelease(lusasLineMesh);

                    object[] barMeshName = lusasLineMesh.getElementNames();

                    foreach (object type in barMeshName)
                    {
                        barType = GetFEAType(type);
                    }
                }
            }
            else
                meshAssigned = false;

            Tuple<bool, double, BarRelease, BarFEAType> lineMeshProperties =
                new Tuple<bool, double, BarRelease, BarFEAType>(meshAssigned, betaAngle, barRelease, barType);

            return lineMeshProperties;
        }
    }
}
