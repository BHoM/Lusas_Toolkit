/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2018, the respective contributors. All rights reserved.
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

        public IFPoint CreatePoint(Node node)
        {
            IFPoint lusasPoint;
            IFDatabaseOperations database_point = d_LusasData.createPoint(
                node.Position.X, node.Position.Y, node.Position.Z);

            lusasPoint = d_LusasData.getPointByNumber(d_LusasData.getLargestPointID());
            lusasPoint.setName("P" + node.CustomData[AdapterId].ToString());

            if (!(node.Tags.Count == 0))
            {
                AssignObjectSet(lusasPoint, node.Tags);
            }

            if (!(node.Constraint == null))
            {
                string constraintName = "Sp" + node.Constraint.CustomData[AdapterId] + "/" + node.Constraint.Name;
                IFAttribute lusasSupport = d_LusasData.getAttribute("Support", constraintName);
                lusasSupport.assignTo(lusasPoint);
            }

            return lusasPoint;
        }

        public IFPoint CreatePoint(Point point)
        {
            Node newNode = new Node { Position = point };

            int adapterID;
            if (newNode.CustomData.ContainsKey(AdapterId))
               adapterID= System.Convert.ToInt32(newNode.CustomData[AdapterId]);
            else
               adapterID= System.Convert.ToInt32(NextId(newNode.GetType()));

            newNode.CustomData[AdapterId] = adapterID;

            IFPoint newPoint = CreatePoint(newNode);

            return newPoint;
        }

    }
}
