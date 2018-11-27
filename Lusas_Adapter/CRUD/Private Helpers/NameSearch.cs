namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        public void NameSearch(string prefix, string customID, string suffix, ref string name)
        {
            for (int i = 1; i < int.Parse(customID); i++)
            {
                string searchName = prefix + i.ToString() + "/" + suffix;
                if (d_LusasData.existsAttribute("Loading", searchName))
                {
                    name = searchName;
                }
            }
        }
    }
}