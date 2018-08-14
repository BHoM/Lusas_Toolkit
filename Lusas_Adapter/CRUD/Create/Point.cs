using System.Collections.Generic;
using System.Linq;
using System;
using BH.oM.Base;
using BH.oM.Structural.Elements;
using BH.oM.Geometry;
using BH.oM.Structural.Properties;
using BH.oM.Structural.Loads;
using BH.oM.Common.Materials;
using BH.Engine.Geometry;
using BH.Engine.Structure;
using LusasM15_2;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {

        public IFPoint CreatePoint(Node node)
        {
            IFPoint newPoint;

            int bhomID;
            if (node.CustomData.ContainsKey(AdapterId))
                bhomID = System.Convert.ToInt32(node.CustomData[AdapterId]);
            else
                bhomID = System.Convert.ToInt32(NextId(node.GetType()));

            node.CustomData[AdapterId] = bhomID;

            if (d_LusasData.existsPointByName("P" + node.CustomData[AdapterId]))
            {
                newPoint = d_LusasData.getPointByName("P" + node.CustomData[AdapterId]);
            }
            else
            {
                IFGeometryData geomData = m_LusasApplication.geometryData();
                geomData.setAllDefaults();
                geomData.addCoords(node.Position.X, node.Position.Y, node.Position.Z);
                IFDatabaseOperations database_point = d_LusasData.createPoint(geomData);
                newPoint = d_LusasData.getPointByNumber(d_LusasData.getLargestPointID());
                newPoint.setName("P" + node.CustomData[AdapterId].ToString());
            }

            if (!(node.Tags.Count == 0))
            {
                assignObjectSet(newPoint, node.Tags);
            }

            if (!(node.Constraint == null))
            {
                String constraintName = "Sp" + node.Constraint.CustomData[AdapterId] + "/" + node.Constraint.Name;
                IFAttribute lusasSupport = d_LusasData.getAttribute("Support", constraintName);
                lusasSupport.assignTo(newPoint);
            }

            return newPoint;
        }

        public IFPoint CreatePoint(Point point)
        {
            Node newNode = new Node { Position = point };
            IFPoint newPoint = null;

            newPoint = CreatePoint(newNode);

            return newPoint;
        }

    }
}
