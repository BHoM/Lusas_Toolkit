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
        public IFLine CreateLine(Bar bar, List<Bar> existingBars)
        {
            IFLine newLine;

            int bhomID;
            if (bar.CustomData.ContainsKey(AdapterId))
                bhomID = System.Convert.ToInt32(bar.CustomData[AdapterId]);
            else
                bhomID = System.Convert.ToInt32(NextId(bar.GetType()));

            bar.CustomData[AdapterId] = bhomID;

            int position = existingBars.FindIndex(m =>
                            Math.Round(m.Geometry().IPointAtParameter(0.5).X, 3).Equals(Math.Round(bar.Geometry().IPointAtParameter(0.5).X, 3)) &&
                            Math.Round(m.Geometry().IPointAtParameter(0.5).Y, 3).Equals(Math.Round(bar.Geometry().IPointAtParameter(0.5).Y, 3)) &&
                            Math.Round(m.Geometry().IPointAtParameter(0.5).Z, 3).Equals(Math.Round(bar.Geometry().IPointAtParameter(0.5).Z, 3)));

            if (position == -1)
            {
                IFPoint startPoint = CreatePoint(bar.StartNode);
                IFPoint endPoint = CreatePoint(bar.EndNode);
                newLine = d_LusasData.createLineByPoints(startPoint, endPoint);
                newLine.setName("L" + bar.CustomData[AdapterId]);
            }
            else
            {
                newLine = d_LusasData.getLineByName("L" + existingBars[position].CustomData[AdapterId].ToString());
            }

            if (!(bar.Tags.Count == 0))
            {
                assignObjectSet(newLine, bar.Tags);
            }
            return newLine;
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
                assignObjectSet(newLine, bar.Tags);
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
