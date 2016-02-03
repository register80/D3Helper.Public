using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

using WindowsInput;

using D3Helper.A_Collection;
using D3Helper.A_Tools;

using Enigma.D3;
using Enigma.D3.UI;
using Enigma.D3.UI.Controls;
using Enigma.D3.Helpers;

namespace D3Helper.Experimental
{
    //public class Vector3D
    //{
    //    public Vector3D(float x, float y, float z)
    //    {
    //        this.X = x;
    //        this.Y = y;
    //        this.Z = z;
    //    }
    //    public float X { get; set; }
    //    public float Y { get; set; }
    //    public float Z { get; set; }
    //}
    //class Wizard
    //{
    //    public static Thread thisThread;

    //    public static bool allowLoop = false;

    //    public static bool isTrialZone_Loop = false;

    //    public static int LevelArea_SnoId_TrialZone = 405915;

    //    public static Vector3D Fix_Position_Wizard = new Vector3D(320, 320, -11);
    //    public static Vector3D Fix_Position_Hydra1 = new Vector3D(298, 296, -11);
    //    public static Vector3D Fix_Position_Hydra2 = new Vector3D(294, 309, -11);
    //    public static Vector3D Fix_Position_Bubble = new Vector3D(288, 287, -11);
        
    //    public static Dictionary<int, string> EquippedSkills = new Dictionary<int, string>();

    //    public static ActorCommonData TargetACD = null;
     
        
    //    public static void Start()
    //    {
            

    //        // Tal Damage Bonus 
    //        // PowerSnoId           429855
    //        // AttribId_Equipped    740
    //        // AttribId_BuffArcane  741
    //        // AttribId_BuffCold    742
    //        // AttribId_BuffFire    743
    //        // AttribId_BuffLight   744
    //        // AttribId_BuffTotal   745
    //        //
    //        // Arcane Dynamo
    //        // PowerSnoId           208823
    //        // AttribId_BuffTotal   741

    //        while (true)
    //        {
    //            try
    //            {
    //                EquippedSkills = A_Collection.Skills.SkillsAndHotkeys; // get all equipped Skills with Hotkeys

    //                if(!allowLoop)
    //                {
    //                    A_Tools.InputSimulator.IS_Mouse.LeftUp(0, 0);
    //                    InputSimulator.SimulateKeyUp(VirtualKeyCode.SHIFT);

    //                    isTrialZone_Loop = false;
    //                    TargetACD = null;
                        
    //                    thisThread.Abort();
    //                    break;
    //                }

    //                //-- --//

    //                if (Enigma.D3.LevelArea.Instance.x044_SnoId == LevelArea_SnoId_TrialZone) // in TrialZone
    //                {
    //                    isTrialZone_Loop = true; // Stop Skill AutoCasts if in TrialZone - check AutoCastManager 

    //                    Move_ToPosA();
    //                    Move_ToPosB();

    //                    Cast_ElectrocuteTrials();

    //                }
    //                else if(!A_Collection.Me.HeroStates.isInTown)
    //                {
    //                    isTrialZone_Loop = true;

    //                    Cast_Electrocute_OutOfTown();
    //                }
                    
                    
                                      
    //                Thread.Sleep(50);
    //            }
    //            catch { Console.WriteLine("Error"); }
    //        }

            
    //    }
    //    private static void Move_ToPosA()
    //    {
    //          if (getDistanceToPoint(Fix_Position_Wizard.X, Fix_Position_Wizard.Y) > 5)
    //            {
    //                while (getDistanceToPoint(Fix_Position_Hydra1.X, Fix_Position_Hydra1.Y) > 2 && allowLoop && A_Collection.Me.HeroDetails.Hitpoints_Percentage > 0.00001)
    //                {
    //                    Vector2D Pos_Hydra = ScreenPos_Position_Hydra1();

    //                    A_Tools.InputSimulator.IS_Mouse.LeftCLick((uint)Pos_Hydra.X, (uint)Pos_Hydra.Y);

    //                    Thread.Sleep(50);
    //                }
    //                Console.WriteLine("Move to Position A");
    //            }
                
    //    }
    //    private static void Move_ToPosB()
    //    {
            
