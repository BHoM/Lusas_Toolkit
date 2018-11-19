using System.Collections.Generic;
using System.Linq;
using System;
using BH.oM.Base;
using BH.oM.Structure.Elements;
using BH.oM.Geometry;
using BH.oM.Structure.Properties;
using BH.oM.Structure.Loads;
using BH.oM.Common.Materials;
using BH.Engine.Geometry;
using BH.Engine.Structure;
using Lusas.LPI;
using BH.oM.Adapter.Lusas;
using BH.Engine.Reflection;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        public IFMeshSurface CreateMeshSettings2D(MeshSettings2D meshSettings2D)
        {
            IFMeshSurface lusasSurfaceMesh = null;
            string lusasAttributeName = "Me" + meshSettings2D.CustomData[AdapterId] + "/" + meshSettings2D.Name;
            if (d_LusasData.existsAttribute("Mesh", lusasAttributeName))
            {
                lusasSurfaceMesh = (IFMeshSurface)d_LusasData.getAttribute("Mesh", lusasAttributeName);
            }
            else
            {
                lusasSurfaceMesh = d_LusasData.createMeshSurface(lusasAttributeName);

                if (meshSettings2D.SplitMethod == Split2D.Divisions)
                {
                    lusasSurfaceMesh.setValue("xDivisions", meshSettings2D.xDivisions);
                    lusasSurfaceMesh.setValue("yDivisions", meshSettings2D.yDivisions);
                }
                else if (meshSettings2D.SplitMethod == Split2D.Size)
                {
                    lusasSurfaceMesh.setValue("size", meshSettings2D.ElementSize);
                }

                if (meshSettings2D.ElementType2D == ElementType2D.ThickShell)
                    lusasSurfaceMesh.addElementName("QTS4");
                else
                    lusasSurfaceMesh.addElementName("QSI4");
            }
            return lusasSurfaceMesh;
        }
    }
}