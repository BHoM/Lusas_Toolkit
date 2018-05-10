using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.Adapter;
using BH.Engine.Lusas;
using System.Diagnostics;
using System.IO;
using LusasM15_2;

namespace BH.Adapter.Lusas
{
    public partial class LusasAdapter : BHoMAdapter
    {

        /***************************************************/
        /**** Constructors                              ****/
        /***************************************************/

        //Add any applicable constructors here, such as linking to a specific file or anything else as well as linking to that file through the (if existing) com link via the API
        public LusasAdapter()
        {
            AdapterId = BH.Engine.Lusas.Convert.AdapterId;   //Set the "AdapterId" to "SoftwareName_id". Generally stored as a constant string in the convert class in the SoftwareName_Engine

            Config.SeparateProperties = true;   //Set to true to push dependant properties of objects before the main objects are being pushed. Example: push nodes before pushing bars
            Config.MergeWithComparer = true;    //Set to true to use EqualityComparers to merge objects. Example: merge nodes in the same location
            Config.ProcessInMemory = false;     //Set to false to to update objects in the toolkit during the push
            Config.CloneBeforePush = true;      //Set to true to clone the objects before they are being pushed through the software. Required if any modifications at all, as adding a software ID is done to the objects
            Config.UseAdapterId = true;         //Tag objects with a software specific id in the CustomData. Requires the NextIndex method to be overridden and implemented

            if (IsApplicationRunning())
            {
                throw new System.Exception("Lusas is already running, close and run again");
            }
            else
            {
                try
                {
                    m_LusasApplication = new LusasWinApp();
                    m_LusasApplication.enableUI(true);
                    m_LusasApplication.setVisible(true);
                    d_LusasData = m_LusasApplication.newDatabase();
                    IFTextWindow w_lusaswindow = m_LusasApplication.textWin();
                    d_LusasData.setLogicalUpAxis("Z");
                    d_LusasData.setModelUnits("kN,m,t,s,C");
                    w_lusaswindow.writeLine("New Model Created");
                }
                catch
                {
                }
            }
        }


        public LusasAdapter(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new System.ArgumentException("No file path given");    
            }      
            else if (IsApplicationRunning())
            {
                throw new System.Exception("Lusas process already running");
            }
            else
            {
                m_LusasApplication = new LusasWinApp();
                m_LusasApplication.enableUI(true);
                m_LusasApplication.setVisible(true);
                d_LusasData = m_LusasApplication.openDatabase(filePath);
                IFTextWindow lusaswindow = m_LusasApplication.textWin();
                lusaswindow.writeLine("Model has been opened");
            }
        }

        /***************************************************/
        /**** Public  Fields                           ****/
        /***************************************************/

        public static bool IsApplicationRunning()
        {
            return (Process.GetProcessesByName("lusas_m").Length > 0) ? true : false;
        }


        /***************************************************/
        /**** Private  Fields                           ****/
        /***************************************************/

        //Add any comlink object as a private field here, example named:

        private LusasWinApp m_LusasApplication;
        private IFDatabase d_LusasData;

        /***************************************************/


    }
}
