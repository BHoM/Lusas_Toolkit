using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.oM.Base;
using BH.oM.Geometry;
using BH.oM.Structural.Elements;
using BH.oM.Structural.Properties;
using BH.oM.Common.Materials;
using LusasM15_2;

namespace BH.Engine.Lusas
{
    public static partial class Convert
    {
        public static Constraint6DOF ToBHoMObject(this IFAttribute lusasAttribute)
        {
            List<String> releaseNames = new List<string> { "U", "V", "W", "THX", "THY", "THZ" };

            List<Boolean> fixity = new List<Boolean>();
            List<Double> stiffness = new List<double>();

            foreach(String releaseName in releaseNames)
            {
                String fixityValue = lusasAttribute.getValue(releaseName);
                
                if(fixityValue == "F")
                {
                    fixity.Add(false);
                    stiffness.Add(0.0);
                }
                else if(fixityValue == "R")
                {
                    fixity.Add(true);
                    stiffness.Add(0.0);
                }
                else if(fixityValue == "S")
                {
                    fixity.Add(false);
                    Double stiffnessValue = lusasAttribute.getValue(releaseName+"stiff");
                    stiffness.Add(stiffnessValue);

                }
            }

            string attributeName = "";

            if(lusasAttribute.getName().Contains("/"))
            {
                attributeName = lusasAttribute.getName().Substring(
                    lusasAttribute.getName().LastIndexOf("/") + 1);
            }
            else
            {
                attributeName = lusasAttribute.getName();
            }

            Constraint6DOF bhomConstraint6DOF = BH.Engine.Structure.Create.Constraint6DOF(
               attributeName, fixity, stiffness);


            String pointName = removePrefix(lusasAttribute.getName(), "P");

            //Extract tags and set tags

            return bhomConstraint6DOF;
        }
    }
}
