/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2019, the respective contributors. All rights reserved.
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


using BH.Engine.Base.Objects;
using BH.oM.Adapter;
using BH.oM.Adapters.Lusas;
using BH.oM.Geometry;
using BH.oM.Structure.Elements;
using BH.oM.Structure.Constraints;
using BH.oM.Structure.SectionProperties;
using BH.oM.Structure.SurfaceProperties;
using BH.oM.Structure.Loads;
using BH.oM.Structure.MaterialFragments;
using Lusas.LPI;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter : BHoMAdapter
    {

        /***************************************************/
        /**** Constructors                              ****/
        /***************************************************/

        public LusasAdapter(string filePath, LusasConfig lusasConfig = null, bool active = false)
        {
            if (active)
            {
                AdapterIdName = "Lusas_id";   //Set the "AdapterId" to "SoftwareName_id". Generally stored as a constant string in the convert class in the SoftwareName_Engine

                BH.Adapter.Modules.Structure.ModuleLoader.LoadModules(this);

                AdapterComparers = new Dictionary<Type, object>
                {
                    {typeof(Node), new Engine.Structure.NodeDistanceComparer(3) },
                    {typeof(Bar), new Engine.Lusas.Object_Comparer.Equality_Comparer.BarMidPointComparer(3)},
                    {typeof(Edge), new Engine.Lusas.Object_Comparer.Equality_Comparer.EdgeMidPointComparer(3) },
                    { typeof(Point), new Engine.Lusas.Object_Comparer.Equality_Comparer.PointDistanceComparer(3) },
                    {typeof(IMaterialFragment), new BHoMObjectNameComparer() },
                    {typeof(LinkConstraint), new BHoMObjectNameComparer() },
                    {typeof(ISurfaceProperty), new BHoMObjectNameComparer() },
                    {typeof(ISectionProperty), new BHoMObjectNameComparer() },
                    {typeof(ILoad), new BHoMObjectNameComparer() },
                    {typeof(Loadcase), new BHoMObjectNameComparer()},
                    {typeof(LoadCombination), new BHoMObjectNameComparer()}
                };

                DependencyTypes = new Dictionary<Type, List<Type>>
                {
                    {typeof(Panel), new List<Type> { typeof(ISurfaceProperty), typeof(Edge)} },
                    {typeof(Bar), new List<Type> { typeof(Node) , typeof(ISectionProperty), typeof(Constraint4DOF) } },
                    {typeof(Node), new List<Type> { typeof(Constraint6DOF) } },
                    {typeof(ISectionProperty), new List<Type> { typeof(IMaterialFragment) } },
                    {typeof(RigidLink), new List<Type> { typeof(LinkConstraint), typeof(Node) } },
                    {typeof(ISurfaceProperty), new List<Type> { typeof(IMaterialFragment) } },
                    {typeof(ILoad), new List<Type> {typeof(Loadcase) } },
                };

                if (string.IsNullOrWhiteSpace(filePath))
                {
                    throw new ArgumentException("No file path given");
                }
                else if (IsApplicationRunning())
                {
                    throw new Exception("Lusas process already running");
                }
                else
                {
                    m_LusasApplication = new LusasWinApp();
                    m_LusasApplication.enableUI(true);
                    m_LusasApplication.setVisible(true);
                    d_LusasData = m_LusasApplication.openDatabase(filePath);
                }
            }
        }

        /***************************************************/
        /**** Public  Fields                           ****/
        /***************************************************/

        public static bool IsApplicationRunning()
        {
            return (Process.GetProcessesByName("lusas_m").Length > 0) ? true : false;
        }

        /***************************************************/
        /**** Private  Fields                           ****/
        /***************************************************/

        //Add any comlink object as a private field here, example named:

        public LusasWinApp m_LusasApplication;
        public IFDatabase d_LusasData;
        private Dictionary<Type, Dictionary<int, HashSet<string>>> m_tags = new Dictionary<Type, Dictionary<int, HashSet<string>>>();
        public LusasConfig lusasConfig;


        /***************************************************/


    }
}
