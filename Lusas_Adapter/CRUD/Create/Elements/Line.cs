/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2023, the respective contributors. All rights reserved.
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
using BH.oM.Adapters.Lusas;
using BH.oM.Structure.Elements;
using BH.oM.Structure.MaterialFragments;
using BH.Engine.Adapter;
using BH.Engine.Geometry;
using Lusas.LPI;
using BH.Engine.Structure;
using BH.oM.Adapters.Lusas.Fragments;
using BH.Engine.Base;

namespace BH.Adapter.Lusas
{
#if Debug18 || Release18
    public partial class LusasV18Adapter
#elif Debug19 || Release19
    public partial class LusasV19Adapter
#else
    public partial class LusasV17Adapter
#endif
    {
        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        private IFLine CreateLine(Bar bar)
        {
            if (!CheckPropertyError(bar, b => b.StartNode, true) || !CheckPropertyError(bar, b => b.EndNode, true) ||
                !CheckPropertyError(bar, b => b.StartNode.Position, true) || !CheckPropertyError(bar, b => b.EndNode.Position, true))
            {
                return null;
            }

            if (
                bar.FEAType == BarFEAType.CompressionOnly ||
                bar.FEAType == BarFEAType.TensionOnly)
            {
                Engine.Base.Compute.RecordError("Lusas does not support " + bar.FEAType.ToString() + " Bars");
                return null;
            }

            string startNodeId = GetAdapterId<string>(bar.StartNode);
            string endNodeId = GetAdapterId<string>(bar.EndNode);

            if (string.IsNullOrEmpty(startNodeId) || string.IsNullOrEmpty(endNodeId))
            {
                Engine.Base.Compute.RecordError("Could not find the ids for at least one end node for at least one Bar. Bar not created.");
                return null;
            }

            IFPoint startPoint = d_LusasData.getPointByNumber(bar.StartNode.AdapterId<int>(typeof(LusasId)));
            IFPoint endPoint = d_LusasData.getPointByNumber(bar.EndNode.AdapterId<int>(typeof(LusasId)));
            IFLine lusasLine = d_LusasData.createLineByPoints(startPoint, endPoint);

            int adapterIdName = lusasLine.getID();
            bar.SetAdapterId(typeof(LusasId), adapterIdName);

            if (bar.Tags.Count != 0)
            {
                AssignObjectSet(lusasLine, bar.Tags);
            }

            if (CheckPropertyWarning(bar, b => b.SectionProperty) && !Engine.Adapters.Lusas.Query.InvalidSectionProperty(bar.SectionProperty))
                if (!Engine.Adapters.Lusas.Query.InvalidSectionProfile(bar.SectionProperty))
                {
                    //Needed in case the SectionProfile is null and was not created
                    if (d_LusasData.existsAttribute("Line Geometric", bar.SectionProperty.AdapterId<int>(typeof(LusasId))))
                    {
                        IFAttribute lusasGeometricLine = d_LusasData.getAttribute("Line Geometric", bar.SectionProperty.AdapterId<int>(typeof(LusasId)));
                        lusasGeometricLine.assignTo(lusasLine);

                    }

                    if (CheckPropertyWarning(bar, b => b.SectionProperty.Material))
                    {
                        if (bar.SectionProperty.Material is IOrthotropic)
                        {
                            Engine.Base.Compute.RecordWarning($"Orthotropic Material {bar.SectionProperty.Material.DescriptionOrName()} cannot be assigned to Bar {bar.AdapterId<int>(typeof(LusasId))}, " +
                                $"orthotropic materials can only be applied to 2D and 3D elements in Lusas.");
                        }
                        else if(bar.SectionProperty.Material is IIsotropic)
                        {
                            if(d_LusasData.existsAttribute("Material", bar.SectionProperty.Material.AdapterId<int>(typeof(LusasId))))
                            {
                                IFAttribute lusasMaterial = d_LusasData.getAttribute("Material", bar.SectionProperty.Material.AdapterId<int>(typeof(LusasId)));
                                lusasMaterial.assignTo(lusasLine);
                            }
                        }
                    }
                }

            if (bar.Support != null)
            {
                IFAttribute lusasSupport = d_LusasData.getAttribute("Support", System.Convert.ToInt32(bar.Support.AdapterId<int>(typeof(LusasId))));
                lusasSupport.assignTo(lusasLine);
                IFLocalCoord barLocalAxis = CreateLocalCoordinate(lusasLine);
                barLocalAxis.assignTo(lusasLine);
            }

            if (bar.Fragments.Contains(typeof(MeshSettings1D)))
            {
                IFAssignment meshAssignment = m_LusasApplication.newAssignment();
                meshAssignment.setAllDefaults();
                if (bar.OrientationAngle != 0 && bar.FEAType == BarFEAType.Axial)
                {
                    Engine.Base.Compute.RecordWarning(
                        "Orientation angle not supported in Lusas for " + bar.FEAType +
                        " element types, this information will be lost when pushed to Lusas");
                }

                meshAssignment.setBetaAngle(bar.OrientationAngle);

                MeshSettings1D meshSettings1D = bar.FindFragment<MeshSettings1D>();
                IFMeshAttr mesh = d_LusasData.getMesh(
                    meshSettings1D.Name + "\\" + bar.FEAType.ToString() + "|" + CreateReleaseString(bar.Release));
                mesh.assignTo(lusasLine, meshAssignment);
            }

            if (bar.Offset != null)
                Engine.Base.Compute.RecordWarning("Offsets are currently unsupported.");

            return lusasLine;

        }

        /***************************************************/

    }
}




