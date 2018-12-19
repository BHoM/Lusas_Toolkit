using System.Collections.Generic;
using System.Linq;
using Lusas.LPI;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        public int DeleteSurfaces(IEnumerable<object> ids)
        {
            int success = 1;

            if (ids != null)
            {
                List<int> indicies = ids.Cast<int>().ToList();

                foreach (int index in indicies)
                {
                    IFSurface lusasSurface = d_LusasData.getSurfaceByName("S" + index.ToString());
                    if (lusasSurface.getHOFs().Count() > 0)
                    {
                        //Engine.Reflection.Compute.RecordWarning("S" + index + @" has higher order features(HOFs)
                            //and cannot be deleted");
                    }
                    else
                    {
                        d_LusasData.Delete(lusasSurface);
                    }
                }
            }
            else
            {
                d_LusasData.deleteSurfaces();
            }

            return success;
        }
    }
}
