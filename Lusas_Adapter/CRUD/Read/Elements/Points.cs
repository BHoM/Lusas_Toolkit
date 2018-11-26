using System.Collections.Generic;
using System.Linq;
using BH.oM.Geometry;
using Lusas.LPI;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        private List<Point> ReadPoints(List<string> ids = null)
        {
            object[] lusasPoints = d_LusasData.getObjects("Point");

            List<Point> bhomPoints = new List<Point>();
            HashSet<string> groupNames = ReadTags();

            for (int i = 0; i < lusasPoints.Count(); i++)
            {
                IFPoint lusasPoint = (IFPoint)lusasPoints[i];
                Point bhomPoint = Engine.Lusas.Convert.ToBHoMPoint(lusasPoint, groupNames);
                bhomPoints.Add(bhomPoint);
            }
            return bhomPoints;
        }
    }
}