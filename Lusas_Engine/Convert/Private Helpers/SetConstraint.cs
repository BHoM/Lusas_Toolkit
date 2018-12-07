using System.Collections.Generic;
using BH.oM.Structure.Properties;

namespace BH.Engine.Lusas
{
    public partial class Convert
    {
        public static Constraint6DOF SetConstraint(List<DOFType> releaseType)
        {
            Constraint6DOF constraint = new Constraint6DOF
            {
                TranslationX = releaseType[0],
                TranslationY = releaseType[1],
                TranslationZ = releaseType[2],
                RotationX = releaseType[3],
                RotationY = releaseType[4],
                RotationZ = releaseType[5]
            };

            return constraint;
        }
    }
}