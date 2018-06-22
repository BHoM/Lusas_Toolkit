using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.oM.Structural.Elements;
using BH.oM.Geometry;
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
            //method unfinished
            PanelPlanar bhomPanel = new PanelPlanar();
            Polyline bhomPolyline = new Polyline();


            Object[] surfLines = lusasSurf.getLOFs();

            int n = surfLines.Length;

            Bar bhomBar = null;

            List<Point> bhomPoints = new List<Point>();

            for(int i = 0 ;i < n-1; i++)
            {
                IFLine edge = lusasSurf.getLOFs()[i];

                bhomBars.TryGetValue(edge.getID().ToString(), out bhomBar);

                Point bhomPointEnd = new Point
                {
                    X = bhomBar.EndNode.Position.X,
                    Y = bhomBar.EndNode.Position.Y,
                    Z = bhomBar.EndNode.Position.Z
                };

                bhomPoints.Add(bhomPointEnd);

                if(i == n-2)
                {
                    bhomBars.TryGetValue(lusasSurf.getLOFs()[0].getID().ToString(), out bhomBar);

                    Point bhomPointStart = new Point
                    {
                        X = bhomBar.StartNode.Position.X,
                        Y = bhomBar.StartNode.Position.Y,
                        Z = bhomBar.StartNode.Position.Z
                    };

                    bhomPoints.Add(bhomPointStart);
                }
            }

            Polyline bhomPLine = new Polyline {ControlPoints = bhomPoints};

            List<ICurve> bhomICurves = new List<ICurve>();

            bhomPanel = BH.Engine.Structure.Create.PanelPlanar(bhomPLine, null,null,lusasSurf.getName());
            //ambigious definition, unsure how to fix

            return bhomPanel;
        }

        public static Bar ToBHoMObject(this IFLine lusasLine, Dictionary<string, Node> bhomNodes)
        {
            Node startNode = null;
            IFPoint startPoint = lusasLine.getLOFs()[0];
            bhomNodes.TryGetValue(startPoint.getID().ToString(), out startNode);

            Node endNode = null;
            IFPoint endPoint = lusasLine.getLOFs()[1];
            bhomNodes.TryGetValue(endPoint.getID().ToString(), out endNode);

            Bar bhomBar = new Bar { StartNode = startNode, EndNode = endNode, Name = lusasLine.getName() };

            bhomBar.CustomData[AdapterId] = lusasLine.getID();

            return bhomBar;
        }

        public static Node ToBHoMObject(this IFPoint lusasPoint)
        {
            Node newNode = new Node { Position = { X = lusasPoint.getX(), Y = lusasPoint.getY(), Z = lusasPoint.getZ() }, Name = lusasPoint.getName() };

            newNode.CustomData[AdapterId] = lusasPoint.getID();

            return newNode;
        }

        //

        /***************************************************/
    }
}
