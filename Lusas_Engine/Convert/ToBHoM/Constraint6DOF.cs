using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.oM.Base;
using BH.oM.Geometry;
using BH.oM.Structure.Elements;
using BH.oM.Structure.Properties;
using BH.oM.Common.Materials;
using Lusas.LPI;

namespace BH.Engine.Lusas
{
    public static partial class Convert
    {
        public static Constraint6DOF ToBHoMConstraint6DOF(this IFSupportStructural lusasAttribute)
        {
            List<String> releaseNames = new List<string> { "U", "V", "W", "THX", "THY", "THZ" };

            List<Boolean> fixity = new List<Boolean>();
            List<Double> stiffness = new List<double>();

            foreach (String releaseName in releaseNames)
            {
                String fixityValue = lusasAttribute.getValue(releaseName);

                if (fixityValue == "F")
                {
                    fixity.Add(false);
                    stiffness.Add(0.0);
                }
                else if (fixityValue == "R")
                {
                    fixity.Add(true);
                    stiffness.Add(0.0);
                }
                else if (fixityValue == "S")
                {
                    fixity.Add(false);
                    Double stiffnessValue = lusasAttribute.getValue(releaseName + "stiff");
                    stiffness.Add(stiffnessValue);

                }
            }

            string attributeName = GetName(lusasAttribute);

            Constraint6DOF bhomConstraint6DOF = BH.Engine.Structure.Create.Constraint6DOF(
               attributeName, fixity, stiffness);

            int bhomID = GetBHoMID(lusasAttribute, 'p');

            bhomConstraint6DOF.CustomData["Lusas_id"] = bhomID;

            return bhomConstraint6DOF;
        }


    }
}
