/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2020, the respective contributors. All rights reserved.
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
using BH.Engine.Reflection;
using BH.oM.Structure.Elements;
using BH.oM.Structure.Constraints;
using BH.oM.Structure.SectionProperties;
using BH.oM.Structure.MaterialFragments;
using Lusas.LPI;
using BH.oM.Adapters.Lusas;
using System;
using BH.Adapter.Lusas;

namespace BH.Adapter.Adapters.Lusas
{
    /***************************************************/
    /**** Public Methods                            ****/
    /***************************************************/

    public static partial class Convert
    {
        public static Bar ToBar(this IFLine lusasLine,
            Dictionary<string, Node> bhomNodes,
            Dictionary<string, Constraint4DOF> bhomSupports,
            HashSet<string> lusasGroups,
            Dictionary<string, IMaterialFragment> bhomMaterials,
            Dictionary<string, ISectionProperty> bhomSections,
            Dictionary<string, MeshSettings1D> bhomMeshes
            )

        {
            Node startNode = LusasAdapter.GetNode(lusasLine, 0, bhomNodes);
            Node endNode = LusasAdapter.GetNode(lusasLine, 1, bhomNodes);

            HashSet<string> tags = new HashSet<string>(LusasAdapter.IsMemberOf(lusasLine, lusasGroups));

            List<string> supportAssignments = LusasAdapter.GetAttributeAssignments(lusasLine, "Support");

            Constraint4DOF barConstraint = null;
            if (!(supportAssignments.Count() == 0))
            {
                bhomSupports.TryGetValue(supportAssignments[0], out barConstraint);
            }

            Bar bhomBar = new Bar
            {
                StartNode = startNode,
                EndNode = endNode,
                Tags = tags,
                Support = barConstraint
            };

            List<string> geometricAssignments = LusasAdapter.GetAttributeAssignments(lusasLine, "Geometric");
            List<string> materialAssignments = LusasAdapter.GetAttributeAssignments(lusasLine, "Material");

            IMaterialFragment lineMaterial = null;
            ISectionProperty lineSection = null;

            if (!(geometricAssignments.Count() == 0))
            {
                bhomSections.TryGetValue(geometricAssignments[0], out lineSection);
                if (!(materialAssignments.Count() == 0))
                {
                    bhomMaterials.TryGetValue(materialAssignments[0], out lineMaterial);
                    lineSection.Material = lineMaterial;
                }
                bhomBar.SectionProperty = lineSection;
            }

            MeshSettings1D lineMesh = null;
            List<string> meshSettings = LusasAdapter.GetAttributeAssignments(lusasLine, "Mesh");

            if (!(meshSettings.Count() == 0))
            {
                bhomMeshes.TryGetValue(meshSettings[0], out lineMesh);
                bhomBar.CustomData["Mesh"] = lineMesh;
            }

            Tuple<bool, double, BarRelease, BarFEAType> barMeshProperties = GetMeshProperties(lusasLine);

            if (barMeshProperties.Item1)
            {
                bhomBar.OrientationAngle = barMeshProperties.Item2;
                bhomBar.Release = barMeshProperties.Item3;
                bhomBar.FEAType = barMeshProperties.Item4;
            }

            string adapterID = Engine.Adapters.Lusas.Modify.RemovePrefix(lusasLine.getName(), "L");

            bhomBar.CustomData[AdapterIdName] = adapterID;

            return bhomBar;
        }

        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        public static Tuple<bool, double, BarRelease, BarFEAType> GetMeshProperties(IFLine lusasLine)
        {
            bool meshAssigned = true;
            double betaAngle = 0;
            BarRelease barRelease = null;
            BarFEAType barType = BarFEAType.Flexural;

            object[] meshAssignments = lusasLine.getAssignments("Mesh");

            if (meshAssignments.Length > 0)
            {
                foreach (object assignment in meshAssignments)
                {
                    IFAssignment lusasAssignment = (IFAssignment)assignment;
                    IFAttribute lusasMesh = lusasAssignment.getAttribute();
                    IFMeshLine lusasLineMesh = (IFMeshLine)lusasMesh;
                    betaAngle = lusasAssignment.getBetaAngle();

                    barRelease = GetBarRelease(lusasLineMesh);

                    object[] barMeshName = lusasLineMesh.getElementNames();

                    foreach (object type in barMeshName)
                    {
                        barType = GetFEAType(type);
                    }
                }
            }
            else
                meshAssigned = false;

            Tuple<bool, double, BarRelease, BarFEAType> lineMeshProperties =
                new Tuple<bool, double, BarRelease, BarFEAType>(meshAssigned, betaAngle, barRelease, barType);

            return lineMeshProperties;
        }

