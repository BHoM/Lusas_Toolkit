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
            string lusasAttributeName = "Sp" + constraint.CustomData[AdapterId] + "/" + constraint.Name;

            if (d_LusasData.existsAttribute("Support", lusasAttributeName))
            {
                lusasSupport = d_LusasData.getAttribute("Support", lusasAttributeName);
            }
            else
            {
                lusasSupport = d_LusasData.createSupportStructural(lusasAttributeName);

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
        public IFAttribute CreateSupport(Constraint4DOF constraint)
        {
            IFAttribute lusasSupport = null;
            string lusasAttributeName = "Sp" + constraint.CustomData[AdapterId] + "/" + constraint.Name;

            if (d_LusasData.existsAttribute("Support", lusasAttributeName))
            {
                lusasSupport = d_LusasData.getAttribute("Support", lusasAttributeName);
            }
            else
            {
                lusasSupport = d_LusasData.createSupportStructural(lusasAttributeName);

                List<string> releaseNames = new List<string> { "U", "V", "W", "THX"};
                List<double> stiffness = new List<double> {constraint.TranslationalStiffnessX,
                    constraint.TranslationalStiffnessY, constraint.TranslationalStiffnessZ,
                    constraint.RotationalStiffnessX};
                List<DOFType> fixities = new List<DOFType> {constraint.TranslationX,
                    constraint.TranslationY, constraint.TranslationZ, constraint.RotationX };

                for (int i = 0; i < releaseNames.Count(); i++)
                {
                    if (fixities[i] == DOFType.Fixed)
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

