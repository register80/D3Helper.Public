using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Enigma.D3;
using Enigma.D3.Helpers;
using Enigma.D3.UI;
using Enigma.D3.UI.Controls;

namespace D3Helper.A_Handler.SkillBuildSwap
{
    public class SkillBuildSwap
    {
        public class ActiveSkill
        {
            public ActiveSkill(int powerSno, int rune)
            {
                this.PowerSno = powerSno;
                this.Rune = rune;
            }
            public int PowerSno { get; set; }
            public int Rune { get; set; }
        }
        public class PassiveSkill
        {
            public PassiveSkill(int powerSno)
            {
                this.PowerSno = powerSno;
                
            }
            public int PowerSno { get; set; }
           
        }
        public class SkillBuild
        {
            public SkillBuild(int id, string name, List<ActiveSkill> activeSkills, List<PassiveSkill> passiveSkills )
            {
                this.Id = id;
                this.Name = name;
                this.ActiveSkills = activeSkills;
                this.PassiveSkills = passiveSkills;
            }
            public int Id { get; set; }
            public string Name { get; set; }
            public List<ActiveSkill> ActiveSkills { get; set; } 
            public List<PassiveSkill> PassiveSkills { get; set; } 
        }

        public static void DoSwap()
        {
            try
            {
                A_Collection.Me.SkillBuilds.Is_SwapingBuild = true;

                int BuildId = A_Collection.Me.SkillBuilds.SelectedSkillBuild;

                var Build =
                    A_Collection.Me.SkillBuilds.Builds.FirstOrDefault(
                        x => x.Value == A_Collection.Me.HeroGlobals.HeroID && x.Key.Id == BuildId);

                bool SkillsRemoved = false;

                if (Build.Key != null)
                {
                    
                    //-- check if one skill is on cooldown

                    Dictionary<int, int> _ActiveSkills;
                    lock (A_Collection.Me.HeroDetails.ActiveSkills)
                        _ActiveSkills = A_Collection.Me.HeroDetails.ActiveSkills;

                    
                    foreach (var _using in _ActiveSkills)
                    {
                        if (A_Tools.Skills.Skills.S_Global.isOnCooldown(_using.Key))
                        {
                            MessageBox.Show("One or more skills are on cooldown. Cannot swap Build now");
                            A_Collection.Me.SkillBuilds.Is_SwapingBuild = false;
                            return;

                        }
                    }

                    //

                    //-- check if in Town

                    if (!A_Collection.Me.HeroStates.isInTown)
                    {
                        MessageBox.Show("You can change Builds only while in Town");
                        A_Collection.Me.SkillBuilds.Is_SwapingBuild = false;
                        return;

                    }

                    //

                    //-- Swap Actives 

                    var Actives = Build.Key.ActiveSkills.Where(x => x.PowerSno != 0).ToList();

                    for (int i = 0; i < Actives.Count; i++)
                    {
                        var activeSkill = Actives[i];

                        Dictionary<int, int> ActiveSkills;
                        lock (A_Collection.Me.HeroDetails.ActiveSkills)
                            ActiveSkills = A_Collection.Me.HeroDetails.ActiveSkills;

                        if (ActiveSkills.FirstOrDefault(x => x.Key == activeSkill.PowerSno && x.Value == activeSkill.Rune).Key != 0)
                            continue;

                        Action_SelectActiveSlot(i);

                        while (!A_Tools.T_D3UI.UIElement.isVisible(A_Enums.UIElements.SkillPanel_Active_PageNext) && isSkillsWindowVisible())
                        {
                            Thread.Sleep(10);
                        }

                        while (!ActivePage_ContainsPower(activeSkill.PowerSno) && isSkillsWindowVisible())
                        {
                            Action_NextPage();
                        }

                        while (!isActivePowerAssigned(activeSkill.PowerSno) && isSkillsWindowVisible())
                        {

                            if (isActiveSelectionDisabled())
                            {
                                Action_RemoveSkillsFromHotbar(i + 1);
                                Action_SelectActiveSlot(i);

                                while (!A_Tools.T_D3UI.UIElement.isVisible(A_Enums.UIElements.SkillPanel_Active_PageNext) && isSkillsWindowVisible())
                                {
                                    Thread.Sleep(10);
                                }

                                while (!ActivePage_ContainsPower(activeSkill.PowerSno) && isSkillsWindowVisible())
                                {
                                    Action_NextPage();
                                }
                            }

                            Action_SelectActivePower(activeSkill.PowerSno);
                        }

                        

                        while (!isSkillPanel_MainWindow() && isSkillsWindowVisible())
                        {
                            Action_SelectRune(activeSkill.PowerSno, activeSkill.Rune);
                        }

                        //while (
                        //    !isActiveRuneAssigned(
                        //        A_Collection.Presets.SkillPowers.AllSkillPowers.First(
                        //            x => x.PowerSNO == activeSkill.PowerSno)
                        //            .Runes.First(x => x.RuneIndex == activeSkill.Rune)
                        //            .Name) && isSkillsWindowVisible())
                        //{
                        //    Action_SelectRune(activeSkill.PowerSno, activeSkill.Rune);
                        //}
                        //Action_AcceptActiveSkill();
                    }

                    //

                    //-- Swap Passives

                    OpenSkillsWindow();

                    
                    while (true)
                    {
                        List<int> PassiveSkills =
                            A_Collection.Me.HeroGlobals.LocalPlayerData.GetPassivePowerSnoIds().ToList();

                        if (PassiveSkills[0] == Build.Key.PassiveSkills[0].PowerSno &&
                            PassiveSkills[1] == Build.Key.PassiveSkills[1].PowerSno &&
                            PassiveSkills[2] == Build.Key.PassiveSkills[2].PowerSno &&
                            PassiveSkills[3] == Build.Key.PassiveSkills[3].PowerSno)
                        {
                            break;
                        }

                        for (int i = 0; i < Build.Key.PassiveSkills.Count; i++)
                        {
                            var passiveSkill = Build.Key.PassiveSkills[i];


                            if (PassiveSkills.Contains(passiveSkill.PowerSno))
                            {
                                continue;
                            }

                            while (!isSkillPanel_PassiveSelection() && isSkillsWindowVisible())
                            {
                                Action_SelectPassiveSlot(i);
                            }

                            while (!isSkillPanel_MainWindow() && isSkillsWindowVisible())
                            {
                                Action_SelectPassivePower(passiveSkill.PowerSno);
                            }

                        }
                    }

                    //

                    CloseSkillsWindow();

                    int _x = A_Collection.D3Client.Window.D3ClientRect.Width / 2;
                    int _y = A_Collection.D3Client.Window.D3ClientRect.Height / 2;

                    Cursor.Position = new Point((int)_x, (int)_y);
                }

                A_Collection.Me.SkillBuilds.Is_SwapingBuild = false;
            }
            catch (Exception)
            {
                A_Collection.Me.SkillBuilds.Is_SwapingBuild = false;
            }
        }

