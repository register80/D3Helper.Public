using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Enigma.D3;

namespace D3Helper.A_Collector
{
    class IC_Preferences
    {
        private static DateTime _nextCollect = DateTime.Now;
        private const int _intervallCollects = 1000; // Collect every x msec

        public static void Collect()
        {
            try
            {
                if (DateTime.Now >= _nextCollect)
                {
                    
                    get_ForceMove();
                    get_ForceStandStill();
                    get_ToggleInventory();
                    get_ToggleParagon();
                    get_ActionBarSkills();
                    get_Potion();
                    get_Townportal();
                    get_SkillsWindow();
                    get_CloseAllWindows();

                    _nextCollect = DateTime.Now.AddMilliseconds(_intervallCollects);
                    
                }
            }
            catch (Exception e)
            {
                A_Handler.Log.ExceptionLogEntry newEntry = new A_Handler.Log.ExceptionLogEntry(e, DateTime.Now,
                    A_Enums.ExceptionThread.ICollector);

                lock (A_Handler.Log.Exception.ExceptionLog) A_Handler.Log.Exception.ExceptionLog.Add(newEntry);
            }
        }
        private static void get_ForceMove()
        {
            try
            {
                A_Collection.Preferences.Hotkeys.Key1_ForceMove = ((Enigma.D3.Enums.Key)Engine.Current.HotkeyPreferences.x430_Key1_ForceMove);
                A_Collection.Preferences.Hotkeys.ModKey1_ForceMove = ((Enigma.D3.Enums.ModKey)Engine.Current.HotkeyPreferences.x434_ModKey1_ForceMove);
                A_Collection.Preferences.Hotkeys.Key2_ForceMove = ((Enigma.D3.Enums.Key)Engine.Current.HotkeyPreferences.x438_Key2_ForceMove);
                A_Collection.Preferences.Hotkeys.ModKey2_ForceMove = ((Enigma.D3.Enums.ModKey)Engine.Current.HotkeyPreferences.x43C_ModKey2_ForceMove);

            }
            catch (Exception e)
            {
                A_Handler.Log.ExceptionLogEntry newEntry = new A_Handler.Log.ExceptionLogEntry(e, DateTime.Now, A_Enums.ExceptionThread.ICollector);

                lock (A_Handler.Log.Exception.ExceptionLog) A_Handler.Log.Exception.ExceptionLog.Add(newEntry);
            }
        }
        private static void get_Potion()
        {
            try
            {
                A_Collection.Preferences.Hotkeys.Key1_Potion = ((Enigma.D3.Enums.Key)Engine.Current.HotkeyPreferences.x260_Key1_PotionButton);
                A_Collection.Preferences.Hotkeys.ModKey1_Potion = ((Enigma.D3.Enums.ModKey)Engine.Current.HotkeyPreferences.x264_ModKey1_PotionButton);
                A_Collection.Preferences.Hotkeys.Key2_Potion = ((Enigma.D3.Enums.Key)Engine.Current.HotkeyPreferences.x268_Key2_PotionButton);
                A_Collection.Preferences.Hotkeys.ModKey2_Potion = ((Enigma.D3.Enums.ModKey)Engine.Current.HotkeyPreferences.x26C_ModKey2_PotionButton);

            }
            catch (Exception e)
            {
                A_Handler.Log.ExceptionLogEntry newEntry = new A_Handler.Log.ExceptionLogEntry(e, DateTime.Now, A_Enums.ExceptionThread.ICollector);

                lock (A_Handler.Log.Exception.ExceptionLog) A_Handler.Log.Exception.ExceptionLog.Add(newEntry);
            }
        }
        private static void get_Townportal()
        {
            try
            {
                A_Collection.Preferences.Hotkeys.Key1_Townportal = ((Enigma.D3.Enums.Key)Engine.Current.HotkeyPreferences.x110_Key1_TownPortalButton);
                A_Collection.Preferences.Hotkeys.ModKey1_Townportal = ((Enigma.D3.Enums.ModKey)Engine.Current.HotkeyPreferences.x114_ModKey1_TownPortalButton);
                A_Collection.Preferences.Hotkeys.Key2_Townportal = ((Enigma.D3.Enums.Key)Engine.Current.HotkeyPreferences.x118_Key2_TownPortalButton);
                A_Collection.Preferences.Hotkeys.ModKey2_Townportal = ((Enigma.D3.Enums.ModKey)Engine.Current.HotkeyPreferences.x11C_ModKey2_TownPortalButton);

            }
            catch (Exception e)
            {
                A_Handler.Log.ExceptionLogEntry newEntry = new A_Handler.Log.ExceptionLogEntry(e, DateTime.Now, A_Enums.ExceptionThread.ICollector);

                lock (A_Handler.Log.Exception.ExceptionLog) A_Handler.Log.Exception.ExceptionLog.Add(newEntry);
            }
        }
        private static void get_SkillsWindow()
        {
            try
            {
                A_Collection.Preferences.Hotkeys.Key1_SkillsWindow = ((Enigma.D3.Enums.Key)Engine.Current.HotkeyPreferences.x20_Key1_ToggleSkillMenu);
                A_Collection.Preferences.Hotkeys.ModKey1_SkillsWindow = ((Enigma.D3.Enums.ModKey)Engine.Current.HotkeyPreferences.x24_ModKey1_ToggleSkillMenu);
                A_Collection.Preferences.Hotkeys.Key2_SkillsWindow = ((Enigma.D3.Enums.Key)Engine.Current.HotkeyPreferences.x28_Key2_ToggleSkillMenu);
                A_Collection.Preferences.Hotkeys.ModKey2_SkillsWindow = ((Enigma.D3.Enums.ModKey)Engine.Current.HotkeyPreferences.x2C_ModKey2_ToggleSkillMenu);

            }
            catch (Exception e)
            {
                A_Handler.Log.ExceptionLogEntry newEntry = new A_Handler.Log.ExceptionLogEntry(e, DateTime.Now, A_Enums.ExceptionThread.ICollector);

                lock (A_Handler.Log.Exception.ExceptionLog) A_Handler.Log.Exception.ExceptionLog.Add(newEntry);
            }
        }
        private static void get_CloseAllWindows()
        {
            try
            {
                A_Collection.Preferences.Hotkeys.Key1_CloseAllWindows = ((Enigma.D3.Enums.Key)Engine.Current.HotkeyPreferences.x120_Key1_CloseAllOpenWindows);
                A_Collection.Preferences.Hotkeys.ModKey1_CloseAllWindows = ((Enigma.D3.Enums.ModKey)Engine.Current.HotkeyPreferences.x124_ModKey1_CloseAllOpenWindows);
                A_Collection.Preferences.Hotkeys.Key2_CloseAllWindows = ((Enigma.D3.Enums.Key)Engine.Current.HotkeyPreferences.x128_Key2_CloseAllOpenWindows);
                A_Collection.Preferences.Hotkeys.ModKey2_CloseAllWindows = ((Enigma.D3.Enums.ModKey)Engine.Current.HotkeyPreferences.x12C_ModKey2_CloseAllOpenWindows);

            }
            catch (Exception e)
            {
                A_Handler.Log.ExceptionLogEntry newEntry = new A_Handler.Log.ExceptionLogEntry(e, DateTime.Now, A_Enums.ExceptionThread.ICollector);

                lock (A_Handler.Log.Exception.ExceptionLog) A_Handler.Log.Exception.ExceptionLog.Add(newEntry);
            }
        }
        private static void get_ForceStandStill()
        {
            try
            {
                A_Collection.Preferences.Hotkeys.Key1_ForceStandStill = ((Enigma.D3.Enums.Key)Engine.Current.HotkeyPreferences.x3B0_Key1_ForceStandStill);
                A_Collection.Preferences.Hotkeys.ModKey1_ForceStandStill = ((Enigma.D3.Enums.ModKey)Engine.Current.HotkeyPreferences.x3B4_ModKey1_ForceStandStill);

                A_Collection.Preferences.Hotkeys.Key2_ForceStandStill = ((Enigma.D3.Enums.Key)Engine.Current.HotkeyPreferences.x3B8_Key2_ForceStandStill);
                A_Collection.Preferences.Hotkeys.ModKey2_ForceStandStill = ((Enigma.D3.Enums.ModKey)Engine.Current.HotkeyPreferences.x3BC_ModKey2_ForceStandStill);

            }
            catch (Exception e)
            {
                A_Handler.Log.ExceptionLogEntry newEntry = new A_Handler.Log.ExceptionLogEntry(e, DateTime.Now, A_Enums.ExceptionThread.ICollector);

                lock (A_Handler.Log.Exception.ExceptionLog) A_Handler.Log.Exception.ExceptionLog.Add(newEntry);
            }
        }
        private static void get_ToggleInventory()
        {
            try
            {
                A_Collection.Preferences.Hotkeys.Key1_OpenInventory = ((Enigma.D3.Enums.Key)Engine.Current.HotkeyPreferences.x0_Key1_ToggleInventoryMenu);
                A_Collection.Preferences.Hotkeys.ModKey1_OpenInventory = ((Enigma.D3.Enums.ModKey)Engine.Current.HotkeyPreferences.x4_ModKey1_ToggleInventoryMenu);
                A_Collection.Preferences.Hotkeys.Key2_OpenInventory = ((Enigma.D3.Enums.Key)Engine.Current.HotkeyPreferences.x8_Key2_ToggleInventoryMenu);
                A_Collection.Preferences.Hotkeys.ModKey2_OpenInventory = ((Enigma.D3.Enums.ModKey)Engine.Current.HotkeyPreferences.xC_ModKey2_ToggleInventoryMenu);

            }
            catch (Exception e)
            {
                A_Handler.Log.ExceptionLogEntry newEntry = new A_Handler.Log.ExceptionLogEntry(e, DateTime.Now, A_Enums.ExceptionThread.ICollector);

                lock (A_Handler.Log.Exception.ExceptionLog) A_Handler.Log.Exception.ExceptionLog.Add(newEntry);
            }
        }
        private static void get_ToggleParagon()
        {
            try
            {
                A_Collection.Preferences.Hotkeys.Key1_OpenParagon = ((Enigma.D3.Enums.Key)Engine.Current.HotkeyPreferences.x440_Key1_Paragon);
                A_Collection.Preferences.Hotkeys.ModKey1_OpenParagon = ((Enigma.D3.Enums.ModKey)Engine.Current.HotkeyPreferences.x444_ModKey1_Paragon);
                A_Collection.Preferences.Hotkeys.Key2_OpenParagon = ((Enigma.D3.Enums.Key)Engine.Current.HotkeyPreferences.x448_Key2_Paragon);
                A_Collection.Preferences.Hotkeys.ModKey2_OpenParagon = ((Enigma.D3.Enums.ModKey)Engine.Current.HotkeyPreferences.x44C_ModKey2_Paragon);
            }
            catch (Exception e)
            {
                A_Handler.Log.ExceptionLogEntry newEntry = new A_Handler.Log.ExceptionLogEntry(e, DateTime.Now, A_Enums.ExceptionThread.ICollector);

                lock (A_Handler.Log.Exception.ExceptionLog) A_Handler.Log.Exception.ExceptionLog.Add(newEntry);
            }
        }
        private static void get_ActionBarSkills()
        {
            try
            {
                A_Collection.Preferences.Hotkeys.Key1_ActionBarSkill1 = ((Enigma.D3.Enums.Key)Engine.Current.HotkeyPreferences.x220_Key1_ActionBarSkill1);
                A_Collection.Preferences.Hotkeys.ModKey1_ActionBarSkill1 = ((Enigma.D3.Enums.ModKey)Engine.Current.HotkeyPreferences.x224_ModKey1_ActionBarSkill1);

                A_Collection.Preferences.Hotkeys.Key1_ActionBarSkill2 = ((Enigma.D3.Enums.Key)Engine.Current.HotkeyPreferences.x230_Key1_ActionBarSkill2);
                A_Collection.Preferences.Hotkeys.ModKey1_ActionBarSkill2 = ((Enigma.D3.Enums.ModKey)Engine.Current.HotkeyPreferences.x234_ModKey1_ActionBarSkill2);

                A_Collection.Preferences.Hotkeys.Key1_ActionBarSkill3 = ((Enigma.D3.Enums.Key)Engine.Current.HotkeyPreferences.x240_Key1_ActionBarSkill3);
                A_Collection.Preferences.Hotkeys.ModKey1_ActionBarSkill3 = ((Enigma.D3.Enums.ModKey)Engine.Current.HotkeyPreferences.x244_ModKey1_ActionBarSkill3);

                A_Collection.Preferences.Hotkeys.Key1_ActionBarSkill4 = ((Enigma.D3.Enums.Key)Engine.Current.HotkeyPreferences.x250_Key1_ActionBarSkill4);
                A_Collection.Preferences.Hotkeys.ModKey1_ActionBarSkill4 = ((Enigma.D3.Enums.ModKey)Engine.Current.HotkeyPreferences.x254_ModKey1_ActionBarSkill4);

                A_Collection.Preferences.Hotkeys.Key2_ActionBarSkill1 = ((Enigma.D3.Enums.Key)Engine.Current.HotkeyPreferences.x228_Key2_ActionBarSkill1);
                A_Collection.Preferences.Hotkeys.ModKey2_ActionBarSkill1 = ((Enigma.D3.Enums.ModKey)Engine.Current.HotkeyPreferences.x22C_ModKey2_ActionBarSkill1);

                A_Collection.Preferences.Hotkeys.Key2_ActionBarSkill2 = ((Enigma.D3.Enums.Key)Engine.Current.HotkeyPreferences.x238_Key2_ActionBarSkill2);
                A_Collection.Preferences.Hotkeys.ModKey2_ActionBarSkill2 = ((Enigma.D3.Enums.ModKey)Engine.Current.HotkeyPreferences.x23C_ModKey2_ActionBarSkill2);

                A_Collection.Preferences.Hotkeys.Key2_ActionBarSkill3 = ((Enigma.D3.Enums.Key)Engine.Current.HotkeyPreferences.x248_Key2_ActionBarSkill3);
                A_Collection.Preferences.Hotkeys.ModKey2_ActionBarSkill3 = ((Enigma.D3.Enums.ModKey)Engine.Current.HotkeyPreferences.x24C_ModKey2_ActionBarSkill3);

                A_Collection.Preferences.Hotkeys.Key2_ActionBarSkill4 = ((Enigma.D3.Enums.Key)Engine.Current.HotkeyPreferences.x258_Key2_ActionBarSkill4);
                A_Collection.Preferences.Hotkeys.ModKey2_ActionBarSkill4 = ((Enigma.D3.Enums.ModKey)Engine.Current.HotkeyPreferences.x25C_ModKey2_ActionBarSkill4);

            }
            catch (Exception e)
            {
                A_Handler.Log.ExceptionLogEntry newEntry = new A_Handler.Log.ExceptionLogEntry(e, DateTime.Now, A_Enums.ExceptionThread.ICollector);

                lock (A_Handler.Log.Exception.ExceptionLog) A_Handler.Log.Exception.ExceptionLog.Add(newEntry);
            }
        }
    }
}
