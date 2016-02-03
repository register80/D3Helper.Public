using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Enigma.D3.Helpers;

namespace D3Helper.A_Tools.Skills
{
    class Skills
    {
        public class S_Maths
        {
            public class FuriousCharge
            {
                public static int get_NumberOfMonstersToHit()
                {
                    try
                    {
                        double cdr;
                        lock(A_Collection.Me.HeroGlobals.LocalACD) cdr = A_Collection.Me.HeroGlobals.LocalACD.GetAttributeValue(Enigma.D3.Enums.AttributeId.PowerCooldownReductionPercentAll);
                        int rechargeTimeinSec = 10;
                        double timeToRecharge = (1 - cdr) * rechargeTimeinSec;
                        int reducePerHitMob = 2;

                        int MobsToHit = Convert.ToInt32(Math.Round(timeToRecharge / reducePerHitMob, 0, MidpointRounding.AwayFromZero));

                        return MobsToHit;
                    }
                    catch { return 100; }
                }
            }
        }
        public class S_Global
        {
            public static int get_EquippedSkillRune(int SkillPowerSnoId)
            {
                try
                {
                    lock (A_Collection.Me.HeroDetails.ActiveSkills)
                    {
                        if (A_Collection.Me.HeroDetails.ActiveSkills.Count >= 5)
                        {
                            int Rune = A_Collection.Me.HeroDetails.ActiveSkills[SkillPowerSnoId];

                            return Rune;
                        }
                        return - 1;
                    }
                }
                catch { return -1; }
            }
            public static bool isDisabled(int PowerSnoId)
            {
                try
                {
                    lock(A_Collection.Skills.UI_Controls.SkillControls)
                    {
                        var isDisabled = A_Collection.Skills.UI_Controls.SkillControls.FirstOrDefault(x => x.x166C_PowerSnoId == PowerSnoId).x1460_IsDisabled;

                        if (isDisabled == 1)
                        {
                            return true;
                        }
                        return false;
                    }
                }
                catch { return true; }
            }
            public static bool isOnCooldown(int PowerSnoId)
            {
                try
                {
                    lock(A_Collection.Me.HeroGlobals.LocalACD)
                    {
                        if (A_Collection.Me.HeroGlobals.LocalACD != null && A_Collection.Me.HeroGlobals.LocalACD.x188_Hitpoints > 0)
                        {
                            var cooldown = A_Collection.Me.HeroGlobals.LocalACD.GetAttributeValue(Enigma.D3.Enums.AttributeId.PowerCooldown, PowerSnoId);
                            
                            if (cooldown == -1)
                            {
                                return false;
                            }
                        }
                        return true;
                    }
                }
                catch { return true; }
            }
            public static int get_Charges(int PowerSnoId)
            {
                try
                {
                    lock(A_Collection.Me.HeroGlobals.LocalACD)
                    {
                        return (int)A_Collection.Me.HeroGlobals.LocalACD.GetAttributeValue(Enigma.D3.Enums.AttributeId.SkillCharges, PowerSnoId);
                    }
                }
                catch { return 0; }
            }
        }
        public class S_Player
        {
            public static bool isPotionReady()
            {
                try
                {
                    lock(A_Collection.Me.HeroGlobals.LocalACD)
                    {
                        var potionCooldown = A_Collection.Me.HeroGlobals.LocalACD.GetAttributeValue(Enigma.D3.Enums.AttributeId.PowerCooldown, A_Enums.Powers.DrinkHealthPotion);
                        if (potionCooldown == -1)
                        {
                            return true;
                        }
                        return false;
                    }
                }
                catch { return false; }
            }
        }
    }
}