        private static void Action_SelectPassivePower(int PowerSno)
        {
            string control = "";

            if (A_Tools.T_D3UI.UIElement.isVisible(A_Enums.UIElements.SkillPanel_Passive_Skill1))
                if (UXHelper.GetControl<UXIcon>(A_Enums.UIElements.SkillPanel_Passive_Skill1).x166C_PowerSnoId ==
                    PowerSno)
                    control = A_Enums.UIElements.SkillPanel_Passive_Skill1;

                else if (A_Tools.T_D3UI.UIElement.isVisible(A_Enums.UIElements.SkillPanel_Passive_Skill2))
                    if (UXHelper.GetControl<UXIcon>(A_Enums.UIElements.SkillPanel_Passive_Skill2).x166C_PowerSnoId ==
                        PowerSno)
                        control = A_Enums.UIElements.SkillPanel_Passive_Skill2;

                    else if (A_Tools.T_D3UI.UIElement.isVisible(A_Enums.UIElements.SkillPanel_Passive_Skill3))
                        if (
                            UXHelper.GetControl<UXIcon>(A_Enums.UIElements.SkillPanel_Passive_Skill3).x166C_PowerSnoId ==
                            PowerSno)
                            control = A_Enums.UIElements.SkillPanel_Passive_Skill3;

                        else if (A_Tools.T_D3UI.UIElement.isVisible(A_Enums.UIElements.SkillPanel_Passive_Skill4))
                            if (
                                UXHelper.GetControl<UXIcon>(A_Enums.UIElements.SkillPanel_Passive_Skill4)
                                    .x166C_PowerSnoId == PowerSno)
                                control = A_Enums.UIElements.SkillPanel_Passive_Skill4;

                            else if (A_Tools.T_D3UI.UIElement.isVisible(A_Enums.UIElements.SkillPanel_Passive_Skill5))
                                if (
                                    UXHelper.GetControl<UXIcon>(A_Enums.UIElements.SkillPanel_Passive_Skill5)
                                        .x166C_PowerSnoId == PowerSno)
                                    control = A_Enums.UIElements.SkillPanel_Passive_Skill5;

                                else if (A_Tools.T_D3UI.UIElement.isVisible(A_Enums.UIElements.SkillPanel_Passive_Skill6))
                                    if (
                                        UXHelper.GetControl<UXIcon>(A_Enums.UIElements.SkillPanel_Passive_Skill6)
                                            .x166C_PowerSnoId == PowerSno)
                                        control = A_Enums.UIElements.SkillPanel_Passive_Skill6;

                                    else if (
                                        A_Tools.T_D3UI.UIElement.isVisible(
                                            A_Enums.UIElements.SkillPanel_Passive_Skill7))
                                        if (
                                            UXHelper.GetControl<UXIcon>(
                                                A_Enums.UIElements.SkillPanel_Passive_Skill7).x166C_PowerSnoId ==
                                            PowerSno)
                                            control = A_Enums.UIElements.SkillPanel_Passive_Skill7;

                                        else if (
                                            A_Tools.T_D3UI.UIElement.isVisible(
                                                A_Enums.UIElements.SkillPanel_Passive_Skill8))
                                            if (
                                                UXHelper.GetControl<UXIcon>(
                                                    A_Enums.UIElements.SkillPanel_Passive_Skill8)
                                                    .x166C_PowerSnoId == PowerSno)
                                                control = A_Enums.UIElements.SkillPanel_Passive_Skill8;

                                            else if (
                                                A_Tools.T_D3UI.UIElement.isVisible(
                                                    A_Enums.UIElements.SkillPanel_Passive_Skill9))
                                                if (
                                                    UXHelper.GetControl<UXIcon>(
                                                        A_Enums.UIElements.SkillPanel_Passive_Skill9)
                                                        .x166C_PowerSnoId == PowerSno)
                                                    control =
                                                        A_Enums.UIElements.SkillPanel_Passive_Skill9;

                                                else if (
                                                    A_Tools.T_D3UI.UIElement.isVisible(
                                                        A_Enums.UIElements
                                                            .SkillPanel_Passive_Skill10))
                                                    if (
                                                        UXHelper.GetControl<UXIcon>(
                                                            A_Enums.UIElements
                                                                .SkillPanel_Passive_Skill10)
                                                            .x166C_PowerSnoId == PowerSno)
                                                        control =
                                                            A_Enums.UIElements
                                                                .SkillPanel_Passive_Skill10;

                                                    else if (
                                                        A_Tools.T_D3UI.UIElement.isVisible(
                                                            A_Enums.UIElements
                                                                .SkillPanel_Passive_Skill11))
                                                        if (
                                                            UXHelper.GetControl<UXIcon>(
                                                                A_Enums.UIElements
                                                                    .SkillPanel_Passive_Skill11)
                                                                .x166C_PowerSnoId == PowerSno)
                                                            control =
                                                                A_Enums.UIElements
                                                                    .SkillPanel_Passive_Skill11;

                                                        else if (
                                                            A_Tools.T_D3UI.UIElement
                                                                .isVisible(
                                                                    A_Enums.UIElements
                                                                        .SkillPanel_Passive_Skill12))
                                                            if (
                                                                UXHelper.GetControl<UXIcon>(
                                                                    A_Enums.UIElements
                                                                        .SkillPanel_Passive_Skill12)
                                                                    .x166C_PowerSnoId ==
                                                                PowerSno)
                                                                control =
                                                                    A_Enums.UIElements
                                                                        .SkillPanel_Passive_Skill12;

                                                            else if (
                                                                A_Tools.T_D3UI.UIElement
                                                                    .isVisible(
                                                                        A_Enums
                                                                            .UIElements
                                                                            .SkillPanel_Passive_Skill13))
                                                                if (
                                                                    UXHelper
                                                                        .GetControl
                                                                        <UXIcon>(
                                                                            A_Enums
                                                                                .UIElements
                                                                                .SkillPanel_Passive_Skill13)
                                                                        .x166C_PowerSnoId ==
                                                                    PowerSno)
                                                                    control =
                                                                        A_Enums
                                                                            .UIElements
                                                                            .SkillPanel_Passive_Skill13;

                                                                else if (
                                                                    A_Tools.T_D3UI
                                                                        .UIElement
                                                                        .isVisible(
                                                                            A_Enums
                                                                                .UIElements
                                                                                .SkillPanel_Passive_Skill14))
                                                                    if (
                                                                        UXHelper
                                                                            .GetControl
                                                                            <UXIcon>
                                                                            (A_Enums
                                                                                .UIElements
                                                                                .SkillPanel_Passive_Skill14)
                                                                            .x166C_PowerSnoId ==
                                                                        PowerSno)
                                                                        control =
                                                                            A_Enums
                                                                                .UIElements
                                                                                .SkillPanel_Passive_Skill14;

                                                                    else if (
                                                                        A_Tools
                                                                            .T_D3UI
                                                                            .UIElement
                                                                            .isVisible
                                                                            (A_Enums
                                                                                .UIElements
                                                                                .SkillPanel_Passive_Skill15))
                                                                        if (
                                                                            UXHelper
                                                                                .GetControl
                                                                                <
                                                                                    UXIcon
                                                                                    >
                                                                                (A_Enums
                                                                                    .UIElements
                                                                                    .SkillPanel_Passive_Skill15)
                                                                                .x166C_PowerSnoId ==
                                                                            PowerSno)
                                                                            control
                                                                                =
                                                                                A_Enums
                                                                                    .UIElements
                                                                                    .SkillPanel_Passive_Skill15;

                                                                        else if (
                                                                            A_Tools
                                                                                .T_D3UI
                                                                                .UIElement
                                                                                .isVisible
                                                                                (A_Enums
                                                                                    .UIElements
                                                                                    .SkillPanel_Passive_Skill16))
                                                                            if
                                                                                (
                                                                                UXHelper
                                                                                    .GetControl
                                                                                    <
                                                                                        UXIcon
                                                                                        >
                                                                                    (A_Enums
                                                                                        .UIElements
                                                                                        .SkillPanel_Passive_Skill16)
                                                                                    .x166C_PowerSnoId ==
                                                                                PowerSno)
                                                                                control
                                                                                    =
                                                                                    A_Enums
                                                                                        .UIElements
                                                                                        .SkillPanel_Passive_Skill16;

                                                                            else if
                                                                                (
                                                                                A_Tools
                                                                                    .T_D3UI
                                                                                    .UIElement
                                                                                    .isVisible
                                                                                    (A_Enums
                                                                                        .UIElements
                                                                                        .SkillPanel_Passive_Skill17))
                                                                                if
                                                                                    (
                                                                                    UXHelper
                                                                                        .GetControl
                                                                                        <
                                                                                            UXIcon
                                                                                            >
                                                                                        (A_Enums
                                                                                            .UIElements
                                                                                            .SkillPanel_Passive_Skill17)
                                                                                        .x166C_PowerSnoId ==
                                                                                    PowerSno)
                                                                                    control
                                                                                        =
                                                                                        A_Enums
                                                                                            .UIElements
                                                                                            .SkillPanel_Passive_Skill17;

                                                                                else if
                                                                                    (
                                                                                    A_Tools
                                                                                        .T_D3UI
                                                                                        .UIElement
                                                                                        .isVisible
                                                                                        (A_Enums
                                                                                            .UIElements
                                                                                            .SkillPanel_Passive_Skill18))
                                                                                    if
                                                                                        (
                                                                                        UXHelper
                                                                                            .GetControl
                                                                                            <
                                                                                                UXIcon
                                                                                                >
                                                                                            (A_Enums
                                                                                                .UIElements
                                                                                                .SkillPanel_Passive_Skill18)
                                                                                            .x166C_PowerSnoId ==
                                                                                        PowerSno)
                                                                                        control
                                                                                            =
                                                                                            A_Enums
                                                                                                .UIElements
                                                                                                .SkillPanel_Passive_Skill18;

                                                                                    else if
                                                                                        (
                                                                                        A_Tools
                                                                                            .T_D3UI
                                                                                            .UIElement
                                                                                            .isVisible
                                                                                            (A_Enums
                                                                                                .UIElements
                                                                                                .SkillPanel_Passive_Skill19))
                                                                                        if
                                                                                            (
                                                                                            UXHelper
                                                                                                .GetControl
                                                                                                <
                                                                                                    UXIcon
                                                                                                    >
                                                                                                (A_Enums
                                                                                                    .UIElements
                                                                                                    .SkillPanel_Passive_Skill19)
                                                                                                .x166C_PowerSnoId ==
                                                                                            PowerSno)
                                                                                            control
                                                                                                =
                                                                                                A_Enums
                                                                                                    .UIElements
                                                                                                    .SkillPanel_Passive_Skill19;

            UIRect Slot = A_Tools.T_D3UI.UIElement.getRect(control);

            A_Tools.InputSimulator.IS_Mouse.LeftClick((int) Slot.Left, (int) Slot.Top, (int) Slot.Right,
                (int) Slot.Bottom);
            A_Tools.InputSimulator.IS_Mouse.LeftClick((int)Slot.Left, (int)Slot.Top, (int)Slot.Right,
                (int)Slot.Bottom);

            Thread.Sleep(100);
        }

