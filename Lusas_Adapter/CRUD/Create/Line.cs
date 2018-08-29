﻿using System.Collections.Generic;
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
                assignObjectSet(newLine, bar.Tags);
            }

            if(!(bar.SectionProperty == null))
            {
                if (!(bar.SectionProperty.Material == null))
                {
                    String materialName = "M" + bar.SectionProperty.Material.CustomData[AdapterId] + "/" + bar.SectionProperty.Material.Name;
                    IFAttribute lusasMaterial = d_LusasData.getAttribute("Material", materialName);
                    lusasMaterial.assignTo(newLine);
                }
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

            if (!(bar.SectionProperty == null))
            {
                if (!(bar.SectionProperty.Material == null))
                {
                    String materialName = "M" + bar.SectionProperty.Material.CustomData[AdapterId] + "/" + bar.SectionProperty.Material.Name;
                    IFAttribute lusasMaterial = d_LusasData.getAttribute("Material", materialName);
                    lusasMaterial.assignTo(newLine);
                }
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
