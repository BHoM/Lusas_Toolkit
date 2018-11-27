using System.Collections.Generic;
using System.Linq;
using BH.oM.Base;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        public void CreateTags(IEnumerable<IBHoMObject> bhomObject)
        {
            List<string> objectTags = bhomObject.SelectMany(x => x.Tags).Distinct().ToList();

            foreach (string tag in objectTags)
            {
                if (!d_LusasData.existsGroupByName(tag))
                {
                    d_LusasData.createGroup(tag);
                }
            }
        }
    }
}