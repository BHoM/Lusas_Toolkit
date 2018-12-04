using System.Collections.Generic;
using System.Linq;
using BH.oM.Structure.Elements;
using BH.oM.Geometry;
using BH.Engine.Geometry;
using Lusas.LPI;
using BH.oM.Adapters.Lusas;
namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        public IFLine CreateLine(Bar bar)
        {
            IFPoint startPoint = d_LusasData.getPointByName(bar.StartNode.CustomData[AdapterId].ToString());
            IFPoint endPoint = d_LusasData.getPointByName(bar.EndNode.CustomData[AdapterId].ToString());
            IFLine lusasLine = d_LusasData.createLineByPoints(startPoint, endPoint);
            lusasLine.setName("L" + bar.CustomData[AdapterId]);

            if (!(bar.Tags.Count == 0))
            {
                AssignObjectSet(lusasLine, bar.Tags);
            }

            if (!(bar.SectionProperty == null))
            {
                string geometricLineName =
                    "G" + bar.SectionProperty.CustomData[AdapterId] + "/" + bar.SectionProperty.Name;

                IFAttribute lusasGeometricLine = d_LusasData.getAttribute("Line Geometric", geometricLineName);
                lusasGeometricLine.assignTo(lusasLine);
                if (!(bar.SectionProperty.Material == null))
                {
                    string materialName =
                        "M" + bar.SectionProperty.Material.CustomData[AdapterId] + "/" +
                        bar.SectionProperty.Material.Name;

                    IFAttribute lusasMaterial = d_LusasData.getAttribute("Material", materialName);
                    lusasMaterial.assignTo(lusasLine);
                }
            }

            if (!(bar.Spring == null))
            {
                string supportName = "Sp" + bar.Spring.CustomData[AdapterId] + "/" + bar.Spring.Name;
                IFAttribute lusasSupport = d_LusasData.getAttribute("Support", supportName);
                lusasSupport.assignTo(lusasLine);
                IFLocalCoord barLocalAxis = CreateLocalCoordinate(lusasLine);
                barLocalAxis.assignTo(lusasLine);
            }

            if (bar.CustomData["Mesh"]!=null)
            {
                MeshSettings1D associatedMesh = (MeshSettings1D) bar.CustomData["Mesh"];
                string meshName = "Me" + associatedMesh.CustomData[AdapterId] + "/" + associatedMesh.Name;
                IFAttribute lusasLineMesh = d_LusasData.getAttribute("Mesh", meshName);
                lusasLineMesh.assignTo(lusasLine);
            }

            return lusasLine;

        }

        public double[] ConvertToDouble(object objectAxis)
        {
            object[] axis = (object[])objectAxis;
            List<double> axisList = new List<double>();
            for (int i = 0; i < axis.Count(); i++)
            {
                double castAxis = (double)axis[i];
                axisList.Add(castAxis);
            }

            return axisList.ToArray();
        }

        public IFLine CreateLine(Bar bar, IFPoint startPoint, IFPoint endPoint)
        {
            IFLine lusasLine = null;

            int adapterID;
            if (bar.CustomData.ContainsKey(AdapterId))
               adapterID= System.Convert.ToInt32(bar.CustomData[AdapterId]);
            else
               adapterID= System.Convert.ToInt32(NextId(bar.GetType()));

            bar.CustomData[AdapterId] = adapterID;

            if (d_LusasData.existsLineByName("L" + bar.CustomData[AdapterId]))
            {
                lusasLine = d_LusasData.getLineByName("L" + bar.CustomData[AdapterId]);
            }
            else
            {
                lusasLine = d_LusasData.createLineByPoints(startPoint, endPoint);
                lusasLine.setName("L" + bar.CustomData[AdapterId]);
            }

            if (!(bar.Tags.Count == 0))
            {
                AssignObjectSet(lusasLine, bar.Tags);
            }

            if (!(bar.SectionProperty == null))
            {
                if (!(bar.SectionProperty.Material == null))
                {
                    string materialName = "M" + bar.SectionProperty.Material.CustomData[AdapterId] +
                        "/" + bar.SectionProperty.Material.Name;

                    IFAttribute lusasMaterial = d_LusasData.getAttribute("Material", materialName);
                    lusasMaterial.assignTo(lusasLine);
                }
            }

            if (!(bar.Spring == null))
            {
                string supportName = "Sp" + bar.Spring.CustomData[AdapterId] + "/" + bar.Spring.Name;
                IFAttribute lusasSupport = d_LusasData.getAttribute("Support", supportName);
                lusasSupport.assignTo(lusasLine);
            }

            return lusasLine;
        }

        public IFLine CreateLine(ICurve iCurve, IFPoint startPoint, IFPoint endPoint)
        {
            Node startNode = new Node { Position = iCurve.IStartPoint() };
            Node endNode = new Node { Position = iCurve.IEndPoint() };
            Bar bhomBar = new Bar { StartNode = startNode, EndNode = endNode };
            IFLine lusasLine = CreateLine(bhomBar, startPoint, endPoint);
            return lusasLine;
        }
    }



}
