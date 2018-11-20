using Lusas.LPI;
using BH.oM.Adapters.Lusas;

namespace BH.Engine.Lusas
{
    public static partial class Convert
    {
        public static MeshSettings2D ToBHoMMeshSettings2D(this IFMeshSurface lusasMeshSurface)
        {
            string attributeName = lusasMeshSurface.getName();
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


            if ((lusasMeshSurface.getValue("size") == 0) && 
                (lusasMeshSurface.getValue("xDivisions") ==0 &&
                lusasMeshSurface.getValue("yDivisions") == 0))
            {
            }
            else if (lusasMeshSurface.getValue("size") == 0)
            {
                splitMethod = Split2D.Divisions;
                xDivisions = lusasMeshSurface.getValue("xDivisions");
                yDivisions = lusasMeshSurface.getValue("yDivisions");

            }
            else
            {
                splitMethod = Split2D.Size;
                size = lusasMeshSurface.getValue("size");
            }

            MeshSettings2D bhomMeshSettings2D = new MeshSettings2D
            {
                Name = attributeName,
                SplitMethod = splitMethod,
                xDivisions = xDivisions,
                yDivisions = yDivisions,
                ElementSize = size
            };

            int bhomID = GetBHoMID(lusasMeshSurface, 'e');
            bhomMeshSettings2D.CustomData["Lusas_id"] = bhomID;
            return bhomMeshSettings2D;
        }

    }
}
