using Lusas.LPI;
using BH.oM.Adapters.Lusas;
using BH.oM.Structure.Elements;
using BH.oM.Structure.Properties.Constraint;
using BH.Engine.Reflection;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        public IFMeshLine CreateMeshSettings1D(MeshSettings1D meshSettings1D, BarFEAType barFEAType = BarFEAType.Flexural, BarRelease barReleases = null)
        {
            if(barReleases!= null && barFEAType == BarFEAType.Axial)
            {
                Compute.RecordWarning(
                    "Axial element used with barReleases, this information will be lost when pushed to Lusas");
            }

            int adapterID;
            if (meshSettings1D.CustomData.ContainsKey(AdapterId))
                adapterID = System.Convert.ToInt32(meshSettings1D.CustomData[AdapterId]);
            else
                adapterID = System.Convert.ToInt32(NextId(meshSettings1D.GetType()));

            string releaseString = CreateReleaseString(barReleases);

            IFMeshLine lusasLineMesh = null;
            string lusasName = 
                "Me" + adapterID + "/" + meshSettings1D.Name + "\\" + barFEAType.ToString() + "|" + releaseString;

            if (d_LusasData.existsAttribute("Mesh", lusasName))
            {
                lusasLineMesh = (IFMeshLine)d_LusasData.getAttribute("Mesh", lusasName);
            }
            else
            {
                lusasLineMesh = d_LusasData.createMeshLine(lusasName);
                SetSplitMethod(lusasLineMesh, meshSettings1D, barFEAType);
                SetEndConditions(lusasLineMesh, barReleases);
            }
            return lusasLineMesh;
        }
    }
}