    //            if (getDistanceToPoint(Fix_Position_Wizard.X, Fix_Position_Wizard.Y) > 3)
    //            {
    //                while (getDistanceToPoint(Fix_Position_Wizard.X, Fix_Position_Wizard.Y) > 2 && allowLoop && A_Collection.Me.HeroDetails.Hitpoints_Percentage > 0.00001)
    //                {
    //                    Vector2D Pos_Wizard = ScreenPos_Position_Wizard();

    //                    A_Tools.InputSimulator.IS_Mouse.LeftCLick((uint)Pos_Wizard.X, (uint)Pos_Wizard.Y);

    //                    Thread.Sleep(50);
    //                }
    //                Console.WriteLine("Move to Position B");
    //            }
                
            
    //    }
    //    private static void Cast_Hydra()
    //    {
    //        if (allowLoop)
    //        {
    //            var Skill_Hydra = EquippedSkills.FirstOrDefault(x => x.Key == 30725);

    //            while (A_Tools.T_LocalPlayer.getBuffCount(429855, 743) == 0 && allowLoop && A_Collection.Me.HeroDetails.Hitpoints_Percentage > 0.00001)
    //            {
    //                var Pos_Hydra = ScreenPos_Position_Hydra2();

    //                A_Tools.InputSimulator.IS_Mouse.MoveCursor((uint)Pos_Hydra.X, (uint)Pos_Hydra.Y);

    //                A_Tools.InputSimulator.IS_Keyboard.PressKey(Skill_Hydra.Value);

    //                Thread.Sleep(25);
    //            }
    //        }
    //    }
    //    private static void Cast_Hydra_SingleTarget(ActorCommonData Target)
    //    {
    //        if (allowLoop)
    //        {
    //            var Skill_Hydra = EquippedSkills.FirstOrDefault(x => x.Key == 30725);

    //            while (A_Tools.T_LocalPlayer.getBuffCount(429855, 743) == 0 && allowLoop && A_Collection.Me.HeroDetails.Hitpoints_Percentage > 0.00001)
    //            {
    //                var Pos_Hydra = ScreenPos_Position_Hydra_SingleTarget(Target);

    //                if (Pos_Hydra == null)
    //                {
    //                    break;
    //                }
    //                A_Tools.InputSimulator.IS_Mouse.MoveCursor((uint)Pos_Hydra.X, (uint)Pos_Hydra.Y);

    //                A_Tools.InputSimulator.IS_Keyboard.PressKey(Skill_Hydra.Value);

    //                Thread.Sleep(25);
    //            }
    //        }
    //    }
    //    private static void Cast_Blizzard(Vector2D TargetLocation)
    //    {
    //        if (allowLoop)
    //        {
    //            var Skill_Blizzard = EquippedSkills.FirstOrDefault(x => x.Key == 30680);

    //            while (A_Tools.T_LocalPlayer.getBuffCount(429855, 742) == 0 && allowLoop && A_Collection.Me.HeroDetails.Hitpoints_Percentage > 0.00001)
    //            {

    //                var Pos_Blizzard = TargetLocation;

    //                A_Tools.InputSimulator.IS_Mouse.MoveCursor((uint)Pos_Blizzard.X, (uint)Pos_Blizzard.Y);

    //                A_Tools.InputSimulator.IS_Keyboard.PressKey(Skill_Blizzard.Value);

    //                Thread.Sleep(25);
    //            }
    //        }
    //    }
    //    private static void Cast_MagicWeapon()
    //    {
    //        if (allowLoop)
    //        {
    //            var Skill_MagicWeapon = EquippedSkills.FirstOrDefault(x => x.Key == 76108);

    //            while (allowLoop && A_Collection.Me.HeroDetails.Hitpoints_Percentage > 0.00001)
    //            {
    //                if (A_Tools.T_LocalPlayer.getBuffCount(76108, 740) == 1)
    //                {
    //                    break;
    //                }

    //                A_Tools.InputSimulator.IS_Keyboard.PressKey(Skill_MagicWeapon.Value);

    //                Thread.Sleep(25);
    //            }
    //        }
    //    }
    //    private static void Cast_ExplosiveBlast()
    //    {
    //        if (allowLoop)
    //        {
    //            var Skill_ExplosiveBlast = EquippedSkills.FirstOrDefault(x => x.Key == 87525);

    //            while (A_Tools.T_LocalPlayer.getBuffCount(429855, 741) == 0 && allowLoop && A_Collection.Me.HeroDetails.Hitpoints_Percentage > 0.00001)
    //            {

