using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.oM.Base;
using BH.oM.Geometry;
using BH.oM.Structure.Elements;
using BH.oM.Structure.Properties;
using BH.oM.Common.Materials;
using Lusas.LPI;
using BH.oM.Adapter.Lusas;

namespace BH.Engine.Lusas
{
    public static partial class Convert
    {
        public static MeshSettings1D ToBHoMMeshSettings1D(this IFMeshLine lusasMeshLine)
        {
            string attributeName = lusasMeshLine.getName();
            object[] ratios = lusasMeshLine.getValue("ratio");
            int ndivisions = ratios.Count();
            if (ndivisions == 0)
                ndivisions = 4;

            object[] elnames = lusasMeshLine.getElementNames();
            ElementType1D elementType1D = ElementType1D.Bar;
            foreach (object name in elnames)
            {
                if (name.ToString() == "BRS2")
                    continue;
                else
                    elementType1D = ElementType1D.Beam;
            }

            double value;
            Split splitMethod = Split.Divisions;
            if (lusasMeshLine.getValue("size")==0)
            {
                splitMethod = Split.Divisions;
                value = ndivisions;
            }
            else
            {
                splitMethod = Split.Length;
                value = lusasMeshLine.getValue("size");
            }

            MeshSettings1D bhomMeshSettings1D = new MeshSettings1D
            {
                Name = attributeName,
                ElementType1D = elementType1D,
                SplitMethod = splitMethod,
                SplitParameter = value,
            };

            int bhomID = GetBHoMID(lusasMeshLine, 'e');
            bhomMeshSettings1D.CustomData["Lusas_id"] = bhomID;
            return bhomMeshSettings1D;
        }

    }
}
