using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.oM.Base;
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
            if (type == typeof(Node))
                return ReadNodes(ids as dynamic);
            else if (type == typeof(Bar))
                return ReadBars(ids as dynamic);
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

        private List<Node> ReadNodes(List<string> ids = null)
        {
        //    int maxpoint = d_LusasApplication.getLargestPointID();
        //    IFPoint lusaspoints = (IFPoint)d_LusasApplication.getGeometric("Point");

        //    for (int i=1;i<lusaspoints.L)
        //    d_LusasApplication.getLine()

        //    List<oM.Geometry.Point> bhomNodes = new List<oM.Geometry.Point>();

        //    for (int i =1; i<=lusasnodes
        //    Dim points As New List(Of Rhino.Geometry.Point3d)
        //    If(Not Da.GetDataList(0, points)) Then Return

        //    Dim geomData As LusasM15_2.IFGeometryData = modeller.geometryData()
        //    geomData.setAllDefaults()

        //    Dim group_name As String = ""
        //    If(Not Da.GetData(0, group_name)) Then Return

        //    Dim point_group As IFObjectSet

        //    'Check if geometry group exists
        //    If modeller.db.existsGroupByName(group_name) Then
        //        point_group = modeller.db.getGroupByName(group_name)
        //        point_group.Delete("Line")
        //        point_group.Delete("Point")
        //    Else
        //        point_group = modeller.db.createGroup(group_name)
        //    End If

        //    For i = 0 To points.Count - 1
        //        'Abort on invalid igrnputs.
        //        If Not points(i).IsValid Then Return
        //        geomData.addCoords(points(i).X, points(i).Y, points(i).Z)
        //    Next

        //    Dim pointDB As IFObjectSet = modeller.db.createPoint(geomData)

        //    point_group.add(pointDB)

            throw new NotImplementedException();
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