    //                A_Tools.InputSimulator.IS_Keyboard.PressKey(Skill_ExplosiveBlast.Value);

    //                Thread.Sleep(25);
    //            }
    //        }
    //    }
    //    private static void Cast_ElectrocuteTrials()
    //    {
    //        if (allowLoop)
    //        {
    //            var Skill_Electrocute = EquippedSkills.FirstOrDefault(x => x.Key == 1765);



    //            while (allowLoop && A_Collection.Me.HeroDetails.Hitpoints_Percentage > 0.00001)
    //            {
    //                var Pos_Electrocute = ScreenPos_Position_TargetLocation_Trials();

    //                if (!A_Tools.Skills.Skills.S_Global.isOnCooldown(1769))
    //                {

    //                    A_Tools.InputSimulator.IS_Mouse.LeftUp(0, 0);
    //                    InputSimulator.SimulateKeyDown(VirtualKeyCode.SHIFT);
    //                    Cast_SlowTime();
    //                }

    //                //--
    //                if (A_Tools.T_LocalPlayer.getBuffCount(76108, 740) == 0) // MagicWeapon if not up
    //                {
    //                    if (A_Collection.Me.HeroDetails.ResourcePrimary >= 25)
    //                    {
    //                        A_Tools.InputSimulator.IS_Mouse.LeftUp(0, 0);
    //                        InputSimulator.SimulateKeyDown(VirtualKeyCode.SHIFT);
    //                        Cast_MagicWeapon();
    //                    }
    //                }                   
    //                //--

    //                if (A_Tools.T_LocalPlayer.getBuffCount(429855, 743) == 0 && A_Tools.T_LocalPlayer.getBuffCount(208823, 741) == 5) // Hydra if not up and 5 Dynamo
    //                {
    //                    if (A_Collection.Me.HeroDetails.ResourcePrimary >= 15)
    //                    {

    //                        A_Tools.InputSimulator.IS_Mouse.LeftUp(0, 0);
    //                        InputSimulator.SimulateKeyDown(VirtualKeyCode.SHIFT);
    //                        Cast_Hydra();
    //                    }
    //                }
    //                /*
    //                if (A_Tools.T_LocalPlayer.getBuffCount(429855, 741) == 0) // Explosive Blast if not up
    //                {
    //                    if (!A_Tools.Skills.Skills.S_Global.isOnCooldown(87525) && Collector.PlayerAttributeCollector.CollectPlayerResource(1) >= 20)
    //                    {

    //                        A_Tools.InputSimulator.IS_Mouse.LeftUp(0, 0);
    //                        InputSimulator.SimulateKeyDown(VirtualKeyCode.SHIFT);
    //                        Cast_ExplosiveBlast();
    //                    }
    //                }
    //                */

                    
    //                if (A_Tools.T_LocalPlayer.getBuffCount(429855, 742) == 0) // Blizzard if not up
    //                {
    //                    if (A_Collection.Me.HeroDetails.ResourcePrimary >= 40)
    //                    {

    //                        A_Tools.InputSimulator.IS_Mouse.LeftUp(0, 0);
    //                        InputSimulator.SimulateKeyDown(VirtualKeyCode.SHIFT);
    //                        Cast_Blizzard(Pos_Electrocute);
    //                    }
    //                }

    //                //--
    //                if (A_Tools.T_LocalPlayer.getBuffCount(429855, 741) == 0) // Arcane Buff if not up
    //                {
    //                    if (A_Collection.Me.HeroDetails.ResourcePrimary >= 40)
    //                    {

    //                        A_Tools.InputSimulator.IS_Mouse.LeftUp(0, 0);
    //                        InputSimulator.SimulateKeyDown(VirtualKeyCode.SHIFT);
    //                        Cast_Meteor_ToBuff(Pos_Electrocute);
    //                    }
    //                }
    //                //--
    //                /*
    //                if (A_Tools.T_LocalPlayer.getBuffCount(208823, 741) == 5 &&
    //                            A_Tools.T_LocalPlayer.getBuffCount(429855, 745) == 4 &&
    //                    Manager.SupportSkillManager.A_Tools.T_LocalPlayer.get_BuffTicksLeft(429855, 745) <= 50)
    //                {

