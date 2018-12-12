using BH.oM.Structure.Properties.Constraint;
using Lusas.LPI;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        public void SetEndConditions(IFMeshLine lusasLineMesh, BarRelease barReleases)
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
    }
}