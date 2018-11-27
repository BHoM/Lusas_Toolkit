namespace BH.Engine.Lusas
{
    public partial class Convert
    {
        public static string RemovePrefix(string name, string forRemoval)
        {
            string adapterID = "";

            if (name.Contains(forRemoval))
            {
                adapterID = name.Replace(forRemoval, "");
            }
            else
            {
                adapterID = name;
            }
            return adapterID;
        }
    }
}