    //                    A_Tools.InputSimulator.IS_Mouse.LeftUp(0, 0);
    //                    InputSimulator.SimulateKeyDown(VirtualKeyCode.SHIFT);
    //                    Cast_Meteor(Pos_Electrocute);
    //                }
    //                */
    //                if (A_Collection.Me.HeroDetails.ResourcePrimary_Percentage >= 70 &&
    //                    A_Tools.T_LocalPlayer.getBuffCount(208823, 741) == 5 &&
    //                    A_Tools.T_LocalPlayer.getBuffCount(429855, 745) == 4 && A_Tools.T_LocalPlayer.get_BuffTicksLeft(429855, 745) >= 75)
    //                {
                        
    //                    A_Tools.InputSimulator.IS_Mouse.LeftUp(0, 0);
    //                    InputSimulator.SimulateKeyDown(VirtualKeyCode.SHIFT);
    //                    Cast_Meteor(Pos_Electrocute);
    //                }

    //                A_Tools.InputSimulator.IS_Mouse.MoveCursor((uint)Pos_Electrocute.X, (uint)Pos_Electrocute.Y);
    //                InputSimulator.SimulateKeyDown(VirtualKeyCode.SHIFT);
    //                A_Tools.InputSimulator.IS_Mouse.LeftDown(0, 0);

    //                Thread.Sleep(25);
    //            }
                
                

    //            A_Tools.InputSimulator.IS_Mouse.LeftUp(0,0);
    //            InputSimulator.SimulateKeyUp(VirtualKeyCode.SHIFT);
    //        }
    //    }
    //    private static void Cast_Electrocute_OutOfTown()
    //    {
    //        if (allowLoop)
    //        {
    //            var Skill_Electrocute = EquippedSkills.FirstOrDefault(x => x.Key == 1765);



    //            while (allowLoop && A_Collection.Me.HeroDetails.Hitpoints_Percentage > 0.00001)
    //            {
    //                ActorCommonData Target;

    //                var Pos_Electrocute = ScreenPos_Position_TargetLocation_OutOfTown(out Target);

    //                if (Pos_Electrocute != null)
    //                {
    //                    if (!A_Tools.Skills.Skills.S_Global.isOnCooldown(1769)) // cast SlowTime if not on Cooldown
    //                    {

    //                        A_Tools.InputSimulator.IS_Mouse.LeftUp(0, 0);
    //                        InputSimulator.SimulateKeyDown(VirtualKeyCode.SHIFT);
    //                        Cast_SlowTime_SingleTarget(Pos_Electrocute);
    //                    }

    //                    if (A_Tools.T_LocalPlayer.getBuffCount(76108, 740) == 0) // MagicWeapon if not up
    //                    {
    //                        if (A_Collection.Me.HeroDetails.ResourcePrimary >= 25)
    //                        {
    //                            A_Tools.InputSimulator.IS_Mouse.LeftUp(0, 0);
    //                            InputSimulator.SimulateKeyDown(VirtualKeyCode.SHIFT);
    //                            Cast_MagicWeapon();
    //                        }
    //                    }


    //                    if (A_Tools.T_LocalPlayer.getBuffCount(429855, 743) == 0 && A_Tools.T_LocalPlayer.getBuffCount(208823, 741) == 5) // Hydra if not up and 5 Dynamo
    //                    {
    //                        if (A_Collection.Me.HeroDetails.ResourcePrimary >= 15)
    //                        {

    //                            A_Tools.InputSimulator.IS_Mouse.LeftUp(0, 0);
    //                            InputSimulator.SimulateKeyDown(VirtualKeyCode.SHIFT);
    //                            Cast_Hydra_SingleTarget(Target);
    //                        }
    //                    }

    //                    if (A_Tools.T_LocalPlayer.getBuffCount(429855, 742) == 0) // Blizzard if not up
    //                    {
    //                        if (A_Collection.Me.HeroDetails.ResourcePrimary >= 40)
    //                        {

    //                            A_Tools.InputSimulator.IS_Mouse.LeftUp(0, 0);
    //                            InputSimulator.SimulateKeyDown(VirtualKeyCode.SHIFT);
    //                            Cast_Blizzard(Pos_Electrocute);
    //                        }
    //                    }

    //                    if (A_Tools.T_LocalPlayer.getBuffCount(429855, 741) == 0) // Arcane Buff if not up
    //                    {
    //                        if (A_Collection.Me.HeroDetails.ResourcePrimary >= 40)
    //                        {

