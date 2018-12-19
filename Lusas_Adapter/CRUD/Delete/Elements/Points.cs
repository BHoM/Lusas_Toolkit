using System.Collections.Generic;
using System.Linq;
using Lusas.LPI;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        public int DeletePoints(IEnumerable<object> ids)
        {
            int success = 1;

            if (ids != null)
            {
                List<int> indicies = ids.Cast<int>().ToList();

                foreach (int index in indicies)
                {
                    IFPoint lusasPoint = d_LusasData.getPointByName("P" + index.ToString());
                    if (lusasPoint.getHOFs().Count() > 0)
                    {
                        //Engine.Reflection.Compute.RecordWarning("P" + index + @" has higher order features(HOFs)
                            //and cannot be deleted");
                    }
                    else
                    {
                        d_LusasData.Delete(lusasPoint);
                    }
                }
            }
            else
            {
                d_LusasData.deletePoints();
            }

            return success;
        }
    }
}
