using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using D3Helper.A_Collector;
using D3Helper.A_Enums;
using D3Helper.A_Handler.SkillBuildSwap;
using D3Helper.A_Tools;
using Key = SlimDX.DirectInput.Key;
using KeyEventArgs = System.Windows.Forms.KeyEventArgs;

namespace D3Helper
{
    public partial class Window_Settings : Form
    {
        public static DataGridView Setup1 = new DataGridView();
        public static DataGridView Setup2 = new DataGridView();
        public static DataGridView Setup3 = new DataGridView();
        public static DataGridView Setup4 = new DataGridView();

        public static Window_Settings _this = null;

        public Window_Settings()
        {
            

            InitializeComponent();

            this.FormClosed += FormClose;
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.SupportsTransparentBackColor, true);
           

        }
        
        private void FormClose(Object sender, FormClosedEventArgs e)
        {
           
            _this = null;
        }
       
       
       
        private void Settings_Load(object sender, EventArgs e)
        {
            _this = this;

            typeof(Panel).InvokeMember("DoubleBuffered",
    BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
    null, Panel_SkillBuilds_View, new object[] { true });

            Load_Setups();

            TabControl_ParagonPoints.BackColor = Color.Transparent;

            tabControl1.SelectedIndexChanged += TabControl1_SelectedIndexChanged;
            

            this.Top = Properties.Settings.Default.D3Helper_MainForm_StartPosition.Y;
            this.Left = Properties.Settings.Default.D3Helper_MainForm_StartPosition.X;

            
            // -----------


            
            this.tb_assignedSkill1.ReadOnly = true;
            this.tb_assignedSkill2.ReadOnly = true;
            this.tb_assignedSkill3.ReadOnly = true;
            this.tb_assignedSkill4.ReadOnly = true;
            this.tb_assignedSkillRMB.ReadOnly = true;
            this.tb_assignedSkillLMB.ReadOnly = true;
            this.tb_assignedEditMode.ReadOnly = true;
            this.tb_assignedGearSwap1.ReadOnly = true;
            this.tb_assignedGearSwap2.ReadOnly = true;
            this.tb_assignedGearSwap3.ReadOnly = true;
            this.tb_assignedGearSwap4.ReadOnly = true;
            this.tb_assignedParagonPoints1.ReadOnly = true;
            this.tb_assignedParagonPoints2.ReadOnly = true;
            this.tb_assignedParagonPoints3.ReadOnly = true;
            this.tb_assignedParagonPoints4.ReadOnly = true;
            this.tb_assignedAutoGambleHotkey.ReadOnly = true;
            this.tb_assignedSkillBuild1.ReadOnly = true;
            this.tb_assignedSkillBuild2.ReadOnly = true;
            this.tb_assignedSkillBuild3.ReadOnly = true;
            this.tb_assignedSkillBuild4.ReadOnly = true;
            tb_assignedAutoPick.ReadOnly = true;
            tb_assignedAutoCube_UpgradeRare.ReadOnly = true;

            this.tb_assignedSkill1.KeyDown += tb_assignedSkill1_KeyDown;
            this.tb_assignedSkill2.KeyDown += tb_assignedSkill2_KeyDown;
            this.tb_assignedSkill3.KeyDown += tb_assignedSkill3_KeyDown;
            this.tb_assignedSkill4.KeyDown += tb_assignedSkill4_KeyDown;
            this.tb_assignedSkillRMB.KeyDown += tb_assignedSkillRMB_KeyDown;
            this.tb_assignedSkillLMB.KeyDown += tb_assignedSkillLMB_KeyDown;
            this.tb_assignedEditMode.KeyDown += tb_assignedEditMode_KeyDown;
            this.tb_assignedGearSwap1.KeyDown += tb_assignedGearSwap1_KeyDown;
            this.tb_assignedGearSwap2.KeyDown += tb_assignedGearSwap2_KeyDown;
            this.tb_assignedGearSwap3.KeyDown += tb_assignedGearSwap3_KeyDown;
            this.tb_assignedGearSwap4.KeyDown += tb_assignedGearSwap4_KeyDown;
            this.tb_assignedParagonPoints1.KeyDown += tb_assignedParagonPoints1_KeyDown;
            this.tb_assignedParagonPoints2.KeyDown += tb_assignedParagonPoints2_KeyDown;
            this.tb_assignedParagonPoints3.KeyDown += tb_assignedParagonPoints3_KeyDown;
            this.tb_assignedParagonPoints4.KeyDown += tb_assignedParagonPoints4_KeyDown;
            this.tb_assignedAutoGambleHotkey.KeyDown += Tb_assignedAutoGambleHotkey_KeyDown;
            this.tb_assignedSkillBuild1.KeyDown += Tb_assignedSkillBuild1_KeyDown;
            this.tb_assignedSkillBuild2.KeyDown += Tb_assignedSkillBuild2_KeyDown;
            this.tb_assignedSkillBuild3.KeyDown += Tb_assignedSkillBuild3_KeyDown;
            this.tb_assignedSkillBuild4.KeyDown += Tb_assignedSkillBuild4_KeyDown;
            tb_assignedAutoPick.KeyDown += Tb_assignedAutoPick_KeyDown;
            tb_assignedAutoCube_UpgradeRare.KeyDown += Tb_assignedAutoCube_UpgradeRare_KeyDown;

            this.tb_assignedSkill1.Text = get_HotkeyText(H_Keyboard.get_HotkeyFromSettingsString(Properties.Settings.Default.HotkeySlot1).Key, H_Keyboard.get_HotkeyFromSettingsString(Properties.Settings.Default.HotkeySlot1).Modifiers);
            this.tb_assignedSkill2.Text = get_HotkeyText(H_Keyboard.get_HotkeyFromSettingsString(Properties.Settings.Default.HotkeySlot2).Key, H_Keyboard.get_HotkeyFromSettingsString(Properties.Settings.Default.HotkeySlot2).Modifiers);
            this.tb_assignedSkill3.Text = get_HotkeyText(H_Keyboard.get_HotkeyFromSettingsString(Properties.Settings.Default.HotkeySlot3).Key, H_Keyboard.get_HotkeyFromSettingsString(Properties.Settings.Default.HotkeySlot3).Modifiers);
            this.tb_assignedSkill4.Text = get_HotkeyText(H_Keyboard.get_HotkeyFromSettingsString(Properties.Settings.Default.HotkeySlot4).Key, H_Keyboard.get_HotkeyFromSettingsString(Properties.Settings.Default.HotkeySlot4).Modifiers);
            this.tb_assignedSkillRMB.Text = get_HotkeyText(H_Keyboard.get_HotkeyFromSettingsString(Properties.Settings.Default.HotkeySlotRmb).Key, H_Keyboard.get_HotkeyFromSettingsString(Properties.Settings.Default.HotkeySlotRmb).Modifiers);
            this.tb_assignedSkillLMB.Text = get_HotkeyText(H_Keyboard.get_HotkeyFromSettingsString(Properties.Settings.Default.HotkeySlotLmb).Key, H_Keyboard.get_HotkeyFromSettingsString(Properties.Settings.Default.HotkeySlotLmb).Modifiers);
            this.tb_assignedEditMode.Text = get_HotkeyText(H_Keyboard.get_HotkeyFromSettingsString(Properties.Settings.Default.HotkeyEditMode).Key, H_Keyboard.get_HotkeyFromSettingsString(Properties.Settings.Default.HotkeyEditMode).Modifiers);
            this.tb_assignedGearSwap1.Text = get_HotkeyText(H_Keyboard.get_HotkeyFromSettingsString(Properties.Settings.Default.HotkeyGearSwap1).Key, H_Keyboard.get_HotkeyFromSettingsString(Properties.Settings.Default.HotkeyGearSwap1).Modifiers);
            this.tb_assignedGearSwap2.Text = get_HotkeyText(H_Keyboard.get_HotkeyFromSettingsString(Properties.Settings.Default.HotkeyGearSwap2).Key, H_Keyboard.get_HotkeyFromSettingsString(Properties.Settings.Default.HotkeyGearSwap2).Modifiers);
            this.tb_assignedGearSwap3.Text = get_HotkeyText(H_Keyboard.get_HotkeyFromSettingsString(Properties.Settings.Default.HotkeyGearSwap3).Key, H_Keyboard.get_HotkeyFromSettingsString(Properties.Settings.Default.HotkeyGearSwap3).Modifiers);
            this.tb_assignedGearSwap4.Text = get_HotkeyText(H_Keyboard.get_HotkeyFromSettingsString(Properties.Settings.Default.HotkeyGearSwap4).Key, H_Keyboard.get_HotkeyFromSettingsString(Properties.Settings.Default.HotkeyGearSwap4).Modifiers);
            this.tb_assignedParagonPoints1.Text = get_HotkeyText(H_Keyboard.get_HotkeyFromSettingsString(Properties.Settings.Default.HotkeyParagonPoints1).Key, H_Keyboard.get_HotkeyFromSettingsString(Properties.Settings.Default.HotkeyParagonPoints1).Modifiers);
            this.tb_assignedParagonPoints2.Text = get_HotkeyText(H_Keyboard.get_HotkeyFromSettingsString(Properties.Settings.Default.HotkeyParagonPoints2).Key, H_Keyboard.get_HotkeyFromSettingsString(Properties.Settings.Default.HotkeyParagonPoints2).Modifiers);
            this.tb_assignedParagonPoints3.Text = get_HotkeyText(H_Keyboard.get_HotkeyFromSettingsString(Properties.Settings.Default.HotkeyParagonPoints3).Key, H_Keyboard.get_HotkeyFromSettingsString(Properties.Settings.Default.HotkeyParagonPoints3).Modifiers);
            this.tb_assignedParagonPoints4.Text = get_HotkeyText(H_Keyboard.get_HotkeyFromSettingsString(Properties.Settings.Default.HotkeyParagonPoints4).Key, H_Keyboard.get_HotkeyFromSettingsString(Properties.Settings.Default.HotkeyParagonPoints4).Modifiers);
            this.tb_assignedAutoGambleHotkey.Text = get_HotkeyText(H_Keyboard.get_HotkeyFromSettingsString(Properties.Settings.Default.HotkeyAutoGamble).Key, H_Keyboard.get_HotkeyFromSettingsString(Properties.Settings.Default.HotkeyAutoGamble).Modifiers);
            this.tb_assignedSkillBuild1.Text = get_HotkeyText(H_Keyboard.get_HotkeyFromSettingsString(Properties.Settings.Default.HotkeySkillBuild1).Key, H_Keyboard.get_HotkeyFromSettingsString(Properties.Settings.Default.HotkeySkillBuild1).Modifiers);
            this.tb_assignedSkillBuild2.Text = get_HotkeyText(H_Keyboard.get_HotkeyFromSettingsString(Properties.Settings.Default.HotkeySkillBuild2).Key, H_Keyboard.get_HotkeyFromSettingsString(Properties.Settings.Default.HotkeySkillBuild2).Modifiers);
            this.tb_assignedSkillBuild3.Text = get_HotkeyText(H_Keyboard.get_HotkeyFromSettingsString(Properties.Settings.Default.HotkeySkillBuild3).Key, H_Keyboard.get_HotkeyFromSettingsString(Properties.Settings.Default.HotkeySkillBuild3).Modifiers);
            this.tb_assignedSkillBuild4.Text = get_HotkeyText(H_Keyboard.get_HotkeyFromSettingsString(Properties.Settings.Default.HotkeySkillBuild4).Key, H_Keyboard.get_HotkeyFromSettingsString(Properties.Settings.Default.HotkeySkillBuild4).Modifiers);
            this.tb_assignedAutoPick.Text = get_HotkeyText(H_Keyboard.get_HotkeyFromSettingsString(Properties.Settings.Default.HotkeyAutoPick).Key, H_Keyboard.get_HotkeyFromSettingsString(Properties.Settings.Default.HotkeyAutoPick).Modifiers);
            this.tb_assignedAutoCube_UpgradeRare.Text = get_HotkeyText(H_Keyboard.get_HotkeyFromSettingsString(Properties.Settings.Default.HotkeyAutoCube_UpgradeRare).Key, H_Keyboard.get_HotkeyFromSettingsString(Properties.Settings.Default.HotkeyAutoCube_UpgradeRare).Modifiers);


            this.cb_autopotion.Checked = Properties.Settings.Default.AutoPotionBool;
            this.tb_autopotionhpvalue.Text = Properties.Settings.Default.AutoPotionValue.ToString();
            

            this.cb_fps.Checked = Properties.Settings.Default.overlayfps;
            this.cb_xptracker.Checked = Properties.Settings.Default.overlayxptracker;
            this.cb_skillbuttons.Checked = Properties.Settings.Default.overlayskillbuttons;
            this.cb_riftprogressinrange.Checked = Properties.Settings.Default.overlayriftprogress;

            this.cb_autogamble.Checked = Properties.Settings.Default.AutoGambleBool;
            this.CB_ExtendedLogging.Checked = Properties.Settings.Default.Logger_extendedLog;
            this.cb_skillbuttonsastext.Checked = Properties.Settings.Default.overlayskillbuttonsastext;
            this.cb_conventionelements.Checked = Properties.Settings.Default.overlayconventiondraws;
            this.CB_ApsAndSnapShotAPs.Checked = Properties.Settings.Default.Overlay_APS;

            this.tb_updaterate.Text = Properties.Settings.Default.D3Helper_UpdateRate.ToString();
            this.tb_riftprogressradius.Text = Properties.Settings.Default.riftprogress_radius.ToString();

            this.CB_AutoPick_Gem.Checked = Properties.Settings.Default.AutoPickSettings_Gem;
            this.CB_AutoPick_Material.Checked = Properties.Settings.Default.AutoPickSettings_Material;
            this.CB_AutoPick_Legendary.Checked = Properties.Settings.Default.AutoPickSettings_Legendary;
            this.CB_AutoPick_LegendaryAncient.Checked = Properties.Settings.Default.AutoPickSettings_LegendaryAncient;
            this.CB_AutoPick_GreaterRiftKeystone.Checked = Properties.Settings.Default.AutoPickSettings_GreaterRiftKeystone;
            this.CB_AutoPick_Whites.Checked = Properties.Settings.Default.AutoPickSettings_Whites;
            this.CB_AutoPick_Magics.Checked = Properties.Settings.Default.AutoPickSettings_Magics;
            this.CB_AutoPick_Rares.Checked = Properties.Settings.Default.AutoPickSettings_Rares;
            this.TB_AutoPick_PickupRadius.Text = Properties.Settings.Default.AutoPickSettings_PickupRadius.ToString();

        }

