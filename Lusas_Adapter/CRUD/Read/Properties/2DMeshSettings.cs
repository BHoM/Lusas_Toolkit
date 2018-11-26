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
        private List<MeshSettings2D> Read2DMeshSettings(List<string> ids = null)
        {
            List<MeshSettings2D> bhomMeshSettings2Ds = new List<MeshSettings2D>();

            object[] lusasMesh2Ds = d_LusasData.getAttributes("Surface Mesh");

            for (int i = 0; i < lusasMesh2Ds.Count(); i++)
            {
                IFMeshSurface lusasMesh2D = (IFMeshSurface)lusasMesh2Ds[i];
                MeshSettings2D bhomMeshSettings2D = Engine.Lusas.Convert.ToBHoMMeshSettings2D(lusasMesh2D);
                List<string> analysisName = new List<string> { lusasMesh2D.getAttributeType() };
                bhomMeshSettings2D.Tags = new HashSet<string>(analysisName);
                bhomMeshSettings2Ds.Add(bhomMeshSettings2D);
            }
            return bhomMeshSettings2Ds;
        }
    }
}