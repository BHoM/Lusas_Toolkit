using System.Collections.Generic;
using System.Linq;
using BH.oM.Structure.Properties.Constraint;
using BH.Engine.Structure;
using Lusas.LPI;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        public IFAttribute CreateSupport(Constraint6DOF constraint)
        {
            if (!CheckIllegalCharacters(constraint.Name))
            {
                return null;
            }

            IFAttribute lusasSupport = null;
            string lusasName = "Sp" + constraint.CustomData[AdapterId] + "/" + constraint.Name;

            if (d_LusasData.existsAttribute("Support", lusasName))
            {
                lusasSupport = d_LusasData.getAttribute("Support", lusasName);
            }
            else
            {
                lusasSupport = d_LusasData.createSupportStructural(lusasName);

                List<string> releaseNames = new List<string> { "U", "V", "W", "THX", "THY", "THZ" };
                List<double> stiffness = new List<double> {
                    constraint.TranslationalStiffnessX, constraint.TranslationalStiffnessY,
                    constraint.TranslationalStiffnessZ,
                    constraint.RotationalStiffnessX,constraint.RotationalStiffnessY,
                    constraint.RotationalStiffnessZ};

                bool [] fixities = constraint.Fixities();

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
            if (!CheckIllegalCharacters(constraint.Name))
            {
                return null;
            }

            IFAttribute lusasSupport = null;
            string lusasName = "Sp" + constraint.CustomData[AdapterId] + "/" + constraint.Name;

            if (d_LusasData.existsAttribute("Support", lusasName))
            {
                lusasSupport = d_LusasData.getAttribute("Support", lusasName);
            }
            else
            {
                lusasSupport = d_LusasData.createSupportStructural(lusasName);

                List<string> releaseNames = new List<string> { "U", "V", "W", "THX"};
                List<double> stiffness = new List<double> {
                    constraint.TranslationalStiffnessX,
                    constraint.TranslationalStiffnessY,
                    constraint.TranslationalStiffnessZ,
                    constraint.RotationalStiffnessX
                };
                List<DOFType> fixities = new List<DOFType> {
                    constraint.TranslationX,
                    constraint.TranslationY,
                    constraint.TranslationZ,
                    constraint.RotationX
                };

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

