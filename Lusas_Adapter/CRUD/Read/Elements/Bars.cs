using System.Collections.Generic;
using System.Linq;
using BH.oM.Structure.Elements;
using BH.oM.Structure.Properties;
using BH.oM.Common.Materials;
using Lusas.LPI;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        private List<Bar> ReadBars(List<string> ids = null)
        {
            object[] lusasLines = d_LusasData.getObjects("Line");

            List<Bar> bhomBars = new List<Bar>();
            IEnumerable<Node> bhomNodesList = ReadNodes();
            Dictionary<string, Node> bhomNodes = bhomNodesList.ToDictionary(
                x => x.CustomData[AdapterId].ToString());

            IEnumerable<Constraint4DOF> bhomSupportList = Read4DOFConstraints();
            Dictionary<string, Constraint4DOF> bhomSupports = bhomSupportList.ToDictionary(
                x => x.Name);

            IEnumerable<Material> materialList = ReadMaterials();
            Dictionary<string, Material> materials = materialList.ToDictionary(
                x => x.Name.ToString());

            IEnumerable<ISectionProperty> geometricList = ReadSectionProperties();
            Dictionary<string, ISectionProperty> geometrics = geometricList.ToDictionary(
                x => x.Name.ToString());

            HashSet<string> groupNames = ReadTags();

            for (int i = 0; i < lusasLines.Count(); i++)
            {
                IFLine lusasLine = (IFLine)lusasLines[i];
                Bar bhomBar = Engine.Lusas.Convert.ToBHoMBar
                    (
                    lusasLine,
                    bhomNodes,
                    bhomSupports,
                    groupNames,
                    materials,
                    geometrics
                    );

                bhomBars.Add(bhomBar);
            }
            return bhomBars;
        }
    }
}