        private void Tb_assignedAutoCube_UpgradeRare_KeyDown(object sender, KeyEventArgs e)
        {
            Keys Key = e.KeyCode;

            SlimDX.DirectInput.Key key = A_Tools.InputSimulator.IS_Keyboard.convert_KeysToKey(Key);

            List<SlimDX.DirectInput.Key> Modifiers = new List<Key>();

            if (e.Alt)
                Modifiers.Add(SlimDX.DirectInput.Key.LeftAlt);
            if (e.Control)
                Modifiers.Add(SlimDX.DirectInput.Key.LeftControl);
            if (e.Shift)
                Modifiers.Add(SlimDX.DirectInput.Key.LeftShift);



            this.tb_assignedAutoCube_UpgradeRare.Text = get_HotkeyText(key, Modifiers);
            Properties.Settings.Default.HotkeyAutoCube_UpgradeRare = get_HotkeyText(key, Modifiers);
            Properties.Settings.Default.Save();
        }

        private void Tb_assignedAutoPick_KeyDown(object sender, KeyEventArgs e)
        {
            Keys Key = e.KeyCode;

            SlimDX.DirectInput.Key key = A_Tools.InputSimulator.IS_Keyboard.convert_KeysToKey(Key);

            List<SlimDX.DirectInput.Key> Modifiers = new List<Key>();

            if (e.Alt)
                Modifiers.Add(SlimDX.DirectInput.Key.LeftAlt);
            if (e.Control)
                Modifiers.Add(SlimDX.DirectInput.Key.LeftControl);
            if (e.Shift)
                Modifiers.Add(SlimDX.DirectInput.Key.LeftShift);



            this.tb_assignedAutoPick.Text = get_HotkeyText(key, Modifiers);
            Properties.Settings.Default.HotkeyAutoPick = get_HotkeyText(key, Modifiers);
            Properties.Settings.Default.Save();
        }

        private void Tb_assignedSkillBuild4_KeyDown(object sender, KeyEventArgs e)
        {
            Keys Key = e.KeyCode;

            SlimDX.DirectInput.Key key = A_Tools.InputSimulator.IS_Keyboard.convert_KeysToKey(Key);

            List<SlimDX.DirectInput.Key> Modifiers = new List<Key>();

            if (e.Alt)
                Modifiers.Add(SlimDX.DirectInput.Key.LeftAlt);
            if (e.Control)
                Modifiers.Add(SlimDX.DirectInput.Key.LeftControl);
            if (e.Shift)
                Modifiers.Add(SlimDX.DirectInput.Key.LeftShift);



            this.tb_assignedSkillBuild4.Text = get_HotkeyText(key, Modifiers);
            Properties.Settings.Default.HotkeySkillBuild4 = get_HotkeyText(key, Modifiers);
            Properties.Settings.Default.Save();
        }