        private static void Action_SelectActivePower(int PowerSno)
        {
            if (!isSkillsWindowVisible())
                return;

            string control = "";

            if (A_Tools.T_D3UI.UIElement.isVisible(A_Enums.UIElements.SkillPanel_Active_SkillSlot1))
                if (UXHelper.GetControl<UXIcon>(A_Enums.UIElements.SkillPanel_Active_SkillSlot1).x166C_PowerSnoId == PowerSno)
                    control = A_Enums.UIElements.SkillPanel_Active_SkillSlot1;

            if (A_Tools.T_D3UI.UIElement.isVisible(A_Enums.UIElements.SkillPanel_Active_SkillSlot2))
                if (UXHelper.GetControl<UXIcon>(A_Enums.UIElements.SkillPanel_Active_SkillSlot2).x166C_PowerSnoId == PowerSno)
                    control = A_Enums.UIElements.SkillPanel_Active_SkillSlot2;

            if (A_Tools.T_D3UI.UIElement.isVisible(A_Enums.UIElements.SkillPanel_Active_SkillSlot3))
                if (UXHelper.GetControl<UXIcon>(A_Enums.UIElements.SkillPanel_Active_SkillSlot3).x166C_PowerSnoId == PowerSno)
                    control = A_Enums.UIElements.SkillPanel_Active_SkillSlot3;

            if (A_Tools.T_D3UI.UIElement.isVisible(A_Enums.UIElements.SkillPanel_Active_SkillSlot4))
                if (UXHelper.GetControl<UXIcon>(A_Enums.UIElements.SkillPanel_Active_SkillSlot4).x166C_PowerSnoId == PowerSno)
                    control = A_Enums.UIElements.SkillPanel_Active_SkillSlot4;

            if (control != "")
            {
                UIRect Slot = A_Tools.T_D3UI.UIElement.getRect(control);


                A_Tools.InputSimulator.IS_Mouse.LeftClick((int) Slot.Left, (int) Slot.Top, (int) Slot.Right,
                    (int) Slot.Bottom);

                Thread.Sleep(50);
            }
        }
        private static void Action_AcceptPassiveSkill()
        {
            if (!isSkillsWindowVisible())
                return;

            UIRect Slot = A_Tools.T_D3UI.UIElement.getRect(A_Enums.UIElements.SkillPanel_AcceptPassive);

            A_Tools.InputSimulator.IS_Mouse.LeftClick((int) Slot.Left, (int) Slot.Top, (int) Slot.Right,
                (int) Slot.Bottom);

            Thread.Sleep(50);
        }
        private static void Action_AcceptActiveSkill()
        {
            if (!isSkillsWindowVisible())
                return;

            UIRect Slot = A_Tools.T_D3UI.UIElement.getRect(A_Enums.UIElements.SkillPanel_AcceptActive);

            A_Tools.InputSimulator.IS_Mouse.LeftClick((int)Slot.Left, (int)Slot.Top, (int)Slot.Right, (int)Slot.Bottom);

            Thread.Sleep(50);
        }
        private static void Action_SelectRune(int PowerSno, int RuneId)
        {
            if (!isSkillsWindowVisible())
                return;

            string control = "";

            if(RuneId == -1)
                control = A_Enums.UIElements.SkillPanel_Active_RuneSlot0;
            else
            {
                if (
                    UXHelper.GetControl<UXIcon>(A_Enums.UIElements.SkillPanel_Active_RuneSlot1_Text)
                        .xA20_Text_StructStart_Min84Bytes ==
                    A_Collection.Presets.SkillPowers.AllSkillPowers.First(x => x.PowerSNO == PowerSno)
                        .Runes.First(x => x.RuneIndex == RuneId)
                        .Name)
                    control = A_Enums.UIElements.SkillPanel_Active_RuneSlot1;

                else if (
                    UXHelper.GetControl<UXIcon>(A_Enums.UIElements.SkillPanel_Active_RuneSlot2_Text)
                        .xA20_Text_StructStart_Min84Bytes ==
                    A_Collection.Presets.SkillPowers.AllSkillPowers.First(x => x.PowerSNO == PowerSno)
                        .Runes.First(x => x.RuneIndex == RuneId)
                        .Name)
                    control = A_Enums.UIElements.SkillPanel_Active_RuneSlot2;

                else if (
                    UXHelper.GetControl<UXIcon>(A_Enums.UIElements.SkillPanel_Active_RuneSlot3_Text)
                        .xA20_Text_StructStart_Min84Bytes ==
                    A_Collection.Presets.SkillPowers.AllSkillPowers.First(x => x.PowerSNO == PowerSno)
                        .Runes.First(x => x.RuneIndex == RuneId)
                        .Name)
                    control = A_Enums.UIElements.SkillPanel_Active_RuneSlot3;

                else if (
                    UXHelper.GetControl<UXIcon>(A_Enums.UIElements.SkillPanel_Active_RuneSlot4_Text)
                        .xA20_Text_StructStart_Min84Bytes ==
                    A_Collection.Presets.SkillPowers.AllSkillPowers.First(x => x.PowerSNO == PowerSno)
                        .Runes.First(x => x.RuneIndex == RuneId)
                        .Name)
                    control = A_Enums.UIElements.SkillPanel_Active_RuneSlot4;

                else if (
                    UXHelper.GetControl<UXIcon>(A_Enums.UIElements.SkillPanel_Active_RuneSlot5_Text)
                        .xA20_Text_StructStart_Min84Bytes ==
                    A_Collection.Presets.SkillPowers.AllSkillPowers.First(x => x.PowerSNO == PowerSno)
                        .Runes.First(x => x.RuneIndex == RuneId)
                        .Name)
                    control = A_Enums.UIElements.SkillPanel_Active_RuneSlot5;
            }

            if (control != "")
            {
                UIRect Slot = A_Tools.T_D3UI.UIElement.getRect(control);

                A_Tools.InputSimulator.IS_Mouse.LeftClick((int) Slot.Left, (int) Slot.Top, (int) Slot.Right,
                    (int) Slot.Bottom);
                A_Tools.InputSimulator.IS_Mouse.LeftClick((int) Slot.Left, (int) Slot.Top, (int) Slot.Right,
                    (int) Slot.Bottom);

                Thread.Sleep(50);
            }
        }
        private static void Action_NextPage()
        {
            if (!isSkillsWindowVisible())
                return;

            UIRect Slot = A_Tools.T_D3UI.UIElement.getRect(A_Enums.UIElements.SkillPanel_Active_PageNext);

            A_Tools.InputSimulator.IS_Mouse.LeftClick((int)Slot.Left, (int)Slot.Top, (int)Slot.Right, (int)Slot.Bottom);

            Thread.Sleep(100);
        }
        private static bool ActivePage_ContainsPower(int PowerSno)
        {

            if(A_Tools.T_D3UI.UIElement.isVisible(A_Enums.UIElements.SkillPanel_Active_SkillSlot1))
                if(UXHelper.GetControl<UXIcon>(A_Enums.UIElements.SkillPanel_Active_SkillSlot1).x166C_PowerSnoId == PowerSno)
                    return true;

            if (A_Tools.T_D3UI.UIElement.isVisible(A_Enums.UIElements.SkillPanel_Active_SkillSlot2))
                if (UXHelper.GetControl<UXIcon>(A_Enums.UIElements.SkillPanel_Active_SkillSlot2).x166C_PowerSnoId == PowerSno)
                    return true;

            if (A_Tools.T_D3UI.UIElement.isVisible(A_Enums.UIElements.SkillPanel_Active_SkillSlot3))
                if (UXHelper.GetControl<UXIcon>(A_Enums.UIElements.SkillPanel_Active_SkillSlot3).x166C_PowerSnoId == PowerSno)
                    return true;

            if (A_Tools.T_D3UI.UIElement.isVisible(A_Enums.UIElements.SkillPanel_Active_SkillSlot4))
                if (UXHelper.GetControl<UXIcon>(A_Enums.UIElements.SkillPanel_Active_SkillSlot4).x166C_PowerSnoId == PowerSno)
                    return true;

            return false;
        }
        private static void Action_SelectActiveSlot(int slot)
        {
            
            string control = "";

            switch (slot)
            {
                case 0:
                    control = A_Enums.UIElements.SkillHotBarLeftclick;
                    break;

                case 1:
                    control = A_Enums.UIElements.SkillHotBarRightclick;
                    break;

                case 2:
                    control = A_Enums.UIElements.SkillHotBar1;
                    break;

                case 3:
                    control = A_Enums.UIElements.SkillHotBar2;
                    break;

                case 4:
                    control = A_Enums.UIElements.SkillHotBar3;
                    break;

                case 5:
                    control = A_Enums.UIElements.SkillHotBar4;
                    break;
            }

            if (control != "")
            {
                UIRect Slot = A_Tools.T_D3UI.UIElement.getRect(control);

                A_Tools.InputSimulator.IS_Mouse.RightCLick((int) Slot.Left, (int) Slot.Top, (int) Slot.Right,
                    (int) Slot.Bottom);

                Thread.Sleep(50);
            }
        }
        private static void Action_SelectPassiveSlot(int slot)
        {
            if (!isSkillsWindowVisible())
                return;

            string control = "";

            switch (slot)
            {
                case 0:
                    control = A_Enums.UIElements.SkillPanel_Passive1;
                    break;

                case 1:
                    control = A_Enums.UIElements.SkillPanel_Passive2;
                    break;

                case 2:
                    control = A_Enums.UIElements.SkillPanel_Passive3;
                    break;

                case 3:
                    control = A_Enums.UIElements.SkillPanel_Passive4;
                    break;

            }

            UIRect Slot = A_Tools.T_D3UI.UIElement.getRect(control);

            A_Tools.InputSimulator.IS_Mouse.LeftClick((int)Slot.Left, (int)Slot.Top, (int)Slot.Right, (int)Slot.Bottom);

            Thread.Sleep(50);
        }
        private static void OpenSkillsWindow()
        {
            try
            {
                while (!isSkillsWindowVisible())
                {
                    A_Tools.InputSimulator.IS_Keyboard.SkillsWindow();

                    Thread.Sleep(50);
                }
            }
            catch { }
        }
        private static void CloseSkillsWindow()
        {
            try
            {
                while (isSkillsWindowVisible())
                {
                    A_Tools.InputSimulator.IS_Keyboard.SkillsWindow();

                    Thread.Sleep(50);
                }
            }
            catch { }
        }
        private static bool isSkillsWindowVisible()
        {
            try
            {
                return A_Tools.T_D3UI.UIElement.isVisible(A_Enums.UIElements.SkillPanel) || A_Tools.T_D3UI.UIElement.isVisible(A_Enums.UIElements.SkillPanel_Passive_Skill1) || A_Tools.T_D3UI.UIElement.isVisible(A_Enums.UIElements.SkillPanel_Active_PageNext);
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static bool isActivePowerAssigned(int PowerSno)
        {
            if (!UXHelper.GetControl<UXIcon>(A_Enums.UIElements.SkillPanel_Active_AssignesSkill).IsVisible())
                return false;

            int Sno = UXHelper.GetControl<UXIcon>(A_Enums.UIElements.SkillPanel_Active_AssignesSkill).x166C_PowerSnoId;

            return  Sno == PowerSno;
        }

        private static bool isPassivePowerAssigned(int slot, int PowerSno)
        {
            switch (slot)
            {
                case 0:
                    return
                        UXHelper.GetControl<UXIcon>(A_Enums.UIElements.SkillPanel_Passive_Assigned1).x166C_PowerSnoId ==
                        PowerSno;

                case 1:
                    return
                        UXHelper.GetControl<UXIcon>(A_Enums.UIElements.SkillPanel_Passive_Assigned2).x166C_PowerSnoId ==
                        PowerSno;

                case 2:
                    return
                        UXHelper.GetControl<UXIcon>(A_Enums.UIElements.SkillPanel_Passive_Assigned3).x166C_PowerSnoId ==
                        PowerSno;

                case 3:
                    return
                        UXHelper.GetControl<UXIcon>(A_Enums.UIElements.SkillPanel_Passive_Assigned4).x166C_PowerSnoId ==
                        PowerSno;

                default:
                    return false;
            }
        }

        private static bool isActiveRuneAssigned(string RuneName)
        {
            return
                UXHelper.GetControl<UXLabel>(A_Enums.UIElements.SkillPanel_Active_AssignedRune)
                    .xA20_Text_StructStart_Min84Bytes == RuneName;
        }

        private static bool isSkillPanel_MainWindow()
        {
            return A_Tools.T_D3UI.UIElement.isVisible(A_Enums.UIElements.SkillPanel_Passive1);
        }

        private static bool isSkillPanel_PassiveSelection()
        {
            return A_Tools.T_D3UI.UIElement.isVisible(A_Enums.UIElements.SkillPanel_Passive_Skill1);
        }

        private static bool isActiveSelectionDisabled()
        {
            bool disabled = false;

            if (UXHelper.GetControl<UXIcon>(A_Enums.UIElements.SkillPanel_Active_SkillSlot1).x1460_IsDisabled == 1)
                disabled = true;

            return disabled;
        }

        private static void Action_RemoveSkillsFromHotbar(int StartSlot)
        {
            for (int i = StartSlot; i < 6; i++)
            {
                if (!isSkillsWindowVisible())
                    return;

                string control = "";

                switch (i)
                {
                    case 0:
                        control = A_Enums.UIElements.SkillHotBarLeftclick;
                        break;

                    case 1:
                        control = A_Enums.UIElements.SkillHotBarRightclick;
                        break;

                    case 2:
                        control = A_Enums.UIElements.SkillHotBar1;
                        break;

                    case 3:
                        control = A_Enums.UIElements.SkillHotBar2;
                        break;

                    case 4:
                        control = A_Enums.UIElements.SkillHotBar3;
                        break;

                    case 5:
                        control = A_Enums.UIElements.SkillHotBar4;
                        break;
                }

                if (UXHelper.GetControl<UXIcon>(control).x166C_PowerSnoId != -1)
                {
                    UIRect Slot = A_Tools.T_D3UI.UIElement.getRect(control);

                    A_Tools.InputSimulator.IS_Mouse.MoveCursor((int) Slot.Left, (int) Slot.Top, (int) Slot.Right,
                        (int) Slot.Bottom);
                    Thread.Sleep(50);
                    A_Tools.InputSimulator.IS_Mouse.LeftDown(0, 0);
                    Thread.Sleep(50);
                    A_Tools.InputSimulator.IS_Mouse.MoveCursor((uint) Slot.Left, (uint) (Slot.Top - (Slot.Height)));
                    Thread.Sleep(50);
                    A_Tools.InputSimulator.IS_Mouse.LeftUp(0, 0);

                    while (Engine.Current.ObjectManager.x9E0_PlayerInput.Dereference().x14_StructStart_Min56Bytes ==
                           37088)
                    {
                        A_Tools.InputSimulator.IS_Mouse.RightCLick();
                        Thread.Sleep(50);
                    }
                }
            }
        }
    }
}
