using System.Collections.Generic;
using BH.oM.Base;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        private List<IBHoMObject> ReadAll(List<string> ids = null)
        {
            List<IBHoMObject> objects = new List<IBHoMObject>();

            objects.AddRange(ReadNodes());
            objects.AddRange(ReadBars());
            objects.AddRange(ReadPlanarPanels());
            objects.AddRange(Read2DProperties());
            objects.AddRange(ReadMaterials());
            objects.AddRange(Read4DOFConstraints());
            objects.AddRange(Read6DOFConstraints());
            objects.AddRange(ReadLoadcases());
            objects.AddRange(ReadLoadCombinations());
            objects.AddRange(ReadPointForces());
            objects.AddRange(ReadPointDisplacements());
            objects.AddRange(ReadBarUniformlyDistributedLoads());
            objects.AddRange(ReadBarPointLoads());
            objects.AddRange(ReadBarVaryingDistributedLoads());
            objects.AddRange(ReadAreaUniformlyDistributedLoads());
            objects.AddRange(ReadBarTemperatureLoads());
            objects.AddRange(ReadAreaTemperatureLoads());
            objects.AddRange(ReadGravityLoads());
            return objects;
        }
    }
}