        private void Tb_assignedSkillBuild3_KeyDown(object sender, KeyEventArgs e)
        {
            Keys Key = e.KeyCode;

            SlimDX.DirectInput.Key key = A_Tools.InputSimulator.IS_Keyboard.convert_KeysToKey(Key);

            List<SlimDX.DirectInput.Key> Modifiers = new List<Key>();

            if (e.Alt)
                Modifiers.Add(SlimDX.DirectInput.Key.LeftAlt);
            if (e.Control)
                Modifiers.Add(SlimDX.DirectInput.Key.LeftControl);
            if (e.Shift)
                Modifiers.Add(SlimDX.DirectInput.Key.LeftShift);



            this.tb_assignedSkillBuild3.Text = get_HotkeyText(key, Modifiers);
            Properties.Settings.Default.HotkeySkillBuild3 = get_HotkeyText(key, Modifiers);
            Properties.Settings.Default.Save();
        }

        private void Tb_assignedSkillBuild2_KeyDown(object sender, KeyEventArgs e)
        {
            Keys Key = e.KeyCode;

            SlimDX.DirectInput.Key key = A_Tools.InputSimulator.IS_Keyboard.convert_KeysToKey(Key);

            List<SlimDX.DirectInput.Key> Modifiers = new List<Key>();

            if (e.Alt)
                Modifiers.Add(SlimDX.DirectInput.Key.LeftAlt);
            if (e.Control)
                Modifiers.Add(SlimDX.DirectInput.Key.LeftControl);
            if (e.Shift)
                Modifiers.Add(SlimDX.DirectInput.Key.LeftShift);



            this.tb_assignedSkillBuild2.Text = get_HotkeyText(key, Modifiers);
            Properties.Settings.Default.HotkeySkillBuild2 = get_HotkeyText(key, Modifiers);
            Properties.Settings.Default.Save();
        }

        private void Tb_assignedSkillBuild1_KeyDown(object sender, KeyEventArgs e)
        {
            Keys Key = e.KeyCode;

            SlimDX.DirectInput.Key key = A_Tools.InputSimulator.IS_Keyboard.convert_KeysToKey(Key);

            List<SlimDX.DirectInput.Key> Modifiers = new List<Key>();

            if (e.Alt)
                Modifiers.Add(SlimDX.DirectInput.Key.LeftAlt);
            if (e.Control)
                Modifiers.Add(SlimDX.DirectInput.Key.LeftControl);
            if (e.Shift)
                Modifiers.Add(SlimDX.DirectInput.Key.LeftShift);



            this.tb_assignedSkillBuild1.Text = get_HotkeyText(key, Modifiers);
            Properties.Settings.Default.HotkeySkillBuild1 = get_HotkeyText(key, Modifiers);
            Properties.Settings.Default.Save();
        }

        private void Tb_assignedAutoGambleHotkey_KeyDown(object sender, KeyEventArgs e)
        {
            Keys Key = e.KeyCode;

            SlimDX.DirectInput.Key key = A_Tools.InputSimulator.IS_Keyboard.convert_KeysToKey(Key);

            List<SlimDX.DirectInput.Key> Modifiers = new List<Key>();

            if (e.Alt)
                Modifiers.Add(SlimDX.DirectInput.Key.LeftAlt);
            if (e.Control)
                Modifiers.Add(SlimDX.DirectInput.Key.LeftControl);
            if (e.Shift)
                Modifiers.Add(SlimDX.DirectInput.Key.LeftShift);



            this.tb_assignedAutoGambleHotkey.Text = get_HotkeyText(key, Modifiers);
            Properties.Settings.Default.HotkeyAutoGamble = get_HotkeyText(key, Modifiers);
            Properties.Settings.Default.Save();
        }

        private void TabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
           
