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

            Polyline bhomPolyline = new Polyline();


            Object[] surfLines = lusasSurf.getLOFs();

            int n = surfLines.Length;

            Bar bhomBar = null;

            List<Point> bhomPoints = new List<Point>();
            for(int i = 0 ;i < n-1; i++)
            {
                IFLine edge = lusasSurf.getLOFs()[i];

                bhomBars.TryGetValue(edge.getID().ToString(), out bhomBar);

                Point bhomPointEnd = bhomBar.EndNode.Position;

                bhomPoints.Add(bhomPointEnd);

                if(i == n-2)
                {
                    bhomBars.TryGetValue(lusasSurf.getLOFs()[0].getID().ToString(), out bhomBar);

                    Point bhomPointStart = bhomBar.StartNode.Position;

                    bhomPoints.Add(bhomPointStart);

                    Point bhomPointEnd2 = bhomBar.EndNode.Position;

                    bhomPoints.Add(bhomPointEnd2);
                }
            }

            Polyline bhomPLine = new Polyline {ControlPoints = bhomPoints};
            ICurve bhomICurve = bhomPLine;
            List<ICurve> bhomICurves = new List<ICurve>();
            //BH.oM.Structural.Properties.IProperty2D bhomIProperty2D = null;
            PanelPlanar bhomPanel = BH.Engine.Structure.Create.PanelPlanar(bhomICurve, bhomICurves);
            bhomPanel.CustomData[AdapterId] = lusasSurf.getName();

            //Read tags from objectsets

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

            //Remove L- if contained
            bhomBar.CustomData[AdapterId] = lusasLine.getName();

            //Read tags from objectsets

            return bhomBar;
        }

        public static Node ToBHoMObject(this IFPoint lusasPoint)
        {
            Node newNode = new Node { Position = { X = lusasPoint.getX(), Y = lusasPoint.getY(), Z = lusasPoint.getZ() }, Name = lusasPoint.getName() };

            //This nees to remove P- if visible
            newNode.CustomData[AdapterId] = lusasPoint.getName();

            //Read tags from objectsets

            return newNode;
        }

        //

        /***************************************************/
    }
}
