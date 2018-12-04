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
            //object[] elnames = lusasMeshLine.getElementNames();
            //ElementType1D elementType1D = ElementType1D.Bar;
            //foreach (object name in elnames)
            //{
            //    if (name.ToString() == "BRS2")
            //        continue;
            //    else
            //        elementType1D = ElementType1D.Beam;
            //}

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

            //object[] startReleases = lusasMeshLine.getValue("start");
            //object[] endReleases = lusasMeshLine.getValue("end");

            //List<int> bhomStartReleases = new List<int>();
            //List<int> bhomEndReleases = new List<int>();

            //for (int i = 1; i<7; i++)
            //{
            //    bool sr = (bool)startReleases[i];
            //    bool er = (bool)endReleases[i];

            //    if (sr)
            //        bhomStartReleases.Add(1);
            //    else
            //        bhomStartReleases.Add(0);

            //    if (er)
            //        bhomEndReleases.Add(1);
            //    else
            //        bhomEndReleases.Add(0);
            //}

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
