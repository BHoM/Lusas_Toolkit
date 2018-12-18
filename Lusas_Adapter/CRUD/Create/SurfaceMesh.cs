﻿using Lusas.LPI;
using BH.oM.Adapters.Lusas;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        public IFMeshSurface CreateMeshSettings2D(MeshSettings2D meshSettings2D)
        {
            if (!CheckIllegalCharacters(meshSettings2D.Name))
            {
                return null;
            }

            IFMeshSurface lusasSurfaceMesh = null;
            string lusasName = "Me" + meshSettings2D.CustomData[AdapterId] + "/" + meshSettings2D.Name;
            if (d_LusasData.existsAttribute("Mesh", lusasName))
            {
                lusasSurfaceMesh = (IFMeshSurface)d_LusasData.getAttribute("Mesh", lusasName);
            }
            else
            {
                lusasSurfaceMesh = d_LusasData.createMeshSurface(lusasName);
                if (meshSettings2D.SplitMethod == Split2D.Automatic)
                {
                    lusasSurfaceMesh.addElementName("QTS4");
                }
                else if (meshSettings2D.SplitMethod == Split2D.Divisions)
                {
                    lusasSurfaceMesh.setRegular("QTS4", meshSettings2D.xDivisions, meshSettings2D.yDivisions);
                }
                else if (meshSettings2D.SplitMethod == Split2D.Size)
                {
                    lusasSurfaceMesh.setRegularSize("QTS4", meshSettings2D.ElementSize);
                }
            }
            return lusasSurfaceMesh;
        }
    }
}