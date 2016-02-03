using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using D3Helper.A_Collection;
using SlimDX.DirectInput;
using SlimDX.DirectWrite;

namespace D3Helper.A_Collector
{
    class H_Keyboard
    {
        private static DateTime lastHotkeyProcess = new DateTime();

        public static void Collect()
        {
            try
            {
                UpdateHotkeyList();
                processPressedKeys();
                ResetLastProcessedHotkey();
                get_PressedIngameKeys();
            }
            catch (Exception e)
            {
                A_Handler.Log.ExceptionLogEntry newEntry = new A_Handler.Log.ExceptionLogEntry(e, DateTime.Now, A_Enums.ExceptionThread.ECollector);

                lock (A_Handler.Log.Exception.ExceptionLog) A_Handler.Log.Exception.ExceptionLog.Add(newEntry);
            }
        }
        private static void UpdateHotkeyList()
        {
            try
            {
                Hotkey slot1 = get_HotkeyFromSettingsString(Properties.Settings.Default.HotkeySlot1);
                Hotkey slot2 = get_HotkeyFromSettingsString(Properties.Settings.Default.HotkeySlot2);
                Hotkey slot3 = get_HotkeyFromSettingsString(Properties.Settings.Default.HotkeySlot3);
                Hotkey slot4 = get_HotkeyFromSettingsString(Properties.Settings.Default.HotkeySlot4);
                Hotkey slotrmb = get_HotkeyFromSettingsString(Properties.Settings.Default.HotkeySlotRmb);
                Hotkey slotlmb = get_HotkeyFromSettingsString(Properties.Settings.Default.HotkeySlotLmb);
                Hotkey editmode = get_HotkeyFromSettingsString(Properties.Settings.Default.HotkeyEditMode);
                Hotkey swap1 = get_HotkeyFromSettingsString(Properties.Settings.Default.HotkeyGearSwap1);
                Hotkey swap2 = get_HotkeyFromSettingsString(Properties.Settings.Default.HotkeyGearSwap2);
                Hotkey swap3 = get_HotkeyFromSettingsString(Properties.Settings.Default.HotkeyGearSwap3);
                Hotkey swap4 = get_HotkeyFromSettingsString(Properties.Settings.Default.HotkeyGearSwap4);
                Hotkey paragonpoints1 = get_HotkeyFromSettingsString(Properties.Settings.Default.HotkeyParagonPoints1);
                Hotkey paragonpoints2 = get_HotkeyFromSettingsString(Properties.Settings.Default.HotkeyParagonPoints2);
                Hotkey paragonpoints3 = get_HotkeyFromSettingsString(Properties.Settings.Default.HotkeyParagonPoints3);
                Hotkey paragonpoints4 = get_HotkeyFromSettingsString(Properties.Settings.Default.HotkeyParagonPoints4);
                Hotkey autogamble = get_HotkeyFromSettingsString(Properties.Settings.Default.HotkeyAutoGamble);
                Hotkey skillbuild1 = get_HotkeyFromSettingsString(Properties.Settings.Default.HotkeySkillBuild1);
                Hotkey skillbuild2 = get_HotkeyFromSettingsString(Properties.Settings.Default.HotkeySkillBuild2);
                Hotkey skillbuild3 = get_HotkeyFromSettingsString(Properties.Settings.Default.HotkeySkillBuild3);
                Hotkey skillbuild4 = get_HotkeyFromSettingsString(Properties.Settings.Default.HotkeySkillBuild4);
                Hotkey autopick = get_HotkeyFromSettingsString(Properties.Settings.Default.HotkeyAutoPick);
                Hotkey autocube_upgraderare = get_HotkeyFromSettingsString(Properties.Settings.Default.HotkeyAutoCube_UpgradeRare);

                Dictionary<Hotkey, string> hotkeys = new Dictionary<Hotkey, string>();


                hotkeys.Add(slot1, "slot1");
                hotkeys.Add(slot2, "slot2");
                hotkeys.Add(slot3, "slot3");
                hotkeys.Add(slot4, "slot4");
                hotkeys.Add(slotrmb, "slotrmb");
                hotkeys.Add(slotlmb, "slotlmb");
                hotkeys.Add(editmode, "editmode");
                hotkeys.Add(swap1, "swap1");
                hotkeys.Add(swap2, "swap2");
                hotkeys.Add(swap3, "swap3");
                hotkeys.Add(swap4, "swap4");
                hotkeys.Add(paragonpoints1, "paragonpoints1");
                hotkeys.Add(paragonpoints2, "paragonpoints2");
                hotkeys.Add(paragonpoints3, "paragonpoints3");
                hotkeys.Add(paragonpoints4, "paragonpoints4");
                hotkeys.Add(autogamble, "autogamble");
                hotkeys.Add(skillbuild1, "skillbuild1");
                hotkeys.Add(skillbuild2, "skillbuild2");
                hotkeys.Add(skillbuild3, "skillbuild3");
                hotkeys.Add(skillbuild4, "skillbuild4");
                hotkeys.Add(autopick, "autopick");
                hotkeys.Add(autocube_upgraderare, "autocube_upgraderare");

                A_Collection.Hotkeys.D3Helper_Hotkeys = hotkeys;
            }
            catch (Exception e)
            {
                A_Handler.Log.ExceptionLogEntry newEntry = new A_Handler.Log.ExceptionLogEntry(e, DateTime.Now, A_Enums.ExceptionThread.ECollector);

                lock (A_Handler.Log.Exception.ExceptionLog) A_Handler.Log.Exception.ExceptionLog.Add(newEntry);
            }
        }

