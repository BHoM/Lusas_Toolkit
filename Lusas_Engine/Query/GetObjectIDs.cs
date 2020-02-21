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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BH.oM.Base;

namespace BH.Engine.Lusas
{
    public static partial class Query
    {
        public static List<int> GetObjectIDs(IList ids)
        {
            if (ids == null)
            {
                return null;
            }
            else
            {
                if (ids is List<string>)
                    return (ids as List<string>).Select(x => int.Parse(x)).ToList();
                else if (ids is List<int>)
                    return ids as List<int>;
                else if (ids is List<double>)
                    return (ids as List<double>).Select(x => (int)Math.Round(x)).ToList();
                else
                {
                    List<int> idsOut = new List<int>();
                    foreach (object o in ids)
                    {
                        int id;
                        object idObj;
                        if (int.TryParse(o.ToString(), out id))
                        {
                            idsOut.Add(id);
                        }
                        else if (o is IBHoMObject && (o as IBHoMObject).CustomData.TryGetValue("Lusas_id", out idObj) && int.TryParse(idObj.ToString(), out id))
                            idsOut.Add(id);
                    }
                    return idsOut;
                }
            }
        } 

    }
}



