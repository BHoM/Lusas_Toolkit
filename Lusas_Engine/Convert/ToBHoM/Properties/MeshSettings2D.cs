using Lusas.LPI;
using BH.oM.Adapters.Lusas;

namespace BH.Engine.Lusas
{
    public static partial class Convert
    {
        public static MeshSettings2D ToBHoMMeshSettings2D(this IFAttribute lusasAttribute)
        {
            string attributeName = GetName(lusasAttribute);
            //object[] elementNames = lusasMeshSurface.getElementNames();

            //foreach (object name in elnames)
            //{
            //    if (name.ToString() == "QTS4")
            //        continue;
            //    else
            //        elementType2D = ElementType2D.ThinShell;
            //}

            int xDivisions = 0;
            int yDivisions = 0;
            double size = 0;

            Split2D splitMethod = Split2D.Automatic;


            if ((lusasAttribute.getValue("size") == 0) &&
                (lusasAttribute.getValue("xDivisions") == 0 &&
                lusasAttribute.getValue("yDivisions") == 0))
            {
            }
            else if (lusasAttribute.getValue("size") == 0)
            {
                splitMethod = Split2D.Divisions;
                xDivisions = lusasAttribute.getValue("xDivisions");
                yDivisions = lusasAttribute.getValue("yDivisions");

            }
            else
            {
                splitMethod = Split2D.Size;
                size = lusasAttribute.getValue("size");
            }

            MeshSettings2D bhomMeshSettings2D = new MeshSettings2D
            {
                Name = attributeName,
                SplitMethod = splitMethod,
                xDivisions = xDivisions,
                yDivisions = yDivisions,
                ElementSize = size
            };

            int adapterID = GetAdapterID(lusasAttribute, 'e');
            bhomMeshSettings2D.CustomData["Lusas_id"] = adapterID;

            return bhomMeshSettings2D;
        }

    }
}
