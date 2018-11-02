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

namespace BH.Engine.Lusas
{
    public static partial class Convert
    {
        public static ISectionProperty ToBHoMSection(this IFAttribute lusasAttribute)
        {
            string attributeName = GetName(lusasAttribute);

            IProfile sectionProfile = createProfile(lusasAttribute);

            double area = lusasAttribute.getValue("A");
            double rgy = lusasAttribute.getValue("ky");
            double rgz = lusasAttribute.getValue("kz");
            double j = lusasAttribute.getValue("J");
            double iz = lusasAttribute.getValue("Iyy");
            double iy = lusasAttribute.getValue("Izz");
            double iw = lusasAttribute.getValue("Cw");
            double wely = Math.Min(lusasAttribute.getValue("Syt"), lusasAttribute.getValue("Syb"));
            double welz = Math.Min(lusasAttribute.getValue("Szt"), lusasAttribute.getValue("Szb"));
            double wply = lusasAttribute.getValue("Zpy");
            double wplz = lusasAttribute.getValue("Zpz");
            double centreZ = lusasAttribute.getValue("zo");
            double centreY = lusasAttribute.getValue("yo");
            double zt = lusasAttribute.getValue("zt");
            double zb = lusasAttribute.getValue("zb");
            double yt = lusasAttribute.getValue("yt");
            double yb = lusasAttribute.getValue("yb");
            double asy = lusasAttribute.getValue("Asy");
            double asz = lusasAttribute.getValue("Asz");

            SteelSection bhomSection = new SteelSection(
                sectionProfile, area, rgy, rgz, j, iy, iz, iw,
                wely, welz, wply, wplz, centreZ, centreY, zt, zb, yt, yb, asy, asz);

            bhomSection.Name = attributeName;

            int bhomID = GetBHoMID(lusasAttribute, 'M');

            bhomSection.CustomData["Lusas_id"] = bhomID;

            return bhomSection;
        }

        public static IProfile createProfile(IFAttribute lusasAttribute)
        {
            int sectionType = lusasAttribute.getValue("Type");

            IProfile sectionProfile = null;

            if (sectionType == 1)
            {
                sectionProfile = BH.Engine.Structure.Create.RectangleProfile(
                    lusasAttribute.getValue("D"),
                    lusasAttribute.getValue("B"),
                    0
                );
            }
            else if (sectionType == 2)
            {
                sectionProfile = BH.Engine.Structure.Create.BoxProfile(
                    lusasAttribute.getValue("D"),
                    lusasAttribute.getValue("B"),
                    lusasAttribute.getValue("t"),
                    lusasAttribute.getValue("r"),
                    lusasAttribute.getValue("r")
                );
            }
            else if(sectionType == 3)
            {
                sectionProfile = BH.Engine.Structure.Create.CircleProfile(
                    lusasAttribute.getValue("D")
                );
            }
            else if (sectionType == 4)
            {
                sectionProfile = BH.Engine.Structure.Create.TubeProfile(
                    lusasAttribute.getValue("D"),
                    lusasAttribute.getValue("t")
                );
            }
            else if (sectionType == 5)
            {
                sectionProfile = BH.Engine.Structure.Create.FabricatedISectionProfile(
                    lusasAttribute.getValue("D"),
                    lusasAttribute.getValue("B"),
                    lusasAttribute.getValue("B"),
                    lusasAttribute.getValue("tw"),
                    lusasAttribute.getValue("tf"),
                    lusasAttribute.getValue("tf"),
                    lusasAttribute.getValue("r")
                );
            }
            else if (sectionType == 14)
            {
                sectionProfile = BH.Engine.Structure.Create.FabricatedISectionProfile(
                    lusasAttribute.getValue("D"),
                    lusasAttribute.getValue("Bt"),
                    lusasAttribute.getValue("Bb"),
                    lusasAttribute.getValue("tw"),
                    lusasAttribute.getValue("tft"),
                    lusasAttribute.getValue("tfb"),
                    lusasAttribute.getValue("r")
                );
            }
            else if (sectionType == 8)
            {
                sectionProfile = BH.Engine.Structure.Create.TSectionProfile(
                    lusasAttribute.getValue("D"),
                    lusasAttribute.getValue("B"),
                    lusasAttribute.getValue("tw"),
                    lusasAttribute.getValue("tf"),
                    lusasAttribute.getValue("r"),
                    lusasAttribute.getValue("r")
                );
            }
            return sectionProfile;

        }
    }
}