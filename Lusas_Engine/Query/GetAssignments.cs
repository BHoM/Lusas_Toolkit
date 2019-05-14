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

using System.Collections.Generic;
using BH.oM.Structure.Elements;
using Lusas.LPI;
using BH.oM.Base;

namespace BH.Engine.Lusas
{
    public partial class Query
    {
        public static IEnumerable<Node> GetNodeAssignments(IEnumerable<IFAssignment> lusasAssignments,
    Dictionary<string, Node> bhomNodes)
        {
            List<Node> assignedNodes = new List<Node>();
            Node bhomNode = new Node();

            foreach (IFAssignment lusasAssignment in lusasAssignments)
            {
                if (lusasAssignment.getDatabaseObject() is IFPoint)
                {
                    IFPoint lusasPoint = (IFPoint)lusasAssignment.getDatabaseObject();
                    bhomNodes.TryGetValue(Engine.Lusas.Modify.RemovePrefix(lusasPoint.getName(), "P"), out bhomNode);
                    assignedNodes.Add(bhomNode);
                }
                else
                {
                    Compute.WarningLineAssignment(lusasAssignment);
                    Compute.WarningSurfaceAssignment(lusasAssignment);
                }
            }

            return assignedNodes;
        }

        public static IEnumerable<Bar> GetBarAssignments(IEnumerable<IFAssignment> lusasAssignments,
            Dictionary<string, Bar> bhomBars)
        {
            List<Bar> assignedBars = new List<Bar>();
            Bar bhomBar = new Bar();

            foreach (IFAssignment lusasAssignment in lusasAssignments)
            {
                if (lusasAssignment.getDatabaseObject() is IFLine)
                {
                    IFLine lusasLine = (IFLine)lusasAssignment.getDatabaseObject();
                    bhomBars.TryGetValue(Engine.Lusas.Modify.RemovePrefix(lusasLine.getName(), "L"), out bhomBar);
                    assignedBars.Add(bhomBar);
                }
                else
                {
                    Lusas.Compute.WarningPointAssignment(lusasAssignment);
                    Lusas.Compute.WarningSurfaceAssignment(lusasAssignment);
                }

            }

            return assignedBars;
        }

        public static IEnumerable<IAreaElement> GetSurfaceAssignments(IEnumerable<IFAssignment> lusasAssignments,
            Dictionary<string, Panel> bhomPanels)
        {
            List<IAreaElement> assignedSurfs = new List<IAreaElement>();
            Panel bhomPanel = new Panel();

            foreach (IFAssignment lusasAssignment in lusasAssignments)
            {
                if (lusasAssignment.getDatabaseObject() is IFSurface)
                {
                    IFSurface lusasSurface = (IFSurface)lusasAssignment.getDatabaseObject();
                    bhomPanels.TryGetValue(Engine.Lusas.Modify.RemovePrefix(lusasSurface.getName(), "S"), out bhomPanel);
                    assignedSurfs.Add(bhomPanel);
                }
                else
                {
                    Lusas.Compute.WarningPointAssignment(lusasAssignment);
                    Lusas.Compute.WarningLineAssignment(lusasAssignment);
                }
            }

            return assignedSurfs;
        }

        public static IEnumerable<BHoMObject> GetGeometryAssignments(IEnumerable<IFAssignment> lusasAssignments,
            Dictionary<string, Node> bhomNodes, Dictionary<string, Bar> bhomBars,
            Dictionary<string, Panel> bhomPanels)
        {
            List<BHoMObject> assignedObjects = new List<BHoMObject>();

            Node bhomNode = new Node();
            Bar bhomBar = new Bar();
            Panel bhomPanel = new Panel();

            foreach (IFAssignment lusasAssignment in lusasAssignments)
            {
                IFGeometry lusasGeometry = (IFGeometry)lusasAssignment.getDatabaseObject();

                if (lusasGeometry is IFPoint)
                {
                    bhomNodes.TryGetValue(Engine.Lusas.Modify.RemovePrefix(lusasGeometry.getName(), "P"), out bhomNode);
                    assignedObjects.Add(bhomNode);
                }
                else if (lusasGeometry is IFLine)
                {
                    bhomBars.TryGetValue(Engine.Lusas.Modify.RemovePrefix(lusasGeometry.getName(), "L"), out bhomBar);
                    assignedObjects.Add(bhomBar);
                }
                else if (lusasGeometry is IFSurface)
                {
                    bhomPanels.TryGetValue(Engine.Lusas.Modify.RemovePrefix(lusasGeometry.getName(), "S"), out bhomPanel);
                    assignedObjects.Add(bhomPanel);
                }
            }

            return assignedObjects;
        }
    }
}