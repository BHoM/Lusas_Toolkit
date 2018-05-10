using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.oM.Structural.Elements;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        /***************************************************/
        /**** Adapter overload method                   ****/
        /***************************************************/

        protected override int Delete(Type type, IEnumerable<object> ids)
        {
            //Insert code here to enable deletion of specific types of objects with specific ids
            int success = 0;

            if (type == typeof(Node))
                success = DeleteNodes(ids);

            return 0;
        }

        public int DeleteNodes(IEnumerable<object> ids)
        {
            int success = 1;

            if (ids != null)
            {
                List<int> indicies = ids.Cast<int>().ToList();

                foreach (int ind in indicies)
                {
                    d_LusasData.deleteContents(d_LusasData.getPointByName(ind.ToString()));
                }
            }
            else
            {
                d_LusasData.deletePoints();
            }
            return success;
        }

        /***************************************************/
    }
}
