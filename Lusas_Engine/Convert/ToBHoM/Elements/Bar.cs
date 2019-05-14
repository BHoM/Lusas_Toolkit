/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2019, the respective contributors. All rights reserved.
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
using BH.oM.Structure.Constraints;
using BH.oM.Structure.SectionProperties;
using BH.oM.Physical.Materials;
using BH.oM.Structure.MaterialFragments;
using Lusas.LPI;
using BH.oM.Adapters.Lusas;
using System;

namespace BH.Engine.Lusas
{
    public static partial class Convert
    {
        public static Bar ToBHoMBar(this IFLine lusasLine, 
            Dictionary<string, Node> bhomNodes, 
            Dictionary<string, Constraint4DOF> bhomSupports,
            HashSet<string> lusasGroups,
            Dictionary<string, Material> bhomMaterials,
            Dictionary<string, ISectionProperty> bhomSections,
            Dictionary<string, MeshSettings1D> bhomMeshes
            )

        {
            Node startNode = Engine.Lusas.Query.GetNode(lusasLine, 0, bhomNodes);

            Node endNode = Engine.Lusas.Query.GetNode(lusasLine, 1, bhomNodes);

            HashSet<string> tags = new HashSet<string>(Query.IsMemberOf(lusasLine, lusasGroups));

            List<string> supportAssignments = Lusas.Query.GetAttributeAssignments(lusasLine, "Support");

            Constraint4DOF barConstraint = null;
            if (!(supportAssignments.Count() == 0))
            {
                bhomSupports.TryGetValue(supportAssignments[0], out barConstraint);
            }

            Bar bhomBar = new Bar { StartNode = startNode,
                EndNode = endNode,
                Tags = tags,
                Spring = barConstraint};

            List<string> geometricAssignments = Lusas.Query.GetAttributeAssignments(lusasLine, "Geometric");
            List<string> materialAssignments = Lusas.Query.GetAttributeAssignments(lusasLine, "Material");

            Material lineMaterial = null;
            ISectionProperty lineSection = null;

            if (!(geometricAssignments.Count() == 0))
            {
                bhomSections.TryGetValue(geometricAssignments[0], out lineSection);
                if (!(materialAssignments.Count() == 0))
                {
                    bhomMaterials.TryGetValue(materialAssignments[0], out lineMaterial);
                    lineSection.Material = (IStructuralMaterial)lineMaterial;
                }
                bhomBar.SectionProperty = lineSection;
            }

            MeshSettings1D lineMesh = null;
            List<string> meshSettings = Lusas.Query.GetAttributeAssignments(lusasLine, "Mesh");

            if (!(meshSettings.Count()==0))
            {
                bhomMeshes.TryGetValue(meshSettings[0], out lineMesh);
                bhomBar.CustomData["Mesh"] = lineMesh;
            }

            Tuple<bool,double, BarRelease, BarFEAType> barMeshProperties = Lusas.Query.GetMeshProperties(lusasLine);

            if (barMeshProperties.Item1)
            {
                bhomBar.OrientationAngle = barMeshProperties.Item2;
                bhomBar.Release = barMeshProperties.Item3;
                bhomBar.FEAType = barMeshProperties.Item4;
            }

            string adapterID = Engine.Lusas.Modify.RemovePrefix(lusasLine.getName(), "L");

            bhomBar.CustomData["Lusas_id"] = adapterID;

            return bhomBar;
        }
    }
}
