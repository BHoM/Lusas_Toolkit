using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BH.oM.Base;
using BH.oM.Geometry;
using BH.oM.Structure.Elements;
using BH.oM.Structure.Properties;
using BH.oM.Structure.Loads;
using BH.oM.Common.Materials;
using Lusas.LPI;
using BH.oM.Adapters.Lusas;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        private List<PanelPlanar> ReadPlanarPanels(List<string> ids = null)
        {
            object[] lusasSurfaces = d_LusasData.getObjects("Surface");
            List<PanelPlanar> bhomSurfaces = new List<PanelPlanar>();

            IEnumerable<Edge> bhomEdgesList = ReadEdges();
            Dictionary<string, Edge> bhomEdges = bhomEdgesList.ToDictionary(
                x => x.CustomData[AdapterId].ToString());

            HashSet<string> groupNames = ReadTags();
            IEnumerable<Material> materialList = ReadMaterials();
            Dictionary<string, Material> materials = materialList.ToDictionary(
                x => x.Name.ToString());

            IEnumerable<IProperty2D> geometricList = Read2DProperties();
            Dictionary<string, IProperty2D> geometrics = geometricList.ToDictionary(
                x => x.Name.ToString());

            IEnumerable<Constraint4DOF> bhomSupportList = Read4DOFConstraints();
            Dictionary<string, Constraint4DOF> bhomSupports = bhomSupportList.ToDictionary(
                x => x.Name);

            for (int i = 0; i < lusasSurfaces.Count(); i++)
            {
                IFSurface lusasSurface = (IFSurface)lusasSurfaces[i];
                PanelPlanar bhomPanel = Engine.Lusas.Convert.ToBHoMPanelPlanar(lusasSurface,
                    bhomEdges,
                    groupNames,
                    geometrics,
                    materials,
                    bhomSupports);

                bhomSurfaces.Add(bhomPanel);
            }

            return bhomSurfaces;
        }
    }
}