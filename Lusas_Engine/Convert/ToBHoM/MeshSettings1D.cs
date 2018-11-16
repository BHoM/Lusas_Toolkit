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
        public static MeshSettings1D ToBHoMMeshSettings1D(this IFAttribute lusasAttribute)
        {
            string attributeName = GetName(lusasAttribute);

            MeshSettings1D bhomMeshSettings1D = new MeshSettings1D
            {
                Name = attributeName,
                MeshType = MeshType.Line,
                ElementType1D 
            }

            IProperty2D bhomProperty2D = new ConstantThickness
            {
                Name = attributeName,
                Thickness = lusasAttribute.getValue("t")
            };


            int bhomID = GetBHoMID(lusasAttribute, 'G');

            bhomProperty2D.CustomData["Lusas_id"] = bhomID;

            return bhomProperty2D;
        }

        public static Dictionary<string, string> Elements1D = new Dictionary<string, string>()
        {
            {"BRS2","Bar"},
            { "BMI21","Thick beam"},
            { "BMX21","Thick cross section beam"},
            { "BMI21W","Thick beam with torsional warping"},
            { "BMX21W","Thick cross section beam with torsional warping"},
            { "BRS3","Bar"},
            { "BMI31","Thick beam"},
            { "BMX31","Thick cross section beam"},
            { "BMI31W","Thick beam with torsional warping"},
            { "BS4","Thin beam"},
            { "BSL4","Semiloof beam"},
            {"BXL4","Semiloof cross section beam"},
            {,"Joint for beams" "JSH4"},
            {,"Joint for semiloof" "JL43"},
            {,"Joint no rotational stiffness" "JNT4"},
            {,"Joint interface""IPN4"},
            {,"Joint interface""IPN6"},
            {,"Non-structural mass""LMS3"},
            {,"Non-structural mass""LMS4"}
        };

    }
}
