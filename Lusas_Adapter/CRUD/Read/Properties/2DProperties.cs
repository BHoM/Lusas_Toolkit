using System.Collections.Generic;
using System.Linq;
using BH.oM.Structure.Properties.Surface;
using Lusas.LPI;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        private List<ISurfaceProperty> Read2DProperties(List<string> ids = null)
        {
            object[] lusasGeometrics = d_LusasData.getAttributes("Surface Geometric");
            List<ISurfaceProperty> bhomProperties2D = new List<ISurfaceProperty>();

            for (int i = 0; i < lusasGeometrics.Count(); i++)
            {
                IFAttribute lusasGeometric = (IFAttribute)lusasGeometrics[i];
                ISurfaceProperty bhomProperty2D = Engine.Lusas.Convert.ToBHoMProperty2D(lusasGeometric);
                bhomProperties2D.Add(bhomProperty2D);
            }

            return bhomProperties2D;
        }
    }
}