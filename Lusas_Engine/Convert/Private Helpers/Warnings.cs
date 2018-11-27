using Lusas.LPI;
using BH.Engine.Reflection;

namespace BH.Engine.Lusas
{
    public partial class Convert
    {
        private static void WarningPointAssignment(IFAssignment lusasAssignment)
        {
            if (lusasAssignment.getDatabaseObject() is IFPoint)
            {
                Compute.RecordWarning(
                    "This attribute does not support assignment to points, these have not been pulled");
            }
        }

        private static void WarningLineAssignment(IFAssignment lusasAssignment)
        {
            if (lusasAssignment.getDatabaseObject() is IFLine)
            {
                Compute.RecordWarning(
                    "This attribute does not support assignment to lines, these have not been pulled");
            }
        }

        private static void WarningSurfaceAssignment(IFAssignment lusasAssignment)
        {
            if (lusasAssignment.getDatabaseObject() is IFSurface)
            {
                Compute.RecordWarning(
                    "This attribute does not support assignment to surfaces, these have not been pulled");
            }
        }
    }
}