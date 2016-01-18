using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using D3Helper.A_Enums;
using Enigma.D3;
using Enigma.D3.Enums;
using Enigma.D3.Helpers;
using DamageType = D3Helper.A_Enums.DamageType;

namespace D3Helper.A_Collector
{
    class IC_Party
    {
        private static DateTime _lastCollect = DateTime.Now;
        private static int _intervall = 500; //msec

        public static void Collect()
        {
            if (_lastCollect.AddMilliseconds(_intervall) < DateTime.Now)
            {
                if (A_Collection.Me.HeroStates.isInGame && A_Collection.Environment.Scene.GameTick > 1 &&
                    A_Collection.Environment.Scene.Counter_CurrentFrame != 0)
                {
                    get_AllPartyMembers();

                    if(A_Collection.Me.HeroGlobals.LocalPlayerData != null)
                        get_AllPlayerData();

                    //get_AllActivePowers();

                    if(Properties.Settings.Default.overlayconventiondraws)
                        get_AllConvention();
                }

                _lastCollect = DateTime.Now;
            }
        }
        private static void get_AllPartyMembers()
        {
            try
            {

                lock (A_Collection.Environment.Actors.AllActors)
                {
                    lock(A_Collection.Me.Party.PartyMemberInRange) A_Collection.Me.Party.PartyMemberInRange = A_Collection.Environment.Actors.AllActors.Where(x => x.IsPlayer).ToList();
                    
                }

            }
            catch (Exception e)
            {
                A_Handler.Log.ExceptionLogEntry newEntry = new A_Handler.Log.ExceptionLogEntry(e, DateTime.Now, A_Enums.ExceptionThread.ICollector);

                lock (A_Handler.Log.Exception.ExceptionLog) A_Handler.Log.Exception.ExceptionLog.Add(newEntry);
            }
        }
        private static void get_AllPlayerData()
        {
            try
            {
                var PlayerDataCollection = Engine.Current.ObjectManager.x798_Storage.x12C_Ptr_PlayerDataManager.Dereference().x0038_Items;

                int index = 1;

                foreach (var playerdata in PlayerDataCollection)
                {
                   if(playerdata.x0004_AcdId == A_Collection.Me.HeroGlobals.LocalPlayerData.x0004_AcdId)
                        continue;

                    switch (index)
                    {
                        case 1:
                            if (playerdata.x0008_ActorId != -1)
                                A_Collection.Me.Party.PartyMember1 = playerdata;
                            else
                            {
                                A_Collection.Me.Party.PartyMember1 = null;
                            }
                            break;

                        case 2:
                            if (playerdata.x0008_ActorId != -1)
                                A_Collection.Me.Party.PartyMember2 = playerdata;
                            else
                            {
                                A_Collection.Me.Party.PartyMember2 = null;
                            }
                            break;

                        case 3:
                            if (playerdata.x0008_ActorId != -1)
                                A_Collection.Me.Party.PartyMember3 = playerdata;
                            else
                            {
                                A_Collection.Me.Party.PartyMember3 = null;
                            }
                            break;
                    }

                    index++;
                }
                
            }
            catch (Exception e)
            {
                A_Handler.Log.ExceptionLogEntry newEntry = new A_Handler.Log.ExceptionLogEntry(e, DateTime.Now, A_Enums.ExceptionThread.ICollector);

                lock (A_Handler.Log.Exception.ExceptionLog) A_Handler.Log.Exception.ExceptionLog.Add(newEntry);
            }
        }
        private static void get_AllActivePowers()
        {
            try
            {

                for (int i = 1; i < 4; i++)
                {
                    switch (i)
                    {
                        case 1:
                            if (A_Collection.Me.Party.PartyMember1 != null)
                            {
                                var acd =
                                    A_Collection.Me.Party.PartyMemberInRange.FirstOrDefault(
                                        x => x._ACD.x08C_ActorId == A_Collection.Me.Party.PartyMember1.x0008_ActorId);
                                if(acd != null)
                                    lock (A_Collection.Me.Party.PartyMember1_ActivePowers)
                                        A_Collection.Me.Party.PartyMember1_ActivePowers =
                                            A_Tools.T_ACD.get_PlayerActivePowers(acd._ACD);
                            }
                            break;

                        case 2:
                            if (A_Collection.Me.Party.PartyMember2 != null)
                            {
                                var acd =
                                    A_Collection.Me.Party.PartyMemberInRange.FirstOrDefault(
                                        x => x._ACD.x08C_ActorId == A_Collection.Me.Party.PartyMember2.x0008_ActorId);
                                if (acd != null)
                                    lock (A_Collection.Me.Party.PartyMember2_ActivePowers)
                                        A_Collection.Me.Party.PartyMember2_ActivePowers =
                                            A_Tools.T_ACD.get_PlayerActivePowers(acd._ACD);
                            }
                            break;

                        case 3:
                            if (A_Collection.Me.Party.PartyMember3 != null)
                            {
                                var acd =
                                    A_Collection.Me.Party.PartyMemberInRange.FirstOrDefault(
                                        x => x._ACD.x08C_ActorId == A_Collection.Me.Party.PartyMember3.x0008_ActorId);
                                if (acd != null)
                                    lock (A_Collection.Me.Party.PartyMember3_ActivePowers)
                                        A_Collection.Me.Party.PartyMember3_ActivePowers =
                                            A_Tools.T_ACD.get_PlayerActivePowers(acd._ACD);
                            }
                            break;
                    }
                }

            }
            catch (Exception e)
            {
                A_Handler.Log.ExceptionLogEntry newEntry = new A_Handler.Log.ExceptionLogEntry(e, DateTime.Now, A_Enums.ExceptionThread.ICollector);

                lock (A_Handler.Log.Exception.ExceptionLog) A_Handler.Log.Exception.ExceptionLog.Add(newEntry);
            }
        }
        private static void get_AllConvention()
        {
            try
            {

                for (int i = 1; i < 4; i++)
                {
                    switch (i)
                    {
                        case 1:
                            if (A_Collection.Me.Party.PartyMember1 != null)
                            {
                                var acd =
                                    A_Collection.Me.Party.PartyMemberInRange.FirstOrDefault(
                                        x => x._ACD.x08C_ActorId == A_Collection.Me.Party.PartyMember1.x0008_ActorId);
                                if (acd != null)
                                {
                                    if (acd._ACD.GetAttributeValue((AttributeId)746, A_Enums.Powers.Convention_PowerSno) > 0)
                                    {
                                        A_Collection.Me.Party.PartyMember1_CurrentConventionElement =
                                            A_Tools.T_ACD.ConventionOfElements.getConventionElement(acd._ACD);

                                        A_Collection.Me.Party.PartyMember1_NextConventionElement =
                                            A_Tools.T_ACD.ConventionOfElements.getNextConventionElement(
                                                A_Collection.Me.Party.PartyMember1_CurrentConventionElement,
                                                (A_Enums.HeroClass) A_Collection.Me.Party.PartyMember1.x964C_HeroClass);

                                        A_Collection.Me.Party.PartyMember1_Convention_TicksLeft =
                                            A_Tools.T_ACD.ConventionOfElements.getConventionTicksLeft(
                                                A_Collection.Me.Party.PartyMember1_CurrentConventionElement, acd._ACD);
                                    }
                                    else
                                    {
                                        A_Collection.Me.Party.PartyMember1_CurrentConventionElement = DamageType.none;

                                        A_Collection.Me.Party.PartyMember1_NextConventionElement = DamageType.none;

                                        A_Collection.Me.Party.PartyMember1_Convention_TicksLeft = 0;
                                    }
                                
                                }
                                    
                            }
                            else
                            {
                                A_Collection.Me.Party.PartyMember1_CurrentConventionElement = DamageType.none;

                                A_Collection.Me.Party.PartyMember1_NextConventionElement = DamageType.none;

                                A_Collection.Me.Party.PartyMember1_Convention_TicksLeft = 0;
                            }
                            break;

                        case 2:
                            if (A_Collection.Me.Party.PartyMember2 != null)
                            {
                                var acd =
                                    A_Collection.Me.Party.PartyMemberInRange.FirstOrDefault(
                                        x => x._ACD.x08C_ActorId == A_Collection.Me.Party.PartyMember2.x0008_ActorId);
                                if (acd != null)
                                {
                                    if (acd._ACD.GetAttributeValue((AttributeId)746, A_Enums.Powers.Convention_PowerSno) > 0)
                                    {
                                        A_Collection.Me.Party.PartyMember2_CurrentConventionElement =
                                            A_Tools.T_ACD.ConventionOfElements.getConventionElement(acd._ACD);

                                        A_Collection.Me.Party.PartyMember2_NextConventionElement =
                                            A_Tools.T_ACD.ConventionOfElements.getNextConventionElement(
                                                A_Collection.Me.Party.PartyMember2_CurrentConventionElement,
                                                (A_Enums.HeroClass)A_Collection.Me.Party.PartyMember2.x964C_HeroClass);

                                        A_Collection.Me.Party.PartyMember2_Convention_TicksLeft =
                                            A_Tools.T_ACD.ConventionOfElements.getConventionTicksLeft(
                                                A_Collection.Me.Party.PartyMember2_CurrentConventionElement, acd._ACD);
                                    }
                                    else
                                    {
                                        A_Collection.Me.Party.PartyMember2_CurrentConventionElement = DamageType.none;

                                        A_Collection.Me.Party.PartyMember2_NextConventionElement = DamageType.none;

                                        A_Collection.Me.Party.PartyMember2_Convention_TicksLeft = 0;
                                    }

                                }

                            }
                            else
                            {
                                A_Collection.Me.Party.PartyMember2_CurrentConventionElement = DamageType.none;

                                A_Collection.Me.Party.PartyMember2_NextConventionElement = DamageType.none;

                                A_Collection.Me.Party.PartyMember2_Convention_TicksLeft = 0;
                            }
                            break;

                        case 3:
                            if (A_Collection.Me.Party.PartyMember3 != null)
                            {
                                var acd =
                                    A_Collection.Me.Party.PartyMemberInRange.FirstOrDefault(
                                        x => x._ACD.x08C_ActorId == A_Collection.Me.Party.PartyMember3.x0008_ActorId);
                                if (acd != null)
                                {
                                    if (acd._ACD.GetAttributeValue((AttributeId)746, A_Enums.Powers.Convention_PowerSno) > 0)
                                    {
                                        A_Collection.Me.Party.PartyMember3_CurrentConventionElement =
                                            A_Tools.T_ACD.ConventionOfElements.getConventionElement(acd._ACD);

                                        A_Collection.Me.Party.PartyMember3_NextConventionElement =
                                            A_Tools.T_ACD.ConventionOfElements.getNextConventionElement(
                                                A_Collection.Me.Party.PartyMember3_CurrentConventionElement,
                                                (A_Enums.HeroClass)A_Collection.Me.Party.PartyMember3.x964C_HeroClass);

                                        A_Collection.Me.Party.PartyMember3_Convention_TicksLeft =
                                            A_Tools.T_ACD.ConventionOfElements.getConventionTicksLeft(
                                                A_Collection.Me.Party.PartyMember3_CurrentConventionElement, acd._ACD);
                                    }
                                    else
                                    {
                                        A_Collection.Me.Party.PartyMember3_CurrentConventionElement = DamageType.none;

                                        A_Collection.Me.Party.PartyMember3_NextConventionElement = DamageType.none;

                                        A_Collection.Me.Party.PartyMember3_Convention_TicksLeft = 0;
                                    }

                                }

                            }
                            else
                            {
                                A_Collection.Me.Party.PartyMember3_CurrentConventionElement = DamageType.none;

                                A_Collection.Me.Party.PartyMember3_NextConventionElement = DamageType.none;

                                A_Collection.Me.Party.PartyMember3_Convention_TicksLeft = 0;
                            }
                            break;
                    }
                }

            }
            catch (Exception e)
            {
                A_Handler.Log.ExceptionLogEntry newEntry = new A_Handler.Log.ExceptionLogEntry(e, DateTime.Now, A_Enums.ExceptionThread.ICollector);

                lock (A_Handler.Log.Exception.ExceptionLog) A_Handler.Log.Exception.ExceptionLog.Add(newEntry);
            }
        }
    }
}
