using System.Collections.Generic;
using System.Linq;
using BH.oM.Common.Materials;
using Lusas.LPI;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        private List<Material> ReadMaterials(List<string> ids = null)
        {
            object[] lusasMaterials = d_LusasData.getAttributes("Material");
            List<Material> bhomMaterials = new List<Material>();

            for (int i = 0; i < lusasMaterials.Count(); i++)
            {
                IFAttribute lusasMaterial = (IFAttribute)lusasMaterials[i];
                Material bhomMaterial = Engine.Lusas.Convert.ToBHoMMaterial(lusasMaterial);
                bhomMaterials.Add(bhomMaterial);
            }

            return bhomMaterials;
        }
    }
}