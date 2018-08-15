using System.Collections.Generic;
using System.Linq;
using System;
using BH.oM.Base;
using BH.oM.Structural.Elements;
using BH.oM.Geometry;
using BH.oM.Structural.Properties;
using BH.oM.Structural.Loads;
using BH.oM.Common.Materials;
using BH.Engine.Geometry;
using BH.Engine.Structure;
using Lusas.LPI;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        public IFAttribute CreateAttribute(Constraint6DOF constraint)
        {
            IFAttribute lusasSupport = d_LusasData.createSupportStructural(constraint.Name);

            List<string> releaseNames = new List<string> { "U", "V", "W", "THX", "THY", "THZ" };
            List<double> stiffness = new List<double> {constraint.TranslationalStiffnessX,
            constraint.TranslationalStiffnessY, constraint.TranslationalStiffnessZ,
            constraint.RotationalStiffnessX,constraint.RotationalStiffnessY,constraint.RotationalStiffnessZ};
            Boolean[] fixities = constraint.Fixities();

            for (int i = 0; i < releaseNames.Count(); i++)
            {
                if (fixities[i])
                {
                    lusasSupport.setValue(releaseNames[i], "R");
                }
                else if (stiffness[i] == 0)
                {
                    lusasSupport.setValue(releaseNames[i], "F");
                }
                else
                {
                    lusasSupport.setValue(releaseNames[i], "S");
                    lusasSupport.setValue(releaseNames[i] + "stiff", stiffness[i]);
                }
            }

            int bhomID;
            if (constraint.CustomData.ContainsKey(AdapterId))
                bhomID = System.Convert.ToInt32(constraint.CustomData[AdapterId]);
            else
                bhomID = System.Convert.ToInt32(NextId(constraint.GetType()));

            constraint.CustomData[AdapterId] = bhomID;

            lusasSupport.setName("Sp" + bhomID + "/" + constraint.Name);

            return lusasSupport;
        }
    }
}

