using System.Collections.Generic;
using System.Linq;
using BH.oM.Structure.Properties;
using Lusas.LPI;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        private List<Constraint4DOF> Read4DOFConstraints(List<string> ids = null)
        {
            object[] lusasSupports = d_LusasData.getAttributes("Support");
            List<Constraint4DOF> bhomConstraints4DOFs = new List<Constraint4DOF>();

            for (int i = 0; i < lusasSupports.Count(); i++)
            {
                IFSupportStructural lusasSupport = (IFSupportStructural)lusasSupports[i];
                Constraint4DOF bhomConstraint4DOF = Engine.Lusas.Convert.ToBHoMConstraint4DOF(lusasSupport);
                bhomConstraints4DOFs.Add(bhomConstraint4DOF);
            }

            return bhomConstraints4DOFs;
        }
    }
}