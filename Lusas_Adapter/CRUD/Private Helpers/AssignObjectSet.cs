using System.Collections.Generic;
using Lusas.LPI;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        public void AssignObjectSet(IFGeometry newGeometry, HashSet<string> tags)
        {
            foreach (string tag in tags)
            {
                IFObjectSet objectSet = d_LusasData.getGroupByName(tag);
                objectSet.add(newGeometry);
            }
        }
    }
}