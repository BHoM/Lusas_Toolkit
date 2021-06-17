/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2021, the respective contributors. All rights reserved.
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
using BH.Engine.Structure;
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
#if Debug18 || Release18
    public partial class LusasV18Adapter : BHoMAdapter
#elif Debug19 || Release19
    public partial class LusasV19Adapter : BHoMAdapter
#else
    public partial class LusasV17Adapter : BHoMAdapter
#endif
    {
        /***************************************************/
        /**** Constructors                              ****/
        /***************************************************/
#if Debug18 || Release18
        public LusasV18Adapter(string filePath, LusasConfig lusasConfig = null, bool active = false)
#elif Debug19 || Release19
        public LusasV19Adapter(string filePath, LusasConfig lusasConfig = null, bool active = false)
#else
        public LusasV17Adapter(string filePath, LusasConfig lusasConfig = null, bool active = false)
#endif
        {
            if (active)
            {
                AdapterIdFragmentType = typeof(LusasId);

                BH.Adapter.Modules.Structure.ModuleLoader.LoadModules(this);

                AdapterComparers = new Dictionary<Type, object>
                {
                    {typeof(Node), new Engine.Structure.NodeDistanceComparer(3) },
                    {typeof(Bar), new Engine.Adapters.Lusas.Object_Comparer.Equality_Comparer.BarMidPointComparer(3)},
                    {typeof(Edge), new Engine.Adapters.Lusas.Object_Comparer.Equality_Comparer.EdgeMidPointComparer(3) },
                    { typeof(Point), new Engine.Adapters.Lusas.Object_Comparer.Equality_Comparer.PointDistanceComparer(3) },
                    {typeof(IMaterialFragment), new NameOrDescriptionComparer() },
                    {typeof(LinkConstraint), new NameOrDescriptionComparer() },
                    {typeof(ISurfaceProperty), new NameOrDescriptionComparer() },
                    {typeof(ISectionProperty), new NameOrDescriptionComparer() },
                    {typeof(Constraint6DOF), new NameOrDescriptionComparer() },
                    {typeof(ILoad), new BHoMObjectNameComparer() },
                    {typeof(Loadcase), new BHoMObjectNameComparer()},
                    {typeof(LoadCombination), new BHoMObjectNameComparer()}
                };

                DependencyTypes = new Dictionary<Type, List<Type>>
                {
                    {typeof(Panel), new List<Type> { typeof(ISurfaceProperty), typeof(Edge)} },
                    {typeof(Edge), new List<Type> { typeof(Constraint4DOF), typeof(Constraint6DOF) } },
                    {typeof(Bar), new List<Type> { typeof(Node) , typeof(ISectionProperty), typeof(Constraint4DOF) } },
                    {typeof(Node), new List<Type> { typeof(Constraint6DOF) } },
                    {typeof(ISectionProperty), new List<Type> { typeof(IMaterialFragment) } },
                    {typeof(RigidLink), new List<Type> { typeof(LinkConstraint), typeof(Node) } },
                    {typeof(ISurfaceProperty), new List<Type> { typeof(IMaterialFragment) } },
                    {typeof(ILoad), new List<Type> {typeof(Loadcase) } },
                };

                if (string.IsNullOrWhiteSpace(filePath))
                {
                    throw new ArgumentException("Please specify a valid .mdl file.");
                }
                else
                {
                    m_LusasApplication = new LusasWinApp();
#if Debug17 || Release17
                    System.Runtime.InteropServices.Marshal.GetActiveObject("Lusas.Modeller.17.0");
#elif Debug18 || Release18
                    System.Runtime.InteropServices.Marshal.GetActiveObject("Lusas.Modeller.18.0");
#elif Debug19 || Release19
                    System.Runtime.InteropServices.Marshal.GetActiveObject("Lusas.Modeller.19.0");
#endif
                    m_LusasApplication.enableUI(true);
                    m_LusasApplication.setVisible(true);
                    try
                    {
                        d_LusasData = m_LusasApplication.openDatabase(filePath);
                    }
                    catch (System.Runtime.InteropServices.COMException)
                    {
                        throw new Exception("An exception has been flagged by Lusas, it is likely the file is from a higher version of Lusas than the adapter being used.");
                    }
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