        public static Hotkey get_HotkeyFromSettingsString(string HotkeyString)
        {
            try
            {
                if(HotkeyString == String.Empty)
                    return new Hotkey(Key.Unknown, new List<Key>());

                var Split = HotkeyString.Split('+');

                Key key = Key.Unknown;
                List<Key> modifiers = new List<Key>();

                for (int i = 0; i < Split.Length; i++)
                {
                    
                    if (i == Split.Length -1)
                        if(!Enum.TryParse<SlimDX.DirectInput.Key>(Split[i], out key))
                            break;
                    
                    if(Split[i].Contains("CTRL"))
                        modifiers.Add(Key.LeftControl);
                    if(Split[i].Contains("ALT"))
                        modifiers.Add(Key.LeftAlt);
                    if(Split[i].Contains("SHIFT"))
                        modifiers.Add(Key.LeftShift);
                }

                return new Hotkey(key, modifiers);
            }
            catch (Exception)
            {
                return new Hotkey(SlimDX.DirectInput.Key.Unknown, new List<Key>());
                throw;
            }
        }
        private static void processPressedKeys()
        {
            try
            {
                if (A_Collection.D3Client.Window.isForeground && !A_Collection.D3UI.isChatting)
                {
                    if (Window_Main.keyboard == null)
                        return;

                    KeyboardState deviceState = Window_Main.keyboard.GetCurrentState();


                    lock (A_Collection.Hotkeys._PressedKeys)
                        A_Collection.Hotkeys._PressedKeys = deviceState.PressedKeys.ToList();

                   
                    foreach (var hotkey in A_Collection.Hotkeys.D3Helper_Hotkeys)
                    {
                        if (hotkey.Key.Key == Key.Unknown)
                        {
                            continue;
                        }

                        if (A_Collection.Hotkeys.lastprocessedHotkey != hotkey.Key.Key)
                        {
                            bool isPressed = true;

                            foreach (var modifier in hotkey.Key.Modifiers)
                            {
                                if (!deviceState.IsPressed(modifier))
                                    isPressed = false;
                            }

                            if (!deviceState.IsPressed(hotkey.Key.Key))
                                isPressed = false;

                            if(!isPressed)
                                continue;

                            switch (hotkey.Value)
                            {

                                case "slot1":
                                    A_Tools.T_ExternalFile.AutoCastOverrides.ChangeOverrides(0);
                                    break;
                                case "slot2":
                                    A_Tools.T_ExternalFile.AutoCastOverrides.ChangeOverrides(1);
                                    break;
                                case "slot3":
                                    A_Tools.T_ExternalFile.AutoCastOverrides.ChangeOverrides(2);
                                    break;
                                case "slot4":
                                    A_Tools.T_ExternalFile.AutoCastOverrides.ChangeOverrides(3);
                                    break;
                                case "slotrmb":
                                    A_Tools.T_ExternalFile.AutoCastOverrides.ChangeOverrides(4);
                                    break;
                                case "slotlmb":
                                    A_Tools.T_ExternalFile.AutoCastOverrides.ChangeOverrides(5);
                                    break;
                                case "editmode":
                                    if (A_Collection.Me.GearSwap.editModeEnabled)
                                    {
                                        A_Collection.Me.GearSwap.editModeEnabled = false;
                                    }
                                    else
                                    {
                                        A_Collection.Me.GearSwap.editModeEnabled = true;
                                    }
                                    break;
                                case "swap1":
                                    A_Collection.Me.GearSwap.Selected_SwapId = 1;

                                    if (!A_Collection.Me.GearSwap.editModeEnabled && !A_Collection.D3UI.isChatting)
                                    {
                                        A_Handler.GearSwap.GearSwap.tryGearSwap();
                                    }
                                    break;
                                case "swap2":
                                    A_Collection.Me.GearSwap.Selected_SwapId = 2;

                                    if (!A_Collection.Me.GearSwap.editModeEnabled && !A_Collection.D3UI.isChatting)
                                    {
                                        A_Handler.GearSwap.GearSwap.tryGearSwap();
                                    }
                                    break;
                                case "swap3":
                                    A_Collection.Me.GearSwap.Selected_SwapId = 3;

                                    if (!A_Collection.Me.GearSwap.editModeEnabled && !A_Collection.D3UI.isChatting)
                                    {
                                        A_Handler.GearSwap.GearSwap.tryGearSwap();
                                    }
                                    break;
                                case "swap4":
                                    A_Collection.Me.GearSwap.Selected_SwapId = 4;

                                    if (!A_Collection.Me.GearSwap.editModeEnabled && !A_Collection.D3UI.isChatting)
                                    {
                                        A_Handler.GearSwap.GearSwap.tryGearSwap();
                                    }
                                    break;
                                case "paragonpoints1":
                                    if (!A_Collection.Me.ParagonPointSpender.Is_SpendingPoints)
                                    {
                                        A_Collection.Me.ParagonPointSpender.SelectedParagonPoints_Setup = 1;

                                        A_Handler.ParagonPointSpender.ParagonPointSpender.Set_ParagonPoints();
                                    }
                                    break;
                                case "paragonpoints2":
                                    if (!A_Collection.Me.ParagonPointSpender.Is_SpendingPoints)
                                    {
                                        A_Collection.Me.ParagonPointSpender.SelectedParagonPoints_Setup = 2;

                                        A_Handler.ParagonPointSpender.ParagonPointSpender.Set_ParagonPoints();
                                    }
                                    break;
                                case "paragonpoints3":
                                    if (!A_Collection.Me.ParagonPointSpender.Is_SpendingPoints)
                                    {
                                        A_Collection.Me.ParagonPointSpender.SelectedParagonPoints_Setup = 3;

                                        A_Handler.ParagonPointSpender.ParagonPointSpender.Set_ParagonPoints();
                                    }
                                    break;
                                case "paragonpoints4":
                                    if (!A_Collection.Me.ParagonPointSpender.Is_SpendingPoints)
                                    {
                                        A_Collection.Me.ParagonPointSpender.SelectedParagonPoints_Setup = 4;

                                        A_Handler.ParagonPointSpender.ParagonPointSpender.Set_ParagonPoints();
                                    }
                                    break;
                                case "autogamble":
                                    Properties.Settings.Default.AutoGambleBool =
                                        !Properties.Settings.Default.AutoGambleBool;
                                    Properties.Settings.Default.Save();
                                    break;

                                case "skillbuild1":
                                    if (!A_Collection.Me.SkillBuilds.Is_SwapingBuild)
                                    {
                                        A_Collection.Me.SkillBuilds.SelectedSkillBuild = 0;

                                        A_Handler.SkillBuildSwap.SkillBuildSwap.DoSwap();
                                    }
                                    break;

                                case "skillbuild2":
                                    if (!A_Collection.Me.SkillBuilds.Is_SwapingBuild)
                                    {
                                        A_Collection.Me.SkillBuilds.SelectedSkillBuild = 1;

                                        A_Handler.SkillBuildSwap.SkillBuildSwap.DoSwap();
                                    }
                                    break;

                                case "skillbuild3":
                                    if (!A_Collection.Me.SkillBuilds.Is_SwapingBuild)
                                    {
                                        A_Collection.Me.SkillBuilds.SelectedSkillBuild = 2;

                                        A_Handler.SkillBuildSwap.SkillBuildSwap.DoSwap();
                                    }
                                    break;

                                case "skillbuild4":
                                    if (!A_Collection.Me.SkillBuilds.Is_SwapingBuild)
                                    {
                                        A_Collection.Me.SkillBuilds.SelectedSkillBuild = 3;

                                        A_Handler.SkillBuildSwap.SkillBuildSwap.DoSwap();
                                    }
                                    break;

                                case "autopick":
                                    if (!A_Handler.AutoPick.AutoPick.IsPicking)
                                    {
                                        A_Handler.AutoPick.AutoPick.DoPickup();
                                    }
                                    break;

                                case "autocube_upgraderare":
                                    if (!A_Handler.AutoCube.UpgradeRare.IsUpgrading_Rare)
                                    {
                                        A_Handler.AutoCube.UpgradeRare.DoUpgrade();
                                    }
                                    break;

                            }

                            A_Collection.Hotkeys.lastprocessedHotkey = hotkey.Key.Key;
                            lastHotkeyProcess = DateTime.Now;
                        }


                    }

                }
            }
            catch (Exception e)
            {
                A_Handler.Log.ExceptionLogEntry newEntry = new A_Handler.Log.ExceptionLogEntry(e, DateTime.Now, A_Enums.ExceptionThread.ECollector);

                lock (A_Handler.Log.Exception.ExceptionLog) A_Handler.Log.Exception.ExceptionLog.Add(newEntry);
            }
        }
        private static void ResetLastProcessedHotkey()
        {
            try
            {
                if(lastHotkeyProcess.AddMilliseconds(300) <= DateTime.Now)
                {
                    A_Collection.Hotkeys.lastprocessedHotkey = new Key();
                }
            }
            catch (Exception e)
            {
                A_Handler.Log.ExceptionLogEntry newEntry = new A_Handler.Log.ExceptionLogEntry(e, DateTime.Now, A_Enums.ExceptionThread.ECollector);

                lock (A_Handler.Log.Exception.ExceptionLog) A_Handler.Log.Exception.ExceptionLog.Add(newEntry);
            }
        }
        private static void get_PressedIngameKeys()
        {
            try
            {
                A_Collection.Hotkeys.IngameKeys.IsForceStandStill = 
                    A_Collection.Hotkeys._PressedKeys.Contains(A_Tools.InputSimulator.IS_Keyboard.convert_KeyToSlimDxKey(A_Collection.Preferences.Hotkeys.Key1_ForceStandStill)) ||
                    A_Collection.Hotkeys._PressedKeys.Contains(A_Tools.InputSimulator.IS_Keyboard.convert_KeyToSlimDxKey(A_Collection.Preferences.Hotkeys.Key2_ForceStandStill));

                A_Collection.Hotkeys.IngameKeys.IsTownPortal =
                    A_Collection.Hotkeys._PressedKeys.Contains(A_Tools.InputSimulator.IS_Keyboard.convert_KeyToSlimDxKey(A_Collection.Preferences.Hotkeys.Key1_Townportal)) ||
                    A_Collection.Hotkeys._PressedKeys.Contains(A_Tools.InputSimulator.IS_Keyboard.convert_KeyToSlimDxKey(A_Collection.Preferences.Hotkeys.Key2_Townportal));
            }
            catch (Exception e)
            {
                A_Handler.Log.ExceptionLogEntry newEntry = new A_Handler.Log.ExceptionLogEntry(e, DateTime.Now,
                    A_Enums.ExceptionThread.ECollector);

                lock (A_Handler.Log.Exception.ExceptionLog) A_Handler.Log.Exception.ExceptionLog.Add(newEntry);
            }
        }
    }
}
