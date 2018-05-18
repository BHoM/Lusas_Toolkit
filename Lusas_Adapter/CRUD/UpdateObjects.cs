using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.oM.Structural.Elements;
using BH.oM.Geometry;
using BH.oM.Structural.Properties;
using BH.oM.Structural.Loads;
using BH.oM.Common.Materials;
using LusasM15_2;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        /***************************************************/
        /**** Adapter overload method                   ****/
        /***************************************************/

        //Method being called for any object already existing in the model in terms of comparers is found.
        //Default implementation first deletes these objects, then creates new ones, if not applicable for the software, override this method

        protected override bool UpdateObjects<T>(IEnumerable<T> objects)
        {
            return base.UpdateObjects<T>(objects);
        }

        protected bool Update(IEnumerable<Node> nodes)
        {

            foreach (Node node in nodes)
            {
                IFPoint LusasNode = d_LusasData.getPointByName(node.CustomData[AdapterId].ToString());

                if (LusasNode == null)
                {
                    return false;
                }
                    
                if (!string.IsNullOrWhiteSpace(node.Constraint.Name))
                {
                    d_LusasData.Delete(LusasNode);
                    createpoint(node);
                }
            }
            return true;
        }

        /***************************************************/
    }
}
