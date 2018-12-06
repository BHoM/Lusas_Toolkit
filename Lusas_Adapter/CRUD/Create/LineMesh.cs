using Lusas.LPI;
using BH.oM.Adapters.Lusas;
using BH.oM.Structure.Elements;
using BH.oM.Structure.Properties;
using System.Linq;
using System.Collections.Generic;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        public IFMeshLine CreateMeshSettings1D(MeshSettings1D meshSettings1D, BarFEAType barType = BarFEAType.Flexural, BarRelease barReleases = null)
        {
            int adapterID;
            if (meshSettings1D.CustomData.ContainsKey(AdapterId))
                adapterID = System.Convert.ToInt32(meshSettings1D.CustomData[AdapterId]);
            else
                adapterID = System.Convert.ToInt32(NextId(meshSettings1D.GetType()));

            string releaseString = null;

            if (barReleases.StartRelease.TranslationX == DOFType.Free)
                releaseString = releaseString + "FX";
            if (barReleases.StartRelease.TranslationY == DOFType.Free)
                releaseString = releaseString + "FY";
            if (barReleases.StartRelease.TranslationZ == DOFType.Free)
                releaseString = releaseString + "FZ";
            if (barReleases.StartRelease.RotationX == DOFType.Free)
                releaseString = releaseString + "MX";
            if (barReleases.StartRelease.RotationY == DOFType.Free)
                releaseString = releaseString + "MY";
            if (barReleases.StartRelease.RotationZ == DOFType.Free)
                releaseString = releaseString + "MZ";

            if(releaseString!=null)
                releaseString = releaseString + ",";

            if (barReleases.EndRelease.TranslationX == DOFType.Free)
                releaseString = releaseString + "FX";
            if (barReleases.EndRelease.TranslationY == DOFType.Free)
                releaseString = releaseString + "FY";
            if (barReleases.EndRelease.TranslationZ == DOFType.Free)
                releaseString = releaseString + "FZ";
            if (barReleases.EndRelease.RotationX == DOFType.Free)
                releaseString = releaseString + "MX";
            if (barReleases.EndRelease.RotationY == DOFType.Free)
                releaseString = releaseString + "MY";
            if (barReleases.EndRelease.RotationZ == DOFType.Free)
                releaseString = releaseString + "MZ";

            IFMeshLine lusasLineMesh = null;
            string lusasName = "Me" + adapterID + "/" + meshSettings1D.Name + "/" + barType.ToString() + "/" + releaseString;
            if (d_LusasData.existsAttribute("Mesh", lusasName))
            {
                lusasLineMesh = (IFMeshLine)d_LusasData.getAttribute("Mesh", lusasName);
            }
            else
            {
                lusasLineMesh = d_LusasData.createMeshLine(lusasName);

                if (meshSettings1D.SplitMethod == Split1D.Automatic)
                {
                    lusasLineMesh.setValue("uiSpacing", "uniform");
                }
                else if (meshSettings1D.SplitMethod == Split1D.Divisions)
                {
                    lusasLineMesh.addSpacing(System.Convert.ToInt32(meshSettings1D.SplitParameter), 1);
                }
                else if (meshSettings1D.SplitMethod == Split1D.Length)
                {
                    lusasLineMesh.setSize("NULL", meshSettings1D.SplitParameter);
                }


                if (barType == BarFEAType.Axial)
                    lusasLineMesh.addElementName("BRS2");
                else
                    lusasLineMesh.addElementName("BMX21");

                //if (barReleases.EndRelease.TranslationX. StartReleases.Concat(meshSettings1D.EndReleases).Contains(1) && meshSettings1D.ElementType1D == ElementType1D.Bar)
                //    BH.Engine.Reflection.Compute.RecordWarning("End Releases only supported with Beam elements in Lusas");

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
            return lusasLineMesh;
        }
    }
}