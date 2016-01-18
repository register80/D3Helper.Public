using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using System.Timers;
using System.Windows.Input;
using D3Helper.A_Collection;
using D3Helper.A_Enums;
using MouseEventArgs = System.Windows.Forms.MouseEventArgs;

namespace D3Helper
{
    
    public partial class Window_SkillEditor : Form
    {
        public class ComboboxItem
        {
            public string Text { get; set; }
            public object Value { get; set; }

            public override string ToString()
            {
                return Text;
            }
        }

        
        public static SkillData _SelectedSkill = null;
        private static CastCondition _SelectedCondition = null;

        private Point MouseDownLocation;
        private static int Clicks = 0;
        private static Button _clickedCondition;
        private static Button _clickedSkill;
        private static bool IsMoving = false;
        private static Keys PressedKey = Keys.None;

        public static bool _PreselectedDefinition = false;
        public static bool _CreateNewDefinition = false;
        public static int _NewDefinitionPowerSNO;

        public static Window_SkillEditor _this = null;


        public Window_SkillEditor()
        {
            

            InitializeComponent();

            this.FormClosed += Window_SkillEditor_FormClosed;

            Panel_SelectedSkillDetails.KeyDown += Panel_SelectedSkillDetails_KeyDown;
            Panel_SelectedSkillDetails.KeyUp += Panel_SelectedSkillDetails_KeyUp;
            Panel_SelectedSkillDetails.Scroll += Panel_SelectedSkillDetails_Scroll;
            Panel_SelectedSkillDetails.MouseWheel += Panel_SelectedSkillDetails_MouseWheel;

            Panel_SkillOverview.KeyDown += Panel_SkillOverview_KeyDown;
            Panel_SkillOverview.KeyUp += Panel_SkillOverview_KeyUp;
            Panel_SkillOverview.Scroll += Panel_SkillOverview_Scroll;
            Panel_SkillOverview.MouseWheel += Panel_SkillOverview_MouseWheel;

            this.CB_ConditionSelection.DrawMode = DrawMode.OwnerDrawFixed;
            this.CB_ConditionSelection.DrawItem += new DrawItemEventHandler(CB_ConditionSelection_DrawItem);

            toolTip1.ShowAlways = false;
            

        }

        private void Panel_SkillOverview_MouseWheel(object sender, MouseEventArgs e)
        {
            Panel_SkillOverview.Refresh();
        }

        private void Panel_SelectedSkillDetails_MouseWheel(object sender, MouseEventArgs e)
        {
            Panel_SelectedSkillDetails.Refresh();
        }

        private void Panel_SkillOverview_Scroll(object sender, ScrollEventArgs e)
        {
            Panel_SkillOverview.Refresh();
        }

        private void Panel_SelectedSkillDetails_Scroll(object sender, ScrollEventArgs e)
        {
            Panel_SelectedSkillDetails.Refresh();
        }

        void CB_ConditionSelection_DrawItem(object sender, DrawItemEventArgs e)
        {
            string text = CB_ConditionSelection.GetItemText(CB_ConditionSelection.Items[e.Index]);
            string t_text = get_ConditionType_Tooltip_byText(CB_ConditionSelection.GetItemText(CB_ConditionSelection.Items[e.Index]));

            e.DrawBackground();
            using (SolidBrush br = new SolidBrush(e.ForeColor))
            { e.Graphics.DrawString(text, e.Font, br, e.Bounds); }

            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected && CB_ConditionSelection.DroppedDown)
            {
                
                this.toolTip1.Show(t_text, CB_ConditionSelection, CB_ConditionSelection.Bounds.Right,
                    CB_ConditionSelection.Bounds.Top - (CB_ConditionSelection.Bounds.Height*2));
            }
            else
            {
                this.toolTip1.Hide(CB_ConditionSelection);
            }
            
            e.DrawFocusRectangle();
        }

        private string get_ConditionType_Tooltip_byText(string ItemText)
        {
            ConditionType type = (ConditionType) Enum.Parse(typeof (ConditionType), ItemText);

            string tooltip = A_Collection.Presets.Manual.Tooltips.ConditionTypes[type];

            if (tooltip.Length >= 80)
                tooltip = tooltip.Insert(80, "\n");

            return tooltip;
        }

