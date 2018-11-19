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

            object[] startReleases = lusasMeshLine.getValue("start");
            object[] endReleases = lusasMeshLine.getValue("end");

            List<int> bhomStartReleases = new List<int>();
            List<int> bhomEndReleases = new List<int>();

            for (int i = 1; i<7; i++)
            {
                bool sr = (bool)startReleases[i];
                bool er = (bool)endReleases[i];

                if (sr)
                    bhomStartReleases.Add(1);
                else
                    bhomStartReleases.Add(0);

                if (er)
                    bhomEndReleases.Add(1);
                else
                    bhomEndReleases.Add(0);
            }

            MeshSettings1D bhomMeshSettings1D = new MeshSettings1D
            {
                Name = attributeName,
                ElementType1D = elementType1D,
                SplitMethod = splitMethod,
                SplitParameter = value,
                StartReleases = bhomStartReleases,
                EndReleases = bhomEndReleases
            };

            int bhomID = GetBHoMID(lusasMeshLine, 'e');
            bhomMeshSettings1D.CustomData["Lusas_id"] = bhomID;
            return bhomMeshSettings1D;
        }

    }
}
