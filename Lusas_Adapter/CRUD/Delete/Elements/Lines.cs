using System.Collections.Generic;
using System.Linq;
using Lusas.LPI;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        public int DeleteLines(IEnumerable<object> ids)
        {
            int success = 1;

            if (ids != null)
            {
                List<int> indicies = ids.Cast<int>().ToList();

                foreach (int index in indicies)
                {
                    IFLine lusasLine = d_LusasData.getLineByName("L" + index.ToString());
                    if (lusasLine.getHOFs().Count() > 0)
                    {
                        //Engine.Reflection.Compute.RecordWarning("L" + index + @" has higher order features(HOFs)
                            //and cannot be deleted");
                    }
                    else
                    {
                        d_LusasData.Delete(lusasLine);
                    }
                }
            }
            else
            {
                d_LusasData.deleteLines();
            }

            return success;
        }
    }
}
