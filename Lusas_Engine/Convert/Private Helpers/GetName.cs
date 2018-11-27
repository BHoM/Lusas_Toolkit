using Lusas.LPI;
namespace BH.Engine.Lusas
{
    public partial class Convert
    {
        public static string GetName(IFAttribute lusasAttribute)
        {
            string attributeName = "";

            if (lusasAttribute.getName().Contains("/"))
            {
                attributeName = lusasAttribute.getName().Substring(
                    lusasAttribute.getName().LastIndexOf("/") + 1);
            }
            else
            {
                attributeName = lusasAttribute.getName();
            }

            return attributeName;
        }

        public static string GetName(IFLoadcase lusasLoadcase)
        {
            string loadcaseName = "";

            if (lusasLoadcase.getName().Contains("/"))
            {
                loadcaseName = lusasLoadcase.getName().Substring(
                    lusasLoadcase.getName().LastIndexOf("/") + 1);
            }
            else
            {
                loadcaseName = lusasLoadcase.getName();
            }

            return loadcaseName;
        }

        public static string GetName(IFBasicCombination lusasLoadCombination)
        {
            string loadcaseName = "";

            if (lusasLoadCombination.getName().Contains("/"))
            {
                loadcaseName = lusasLoadCombination.getName().Substring(
                    lusasLoadCombination.getName().LastIndexOf("/") + 1);
            }
            else
            {
                loadcaseName = lusasLoadCombination.getName();
            }

            return loadcaseName;
        }

        public static string GetName(string loadName)
        {
            string bhomLoadName = "";

            if (loadName.Contains("/"))
            {
                bhomLoadName = loadName.Substring(
                    loadName.LastIndexOf("/") + 1);
            }
            else
            {
                bhomLoadName = loadName;
            }

            return bhomLoadName;
        }
    }
}