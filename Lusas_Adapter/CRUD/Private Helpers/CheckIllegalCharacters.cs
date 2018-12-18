using System.Collections.Generic;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        public bool CheckIllegalCharacters(string objectName)
        {
            List<char> illegalCharacters = new List<char>() { '/' ,'|', '\\'};

            foreach(char character in illegalCharacters)
            {
                if (objectName.Contains(character.ToString()))
                {
                    Engine.Reflection.Compute.RecordError(
                        "Illegal character: " + character.ToString() + " present in object name: "
                         + objectName);
                    return false;
                }
            }

            return true;
        }
    }
}