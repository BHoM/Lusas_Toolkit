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
using BH.oM.Structure.SurfaceProperties;
using BH.oM.Structure.MaterialFragments;
using Lusas.LPI;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        private List<Panel> ReadPanels(List<string> ids = null)
        {
            object[] lusasSurfaces = d_LusasData.getObjects("Surface");
            List<Panel> bhomSurfaces = new List<Panel>();

            if (!(lusasSurfaces.Count() == 0))
            {
                IEnumerable<Edge> bhomEdgesList = ReadEdges();
                Dictionary<string, Edge> bhomEdges = bhomEdgesList.ToDictionary(
                    x => x.CustomData[AdapterIdName].ToString());

                HashSet<string> groupNames = ReadTags();
                IEnumerable<IMaterialFragment> materialList = ReadMaterials();
                Dictionary<string, IMaterialFragment> materials = materialList.ToDictionary(
                    x => x.Name.ToString());

                IEnumerable<ISurfaceProperty> geometricList = Read2DProperties();
                Dictionary<string, ISurfaceProperty> geometrics = geometricList.ToDictionary(
                    x => x.Name.ToString());

                IEnumerable<Constraint4DOF> bhomSupportList = Read4DOFConstraints();
                Dictionary<string, Constraint4DOF> bhomSupports = bhomSupportList.ToDictionary(
                    x => x.Name);

                for (int i = 0; i < lusasSurfaces.Count(); i++)
                {
                    IFSurface lusasSurface = (IFSurface)lusasSurfaces[i];
                    Panel bhomPanel = Adapter.Adapters.Lusas.Convert.ToPanel(lusasSurface,
                        bhomEdges,
                        groupNames,
                        geometrics,
                        materials,
                        bhomSupports);

                    bhomSurfaces.Add(bhomPanel);
                }
            }

            return bhomSurfaces;
        }

        /***************************************************/

    }
}
