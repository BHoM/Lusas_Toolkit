/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2020, the respective contributors. All rights reserved.
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
using BH.oM.Structure.Elements;
using BH.oM.Structure.Constraints;
using BH.oM.Structure.SectionProperties;
using BH.oM.Structure.MaterialFragments;
using BH.oM.Adapters.Lusas;
using Lusas.LPI;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        private List<Bar> ReadBars(List<string> ids = null)
        {
            object[] lusasLines = d_LusasData.getObjects("Line");
            List<Bar> bhomBars = new List<Bar>();

            if(!(lusasLines.Count()==0))
            {
                IEnumerable<Node> bhomNodesList = ReadNodes();
                Dictionary<string, Node> bhomNodes = bhomNodesList.ToDictionary(
                    x => x.CustomData[AdapterIdName].ToString());

                IEnumerable<Constraint4DOF> bhomSupportList = Read4DOFConstraints();
                Dictionary<string, Constraint4DOF> bhomSupports = bhomSupportList.ToDictionary(
                    x => x.Name);

                IEnumerable<IMaterialFragment> materialList = ReadMaterials();
                Dictionary<string, IMaterialFragment> materials = materialList.ToDictionary(
                    x => x.Name.ToString());

                IEnumerable<ISectionProperty> geometricList = ReadSectionProperties();
                Dictionary<string, ISectionProperty> geometrics = geometricList.ToDictionary(
                    x => x.Name.ToString());

                List<MeshSettings1D> meshList = ReadMeshSettings1D();
                Dictionary<string, MeshSettings1D> meshes = meshList.ToDictionary(
                    x => x.Name.ToString());


                HashSet<string> groupNames = ReadTags();

                for (int i = 0; i < lusasLines.Count(); i++)
                {
                    IFLine lusasLine = (IFLine)lusasLines[i];
                    Bar bhomBar = Engine.Lusas.Convert.ToBHoMBar
                        (
                        lusasLine,
                        bhomNodes,
                        bhomSupports,
                        groupNames,
                        materials,
                        geometrics,
                        meshes
                        );

                    bhomBars.Add(bhomBar);
                }
            }

            return bhomBars;
        }
    }
}
