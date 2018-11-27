using System.Collections.Generic;
using BH.oM.Structure.Elements;
using Lusas.LPI;
using BH.oM.Base;
using BH.oM.Structure.Loads;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        public IFPoint[] GetAssignedPoints(Load<Node> bhomLoads)
        {
            List<IFPoint> assignedGeometry = new List<IFPoint>();
            foreach (BHoMObject bhomObject in bhomLoads.Objects.Elements)
            {
                IFPoint lusasPoint = d_LusasData.getPointByName(
                    "P" + bhomObject.CustomData[AdapterId].ToString());

                assignedGeometry.Add(lusasPoint);
            }

            IFPoint[] arrayGeometry = assignedGeometry.ToArray();

            return arrayGeometry;
        }

        public IFLine[] GetAssignedLines(Load<Bar> bhomLoads)
        {
            List<IFLine> assignedGeometry = new List<IFLine>();
            foreach (BHoMObject bhomObject in bhomLoads.Objects.Elements)
            {
                IFLine lusasLine = d_LusasData.getLineByName(
                    "L" + bhomObject.CustomData[AdapterId].ToString());

                assignedGeometry.Add(lusasLine);
            }

            IFLine[] arrayGeometry = assignedGeometry.ToArray();

            return arrayGeometry;
        }

        public IFSurface[] GetAssignedSurfaces(Load<IAreaElement> bhomLoads)
        {
            List<IFSurface> assignedGeometry = new List<IFSurface>();
            foreach (BHoMObject bhomObject in bhomLoads.Objects.Elements)
            {
                IFSurface lusasSurface = d_LusasData.getSurfaceByName(
                    "S" + bhomObject.CustomData[AdapterId].ToString());

                assignedGeometry.Add(lusasSurface);
            }

            IFSurface[] arrayGeometry = assignedGeometry.ToArray();

            return arrayGeometry;
        }

        public IFGeometry[] GetAssignedObjects(Load<BHoMObject> bhomLoads)
        {
            List<IFGeometry> assignedGeometry = new List<IFGeometry>();

            foreach (BHoMObject bhomObject in bhomLoads.Objects.Elements)
            {
                if (bhomObject is Node)
                {
                    IFGeometry lusasPoint = d_LusasData.getPointByName(
                        "P" + bhomObject.CustomData[AdapterId].ToString());

                    assignedGeometry.Add(lusasPoint);
                }
                else if (bhomObject is Bar)
                {
                    IFGeometry lusasBar = d_LusasData.getLineByName(
                        "L" + bhomObject.CustomData[AdapterId].ToString());

                    assignedGeometry.Add(lusasBar);
                }
                else if (bhomObject is PanelPlanar)
                {
                    IFGeometry lusasSurface = d_LusasData.getSurfaceByName(
                        "S" + bhomObject.CustomData[AdapterId].ToString());

                    assignedGeometry.Add(lusasSurface);
                }
            }

            IFGeometry[] arrayGeometry = assignedGeometry.ToArray();

            return arrayGeometry;
        }
    }
}
