using System.Collections.Generic;
using Lusas.LPI;

namespace BH.Engine.Lusas
{
    public partial class Convert
    {
        public static HashSet<string> IsMemberOf(IFGeometry lusasGeometry, HashSet<string> bhomTags)
        {

            HashSet<string> geometryTag = new HashSet<string>();

            foreach (string tag in bhomTags)
            {
                if (lusasGeometry.isMemberOfGroup(tag))
                {
                    geometryTag.Add(tag);
                }
            }

            return geometryTag;
        }
    }
}