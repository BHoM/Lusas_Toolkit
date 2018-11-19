using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.oM.Base;
using BH.oM.Geometry;
using BH.oM.Structure.Elements;
using BH.oM.Structure.Properties;
using BH.oM.Common.Materials;
using Lusas.LPI;
using BH.oM.Adapter.Lusas;

namespace BH.Engine.Lusas
{
    public static partial class Convert
    {
        public static MeshSettings2D ToBHoMMeshSettings2D(this IFMeshSurface lusasMeshSurface)
        {
            string attributeName = lusasMeshSurface.getName();
            object[] elnames = lusasMeshSurface.getElementNames();
            ElementType2D elementType2D = ElementType2D.ThickShell;
            foreach (object name in elnames)
            {
                if (name.ToString() == "QTS4")
                    continue;
                else
                    elementType2D = ElementType2D.ThinShell;
            }

            int xDivisions = 0;
            int yDivisions = 0;
            double size = 1;

            Split2D splitMethod = Split2D.Divisions;

            if (lusasMeshSurface.getValue("size") == 0)
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
                ElementType2D = elementType2D,
                SplitMethod = splitMethod,
                xDivisions = xDivisions,
                yDivisions = yDivisions
            };

            int bhomID = GetBHoMID(lusasMeshSurface, 'e');
            bhomMeshSettings2D.CustomData["Lusas_id"] = bhomID;
            return bhomMeshSettings2D;
        }

    }
}
