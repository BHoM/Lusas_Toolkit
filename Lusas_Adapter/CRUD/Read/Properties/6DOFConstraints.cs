using System.Collections.Generic;
using System.Linq;
using BH.oM.Structure.Properties.Constraint;
using Lusas.LPI;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        private List<Constraint6DOF> Read6DOFConstraints(List<string> ids = null)
        {
            object[] lusasSupports = d_LusasData.getAttributes("Support");
            List<Constraint6DOF> bhomConstraints6DOF = new List<Constraint6DOF>();

            for (int i = 0; i < lusasSupports.Count(); i++)
            {
                IFSupportStructural lusasSupport = (IFSupportStructural)lusasSupports[i];
                Constraint6DOF bhomConstraint6DOF = Engine.Lusas.Convert.ToBHoMConstraint6DOF(lusasSupport);
                bhomConstraints6DOF.Add(bhomConstraint6DOF);
            }

            return bhomConstraints6DOF;
        }
    }
}