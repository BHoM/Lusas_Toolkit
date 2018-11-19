using Lusas.LPI;
using BH.oM.Adapters.Lusas;

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
                lusasSurfaceMesh.addElementName("QTS4");
            }
            return lusasSurfaceMesh;
        }
    }
}