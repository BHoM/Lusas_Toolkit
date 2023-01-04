/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2023, the respective contributors. All rights reserved.
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

        private void ReduceRuntime(bool active)
        {
            //    if(active)
            //    {
            //        m_LusasApplication.enableUI(false);
            //        m_LusasApplication.enableTrees(false);
            //        m_LusasApplication.suppressMessages(1);
            //        m_LusasApplication.setManualRefresh(true);
            //        d_LusasData.beginCommandBatch("label", "undoable");
            //    }
            //    else
            //    {
            //        d_LusasData.closeCommandBatch();
            //        m_LusasApplication.enableTrees(true);
            //        m_LusasApplication.enableUI(true);
            //        m_LusasApplication.suppressMessages(0);
            //        m_LusasApplication.setManualRefresh(false);
            //        m_LusasApplication.updateAllViews();
            //    }
        }
    }
}



