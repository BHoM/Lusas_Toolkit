using BH.oM.Structure.Properties.Constraint;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        public string CreateReleaseString(BarRelease barReleases)
        {
            string releaseString = "";

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

            if (releaseString != null)
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

            return releaseString;
        }
    }
}