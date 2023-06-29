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

using Lusas.LPI;
using BH.oM.Adapters.Lusas;
using BH.oM.Adapters.Lusas.Fragments;
using BH.oM.Structure.Elements;
using BH.oM.Structure.Constraints;
using BH.Engine.Adapter;

namespace BH.Adapter.Lusas
{
#if Debug18 || Release18
    public partial class LusasV18Adapter
#elif Debug19 || Release19
    public partial class LusasV19Adapter
#elif Debug191 || Release191
    public partial class LusasV191Adapter
#elif Debug200 || Release200
    public partial class LusasV200Adapter
#else
    public partial class LusasV17Adapter
#endif
    {
        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        private IFMeshLine CreateMeshSettings1D(MeshSettings1D meshSettings1D, BarFEAType barFEAType = BarFEAType.Flexural, BarRelease barRelease = null)
        {
            if (!Engine.Adapters.Lusas.Query.CheckIllegalCharacters(meshSettings1D.Name))
            {
                return null;
            }

            if (barRelease != null && barFEAType == BarFEAType.Axial)
            {
                Engine.Base.Compute.RecordWarning(
                    barFEAType + " used with barReleases, this information will be lost when pushed to Lusas");
            }
            else if (barRelease == null)
            {
                barRelease = Engine.Structure.Create.BarReleaseFixFix();
            }

            string releaseString = CreateReleaseString(barRelease);

            IFMeshLine lusasLineMesh;
            string lusasName = meshSettings1D.Name + "\\" + barFEAType.ToString() + "|" + releaseString;

            if (d_LusasData.existsAttribute("Mesh", lusasName))
            {
                lusasLineMesh = (IFMeshLine)d_LusasData.getAttribute("Mesh", lusasName);
            }
            else
            {
                lusasLineMesh = d_LusasData.createMeshLine(lusasName);
                SetSplitMethod(lusasLineMesh, meshSettings1D, barFEAType);
                if (barRelease != null)
                    SetEndConditions(lusasLineMesh, barRelease);
            }

            if (lusasLineMesh != null)
            {
                long adapterNameId = d_LusasData.getLargestAttributeID("Mesh");
                meshSettings1D.SetAdapterId(typeof(LusasId), adapterNameId);

                return lusasLineMesh;
            }

            return null;
        }

        /***************************************************/

        private static string CreateReleaseString(BarRelease barRelease)
        {
            string releaseString = "";

            if (barRelease != null)
                if (barRelease.StartRelease != null && barRelease.EndRelease != null)
                {
                    if (barRelease.StartRelease.TranslationX == DOFType.Free)
                        releaseString = releaseString + "FX";
                    if (barRelease.StartRelease.TranslationY == DOFType.Free)
                        releaseString = releaseString + "FY";
                    if (barRelease.StartRelease.TranslationZ == DOFType.Free)
                        releaseString = releaseString + "FZ";
                    if (barRelease.StartRelease.RotationX == DOFType.Free)
                        releaseString = releaseString + "MX";
                    if (barRelease.StartRelease.RotationY == DOFType.Free)
                        releaseString = releaseString + "MY";
                    if (barRelease.StartRelease.RotationZ == DOFType.Free)
                        releaseString = releaseString + "MZ";

                    if (releaseString != "")
                        releaseString = releaseString + ",";

                    if (barRelease.EndRelease.TranslationX == DOFType.Free)
                        releaseString = releaseString + "FX";
                    if (barRelease.EndRelease.TranslationY == DOFType.Free)
                        releaseString = releaseString + "FY";
                    if (barRelease.EndRelease.TranslationZ == DOFType.Free)
                        releaseString = releaseString + "FZ";
                    if (barRelease.EndRelease.RotationX == DOFType.Free)
                        releaseString = releaseString + "MX";
                    if (barRelease.EndRelease.RotationY == DOFType.Free)
                        releaseString = releaseString + "MY";
                    if (barRelease.EndRelease.RotationZ == DOFType.Free)
                        releaseString = releaseString + "MZ";
                }

            return releaseString;
        }

        /***************************************************/

        private static void SetSplitMethod(IFMeshLine lusasLineMesh, MeshSettings1D meshSettings1D, BarFEAType barFEAType)
        {
            if (meshSettings1D.SplitMethod == Split1D.Length)
            {
                if (barFEAType == BarFEAType.Axial)
                    lusasLineMesh.setSize("BRS2", meshSettings1D.SplitParameter);
                else if (barFEAType == BarFEAType.Flexural)
                    lusasLineMesh.setSize("BMI21", meshSettings1D.SplitParameter);
            }
            else if (meshSettings1D.SplitMethod == Split1D.Automatic)
            {
                lusasLineMesh.setValue("uiSpacing", "uniform");
                SetElementType(lusasLineMesh, barFEAType);
            }
            else if (meshSettings1D.SplitMethod == Split1D.Divisions)
            {
                lusasLineMesh.addSpacing(System.Convert.ToInt32(meshSettings1D.SplitParameter), 1);
                SetElementType(lusasLineMesh, barFEAType);
            }
        }

        /***************************************************/

        private static void SetElementType(IFMeshLine lusasLineMesh, BarFEAType barFEAType)
        {
            if (barFEAType == BarFEAType.Axial)
                lusasLineMesh.addElementName("BRS2");
            else if (barFEAType == BarFEAType.Flexural)
                lusasLineMesh.addElementName("BMI21");
        }

        /***************************************************/

        private static void SetEndConditions(IFMeshLine lusasLineMesh, BarRelease barReleases)
        {
            if (barReleases.StartRelease.TranslationX == DOFType.Free)
                lusasLineMesh.setEndRelease("Start", "u", "free");
            if (barReleases.StartRelease.TranslationY == DOFType.Free)
                lusasLineMesh.setEndRelease("Start", "v", "free");
            if (barReleases.StartRelease.TranslationY == DOFType.Free)
                lusasLineMesh.setEndRelease("Start", "w", "free");
            if (barReleases.StartRelease.RotationX == DOFType.Free)
                lusasLineMesh.setEndRelease("Start", "thx", "free");
            if (barReleases.StartRelease.RotationY == DOFType.Free)
                lusasLineMesh.setEndRelease("Start", "thy", "free");
            if (barReleases.StartRelease.RotationZ == DOFType.Free)
                lusasLineMesh.setEndRelease("Start", "thz", "free");

            if (barReleases.EndRelease.TranslationX == DOFType.Free)
                lusasLineMesh.setEndRelease("End", "u", "free");
            if (barReleases.EndRelease.TranslationY == DOFType.Free)
                lusasLineMesh.setEndRelease("End", "v", "free");
            if (barReleases.EndRelease.TranslationY == DOFType.Free)
                lusasLineMesh.setEndRelease("End", "w", "free");
            if (barReleases.EndRelease.RotationX == DOFType.Free)
                lusasLineMesh.setEndRelease("End", "thx", "free");
            if (barReleases.EndRelease.RotationY == DOFType.Free)
                lusasLineMesh.setEndRelease("End", "thy", "free");
            if (barReleases.EndRelease.RotationZ == DOFType.Free)
                lusasLineMesh.setEndRelease("End", "thz", "free");
        }

        /***************************************************/

    }
}



