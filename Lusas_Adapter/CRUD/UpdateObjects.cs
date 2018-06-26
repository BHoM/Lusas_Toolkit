using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.oM.Base;
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
            bool success = true;
            if (objects.Count() > 0)
            {
                if (objects.First() is Node)
                {
                    success = Update(objects as IEnumerable<Node>);
                }
            }
            m_LusasApplication.updateAllViews();
            return success;
        }

        protected bool Update(IEnumerable<IBHoMObject> bhomObjects)
        {
            return true;
        }

        protected bool Update(IEnumerable<Node> nodes)
        {

            foreach (Node node in nodes)
            {
                IFPoint LusasNode = d_LusasData.getPointByName(node.Name);

                if (LusasNode == null)
                {
                    return false;
                }
                    
                if (!string.IsNullOrWhiteSpace(node.Name))
                {
                    d_LusasData.Delete(LusasNode);
                    createPoint(node);
                }
            }
            return true;
        }

        /***************************************************/
    }
}
