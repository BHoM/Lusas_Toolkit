using System.Collections.Generic;
using System.Linq;
using BH.oM.Structure.Elements;
using BH.oM.Structure.Properties.Surface;
using BH.oM.Structure.Properties.Constraint;
using BH.oM.Geometry;
using BH.oM.Common.Materials;
using Lusas.LPI;

namespace BH.Engine.Lusas
{
    public static partial class Convert
    {
        public static PanelPlanar ToBHoMPanelPlanar(this IFSurface lusasSurface,
            Dictionary<string, Edge> bhomEdges,
            HashSet<string> groupNames,
            Dictionary<string, ISurfaceProperty> bhom2DProperties,
            Dictionary<string, Material> bhomMaterials,
            Dictionary<string, Constraint4DOF> bhomSupports)

        {
            object[] lusasSurfaceLines = lusasSurface.getLOFs();
            List<ICurve> dummyCurve = new List<ICurve>();

            int n = lusasSurfaceLines.Length;
            HashSet<string> tags = new HashSet<string>(IsMemberOf(lusasSurface, groupNames));

            List<Edge> surfaceEdges = new List<Edge>();

            for (int i = 0; i < n; i++)
            {
                Edge bhomEdge = GetEdge(lusasSurface, i, bhomEdges);
                surfaceEdges.Add(bhomEdge);
            }

            PanelPlanar bhomPanel = Structure.Create.PanelPlanar(surfaceEdges, dummyCurve);

            bhomPanel.Tags = tags;
            bhomPanel.CustomData["Lusas_id"] = lusasSurface.getName();

            List<string> geometricAssignments = AttributeAssignments(lusasSurface, "Geometric");
            List<string> materialAssignments = AttributeAssignments(lusasSurface, "Material");

            Material panelMaterial = null;
            ISurfaceProperty bhomProperty2D = null;

            if (!(geometricAssignments.Count() == 0))
            {
                bhom2DProperties.TryGetValue(geometricAssignments[0], out bhomProperty2D);
                if (!(materialAssignments.Count() == 0))
                {
                    bhomMaterials.TryGetValue(materialAssignments[0], out panelMaterial);
                    bhomProperty2D.Material = panelMaterial;
                }

                bhomPanel.Property = bhomProperty2D;
            }

            return bhomPanel;
        }
    }
}