    //                            A_Tools.InputSimulator.IS_Mouse.LeftUp(0, 0);
    //                            InputSimulator.SimulateKeyDown(VirtualKeyCode.SHIFT);
    //                            Cast_Meteor_ToBuff(Pos_Electrocute);
    //                        }
    //                    }
                                                                        
                        
    //                    if (A_Collection.Me.HeroDetails.ResourcePrimary_Percentage == 100 &&
    //                        A_Tools.T_LocalPlayer.getBuffCount(208823, 741) == 5 &&
    //                        A_Tools.T_LocalPlayer.getBuffCount(429855, 745) == 4 ) //&& Manager.SupportSkillManager.A_Tools.T_LocalPlayer.get_BuffTicksLeft(429855, 745) >= 75)
    //                    {

    //                        A_Tools.InputSimulator.IS_Mouse.LeftUp(0, 0);
    //                        InputSimulator.SimulateKeyDown(VirtualKeyCode.SHIFT);
    //                        Cast_Meteor(Pos_Electrocute);
    //                    }

    //                    A_Tools.InputSimulator.IS_Mouse.MoveCursor((uint)Pos_Electrocute.X, (uint)Pos_Electrocute.Y);
    //                    InputSimulator.SimulateKeyDown(VirtualKeyCode.SHIFT);
    //                    A_Tools.InputSimulator.IS_Mouse.LeftDown(0, 0);
    //                }

    //                A_Tools.InputSimulator.IS_Mouse.LeftUp(0, 0);
    //                InputSimulator.SimulateKeyUp(VirtualKeyCode.SHIFT);
    //                Thread.Sleep(25);
    //            }



    //            A_Tools.InputSimulator.IS_Mouse.LeftUp(0, 0);
    //            InputSimulator.SimulateKeyUp(VirtualKeyCode.SHIFT);
    //        }
    //    }
    //    private static void Cast_Meteor(Vector2D TargetLocation)
    //    {
    //        while (allowLoop && A_Collection.Me.HeroDetails.Hitpoints_Percentage > 0.00001)
    //        {

    //            if (A_Collection.Me.HeroDetails.ResourcePrimary_Percentage < 30)
    //            {
    //                break;
    //            }

    //            var Skill_Meteor = EquippedSkills.FirstOrDefault(x => x.Key == 69190);

    //            var Pos_Meteor = TargetLocation;

    //            A_Tools.InputSimulator.IS_Mouse.MoveCursor((uint)Pos_Meteor.X, (uint)Pos_Meteor.Y);

    //            A_Tools.InputSimulator.IS_Keyboard.PressKey(Skill_Meteor.Value);

    //            Thread.Sleep(25);

                
    //        }
    //    }
    //    private static void Cast_Meteor_ToBuff(Vector2D TargetLocation)
    //    {
    //        while (allowLoop && A_Collection.Me.HeroDetails.Hitpoints_Percentage > 0.00001)
    //        {

    //            if (A_Tools.T_LocalPlayer.getBuffCount(429855, 741) == 1)
    //            {
    //                break;
    //            }

    //            var Skill_Meteor = EquippedSkills.FirstOrDefault(x => x.Key == 69190);

    //            var Pos_Meteor = TargetLocation;

    //            A_Tools.InputSimulator.IS_Mouse.MoveCursor((uint)Pos_Meteor.X, (uint)Pos_Meteor.Y);

    //            A_Tools.InputSimulator.IS_Keyboard.PressKey(Skill_Meteor.Value);

    //            Thread.Sleep(25);
    //        }
    //    }
    //    private static void Cast_SlowTime()
    //    {
    //        var Skill_SlowTime = EquippedSkills.FirstOrDefault(x => x.Key == 1769);

    //        int skillrune = A_Collection.Me.HeroDetails.ActiveSkills.FirstOrDefault(x => x.Key == 1769).Value;

    //        switch(skillrune)
    //        {
    //            case 4:
    //                while (allowLoop && A_Collection.Me.HeroDetails.Hitpoints_Percentage > 0.00001)
    //                {

    //                    if (!!A_Tools.Skills.Skills.S_Global.isOnCooldown(1769))
    //                    {
    //                        break;
    //                    }

    //                    var Pos_IasBubble = ScreenPos_Position_Wizard();

