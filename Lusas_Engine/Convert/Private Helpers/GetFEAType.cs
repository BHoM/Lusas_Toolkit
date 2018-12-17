using BH.oM.Structure.Elements;
using BH.Engine.Reflection;

namespace BH.Engine.Lusas
{
    public partial class Convert
    {
        public static BarFEAType GetFEAType(object type)
        {
            BarFEAType barFEAType = BarFEAType.Flexural;

            if (
                                type.ToString() == "BMI21" ||
                                type.ToString() == "BMI31" ||
                                type.ToString() == "BMX21" ||
                                type.ToString() == "BMX31" ||
                                type.ToString() == "BMI21W" ||
                                type.ToString() == "BMI31W" ||
                                type.ToString() == "BMX21W" ||
                                type.ToString() == "BMX31W")
            {
                barFEAType = BarFEAType.Flexural;
            }
            else if (
                type.ToString() == "BRS2" ||
                type.ToString() == "BRS3")
                barFEAType = BarFEAType.Axial;
            else if (
                type.ToString() == "BS4" ||
                type.ToString() == "BSL4" ||
                type.ToString() == "BXL4" ||
                type.ToString() == "JSH4" ||
                type.ToString() == "JL43" ||
                type.ToString() == "JNT4" ||
                type.ToString() == "IPN4" ||
                type.ToString() == "IPN6" ||
                type.ToString() == "LMS3" ||
                type.ToString() == "LMS4")
            {
                Compute.RecordWarning(
                    type.ToString() + " not supported, FEAType defaulted to Flexural");
            }

            return barFEAType;
        }
    }
}