using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.oM.Adapter.Lusas
{
    public enum MeshType
    {
        Line, Surface
    }

    public enum Split
    {
        Divisions, Length
    }

    public enum ElementType1D
    {
            BarL,
            ThickBeamL,
            ThickCrossSectionBeamL,
            ThickBeamWithTorsionalWarpingL,
            ThickCrossSectionBeamWithTorsionalWarpingL,
            BarQ,
            ThickBeamQ,
            ThickCrossSectionBeamQ,
            ThickBeamWithTorsionalWarpingQ,
            ThinBeam,
            SemiloofBeam,
            SemiloofCrossSectionBeam,
            JointForBeams,
            JointForSemiloof,
            JointNoRotationalStiffness,
            JointInterfaceL,
            JointInterfaceQ,
            NonStructuralMassL,
            NonStructuralMassQ
    }
}
