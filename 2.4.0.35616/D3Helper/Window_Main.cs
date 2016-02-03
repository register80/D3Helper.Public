using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Timers;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Resources;
using System.Collections;
using System.Diagnostics;
using System.Threading;
using System.Runtime.InteropServices;
using System.Drawing.Text;
using System.Reflection;
using Enigma.D3;
using Enigma.Memory;
using D3Helper.A_Collection;
using D3Helper;
using D3Helper.A_WPFOverlay;


using SlimDX.DirectInput;


namespace D3Helper
{
    
    public partial class Window_Main : Form
    {
        

        public static Form d3helperform;
        public static DateTime Start = DateTime.Now;
        public static PrivateFontCollection _FontCollection = new PrivateFontCollection();

        public static readonly Version SupportedVersion = new Version(2, 4, 0, 35616);
        
        public Window_Main()
        {

            try
            {

                InitializeComponent();

                
                //-- attach Events
                this.FormClosed += FormClose;
                this.FormClosing += Form1_FormClosing;
                //
                if (SupportedProcessVersion())
                {
                    
                    //-- Initialize Collector and Handler Thread
                    if(!Program.SingleThreaded)
                        A_Initialize.Th_ICollector.New_ICollector();
                    A_Initialize.Th_Handler.New_Handler();
                    //
                    if (A_Tools.Version.AppVersion.isOutdated()) // !!!!! REENABLE THIS!!!!!!
                    {
                        Window_Outdated WO = new Window_Outdated();
                        WO.ShowDialog();
                    }
                    //-- Access Validation
                    if (!A_Tools.Authentification.Validation.IsValidated()) // !!!!! REENABLE THIS!!!!!!
                        ;
                    //



                    System.Timers.Timer UpdateUI = new System.Timers.Timer(250);
                    UpdateUI.Elapsed += RefreshUI;
                    UpdateUI.Start();


                    d3helperform = this;

                    Load_CustomFonts();
                }
                

                
            }
            catch (Exception e)
            {
                A_Handler.Log.ExceptionLogEntry newEntry = new A_Handler.Log.ExceptionLogEntry(e, DateTime.Now,
                    A_Enums.ExceptionThread.MainWindow);

                lock (A_Handler.Log.Exception.ExceptionLog) A_Handler.Log.Exception.ExceptionLog.Add(newEntry);
            }
        }

        
        private bool SupportedProcessVersion()
        {
            if (GetFileVersion() != SupportedVersion)
            {
                return false;
            }
            return true;
        }
        private static Version GetFileVersion()
        {
            var process = Process.GetProcessesByName("Diablo III")
                .FirstOrDefault();
            if (process != null)
            {

                var fileVersionInfo = process.MainModule.FileVersionInfo;
                return new Version(
                    fileVersionInfo.FileMajorPart,
                    fileVersionInfo.FileMinorPart,
                    fileVersionInfo.FileBuildPart,
                    fileVersionInfo.FilePrivatePart);
            }
            return default(Version);
        }
        private void Load_CustomFonts()
        {
            try
            {
                //Create your private font collection object.
                PrivateFontCollection pfc = new PrivateFontCollection();

                //Select your font from the resources.
                //My font here is "Digireu.ttf"
                int fontLength = Properties.Resources.EXL_____.Length;

                // create a buffer to read in to
                byte[] fontdata = Properties.Resources.EXL_____;

                // create an unsafe memory block for the font data
                System.IntPtr data = Marshal.AllocCoTaskMem(fontLength);

                // copy the bytes to the unsafe memory block
                Marshal.Copy(fontdata, 0, data, fontLength);

                // pass the font to the font collection
                pfc.AddMemoryFont(data, fontLength);

                // free up the unsafe memory
                Marshal.FreeCoTaskMem(data);

                _FontCollection = pfc;
            }
            catch (Exception)
            {
                
                throw;
            }
        }
        void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.D3Helper_MainForm_StartPosition = new Point(this.Left, this.Top);
            Properties.Settings.Default.Save();
        }
        
                        
        public static SlimDX.DirectInput.DirectInput directInput;
        public static SlimDX.DirectInput.Keyboard keyboard;
       