        private void Panel_SkillOverview_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            PressedKey = Keys.None;
        }

        private void Panel_SkillOverview_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            PressedKey = e.KeyCode;
        }

        private void Panel_SelectedSkillDetails_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            PressedKey = Keys.None;
        }

        private void Panel_SelectedSkillDetails_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            PressedKey = e.KeyCode;
        }

        private void Window_SkillEditor_FormClosed(object sender, FormClosedEventArgs e)
        {
            Remove_UnassignedConditions();

            A_Tools.T_ExternalFile.CustomSkillDefinitions.Save(A_Collection.SkillCastConditions.Custom.CustomDefinitions);

            _this = null;
        }

        private void Remove_UnassignedConditions()
        {
            foreach (var definition in A_Collection.SkillCastConditions.Custom.CustomDefinitions)
            {
                var tryGetEmptyAssign = definition.CastConditions.FirstOrDefault(x => x.ConditionGroup == -1);

                if (tryGetEmptyAssign != null)
                    definition.CastConditions.Remove(tryGetEmptyAssign);
            }
        }

        private void remove_PowerNamePrefixed()
        {
            var outdatedEntries =
                A_Collection.SkillCastConditions.Custom.CustomDefinitions.Where(
                    x => x.Power.Name.ToLower().StartsWith("x1_"));

            foreach (var entry in outdatedEntries)
            {
                entry.Power.Name = entry.Power.Name.TrimStart(new char[] {'X', '1', '_'});
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            
            remove_PowerNamePrefixed();

            _this = this;

            typeof(Panel).InvokeMember("DoubleBuffered",
    BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
    null, Panel_SelectedSkillDetails, new object[] { true });

            typeof(Panel).InvokeMember("DoubleBuffered",
    BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
    null, Panel_SkillOverview, new object[] { true });

            Window_ActivePowerView.Get_LatestPowers();

            LBL_CopyInfo.BackColor = Color.Transparent;
            LBL_CopyInfo.AutoSize = true;
            LBL_CopyInfo.Font = new Font(Window_Main._FontCollection.Families[0], (float)8.25, FontStyle.Italic);
            LBL_CopyInfo.ForeColor = Color.Black;
            LBL_CopyInfo.Text = "+ Shift & LeftClick\nCopy an existing Condition OR SkillDefinition";
            LBL_CopyInfo.TextAlign = ContentAlignment.MiddleCenter;

            this.Top = Properties.Settings.Default.D3Helper_MainForm_StartPosition.Y;
            this.Left = Properties.Settings.Default.D3Helper_MainForm_StartPosition.X;

            Panel_SkillOverview.AutoScroll = true;
            Panel_SelectedSkillDetails.AutoScroll = true;

            CB_PowerSelection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            CB_PowerSelection.AutoCompleteMode = AutoCompleteMode.Suggest;
            CB_PowerSelection.AutoCompleteSource = AutoCompleteSource.ListItems;

            CB_SelectedRune.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            CB_SelectedRune.AutoCompleteMode = AutoCompleteMode.Suggest;
            CB_SelectedRune.AutoCompleteSource = AutoCompleteSource.ListItems;

            CB_ConditionSelection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            //CB_ConditionSelection.AutoCompleteMode = AutoCompleteMode.Suggest;
            //CB_ConditionSelection.AutoCompleteSource = AutoCompleteSource.None;
            
            if (_SelectedSkill == null)
            {
                BTN_Update.Visible = false;
                Panel_ConditionEditor.Visible = false;
                BTN_Import.Visible = false;
                BTN_Export.Visible = false;

            }
            if(_SelectedCondition == null)
            {
                BTN_ConditionEdit.Visible = false;
                BTN_ContitionRemove.Visible = false;
            }

            Populate_ComboBox_PowerSelection();
            Populate_ComboBox_ConditionSelection();

            Update_View();

        }
        private void Populate_ComboBox_PowerSelection()
        {
            CB_PowerSelection.Items.Clear();

            foreach (var power in Presets.SkillPowers.AllSkillPowers)
            {
                ComboboxItem NewItem = new ComboboxItem();
                NewItem.Text = power.Name;
                NewItem.Value = power;

                CB_PowerSelection.Items.Add(NewItem);
            }

            if(_SelectedSkill == null)
                CB_PowerSelection.SelectedIndex = 0;
        }
        private void Populate_ComboBox_ConditionSelection()
        {
            foreach (var condition in Enum.GetValues(typeof(ConditionType)).Cast<ConditionType>().OrderBy(x => x.ToString()).ToList())
            {
                ComboboxItem NewItem = new ComboboxItem();
                NewItem.Text = condition.ToString();
                NewItem.Value = condition;
                
                CB_ConditionSelection.Items.Add(NewItem);
                
            }

            CB_ConditionSelection.SelectedIndex = 0;
            
        }
        private void Update_View()
        {
            if (_SelectedSkill != null)
            {
                BTN_Update.Visible = true;
                Panel_ConditionEditor.Visible = true;
                BTN_Import.Visible = false;
                BTN_Export.Visible = false;

            }
            else
            {
                BTN_Update.Visible = false;
                Panel_ConditionEditor.Visible = false;
                BTN_Import.Visible = true;
                BTN_Export.Visible = true;

            }

            Update_PanelSkillOverview();
            Update_PanelSelectedSkillDetails();
        }
        private void Update_PanelSkillOverview()
        {
            Panel_SkillOverview.Controls.Clear();

            for(int i = 0; i < SkillCastConditions.Custom.CustomDefinitions.Count; i++)
            {
                SkillData Data = SkillCastConditions.Custom.CustomDefinitions[i];

                Button NewSkill = new Button();
                NewSkill.AutoSize = true;
                NewSkill.MouseClick += SkillDefinition_Click;
                NewSkill.Text = Data.Name;
                NewSkill.TextAlign = ContentAlignment.MiddleLeft;
                NewSkill.Name = Data.Power.PowerSNO.ToString();
                NewSkill.Top = NewSkill.Bottom * i;

                Panel_SkillOverview.Controls.Add(NewSkill);
            }

            Mark_SelectedSkill();
        }
        private void Mark_SelectedSkill()
        {
            if(_SelectedSkill != null)
            {
                Button _selected = Panel_SkillOverview.Controls.OfType<Button>().FirstOrDefault(x => x.Text == _SelectedSkill.Name);

                _selected.BackColor = Color.DarkGray;
            }
        }
        private void Mark_SelectedCondition()
        {
            if (_SelectedCondition != null)
            {
                var buttonsOfType =
                    Panel_SelectedSkillDetails.Controls.OfType<Button>()
                        .Where(x => x.Text.Contains(_SelectedCondition.Type.ToString()));

                var selected = buttonsOfType.FirstOrDefault(x => x.Name.Split('|').Count() > 1 && x.Name.Split('|')[0] == _SelectedCondition.ConditionGroup.ToString() && x.Name.Split('|')[1] == _SelectedCondition.Values[0].ToString());
                if (selected != null)
                {
                    Button _selected = selected;

                    _selected.BackColor = Color.DarkGray;
                }
            }
        }
        private void Update_PanelSelectedSkillDetails(bool IsEdit = false)
        {
            
           
            Panel_SelectedSkillDetails.Controls.Clear();

            if (!_CreateNewDefinition)
            {
                if (_SelectedSkill == null)
                    return;

                if (_SelectedCondition != null)
                {
                    BTN_ConditionEdit.Visible = true;
                    BTN_ContitionRemove.Visible = true;
                }

                BTN_Import.Visible = true;
                BTN_Export.Visible = true;
                BTN_Update.Visible = true;
                Panel_ConditionEditor.Visible = true;
                TB_SkillName.Text = _SelectedSkill.Name;

                ComboboxItem _selection = CB_PowerSelection.Items.OfType<ComboboxItem>()
                        .FirstOrDefault(x => x.Text == _SelectedSkill.Power.Name);

                CB_PowerSelection.SelectedItem = _selection;

                int index = CB_PowerSelection.Items.IndexOf(_selection);
                CB_PowerSelection.SelectedIndex = index;
                
                CB_SelectedRune.SelectedItem =
                    CB_SelectedRune.Items.OfType<ComboboxItem>()
                        .FirstOrDefault(x => x.Text == _SelectedSkill.SelectedRune.Name);


                //-- Add Skill Image
                Image icon =
                    Properties.Resources.ResourceManager.GetObject(_SelectedSkill.Power.Name.ToLower()) as Image;



                Button SkillIcon = new Button();
                SkillIcon.Name = "SkillIcon";
                SkillIcon.Width = icon.Width;
                SkillIcon.Height = icon.Height;
                SkillIcon.FlatStyle = FlatStyle.Flat;
                SkillIcon.BackgroundImage = icon;
                SkillIcon.FlatAppearance.BorderSize = 0;
                SkillIcon.Left = SkillIcon.Left + 10;

                Panel_SelectedSkillDetails.Controls.Add(SkillIcon);
                //
                //-- Add Info Label
                SkillPower PowerFromFile =
                    A_Collection.Presets.SkillPowers.AllSkillPowers.FirstOrDefault(
                        x => x.PowerSNO == _SelectedSkill.Power.PowerSNO);
                if (PowerFromFile != null)
                {
                    Label Info = new Label();
                    Info.Font = new Font(Window_Main._FontCollection.Families[0], (float) 8.3, FontStyle.Bold);
                    Info.ForeColor = Color.Black;
                    Info.AutoSize = true;
                    Info.Top =
                        Panel_SelectedSkillDetails.Controls.OfType<Button>()
                            .FirstOrDefault(x => x.Name == "SkillIcon")
                            .Top;
                    Info.Left =
                        Panel_SelectedSkillDetails.Controls.OfType<Button>()
                            .FirstOrDefault(x => x.Name == "SkillIcon")
                            .Right;


                    if (PowerFromFile.IsCooldownSpell)
                        Info.Text += "This is a COOLDOWN Skill!" + System.Environment.NewLine +
                                     "Add Player_Skill_IsNotOnCooldown!" + System.Environment.NewLine;
                    if (PowerFromFile.ResourceCost < -1 || PowerFromFile.ResourceCost > 0)
                        Info.Text += "This Skill requires RESOURCE!" + System.Environment.NewLine +
                                     "Add atleast Player_Skill_MinResource!" +
                                     System.Environment.NewLine;

                    Panel_SelectedSkillDetails.Controls.Add(Info);
                }
                //
                //-- load stored Conditions

                _SelectedSkill.CastConditions = SortGroup(_SelectedSkill.CastConditions);

                foreach (var condition in _SelectedSkill.CastConditions)
                {
                    Button NewCondition = new Button();
                    NewCondition.AutoSize = true;
                    NewCondition.MouseClick += Condition_Click;
                    NewCondition.MouseMove += Condition_onMouseMove;
                    NewCondition.MouseDown += Condition_onMouseDown;
                    NewCondition.MouseUp += Condition_onMouseUp;

                    //-- add Tooltip to ConditonButtons which have a PowerSNO Value input
                    if (condition.ValueNames.Contains(ConditionValueName.PowerSNO))
                    {
                        var defaultPower =
                            A_Collection.Presets.SNOPowers.AllPowers.FirstOrDefault(
                                x => x.Key == condition.Values.First());
                        var customPower =
                            A_Collection.Presets.SNOPowers.CustomPowerNames.FirstOrDefault(
                                x => x.Key == condition.Values.First());

                        string text = "";

                        if (customPower.Value != null)
                            text = customPower.Value;
                        else if (defaultPower.Value != null)
                            text = defaultPower.Value;

                        ToolTip t = new ToolTip();
                        t.SetToolTip(NewCondition, text);
                    }
                    //

                    NewCondition.Name = condition.ConditionGroup.ToString() + "|";

                    for (int i = 0; i < condition.Values.Count(); i++)
                    {
                        NewCondition.Name += condition.Values[i].ToString() + "|";
                    }

                    NewCondition.Name = NewCondition.Name.TrimEnd('|');

                    NewCondition.Text = condition.Type.ToString();

                    //Set Bounds

                    if (condition.ConditionGroup != -1)
                    {
                        var tryGetExisting =
                            Panel_SelectedSkillDetails.Controls.OfType<Button>()
                                .Where(x => x.Name.Split('|')[0].Contains(condition.ConditionGroup.ToString()));

                        if (tryGetExisting.Count() == 0)
                        {
                            NewCondition.Top = Panel_SelectedSkillDetails.Controls.OfType<Button>().Last().Bottom + 20;
                            NewCondition.Left = Panel_SelectedSkillDetails.Controls.OfType<Button>().First().Left;

                        }
                        else
                        {
                            NewCondition.Top = tryGetExisting.Last().Top;
                            NewCondition.Left = tryGetExisting.Last().Right;

                            // Update Bounds if outside the panel

                            if (
                                !Panel_SelectedSkillDetails.ClientRectangle.Contains(NewCondition.Right,
                                    NewCondition.Top))
                            {
                                NewCondition.Top = Panel_SelectedSkillDetails.Controls.OfType<Button>().Last().Bottom;
                                NewCondition.Left = Panel_SelectedSkillDetails.Controls.OfType<Button>().First().Left;
                            }

                            //
                        }


                    }
                    else
                    {
                        NewCondition.Top = Panel_SelectedSkillDetails.Bottom - NewCondition.Height - 15;
                        NewCondition.Left = Panel_SelectedSkillDetails.Left;
                        NewCondition.BackColor = Color.LightGreen;
                        NewCondition.FlatStyle = FlatStyle.Flat;


                        Label DragMe = new Label();
                        DragMe.AutoSize = true;
                        DragMe.Font = new Font(Window_Main._FontCollection.Families[0], (float) 9, FontStyle.Bold);
                        DragMe.Text = "Drag and Drop!";
                        DragMe.ForeColor = Color.Green;
                        DragMe.Left = NewCondition.Left + 5;
                        DragMe.Top = NewCondition.Top - 15;


                        Panel_SelectedSkillDetails.Controls.Add(DragMe);
                    }
                    //

                    Panel_SelectedSkillDetails.Controls.Add(NewCondition);

                }
                //
                //-- set Group Split Labels
                var groups =
                    Panel_SelectedSkillDetails.Controls.OfType<Button>()
                        .Where(x => x.Name != "SkillIcon" && x.Name.Split('|')[0] != "-1")
                        .GroupBy(x => int.Parse(x.Name.Split('|')[0]))
                        .ToList();

                if (groups.Count() > 1)
                {
                    for (int i = 0; i < groups.Count() - 1; i++)
                    {
                        var minLeft = groups[i].OrderByDescending(x => x.Left).Last().Left;
                        var maxRight = groups[i].OrderByDescending(x => x.Right).First().Right;
                        var minTop = groups[i].OrderByDescending(x => x.Top).Last().Top;
                        var maxBottom = groups[i].OrderByDescending(x => x.Bottom).First().Bottom;

                        Label OrSplit = new Label();
                        OrSplit.AutoSize = true;
                        OrSplit.BackColor = Color.Transparent;
                        OrSplit.Text = "OR";
                        OrSplit.Font = new Font(Window_Main._FontCollection.Families[0], (float) 8.25, FontStyle.Bold);
                        OrSplit.Top = maxBottom + 3;
                        OrSplit.Left = minLeft + ((maxRight - minLeft)/2) - 12;

                        Panel_SelectedSkillDetails.Controls.Add(OrSplit);
                    }
                }
                //
            }
            else
            {
                string PowerName = A_Collection.Presets.SkillPowers.AllSkillPowers.FirstOrDefault(x => x.PowerSNO == _NewDefinitionPowerSNO).Name;

                //Populate_ComboBox_PowerSelection();

                TB_SkillName.Text = PowerName;

                ComboboxItem _selection = CB_PowerSelection.Items.OfType<ComboboxItem>()
                        .FirstOrDefault(x => x.Text == PowerName);

                CB_PowerSelection.SelectedItem = _selection;

                int index = CB_PowerSelection.Items.IndexOf(_selection);
                CB_PowerSelection.SelectedIndex = index;

                _CreateNewDefinition = false;
            }

        }

        private static List<CastCondition> SortGroup(
           List<CastCondition> ConditionGroup)
        {
            var Buffer = ConditionGroup.ToList();

            if (ConditionGroup.FirstOrDefault(x => x.Type.ToString().Contains("Property")) == null)
                return ConditionGroup;

                if (ConditionGroup.Last().Type.ToString().Contains("Property"))
                return ConditionGroup; 

                var Properties = ConditionGroup.Where(x => x.Type.ToString().Contains("Property")).ToList();

                if (Properties.Count() > 0)
                {
                ConditionGroup.RemoveAll(x => x.Type.ToString().Contains("Property"));

                ConditionGroup.AddRange(Properties);
                }
           

            return ConditionGroup;
        }

        private void Condition_Copy()
        {

            Button _this = _clickedCondition;

                ConditionType Type = (ConditionType) Enum.Parse(typeof (ConditionType), _this.Text.Split(' ').Last());

                List<double> Values = new List<double>();
                string[] Splits = _this.Name.Split('|');

                for (int i = 1; i < Splits.Length; i++)
                {
                    Values.Add(double.Parse(Splits[i]));
                }

                _SelectedSkill.CastConditions.Add(new CastCondition(-1, Type, Values.ToArray(),
                    A_Collection.Presets.DefaultCastConditions._Default_CastConditions.FirstOrDefault(
                        x => x.Type == Type).ValueNames));

                Update_PanelSelectedSkillDetails();
            Mark_SelectedCondition();

                
        }
        private void Condition_onMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                MouseDownLocation = e.Location;
                
            }
        }
        private void Condition_onMouseUp(object sender, MouseEventArgs e)
        {
            if (IsMoving)
                IsMoving = false;

            Button _this = sender as Button;

            MovingCondition_AddToHoveredGroup(Panel_SelectedSkillDetails, _this);

        }
        private void Condition_onMouseMove(object sender, MouseEventArgs e)
        {
            IsMoving = true;
            
            Button _this = sender as Button;

            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                Panel_SelectedSkillDetails.SuspendLayout();

                _this.Left = e.X + _this.Left - MouseDownLocation.X;
                _this.Top = e.Y + _this.Top - MouseDownLocation.Y;
                
                Panel_SelectedSkillDetails.Update();
                Panel_SelectedSkillDetails.ResumeLayout();
                
            }
        }
        private void MovingCondition_AddToHoveredGroup(Panel Panel, Button MovingObject)
        {
            Button HoveredButton =
                Panel.Controls.OfType<Button>()
                    .FirstOrDefault(x =>
                    x.Bounds.Contains(MovingObject.Bounds.Left, MovingObject.Bounds.Top) ||
                    x.Bounds.Contains(MovingObject.Bounds.Left, MovingObject.Bounds.Bottom) ||
                    x.Bounds.Contains(MovingObject.Bounds.Right, MovingObject.Bounds.Top) ||
                    x.Bounds.Contains(MovingObject.Bounds.Right, MovingObject.Bounds.Bottom) && x != MovingObject && x.Name != "SkillIcon");

            if (HoveredButton != null)
            {
                var SplitHovered = HoveredButton.Name.Split('|');

                int group = int.Parse(SplitHovered[0]);

                var SplitMoving = MovingObject.Name.Split('|');

                int movinggroup = int.Parse(SplitMoving[0]);

                ConditionType Type = (ConditionType) Enum.Parse(typeof (ConditionType), MovingObject.Text);

                List<double> Values = new List<double>();

                for (int i = 1; i < SplitMoving.Length; i++)
                {
                    Values.Add(double.Parse(SplitMoving[i]));
                }


                var MovingCondition =
                    _SelectedSkill.CastConditions.FirstOrDefault(
                        x => x.ConditionGroup == movinggroup && x.Type == Type);

                if (MovingCondition != null)
                {
                    MovingCondition.ConditionGroup = group;
                }


            }
            else
            {

                var tryGet = _SelectedSkill.CastConditions.Where(x => x.ConditionGroup != -1).OrderByDescending(x => x.ConditionGroup).FirstOrDefault();

                int group = -1;

                if (tryGet != null)
                    group = tryGet.ConditionGroup + 1;
                else
                {
                    group = 0;
                }

                var SplitMoving = MovingObject.Name.Split('|');

                int movinggroup = int.Parse(SplitMoving[0]);

                ConditionType Type = (ConditionType) Enum.Parse(typeof (ConditionType), MovingObject.Text);

                if (group == -1)
                {
                    var MovingCondition =
                        _SelectedSkill.CastConditions.FirstOrDefault(
                            x => x.ConditionGroup == movinggroup && x.Type == Type);

                    if(MovingCondition != null)
                        MovingCondition.ConditionGroup = group;
                }
                else
                {
                    var MovingCondition =
                        _SelectedSkill.CastConditions.FirstOrDefault(
                            x => x.ConditionGroup == movinggroup && x.Type == Type);

                    if(MovingCondition != null)
                        MovingCondition.ConditionGroup = group;
                }

            }

            Update_PanelSelectedSkillDetails();
            Mark_SelectedCondition();
        }
        
        private void Condition_Click(object sender, MouseEventArgs e)
        {
            _clickedCondition = sender as Button;

            
            if(e.Button == MouseButtons.Left && PressedKey != Keys.ShiftKey)
                Condition_Select();
            else if(e.Button == MouseButtons.Left && PressedKey == Keys.ShiftKey)
                Condition_Copy();


        }

        private void Condition_Select()
        {
            
            
                Button _this = _clickedCondition;

                if (_this != null)
                {
                    ConditionType Type =
                        (ConditionType) Enum.Parse(typeof (ConditionType), _this.Text);
                
                    List<double> Values = new List<double>();
                    string[] Splits = _this.Name.Split('|');

                    int group = int.Parse(Splits[0]);

                    for (int i = 1; i < Splits.Length; i++)
                    {
                        Values.Add(double.Parse(Splits[i]));
                    }

                    if (Values.Count == 1)
                    {
                        _SelectedCondition =
                            _SelectedSkill.CastConditions.FirstOrDefault(x => x.Type == Type && x.ConditionGroup == group && x.Values[0] == Values[0]);
                    }
                    else if (Values.Count == 2)
                    {
                        _SelectedCondition =
                            _SelectedSkill.CastConditions.FirstOrDefault(
                                x => x.Type == Type && x.ConditionGroup == group && x.Values[0] == Values[0] && x.Values[1] == Values[1]);
                    }
                    else if (Values.Count == 3)
                    {
                        _SelectedCondition =
                            _SelectedSkill.CastConditions.FirstOrDefault(
                                x =>
                                    x.Type == Type && x.ConditionGroup == group && x.Values[0] == Values[0] && x.Values[1] == Values[1] &&
                                    x.Values[2] == Values[2]);
                    }
                else if (Values.Count == 5)
                {
                    _SelectedCondition =
                        _SelectedSkill.CastConditions.FirstOrDefault(
                            x =>
                                x.Type == Type && x.ConditionGroup == group && x.Values[0] == Values[0] && x.Values[1] == Values[1] &&
                                x.Values[2] == Values[2] && x.Values[3] == Values[3] && x.Values[4] == Values[4]);
                }


                var SelectionItem =
                        CB_ConditionSelection.Items.OfType<ComboboxItem>()
                            .ToList()
                            .FirstOrDefault(x => x.Text == Type.ToString());
                    CB_ConditionSelection.SelectedItem = SelectionItem;

                    Update_PanelSelectedSkillDetails(true);
                    Load_ConditionValues();
                    Mark_SelectedCondition();
                
            }
            
        }

        private void CB_PowerSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboboxItem SelectedItem = CB_PowerSelection.SelectedItem as ComboboxItem;
            if (SelectedItem != null)
            {

                SkillPower Power = SelectedItem.Value as SkillPower;

                CB_SelectedRune.Items.Clear();

                foreach (var rune in Power.Runes)
                {
                    ComboboxItem NewItem = new ComboboxItem();
                    NewItem.Text = rune.Name;
                    NewItem.Value = rune;

                    CB_SelectedRune.Items.Add(NewItem);
                }

                CB_SelectedRune.SelectedIndex = 0;
                CB_PowerSelection.Update();
            }
        }
        private void BTN_Add_Click(object sender, EventArgs e)
        {
            if (TB_SkillName.Text.Length < 3)
            {
                MessageBox.Show("Name too short. Must contain atleast 3 chars!");
                return;
            }
            var tryGetEntry = SkillCastConditions.Custom.CustomDefinitions.FirstOrDefault(x => x.Name == TB_SkillName.Text);
            if(tryGetEntry != null)
            {
                MessageBox.Show("Name already exists. Please choose another!");
                return;
            }

            ComboboxItem SelectedPower = CB_PowerSelection.SelectedItem as ComboboxItem;
            ComboboxItem SelectedRune = CB_SelectedRune.SelectedItem as ComboboxItem;

            string SkillDefinitionName = TB_SkillName.Text;
            SkillPower Power = SelectedPower.Value as SkillPower;
            Rune _Rune = SelectedRune.Value as Rune;

            SkillCastConditions.Custom.CustomDefinitions.Add(new SkillData(Power, SkillDefinitionName, _Rune, new List<CastCondition>()));

            _SelectedSkill = null;
            TB_SkillName.Text = "";
            
            Update_View();
        }
        private void SkillDefinition_Click(object sender, MouseEventArgs e)
        {
            _clickedSkill = sender as Button;


            if (e.Button == MouseButtons.Left && PressedKey != Keys.ShiftKey)
                Skill_Select();
            else if (e.Button == MouseButtons.Left && PressedKey == Keys.ShiftKey)
                Skill_Copy();
        }

        private void Skill_Select()
        {
            Button _this = _clickedSkill;

            SkillData _selected = SkillCastConditions.Custom.CustomDefinitions.FirstOrDefault(x => x.Name == _this.Text && x.Power.PowerSNO == int.Parse(_this.Name));

            _SelectedSkill = _selected;
            _SelectedCondition = null;

            BTN_ConditionEdit.Visible = false;
            BTN_ContitionRemove.Visible = false;

            Update_View();
        }

        private void Skill_Copy()
        {
            SkillData ToCopy =
                A_Collection.SkillCastConditions.Custom.CustomDefinitions.FirstOrDefault(
                    x => x.Name == _clickedSkill.Text);

            int counter = 0;

            string newName = ToCopy.Name + "_" + counter.ToString();

            while (true)
            {
                if (A_Collection.SkillCastConditions.Custom.CustomDefinitions.FirstOrDefault(
                    x => x.Name == newName) == null)
                    break;

                counter++;

                newName = ToCopy.Name + "_" + counter.ToString();
            }

            SkillData NewData = new SkillData(ToCopy.Power, newName, ToCopy.SelectedRune, ToCopy.CastConditions);
            
            A_Collection.SkillCastConditions.Custom.CustomDefinitions.Add(NewData);

            Update_View();
        }
        private void BTN_Update_Click(object sender, EventArgs e)
        {
            
            SkillData _selected = SkillCastConditions.Custom.CustomDefinitions.FirstOrDefault(x => x.Name == _SelectedSkill.Name && x.Power.PowerSNO == _SelectedSkill.Power.PowerSNO);

            ComboboxItem SelectedPower = CB_PowerSelection.SelectedItem as ComboboxItem;
            ComboboxItem SelectedRune = CB_SelectedRune.SelectedItem as ComboboxItem;

            if (TB_SkillName.Text.Length < 3)
            {
                MessageBox.Show("Name too short. Must contain atleast 3 chars!");
                return;
            }
            
            _selected.Name = TB_SkillName.Text;
            _selected.Power = SelectedPower.Value as SkillPower;
            _selected.SelectedRune = SelectedRune.Value as Rune;

            Update_View();
        }
        private void BNT_DeleteSelection_Click(object sender, EventArgs e)
        {
            if (_SelectedSkill != null)
            {
                SkillData _selected = SkillCastConditions.Custom.CustomDefinitions.FirstOrDefault(x => x.Name == _SelectedSkill.Name && x.Power.PowerSNO == _SelectedSkill.Power.PowerSNO);

                SkillCastConditions.Custom.CustomDefinitions.Remove(_selected);

                _SelectedSkill = null;

                Update_View();
            }
        }
        private void CB_ConditionSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            ComboBox _this = sender as ComboBox;
            ComboboxItem _selected = _this.SelectedItem as ComboboxItem;

            ConditionType _type = (ConditionType)_selected.Value;

            CastCondition _default = Presets.DefaultCastConditions._Default_CastConditions.FirstOrDefault(x => x.Type == _type);

            Panel_ConditionEditor_Values.Controls.Clear();

            for(int i = 0; i < _default.Values.Count(); i++)
            {
                if (_default.ValueNames[i] != ConditionValueName.Bool)
                {
                    Label ValueName = new Label();
                    ValueName.Text = _default.ValueNames[i].ToString();
                    ValueName.Top = (Panel_ConditionEditor_Values.Top + 5) + (i*ValueName.Height);

                    Panel_ConditionEditor_Values.Controls.Add(ValueName);

                    TextBox Value = new TextBox();
                    Value.Text = _default.Values[i].ToString();
                    Value.Top = ValueName.Top;
                    Value.Left = ValueName.Right;

                    Panel_ConditionEditor_Values.Controls.Add(Value);
                }
                else
                {
                    CheckBox BoolValue = new CheckBox();
                    BoolValue.Top = (Panel_ConditionEditor_Values.Top + 5) + (i * BoolValue.Height);
                    BoolValue.Checked = true;
                    if (_default.Values[i] == 0)
                        BoolValue.Checked = false;

                    BoolValue.AutoSize = true;
                    BoolValue.CheckedChanged += BoolValue_CheckedChanged;

                    if (
                        _default.Type.ToString().Contains("StandStillTime") ||
                        _default.Type.ToString().Contains("MonstersInRange") || 
                        _default.Type.ToString().Contains("EliteInRange") || 
                        _default.Type.ToString().Contains("BossInRange") ||
                        _default.Type.ToString().Contains("RiftProgress") ||
                        _default.Type.ToString().Contains("PartyMember_InRange")
                        )
                    {
                        BoolValue.Text = "greater then or equal";

                        if (_default.Values[i] == 0)
                            BoolValue.Text = "less then or equal";
                    }

                    Panel_ConditionEditor_Values.Controls.Add(BoolValue);
                }
            }

            
        }

        private void BoolValue_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox _this = sender as CheckBox;

            if (_this.Text == "greater then or equal" || _this.Text == "less then or equal")
            {
                switch (_this.Text)
                {
                    case "greater then or equal":
                        _this.Text = "less then or equal";
                        break;

                    case "less then or equal":
                        _this.Text = "greater then or equal";
                        break;
                }
            }
        }

        private void Load_ConditionValues()
        {
            Panel_ConditionEditor_Values.Controls.Clear();

            for (int i = 0; i < _SelectedCondition.Values.Count(); i++)
            {
                if (_SelectedCondition.ValueNames[i] != ConditionValueName.Bool)
                {
                    Label ValueName = new Label();
                    ValueName.Text = _SelectedCondition.ValueNames[i].ToString();
                    ValueName.Top = (Panel_ConditionEditor_Values.Top + 5) + (i*ValueName.Height);

                    Panel_ConditionEditor_Values.Controls.Add(ValueName);

                    TextBox Value = new TextBox();
                    Value.Text = _SelectedCondition.Values[i].ToString();
                    Value.Top = ValueName.Top;
                    Value.Left = ValueName.Right;

                    Panel_ConditionEditor_Values.Controls.Add(Value);
                }
                else
                {
                    CheckBox BoolValue = new CheckBox();
                    BoolValue.Top = (Panel_ConditionEditor_Values.Top + 5) + (i * BoolValue.Height);
                    BoolValue.Checked = true;
                    if (_SelectedCondition.Values[i] == 0)
                        BoolValue.Checked = false;

                    BoolValue.AutoSize = true;
                    BoolValue.CheckedChanged += BoolValue_CheckedChanged;

                    if (
                        _SelectedCondition.Type.ToString().Contains("StandStillTime") || 
                        _SelectedCondition.Type.ToString().Contains("MonstersInRange") || 
                        _SelectedCondition.Type.ToString().Contains("EliteInRange") || 
                        _SelectedCondition.Type.ToString().Contains("BossInRange") ||
                        _SelectedCondition.Type.ToString().Contains("RiftProgress") ||
                        _SelectedCondition.Type.ToString().Contains("PartyMember_InRange")
                        )
                    {
                        BoolValue.Text = "greater then or equal";

                        if (_SelectedCondition.Values[i] == 0)
                            BoolValue.Text = "less then or equal";
                    }


                    Panel_ConditionEditor_Values.Controls.Add(BoolValue);

                }
            }
        }
        private void BTN_Condition_Add_Click(object sender, EventArgs e)
        {
            if(!Validated_ConditionsInput())
                return;

            if (IsUnassignedConditionLeft())
                return;

            
            var EmptyValueFields = Panel_ConditionEditor_Values.Controls.OfType<TextBox>().FirstOrDefault(x => x.Text.Length == 0);

            if (EmptyValueFields != null)
            { MessageBox.Show("Not all Conditions Values are set. Please enter a Value!"); return; }

            ComboboxItem _selected = CB_ConditionSelection.SelectedItem as ComboboxItem;
            ConditionType Type = (ConditionType)_selected.Value;

            CastCondition _default = Presets.DefaultCastConditions._Default_CastConditions.FirstOrDefault(x => x.Type == Type);
            
            var TextBoxes = Panel_ConditionEditor_Values.Controls.OfType<TextBox>().ToList();
            var CheckBoxes = Panel_ConditionEditor_Values.Controls.OfType<CheckBox>().ToList();

            List<double> Values = new List<double>();

            if (CheckBoxes.Count == 0)
            {
                for (int i = 0; i < _default.Values.Count(); i++)
                {
                    Values.Add(double.Parse(TextBoxes[i].Text));
                }
            }
            else
            {
                if (_default.Type == ConditionType.Add_Property_Channeling)
                {
                    if (CheckBoxes[0].Checked)
                        Values.Add(1);
                    else
                    {
                        Values.Add(0);
                    }

                    Values.Add(double.Parse(TextBoxes[0].Text));
                   
                    
                }
                else if (_default.Type == ConditionType.Player_StandStillTime)
                {
                    Values.Add(double.Parse(TextBoxes[0].Text));

                    if (CheckBoxes[0].Checked)
                        Values.Add(1);
                    else
                    {
                        Values.Add(0);
                    }
                }
                else if (_default.Type == ConditionType.PartyMember_InRangeIsBuff || _default.Type == ConditionType.PartyMember_InRangeIsNotBuff)
                {
                    Values.Add(double.Parse(TextBoxes[0].Text));
                    Values.Add(double.Parse(TextBoxes[1].Text));
                    Values.Add(double.Parse(TextBoxes[2].Text));
                    Values.Add(double.Parse(TextBoxes[3].Text));

                    if (CheckBoxes[0].Checked)
                        Values.Add(1);
                    else
                    {
                        Values.Add(0);
                    }
                }
                else if (_default.Type == ConditionType.MonstersInRange_RiftProgress ||
                    _default.Type == ConditionType.SelectedMonster_MonstersInRange_RiftProgress ||
                    _default.Type == ConditionType.SelectedMonster_RiftProgress)
                {
                    Values.Add(double.Parse(TextBoxes[0].Text));

                    if (CheckBoxes[0].Checked)
                        Values.Add(1);
                    else
                    {
                        Values.Add(0);
                    }
                }
                else if (_default.Type != ConditionType.Add_Property_Channeling &&
                         (!_default.Type.ToString().Contains("MonstersInRange") ||
                          !_default.Type.ToString().Contains("EliteInRange") ||
                          !_default.Type.ToString().Contains("BossInRange")) && _default.ValueNames.Count() < 3)
                {
                    for (int i = 0; i < _default.Values.Count(); i++)
                    {
                        if (CheckBoxes[i].Checked)
                            Values.Add(1);
                        else
                        {
                            Values.Add(0);
                        }
                    }
                }
                else
                {
                    Values.Add(double.Parse(TextBoxes[0].Text));
                    Values.Add(double.Parse(TextBoxes[1].Text));

                    if (CheckBoxes[0].Checked)
                        Values.Add(1);
                    else
                    {
                        Values.Add(0);
                    }
                }
            }

            _SelectedSkill.CastConditions.Add(new CastCondition(-1, Type, Values.ToArray(), _default.ValueNames));
            

            Update_PanelSelectedSkillDetails();
            Mark_SelectedCondition();

        }

        private void BTN_ContitionRemove_Click(object sender, EventArgs e)
        {
            _SelectedSkill.CastConditions.Remove(_SelectedCondition);
            _SelectedCondition = null;

            Update_PanelSelectedSkillDetails();
            Mark_SelectedCondition();
        }

        private bool IsUnassignedConditionLeft()
        {
            var emptyGroup =
                Panel_SelectedSkillDetails.Controls.OfType<Button>()
                    .FirstOrDefault(x => x.Name != "SkillIcon" && int.Parse(x.Name.Split('|')[0]) == -1);

            if (emptyGroup == null)
                return false;
            else
            {
                MessageBox.Show(
                    "There is an unassigned Condition already. Assign it to a ConditionGroup before you add a new Condition");
            }

            return true;
        }
        private bool Validated_ConditionsInput()
        {
            ComboboxItem _selected = CB_ConditionSelection.SelectedItem as ComboboxItem;
            ConditionType Type = (ConditionType)_selected.Value;

            ConditionType TypeRequired;
            ConditionType[] TypesRequired;

            switch (Type)
            {
                case ConditionType.SelectedMonster_MinDistance:
                    TypeRequired = ConditionType.Player_IsMonsterSelected;
                    if (!IsConditionAvailable(TypeRequired))
                    {
                        MessageBox.Show("Add a Conditions of Type (" + TypeRequired.ToString() +
                                        ") before you add this Condition!");
                        return false;
                    }
                    return true;

                case ConditionType.SelectedMonster_MaxDistance:
                    TypeRequired = ConditionType.Player_IsMonsterSelected;
                    if (!IsConditionAvailable(TypeRequired))
                    {
                        MessageBox.Show("Add a Conditions of Type (" + TypeRequired.ToString() +
                                        ") before you add this Condition!");
                        return false;
                    }
                    return true;

                case ConditionType.MonstersInRange_IsBuffActive:
                    TypeRequired = ConditionType.World_MonstersInRange;
                    if (!IsConditionAvailable(TypeRequired))
                    {
                        MessageBox.Show("Add a Conditions of Type (" + TypeRequired.ToString() +
                                        ") before you add this Condition!");
                        return false;
                    }
                    return true;

                case ConditionType.MonstersInRange_IsBuffNotActive:
                    TypeRequired = ConditionType.World_MonstersInRange;
                    if (!IsConditionAvailable(TypeRequired))
                    {
                        MessageBox.Show("Add a Conditions of Type (" + TypeRequired.ToString() +
                                        ") before you add this Condition!");
                        return false;
                    }
                    return true;

                case ConditionType.World_MonstersInRange:
                    return true;

                case ConditionType.SelectedMonster_MonstersInRange:
                    TypeRequired = ConditionType.Player_IsMonsterSelected;
                    if (!IsConditionAvailable(TypeRequired))
                    {
                        MessageBox.Show("Add a Conditions of Type (" + TypeRequired.ToString() +
                                        ") before you add this Condition!");
                        return false;
                    }
                    return true;

                case ConditionType.Player_IsMonsterSelected:
                    return true;

                case ConditionType.PartyMember_InRangeIsBuff:
                    return true;

                case ConditionType.PartyMember_InRangeIsNotBuff:
                    return true;

                case ConditionType.PartyMember_InRangeMinHitpoints:
                    return true;

                case ConditionType.Party_AllInRange:
                    return true;

                case ConditionType.Party_NotAllInRange:
                    return true;

                case ConditionType.Player_BuffTicksLeft:
                    return true;

                case ConditionType.Player_IsBuffActive:
                    return true;

                case ConditionType.Player_IsBuffNotActive:
                    return true;

                case ConditionType.Player_IsBuffCount:
                    return true;

                case ConditionType.Player_IsNotBuffCount:
                    return true;

                case ConditionType.Player_MaxHitpointsPercentage:
                    return true;

                case ConditionType.Player_MinPrimaryResource:
                    return true;

                case ConditionType.Player_MinPrimaryResourcePercentage:
                    return true;

                case ConditionType.Player_MinSecondaryResource:
                    return true;

                case ConditionType.Player_MinSecondaryResourcePercentage:
                    return true;

                case ConditionType.Player_Skill_MinCharges:
                    return true;

                case ConditionType.Player_Skill_MinResource:
                    return true;

                case ConditionType.Player_Skill_IsNotOnCooldown:
                    return true;

                case ConditionType.SelectedMonster_IsBuffActive:
                    TypeRequired = ConditionType.Player_IsMonsterSelected;
                    if (!IsConditionAvailable(TypeRequired))
                    {
                        MessageBox.Show("Add a Conditions of Type (" + TypeRequired.ToString() +
                                        ") before you add this Condition!");
                        return false;
                    }
                    return true;

                case ConditionType.SelectedMonster_IsBuffNotActive:
                    TypeRequired = ConditionType.Player_IsMonsterSelected;
                    if (!IsConditionAvailable(TypeRequired))
                    {
                        MessageBox.Show("Add a Conditions of Type (" + TypeRequired.ToString() +
                                        ") before you add this Condition!");
                        return false;
                    }
                    return true;

                case ConditionType.World_BossInRange:
                    return true;

                case ConditionType.World_EliteInRange:
                    return true;

                case ConditionType.World_IsGRift:
                    return true;

                case ConditionType.World_IsRift:
                    return true;

                case ConditionType.Player_MaxPrimaryResource:
                    return true;

                case ConditionType.Player_MaxSecondaryResource:
                    return true;

                case ConditionType.Player_MaxPrimaryResourcePercentage:
                    return true;

                case ConditionType.Player_MaxSecondaryResourcePercentage:
                    return true;

                case ConditionType.Player_IsMoving:
                    return true;

                case ConditionType.Player_Pet_MinFetishesCount:
                    return true;

                case ConditionType.Player_Pet_MinZombieDogsCount:
                    return true;

                case ConditionType.Player_Pet_MinGargantuanCount:
                    return true;

                case ConditionType.Player_Pet_MaxFetishesCount:
                    return true;

                case ConditionType.Player_Pet_MaxZombieDogsCount:
                    return true;

                case ConditionType.Player_Pet_MaxGargantuanCount:
                    return true;

                case ConditionType.SelectedMonster_IsElite:
                    TypeRequired = ConditionType.Player_IsMonsterSelected;
                    if (!IsConditionAvailable(TypeRequired))
                    {
                        MessageBox.Show("Add a Conditions of Type (" + TypeRequired.ToString() +
                                        ") before you add this Condition!");
                        return false;
                    }
                    return true;

                case ConditionType.SelectedMonster_IsBoss:
                    TypeRequired = ConditionType.Player_IsMonsterSelected;
                    if (!IsConditionAvailable(TypeRequired))
                    {
                        MessageBox.Show("Add a Conditions of Type (" + TypeRequired.ToString() +
                                        ") before you add this Condition!");
                        return false;
                    }
                    return true;

                    case ConditionType.Player_Power_IsNotOnCooldown:
                    return true;

                    case ConditionType.Player_Power_IsOnCooldown:
                        return true;

                    case ConditionType.Player_HasSkillEquipped:
                    return true;

                case ConditionType.MonstersInRange_HaveArcaneEnchanted:
                    TypesRequired = new ConditionType[] {ConditionType.World_MonstersInRange, ConditionType.World_EliteInRange};
                    if (!IsConditionAvailable(TypesRequired))
                    {
                        MessageBox.Show("Add a Conditions of Type (" + TypesRequired[0].ToString() + " or " + TypesRequired[1].ToString() +
                                        ") before you add this Condition!");
                        return false;
                    }
                    return true;

                case ConditionType.MonstersInRange_HaveAvenger:
                    TypesRequired = new ConditionType[] { ConditionType.World_MonstersInRange, ConditionType.World_EliteInRange };
                    if (!IsConditionAvailable(TypesRequired))
                    {
                        MessageBox.Show("Add a Conditions of Type (" + TypesRequired[0].ToString() + " or " + TypesRequired[1].ToString() +
                                        ") before you add this Condition!");
                        return false;
                    }
                    return true;

                case ConditionType.MonstersInRange_HaveDesecrator:
                    TypesRequired = new ConditionType[] { ConditionType.World_MonstersInRange, ConditionType.World_EliteInRange };
                    if (!IsConditionAvailable(TypesRequired))
                    {
                        MessageBox.Show("Add a Conditions of Type (" + TypesRequired[0].ToString() + " or " + TypesRequired[1].ToString() +
                                        ") before you add this Condition!");
                        return false;
                    }
                    return true;

                case ConditionType.MonstersInRange_HaveElectrified:
                    TypesRequired = new ConditionType[] { ConditionType.World_MonstersInRange, ConditionType.World_EliteInRange };
                    if (!IsConditionAvailable(TypesRequired))
                    {
                        MessageBox.Show("Add a Conditions of Type (" + TypesRequired[0].ToString() + " or " + TypesRequired[1].ToString() +
                                        ") before you add this Condition!");
                        return false;
                    }
                    return true;

                case ConditionType.MonstersInRange_HaveExtraHealth:
                    TypesRequired = new ConditionType[] { ConditionType.World_MonstersInRange, ConditionType.World_EliteInRange };
                    if (!IsConditionAvailable(TypesRequired))
                    {
                        MessageBox.Show("Add a Conditions of Type (" + TypesRequired[0].ToString() + " or " + TypesRequired[1].ToString() +
                                        ") before you add this Condition!");
                        return false;
                    }
                    return true;

                case ConditionType.MonstersInRange_HaveFast:
                    TypesRequired = new ConditionType[] { ConditionType.World_MonstersInRange, ConditionType.World_EliteInRange };
                    if (!IsConditionAvailable(TypesRequired))
                    {
                        MessageBox.Show("Add a Conditions of Type (" + TypesRequired[0].ToString() + " or " + TypesRequired[1].ToString() +
                                        ") before you add this Condition!");
                        return false;
                    }
                    return true;

                case ConditionType.MonstersInRange_HaveFrozen:
                    TypesRequired = new ConditionType[] { ConditionType.World_MonstersInRange, ConditionType.World_EliteInRange };
                    if (!IsConditionAvailable(TypesRequired))
                    {
                        MessageBox.Show("Add a Conditions of Type (" + TypesRequired[0].ToString() + " or " + TypesRequired[1].ToString() +
                                        ") before you add this Condition!");
                        return false;
                    }
                    return true;

                case ConditionType.MonstersInRange_HaveHealthlink:
                    TypesRequired = new ConditionType[] { ConditionType.World_MonstersInRange, ConditionType.World_EliteInRange };
                    if (!IsConditionAvailable(TypesRequired))
                    {
                        MessageBox.Show("Add a Conditions of Type (" + TypesRequired[0].ToString() + " or " + TypesRequired[1].ToString() +
                                        ") before you add this Condition!");
                        return false;
                    }
                    return true;

                case ConditionType.MonstersInRange_HaveIllusionist:
                    TypesRequired = new ConditionType[] { ConditionType.World_MonstersInRange, ConditionType.World_EliteInRange };
                    if (!IsConditionAvailable(TypesRequired))
                    {
                        MessageBox.Show("Add a Conditions of Type (" + TypesRequired[0].ToString() + " or " + TypesRequired[1].ToString() +
                                        ") before you add this Condition!");
                        return false;
                    }
                    return true;

                case ConditionType.MonstersInRange_HaveJailer:
                    TypesRequired = new ConditionType[] { ConditionType.World_MonstersInRange, ConditionType.World_EliteInRange };
                    if (!IsConditionAvailable(TypesRequired))
                    {
                        MessageBox.Show("Add a Conditions of Type (" + TypesRequired[0].ToString() + " or " + TypesRequired[1].ToString() +
                                        ") before you add this Condition!");
                        return false;
                    }
                    return true;

                case ConditionType.MonstersInRange_HaveKnockback:
                    TypesRequired = new ConditionType[] { ConditionType.World_MonstersInRange, ConditionType.World_EliteInRange };
                    if (!IsConditionAvailable(TypesRequired))
                    {
                        MessageBox.Show("Add a Conditions of Type (" + TypesRequired[0].ToString() + " or " + TypesRequired[1].ToString() +
                                        ") before you add this Condition!");
                        return false;
                    }
                    return true;

                case ConditionType.MonstersInRange_HaveFirechains:
                    TypesRequired = new ConditionType[] { ConditionType.World_MonstersInRange, ConditionType.World_EliteInRange };
                    if (!IsConditionAvailable(TypesRequired))
                    {
                        MessageBox.Show("Add a Conditions of Type (" + TypesRequired[0].ToString() + " or " + TypesRequired[1].ToString() +
                                        ") before you add this Condition!");
                        return false;
                    }
                    return true;

                case ConditionType.MonstersInRange_HaveMolten:
                    TypesRequired = new ConditionType[] { ConditionType.World_MonstersInRange, ConditionType.World_EliteInRange };
                    if (!IsConditionAvailable(TypesRequired))
                    {
                        MessageBox.Show("Add a Conditions of Type (" + TypesRequired[0].ToString() + " or " + TypesRequired[1].ToString() +
                                        ") before you add this Condition!");
                        return false;
                    }
                    return true;

                case ConditionType.MonstersInRange_HaveMortar:
                    TypesRequired = new ConditionType[] { ConditionType.World_MonstersInRange, ConditionType.World_EliteInRange };
                    if (!IsConditionAvailable(TypesRequired))
                    {
                        MessageBox.Show("Add a Conditions of Type (" + TypesRequired[0].ToString() + " or " + TypesRequired[1].ToString() +
                                        ") before you add this Condition!");
                        return false;
                    }
                    return true;

                case ConditionType.MonstersInRange_HaveNightmarish:
                    TypesRequired = new ConditionType[] { ConditionType.World_MonstersInRange, ConditionType.World_EliteInRange };
                    if (!IsConditionAvailable(TypesRequired))
                    {
                        MessageBox.Show("Add a Conditions of Type (" + TypesRequired[0].ToString() + " or " + TypesRequired[1].ToString() +
                                        ") before you add this Condition!");
                        return false;
                    }
                    return true;

                case ConditionType.MonstersInRange_HavePlagued:
                    TypesRequired = new ConditionType[] { ConditionType.World_MonstersInRange, ConditionType.World_EliteInRange };
                    if (!IsConditionAvailable(TypesRequired))
                    {
                        MessageBox.Show("Add a Conditions of Type (" + TypesRequired[0].ToString() + " or " + TypesRequired[1].ToString() +
                                        ") before you add this Condition!");
                        return false;
                    }
                    return true;

                case ConditionType.MonstersInRange_HaveReflectsDamage:
                    TypesRequired = new ConditionType[] { ConditionType.World_MonstersInRange, ConditionType.World_EliteInRange };
                    if (!IsConditionAvailable(TypesRequired))
                    {
                        MessageBox.Show("Add a Conditions of Type (" + TypesRequired[0].ToString() + " or " + TypesRequired[1].ToString() +
                                        ") before you add this Condition!");
                        return false;
                    }
                    return true;

                case ConditionType.MonstersInRange_HaveShielding:
                    TypesRequired = new ConditionType[] { ConditionType.World_MonstersInRange, ConditionType.World_EliteInRange };
                    if (!IsConditionAvailable(TypesRequired))
                    {
                        MessageBox.Show("Add a Conditions of Type (" + TypesRequired[0].ToString() + " or " + TypesRequired[1].ToString() +
                                        ") before you add this Condition!");
                        return false;
                    }
                    return true;

                case ConditionType.MonstersInRange_HaveTeleporter:
                    TypesRequired = new ConditionType[] { ConditionType.World_MonstersInRange, ConditionType.World_EliteInRange };
                    if (!IsConditionAvailable(TypesRequired))
                    {
                        MessageBox.Show("Add a Conditions of Type (" + TypesRequired[0].ToString() + " or " + TypesRequired[1].ToString() +
                                        ") before you add this Condition!");
                        return false;
                    }
                    return true;

                case ConditionType.MonstersInRange_HaveThunderstorm:
                    TypesRequired = new ConditionType[] { ConditionType.World_MonstersInRange, ConditionType.World_EliteInRange };
                    if (!IsConditionAvailable(TypesRequired))
                    {
                        MessageBox.Show("Add a Conditions of Type (" + TypesRequired[0].ToString() + " or " + TypesRequired[1].ToString() +
                                        ") before you add this Condition!");
                        return false;
                    }
                    return true;

                case ConditionType.MonstersInRange_HaveVortex:
                    TypesRequired = new ConditionType[] { ConditionType.World_MonstersInRange, ConditionType.World_EliteInRange };
                    if (!IsConditionAvailable(TypesRequired))
                    {
                        MessageBox.Show("Add a Conditions of Type (" + TypesRequired[0].ToString() + " or " + TypesRequired[1].ToString() +
                                        ") before you add this Condition!");
                        return false;
                    }
                    return true;

                case ConditionType.MonstersInRange_HaveWaller:
                    TypesRequired = new ConditionType[] { ConditionType.World_MonstersInRange, ConditionType.World_EliteInRange };
                    if (!IsConditionAvailable(TypesRequired))
                    {
                        MessageBox.Show("Add a Conditions of Type (" + TypesRequired[0].ToString() + " or " + TypesRequired[1].ToString() +
                                        ") before you add this Condition!");
                        return false;
                    }
                    return true;

                case ConditionType.Player_HasSkillNotEquipped:
                    return true;

                    case ConditionType.SelectedMonster_IsBuffCount:
                    TypeRequired = ConditionType.Player_IsMonsterSelected;
                    if (!IsConditionAvailable(TypeRequired))
                    {
                        MessageBox.Show("Add a Conditions of Type (" + TypeRequired.ToString() +
                                        ") before you add this Condition!");
                        return false;
                    }
                    return true;

                    case ConditionType.SelectedMonster_IsNotBuffCount:
                    TypeRequired = ConditionType.Player_IsMonsterSelected;
                    if (!IsConditionAvailable(TypeRequired))
                    {
                        MessageBox.Show("Add a Conditions of Type (" + TypeRequired.ToString() +
                                        ") before you add this Condition!");
                        return false;
                    }
                    return true;

                    case ConditionType.MonstersInRange_IsBuffCount:
                    TypeRequired = ConditionType.World_MonstersInRange;
                    if (!IsConditionAvailable(TypeRequired))
                    {
                        MessageBox.Show("Add a Conditions of Type (" + TypeRequired.ToString() +
                                        ") before you add this Condition!");
                        return false;
                    }
                    return true;

                    case ConditionType.MonstersInRange_IsNotBuffCount:
                    TypeRequired = ConditionType.World_MonstersInRange;
                    if (!IsConditionAvailable(TypeRequired))
                    {
                        MessageBox.Show("Add a Conditions of Type (" + TypeRequired.ToString() +
                                        ") before you add this Condition!");
                        return false;
                    }
                    return true;

                    case ConditionType.Player_IsDestructableSelected:
                    return true;

                    case ConditionType.Key_ForceStandStill:
                    return true;

                    case ConditionType.Add_Property_TimedUse:
                    return true;

                case ConditionType.SelectedMonster_MonstersInRange_IsBuffActive:
                    TypeRequired = ConditionType.SelectedMonster_MonstersInRange;
                    if (!IsConditionAvailable(TypeRequired))
                    {
                        MessageBox.Show("Add a Conditions of Type (" + TypeRequired.ToString() +
                                        ") before you add this Condition!");
                        return false;
                    }
                    return true;

                case ConditionType.SelectedMonster_MonstersInRange_IsBuffNotActive:
                    TypeRequired = ConditionType.SelectedMonster_MonstersInRange;
                    if (!IsConditionAvailable(TypeRequired))
                    {
                        MessageBox.Show("Add a Conditions of Type (" + TypeRequired.ToString() +
                                        ") before you add this Condition!");
                        return false;
                    }
                    return true;

                    case ConditionType.Player_MinAPS:
                    return true;

                    case ConditionType.Add_Property_Channeling:
                    return true;

                    case ConditionType.Add_Property_APSSnapShot:
                    TypeRequired = ConditionType.Player_MinAPS;
                    if (!IsConditionAvailable(TypeRequired))
                    {
                        MessageBox.Show("Add a Conditions of Type (" + TypeRequired.ToString() +
                                        ") before you add this Condition!");
                        return false;
                    }
                    return true;

                    case ConditionType.MonstersInRange_MinHitpointsPercentage:
                    TypeRequired = ConditionType.World_MonstersInRange;
                    if (!IsConditionAvailable(TypeRequired))
                    {
                        MessageBox.Show("Add a Conditions of Type (" + TypeRequired.ToString() +
                                        ") before you add this Condition!");
                        return false;
                    }
                    return true;

                    case ConditionType.MonstersInRange_MaxHitpointsPercentage:
                    TypeRequired = ConditionType.World_MonstersInRange;
                    if (!IsConditionAvailable(TypeRequired))
                    {
                        MessageBox.Show("Add a Conditions of Type (" + TypeRequired.ToString() +
                                        ") before you add this Condition!");
                        return false;
                    }
                    return true;

                    case ConditionType.SelectedMonster_MonstersInRange_MinHitpointsPercentage:
                    TypeRequired = ConditionType.SelectedMonster_MonstersInRange;
                    if (!IsConditionAvailable(TypeRequired))
                    {
                        MessageBox.Show("Add a Conditions of Type (" + TypeRequired.ToString() +
                                        ") before you add this Condition!");
                        return false;
                    }
                    return true;

                case ConditionType.SelectedMonster_MonstersInRange_MaxHitpointsPercentage:
                    TypeRequired = ConditionType.SelectedMonster_MonstersInRange;
                    if (!IsConditionAvailable(TypeRequired))
                    {
                        MessageBox.Show("Add a Conditions of Type (" + TypeRequired.ToString() +
                                        ") before you add this Condition!");
                        return false;
                    }
                    return true;

                    case ConditionType.SelectedMonster_MinHitpointsPercentage:
                    TypeRequired = ConditionType.Player_IsMonsterSelected;
                    if (!IsConditionAvailable(TypeRequired))
                    {
                        MessageBox.Show("Add a Conditions of Type (" + TypeRequired.ToString() +
                                        ") before you add this Condition!");
                        return false;
                    }
                    return true;

                case ConditionType.SelectedMonster_MaxHitpointsPercentage:
                    TypeRequired = ConditionType.Player_IsMonsterSelected;
                    if (!IsConditionAvailable(TypeRequired))
                    {
                        MessageBox.Show("Add a Conditions of Type (" + TypeRequired.ToString() +
                                        ") before you add this Condition!");
                        return false;
                    }
                    return true;

                    case ConditionType.Player_StandStillTime:
                    return true;

                case ConditionType.MonstersInRange_RiftProgress:
                    TypeRequired = ConditionType.World_MonstersInRange;
                    if (!IsConditionAvailable(TypeRequired))
                    {
                        MessageBox.Show("Add a Conditions of Type (" + TypeRequired.ToString() +
                                        ") before you add this Condition!");
                        return false;
                    }
                    return true;

                case ConditionType.SelectedMonster_MonstersInRange_RiftProgress:
                    TypeRequired = ConditionType.SelectedMonster_MonstersInRange;
                    if (!IsConditionAvailable(TypeRequired))
                    {
                        MessageBox.Show("Add a Conditions of Type (" + TypeRequired.ToString() +
                                        ") before you add this Condition!");
                        return false;
                    }
                    return true;

                case ConditionType.SelectedMonster_RiftProgress:
                    TypeRequired = ConditionType.Player_IsMonsterSelected;
                    if (!IsConditionAvailable(TypeRequired))
                    {
                        MessageBox.Show("Add a Conditions of Type (" + TypeRequired.ToString() +
                                        ") before you add this Condition!");
                        return false;
                    }
                    return true;

                default:
                    MessageBox.Show(
                        "No Validation Definition added for the selected Condition. Please send a BugReport!");
                    return false;
            }

        }
        private bool IsConditionAvailable(ConditionType Type)
        {
            foreach (var button in Panel_SelectedSkillDetails.Controls.OfType<Button>().Where(x => x.Name != "SkillIcon"))
            {
                ConditionType _Type = (ConditionType)Enum.Parse(typeof(ConditionType), button.Text.Split(' ').Last());

                if (_Type == Type)
                    return true;
            }
            return false;
        }
        private bool IsConditionAvailable(ConditionType[] Types)
        {
            foreach (var button in Panel_SelectedSkillDetails.Controls.OfType<Button>().Where(x => x.Name != "SkillIcon"))
            {
                ConditionType _Type = (ConditionType)Enum.Parse(typeof(ConditionType), button.Text.Split(' ').Last());

                if (Types.Contains(_Type))
                    return true;
            }
            return false;
        }
        private void BTN_ConditionEdit_Click(object sender, EventArgs e)
        {
            if (_SelectedCondition != null)
            {
                if (!Validated_ConditionsInput())
                    return;

                var EmptyValueFields =
                    Panel_ConditionEditor_Values.Controls.OfType<TextBox>().FirstOrDefault(x => x.Text.Length == 0);

                if (EmptyValueFields != null)
                {
                    MessageBox.Show("Not all Conditions Values are set. Please enter a Value!");
                    return;
                }

                ComboboxItem _selected = CB_ConditionSelection.SelectedItem as ComboboxItem;
                ConditionType Type = (ConditionType) _selected.Value;

                var _default =
                    A_Collection.Presets.DefaultCastConditions._Default_CastConditions.FirstOrDefault(
                        x => x.Type == Type);

                var TextBoxes = Panel_ConditionEditor_Values.Controls.OfType<TextBox>().ToList();
                var CheckBoxes = Panel_ConditionEditor_Values.Controls.OfType<CheckBox>().ToList();

                List<double> Values = new List<double>();

                if (CheckBoxes.Count == 0)
                {
                    for (int i = 0; i < TextBoxes.Count(); i++)
                    {
                        Values.Add(double.Parse(TextBoxes[i].Text));
                    }
                }
                else
                {
                    if (_default.Type == ConditionType.Add_Property_Channeling)
                    {
                        if (CheckBoxes[0].Checked)
                            Values.Add(1);
                        else
                        {
                            Values.Add(0);
                        }

                        Values.Add(double.Parse(TextBoxes[0].Text));


                    }
                    else if (_default.Type == ConditionType.PartyMember_InRangeIsBuff || _default.Type == ConditionType.PartyMember_InRangeIsNotBuff)
                    {
                        Values.Add(double.Parse(TextBoxes[0].Text));
                        Values.Add(double.Parse(TextBoxes[1].Text));
                        Values.Add(double.Parse(TextBoxes[2].Text));
                        Values.Add(double.Parse(TextBoxes[3].Text));

                        if (CheckBoxes[0].Checked)
                            Values.Add(1);
                        else
                        {
                            Values.Add(0);
                        }
                    }
                    else if (_default.Type == ConditionType.Player_StandStillTime)
                    {
                        Values.Add(double.Parse(TextBoxes[0].Text));

                        if (CheckBoxes[0].Checked)
                            Values.Add(1);
                        else
                        {
                            Values.Add(0);
                        }




                    }
                    else if (_default.Type == ConditionType.MonstersInRange_RiftProgress ||
                    _default.Type == ConditionType.SelectedMonster_MonstersInRange_RiftProgress ||
                    _default.Type == ConditionType.SelectedMonster_RiftProgress)
                    {
                        Values.Add(double.Parse(TextBoxes[0].Text));

                        if (CheckBoxes[0].Checked)
                            Values.Add(1);
                        else
                        {
                            Values.Add(0);
                        }
                    }
                    else if (_default.Type != ConditionType.Add_Property_Channeling && (!Type.ToString().Contains("MonstersInRange") || !Type.ToString().Contains("EliteInRange") || !Type.ToString().Contains("BossInRange")) && _default.ValueNames.Count() < 3)
                    {
                        for (int i = 0; i < _default.Values.Count(); i++)
                        {
                            if (i < CheckBoxes.Count)
                            {
                                if (CheckBoxes[i].Checked)
                                    Values.Add(1);
                                else
                                {
                                    Values.Add(0);
                                }
                            }
                            
                        }
                    }
                    else
                    {
                        Values.Add(double.Parse(TextBoxes[0].Text));
                        Values.Add(double.Parse(TextBoxes[1].Text));

                        if (CheckBoxes[0].Checked)
                            Values.Add(1);
                        else
                        {
                            Values.Add(0);
                        }
                    }
                }

                _SelectedCondition.Type = Type;
                _SelectedCondition.Values = Values.ToArray();
                _SelectedCondition.ValueNames =
                    A_Collection.Presets.DefaultCastConditions._Default_CastConditions.First(x => x.Type == Type)
                        .ValueNames;

                Update_PanelSelectedSkillDetails();
                Mark_SelectedCondition();
            }
        }
        
        private void CB_SelectedRune_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void BTN_Export_Click(object sender, EventArgs e)
        {
            if (Window_ImportExport._this == null)
            {
                Window_ImportExport IE = new Window_ImportExport();
                IE._SelectedSkill = _SelectedSkill;
                IE.isExport = true;
                IE.isImport = false;
                IE.Show();
            }
        }

        private void BTN_Import_Click(object sender, EventArgs e)
        {
            if (Window_ImportExport._this == null)
            {
                Window_ImportExport IE = new Window_ImportExport();
                IE._SelectedSkill = _SelectedSkill;
                IE.isExport = false;
                IE.isImport = true;
                IE.Closed += IE_AfterImport;
                IE.Show();
            }
        }

        private void IE_AfterImport(object sender, EventArgs e)
        {
            A_Tools.T_ExternalFile.CustomSkillDefinitions.Fix();
            Update_PanelSelectedSkillDetails();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Window_ActivePowerView._this == null)
            {
                Window_ActivePowerView APV = new Window_ActivePowerView();
                APV.Show();
            }
        }
    }
}
