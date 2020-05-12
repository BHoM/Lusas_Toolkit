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

using BH.oM.Structure.Elements;
using BH.oM.Geometry;
using Lusas.LPI;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        private IFPoint CreatePoint(Node node)
        {
            IFPoint lusasPoint;
            Point position = Engine.Structure.Query.Position(node);
            IFDatabaseOperations database_point = d_LusasData.createPoint(
                position.X, position.Y, position.Z);

            lusasPoint = d_LusasData.getPointByNumber(d_LusasData.getLargestPointID());
            lusasPoint.setName("P" + node.CustomData[AdapterIdName].ToString());

            if (!(node.Tags.Count == 0))
            {
                AssignObjectSet(lusasPoint, node.Tags);
            }

            if (!(node.Support == null))
            {
                string constraintName = "Sp" + node.Support.CustomData[AdapterIdName] + "/" + node.Support.Name;
                IFAttribute lusasSupport = d_LusasData.getAttribute("Support", constraintName);
                lusasSupport.assignTo(lusasPoint);
            }

            return lusasPoint;
        }

        /***************************************************/

        private IFPoint CreatePoint(Point point)
        {
            Node newNode = Engine.Structure.Create.Node(new Point { X = point.X, Y = point.Y, Z = point.Z });

            int adapterID;
            if (newNode.CustomData.ContainsKey(AdapterIdName))
                adapterID = System.Convert.ToInt32(newNode.CustomData[AdapterIdName]);
            else
                adapterID = System.Convert.ToInt32(NextFreeId(newNode.GetType()));

            newNode.CustomData[AdapterIdName] = adapterID;

            IFPoint newPoint = CreatePoint(newNode);

            return newPoint;
        }

        /***************************************************/

    }
}