    //                    A_Tools.InputSimulator.IS_Mouse.MoveCursor((uint)Pos_IasBubble.X, (uint)Pos_IasBubble.Y);

    //                    A_Tools.InputSimulator.IS_Keyboard.PressKey(Skill_SlowTime.Value);

    //                    Thread.Sleep(25);
    //                }
    //                break;
    //            case 0:
    //                while (allowLoop && A_Collection.Me.HeroDetails.Hitpoints_Percentage > 0.00001)
    //                {
    //                    if (!!A_Tools.Skills.Skills.S_Global.isOnCooldown(1769))
    //                    {
    //                        break;
    //                    }

    //                    var Pos_DpsBubble = ScreenPos_Position_BubbleLocation();

    //                    A_Tools.InputSimulator.IS_Mouse.MoveCursor((uint)Pos_DpsBubble.X, (uint)Pos_DpsBubble.Y);

    //                    A_Tools.InputSimulator.IS_Keyboard.PressKey(Skill_SlowTime.Value);

    //                    Thread.Sleep(25);
    //                }
    //                break;
    //        }
    //    }
    //    private static void Cast_SlowTime_SingleTarget(Vector2D Target)
    //    {
    //        var Skill_SlowTime = EquippedSkills.FirstOrDefault(x => x.Key == 1769);

    //        int skillrune = A_Collection.Me.HeroDetails.ActiveSkills.FirstOrDefault(x => x.Key == 1769).Value;

    //        switch (skillrune)
    //        {
    //            case 4:
    //                while (allowLoop && A_Collection.Me.HeroDetails.Hitpoints_Percentage > 0.00001)
    //                {

    //                    if (!!A_Tools.Skills.Skills.S_Global.isOnCooldown(1769))
    //                    {
    //                        break;
    //                    }

    //                    var Pos_IasBubble = ScreenPos_LocalPlayer();

    //                    A_Tools.InputSimulator.IS_Mouse.MoveCursor((uint)Pos_IasBubble.X, (uint)Pos_IasBubble.Y);

    //                    A_Tools.InputSimulator.IS_Keyboard.PressKey(Skill_SlowTime.Value);

    //                    Thread.Sleep(25);
    //                }
    //                break;
    //            case 0:
    //                while (allowLoop && A_Collection.Me.HeroDetails.Hitpoints_Percentage > 0.00001)
    //                {
    //                    if (!!A_Tools.Skills.Skills.S_Global.isOnCooldown(1769))
    //                    {
    //                        break;
    //                    }

    //                    var Pos_DpsBubble = Target;

    //                    A_Tools.InputSimulator.IS_Mouse.MoveCursor((uint)Pos_DpsBubble.X, (uint)Pos_DpsBubble.Y);

    //                    A_Tools.InputSimulator.IS_Keyboard.PressKey(Skill_SlowTime.Value);

    //                    Thread.Sleep(25);
    //                }
    //                break;
    //        }
    //    }
        
    //    public static Vector3D LocalPlayer_Pos()
    //    {
    //        ActorCommonData local = ActorCommonData.Local;

    //        return new Vector3D(local.x0D0_WorldPosX, local.x0D4_WorldPosY, local.x0D8_WorldPosZ);
    //    }
    //    public static Vector2D ScreenPos_Position_Wizard()
    //    {
    //        float RX;
    //        float RY;

    //        A_Tools.T_World.ToScreenCoordinate(Fix_Position_Wizard.X, Fix_Position_Wizard.Y, Fix_Position_Wizard.Z, out RX, out RY);

    //        return new Vector2D(RX, RY);
    //    }
    //    public static Vector2D ScreenPos_LocalPlayer()
    //    {
    //        float RX;
    //        float RY;

    //        var local = A_Collection.Me.HeroGlobals.LocalACD;

    //        A_Tools.T_World.ToScreenCoordinate(local.x0D0_WorldPosX, local.x0D4_WorldPosY, local.x0D8_WorldPosZ, out RX, out RY);

    //        return new Vector2D(RX, RY);
    //    }
    //    public static Vector2D ScreenPos_Position_Hydra1()
    //    {
    //        float RX;
    //        float RY;

    //        A_Tools.T_World.ToScreenCoordinate(Fix_Position_Hydra1.X, Fix_Position_Hydra1.Y, Fix_Position_Hydra1.Z, out RX, out RY);

