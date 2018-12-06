using System.Collections.Generic;
using BH.oM.Structure.Elements;
using Lusas.LPI;
using BH.oM.Base;
using BH.oM.Structure.Loads;
using System;
using BH.oM.Structure.Properties;
using BH.Engine.Structure;

namespace BH.Engine.Lusas
{
    public partial class Convert
    {
        public static Tuple<bool,double, BarRelease, BarFEAType> GetMeshProperties(IFLine lusasLine)
        {
            bool meshAssigned = true;
            double betaAngle = 0;
            BarRelease barReleases = null;
            BarFEAType barType = BarFEAType.Flexural;

            object[] meshAssignments = lusasLine.getAssignments("Mesh");

            if (meshAssignments.Length > 0)
            {
                foreach (object assignment in meshAssignments)
                {
                    IFAssignment lusasAssignment = (IFAssignment) assignment;
                    IFAttribute lusasMesh = lusasAssignment.getAttribute();
                    IFMeshLine lusasLineMesh = (IFMeshLine)lusasMesh;
                    betaAngle = lusasAssignment.getBetaAngle();
                    object[] startReleases = lusasLineMesh.getValue("start");
                    object[] endReleases = lusasLineMesh.getValue("end");

                    List<DOFType> startReleaseType = new List<DOFType>();
                    List<DOFType> endReleaseType = new List<DOFType>();

                    for (int i = 0; i <= 6; i++)
                    {
                        if ((bool)startReleases[i])
                            startReleaseType.Add(DOFType.Free);
                        else
                            startReleaseType.Add(DOFType.Fixed);
                        if ((bool)endReleases[i])
                            endReleaseType.Add(DOFType.Free);
                        else
                            endReleaseType.Add(DOFType.Fixed);
                    }

                        Constraint6DOF startConstraint = new Constraint6DOF
                        {
                            TranslationX = startReleaseType[0],
                            TranslationY = startReleaseType[1],
                            TranslationZ = startReleaseType[2],
                            RotationX = startReleaseType[3],
                            RotationY = startReleaseType[4],
                            RotationZ = startReleaseType[5]
                        };

                        Constraint6DOF endConstraint = new Constraint6DOF
                        {
                            TranslationX = endReleaseType[0],
                            TranslationY = endReleaseType[1],
                            TranslationZ = endReleaseType[2],
                            RotationX = endReleaseType[3],
                            RotationY = endReleaseType[4],
                            RotationZ = endReleaseType[5]
                        };

                    barReleases = new BarRelease
                    {
                        StartRelease = startConstraint,
                        EndRelease = endConstraint
                    };

                    object[] barMeshName = lusasLineMesh.getElementNames();

                    foreach (object type in barMeshName)
                    {
                        if (type.ToString() == "BMI21")
                            continue;
                        else
                            barType = BarFEAType.Axial;
                    }
                }
            }
            else
                meshAssigned = false;

            Tuple<bool,double,BarRelease,BarFEAType> lineMeshProperties = new Tuple<bool,double, BarRelease,BarFEAType> (meshAssigned, betaAngle, barReleases,barType);

            return lineMeshProperties;
        }
    }
}
