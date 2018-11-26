using BH.oM.Adapters.Lusas;

namespace BH.Engine.Adapters.Lusas
{
    public static partial class Create
    {
        /***************************************************/
        /****           Public Constructors             ****/
        /***************************************************/

        public static LusasConfig LusasConfig(LibrarySettings librarySettings = null)
        {
            LusasConfig lusasConfig = new LusasConfig();
            if (librarySettings != null)
                lusasConfig.LibrarySettings = librarySettings;

            return lusasConfig;
        }
        /***************************************************/
    }
}
