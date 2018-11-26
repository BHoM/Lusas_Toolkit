using BH.oM.Base;

namespace BH.oM.Adapters.Lusas
{
    public class LusasConfig : BHoMObject
    {
        /***************************************************/
        /****            Public Properties              ****/
        /***************************************************/

        public LibrarySettings LibrarySettings { get; set; } = new LibrarySettings();

        /***************************************************/
    }
}
