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
            HashSet<string> groupNames,
            Dictionary<string, IProperty2D> bhomProperties2D,
            Dictionary<string, Material> bhomMaterials,
            Dictionary<string, Constraint4DOF> bhomSupports)

        {
            Object[] surfLines = lusasSurf.getLOFs();
            List<ICurve> dummyCurve = new List<ICurve>();

            int n = surfLines.Length;
            HashSet<string> tags = new HashSet<string>(IsMemberOf(lusasSurf, groupNames));

            List<Edge> surfEdges = new List<Edge>();

            for (int i = 0; i < n; i++)
            {
                Edge bhomEdge = GetEdge(lusasSurf, i, bhomEdges);
                surfEdges.Add(bhomEdge);
            }



            PanelPlanar bhomPanel = BH.Engine.Structure.Create.PanelPlanar(surfEdges,dummyCurve);


            bhomPanel.Tags = tags;
            bhomPanel.CustomData["Lusas_id"] = lusasSurf.getName();

            List<string> geometricAssignments = AttributeAssignments(lusasSurf, "Geometric");
            List<string> materialAssignments = AttributeAssignments(lusasSurf, "Material");

            Material panelMaterial = null;
            IProperty2D bhomProperty2D = null;

            if (!(geometricAssignments.Count() == 0))
            {
                bhomProperties2D.TryGetValue(geometricAssignments[0], out bhomProperty2D);
                if (!(materialAssignments.Count() == 0))
                {
                    bhomMaterials.TryGetValue(materialAssignments[0], out panelMaterial);
                    bhomProperty2D.Material = panelMaterial;
                }

                bhomPanel.Property = bhomProperty2D;
            }

            //List<string> supportAssignments = AttributeAssignments(lusasSurf, "Support");

            //Constraint4DOF barConstraint = null;
            //if (!(supportAssignments.Count() == 0))
            //{
            //    bhomSupports.TryGetValue(supportAssignments[0], out barConstraint);
                
            //}


            return bhomPanel;
        }
    }
}