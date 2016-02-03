using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using D3Helper.A_Tools;
using D3Helper.A_Collection;
using D3Helper.A_Enums;

namespace D3Helper.A_Handler.EventHandler
{
    public class Pylon
    {
        public Pylon(PylonName name, LevelArea levelArea)
        {
            this.Name = name;
            this.LevelArea = levelArea;
        }
        public PylonName Name { get; set; }
        public LevelArea LevelArea { get; set; }
    }
    class EventHandler
    {
        public static void handleEvents()
        {
            try
            {
                start_AutoGamble();
                start_GearSwapSelection();
            }
            catch (Exception e)
            {
                A_Handler.Log.ExceptionLogEntry newEntry = new A_Handler.Log.ExceptionLogEntry(e, DateTime.Now, A_Enums.ExceptionThread.Handler);

                lock (A_Handler.Log.Exception.ExceptionLog) A_Handler.Log.Exception.ExceptionLog.Add(newEntry);
            }
        }
        private static void start_AutoGamble()
        {
            try
            {
                if (Properties.Settings.Default.AutoGambleBool)
                {
                    if (A_Collection.Me.HeroStates.isInTown)
                    {
                        A_Handler.AutoGamble.AutoGamble.start_Gamble();
                    }
                }
            }
            catch (Exception e)
            {
                A_Handler.Log.ExceptionLogEntry newEntry = new A_Handler.Log.ExceptionLogEntry(e, DateTime.Now, A_Enums.ExceptionThread.Handler);

                lock (A_Handler.Log.Exception.ExceptionLog) A_Handler.Log.Exception.ExceptionLog.Add(newEntry);
            }
        }
        private static void start_GearSwapSelection()
        {
            try
            {
                if(A_Collection.Me.GearSwap.editModeEnabled)
                {
                    A_Handler.GearSwap.GearSwap.SelectionMode();
                }
            }
            catch (Exception e)
            {
                A_Handler.Log.ExceptionLogEntry newEntry = new A_Handler.Log.ExceptionLogEntry(e, DateTime.Now, A_Enums.ExceptionThread.Handler);

                lock (A_Handler.Log.Exception.ExceptionLog) A_Handler.Log.Exception.ExceptionLog.Add(newEntry);
            }
        }
    }
}