        private void Window_Main_Load(object sender, EventArgs e)
        {
            if (!SupportedProcessVersion())
            {
                this.Text = "You are running a not supported D3Client(" + GetFileVersion().ToString() +
                            ") Supported Version is " + SupportedVersion;

                return;
            }

            typeof(Button).InvokeMember("DoubleBuffered",
    BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
    null, bt_Skill1, new object[] { true });

            typeof(Button).InvokeMember("DoubleBuffered",
    BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
    null, bt_Skill2, new object[] { true });

            typeof(Button).InvokeMember("DoubleBuffered",
    BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
    null, bt_Skill3, new object[] { true });

            typeof(Button).InvokeMember("DoubleBuffered",
    BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
    null, bt_Skill4, new object[] { true });

            typeof(Button).InvokeMember("DoubleBuffered",
    BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
    null, bt_SkillLmb, new object[] { true });

            typeof(Button).InvokeMember("DoubleBuffered",
    BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
    null, bt_SkillRmb, new object[] { true });

            A_WPFOverlay.Overlay o = new A_WPFOverlay.Overlay();
            o.Show();

            directInput = new DirectInput();
            keyboard = new SlimDX.DirectInput.Keyboard(directInput);
            keyboard.Acquire();
           
           
            Point d3helpermainwindowpos = Properties.Settings.Default.D3Helper_MainForm_StartPosition;
            var screens = Screen.AllScreens.OrderByDescending(x => x.Bounds.X);

            if (d3helpermainwindowpos.X >= screens.Last().Bounds.Left && d3helpermainwindowpos.X <= screens.First().Bounds.Right && d3helpermainwindowpos.Y <= screens.First().Bounds.Bottom && d3helpermainwindowpos.Y >= screens.First().Bounds.Top)
            {
                this.Top = Properties.Settings.Default.D3Helper_MainForm_StartPosition.Y;
                this.Left = Properties.Settings.Default.D3Helper_MainForm_StartPosition.X;
            }
            else
            {
                this.Top = 0;
                this.Left = 0;
            }




            this.btn_donate.Image = new Bitmap(Properties.Resources.paypal_donate_button11, new Size(this.btn_donate.Width, this.btn_donate.Height));
            
            this.btn_info.Image = new Bitmap(Properties.Resources._480px_Info_icon_002_svg, new Size(this.btn_info.Width, this.btn_info.Height));
            this.btn_settings.Image = new Bitmap(Properties.Resources.pignon, new Size(this.btn_settings.Width, this.btn_settings.Height));
            

            this.bt_update.Visible = false;

            this.Text = "D3Helper - V" + A_Tools.Version.AppVersion.version;
            

            DateTime latestOnlineVersion = A_Tools.Version.AppVersion.LatestOnlineVersion;
            DateTime currentVersion = A_Tools.Version.AppVersion.get_CurrentVersionDate();

            if (latestOnlineVersion > currentVersion)
            {
                this.lb_versionlb.Text = "New Version Available!" + System.Environment.NewLine + latestOnlineVersion.ToString("yy.MM.d.H");
                //this.lb_versionlb.Invoke((MethodInvoker)(() => this.lb_versionlb.Text = "New Version Available!"));
                this.bt_update.Visible = true;
            }
            else
            {
                this.lb_versionlb.Text = "";
                //this.lb_versionlb.Invoke((MethodInvoker)(() => this.lb_versionlb.Text = ""));
            }


        }
        private void FormClose(Object sender, FormClosedEventArgs e)
        {
            

            this.Dispose();
            System.Environment.Exit(1);
            
        }

        private void Populate_SkillButtonContextMenu()
        {
            try
            {
                List<Button> MainWindow_SkillButtons = new List<Button>()
                {
                    bt_Skill1,
                    bt_Skill2,
                    bt_Skill3,
                    bt_Skill4,
                    bt_SkillLmb,
                    bt_SkillRmb
                };
                List<SkillPower> AllSkillPowers = new List<SkillPower>()
                {
                    Skills.SkillInfos._HotBar1Skill,
                    Skills.SkillInfos._HotBar2Skill,
                    Skills.SkillInfos._HotBar3Skill,
                    Skills.SkillInfos._HotBar4Skill,
                    Skills.SkillInfos._HotBarLeftClickSkill,
                    Skills.SkillInfos._HotBarRightClickSkill
                };

                for (int i = 0; i < MainWindow_SkillButtons.Count; i++)
                {
                    Button _this = MainWindow_SkillButtons[i];
                    SkillPower AssignedPower = AllSkillPowers[i];

                    if (AssignedPower != null)
                    {
                        List<SkillData> AllDefinitions =
                            A_Collection.SkillCastConditions.Custom.CustomDefinitions.Where(
                                x => x.Power.PowerSNO == AssignedPower.PowerSNO).ToList();

                        ContextMenuStrip CMS = new ContextMenuStrip();


                        foreach (var definition in AllDefinitions)
                            {
                                ToolStripItem newItem = new ToolStripMenuItem();
                                newItem.Text = definition.Name;
                                newItem.Name = AssignedPower.PowerSNO.ToString();
                                newItem.AutoSize = true;

                                CMS.Items.Add(newItem);
                            }
                       ToolStripItem _newItem = new ToolStripMenuItem();
                        _newItem.Text = "Create New Definition";
                        _newItem.Name = AssignedPower.PowerSNO.ToString();
                        _newItem.AutoSize = true;

                            CMS.Items.Add(_newItem);

                        
                        CMS.ItemClicked += CMS_ItemClicked;

                        _this.ContextMenuStrip = CMS;
                    }
                }
            }
            catch (Exception)
            {
                
            }
        }

        private void CMS_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ToolStripItem _this = e.ClickedItem;

            SkillData _selected =
                A_Collection.SkillCastConditions.Custom.CustomDefinitions.FirstOrDefault(x => x.Name == _this.Text);

