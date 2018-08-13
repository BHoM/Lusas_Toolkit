using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.oM.Base;
using BH.oM.Geometry;
using BH.oM.Structural.Elements;
using BH.oM.Structural.Properties;
using BH.oM.Common.Materials;
using LusasM15_2;
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


        public static PanelPlanar ToBHoMObject(this IFSurface lusasSurf, Dictionary<string, Bar> bhomBars, 
            Dictionary<string, Node> bhomNodes, HashSet<String> groupNames)
        {
            Polyline bhomPolyline = new Polyline();

            Object[] surfLines = lusasSurf.getLOFs();

            int n = surfLines.Length;
            HashSet<String> tags = new HashSet<string>(isMemberOf(lusasSurf, groupNames));

            List<Point> bhomPoints = new List<Point>();
            for (int i = 0; i < n - 1; i++)
            {
                Bar bhomBar = getBar(lusasSurf, i, bhomBars);

                Point bhomPointEnd = bhomBar.EndNode.Position;

                bhomPoints.Add(bhomPointEnd);

                if (i == n - 2)
                {
                    Bar bhomFirstBar = getBar(lusasSurf, 0, bhomBars);

                    bhomPoints.Add(bhomFirstBar.StartNode.Position);

                    bhomPoints.Add(bhomFirstBar.EndNode.Position);
                }
            }

            Polyline bhomPLine = new Polyline { ControlPoints = bhomPoints };
            ICurve bhomICurve = bhomPLine;
            List<ICurve> bhomICurves = new List<ICurve>();
            PanelPlanar bhomPanel = BH.Engine.Structure.Create.PanelPlanar(bhomICurve, bhomICurves);
            bhomPanel.Tags = tags;
            bhomPanel.CustomData["Lusas_id"] = lusasSurf.getName();

            List<String> supportAssignments = attributeAssignments(lusasSurf, "Support");

            //Constraint6DOF panelPlanarConstraint = null;
            //if (!(supportAssignments.Count() == 0))
            //{
            //    constraints6DOF.TryGetValue(supportAssignments[0], out panelPlanarConstraint);
            //}

            //bhomPanel.Constraint = panelPlanarConstraint;

            return bhomPanel;
        }
    }
}