using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.oM.Base;
using BH.oM.Geometry;
using BH.oM.Structural.Elements;
using BH.oM.Structural.Properties;
using BH.oM.Common.Materials;
using LusasM15_2;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        /***************************************************/
        /**** Adapter overload method                   ****/
        /***************************************************/
        protected override IEnumerable<IBHoMObject> Read(Type type, IList ids)
        {
            //Choose what to pull out depending on the type. Also see example methods below for pulling out bars and dependencies
            if (type == typeof(Bar))
                return ReadBars(ids as dynamic);
            else if (type == typeof(Point))
                return ReadPoints(ids as dynamic);
            else if (type == typeof(ISectionProperty) || type.GetInterfaces().Contains(typeof(ISectionProperty)))
                return ReadSectionProperties(ids as dynamic);
            else if (type == typeof(Material))
                return ReadMaterials(ids as dynamic);

            return null;
        }

        /***************************************************/
        /**** Private specific read methods             ****/
        /***************************************************/

        //The List<string> in the methods below can be changed to a list of any type of identification more suitable for the toolkit

        private List<Bar> ReadBars(List<string> ids = null)
        {
            //Implement code for reading bars
            throw new NotImplementedException();
        }

        /***************************************/

        private List<Point> ReadPoints(List<string> ids = null)
        {
            int maxPointID = d_LusasData.getLargestPointID();
            List<Point> bhomPoints = new List<Point>();

            for (int i = 1; i <= maxPointID; i++)
            {
                if (d_LusasData.existsPointByID(i))
                {
                    IFPoint LusasPoint = d_LusasData.getPointByNumber(i);
                    double[] pointcoords = new double[] { 0, 0, 0 };
                    LusasPoint.getXYZ(pointcoords);
                    Point bhomPoint = BH.Engine.Lusas.Convert.ToBHoMGeometry(pointcoords[1], pointcoords[2], pointcoords[3]);
                    bhomPoints.Add(bhomPoint);
                }
            }
            return bhomPoints;
         }

        /***************************************/

        private List<ISectionProperty> ReadSectionProperties(List<string> ids = null)
        {
            //Implement code for reading section properties
            throw new NotImplementedException();
        }

        /***************************************/

        private List<Material> ReadMaterials(List<string> ids = null)
        {
            //Implement code for reading materials
            throw new NotImplementedException();
        }

        /***************************************************/


    }
}
