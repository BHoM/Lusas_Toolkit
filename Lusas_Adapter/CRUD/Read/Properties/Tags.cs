using System.Collections.Generic;
using System.Linq;
using Lusas.LPI;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        private HashSet<string> ReadTags(List<string> ids = null)
        {
            object[] lusasGroups = d_LusasData.getObjects("Groups");
            HashSet<string> bhomTags = new HashSet<string>();

            for (int i = 0; i < lusasGroups.Count(); i++)
            {
                IFGroup lusasGroup = (IFGroup)lusasGroups[i];
                bhomTags.Add(lusasGroup.getName());
            }

            return bhomTags;
        }
    }
}