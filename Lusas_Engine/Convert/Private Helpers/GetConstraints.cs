using System.Collections.Generic;
using BH.oM.Structure.Properties;

namespace BH.Engine.Lusas
{
    public partial class Convert
    {
        public static List<DOFType> GetConstraints(object[] releases)
        {

            List<DOFType> releaseType = new List<DOFType>();

            if ((bool)releases[0] || (bool)releases[7] || (bool)releases[8])
            {
                releaseType = CheckPresets(releases);
            }
            else
            {
                for (int i = 1; i <= 7; i++)
                {
                    if ((bool)releases[i])
                        releaseType.Add(DOFType.Free);
                    else
                        releaseType.Add(DOFType.Fixed);
                }
            }

            return releaseType;
        }
    }
}