    //        return new Vector2D(RX, RY);
    //    }
    //    public static Vector2D ScreenPos_Position_Hydra2()
    //    {
    //        float RX;
    //        float RY;

    //        A_Tools.T_World.ToScreenCoordinate(Fix_Position_Hydra2.X, Fix_Position_Hydra2.Y, Fix_Position_Hydra2.Z, out RX, out RY);

    //        return new Vector2D(RX, RY);
    //    }
    //    public static Vector2D ScreenPos_Position_Hydra_SingleTarget(ActorCommonData Target)
    //    {
    //        try
    //        {
    //            double HydraDistance = 5;

    //            ActorCommonData Local = A_Collection.Me.HeroGlobals.LocalACD;

    //            double radius = (Target.x0DC_Radius + HydraDistance);

    //            float RX;
    //            float RY;

    //            A_Tools.T_World.ToScreenCoordinate(Target.x0D0_WorldPosX, Target.x0D4_WorldPosY, Target.x0D8_WorldPosZ, out RX, out RY);

    //            float Local_RX;
    //            float Local_RY;

    //            A_Tools.T_World.ToScreenCoordinate(Local.x0D0_WorldPosX, Local.x0D4_WorldPosY, Local.x0D8_WorldPosZ, out Local_RX, out Local_RY);


    //            System.Windows.Vector Hydra_Pos = Overlay.getPoint_Circle_Line_Intersection_Shorten(Local.x0D0_WorldPosX, Local.x0D4_WorldPosY, Target.x0D0_WorldPosX, Target.x0D4_WorldPosY, radius);

    //            float HydraSX, HydraSY;

    //            A_Tools.T_World.ToScreenCoordinate((float)Hydra_Pos.X, (float)Hydra_Pos.Y, Target.x0D8_WorldPosZ, out HydraSX, out HydraSY);

    //            return new Vector2D(HydraSX, HydraSY);
    //        }
    //        catch { return null; }
    //    }
    //    public static Vector2D ScreenPos_Position_BubbleLocation()
    //    {
    //        float RX;
    //        float RY;

    //        A_Tools.T_World.ToScreenCoordinate(Fix_Position_Bubble.X, Fix_Position_Bubble.Y, Fix_Position_Bubble.Z, out RX, out RY);

    //        return new Vector2D(RX, RY);
    //    }
    //    public static Vector2D ScreenPos_Position_TargetLocation_Trials()
    //    {
    //        float RX;
    //        float RY;

    //        ActorCommonData PalmDebuff_LowestHP = getMonster_PalmDebuff_LowestHP();

    //        if(PalmDebuff_LowestHP != null)
    //        {
    //            TargetACD = PalmDebuff_LowestHP;

    //            A_Tools.T_World.ToScreenCoordinate(PalmDebuff_LowestHP.x0D0_WorldPosX, PalmDebuff_LowestHP.x0D4_WorldPosY, PalmDebuff_LowestHP.x0D8_WorldPosZ, out RX, out RY);

    //            return new Vector2D(RX, RY);
    //        }

    //        ActorCommonData LowestHP = getMonster_LowestHP(getAllMonsters_OnScreen());

    //        if(LowestHP != null)
    //        {
    //            TargetACD = LowestHP;

    //            A_Tools.T_World.ToScreenCoordinate(LowestHP.x0D0_WorldPosX, LowestHP.x0D4_WorldPosY, LowestHP.x0D8_WorldPosZ, out RX, out RY);

    //            return new Vector2D(RX, RY);
    //        }

    //        TargetACD = null;

    //        A_Tools.T_World.ToScreenCoordinate(Fix_Position_Bubble.X, Fix_Position_Bubble.Y, Fix_Position_Bubble.Z, out RX, out RY);

    //        return new Vector2D(RX, RY);
    //    }
    //    public static Vector2D ScreenPos_Position_TargetLocation_OutOfTown(out ActorCommonData ClosestMonster)
    //    {
    //        float RX;
    //        float RY;

    //        ClosestMonster = getMonster_ClosestDistance();

    //        if (ClosestMonster != null)
    //        {
    //            TargetACD = ClosestMonster;

    //            A_Tools.T_World.ToScreenCoordinate(ClosestMonster.x0D0_WorldPosX, ClosestMonster.x0D4_WorldPosY, ClosestMonster.x0D8_WorldPosZ, out RX, out RY);

