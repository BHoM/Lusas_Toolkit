using Lusas.LPI;

namespace BH.Engine.Lusas
{
    public partial class Convert
    {
        public static int GetAdapterID(IFAttribute lusasAttribute, char lastCharacter)
        {
            int adapterID = 0;

            lusasAttribute.getName();

            if (lusasAttribute.getName().Contains("/"))
            {
                adapterID = int.Parse(lusasAttribute.getName().Split(lastCharacter, '/')[1]);
            }
            else
            {
                adapterID = lusasAttribute.getID();
            }

            return adapterID;
        }

        public static int GetAdapterID(IFLoadcase lusasLoadcase, char lastCharacter)
        {
            int adapterID = 0;

            lusasLoadcase.getName();

            if (lusasLoadcase.getName().Contains("/"))
            {
                adapterID = int.Parse(lusasLoadcase.getName().Split(lastCharacter, '/')[1]);
            }
            else
            {
                adapterID = lusasLoadcase.getID();
            }

            return adapterID;
        }

        public static int GetAdapterID(IFBasicCombination lusasLoadCombination, char lastCharacter)
        {
            int adapterID = 0;

            lusasLoadCombination.getName();

            if (lusasLoadCombination.getName().Contains("/"))
            {
                adapterID = int.Parse(lusasLoadCombination.getName().Split(lastCharacter, '/')[1]);
            }
            else
            {
                adapterID = lusasLoadCombination.getID();
            }

            return adapterID;
        }
    }
}