using Lusas.LPI;
using BH.oM.Structure.Elements;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        public void SetElementType(IFMeshLine lusasLineMesh, BarFEAType barFEAType)
        {
            if (barFEAType == BarFEAType.Axial)
                lusasLineMesh.addElementName("BRS2");
            else if(barFEAType == BarFEAType.Flexural)
                lusasLineMesh.addElementName("BMX21");
        }
    }
}