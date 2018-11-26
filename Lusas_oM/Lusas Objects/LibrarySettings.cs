using BH.oM.Base;

namespace BH.oM.Adapters.Lusas
{
    public class LibrarySettings : BHoMObject
    {
        /***************************************************/
        /****            Public Properties              ****/
        /***************************************************/

        public SectionLibrary SectionDatabase { get; set; } = SectionLibrary.UK_Sections;

        /***************************************************/

    }
}
