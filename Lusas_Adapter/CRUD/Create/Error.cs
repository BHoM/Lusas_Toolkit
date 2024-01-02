/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2024, the respective contributors. All rights reserved.
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
using System.Linq;
using BH.Engine.Adapter;
using BH.oM.Adapters.Lusas;
using System;

namespace BH.Adapter.Lusas
{
#if Debug18 || Release18
    public partial class LusasV18Adapter
#elif Debug19 || Release19
    public partial class LusasV19Adapter
#elif Debug191 || Release191
    public partial class LusasV191Adapter
#elif Debug200 || Release200
    public partial class LusasV200Adapter
#else
    public partial class LusasV17Adapter
#endif
    {
        /***************************************************/

        private bool CheckPropertyWarning<T, P>(T obj, Func<T, P> selector, bool couldNotBeCreated = false)
        {
            return CheckPropertyEvent(obj, selector, couldNotBeCreated, oM.Base.Debugging.EventType.Warning);
        }

        /***************************************************/

        private bool CheckPropertyError<T, P>(T obj, Func<T, P> selector, bool couldNotBeCreated = false)
        {
            return CheckPropertyEvent(obj, selector, couldNotBeCreated, oM.Base.Debugging.EventType.Error);
        }

        /***************************************************/

        private bool CheckPropertyEvent<T, P>(T obj, Func<T, P> selector, bool couldNotBeCreated = false, BH.oM.Base.Debugging.EventType errorLevel = oM.Base.Debugging.EventType.Error)
        {
            if (obj == null || selector == null)
                return false;

            if (selector(obj) == null)
            {
                if (couldNotBeCreated)
                    Engine.Base.Compute.RecordEvent($"An object of type {obj.GetType().Name} could not be created due to a property of type {typeof(P).Name} being null. Please check your input data!", errorLevel);
                else
                    Engine.Base.Compute.RecordEvent($"A property of type {typeof(P).Name} on an object of type {obj.GetType().Name} is null and could not be set.", errorLevel);

                return false;
            }
            return true;
        }

        /***************************************************/

        private void CreateElementError(string elemType, string elemName)
        {
            Engine.Base.Compute.RecordError("Failed to create the element of type " + elemType + ", with id: " + elemName);
        }

        /***************************************************/

        private void CreatePropertyError(string failedProperty, string elemType, string elemName)
        {
            CreatePropertyEvent(failedProperty, elemType, elemName, oM.Base.Debugging.EventType.Error);
        }

        /***************************************************/

        private void CreatePropertyWarning(string failedProperty, string elemType, string elemName)
        {
            CreatePropertyEvent(failedProperty, elemType, elemName, oM.Base.Debugging.EventType.Warning);
        }

        /***************************************************/

        private void CreatePropertyEvent(string failedProperty, string elemType, string elemName, oM.Base.Debugging.EventType eventType)
        {
            Engine.Base.Compute.RecordEvent("Failed to set property " + failedProperty + " for the " + elemType + "with id: " + elemName, eventType);
        }

        /***************************************************/
    }
}


