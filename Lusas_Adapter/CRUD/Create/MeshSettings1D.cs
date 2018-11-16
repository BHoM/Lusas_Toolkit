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

                if (meshSettings1D.SplitMethod==Split.Divisions)
                {
                    int ndivisions = (int) meshSettings1D.SplitParameter;
                    int[] ratios = new int[ndivisions]; 
                    for (int i=0; i<ndivisions; i++)
                    {
                        ratios[i] = 1;
                    }
                    lusasLineMesh.setValue("ratio", ratios);
                    if (meshSettings1D.ElementType1D == ElementType1D.Bar)
                        lusasLineMesh.addElementName("BRS2");
                    else
                        lusasLineMesh.addElementName("BMX21");
                }
                else
                {
                    lusasLineMesh.setValue("size", meshSettings1D.SplitParameter);
                    if (meshSettings1D.ElementType1D == ElementType1D.Bar)
                        lusasLineMesh.addElementName("BRS2");
                    else
                        lusasLineMesh.addElementName("BMX21");
                }
            }
            return lusasLineMesh;
        }
    }
}