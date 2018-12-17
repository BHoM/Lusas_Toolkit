using Lusas.LPI;
using BH.oM.Adapters.Lusas;
using BH.oM.Structure.Elements;
using BH.oM.Structure.Properties.Constraint;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        public IFMeshLine CreateMeshSettings1D(MeshSettings1D meshSettings1D, BarFEAType barFEAType = BarFEAType.Flexural, BarRelease barRelease = null)
        {
            if (barRelease != null && barFEAType == BarFEAType.Axial)
            {
                Engine.Reflection.Compute.RecordWarning(
                    barFEAType + " used with barReleases, this information will be lost when pushed to Lusas");
            }
            else if(barRelease == null)
            {
                barRelease = Engine.Structure.Create.BarReleaseFixFix();
            }


            int adapterID;
            if (meshSettings1D.CustomData.ContainsKey(AdapterId))
                adapterID = System.Convert.ToInt32(meshSettings1D.CustomData[AdapterId]);
            else
                adapterID = System.Convert.ToInt32(NextId(meshSettings1D.GetType()));

            string releaseString = CreateReleaseString(barRelease);

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
                SetEndConditions(lusasLineMesh, barRelease);
            }
            return lusasLineMesh;
        }
    }
}