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

using BH.oM.Adapters.Lusas;
using BH.oM.Structure.Elements;
using BH.oM.Geometry;
using BH.Engine.Structure;
using BH.Engine.Adapter;
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

        private IFPoint CreatePoint(Node node)
        {
            Point position = Engine.Structure.Query.Position(node);
            IFDatabaseOperations databasePoint = d_LusasData.createPoint(
                position.X, position.Y, position.Z);
            IFPoint lusasPoint = d_LusasData.getPointByNumber(d_LusasData.getLargestPointID());


            int adapterIdName = lusasPoint.getID();
            node.SetAdapterId(typeof(LusasId), adapterIdName);


            if (!(node.Tags.Count == 0))
            {
                AssignObjectSet(lusasPoint, node.Tags);
            }

            if (!(node.Support == null))
            {
                IFAttribute lusasSupport = d_LusasData.getAttribute("Support", System.Convert.ToInt32(node.Support.AdapterId<int>(typeof(LusasId))));
                lusasSupport.assignTo(lusasPoint);
            }

            return lusasPoint;
        }

        /***************************************************/

        private IFPoint CreatePoint(Point point)
        {
            Node newNode = Create.Node(new Point { X = point.X, Y = point.Y, Z = point.Z });

            IFPoint newPoint = CreatePoint(newNode);

            return newPoint;
        }

        /***************************************************/

    }
}

