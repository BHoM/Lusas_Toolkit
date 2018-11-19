using Lusas.LPI;
using BH.oM.Adapters.Lusas;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        public IFMeshLine CreateMeshSettings1D(MeshSettings1D meshSettings1D)
        {
            IFMeshLine lusasLineMesh = null;
            string lusasAttributeName = "Me" + meshSettings1D.CustomData[AdapterId] + "/" + meshSettings1D.Name;
            if (d_LusasData.existsAttribute("Mesh", lusasAttributeName))
            {
                lusasLineMesh = (IFMeshLine) d_LusasData.getAttribute("Mesh", lusasAttributeName);
            }
            else
            {
                lusasLineMesh = d_LusasData.createMeshLine(lusasAttributeName);

                if (meshSettings1D.SplitMethod == Split1D.Divisions)
                {
                    int ndivisions = (int)meshSettings1D.SplitParameter;
                    int[] ratios = new int[ndivisions];
                    for (int i = 0; i < ndivisions; i++)
                    {
                        ratios[i] = 1;
                    }
                    lusasLineMesh.setValue("ratio", ratios);
                }
                else if (meshSettings1D.SplitMethod == Split1D.Length)
                {
                    lusasLineMesh.setValue("size", meshSettings1D.SplitParameter);
                }

                //if (meshSettings1D.ElementType1D == ElementType1D.Bar)
                //    lusasLineMesh.addElementName("BRS2");
                //else
                //    lusasLineMesh.addElementName("BMX21");

                //List<string> dof = new List<string> { "u", "v", "w", "thx", "thy", "thz" };


                //if(meshSettings1D.StartReleases.Concat(meshSettings1D.EndReleases).Contains(1)&&meshSettings1D.ElementType1D==ElementType1D.Bar)
                //    BH.Engine.Reflection.Compute.RecordWarning("End Releases only supported with Beam elements in Lusas");

                //if (meshSettings1D.StartReleases.Contains(1) && meshSettings1D.ElementType1D == ElementType1D.Beam)
                //{
                //   for (int i =0; i<6; i++)
                //    {
                //        if (meshSettings1D.StartReleases[i] == 1)
                //            lusasLineMesh.setEndRelease("Start", dof[i], "free");
                //    }
                //}

                //if (meshSettings1D.EndReleases.Contains(1)&& meshSettings1D.ElementType1D == ElementType1D.Beam)
                //{
                //    if (meshSettings1D.EndReleases.Equals(meshSettings1D.StartReleases))
                //        lusasLineMesh.setEndReleasesSameAsStart(true);
                //    else
                //    {
                //        for (int i = 0; i < 6; i++)
                //        {
                //            if (meshSettings1D.EndReleases[i] == 1)
                //                lusasLineMesh.setEndRelease("End", dof[i], "free");
                //        }
                //    }
                //}

            }
            return lusasLineMesh;
        }
    }
}