        /***************************************************/

        private static BarRelease GetBarRelease(IFMeshLine lusasLineMesh)
        {
            object[] startReleases = lusasLineMesh.getValue("start");
            object[] endReleases = lusasLineMesh.getValue("end");
            List<DOFType> startReleaseType = GetConstraints(startReleases);
            List<DOFType> endReleaseType = GetConstraints(endReleases);

            Constraint6DOF startConstraint = SetConstraint(startReleaseType);
            Constraint6DOF endConstraint = SetConstraint(endReleaseType);

            BarRelease barRelease = new BarRelease
            {
                StartRelease = startConstraint,
                EndRelease = endConstraint
            };

            return barRelease;
        }

        /***************************************************/

        private static Constraint6DOF SetConstraint(List<DOFType> releaseType)
        {
            Constraint6DOF constraint = new Constraint6DOF
            {
                TranslationX = releaseType[0],
                TranslationY = releaseType[1],
                TranslationZ = releaseType[2],
                RotationX = releaseType[3],
                RotationY = releaseType[4],
                RotationZ = releaseType[5]
            };

            return constraint;
        }

        /***************************************************/

        private static List<DOFType> GetConstraints(object[] releases)
        {

            List<DOFType> releaseType = new List<DOFType>();

            if ((bool)releases[0] || (bool)releases[7] || (bool)releases[8])
            {
                releaseType = CheckPresets(releases);
            }
            else
            {
                for (int i = 1; i <= 7; i++)
                {
                    if ((bool)releases[i])
                        releaseType.Add(DOFType.Free);
                    else
                        releaseType.Add(DOFType.Fixed);
                }
            }

            return releaseType;
        }

        /***************************************************/

        private static List<DOFType> CheckPresets(object[] releases)
        {
            List<DOFType> releaseType = new List<DOFType>();

            if ((bool)releases[7])
            {
                List<DOFType> pinList = new List<DOFType>() { DOFType.Fixed, DOFType.Fixed, DOFType.Fixed,
                    DOFType.Fixed, DOFType.Free, DOFType.Free };
                releaseType.AddRange(pinList);
            }
            else if ((bool)releases[8])
            {
                List<DOFType> fixList = new List<DOFType>() { DOFType.Fixed, DOFType.Fixed, DOFType.Fixed,
                    DOFType.Fixed, DOFType.Fixed, DOFType.Fixed };
                releaseType.AddRange(fixList);
            }
            else if ((bool)releases[0])
            {
                Engine.Reflection.Compute.RecordWarning(
                    "Lusas joints are not supported in the BHoM, verify the constraint output is correct");
            }

            return releaseType;
        }

        /***************************************************/

        private static BarFEAType GetFEAType(object type)
        {
            BarFEAType barFEAType = BarFEAType.Flexural;

            if (
                                type.ToString() == "BMI21" ||
                                type.ToString() == "BMI31" ||
                                type.ToString() == "BMX21" ||
                                type.ToString() == "BMX31" ||
                                type.ToString() == "BMI21W" ||
                                type.ToString() == "BMI31W" ||
                                type.ToString() == "BMX21W" ||
                                type.ToString() == "BMX31W")
            {
                barFEAType = BarFEAType.Flexural;
            }
            else if (
                type.ToString() == "BRS2" ||
                type.ToString() == "BRS3")
                barFEAType = BarFEAType.Axial;
            else if (
                type.ToString() == "BS4" ||
                type.ToString() == "BSL4" ||
                type.ToString() == "BXL4" ||
                type.ToString() == "JSH4" ||
                type.ToString() == "JL43" ||
                type.ToString() == "JNT4" ||
                type.ToString() == "IPN4" ||
                type.ToString() == "IPN6" ||
                type.ToString() == "LMS3" ||
                type.ToString() == "LMS4")
            {
                Engine.Reflection.Compute.RecordWarning(
                    type.ToString() + " not supported, FEAType defaulted to Flexural");
            }

            return barFEAType;
        }

        /***************************************************/

    }
}

