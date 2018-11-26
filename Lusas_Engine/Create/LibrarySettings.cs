using BH.oM.Adapters.Lusas;

namespace BH.Engine.Adapters.Lusas
{
    public static partial class Create
    {
        /***************************************************/
        /****           Public Constructors             ****/
        /***************************************************/

        public static LibrarySettings LibrarySettings(SectionLibrary sectionLibrary = SectionLibrary.UK_Sections)
        {
            LibrarySettings librarySettings = new LibrarySettings();
            librarySettings.SectionDatabase = sectionLibrary;

            return librarySettings;
        }
    }
}