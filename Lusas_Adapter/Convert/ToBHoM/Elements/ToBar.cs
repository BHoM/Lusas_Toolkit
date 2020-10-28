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
using BH.Engine.Adapter;

namespace BH.Adapter.Adapters.Lusas
{
    public static partial class Convert
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static Bar ToBar(this IFLine lusasLine,
            Dictionary<string, Node> nodes,
            Dictionary<string, Constraint4DOF> supports,
            HashSet<string> lusasGroups,
            Dictionary<string, IMaterialFragment> materials,
            Dictionary<string, ISectionProperty> sections,
            Dictionary<string, MeshSettings1D> meshes
            )

        {
            Node startNode = GetNode(lusasLine, 0, nodes);
            Node endNode = GetNode(lusasLine, 1, nodes);

            HashSet<string> tags = new HashSet<string>(IsMemberOf(lusasLine, lusasGroups));

            List<string> supportAssignments = GetAttributeAssignments(lusasLine, "Support");

            Constraint4DOF barConstraint = null;
            if (!(supportAssignments.Count() == 0))
            {
                supports.TryGetValue(supportAssignments[0], out barConstraint);
            }

            Bar bar = new Bar
            {
                StartNode = startNode,
                EndNode = endNode,
                Tags = tags,
                Support = barConstraint
            };

            List<string> geometricAssignments = GetAttributeAssignments(lusasLine, "Geometric");
            List<string> materialAssignments = GetAttributeAssignments(lusasLine, "Material");

            IMaterialFragment lineMaterial;
            ISectionProperty lineSection;

            if (!(geometricAssignments.Count() == 0))
            {
                sections.TryGetValue(geometricAssignments[0], out lineSection);
                if (!(materialAssignments.Count() == 0))
                {
                    materials.TryGetValue(materialAssignments[0], out lineMaterial);
                    lineSection.Material = lineMaterial;
                }
                bar.SectionProperty = lineSection;
            }

            MeshSettings1D lineMesh;
            List<string> meshSettings = GetAttributeAssignments(lusasLine, "Mesh");

            if (!(meshSettings.Count() == 0))
            {
                meshes.TryGetValue(meshSettings[0], out lineMesh);
                bar.CustomData["Mesh"] = lineMesh;
            }

            Tuple<bool, double, BarRelease, BarFEAType> barMeshProperties = GetMeshProperties(lusasLine);

            if (barMeshProperties.Item1)
            {
                bar.OrientationAngle = barMeshProperties.Item2;
                bar.Release = barMeshProperties.Item3;
                bar.FEAType = barMeshProperties.Item4;
            }

            string adapterID = lusasLine.getID().ToString();
            bar.SetAdapterId(typeof(LusasId), adapterID);

            return bar;
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

