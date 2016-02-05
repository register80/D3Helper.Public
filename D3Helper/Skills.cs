using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace D3Helper
{
    //public class SkillInfo
    //{
    //    public SkillInfo(int SNOPower, string name, int minResource, double powerDistance, Bitmap iconPath, string iconFileName )
    //    {
    //        this.SNOPower = SNOPower;
    //        this.Name = name;
    //        this.MinResource = minResource;
    //        this.PowerDistance = powerDistance;
    //        this.IconPath = iconPath;
    //        this.IconFileName = iconFileName;
    //    }

    //    public int SNOPower { get; set; }
    //    public string Name { get; set; }
    //    public int MinResource { get; set; }
    //    public double PowerDistance { get; set; }
    //    public Bitmap IconPath { get; set; }
    //    public string IconFileName { get; set; }
    //}
    //class Skills
    //{
        

    //    public static Dictionary<int, SkillInfo> ManagedSkills = new Dictionary<int,SkillInfo>()
    //    {
            
    //        {69130, new SkillInfo(69130, "Monk_BreathOfHeaven", 0, 0, Properties.Resources.monk_breathofheaven, "monk_breathofheaven")},
    //        {96215, new SkillInfo(96215, "Monk_Serenity", 0, 0, Properties.Resources.monk_serenity, "monk_serenity")},            
    //        {312307, new SkillInfo(312307, "Monk_Epiphany", 0, 0, Properties.Resources.x1_monk_epiphany, "x1_monk_epiphany")},
    //        {375049, new SkillInfo(375049, "Monk_MantraOfEvasion", 0, 0, Properties.Resources.x1_monk_mantraofevasion_v2, "x1_monk_mantraofevasion_v2")},
    //        {375082, new SkillInfo(375082, "Monk_MantraOfRetribution", 0, 0, Properties.Resources.x1_monk_mantraofretribution_v2, "x1_monk_mantraofretribution_v2")},
    //        {373143, new SkillInfo(373143, "Monk_MantraOfHealing", 0, 0, Properties.Resources.x1_monk_mantraofhealing_v2, "x1_monk_mantraofhealing_v2")},
    //        {375088, new SkillInfo(375088, "Monk_MantraOfConviction", 0, 0, Properties.Resources.x1_monk_mantraofconviction_v2, "x1_monk_mantraofconviction_v2")},
    //        {97328, new SkillInfo(97328, "Monk_ExplodingPalm", 0, 0, Properties.Resources.monk_explodingpalm, "monk_explodingpalm")},
    //        {96033, new SkillInfo(96033, "Monk_WaveOfLight", 0, 0, Properties.Resources.monk_waveoflight, "monk_waveoflight")},
    //        {136954, new SkillInfo(136954, "Monk_BlindingFlash", 0, 0, Properties.Resources.monk_blindingflash, "monk_blindingflash")},
    //        {317076, new SkillInfo(317076, "Monk_InnerSanctuary", 0, 0, Properties.Resources.x1_monk_innersanctuary, "x1_monk_innersanctuary")},
    //        {362102, new SkillInfo(362102, "Monk_MysticAlly", 0, 0, Properties.Resources.x1_monk_mystically_v2, "x1_monk_mystically_v2")},
    //        {96090, new SkillInfo(96090, "Monk_SweepingWind", 75, 0, Properties.Resources.monk_sweepingwind, "monk_sweepingwind")},
    //        {312736, new SkillInfo(312736, "Monk_DashingStrike", 75, 0, Properties.Resources.x1_monk_dashingstrike, "x1_monk_dashingstrike")},
    //        {87525, new SkillInfo(87525, "Wizard_ExplosiveBlast", 0, 0, Properties.Resources.wizard_explosiveblast, "wizard_explosiveblast")},
    //        {30718, new SkillInfo(30718, "Wizard_FrostNova", 0, 0, Properties.Resources.wizard_frostnova, "wizard_frostnova")},
    //        {76108, new SkillInfo(76108, "Wizard_MagicWeapon", 0, 0, Properties.Resources.wizard_magicweapon, "wizard_magicweapon")},
    //        {99120, new SkillInfo(99120, "Wizard_Familiar", 0, 0, Properties.Resources.wizard_familiar, "wizard_familiar")},
    //        {73223, new SkillInfo(73223, "Wizard_IceArmor", 0, 0, Properties.Resources.wizard_icearmor, "wizard_icearmor")},
    //        {74499, new SkillInfo(74499, "Wizard_StormArmor", 0, 0, Properties.Resources.wizard_stormarmor, "wizard_stormarmor")},
    //        {86991, new SkillInfo(86991, "Wizard_EnergyArmor", 0, 0, Properties.Resources.wizard_energyarmor, "wizard_energyarmor")},
    //        {75599, new SkillInfo(75599, "Wizard_DiamondSkin", 0, 0, Properties.Resources.wizard_diamondskin, "wizard_diamondskin")},
    //        {30668, new SkillInfo(30668, "Wizard_ArcaneOrb", 0, 0, Properties.Resources.wizard_arcaneorb, "wizard_arcaneorb")},
    //        {1769, new SkillInfo(1769, "Wizard_SlowTime", 0, 0, Properties.Resources.wizard_slowtime, "wizard_slowtime")},
    //        {98027, new SkillInfo(98027, "Wizard_MirrorImage", 0, 0, Properties.Resources.wizard_mirrorimage, "wizard_mirrorimage")},
    //        {342279, new SkillInfo(342279, "Crusader_LawsOfHope", 0, 60, Properties.Resources.x1_crusader_lawsofhope2, "x1_crusader_lawsofhope2")},
    //        {342280, new SkillInfo(342280, "Crusader_LawsOfJustice", 0, 60, Properties.Resources.x1_crusader_lawsofjustice2, "x1_crusader_lawsofjustice2")},
    //        {342281, new SkillInfo(342281, "Crusader_LawsOfValor", 0, 60, Properties.Resources.x1_crusader_lawsofvalor2, "x1_crusader_lawsofvalor2")},
    //        {269032, new SkillInfo(269032, "Crusader_AkaratsChampion", 0, 0, Properties.Resources.x1_crusader_akaratschampion, "x1_crusader_akaratschampion")},
    //        {243853, new SkillInfo(243853, "Crusader_SteedCharge", 0, 0, Properties.Resources.x1_crusader_steedcharge, "x1_crusader_steedcharge")},
    //        {291804, new SkillInfo(291804, "Crusader_IronSkin", 0, 0, Properties.Resources.x1_crusader_ironskin, "x1_crusader_ironskin")},
    //        {266627, new SkillInfo(266627, "Crusader_Condemn", 0, 0, Properties.Resources.x1_crusader_condemn, "x1_crusader_condemn")},
    //        {268530, new SkillInfo(268530, "Crusader_ShieldGlare", 0, 0, Properties.Resources.x1_crusader_shieldglare, "x1_crusader_shieldglare")},
    //        {290545, new SkillInfo(290545, "Crusader_Provoke", 0, 0, Properties.Resources.x1_crusader_provoke, "x1_crusader_provoke")},
    //        {273941, new SkillInfo(273941, "Crusader_Consecration", 0, 0, Properties.Resources.x1_crusader_consecration, "x1_crusader_consecration")},
    //        {330729, new SkillInfo(330729, "Crusader_Phalanx", 0, 0, Properties.Resources.x1_crusader_phalanx3, "x1_crusader_phalanx3")},
    //        {285903, new SkillInfo(285903, "Crusader_Punish", 0, 0, Properties.Resources.x1_crusader_punish, "x1_crusader_punish")},
    //        {267600, new SkillInfo(267600, "Crusader_Judgement", 0, 0, Properties.Resources.x1_crusader_judgment, "x1_crusader_judgment")},
    //        {353492, new SkillInfo(353492, "Crusader_ShieldBash", 30, 0, Properties.Resources.x1_crusader_shieldbash2, "x1_crusader_shieldbash2")},
    //        {239042, new SkillInfo(239042, "Crusader_SweepAttack", 20, 0, Properties.Resources.x1_crusader_sweepattack, "x1_crusader_sweepattack")},
    //        {67668, new SkillInfo(67668, "WitchDoctor_Horrify", 0, 0, Properties.Resources.witchdoctor_horrify, "witchdoctor_horrify")},
    //        {106237, new SkillInfo(106237, "WitchDoctor_SpiritWalk", 0, 0, Properties.Resources.witchdoctor_spiritwalk, "witchdoctor_spiritwalk")},
    //        {83602, new SkillInfo(83602, "WitchDoctor_Haunt", 50, 0, Properties.Resources.witchdoctor_haunt, "witchdoctor_haunt")},
    //        {72785, new SkillInfo(72785, "WitchDoctor_FetishArmy", 0, 0, Properties.Resources.witchdoctor_fetisharmy, "witchdoctor_fetisharmy")},
    //        {67600, new SkillInfo(67600, "WitchDoctor_MassConfusion", 0, 0, Properties.Resources.witchdoctor_massconfusion, "witchdoctor_massconfusion")},
    //        {102573, new SkillInfo(102573, "WitchDoctor_SummonZombieDog", 0, 0, Properties.Resources.witchdoctor_summonzombiedog, "witchdoctor_summonzombiedog")},
    //        {102572, new SkillInfo(102572, "WitchDoctor_Sacrifice", 0, 0, Properties.Resources.witchdoctor_sacrifice, "witchdoctor_sacrifice")},
    //        {117402, new SkillInfo(117402, "WitchDoctor_BigBadVoodoo", 0, 0, Properties.Resources.witchdoctor_bigbadvoodoo, "witchdoctor_bigbadvoodoo")},
    //        {30631, new SkillInfo(30631, "WitchDoctor_Hex", 0, 0, Properties.Resources.witchdoctor_hex, "witchdoctor_hex")},
    //        {347265, new SkillInfo(347265, "WitchDoctor_Piranhas", 250, 0, Properties.Resources.Witchdoctor_Piranhas_Normal, "Witchdoctor_Piranhas_Normal")},
    //        {67616, new SkillInfo(67616, "WitchDoctor_SoulHarvest", 0, 18, Properties.Resources.witchdoctor_soulharvest, "witchdoctor_soulharvest")},
    //        {69866, new SkillInfo(69866, "WitchDoctor_CorpseSpiders", 0, 0, Properties.Resources.witchdoctor_corpsespider, "witchdoctor_corpsespider")},
    //        {78551, new SkillInfo(78551, "Barbarian_Sprint", 0, 50, Properties.Resources.barbarian_sprint, "barbarian_sprint")},
    //        {375483, new SkillInfo(375483, "Barbarian_Warcry", 0, 100, Properties.Resources.x1_barbarian_warcry_v2, "x1_barbarian_warcry_v2")},
    //        {159169, new SkillInfo(159169, "Barbarian_Overpower", 0, 0, Properties.Resources.barbarian_overpower, "barbarian_overpower")},
    //        {79076, new SkillInfo(79076, "Barbarian_Battlerage", 0, 0, Properties.Resources.barbarian_battlerage, "barbarian_battlerage")},
    //        {79607, new SkillInfo(79607, "Barbarian_WrathOfTheBerserker", 0, 0, Properties.Resources.barbarian_wrathoftheberserker, "barbarian_wrathoftheberserker")},
    //        {79528, new SkillInfo(79528, "Barbarian_IgnorePain", 0, 0, Properties.Resources.barbarian_ignorepain, "barbarian_ignorepain")},
    //        {80049, new SkillInfo(80049, "Barbarian_CallOfTheAncients", 0, 0, Properties.Resources.barbarian_calloftheancients, "barbarian_calloftheancients")},
    //        {79077, new SkillInfo(79077, "Barbarian_ThreateningShout", 0, 25, Properties.Resources.barbarian_threateningshout, "barbarian_threateningshout")},
    //        {97435, new SkillInfo(97435, "Barbarian_ForiousCharge", 0, 0, Properties.Resources.barbarian_furiouscharge, "barbarian_furiouscharge")},
    //        {109342, new SkillInfo(109342, "Barbarian_Revenge", 0, 0, Properties.Resources.barbarian_revenge, "barbarian_revenge")},
    //        {365311, new SkillInfo(365311, "Demonhunter_Companion", 0, 0, Properties.Resources.x1_demonhunter_companion, "x1_demonhunter_companion")},
    //        {130830, new SkillInfo(130830, "Demonhunter_ShadowPower", 0, 0, Properties.Resources.demonhunter_shadowpower, "demonhunter_shadowpower")},
    //        {129212, new SkillInfo(129212, "Demonhunter_Preparation", 0, 0, Properties.Resources.demonhunter_preparation, "demonhunter_preparation")},
    //        {130695, new SkillInfo(130695, "Demonhunter_Smokescreen", 0, 0, Properties.Resources.demonhunter_smokescreen, "demonhunter_smokescreen")},
    //        {130831, new SkillInfo(130831, "Demonhunter_RainOfVengeance", 0, 0, Properties.Resources.demonhunter_rainofvengeance, "demonhunter_rainofvengeance")},
    //        {302846, new SkillInfo(302846, "Demonhunter_Vengeance", 0, 0, Properties.Resources.x1_demonhunter_vengeance, "x1_demonhunter_vengeance")},
    //        {129216, new SkillInfo(129216, "Demonhunter_Caltrops", 6, 12, Properties.Resources.demonhunter_caltrops, "demonhunter_caltrops")},
    //        {130738, new SkillInfo(130738, "Demonhunter_MarkedForDeath", 3, 0, Properties.Resources.demonhunter_markedfordeath, "demonhunter_markedfordeath")},
    //        {77649, new SkillInfo(77649, "Demonhunter_Multishot", 25, 0, Properties.Resources.demonhunter_multishot, "demonhunter_multishot")},
    //        {377450, new SkillInfo(377450, "Demonhunter_EvasiveFire", 0, 0, Properties.Resources.x1_demonhunter_evasivefire, "x1_demonhunter_evasivefire")}
            

    //    };
       
    //    public static Dictionary<string, int> BuffIcons = new Dictionary<string, int>()
    //    {
    //        {"Monk_MantraOfSalvation", 375050},
    //        {"Monk_MantraOfRetribution", 375083},
    //        {"Monk_MantraOfHealing", 373154},
    //        {"Monk_MantraOfConviction", 375089},
            
    //    };
    //}
}
