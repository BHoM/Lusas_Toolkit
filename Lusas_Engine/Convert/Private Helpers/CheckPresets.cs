using System.Collections.Generic;
using BH.oM.Structure.Properties.Constraint;

namespace BH.Engine.Lusas
{
    public partial class Convert
    {
        public static List<DOFType> CheckPresets(object[] releases)
        {
            List<DOFType> releaseType = new List<DOFType>();

            if ((bool)releases[7])
            {
                List<DOFType> pinList = new List<DOFType>() { DOFType.Fixed, DOFType.Fixed, DOFType.Fixed,
                    DOFType.Fixed, DOFType.Free, DOFType.Free };
                releaseType.AddRange(pinList);
            }
            else if ((bool)releases[8])
            {
                List<DOFType> fixList = new List<DOFType>() { DOFType.Fixed, DOFType.Fixed, DOFType.Fixed,
                    DOFType.Fixed, DOFType.Fixed, DOFType.Fixed };
                releaseType.AddRange(fixList);
            }
            else if ((bool)releases[0])
            {
                Reflection.Compute.RecordWarning(
                    "Lusas joints are not supported in the BHoM, verify the constraint output is correct");
            }

            return releaseType;
        }
    }
}
