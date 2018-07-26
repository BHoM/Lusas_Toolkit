using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.oM.Structural.Elements;
using BH.oM.Geometry;
using BH.Engine.Geometry;

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


        public static PanelPlanar ToBHoMObject(this IFSurface lusasSurf, Dictionary<string, Bar> bhomBars, Dictionary<string, Node> bhomNodes)
        {
            Polyline bhomPolyline = new Polyline();

            Object[] surfLines = lusasSurf.getLOFs();

            int n = surfLines.Length;

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
            bhomPanel.CustomData["Lusas_id"] = lusasSurf.getName();

            //Read tags from objectsets

            return bhomPanel;
        }

        public static Bar ToBHoMObject(this IFLine lusasLine, Dictionary<string, Node> bhomNodes)
        {

            Node startNode = getNode(lusasLine, 0, bhomNodes);

            Node endNode = getNode(lusasLine, 1, bhomNodes);

            Bar bhomBar = new Bar { StartNode = startNode, EndNode = endNode };

            String lineName = removePrefix(lusasLine.getName(), "L-");

            bhomBar.CustomData["Lusas_id"] = lineName;

            //Read tags from objectsets

            return bhomBar;
        }

        //public static Edge ToBHoMObject(this IFLine lusasLine, Dictionary<string, Node> bhomNodes)
        //{

        //    Node startNode = getNode(lusasLine, 0, bhomNodes);

        //    Point startPoint = startNode.Position;

        //    Node endNode = getNode(lusasLine, 1, bhomNodes);

        //    Point endPoint = endNode.Position;

        //    String lineName = removePrefix(lusasLine.getName(), "L-");

        //    bhomEdge.CustomData["Lusas_id"] = lineName;

        //    Read tags from objectsets

        //    return bhomEdge;
        //}

        public static Node ToBHoMObject(this IFPoint lusasPoint)
        {
            Node bhomNode = new Node { Position = { X = lusasPoint.getX(), Y = lusasPoint.getY(), Z = lusasPoint.getZ() } };

            String pointName = removePrefix(lusasPoint.getName(), "P-");

            bhomNode.CustomData["Lusas_id"] = pointName;

            //Read tags from objectsets

            return bhomNode;
        }
        /***************************************************/
        /**** Additional Methods                        ****/
        /***************************************************/

        public static string removePrefix(string geometryName, string forRemoval)
        {
            if (geometryName.Contains(forRemoval))
            {
                geometryName.Replace(forRemoval, "");
            }
            return geometryName;
        }

        public static Node getNode(IFLine lusasLine, int nodeIndex, Dictionary<string, Node> bhomNodes)
        {
            Node bhomNode = null;
            IFPoint lusasPoint = lusasLine.getLOFs()[nodeIndex];
            String pointName = removePrefix(lusasPoint.getName(), "P-");
            bhomNodes.TryGetValue(pointName, out bhomNode);

            return bhomNode;
        }

        public static Bar getBar(IFSurface lusasSurf, int lineIndex, Dictionary<string, Bar> bhomBars)
        {
            Bar bhomBar = null;
            IFLine lusasEdge = lusasSurf.getLOFs()[lineIndex];
            String lineName = removePrefix(lusasEdge.getName(), "P-");
            bhomBars.TryGetValue(lineName, out bhomBar);

            return bhomBar;
        }
        //

        /***************************************************/
    }
}
