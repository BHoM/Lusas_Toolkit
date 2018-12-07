using System.Linq;
using Lusas.LPI;
using BH.oM.Adapters.Lusas;

namespace BH.Engine.Lusas
{
    public static partial class Convert
    {
        public static MeshSettings1D ToBHoMMeshSettings1D(this IFAttribute lusasAttrbute)
        {
            string attributeName = GetName(lusasAttrbute);

            IFMeshLine lusasMeshLine = (IFMeshLine)lusasAttrbute;

            double value = 0;
            Split1D splitMethod = Split1D.Automatic;
            int meshType = 0;
            lusasMeshLine.getMeshDivisions(ref meshType);

            if (meshType == 0)
            {
                value = 0;
            }
            else if (meshType == 1)
            {
                splitMethod = Split1D.Divisions;
                object[] ratios = lusasMeshLine.getValue("ratio");
                value = ratios.Count();
                if (value == 0)
                    value = 4;
            }
            else if (meshType == 2)
            {
                splitMethod = Split1D.Length;
                value = lusasMeshLine.getValue("size");
            }

            MeshSettings1D bhomMeshSettings1D = new MeshSettings1D
            {
                Name = attributeName,
                SplitMethod = splitMethod,
                SplitParameter = value
            };

            int adapterID = GetAdapterID(lusasMeshLine, 'e');
            bhomMeshSettings1D.CustomData["Lusas_id"] = adapterID;

            return bhomMeshSettings1D;
        }

    }
}
