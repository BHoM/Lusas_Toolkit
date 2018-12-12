using System.Collections.Generic;
using Lusas.LPI;
using BH.oM.Structure.Properties.Constraint;

namespace BH.Engine.Lusas
{
    public partial class Convert
    {
        public static BarRelease GetBarRelease(IFMeshLine lusasLineMesh)
        {
            object[] startReleases = lusasLineMesh.getValue("start");
            object[] endReleases = lusasLineMesh.getValue("end");

            List<DOFType> startReleaseType = GetConstraints(startReleases);
            List<DOFType> endReleaseType = GetConstraints(endReleases);

            Constraint6DOF startConstraint = SetConstraint(startReleaseType);
            Constraint6DOF endConstraint = SetConstraint(endReleaseType);

            BarRelease barRelease = new BarRelease
            {
                StartRelease = startConstraint,
                EndRelease = endConstraint
            };

            return barRelease;
        }
    }
}