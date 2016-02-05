using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Enigma.D3;
using Enigma.D3.Enums;
using Enigma.D3.Helpers;

namespace D3Helper.A_Tools.Buffs
{
    class Buffs
    {
        public class B_Global
        {
            public static int get_PartyMembers_WithBuff(int SnoPowerId, int AttribId, double PowerMaxDistance, out int PartyMembersInRange)
            {
                PartyMembersInRange = 0;

                try
                {
                    var local = A_Collection.Me.HeroGlobals.LocalACD;

                    List<A_Collector.ACD> partyMemberContainer;
                    lock (A_Collection.Environment.Actors.AllActors)
                        partyMemberContainer =
                            A_Collection.Environment.Actors.AllActors.ToList()
                                .Where(x => x._ACD.x184_ActorType == Enigma.D3.Enums.ActorType.Player)
                                .ToList();

                    int counter = 0;

                    if (partyMemberContainer.Count > 1)
                        PartyMembersInRange = partyMemberContainer.Count - 1;

                    foreach (var member in partyMemberContainer)
                    {

                        if (member.Distance <= PowerMaxDistance && member.Distance >= 3)
                        {
                            if (A_Tools.T_ACD.isBuff(SnoPowerId, AttribId, member._ACD))
                            {
                                counter++;
                            }

                        }

                    }

                    return counter;


                }
                catch
                {
                    return 0;
                }
            }
            public static int get_PartyMembers_WithoutBuff(int SnoPowerId, int AttribId, double PowerMaxDistance, out int PartyMembersInRange)
            {
                PartyMembersInRange = 0;

                try
                {
                    var local = A_Collection.Me.HeroGlobals.LocalACD;

                    List<A_Collector.ACD> partyMemberContainer;
                    lock (A_Collection.Environment.Actors.AllActors)
                        partyMemberContainer =
                            A_Collection.Environment.Actors.AllActors.ToList()
                                .Where(x => x._ACD.x184_ActorType == Enigma.D3.Enums.ActorType.Player)
                                .ToList();

                    int counter = 0;

                    if (partyMemberContainer.Count > 1)
                        PartyMembersInRange = partyMemberContainer.Count - 1;
                   
                    foreach (var member in partyMemberContainer)
                    {

                        if (member.Distance <= PowerMaxDistance && member.Distance >= 3)
                        {
                            if (!A_Tools.T_ACD.isBuff(SnoPowerId, AttribId, member._ACD))
                            {
                                counter++;
                            }

                        }

                    }

                    return counter;


                }
                catch
                {
                    return 0;
                }
            }
        }
        public class B_WitchDoctor
        {
            public static int get_ZombieDogCount()
            {
                try
                {
                    List<A_Collector.ACD> acdcontainer;
                    lock (A_Collection.Environment.Actors.AllActors) acdcontainer = A_Collection.Environment.Actors.AllActors.ToList();

                    PlayerData local;
                    lock (A_Collection.Me.HeroGlobals.LocalPlayerData)
                        local = A_Collection.Me.HeroGlobals.LocalPlayerData;
                    var alldogs = acdcontainer.Where(x => x._ACD.x000_Id != -1 && x._ACD.GetAttributeValue(AttributeId.PetOwner) == local.x0000_Index && x._ACD.x004_Name.ToLower().Contains("zombiedog"));

                    return alldogs.Count(x => x._ACD.GetAttributeValue(AttributeId.PetType) == 8);
                }
                catch { return 0; }
            }
            public static int get_FetishCount()
            {
                try
                {
                    List<A_Collector.ACD> acdcontainer;
                    lock (A_Collection.Environment.Actors.AllActors) acdcontainer = A_Collection.Environment.Actors.AllActors.ToList();

                    PlayerData local;
                    lock (A_Collection.Me.HeroGlobals.LocalPlayerData)
                        local = A_Collection.Me.HeroGlobals.LocalPlayerData;

                    var allfetishes = acdcontainer.Where(x => x._ACD.x000_Id != -1 && x._ACD.GetAttributeValue(AttributeId.PetOwner) == local.x0000_Index && x._ACD.x004_Name.ToLower().Contains("fetish"));

                    return allfetishes.Count(x => x._ACD.GetAttributeValue(AttributeId.PetType) == 11);
                }
                catch { return 0; }
            }
            public static int get_GargantuanCount()
            {
                try
                {
                    List<A_Collector.ACD> acdcontainer;
                    lock (A_Collection.Environment.Actors.AllActors) acdcontainer = A_Collection.Environment.Actors.AllActors.ToList();

                    PlayerData local;
                    lock (A_Collection.Me.HeroGlobals.LocalPlayerData)
                        local = A_Collection.Me.HeroGlobals.LocalPlayerData;

                    var allgargantuans = acdcontainer.Where(x => x._ACD.x000_Id != -1 && x._ACD.GetAttributeValue(AttributeId.PetOwner) == local.x0000_Index && x._ACD.x004_Name.ToLower().Contains("gargantuan"));

                    return allgargantuans.Count(x => x._ACD.GetAttributeValue(AttributeId.PetType) == 10);
                }
                catch { return 0; }
            }
        }
        public class B_Crusader
        {
            
        }
        public class B_Monk
        {
            
        }
        public class B_Barbarian
        {
            
        }
        public class B_DemonHunter
        {
            public static bool isPetAlive(int PetType)
            {
                try
                {
                    int localindex = A_Collection.Me.HeroGlobals.LocalDataIndex;

                    List<A_Collector.ACD> acds;

                    lock (A_Collection.Environment.Actors.AllActors) acds = A_Collection.Environment.Actors.AllActors.ToList();

                    var pet = acds.FirstOrDefault(x => x._ACD.x000_Id != -1 && Attributes.PetOwner.GetValue(x._ACD) == localindex && Attributes.PetType.GetValue(x._ACD) == PetType);

                    //for (int i = 0; i < acdcontainer.Count; i++)
                    //{
                    //    try
                    //    {
                    //        var acd = acdcontainer[i]._ACD;
                    //        acd.TakeSnapshot();

                    //        var petowner = Attributes.PetOwner.GetValue(acd);

                    //        if (petowner < 0) { continue; }

                    //        var pettype = Attributes.PetType.GetValue(acd);

                    //        if (petowner == localindex && pettype == PetType)
                    //        {
                    //            return true;
                    //        }

                    //        acd.FreeSnapshot();
                    //    }
                    //    catch { }
                    //}

                    //return false;

                    //var trygetpet = acdcontainer.Where(x => x._ACD.x000_Id != -1).ToList().FirstOrDefault(x => x._ACD.GetAttributeValue(Enigma.D3.Enums.AttributeId.PetOwner) == localindex && x._ACD.GetAttributeValue(Enigma.D3.Enums.AttributeId.PetType) == PetType);

                    if (pet != null)
                    {
                        return true;
                    }
                    return false;
                }
                catch { return false; }
            }
        }
    }
}
