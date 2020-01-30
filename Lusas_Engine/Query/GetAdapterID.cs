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

using Lusas.LPI;

namespace BH.Engine.Lusas
{
    public partial class Query
    {
        public static int GetAdapterID(IFAttribute lusasAttribute, char lastCharacter)
        {
            int adapterID = 0;

            lusasAttribute.getName();

            if (lusasAttribute.getName().Contains("/"))
            {
                adapterID = int.Parse(lusasAttribute.getName().Split(lastCharacter, '/')[1]);
            }
            else
            {
                adapterID = lusasAttribute.getID();
            }

            return adapterID;
        }

        public static int GetAdapterID(IFLoadcase lusasLoadcase, char lastCharacter)
        {
            int adapterID = 0;

            lusasLoadcase.getName();

            if (lusasLoadcase.getName().Contains("/"))
            {
                adapterID = int.Parse(lusasLoadcase.getName().Split(lastCharacter, '/')[1]);
            }
            else
            {
                adapterID = lusasLoadcase.getID();
            }

            return adapterID;
        }

        public static int GetAdapterID(IFBasicCombination lusasLoadCombination, char lastCharacter)
        {
            int adapterID = 0;

            lusasLoadCombination.getName();

            if (lusasLoadCombination.getName().Contains("/"))
            {
                adapterID = int.Parse(lusasLoadCombination.getName().Split(lastCharacter, '/')[1]);
            }
            else
            {
                adapterID = lusasLoadCombination.getID();
            }

            return adapterID;
        }
    }
}
