﻿using System.Collections.Generic;
using System.Linq;
using BH.oM.Structure.Loads;
using Lusas.LPI;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        public IFPrescribedDisplacementLoad CreatePrescribedDisplacement(PointDisplacement pointDisplacement, IFPoint[] lusasPoints)
        {
            if (!CheckIllegalCharacters(pointDisplacement.Name))
            {
                return null;
            }

            IFPrescribedDisplacementLoad lusasPrescribedDisplacement = null;
            IFLoadcase assignedLoadcase = (IFLoadcase)d_LusasData.getLoadset(
                "Lc" + pointDisplacement.Loadcase.CustomData[AdapterId] + "/" + pointDisplacement.Loadcase.Name);

            string lusasName = "Pd" + 
                pointDisplacement.CustomData[AdapterId] + "/" + pointDisplacement.Name;

            NameSearch("Pd", pointDisplacement.CustomData[AdapterId].ToString(), 
                pointDisplacement.Name, ref lusasName);

            if (d_LusasData.existsAttribute("Loading", lusasName))
            {
                lusasPrescribedDisplacement = (IFPrescribedDisplacementLoad)d_LusasData.getAttribute(
                    "Loading", lusasName);
            }
            else
            {
                List<string> valueNames = new List<string> { "u", "v", "w", "thx", "thy", "thz" };
                List<string> boolCheck = new List<string> { "haveDispX", "haveDispY", "haveDispZ",
                    "haveRotX", "haveRotY", "haveRotZ" };
                List<double> displacements = new List<double>
                {
                    pointDisplacement.Translation.X, pointDisplacement.Translation.Y,
                    pointDisplacement.Translation.Z,
                    pointDisplacement.Rotation.X,pointDisplacement.Rotation.Y,
                    pointDisplacement.Rotation.Z
                };

                lusasPrescribedDisplacement = d_LusasData.createPrescribedDisplacementLoad(
                    lusasName, "Total");

                for(int i=0; i < valueNames.Count(); i++)
                {
                    if(!(displacements[i] == 0))
                    {
                        lusasPrescribedDisplacement.setValue(boolCheck[i], true);
                        lusasPrescribedDisplacement.setValue(valueNames[i], displacements[i]);
                    }
                    else
                    {
                        lusasPrescribedDisplacement.setValue(boolCheck[i], false);
                    }
                }

            }

            IFAssignment lusasAssignment = m_LusasApplication.assignment();
            lusasAssignment.setLoadset(assignedLoadcase);
            lusasPrescribedDisplacement.assignTo(lusasPoints, lusasAssignment);

            return lusasPrescribedDisplacement;
        }
    }
}