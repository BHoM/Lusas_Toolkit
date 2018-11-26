using System.Collections.Generic;
using System.Linq;
using Lusas.LPI;
using BH.oM.Adapters.Lusas;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        private List<MeshSettings1D> Read1DMeshSettings(List<string> ids = null)
        {
            List<MeshSettings1D> bhomMeshSettings1Ds = new List<MeshSettings1D>();

            object[] lusasMesh1Ds = d_LusasData.getAttributes("Line Mesh");

            for (int i = 0; i < lusasMesh1Ds.Count(); i++)
            {
                IFMeshLine lusasMesh1D = (IFMeshLine)lusasMesh1Ds[i];
                MeshSettings1D bhomMeshSettings1D = Engine.Lusas.Convert.ToBHoMMeshSettings1D(lusasMesh1D);
                List<string> analysisName = new List<string> { lusasMesh1D.getAttributeType() };
                bhomMeshSettings1D.Tags = new HashSet<string>(analysisName);
                bhomMeshSettings1Ds.Add(bhomMeshSettings1D);
            }

            return bhomMeshSettings1Ds;
        }
    }
}