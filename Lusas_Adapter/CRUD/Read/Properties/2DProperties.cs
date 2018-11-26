using System.Collections.Generic;
using System.Linq;
using BH.oM.Structure.Properties;
using Lusas.LPI;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        private List<IProperty2D> Read2DProperties(List<string> ids = null)
        {
            object[] lusasGeometrics = d_LusasData.getAttributes("Surface Geometric");
            List<IProperty2D> bhomProperties2D = new List<IProperty2D>();

            for (int i = 0; i < lusasGeometrics.Count(); i++)
            {
                IFAttribute lusasGeometric = (IFAttribute)lusasGeometrics[i];
                IProperty2D bhomProperty2D = Engine.Lusas.Convert.ToBHoMProperty2D(lusasGeometric);
                bhomProperties2D.Add(bhomProperty2D);
            }

            return bhomProperties2D;
        }
    }
}