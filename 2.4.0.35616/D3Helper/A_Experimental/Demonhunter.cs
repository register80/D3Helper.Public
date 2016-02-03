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
    //class Demonhunter
    //{
    //    public static Thread thisThread;

    //    public static bool allowLoop = false;
                
                
    //    public static Dictionary<int, string> EquippedSkills = new Dictionary<int, string>();

    //    public static ActorCommonData TargetACD = null;

    //    public static float RightSX;
    //    public static float RightSY;
    //    public static float LeftSX;
    //    public static float LeftSY;
    //    public static float FrontSX;
    //    public static float FrontSY;
    //    public static float BackSX;
    //    public static float BackSY;
    //    public static float TargetSX;
    //    public static float TargetSY;

    //    public static void Start()
    //    {
    //        while (true)
    //        {
    //            try
    //            {
    //                EquippedSkills = A_Collection.Skills.SkillsAndHotkeys; // get all equipped Skills with Hotkeys

    //                if (!allowLoop)
    //                {
                        

    //                    Wizard.isTrialZone_Loop = false;
    //                    TargetACD = null;

    //                    thisThread.Abort();
    //                    break;
    //                }

    //                //-- --//

    //                if (!A_Collection.Me.HeroStates.isInTown)
    //                {
    //                    Wizard.isTrialZone_Loop = true; // Stop Skill AutoCasts if in TrialZone - check AutoCastManager 

    //                    Cast_NatRotation_OutOfTown();
    //                }



    //                Thread.Sleep(50);
    //            }
    //            catch { Console.WriteLine("Error"); }
    //        }
    //    }
    //    private static void Cast_NatRotation_OutOfTown()
    //    {
    //        if (allowLoop)
    //        {
    //            var Skill_Strafe = EquippedSkills.FirstOrDefault(x => x.Key == 134030);

    //            double startDistance = -1;

    //            while (allowLoop && A_Collection.Me.HeroDetails.Hitpoints_Percentage > 0.00001)
    //            {
    //                get_Target();



    //                if (TargetACD != null)
    //                {
    //                    if (startDistance == -1)
    //                    {
    //                        startDistance = A_Tools.T_ACD.get_Distance(TargetACD.x0D0_WorldPosX, TargetACD.x0D4_WorldPosY);
    //                    }

    //                    Vector2D pos_buffer = new Vector2D(A_Collection.Me.HeroGlobals.LocalACD.x0D0_WorldPosX, A_Collection.Me.HeroGlobals.LocalACD.x0D4_WorldPosY);
                        
    //                    InputSimulator.SimulateKeyDown(VirtualKeyCode.VK_1);

                        
    //                    if (!A_Tools.Skills.Skills.S_Global.isOnCooldown(130831)) // cast RoV if not on Cooldown
    //                    {
    //                        InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_1);
    //                        Cast_RoV();
    //                        InputSimulator.SimulateKeyDown(VirtualKeyCode.VK_1);
    //                    }

    //                    TargetShot_EvasiveFire(TargetACD, startDistance);

    //                    SpinTo_Right(pos_buffer);
    //                    SpinTo_Back(TargetACD, startDistance);
    //                    SpinTo_Left(pos_buffer);

    //                    if (!A_Tools.Skills.Skills.S_Global.isOnCooldown(130831)) // cast RoV if not on Cooldown
    //                    {
    //                        InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_1);
    //                        Cast_RoV();
    //                        InputSimulator.SimulateKeyDown(VirtualKeyCode.VK_1);
    //                    }

    //                    TargetShot_EvasiveFire(TargetACD, startDistance);

    //                    SpinTo_Left(pos_buffer);
    //                    SpinTo_Back(TargetACD, startDistance);
    //                    SpinTo_Right(pos_buffer);
                                                                       
                                                
                        
    //                }
    //                else
    //                {
    //                    startDistance = -1;
    //                }
                    
    //                Thread.Sleep(50);
    //            }

    //            InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_1);
                
                
    //        }
    //    }
    //    private static void Cast_RoV()
    //    {
    //        InputSimulator.SimulateKeyDown(VirtualKeyCode.SHIFT);

    //        //while(allowLoop && Collector.PlayerAttributeCollector.tryGetCurrentHPPercent() > 0.00001)
    //        //{
    //            //if (!!A_Tools.Skills.Skills.S_Global.isOnCooldown(130831)) // cast RoV if not on Cooldown
    //            //{
    //             //   break;
    //            //}

    //            var Skill_RoV = EquippedSkills.FirstOrDefault(x => x.Key == 130831);

    //            A_Tools.InputSimulator.IS_Mouse.MoveCursor((uint)TargetSX, (uint)TargetSY);

    //            A_Tools.InputSimulator.IS_Keyboard.PressKey(Skill_RoV.Value);

    //            Thread.Sleep(25);
    //        //}

    //        InputSimulator.SimulateKeyUp(VirtualKeyCode.SHIFT);
    //    }
    //    private static void TargetShot_EvasiveFire(ActorCommonData Target, double startDistance)
    //    {

    //        var startResource = A_Collection.Me.HeroDetails.ResourcePrimary;

    //        var Skill_EvasiveFire = EquippedSkills.FirstOrDefault(x => x.Key == 377450);

    //        InputSimulator.SimulateKeyDown(VirtualKeyCode.SHIFT);


    //        A_Tools.InputSimulator.IS_Mouse.MoveCursor((uint)TargetSX, (uint)TargetSY);

    //        A_Tools.InputSimulator.IS_Keyboard.PressKey(Skill_EvasiveFire.Value);

    //        Thread.Sleep(25);


    //        InputSimulator.SimulateKeyUp(VirtualKeyCode.SHIFT);

    //        if (A_Tools.T_ACD.get_Distance(Target.x0D0_WorldPosX, Target.x0D4_WorldPosY) > startDistance)
    //        {

    //            A_Tools.InputSimulator.IS_Mouse.MoveCursor((uint)TargetSX, (uint)TargetSY);

    //            Thread.Sleep(250);

    //        }

    //    }
    //    private static void SpinTo_Right(Vector2D posBuffer)
    //    {
    //        if (allowLoop)
    //        {
    //            A_Tools.InputSimulator.IS_Mouse.MoveCursor((uint)RightSX, (uint)RightSY);


    //            Thread.Sleep(25);

    //        }
    //    }
    //    private static void SpinTo_Left(Vector2D posBuffer)
    //    {
    //        if (allowLoop)
    //        {
    //            A_Tools.InputSimulator.IS_Mouse.MoveCursor((uint)LeftSX, (uint)LeftSY);


    //            Thread.Sleep(25);

    //        }
    //    }
    //    private static void SpinTo_Back(ActorCommonData Target, double startDistance)
    //    {
    //        if (allowLoop)
    //        {
    //            A_Tools.InputSimulator.IS_Mouse.MoveCursor((uint)BackSX, (uint)BackSY);

    //            Thread.Sleep(25);

    //            if (A_Tools.T_ACD.get_Distance(Target.x0D0_WorldPosX, Target.x0D4_WorldPosY) < startDistance)
    //            {

    //                A_Tools.InputSimulator.IS_Mouse.MoveCursor((uint)BackSX, (uint)BackSY);

    //                    Thread.Sleep(250);
                    
    //            }
    //        }
    //    }
    //    private static void SpinTo_Front()
    //    {
    //        if (allowLoop)
    //        {
    //            A_Tools.InputSimulator.IS_Mouse.MoveCursor((uint)FrontSX, (uint)FrontSY);

    //            Thread.Sleep(25);
    //        }
    //    }
    //    private static void get_Target()
    //    {
            

    //        ActorCommonData ClosestMonster = getMonster_ClosestDistance();

    //        if (ClosestMonster != null)
    //        {
    //            TargetACD = ClosestMonster;


    //        }
    //        else
    //        {

    //            TargetACD = null;

    //        }          
    //    }
    //    private static ActorCommonData getMonster_ClosestDistance()
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
    //    private static List<ActorCommonData> getAllMonsters_OnScreen()
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
        
    //}
}
