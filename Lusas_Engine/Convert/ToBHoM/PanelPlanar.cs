using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.oM.Structure.Elements;
using BH.oM.Structure.Properties;
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
            Dictionary<string, ConstantThickness> bhomGeometrics,
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

            List<String> geometricAssignments = attributeAssignments(lusasSurf, "Geometric");
            List<String> materialAssignments = attributeAssignments(lusasSurf, "Material");

            //This will be needed when Property is added
            Material panelMaterial = null;
            ConstantThickness panelGeometric = null;

            if (!(geometricAssignments.Count() == 0))
            {
                bhomGeometrics.TryGetValue(geometricAssignments[0], out panelGeometric);
                if (!(materialAssignments.Count() == 0))
                {
                    bhomMaterials.TryGetValue(materialAssignments[0], out panelMaterial);
                    panelGeometric.Material = panelMaterial;
                }

                bhomPanel.Property = panelGeometric;
            }

            return bhomPanel;
        }
    }
}