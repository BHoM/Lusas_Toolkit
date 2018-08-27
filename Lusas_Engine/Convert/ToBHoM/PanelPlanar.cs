using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.oM.Structural.Elements;
using BH.oM.Geometry;
using BH.Engine.Geometry;
using BH.oM.Common.Materials;
using Lusas.LPI;

namespace BH.Engine.Lusas
{
    public static partial class Convert
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        //Add methods for converting to BHoM from the specific software types, if possible to do without any BHoM calls
        //Example:
        //public static Node ToBHoM(this LusasNode node)
        //{

        //#region Geometry Converters

        public static PanelPlanar ToBHoMPanelPlanar(this IFSurface lusasSurf, 
            Dictionary<string, Edge> bhomEdges, 
            HashSet<String> groupNames, 
            Dictionary<string, Material> bhomMaterials)

        {
            Object[] surfLines = lusasSurf.getLOFs();
            List<ICurve> dummyCurve = new List<ICurve>();

            int n = surfLines.Length;
            HashSet<String> tags = new HashSet<string>(isMemberOf(lusasSurf, groupNames));

            List<Edge> surfEdges = new List<Edge>();
            for (int i = 0; i < n; i++)
            {
                Edge bhomEdge = getEdge(lusasSurf, i, bhomEdges);
                surfEdges.Add(bhomEdge);
            }

            PanelPlanar bhomPanel = BH.Engine.Structure.Create.PanelPlanar(surfEdges,dummyCurve);

            bhomPanel.Tags = tags;
            bhomPanel.CustomData["Lusas_id"] = lusasSurf.getName();

            List<String> materialAssignments = attributeAssignments(lusasSurf, "Material");

            Material panelMaterial = null;
            if (!(materialAssignments.Count() == 0))
            {
                bhomMaterials.TryGetValue(materialAssignments[0], out panelMaterial);
                bhomPanel.Property.Material = panelMaterial;
            }

            return bhomPanel;
        }
    }
}