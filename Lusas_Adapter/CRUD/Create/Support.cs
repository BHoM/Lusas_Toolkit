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

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        public IFAttribute CreateSupport(Constraint6DOF constraint)
        {
            IFAttribute lusasSupport = null;

            if (d_LusasData.existsAttribute("Support", "Sp" + constraint.CustomData[AdapterId] + "/" + constraint.Name))
            {
                lusasSupport = d_LusasData.getAttribute("Support", "Sp" + constraint.CustomData[AdapterId] + "/" + constraint.Name);
            }
            else
            {
                lusasSupport = d_LusasData.createSupportStructural("Sp" + constraint.CustomData[AdapterId] + "/" + constraint.Name);

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
            }

            return lusasSupport;
        }
    }
}

