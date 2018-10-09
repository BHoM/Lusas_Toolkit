using System.Collections.Generic;
using System.Linq;
using System;
using BH.oM.Base;
using BH.oM.Structure.Elements;
using BH.oM.Geometry;
using BH.oM.Structure.Properties;
using BH.oM.Structure.Loads;
using BH.oM.Common.Materials;
using BH.Engine.Geometry;
using BH.Engine.Structure;
using Lusas.LPI;
namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        public IFLine CreateLine(Bar bar)
        {
            IFLine newLine;

            IFPoint startPoint = d_LusasData.getPointByName(bar.StartNode.CustomData[AdapterId].ToString());
            IFPoint endPoint = d_LusasData.getPointByName(bar.EndNode.CustomData[AdapterId].ToString());
            newLine = d_LusasData.createLineByPoints(startPoint, endPoint);
            newLine.setName("L" + bar.CustomData[AdapterId]);

            if (!(bar.Tags.Count == 0))
            {
                AssignObjectSet(newLine, bar.Tags);
            }

            if (!(bar.SectionProperty == null))
            {
                if (!(bar.SectionProperty.Material == null))
                {
                    string materialName = "M" + bar.SectionProperty.Material.CustomData[AdapterId] + "/" + bar.SectionProperty.Material.Name;
                    IFAttribute lusasMaterial = d_LusasData.getAttribute("Material", materialName);
                    lusasMaterial.assignTo(newLine);
                }
            }

            if (!(bar.Spring == null))
            {
                string supportName = "Sp" + bar.Spring.CustomData[AdapterId] + "/" + bar.Spring.Name;
                IFAttribute lusasSupport = d_LusasData.getAttribute("Support", supportName);
                lusasSupport.assignTo(newLine);
                object lineXAxis = null;
                object lineYAxis = null;
                object lineZAxis = null;
                object origin = null;

                newLine.getAxesAtNrmCrds(0.5, ref origin, ref lineXAxis, ref lineYAxis, ref lineZAxis);

                double[] localXAxis = ConvertToDouble(lineXAxis);
                double[] localYAxis = ConvertToDouble(lineYAxis);
                double[] localZAxis = ConvertToDouble(lineZAxis);

                Point barStart = bar.StartNode.Position;
                double[] worldXAxis = new double[] { 1, 0, 0 };
                double[] worldYAxis = new double[] { 0, 1, 0 };
                double[] worldZAxis = new double[] { 0, 0, 1 };

                double[] barorigin = new double[] { barStart.X, barStart.Y, barStart.Z };

                double[] matrixCol0 = new double[]
                {
                worldXAxis.Zip(localXAxis, (d1,d2) => d1 * d2).Sum(),
                worldYAxis.Zip(localXAxis, (d1,d2) => d1 * d2).Sum(),
                worldZAxis.Zip(localXAxis, (d1,d2) => d1 * d2).Sum(),
                };

                double[] matrixCol1 = new double[]
                {
                worldXAxis.Zip(localYAxis, (d1,d2) => d1 * d2).Sum(),
                worldYAxis.Zip(localYAxis, (d1,d2) => d1 * d2).Sum(),
                worldZAxis.Zip(localYAxis, (d1,d2) => d1 * d2).Sum(),
                };

                double[] matrixCol2 = new double[]
                {
                worldXAxis.Zip(localZAxis, (d1,d2) => d1 * d2).Sum(),
                worldYAxis.Zip(localZAxis, (d1,d2) => d1 * d2).Sum(),
                worldZAxis.Zip(localZAxis, (d1,d2) => d1 * d2).Sum(),
                };

                IFLocalCoord barLocalAxis = d_LusasData.createLocalCartesianAttr(
                    "LocalCoordinateTest",
                    barorigin,
                    matrixCol0,
                    matrixCol1,
                    matrixCol2
                    );

                barLocalAxis.assignTo(newLine);

            }



            return newLine;

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
            IFLine newLine;

            int bhomID;
            if (bar.CustomData.ContainsKey(AdapterId))
                bhomID = System.Convert.ToInt32(bar.CustomData[AdapterId]);
            else
                bhomID = System.Convert.ToInt32(NextId(bar.GetType()));

            bar.CustomData[AdapterId] = bhomID;

            if (d_LusasData.existsLineByName("L" + bar.CustomData[AdapterId]))
            {
                newLine = d_LusasData.getLineByName("L" + bar.CustomData[AdapterId]);
            }
            else
            {
                newLine = d_LusasData.createLineByPoints(startPoint, endPoint);
                newLine.setName("L" + bar.CustomData[AdapterId]);
            }

            if (!(bar.Tags.Count == 0))
            {
                AssignObjectSet(newLine, bar.Tags);
            }

            if (!(bar.SectionProperty == null))
            {
                if (!(bar.SectionProperty.Material == null))
                {
                    String materialName = "M" + bar.SectionProperty.Material.CustomData[AdapterId] + "/" + bar.SectionProperty.Material.Name;
                    IFAttribute lusasMaterial = d_LusasData.getAttribute("Material", materialName);
                    lusasMaterial.assignTo(newLine);
                }
            }

            if (!(bar.Spring == null))
            {
                string supportName = "Sp" + bar.Spring.CustomData[AdapterId] + "/" + bar.Spring.Name;
                IFAttribute lusasSupport = d_LusasData.getAttribute("Support", supportName);
                lusasSupport.assignTo(newLine);
            }

            return newLine;
        }

        //public IFLine CreateLine(Line line)
        //{
        //    Node startNode = new Node { Position = line.StartPoint() };
        //    Node endNode = new Node { Position = line.EndPoint() };
        //    Bar newBar = new Bar { StartNode = startNode, EndNode = endNode };
        //    IFLine newLine = CreateLine(newBar);
        //    return newLine;
        //}

        public IFLine CreateLine(ICurve iCurve, IFPoint startPoint, IFPoint endPoint)
        {
            Node startNode = new Node { Position = iCurve.IStartPoint() };
            Node endNode = new Node { Position = iCurve.IEndPoint() };
            Bar newBar = new Bar { StartNode = startNode, EndNode = endNode };
            IFLine newLine = CreateLine(newBar, startPoint, endPoint);
            return newLine;
        }
    }



}