    //            return new Vector2D(RX, RY);
    //        }
                        

    //        TargetACD = null;
                        

    //        return null;
    //    }
    //    public static double getDistanceToPoint(float pointX, float pointY)
    //    {
    //        Vector3D Local = LocalPlayer_Pos();

    //        float localX = Local.X;
    //        float localY = Local.Y;

    //        float diffX = pointX - localX;
    //        float diffY = pointY - localY;


    //        float distance = (diffX * diffX) + (diffY * diffY);

    //        return Math.Sqrt((double)distance);

    //    }
    //    public static List<ActorCommonData> getAllMonsters_OnScreen()
    //    {
    //        //lock (A_Collection.Environment.Actors.AllActors)
    //        //{
    //            List<ActorCommonData> OnScreen = new List<ActorCommonData>();

    //        List<A_Collector.ACD> acdcontainer;
    //        lock (A_Collection.Environment.Actors.AllActors) acdcontainer = A_Collection.Environment.Actors.AllActors.ToList();

    //            var d3client = Overlay.d3clientrect;

    //            foreach (var acd in acdcontainer)
    //            {
    //                if (A_Tools.T_ACD.IsValidMonster(acd._ACD))
    //                {
    //                    float RX;
    //                    float RY;

    //                    A_Tools.T_World.ToScreenCoordinate(acd._ACD.x0D0_WorldPosX, acd._ACD.x0D4_WorldPosY, acd._ACD.x0D8_WorldPosZ, out RX, out RY);

    //                    if (RX > d3client.X &&
    //                        RX < (d3client.X + d3client.Width) &&
    //                        RY > d3client.Y &&
    //                        RY < (d3client.Y + d3client.Height)
    //                        )
    //                    {
    //                        OnScreen.Add(acd._ACD);
    //                    }

    //                }
    //            }

    //            return OnScreen;
    //        //}
    //    }
    //    public static List<ActorCommonData> getAllMonsters_PalmDebuff(List<ActorCommonData> OnScreen)
    //    {
    //        List<ActorCommonData> PalmDebuff = new List<ActorCommonData>();

    //        foreach(var monster in OnScreen)
    //        {
    //            if(A_Tools.T_ACD.isBuff(97328, monster))
    //            {
    //                PalmDebuff.Add(monster);
    //            }
    //        }

    //        return PalmDebuff;
    //    }
    //    public static ActorCommonData getMonster_PalmDebuff_LowestHP()
    //    {
    //        var OnScreen = getAllMonsters_OnScreen();

    //        var PalmDebuff = getAllMonsters_PalmDebuff(OnScreen);

    //        ActorCommonData Monster = null;
    //        double LowestCurHP = float.MaxValue;

    //        foreach(var monster in PalmDebuff)
    //        {
    //            double CurHp = monster.GetAttributeValue(Enigma.D3.Enums.AttributeId.HitpointsCur);

    //            if(CurHp < LowestCurHP)
    //            {
    //                LowestCurHP = CurHp;
    //                Monster = monster;
    //            }
    //        }

    //        return Monster;
    //    }
    //    public static ActorCommonData getMonster_LowestHP(List<ActorCommonData> onScreen)
    //    {
    //        var OnScreen = onScreen;
                        
    //        ActorCommonData Monster = null;
    //        double LowestCurHP = float.MaxValue;

    //        foreach (var monster in OnScreen)
    //        {
    //            double CurHp = monster.GetAttributeValue(Enigma.D3.Enums.AttributeId.HitpointsCur);

    //            if (CurHp < LowestCurHP)
    //            {
    //                LowestCurHP = CurHp;
    //                Monster = monster;
    //            }
    //        }

    //        return Monster;
    //    }
    //    public static ActorCommonData getMonster_ClosestDistance()
    //    {
    //        var OnScreen = getAllMonsters_OnScreen();

    //        ActorCommonData Monster = null;
    //        double ClosestDist = double.MaxValue;

    //        foreach (var monster in OnScreen)
    //        {
    //            double distance = A_Tools.T_ACD.get_Distance(monster.x0D0_WorldPosX, monster.x0D4_WorldPosY);

    //            if (distance < ClosestDist)
    //            {
    //                ClosestDist = distance;
    //                Monster = monster;
    //            }
    //        }

    //        return Monster;
    //    }
    //}
    
        
}
