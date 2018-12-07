using Lusas.LPI;
using BH.oM.Structure.Elements;
using BH.oM.Adapters.Lusas;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        public void SetSplitMethod(IFMeshLine lusasLineMesh, MeshSettings1D meshSettings1D, BarFEAType barFEAType)
        {
            if (meshSettings1D.SplitMethod == Split1D.Length)
            {
                if (barFEAType == BarFEAType.Axial)
                    lusasLineMesh.setSize("BRS2", meshSettings1D.SplitParameter);
                else if (barFEAType == BarFEAType.Flexural)
                    lusasLineMesh.setSize("BMX21", meshSettings1D.SplitParameter);
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
    }
}
