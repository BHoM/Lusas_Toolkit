/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2022, the respective contributors. All rights reserved.
 *
 * Each contributor holds copyright over their respective contributions.
 * The project versioning (Git) records all such contribution source information.
 *                                           
 *                                                                              
 * The BHoM is free software: you can redistribute it and/or modify         
 * it under the terms of the GNU Lesser General Public License as published by  
 * the Free Software Foundation, either version 3.0 of the License, or          
 * (at your option) any later version.                                          
 *                                                                              
 * The BHoM is distributed in the hope that it will be useful,              
 * but WITHOUT ANY WARRANTY; without even the implied warranty of               
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the                 
 * GNU Lesser General Public License for more details.                          
 *                                                                            
 * You should have received a copy of the GNU Lesser General Public License     
 * along with this code. If not, see <https://www.gnu.org/licenses/lgpl-3.0.html>.      
 */

using System.Collections.Generic;
using System.Linq;
using BH.Engine.Adapter;
using BH.oM.Structure.Elements;
using BH.oM.Structure.Constraints;
using BH.oM.Structure.SectionProperties;
using BH.oM.Structure.MaterialFragments;
using BH.oM.Adapters.Lusas;
using BH.oM.Adapters.Lusas.Fragments;
using Lusas.LPI;

namespace BH.Adapter.Lusas
{
#if Debug18 || Release18
    public partial class LusasV18Adapter
#elif Debug19 || Release19
    public partial class LusasV19Adapter
#else
    public partial class LusasV17Adapter
#endif
    {
        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        private List<Bar> ReadBars(List<string> ids = null)
        {
            object[] lusasLines = d_LusasData.getObjects("Line");
            List<Bar> bars = new List<Bar>();

            if (!(lusasLines.Count() == 0))
            {
                IEnumerable<Node> nodesList = ReadNodes();
                Dictionary<string, Node> nodes = nodesList.ToDictionary(
                    x => x.AdapterId<string>(typeof(LusasId)));

                IEnumerable<Constraint4DOF> supportsList = Read4DOFConstraints();
                Dictionary<string, Constraint4DOF> supports = supportsList.ToDictionary(
                    x => x.Name);

                IEnumerable<IMaterialFragment> materialList = ReadMaterials();
                Dictionary<string, IMaterialFragment> materials = materialList.ToDictionary(
                    x => x.Name.ToString());

                IEnumerable<ISectionProperty> sectionPropertiesList = ReadSectionProperties();
                Dictionary<string, ISectionProperty> sectionProperties = sectionPropertiesList.ToDictionary(
                    x => x.Name.ToString());

                List<MeshSettings1D> meshesList = ReadMeshSettings1D();
                Dictionary<string, MeshSettings1D> meshes = meshesList.ToDictionary(
                    x => x.Name.ToString());

                HashSet<string> groupNames = ReadTags();

                for (int i = 0; i < lusasLines.Count(); i++)
                {
                    IFLine lusasLine = (IFLine)lusasLines[i];
                    Bar bar = Adapters.Lusas.Convert.ToBar
                        (
                        lusasLine,
                        nodes,
                        supports,
                        groupNames,
                        materials,
                        sectionProperties,
                        meshes
                        );

                    bars.Add(bar);
                }
            }

            return bars;
        }

        /***************************************************/

    }
}


