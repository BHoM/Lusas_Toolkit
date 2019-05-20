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
using BH.oM.Geometry;
using BH.Engine.Structure;

namespace BH.Engine.Lusas.Object_Comparer.Equality_Comparer
{
    public class PanelCentreComparer : IEqualityComparer<Panel>
    {
        /***************************************************/
        /**** Constructors                              ****/
        /***************************************************/
        public PanelCentreComparer()
        {
            nodeComparer = new NodeDistanceComparer();
        }

        /***************************************************/
        public PanelCentreComparer(int decimals)
        {
            nodeComparer = new NodeDistanceComparer(decimals);
        }

        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/
        public bool Equals(Panel panel1, Panel panel2)
        {
            if (ReferenceEquals(panel1, panel2)) return true;

            if (ReferenceEquals(panel1, null) || ReferenceEquals(panel2, null))
                return false;

            if (panel1.BHoM_Guid == panel2.BHoM_Guid)
                return true;

            List<Point> controlPoints1 = BH.Engine.Structure.Query.ControlPoints(panel1, true);
            List<Point> controlPoints2 = BH.Engine.Structure.Query.ControlPoints(panel2, true);
            Point centrePoint1 = BH.Engine.Geometry.Query.Average(controlPoints1);
            Point centrePoint2 = BH.Engine.Geometry.Query.Average(controlPoints2);

            if (nodeComparer.Equals(BH.Engine.Lusas.Convert.PointToNode(centrePoint1), BH.Engine.Lusas.Convert.PointToNode(centrePoint2)))
                return nodeComparer.Equals(BH.Engine.Lusas.Convert.PointToNode(centrePoint1), BH.Engine.Lusas.Convert.PointToNode(centrePoint2));

            return false;
        }

        /***************************************************/
        public int GetHashCode(Panel panel)
        {
            return panel.GetHashCode();
        }

        /***************************************************/
        /**** Private Fields                            ****/
        /***************************************************/

        private NodeDistanceComparer nodeComparer;

        /***************************************************/

    }
}
