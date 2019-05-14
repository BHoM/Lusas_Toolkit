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

using System.Linq;
using Lusas.LPI;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter
    {
        public void DeleteLineAssignments(object[] lusasAttributes)
        {
            for (int i = 0; i < lusasAttributes.Count(); i++)
            {
                IFAttribute lusasAttribute = (IFAttribute)lusasAttributes[i];
                object[] lusasAssignments = lusasAttribute.getAssignments();
                for (int j = 0; j < lusasAssignments.Count(); j++)
                {
                    IFAssignment lusasAssignment = (IFAssignment)lusasAssignments[j];
                    IFGeometry lusasGeometry = (IFGeometry)lusasAssignment.getDatabaseObject();
                    if (lusasGeometry is IFLine)
                    {
                        Engine.Reflection.Compute.RecordWarning(lusasAttribute.getName() + " has been deleted because it was assigned to a line");
                        d_LusasData.Delete(lusasGeometry);
                        break;
                    }
                }
            }
        }
    }
}