            if (_selected != null)
            {
                if (Window_SkillEditor._this == null)
                {
                    Window_SkillEditor._PreselectedDefinition = true;
                    Window_SkillEditor._SelectedSkill = _selected;
                    Window_SkillEditor
                        Editor = new Window_SkillEditor();
                    Editor.Show();
                }
            }
            else
            {
                if (Window_SkillEditor._this == null)
                {
                    Window_SkillEditor._CreateNewDefinition = true;
                    Window_SkillEditor._NewDefinitionPowerSNO = int.Parse(_this.Name);
                    Window_SkillEditor
                        Editor = new Window_SkillEditor();
                    Editor.Show();
                }
            }
        }

        private void RefreshUI(object sender, ElapsedEventArgs e)
        {


            
            try
            {
                List<Button> MainWindow_SkillButtons = new List<Button>()
                {
                    bt_Skill1,
                    bt_Skill2,
                    bt_Skill3,
                    bt_Skill4,
                    bt_SkillLmb,
                    bt_SkillRmb
                };
                List<SkillPower> AllSkillPowers = new List<SkillPower>()
                {
                    Skills.SkillInfos._HotBar1Skill,
                    Skills.SkillInfos._HotBar2Skill,
                    Skills.SkillInfos._HotBar3Skill,
                    Skills.SkillInfos._HotBar4Skill,
                    Skills.SkillInfos._HotBarLeftClickSkill,
                    Skills.SkillInfos._HotBarRightClickSkill
                };
                List<bool> AutoCastOverrides = new List<bool>()
                {
                    Me.AutoCastOverrides.AutoCast1Override,
                    Me.AutoCastOverrides.AutoCast2Override,
                    Me.AutoCastOverrides.AutoCast3Override,
                    Me.AutoCastOverrides.AutoCast4Override,
                    Me.AutoCastOverrides.AutoCastLMBOverride,
                    Me.AutoCastOverrides.AutoCastRMBOverride
                };

                for (int i = 0; i < MainWindow_SkillButtons.Count; i++)
                {
                    Button control = MainWindow_SkillButtons[i];
                    SkillPower power = AllSkillPowers[i];
                    bool autocastoverride = AutoCastOverrides[i];
                    Image SkillIcon = null;
                    if (power != null)
                        SkillIcon = Properties.Resources.ResourceManager.GetObject(power.Name.ToLower()) as Image;

                    //-- Set SkillIcon
                    if (power != null)
                    {
                        if(control.Image != SkillIcon)
                            control.Invoke((MethodInvoker) (() => control.Image = SkillIcon));
                    }
                    else
                    {
                        if (control.Image != null)
                            control.Invoke((MethodInvoker) (() => control.Image = null));

                    }
                    //
                    //-- Set BackColor
                    if (power != null)
                    {
                        if (autocastoverride)
                        {
                            if (control.BackColor != Color.Red)
                                control.Invoke((MethodInvoker) (() => control.BackColor = Color.Red));
                        }

                        else
                        {
                            if (control.BackColor != Color.Green)
                                control.Invoke((MethodInvoker) (() => control.BackColor = Color.Green));
                        }
                    }
                    else
                    {
                        if (control.BackColor != Color.Transparent)
                            control.Invoke((MethodInvoker) (() => control.BackColor = Color.Transparent));
                    }
                    //
                }

                Populate_SkillButtonContextMenu();
            }
            catch { }

            

        }
                
        private void bt_Skill1_Click(object sender, EventArgs e)
        {
            A_Tools.T_ExternalFile.AutoCastOverrides.ChangeOverrides(0);
        }

        private void bt_Skill2_Click(object sender, EventArgs e)
        {
            A_Tools.T_ExternalFile.AutoCastOverrides.ChangeOverrides(1);
        }

        private void bt_Skill3_Click(object sender, EventArgs e)
        {
            A_Tools.T_ExternalFile.AutoCastOverrides.ChangeOverrides(2);
        }

        private void bt_Skill4_Click(object sender, EventArgs e)
        {
            A_Tools.T_ExternalFile.AutoCastOverrides.ChangeOverrides(3);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Window_Info._this == null)
            {
                Window_Info s = new Window_Info();
                s.Show();
            }
        }
        
        private void bt_hotkeys_Click(object sender, EventArgs e)
        {
            if (Window_Settings._this == null)
            {
                Window_Settings hf = new Window_Settings();
                hf.Show();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (Window_Changelog._this == null)
            {
                Window_Changelog cl = new Window_Changelog();
                cl.Show();
            }
        }

        private void bt_update_Click(object sender, EventArgs e)
        {
            Process.Start("http://d3helper.freeforums.net/board/3/releases");
        }

        private void bt_SkillRmb_Click(object sender, EventArgs e)
        {
            A_Tools.T_ExternalFile.AutoCastOverrides.ChangeOverrides(4);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=QS458DYXLJYWJ");
        }
        
        private void bt_SkillLmb_Click(object sender, EventArgs e)
        {
            A_Tools.T_ExternalFile.AutoCastOverrides.ChangeOverrides(5);
        }

        private void BTN_Forum_Click(object sender, EventArgs e)
        {
            Process.Start("http://d3helper.freeforums.net/");
        }
    }
}