            if (tabControl1.SelectedIndex == 4)
            {
                tabControl1.SelectedIndex = 0;
                if (Window_SkillEditor._this == null)
                {
                    Window_SkillEditor SE = new Window_SkillEditor();
                    SE.Show();
                }
                
            }
            if (tabControl1.SelectedIndex == 6)
            {
                SkillBuild_UpdateView();


            }

        }

        void tb_assignedSkillLMB_KeyDown(object sender, KeyEventArgs e)
        {
            Keys Key = e.KeyCode;

            SlimDX.DirectInput.Key key = A_Tools.InputSimulator.IS_Keyboard.convert_KeysToKey(Key);

            List<SlimDX.DirectInput.Key> Modifiers = new List<Key>();

            if (e.Alt)
                Modifiers.Add(SlimDX.DirectInput.Key.LeftAlt);
            if (e.Control)
                Modifiers.Add(SlimDX.DirectInput.Key.LeftControl);
            if (e.Shift)
                Modifiers.Add(SlimDX.DirectInput.Key.LeftShift);

            

                this.tb_assignedSkillLMB.Text = get_HotkeyText(key, Modifiers);
                Properties.Settings.Default.HotkeySlotLmb = get_HotkeyText(key, Modifiers);
                Properties.Settings.Default.Save();
            
        }

        void tb_assignedParagonPoints4_KeyDown(object sender, KeyEventArgs e)
        {
            Keys Key = e.KeyCode;

            SlimDX.DirectInput.Key key = A_Tools.InputSimulator.IS_Keyboard.convert_KeysToKey(Key);

            List<SlimDX.DirectInput.Key> Modifiers = new List<Key>();

            if (e.Alt)
                Modifiers.Add(SlimDX.DirectInput.Key.LeftAlt);
            if (e.Control)
                Modifiers.Add(SlimDX.DirectInput.Key.LeftControl);
            if (e.Shift)
                Modifiers.Add(SlimDX.DirectInput.Key.LeftShift);

            

                this.tb_assignedParagonPoints4.Text = get_HotkeyText(key, Modifiers);
                Properties.Settings.Default.HotkeyParagonPoints4 = get_HotkeyText(key, Modifiers);
                Properties.Settings.Default.Save();
            
        }

        void tb_assignedParagonPoints3_KeyDown(object sender, KeyEventArgs e)
        {
            Keys Key = e.KeyCode;

            SlimDX.DirectInput.Key key = A_Tools.InputSimulator.IS_Keyboard.convert_KeysToKey(Key);

            List<SlimDX.DirectInput.Key> Modifiers = new List<Key>();

            if (e.Alt)
                Modifiers.Add(SlimDX.DirectInput.Key.LeftAlt);
            if (e.Control)
                Modifiers.Add(SlimDX.DirectInput.Key.LeftControl);
            if (e.Shift)
                Modifiers.Add(SlimDX.DirectInput.Key.LeftShift);

           

                this.tb_assignedParagonPoints3.Text = get_HotkeyText(key, Modifiers);
                Properties.Settings.Default.HotkeyParagonPoints3 = get_HotkeyText(key, Modifiers);
                Properties.Settings.Default.Save();
            
        }

        void tb_assignedParagonPoints2_KeyDown(object sender, KeyEventArgs e)
        {
            Keys Key = e.KeyCode;

            SlimDX.DirectInput.Key key = A_Tools.InputSimulator.IS_Keyboard.convert_KeysToKey(Key);

            List<SlimDX.DirectInput.Key> Modifiers = new List<Key>();

            if (e.Alt)
                Modifiers.Add(SlimDX.DirectInput.Key.LeftAlt);
            if (e.Control)
                Modifiers.Add(SlimDX.DirectInput.Key.LeftControl);
            if (e.Shift)
                Modifiers.Add(SlimDX.DirectInput.Key.LeftShift);

            

                this.tb_assignedParagonPoints2.Text = get_HotkeyText(key, Modifiers);
                Properties.Settings.Default.HotkeyParagonPoints2 = get_HotkeyText(key, Modifiers);
                Properties.Settings.Default.Save();
            
        }

        void tb_assignedParagonPoints1_KeyDown(object sender, KeyEventArgs e)
        {
            Keys Key = e.KeyCode;

            SlimDX.DirectInput.Key key = A_Tools.InputSimulator.IS_Keyboard.convert_KeysToKey(Key);

            List<SlimDX.DirectInput.Key> Modifiers = new List<Key>();

            if (e.Alt)
                Modifiers.Add(SlimDX.DirectInput.Key.LeftAlt);
            if (e.Control)
                Modifiers.Add(SlimDX.DirectInput.Key.LeftControl);
            if (e.Shift)
                Modifiers.Add(SlimDX.DirectInput.Key.LeftShift);

            

                this.tb_assignedParagonPoints1.Text = get_HotkeyText(key, Modifiers);
                Properties.Settings.Default.HotkeyParagonPoints1 = get_HotkeyText(key, Modifiers);
                Properties.Settings.Default.Save();
            
        }
        
        void tb_assignedEditMode_KeyDown(object sender, KeyEventArgs e)
        {
            Keys Key = e.KeyCode;
            
            SlimDX.DirectInput.Key key = A_Tools.InputSimulator.IS_Keyboard.convert_KeysToKey(Key);

            List<SlimDX.DirectInput.Key> Modifiers = new List<Key>();

            if(e.Alt)
                Modifiers.Add(SlimDX.DirectInput.Key.LeftAlt);
            if(e.Control)
                Modifiers.Add(SlimDX.DirectInput.Key.LeftControl);
            if(e.Shift)
                Modifiers.Add(SlimDX.DirectInput.Key.LeftShift);
            
            

                this.tb_assignedEditMode.Text = get_HotkeyText(key, Modifiers);
                Properties.Settings.Default.HotkeyEditMode = get_HotkeyText(key,Modifiers);
                Properties.Settings.Default.Save();
            
        }

        
        private string get_HotkeyText(Key key, List<Key> modifiers)
        {
            string text = "";
            
            foreach (var modifier in modifiers)
            {
                if (modifier == Key.LeftControl)
                    text += "CTRL+";
                if (modifier == Key.LeftAlt)
                    text += "ALT+";
                if (modifier == Key.LeftShift)
                    text += "SHIFT+";
            }

            text += key.ToString();

            return text;
        }
        void tb_assignedSkillRMB_KeyDown(object sender, KeyEventArgs e)
        {
            Keys Key = e.KeyCode;

            SlimDX.DirectInput.Key key = A_Tools.InputSimulator.IS_Keyboard.convert_KeysToKey(Key);

            List<SlimDX.DirectInput.Key> Modifiers = new List<Key>();

            if (e.Alt)
                Modifiers.Add(SlimDX.DirectInput.Key.LeftAlt);
            if (e.Control)
                Modifiers.Add(SlimDX.DirectInput.Key.LeftControl);
            if (e.Shift)
                Modifiers.Add(SlimDX.DirectInput.Key.LeftShift);

            

                this.tb_assignedSkillRMB.Text = get_HotkeyText(key, Modifiers);
                Properties.Settings.Default.HotkeySlotRmb = get_HotkeyText(key, Modifiers);
                Properties.Settings.Default.Save();
            
        }

        void tb_assignedGearSwap4_KeyDown(object sender, KeyEventArgs e)
        {
            Keys Key = e.KeyCode;

            SlimDX.DirectInput.Key key = A_Tools.InputSimulator.IS_Keyboard.convert_KeysToKey(Key);

            List<SlimDX.DirectInput.Key> Modifiers = new List<Key>();

            if (e.Alt)
                Modifiers.Add(SlimDX.DirectInput.Key.LeftAlt);
            if (e.Control)
                Modifiers.Add(SlimDX.DirectInput.Key.LeftControl);
            if (e.Shift)
                Modifiers.Add(SlimDX.DirectInput.Key.LeftShift);

            

                this.tb_assignedGearSwap4.Text = get_HotkeyText(key, Modifiers);
                Properties.Settings.Default.HotkeyGearSwap4 = get_HotkeyText(key, Modifiers);
                Properties.Settings.Default.Save();
            
        }

        void tb_assignedGearSwap3_KeyDown(object sender, KeyEventArgs e)
        {
            Keys Key = e.KeyCode;

            SlimDX.DirectInput.Key key = A_Tools.InputSimulator.IS_Keyboard.convert_KeysToKey(Key);

            List<SlimDX.DirectInput.Key> Modifiers = new List<Key>();

            if (e.Alt)
                Modifiers.Add(SlimDX.DirectInput.Key.LeftAlt);
            if (e.Control)
                Modifiers.Add(SlimDX.DirectInput.Key.LeftControl);
            if (e.Shift)
                Modifiers.Add(SlimDX.DirectInput.Key.LeftShift);

            

                this.tb_assignedGearSwap3.Text = get_HotkeyText(key, Modifiers);
                Properties.Settings.Default.HotkeyGearSwap3 = get_HotkeyText(key, Modifiers);
                Properties.Settings.Default.Save();
            
        }

        void tb_assignedGearSwap2_KeyDown(object sender, KeyEventArgs e)
        {
            Keys Key = e.KeyCode;

            SlimDX.DirectInput.Key key = A_Tools.InputSimulator.IS_Keyboard.convert_KeysToKey(Key);

            List<SlimDX.DirectInput.Key> Modifiers = new List<Key>();

            if (e.Alt)
                Modifiers.Add(SlimDX.DirectInput.Key.LeftAlt);
            if (e.Control)
                Modifiers.Add(SlimDX.DirectInput.Key.LeftControl);
            if (e.Shift)
                Modifiers.Add(SlimDX.DirectInput.Key.LeftShift);

            

                this.tb_assignedGearSwap2.Text = get_HotkeyText(key, Modifiers);
                Properties.Settings.Default.HotkeyGearSwap2 = get_HotkeyText(key, Modifiers);
                Properties.Settings.Default.Save();
            
        }

        void tb_assignedGearSwap1_KeyDown(object sender, KeyEventArgs e)
        {
            Keys Key = e.KeyCode;

            SlimDX.DirectInput.Key key = A_Tools.InputSimulator.IS_Keyboard.convert_KeysToKey(Key);

            List<SlimDX.DirectInput.Key> Modifiers = new List<Key>();

            if (e.Alt)
                Modifiers.Add(SlimDX.DirectInput.Key.LeftAlt);
            if (e.Control)
                Modifiers.Add(SlimDX.DirectInput.Key.LeftControl);
            if (e.Shift)
                Modifiers.Add(SlimDX.DirectInput.Key.LeftShift);

            

                this.tb_assignedGearSwap1.Text = get_HotkeyText(key, Modifiers);
                Properties.Settings.Default.HotkeyGearSwap1 = get_HotkeyText(key, Modifiers);
                Properties.Settings.Default.Save();
            
        }

        void tb_assignedSkill4_KeyDown(object sender, KeyEventArgs e)
        {
            Keys Key = e.KeyCode;

            SlimDX.DirectInput.Key key = A_Tools.InputSimulator.IS_Keyboard.convert_KeysToKey(Key);

            List<SlimDX.DirectInput.Key> Modifiers = new List<Key>();

            if (e.Alt)
                Modifiers.Add(SlimDX.DirectInput.Key.LeftAlt);
            if (e.Control)
                Modifiers.Add(SlimDX.DirectInput.Key.LeftControl);
            if (e.Shift)
                Modifiers.Add(SlimDX.DirectInput.Key.LeftShift);

             this.tb_assignedSkill4.Text = get_HotkeyText(key, Modifiers);
                Properties.Settings.Default.HotkeySlot4 = get_HotkeyText(key, Modifiers);
                Properties.Settings.Default.Save();
            }

        void tb_assignedSkill3_KeyDown(object sender, KeyEventArgs e)
        {
            Keys Key = e.KeyCode;

            SlimDX.DirectInput.Key key = A_Tools.InputSimulator.IS_Keyboard.convert_KeysToKey(Key);

            List<SlimDX.DirectInput.Key> Modifiers = new List<Key>();

            if (e.Alt)
                Modifiers.Add(SlimDX.DirectInput.Key.LeftAlt);
            if (e.Control)
                Modifiers.Add(SlimDX.DirectInput.Key.LeftControl);
            if (e.Shift)
                Modifiers.Add(SlimDX.DirectInput.Key.LeftShift);

           
                this.tb_assignedSkill3.Text = get_HotkeyText(key, Modifiers);
                Properties.Settings.Default.HotkeySlot3 = get_HotkeyText(key, Modifiers);
                Properties.Settings.Default.Save();
            
        }

        void tb_assignedSkill2_KeyDown(object sender, KeyEventArgs e)
        {
            Keys Key = e.KeyCode;

            SlimDX.DirectInput.Key key = A_Tools.InputSimulator.IS_Keyboard.convert_KeysToKey(Key);

            List<SlimDX.DirectInput.Key> Modifiers = new List<Key>();

            if (e.Alt)
                Modifiers.Add(SlimDX.DirectInput.Key.LeftAlt);
            if (e.Control)
                Modifiers.Add(SlimDX.DirectInput.Key.LeftControl);
            if (e.Shift)
                Modifiers.Add(SlimDX.DirectInput.Key.LeftShift);

            
                this.tb_assignedSkill2.Text = get_HotkeyText(key, Modifiers);
                Properties.Settings.Default.HotkeySlot2 = get_HotkeyText(key, Modifiers);
                Properties.Settings.Default.Save();
            
        }

        void tb_assignedSkill1_KeyDown(object sender, KeyEventArgs e)
        {
            Keys Key = e.KeyCode;

            SlimDX.DirectInput.Key key = A_Tools.InputSimulator.IS_Keyboard.convert_KeysToKey(Key);

            List<SlimDX.DirectInput.Key> Modifiers = new List<Key>();

            if (e.Alt)
                Modifiers.Add(SlimDX.DirectInput.Key.LeftAlt);
            if (e.Control)
                Modifiers.Add(SlimDX.DirectInput.Key.LeftControl);
            if (e.Shift)
                Modifiers.Add(SlimDX.DirectInput.Key.LeftShift);

            

                this.tb_assignedSkill1.Text = get_HotkeyText(key, Modifiers);
                Properties.Settings.Default.HotkeySlot1 = get_HotkeyText(key, Modifiers);
                Properties.Settings.Default.Save();
            
        }
                               
        

        

        

        private void cb_autopotion_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.AutoPotionBool = this.cb_autopotion.Checked;
            Properties.Settings.Default.Save();
        }

        private void tb_autopotionhpvalue_TextChanged(object sender, EventArgs e)
        {
            int value;
            if(int.TryParse(this.tb_autopotionhpvalue.Text, out value))
            {
                if(value > 0 && value <= 100)
                {
                    Properties.Settings.Default.AutoPotionValue = value;
                    Properties.Settings.Default.Save();
                }
            }
        }

        private void tb_assignedGearSwap2_TextChanged(object sender, EventArgs e)
        {

        }

        

        private void button1_Click(object sender, EventArgs e)
        {

        }

        

        private void tb_assignedSkillRMB_TextChanged(object sender, EventArgs e)
        {

        }

        private void tb_assignedSkill4_TextChanged(object sender, EventArgs e)
        {

        }

        private void cb_fps_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.overlayfps = this.cb_fps.Checked;
            Properties.Settings.Default.Save();
        }

        private void cb_xptracker_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.overlayxptracker = this.cb_xptracker.Checked;
            Properties.Settings.Default.Save();
        }

        private void cb_skillbuttons_CheckedChanged(object sender, EventArgs e)
        {
            if (cb_skillbuttonsastext.Checked)
                cb_skillbuttonsastext.Checked = false;

            Properties.Settings.Default.overlayskillbuttons = this.cb_skillbuttons.Checked;
            Properties.Settings.Default.Save();
        }

       
                
        private void tb_assignedEditMode_TextChanged(object sender, EventArgs e)
        {

        }

        private void bt_delete_hotkey_skillslot1_Click(object sender, EventArgs e)
        {
            
            if(this.tb_assignedSkill1.Text.Length > 1)
            {
                this.tb_assignedSkill1.Text = "";
                Properties.Settings.Default.HotkeySlot1 = "";
                Properties.Settings.Default.Save();

            }
             
        }

        private void bt_delete_hotkey_skillslot2_Click(object sender, EventArgs e)
        {
            if (this.tb_assignedSkill2.Text.Length > 1)
            {
                this.tb_assignedSkill2.Text = "";
                Properties.Settings.Default.HotkeySlot2 = "";
                Properties.Settings.Default.Save();

            }
        }

        private void bt_delete_hotkey_skillslot3_Click(object sender, EventArgs e)
        {
            if (this.tb_assignedSkill3.Text.Length > 1)
            {
                this.tb_assignedSkill3.Text = "";
                Properties.Settings.Default.HotkeySlot3 = "";
                Properties.Settings.Default.Save();

            }
        }

        private void bt_delete_hotkey_skillslot4_Click(object sender, EventArgs e)
        {
            if (this.tb_assignedSkill4.Text.Length > 1)
            {
                this.tb_assignedSkill4.Text = "";
                Properties.Settings.Default.HotkeySlot4 = "";
                Properties.Settings.Default.Save();

            }
        }

        private void bt_delete_hotkey_skillslotrmb_Click(object sender, EventArgs e)
        {
            if (this.tb_assignedSkillRMB.Text.Length > 1)
            {
                this.tb_assignedSkillRMB.Text = "";
                Properties.Settings.Default.HotkeySlotRmb = "";
                Properties.Settings.Default.Save();

            }
        }

        

        private void bt_delete_hotkey_gearswap_editmode_Click(object sender, EventArgs e)
        {
            if (this.tb_assignedEditMode.Text.Length > 1)
            {
                this.tb_assignedEditMode.Text = "";
                Properties.Settings.Default.HotkeyEditMode = "";
                Properties.Settings.Default.Save();

            }
        }

        private void bt_delete_hotkey_gearswap1_Click(object sender, EventArgs e)
        {
            if (this.tb_assignedGearSwap1.Text.Length > 1)
            {
                this.tb_assignedGearSwap1.Text = "";
                Properties.Settings.Default.HotkeyGearSwap1 = "";
                Properties.Settings.Default.Save();

            }
        }

        private void bt_delete_hotkey_gearswap2_Click(object sender, EventArgs e)
        {
            if (this.tb_assignedGearSwap2.Text.Length > 1)
            {
                this.tb_assignedGearSwap2.Text = "";
                Properties.Settings.Default.HotkeyGearSwap2 = "";
                Properties.Settings.Default.Save();

            }
        }

        private void bt_delete_hotkey_gearswap3_Click(object sender, EventArgs e)
        {
            if (this.tb_assignedGearSwap3.Text.Length > 1)
            {
                this.tb_assignedGearSwap3.Text = "";
                Properties.Settings.Default.HotkeyGearSwap3 = "";
                Properties.Settings.Default.Save();

            }
        }

        private void bt_delete_hotkey_gearswap4_Click(object sender, EventArgs e)
        {
            if (this.tb_assignedGearSwap4.Text.Length > 1)
            {
                this.tb_assignedGearSwap4.Text = "";
                Properties.Settings.Default.HotkeyGearSwap4 = "";
                Properties.Settings.Default.Save();

            }
        }
               

        
        

        private void tb_assignedSkill1_TextChanged(object sender, EventArgs e)
        {

        }

        private void cb_autogamble_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.AutoGambleBool = this.cb_autogamble.Checked;
            Properties.Settings.Default.Save();
        }

         

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void tb_assignedGearSwap4_TextChanged(object sender, EventArgs e)
        {

        }

        private void tb_assignedGearSwap1_TextChanged(object sender, EventArgs e)
        {

        }

        private void bt_delete_hotkey_paragonpoints1_Click(object sender, EventArgs e)
        {
            if (this.tb_assignedParagonPoints1.Text.Length > 1)
            {
                this.tb_assignedParagonPoints1.Text = "";
                Properties.Settings.Default.HotkeyParagonPoints1 = "";
                Properties.Settings.Default.Save();

            }
        }

        private void bt_delete_hotkey_paragonpoints2_Click(object sender, EventArgs e)
        {
            if (this.tb_assignedParagonPoints2.Text.Length > 1)
            {
                this.tb_assignedParagonPoints2.Text = "";
                Properties.Settings.Default.HotkeyParagonPoints2 = "";
                Properties.Settings.Default.Save();

            }
        }

        private void bt_delete_hotkey_paragonpoints3_Click(object sender, EventArgs e)
        {
            if (this.tb_assignedParagonPoints3.Text.Length > 1)
            {
                this.tb_assignedParagonPoints3.Text = "";
                Properties.Settings.Default.HotkeyParagonPoints3 = "";
                Properties.Settings.Default.Save();

            }
        }

        private void bt_delete_hotkey_paragonpoints4_Click(object sender, EventArgs e)
        {
            if (this.tb_assignedParagonPoints4.Text.Length > 1)
            {
                this.tb_assignedParagonPoints4.Text = "";
                Properties.Settings.Default.HotkeyParagonPoints4 = "";
                Properties.Settings.Default.Save();

            }
        }

        

        private void bt_delete_hotkey_skillslotlmb_Click(object sender, EventArgs e)
        {
            if (this.tb_assignedSkillLMB.Text.Length > 1)
            {
                this.tb_assignedSkillLMB.Text = "";
                Properties.Settings.Default.HotkeySlotLmb = "";
                Properties.Settings.Default.Save();

            }
        }
                
                
        
                
        private void tb_updaterate_TextChanged(object sender, EventArgs e)
        {
            int value;
            if (int.TryParse(this.tb_updaterate.Text, out value))
            {
                if (value >= 1 && value <= 60)
                {
                    Properties.Settings.Default.D3Helper_UpdateRate = value;
                    Properties.Settings.Default.Save();
                }
                else
                {
                    MessageBox.Show("Wrong Value! Min: 1 Max: 60");
                }
            }
        }

        private void cb_riftprogressinrange_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.overlayriftprogress = this.cb_riftprogressinrange.Checked;
            Properties.Settings.Default.Save();
        }

        private void tb_riftprogressradius_TextChanged(object sender, EventArgs e)
        {
            double radius;
            if (double.TryParse(tb_riftprogressradius.Text, out radius))
            {
                if (radius > 0 && radius <= 100)
                {
                    Properties.Settings.Default.riftprogress_radius = radius;
                    Properties.Settings.Default.Save();
                }
            }
        }

        private void tb_assignedParagonPoints4_TextChanged(object sender, EventArgs e)
        {

        }

        private void bt_delete_hotkey_autogamble_Click(object sender, EventArgs e)
        {
            if (this.tb_assignedAutoGambleHotkey.Text.Length > 1)
            {
                this.tb_assignedAutoGambleHotkey.Text = "";
                Properties.Settings.Default.HotkeyAutoGamble = "";
                Properties.Settings.Default.Save();

            }
        }

        private void CB_ExtendedLogging_CheckedChanged(object sender, EventArgs e)
        {
            
            Properties.Settings.Default.Logger_extendedLog = this.CB_ExtendedLogging.Checked;
            Properties.Settings.Default.Save();
        }

        private void cb_skillbuttonsastext_CheckedChanged(object sender, EventArgs e)
        {
            if (cb_skillbuttons.Checked)
                cb_skillbuttons.Checked = false;

            Properties.Settings.Default.overlayskillbuttonsastext = this.cb_skillbuttonsastext.Checked;
            Properties.Settings.Default.Save();
        }

        private void cb_conventionelements_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.overlayconventiondraws = this.cb_conventionelements.Checked;
            Properties.Settings.Default.Save();
        }

        private void BTN_ParagonPointsNew_Click(object sender, EventArgs e)
        {
            if (this.TB_ParagonPointsSetupName.Text.Length < 1)
            {
                MessageBox.Show("Please enter a name for the new Setup");
                return;
            }

            if (this.TabControl_ParagonPoints.TabPages.Count == 4)
            {
                MessageBox.Show("You can not add more then 4 Setups per Hero");
                return;
            }

            this.TabControl_ParagonPoints.TabPages.Add(this.TB_ParagonPointsSetupName.Text, this.TB_ParagonPointsSetupName.Text);

            Populate_ParagonPoints_DefaultValues(TabControl_ParagonPoints.TabPages.OfType<TabPage>().Last());
        }

        private static List<string> ComboboxContent = new List<string>() { "core0", "core1", "core2", "core3", "offense0", "offense1", "offense2", "offense3", "defense0", "defense1", "defense2", "defense3", "utility0", "utility1", "utility2", "utility3" };

        private void Populate_ParagonPoints_DefaultValues(TabPage _this)
        {
            _this.BackColor = Color.Transparent;

            DataGridView DG = new DataGridView();
            DG.Width = _this.Width;
            DG.Height = _this.Height;
            DG.CellEndEdit += DG_CellEndEdit; ;
            


            DataGridViewComboBoxColumn comboBox = new DataGridViewComboBoxColumn();
            comboBox.Name = "Property";
            comboBox.DataSource = ComboboxContent;
            
            DataGridViewTextBoxColumn value = new DataGridViewTextBoxColumn();
            value.Name = "Value";

            DataGridViewTextBoxColumn maxvalue = new DataGridViewTextBoxColumn();
            maxvalue.Name = "MaxValue";
            maxvalue.ReadOnly = true;

            DG.Columns.Add(comboBox);
            DG.Columns.Add(value);
            DG.Columns.Add(maxvalue);

            _this.Controls.Add(DG);
        }

        private void DG_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView _this = sender as DataGridView;

            if (_this.CurrentCellAddress.X == 0)
            {
                DataGridViewComboBoxCell ComboBox = _this.Rows[e.RowIndex].Cells["Property"] as DataGridViewComboBoxCell;
                _this.Rows[e.RowIndex].Cells["MaxValue"].Value = Load_MaxValue(ComboBox.Value as string);

            }

            if (_this.CurrentCellAddress.X == 1)
            {
                DataGridViewTextBoxCell TextBox = _this.Rows[e.RowIndex].Cells["Value"] as DataGridViewTextBoxCell;

                var type = _this.Rows[e.RowIndex].Cells["Property"] as DataGridViewComboBoxCell;

                int maxvalue = Load_MaxValue(type.Value as string);

                int value = int.Parse(TextBox.Value as string);

                if (value < -1)
                    TextBox.Value = "-1";
                else if (value > maxvalue && maxvalue != -1)
                    TextBox.Value = maxvalue.ToString();
            }
        }

        private int Load_MaxValue(string PropertyType)
        {
            switch (PropertyType)
            {
                case "core0":
                case "core1":
                    return -1;

                case "core2":
                case "core3":
                case "offense0":
                case "offense1":
                case "offense2":
                case "offense3":
                case "defense0":
                case "defense1":
                case "defense2":
                case "defense3":
                case "utility0":
                case "utility1":
                case "utility2":
                case "utility3":
                    return 50;
                    
                default:
                    return 0;
            }
        }

        private void Load_Setups()
        {
            try
            {
                TabControl_ParagonPoints.TabPages.Clear();

                if (A_Collection.Me.ParagonPointSpender.Setups.ContainsKey(A_Collection.Me.HeroGlobals.HeroID))
                {
                    var Setups = A_Collection.Me.ParagonPointSpender.Setups[A_Collection.Me.HeroGlobals.HeroID];

                    foreach (var setup in Setups)
                    {
                        TabPage page = new TabPage();
                        page.Name = setup.Name;
                        page.Text = setup.Name;
                        page.BackColor = Color.Transparent;

                        DataGridView DG = new DataGridView();
                        DG.Width = _this.Width;
                        DG.Height = _this.Height;
                        DG.CellEndEdit += DG_CellEndEdit;
                        

                        DataGridViewComboBoxColumn comboBox = new DataGridViewComboBoxColumn();
                        comboBox.Name = "Property";
                        comboBox.DataSource = ComboboxContent;

                        DataGridViewTextBoxColumn value = new DataGridViewTextBoxColumn();
                        value.Name = "Value";

                        DataGridViewTextBoxColumn maxvalue = new DataGridViewTextBoxColumn();
                        maxvalue.Name = "MaxValue";
                        maxvalue.ReadOnly = true;

                        DG.Columns.Add(comboBox);
                        DG.Columns.Add(value);
                        DG.Columns.Add(maxvalue);

                        page.Controls.Add(DG);

                        TabControl_ParagonPoints.TabPages.Add(page);

                        foreach (var bonuspoint in setup.BonusPoints)
                        {
                            DG.Rows.Add(bonuspoint.Type.ToString(), bonuspoint.Value.ToString(),
                                bonuspoint.MaxValue.ToString());
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void Save_Setups()
        {
            try
            {
                List<ParagonPointSetup> Buffer = new List<ParagonPointSetup>();

                foreach (var tabpage in TabControl_ParagonPoints.TabPages.OfType<TabPage>())
                {
                    DataGridView DG = tabpage.Controls.OfType<DataGridView>().FirstOrDefault();

                    List<BonusPoint> BonusPoints = new List<BonusPoint>();

                    foreach (var row in DG.Rows.OfType<DataGridViewRow>())
                    {
                        if (row.Cells["Property"].Value != null)
                        {
                            BonusPoints Type =
                                (BonusPoints) Enum.Parse(typeof (BonusPoints), row.Cells["Property"].Value as string);

                            int Value = int.Parse(row.Cells["Value"].Value as string);

                            BonusPoints.Add(new BonusPoint(Type, Value, Load_MaxValue(Type.ToString())));
                        }
                    }

                    Buffer.Add(new ParagonPointSetup(tabpage.TabIndex, tabpage.Name, BonusPoints));
                }

                if(!A_Collection.Me.ParagonPointSpender.Setups.ContainsKey(A_Collection.Me.HeroGlobals.HeroID))
                    A_Collection.Me.ParagonPointSpender.Setups.Add(A_Collection.Me.HeroGlobals.HeroID, Buffer);
                else
                {
                    A_Collection.Me.ParagonPointSpender.Setups[A_Collection.Me.HeroGlobals.HeroID] = Buffer;
                }

                A_Tools.T_ExternalFile.ParagonPointSpenderSettings.Save();
            }
            catch (Exception)
            {
            }
        }
        private void BTN_ParagonPointsReload_Click(object sender, EventArgs e)
        {
            Load_Setups();
        }

        private void BTN_ParagonPointsSave_Click(object sender, EventArgs e)
        {
            Save_Setups();
        }

        private void BTN_ParagonPointsDeleteTab_Click(object sender, EventArgs e)
        {
            if(TabControl_ParagonPoints.SelectedIndex >= 0 && TabControl_ParagonPoints.SelectedIndex < TabControl_ParagonPoints.TabCount)
                this.TabControl_ParagonPoints.TabPages.RemoveAt(TabControl_ParagonPoints.SelectedIndex);
        }

        private void BTN_SkillBuilds_LoadIngameBuild_Click(object sender, EventArgs e)
        {
            SkillBuild_LoadIngame();
            SkillBuild_UpdateView();

            A_Tools.T_ExternalFile.SkillBuilds.Save();
        }

        private void SkillBuild_LoadIngame()
        {
            try
            {
                var selected = Panel_SkillBuilds_View.Controls.OfType<CheckBox>().Where(x => x.Checked);

                if (selected.Count() == 0)
                {
                    if (TB_SkillBuilds_NameInput.Text.Length < 1)
                    {
                        MessageBox.Show("Please enter a name");
                        return;
                    }

                    int countExistingBuilds =
                        A_Collection.Me.SkillBuilds.Builds.Where(x => x.Value == A_Collection.Me.HeroGlobals.HeroID)
                            .Count();

                    if (countExistingBuilds == 4)
                    {
                        MessageBox.Show("You cannot add more then 4 Builds per hero");
                        return;
                    }

                    Dictionary<int, int> ActiveSkills;
                    lock (A_Collection.Me.HeroDetails.ActiveSkills)
                        ActiveSkills = A_Collection.Me.HeroDetails.ActiveSkills.ToDictionary(x => x.Key, y => y.Value);

                    List<int> PassiveSkills;
                    lock (A_Collection.Me.HeroDetails.PassiveSkills)
                        PassiveSkills = A_Collection.Me.HeroDetails.PassiveSkills.ToList();

                    SkillBuildSwap.SkillBuild NewBuild = new SkillBuildSwap.SkillBuild(countExistingBuilds,
                        TB_SkillBuilds_NameInput.Text, new List<SkillBuildSwap.ActiveSkill>(),
                        new List<SkillBuildSwap.PassiveSkill>());

                    foreach (var active in ActiveSkills)
                    {
                        NewBuild.ActiveSkills.Add(new SkillBuildSwap.ActiveSkill(active.Key, active.Value));
                    }

                    foreach (var passive in PassiveSkills)
                    {
                        NewBuild.PassiveSkills.Add(new SkillBuildSwap.PassiveSkill(passive));
                    }

                    if (!SkillBuild_AlreadyExists(NewBuild))
                    {

                        A_Collection.Me.SkillBuilds.Builds.Add(NewBuild, A_Collection.Me.HeroGlobals.HeroID);

                        TB_SkillBuilds_NameInput.Clear();
                    }
                }
                else
                {
                    if (selected.Count() > 1)
                    {
                        MessageBox.Show(
                            "You have selected more then one Build. Please choose only one Build for the override!");
                        return;
                    }
                    else if (selected.Count() == 1)
                    {
                        Dictionary<int, int> ActiveSkills;
                        lock (A_Collection.Me.HeroDetails.ActiveSkills)
                            ActiveSkills = A_Collection.Me.HeroDetails.ActiveSkills.ToDictionary(x => x.Key, y => y.Value);

                        List<int> PassiveSkills;
                        lock (A_Collection.Me.HeroDetails.PassiveSkills)
                            PassiveSkills = A_Collection.Me.HeroDetails.PassiveSkills.ToList();

                        int buildId = int.Parse(selected.First().Name);

                        SkillBuildSwap.SkillBuild ExistingBuild = A_Collection.Me.SkillBuilds.Builds.FirstOrDefault(x => x.Value == A_Collection.Me.HeroGlobals.HeroID && x.Key.Id == buildId).Key;

                        ExistingBuild.ActiveSkills.Clear();
                        ExistingBuild.PassiveSkills.Clear();

                        foreach (var active in ActiveSkills)
                        {
                            ExistingBuild.ActiveSkills.Add(new SkillBuildSwap.ActiveSkill(active.Key, active.Value));
                        }

                        foreach (var passive in PassiveSkills)
                        {
                            ExistingBuild.PassiveSkills.Add(new SkillBuildSwap.PassiveSkill(passive));
                        }
                        
                    }
                }
            }
            catch (Exception)
            {
                
            }
        }

        private bool SkillBuild_AlreadyExists(SkillBuildSwap.SkillBuild Build)
        {
            var Builds = A_Collection.Me.SkillBuilds.Builds.Where(x => x.Value == A_Collection.Me.HeroGlobals.HeroID);

            

            foreach (var build in Builds)
            {
                var actives = build.Key.ActiveSkills;
                var passives = build.Key.PassiveSkills;

                var count_actives = actives.Count;
                var count_passives = passives.Count;

                var count_equals = 0;

                foreach (var active in Build.ActiveSkills)
                {
                    if (actives.FirstOrDefault(x => x.PowerSno == active.PowerSno && x.Rune == active.Rune) != null)
                        count_equals++;
                }

                foreach (var passive in Build.PassiveSkills)
                {
                    if (passives.FirstOrDefault(x => x.PowerSno == passive.PowerSno) != null)
                        count_equals++;
                }

                if (count_equals == count_actives + count_passives)
                    return true;

                count_equals = 0;

            }

            return false;
        }
        
        private void SkillBuild_UpdateView()
        {
            try
            {

                Panel_SkillBuilds_View.Controls.Clear();

                var CurrentBuilds =
                    A_Collection.Me.SkillBuilds.Builds.Where(x => x.Value == A_Collection.Me.HeroGlobals.HeroID)
                        .ToList();

                for (int i = 0; i < CurrentBuilds.Count(); i++)
                {
                    var Build = CurrentBuilds[i];

                    // Add CheckBox
                    CheckBox selector = new CheckBox();
                    selector.Top = Panel_SkillBuilds_View.Top + 32 * i;
                    selector.Left = Panel_SkillBuilds_View.Left;
                    selector.Name = i.ToString();
                    selector.BackColor = Color.Transparent;
                    selector.AutoSize = true;

                    Panel_SkillBuilds_View.Controls.Add(selector);

                    foreach (var active in Build.Key.ActiveSkills)
                    {
                        if (active.PowerSno == 0)
                            continue;

                        var power =
                            A_Collection.Presets.SkillPowers.AllSkillPowers.First(x => x.PowerSNO == active.PowerSno);

                        //-- Add Active Skill Image
                        Image icon =
                            Properties.Resources.ResourceManager.GetObject(power.Name.ToLower()) as Image;

                        icon = resizeImage(icon, new Size(32, 32));

                        Button SkillIcon = new Button();
                        SkillIcon.Name = i.ToString() + "|" + active.PowerSno.ToString();
                        SkillIcon.Width = 32;
                        SkillIcon.Height = 32;
                        SkillIcon.FlatStyle = FlatStyle.Flat;
                        SkillIcon.BackgroundImage = icon;
                        SkillIcon.FlatAppearance.BorderSize = 0;
                        SkillIcon.Top = Panel_SkillBuilds_View.Top + SkillIcon.Height*i;

                        ToolTip t = new ToolTip();
                        t.SetToolTip(SkillIcon, "Rune: " + power.Runes.FirstOrDefault(x => x.RuneIndex == active.Rune).Name);

                        if (
                            Panel_SkillBuilds_View.Controls.OfType<CheckBox>()
                                .Where(x => x.Name == i.ToString())
                                .Count() > 0 &&
                            Panel_SkillBuilds_View.Controls.OfType<Button>()
                                .Where(x => int.Parse(x.Name.Split('|')[0]) == i)
                                .Count() < 1)
                        {
                            SkillIcon.Left =
                                Panel_SkillBuilds_View.Controls.OfType<CheckBox>()
                                    .Where(x => x.Name == i.ToString())
                                    .Last()
                                    .Right;
                        }
                        else if (
                            Panel_SkillBuilds_View.Controls.OfType<Button>()
                                .Where(x => int.Parse(x.Name.Split('|')[0]) == i)
                                .Count() > 0)
                            SkillIcon.Left =
                                Panel_SkillBuilds_View.Controls.OfType<Button>()
                                    .Where(x => int.Parse(x.Name.Split('|')[0]) == i)
                                    .Last()
                                    .Right;

                        Panel_SkillBuilds_View.Controls.Add(SkillIcon);
                        //
                    }

                    foreach (var passive in Build.Key.PassiveSkills)
                    {

                        string powerName =
                            A_Collection.Presets.SNOPowers.AllPowers.First(x => x.Key == passive.PowerSno)
                                .Value;

                        //-- Add Passive Skill Image
                        
                        Image icon =
                            Properties.Resources.ResourceManager.GetObject(powerName) as Image;

                        icon = resizeImage(icon, new Size(32, 32));

                        Button SkillIcon = new Button();
                        SkillIcon.Name = i.ToString() + "|" + passive.PowerSno.ToString();
                        SkillIcon.Width = 32;
                        SkillIcon.Height = 32;
                        SkillIcon.FlatStyle = FlatStyle.Flat;
                        SkillIcon.BackgroundImage = icon;
                        SkillIcon.FlatAppearance.BorderSize = 0;
                        SkillIcon.Top = Panel_SkillBuilds_View.Top + SkillIcon.Height*i;

                        if (
                            Panel_SkillBuilds_View.Controls.OfType<Button>()
                                .Where(x => int.Parse(x.Name.Split('|')[0]) == i)
                                .Count() > 0)
                            SkillIcon.Left =
                                Panel_SkillBuilds_View.Controls.OfType<Button>()
                                    .Where(x => int.Parse(x.Name.Split('|')[0]) == i)
                                    .Last()
                                    .Right;

                        Panel_SkillBuilds_View.Controls.Add(SkillIcon);
                        //
                    }

                    // Add Build Name Label

                Label name = new Label();
                    name.BackColor = Color.Transparent;
                    name.AutoSize = true;
                    name.Text = Build.Key.Name;
                    name.Height = 32;
                    name.Font = new Font(name.Font, FontStyle.Bold);
                    name.Top = Panel_SkillBuilds_View.Top + name.Height * i + 9;

                    if (
                        Panel_SkillBuilds_View.Controls.OfType<Button>()
                            .Where(x => int.Parse(x.Name.Split('|')[0]) == i)
                            .Count() > 0)
                        name.Left =
                            Panel_SkillBuilds_View.Controls.OfType<Button>()
                                .Where(x => int.Parse(x.Name.Split('|')[0]) == i)
                                .Last()
                                .Right;

                    Panel_SkillBuilds_View.Controls.Add(name);

                }
            }
            catch { }
        }
        public static Image resizeImage(Image imgToResize, Size size)
        {
            return (Image)(new Bitmap(imgToResize, size));
        }

        private void BTN_SkillBuilds_DeleteSelected_Click(object sender, EventArgs e)
        {
            var selected = Panel_SkillBuilds_View.Controls.OfType<CheckBox>().Where(x => x.Checked);

            var Builds = A_Collection.Me.SkillBuilds.Builds.Where(x => x.Value == A_Collection.Me.HeroGlobals.HeroID);

            List<SkillBuildSwap.SkillBuild> RemoveBuffer = new List<SkillBuildSwap.SkillBuild>();

            if (Builds.Count() > 0)
            {
                foreach (var select in selected)
                {
                    int buildId = int.Parse(select.Name);

                    RemoveBuffer.Add(Builds.FirstOrDefault(x => x.Key.Id == buildId).Key);
                }
            }

            foreach (var toRemove in RemoveBuffer)
            {
                A_Collection.Me.SkillBuilds.Builds.Remove(toRemove);
            }

            A_Tools.T_ExternalFile.SkillBuilds.Save();

            SkillBuild_UpdateView();
        }

        private void BTN_SkillBuilds_UpdateView_Click(object sender, EventArgs e)
        {
            SkillBuild_UpdateView();
        }

        private void bt_delete_hotkey_skillbuild1_Click(object sender, EventArgs e)
        {
            if (this.tb_assignedSkillBuild1.Text.Length > 1)
            {
                this.tb_assignedSkillBuild1.Text = "";
                Properties.Settings.Default.HotkeySkillBuild1 = "";
                Properties.Settings.Default.Save();

            }
        }

        private void bt_delete_hotkey_skillbuild2_Click(object sender, EventArgs e)
        {
            if (this.tb_assignedSkillBuild2.Text.Length > 1)
            {
                this.tb_assignedSkillBuild2.Text = "";
                Properties.Settings.Default.HotkeySkillBuild2 = "";
                Properties.Settings.Default.Save();

            }
        }

        private void bt_delete_hotkey_skillbuild3_Click(object sender, EventArgs e)
        {
            if (this.tb_assignedSkillBuild3.Text.Length > 1)
            {
                this.tb_assignedSkillBuild3.Text = "";
                Properties.Settings.Default.HotkeySkillBuild3 = "";
                Properties.Settings.Default.Save();

            }
        }

        private void bt_delete_hotkey_skillbuild4_Click(object sender, EventArgs e)
        {
            if (this.tb_assignedSkillBuild4.Text.Length > 1)
            {
                this.tb_assignedSkillBuild4.Text = "";
                Properties.Settings.Default.HotkeySkillBuild4 = "";
                Properties.Settings.Default.Save();

            }
        }

        private void CB_ApsAndSnapShotAPs_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.Overlay_APS = this.CB_ApsAndSnapShotAPs.Checked;
            Properties.Settings.Default.Save();
        }

        private void bt_delete_hotkey_autopick_Click(object sender, EventArgs e)
        {
            if (this.tb_assignedAutoPick.Text.Length > 1)
            {
                this.tb_assignedAutoPick.Text = "";
                Properties.Settings.Default.HotkeyAutoPick = "";
                Properties.Settings.Default.Save();

            }
        }

        private void CB_AutoPick_Gem_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.AutoPickSettings_Gem = this.CB_AutoPick_Gem.Checked;
            Properties.Settings.Default.Save();
        }

        private void CB_AutoPick_Material_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.AutoPickSettings_Material = this.CB_AutoPick_Material.Checked;
            Properties.Settings.Default.Save();
        }

        private void CB_AutoPick_Legendary_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.AutoPickSettings_Legendary = this.CB_AutoPick_Legendary.Checked;
            Properties.Settings.Default.Save();
        }

        private void CB_AutoPick_LegendaryAncient_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.AutoPickSettings_LegendaryAncient = this.CB_AutoPick_LegendaryAncient.Checked;
            Properties.Settings.Default.Save();
        }

        private void CB_AutoPick_GreaterRiftKeystone_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.AutoPickSettings_GreaterRiftKeystone = this.CB_AutoPick_GreaterRiftKeystone.Checked;
            Properties.Settings.Default.Save();
        }

        private void TB_AutoPick_PickupRadius_TextChanged(object sender, EventArgs e)
        {
            int radius;
            if (int.TryParse(TB_AutoPick_PickupRadius.Text, out radius))
            {
                if (radius > 0 && radius <= 100)
                {
                    Properties.Settings.Default.AutoPickSettings_PickupRadius = radius;
                    Properties.Settings.Default.Save();
                }
            }
        }

        private void CB_AutoPick_Whites_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.AutoPickSettings_Whites = this.CB_AutoPick_Whites.Checked;
            Properties.Settings.Default.Save();
        }

        private void CB_AutoPick_Magics_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.AutoPickSettings_Magics = this.CB_AutoPick_Magics.Checked;
            Properties.Settings.Default.Save();
        }

        private void CB_AutoPick_Rares_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.AutoPickSettings_Rares = this.CB_AutoPick_Rares.Checked;
            Properties.Settings.Default.Save();
        }

        private void bt_delete_hotkey_autocube_upgradeRare_Click(object sender, EventArgs e)
        {
            if (this.tb_assignedAutoCube_UpgradeRare.Text.Length > 1)
            {
                this.tb_assignedAutoCube_UpgradeRare.Text = "";
                Properties.Settings.Default.HotkeyAutoCube_UpgradeRare = "";
                Properties.Settings.Default.Save();

            }
        }
    }
}
