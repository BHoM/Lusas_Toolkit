using System.Collections.Generic;
using System.Linq;
using System;
using BH.oM.Base;
using BH.oM.Structure.Elements;
using BH.oM.Geometry;
using BH.oM.Structure.Properties;
using BH.oM.Structure.Loads;
using BH.oM.Common.Materials;
using BH.Engine.Geometry;
using BH.Engine.Structure;
using Lusas.LPI;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        public List<IFLoadingBeamDistributed> CreateBarDistributedLoad(BarVaryingDistributedLoad bhomBarDistributedLoad, IFLine[] lusasLines)
        {
            List<IFLoadingBeamDistributed> lusasBarDistributedLoads = new List<IFLoadingBeamDistributed>();
            IFAssignment assignToBars = m_LusasApplication.assignment();
            IFLoadcase assignedLoadcase = (IFLoadcase)d_LusasData.getLoadset("Lc" + bhomBarDistributedLoad.Loadcase.CustomData[AdapterId] + "/" + bhomBarDistributedLoad.Loadcase.Name);
            string lusasAttributeName = "Pl" + bhomBarDistributedLoad.CustomData[AdapterId] + "/" + bhomBarDistributedLoad.Name;

            if (d_LusasData.existsAttribute("Loading", lusasAttributeName))
            {
                IFLoadingBeamDistributed lusasBarDistributedLoad = (IFLoadingBeamDistributed)d_LusasData.getAttribute("Loading", lusasAttributeName);
                lusasBarDistributedLoads.Add(lusasBarDistributedLoad);
            }
            else
            {
                List<double> values = new List<double> { bhomBarDistributedLoad.ForceA.X, bhomBarDistributedLoad.ForceA.Y, bhomBarDistributedLoad.ForceA.Z, bhomBarDistributedLoad.MomentA.X, bhomBarDistributedLoad.MomentA.Y, bhomBarDistributedLoad.MomentA.Z};
                List<string> keys = new List<string> { "FX", "FY", "FZ", "MX", "MY", "MZ"};
                Dictionary<string, double> dict = keys.Zip(values, (k, v) => new { k, v }).ToDictionary(x => x.k, x => x.v);

                foreach (string key in dict.Keys)
                {
                    double value;
                    dict.TryGetValue(key, out value);
                    if (value!=0)
                    {
                        IFLoadingBeamDistributed lusasBarDistributedLoad = d_LusasData.createLoadingBeamDistributed(bhomBarDistributedLoad.Name + key);

                        if (bhomBarDistributedLoad.Axis.ToString() == "Global")
                            lusasBarDistributedLoad.setBeamDistributed("parametric", "global", "beam");
                        else
                            lusasBarDistributedLoad.setBeamDistributed("parametric", "local", "beam");

                        switch(key)
                        {
                            case "FX":
                                lusasBarDistributedLoad.addRow(bhomBarDistributedLoad.DistanceFromA, bhomBarDistributedLoad.ForceA.X, 0, 0, 0, 0, 0, bhomBarDistributedLoad.DistanceFromB, bhomBarDistributedLoad.ForceB.X,0, 0, 0, 0, 0);
                                lusasBarDistributedLoads.Add(lusasBarDistributedLoad);
                                assignToBars.setLoadset(assignedLoadcase);
                                lusasBarDistributedLoad.assignTo(lusasLines, assignToBars);
                                break;

                            case "FY":
                                lusasBarDistributedLoad.addRow(bhomBarDistributedLoad.DistanceFromA, 0, bhomBarDistributedLoad.ForceA.Y, 0, 0, 0, 0, bhomBarDistributedLoad.DistanceFromB, 0, bhomBarDistributedLoad.ForceB.Y, 0, 0, 0, 0);
                                lusasBarDistributedLoads.Add(lusasBarDistributedLoad);
                                assignToBars.setLoadset(assignedLoadcase);
                                lusasBarDistributedLoad.assignTo(lusasLines, assignToBars);
                                break;

                            case "FZ":
                                lusasBarDistributedLoad.addRow(bhomBarDistributedLoad.DistanceFromA, 0, 0, bhomBarDistributedLoad.ForceA.Z, 0, 0, 0, bhomBarDistributedLoad.DistanceFromB, 0, 0, bhomBarDistributedLoad.ForceB.Z, 0, 0, 0);
                                lusasBarDistributedLoads.Add(lusasBarDistributedLoad);
                                assignToBars.setLoadset(assignedLoadcase);
                                lusasBarDistributedLoad.assignTo(lusasLines, assignToBars);
                                break;

                            case "MX":
                                lusasBarDistributedLoad.addRow(bhomBarDistributedLoad.DistanceFromA, 0, 0, 0, bhomBarDistributedLoad.MomentA.X, 0, 0, bhomBarDistributedLoad.DistanceFromB, 0, 0, 0, bhomBarDistributedLoad.MomentB.X, 0, 0);
                                lusasBarDistributedLoads.Add(lusasBarDistributedLoad);
                                assignToBars.setLoadset(assignedLoadcase);
                                lusasBarDistributedLoad.assignTo(lusasLines, assignToBars);
                                break;

                            case "MY":
                                lusasBarDistributedLoad.addRow(bhomBarDistributedLoad.DistanceFromA, 0, 0, 0, 0, bhomBarDistributedLoad.MomentA.Y, 0, bhomBarDistributedLoad.DistanceFromB, 0, 0, 0, 0, bhomBarDistributedLoad.MomentB.Y, 0);
                                lusasBarDistributedLoads.Add(lusasBarDistributedLoad);
                                assignToBars.setLoadset(assignedLoadcase);
                                lusasBarDistributedLoad.assignTo(lusasLines, assignToBars);
                                break;

                            case "MZ":
                                lusasBarDistributedLoad.addRow(bhomBarDistributedLoad.DistanceFromA, 0, 0, 0, 0, 0, bhomBarDistributedLoad.MomentA.Z, bhomBarDistributedLoad.DistanceFromB, 0, 0, 0, 0, 0, bhomBarDistributedLoad.MomentB.Z);
                                lusasBarDistributedLoads.Add(lusasBarDistributedLoad);
                                assignToBars.setLoadset(assignedLoadcase);
                                lusasBarDistributedLoad.assignTo(lusasLines, assignToBars);
                                break;
                        }

                    }
                }
            }

            return lusasBarDistributedLoads;
        }
    }
}