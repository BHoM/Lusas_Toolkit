/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2018, the respective contributors. All rights reserved.
 *
 * Each contributor holds copyright over their respective contributions.
 * The project versioning (Git) records all such contribution source information.
 *                                           
 *                                                                              
 * The BHoM is free software: you can redistribute it and/or modify         
 * it under the terms of the GNU Lesser General Public License as published by  
 * the Free Software Foundation, either version 3.0 of the License, or          
 * (at your option) any later version.                                          
 *                                                                              
 * The BHoM is distributed in the hope that it will be useful,              
 * but WITHOUT ANY WARRANTY; without even the implied warranty of               
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the                 
 * GNU Lesser General Public License for more details.                          
 *                                                                            
 * You should have received a copy of the GNU Lesser General Public License     
 * along with this code. If not, see <https://www.gnu.org/licenses/lgpl-3.0.html>.      
 */

using System.Collections.Generic;
using System.Linq;
using BH.oM.Structure.Elements;
using BH.oM.Geometry;
using BH.Engine.Geometry;
using Lusas.LPI;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        public IFLine CreateLine(Bar bar, IFMeshLine mesh)
        {
            if (
                bar.FEAType == BarFEAType.CompressionOnly ||
                bar.FEAType == BarFEAType.TensionOnly)
            {
                Engine.Reflection.Compute.RecordError(
                    "Lusas does not support " + bar.FEAType.ToString() + " bars");
                return null;
            }

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

            IFAssignment meshAssignment = m_LusasApplication.newAssignment();
            meshAssignment.setAllDefaults();

            if (bar.CustomData.ContainsKey("Mesh"))
            {
                if (bar.OrientationAngle != 0 && bar.FEAType == BarFEAType.Axial)
                {
                    Engine.Reflection.Compute.RecordWarning(
                        "Orientation angle not supported in Lusas for " + bar.FEAType +
                        " element types, this information will be lost when pushed to Lusas");
                }
                meshAssignment.setBetaAngle(bar.OrientationAngle);
                mesh.assignTo(lusasLine, meshAssignment);
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
            if (
                bar.FEAType == BarFEAType.CompressionOnly ||
                bar.FEAType == BarFEAType.TensionOnly)
            {
                Engine.Reflection.Compute.RecordError(
                    "Lusas does not support " + bar.FEAType.ToString() + " bars");
                return null;
            }

            IFLine lusasLine = null;

            int adapterID;
            if (bar.CustomData.ContainsKey(AdapterId))
                adapterID = System.Convert.ToInt32(bar.CustomData[AdapterId]);
            else
                adapterID = System.Convert.ToInt32(NextId(bar.GetType()));

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
