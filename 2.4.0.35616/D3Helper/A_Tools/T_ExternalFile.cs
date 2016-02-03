using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using D3Helper.A_Collection;
using D3Helper.A_Enums;
using D3Helper.A_Handler.SkillBuildSwap;
using ProtoBuf;

namespace D3Helper.A_Tools
{
    public class ParagonPointSetup
    {
        public ParagonPointSetup(int id, string name, List<BonusPoint> bonusPoints)
        {
            this.ID = id;
            this.Name = name;
            this.BonusPoints = bonusPoints;
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public List<BonusPoint> BonusPoints { get; set; }
    }

    public class BonusPoint
    {
        public BonusPoint(BonusPoints type, int value, int maxvalue)
        {
            this.Type = type;
            this.Value = value;
            this.MaxValue = maxvalue;
        }
        public BonusPoints Type { get; set; }
        public int Value { get; set; }
        public int MaxValue { get; set; }
    }

    

    public class GearSwapItem
    {
        public GearSwapItem(long heroID, int swapId, double itemSeed, int itemSlotX, int itemSlotY,
            ItemSlotSize itemSize)
        {
            this.HeroID = heroID;
            this.SwapId = swapId;
            this.ItemSeed = itemSeed;
            this.ItemSlotX = itemSlotX;
            this.ItemSlotY = itemSlotY;
            this.ItemSize = itemSize;
        }

        public long HeroID { get; set; }
        public int SwapId { get; set; }
        public double ItemSeed { get; set; }
        public int ItemSlotX { get; set; }
        public int ItemSlotY { get; set; }
        public ItemSlotSize ItemSize { get; set; }
    }

    public enum ItemSlotSize
    {
        _1x1 = 0,
        _2x1 = 1

    }

    public class Override
    {
        public Override(long heroId, int powerSnoId, bool enabled)
        {
            this.HeroId = heroId;
            this.PowerSnoId = powerSnoId;
            this.Enabled = enabled;
        }

        public long HeroId { get; set; }
        public int PowerSnoId { get; set; }
        public bool Enabled { get; set; }
    }

    internal class T_ExternalFile
    {

        public static bool AutoCastOverrides_changed = false;

        public class AreaList
        {
            public static void Load()
            {
                string resource_data = Properties.Resources.levelarea;
                List<string> lines =
                    resource_data.Split(new[] {System.Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries)
                        .ToList();

                for (int i = 1; i < lines.Count; i++)
                {
                    try
                    {
                        var line = lines[i];

                        var splitline = line.Split(',');

                        int sno = Convert.ToInt32(splitline[0]);
                        int act = Convert.ToInt32(splitline[1]);
                        string internalname = splitline[2];
                        string name = splitline[3];
                        int hostareasno = Convert.ToInt32(splitline[4]);

                        A_Collection.Environment.Areas.AreaList.Add(new SNOid(sno),
                            new LevelArea(act, internalname, name, hostareasno));
                    }
                    catch
                    {
                    }
                }
            }
        }

        public class SkillPowers
        {
            public static void Load()
            {
                try
                {
                    List<string> AllLines =
                        Properties.Resources.power_stats.Split(new[] {System.Environment.NewLine},
                            StringSplitOptions.RemoveEmptyEntries).ToList();

                    for (int i = 1; i < AllLines.Count(); i++)
                    {
                        var LineSplit = AllLines[i].Split('\t');

                        int PowerSNO = int.Parse(LineSplit[0]);
                        int RuneIndex = int.Parse(LineSplit[1]) - 1;
                        DamageType damagetype = (DamageType) int.Parse(LineSplit[2]);
                        int ResourceCost = int.Parse(LineSplit[5]);
                        int Cooldown = int.Parse(LineSplit[6]);
                        string SkillName = LineSplit[7];
                        string RuneName = LineSplit[8];
                        bool IsPrimaryResource = true;
                        bool IsCooldownSpell = false;

                        if (ResourceCost < -1)
                            IsPrimaryResource = false;

                        if (Cooldown > -1)
                            IsCooldownSpell = true;


                        var tryGetEntry =
                            Presets.SkillPowers.AllSkillPowers.FirstOrDefault(
                                x => x.Name == SkillName && x.PowerSNO == PowerSNO);

                        if (tryGetEntry == null && RuneIndex == -1)
                        {
                            if (RuneName == "No Rune")
                                RuneName = "Any";

                            Presets.SkillPowers.AllSkillPowers.Add(new SkillPower(PowerSNO, SkillName,
                                new List<Rune>() {new Rune(RuneName, -1, damagetype)}, ResourceCost, IsPrimaryResource,
                                IsCooldownSpell));
                        }
                        else
                            tryGetEntry.Runes.Add(new Rune(RuneName, RuneIndex, damagetype));
                    }
                }
                catch
                {
                }

            }
        }

        public class SNO_Powers
        {
            public static void Load()
            {
                try
                {
                    List<string> AllLines =
                        Properties.Resources.Powers.Split(new[] {System.Environment.NewLine},
                            StringSplitOptions.RemoveEmptyEntries).ToList();

                    for (int i = 1; i < AllLines.Count(); i++)
                    {
                        var LineSplit = AllLines[i].Split('\t');

                        int PowerSNO = int.Parse(LineSplit[0]);

                        string PowerName = LineSplit[1];

                        A_Collection.Presets.SNOPowers.AllPowers.Add(PowerSNO, PowerName);

                    }
                }
                catch
                {
                }
            }
        }

        public class Monsters
        {
            public class RiftProgress
            {
                public static void Load()
                {
                    try
                    {
                        List<string> AllLines =
                            Properties.Resources.monster.Split(new[] {"\n"}, StringSplitOptions.RemoveEmptyEntries)
                                .ToList();

                        for (int i = 0; i < AllLines.Count(); i++)
                        {
                            var LineSplit = AllLines[i].Split('\t');

                            int PowerSNO = int.Parse(LineSplit[0]);

                            double ProgressPoints = double.Parse(LineSplit[5]);

                            A_Collection.Presets.Monsters.Monsters_RiftProgress.Add(PowerSNO, ProgressPoints);

                        }
                    }
                    catch (Exception)
                    {

                    }
                }
            }
        }

        public class ParagonPointSpenderSettings
        {
           
            public static void Load()
            {
                if (File.Exists(A_Collection.Me.ParagonPointSpender.ParagonPoints_FilePath))
                {
                    Dictionary<long, List<ParagonPointSetup>> Buffer = new Dictionary<long, List<ParagonPointSetup>>();

                    var Lines = File.ReadAllLines(A_Collection.Me.ParagonPointSpender.ParagonPoints_FilePath);

                    foreach (var line in Lines)
                    {
                        var split = line.Split('\t');

                        long heroid = long.Parse(split[0]);
                        int id = int.Parse(split[1]);
                        string name = split[2];

                        var bonuspoints = split[3];

                        var b_split = bonuspoints.Split(':');

                        foreach (var bonuspoint in b_split)
                        {
                            var _split = bonuspoint.Split('|');

                            BonusPoints Type = (BonusPoints) Enum.Parse(typeof (BonusPoints), _split[0]);
                            int Value = int.Parse(_split[1]);
                            int MaxValue = int.Parse(_split[2]);

                            if (!Buffer.ContainsKey(heroid))
                                Buffer.Add(heroid, new List<ParagonPointSetup>());

                            if(Buffer[heroid].FirstOrDefault(x => x.ID == id) == null)
                                Buffer[heroid].Add(new ParagonPointSetup(id, name, new List<BonusPoint>()));

                            Buffer[heroid].FirstOrDefault(x => x.ID == id).BonusPoints.Add(new BonusPoint(Type, Value, MaxValue));
                        }
                    }

                    A_Collection.Me.ParagonPointSpender.Setups = Buffer;
                }
            }

            public static void Save()
            {
                //heroid,id,setupname,bonuspoints(type|value|maxvalue)
                //12345\tid\tsomename\t0|50|50:1|-1|-1:2|50|50

                if (!File.Exists(A_Collection.Me.ParagonPointSpender.ParagonPoints_FilePath))
                    File.Create(A_Collection.Me.ParagonPointSpender.ParagonPoints_FilePath).Close();

                string output = "";

                foreach (var hero in A_Collection.Me.ParagonPointSpender.Setups)
                {
                    
                    foreach (var setup in hero.Value)
                    {
                        output += hero.Key + "\t" + setup.ID + "\t" + setup.Name + "\t";

                        foreach (var bonuspoint in setup.BonusPoints)
                        {
                            output += (int) bonuspoint.Type + "|" + bonuspoint.Value + "|" + bonuspoint.MaxValue + ":";
                        }

                        output = output.TrimEnd(':');
                        output += System.Environment.NewLine;
                    }
                }

                File.WriteAllText(A_Collection.Me.ParagonPointSpender.ParagonPoints_FilePath, output);
            }
        }

        public class GearSwaps
        {
            public static void Load()
            {
                try
                {
                    if (File.Exists(A_Collection.Me.GearSwap.GearSwaps_FilePath))
                    {
                        List<GearSwapItem> _GearSwaps = new List<GearSwapItem>();

                        var Lines = File.ReadAllLines(A_Collection.Me.GearSwap.GearSwaps_FilePath);

                        foreach (var line in Lines)
                        {
                            var splitContent = line.Split('\t');

                            long HeroId = long.Parse(splitContent[0]);
                            double ItemSeed = double.Parse(splitContent[1]);
                            ItemSlotSize ItemSize =
                                (ItemSlotSize) System.Enum.Parse(typeof (ItemSlotSize), splitContent[2]);
                            int ItemSlotX = int.Parse(splitContent[3]);
                            int ItemSlotY = int.Parse(splitContent[4]);
                            int SwapId = int.Parse(splitContent[5]);

                            _GearSwaps.Add(new GearSwapItem(HeroId, SwapId, ItemSeed, ItemSlotX, ItemSlotY, ItemSize));
                        }

                        A_Collection.Me.GearSwap.GearSwaps = _GearSwaps;
                    }
                }
                catch
                {
                }
            }

            public static void Save()
            {
                try
                {
                    var _GearSwaps = A_Collection.Me.GearSwap.GearSwaps;

                    string Content = "";

                    foreach (var Swap in _GearSwaps)
                    {
                        Content =
                            Content +
                            Swap.HeroID + "\t" +
                            Swap.ItemSeed + "\t" +
                            Swap.ItemSize + "\t" +
                            Swap.ItemSlotX + "\t" +
                            Swap.ItemSlotY + "\t" +
                            Swap.SwapId +
                            System.Environment.NewLine;
                    }

                    File.WriteAllText(A_Collection.Me.GearSwap.GearSwaps_FilePath, Content);
                }
                catch
                {
                }
            }
        }

        public class AutoCastOverrides
        {
            public static void UpdateOverrides()
            {
                lock (A_Collection.Me.AutoCastOverrides._Overrides)
                {
                    long HeroId = A_Collection.Me.HeroGlobals.HeroID;

                    List<bool> Buffer = new List<bool>()
                    {
                        A_Collection.Me.AutoCastOverrides.AutoCast1Override,
                        A_Collection.Me.AutoCastOverrides.AutoCast2Override,
                        A_Collection.Me.AutoCastOverrides.AutoCast3Override,
                        A_Collection.Me.AutoCastOverrides.AutoCast4Override,
                        A_Collection.Me.AutoCastOverrides.AutoCastRMBOverride,
                        A_Collection.Me.AutoCastOverrides.AutoCastLMBOverride
                    };

                    if (HeroId != 0)
                    {
                        if (A_Collection.Skills.SkillInfos._HotBar1Skill != null)
                        {
                            var tryGetEntry1 =
                                A_Collection.Me.AutoCastOverrides._Overrides.FirstOrDefault(
                                    x =>
                                        x.HeroId == HeroId &&
                                        x.PowerSnoId == A_Collection.Skills.SkillInfos._HotBar1Skill.PowerSNO);
                            if (tryGetEntry1 != null)
                            {
                                A_Collection.Me.AutoCastOverrides.AutoCast1Override = tryGetEntry1.Enabled;
                            }
                            else
                            {
                                A_Collection.Me.AutoCastOverrides.AutoCast1Override = false;
                            }
                        }

                        if (A_Collection.Skills.SkillInfos._HotBar2Skill != null)
                        {
                            var tryGetEntry2 =
                                A_Collection.Me.AutoCastOverrides._Overrides.FirstOrDefault(
                                    x =>
                                        x.HeroId == HeroId &&
                                        x.PowerSnoId == A_Collection.Skills.SkillInfos._HotBar2Skill.PowerSNO);
                            if (tryGetEntry2 != null)
                            {
                                A_Collection.Me.AutoCastOverrides.AutoCast2Override = tryGetEntry2.Enabled;
                            }
                            else
                            {
                                A_Collection.Me.AutoCastOverrides.AutoCast2Override = false;
                            }
                        }

                        if (A_Collection.Skills.SkillInfos._HotBar3Skill != null)
                        {
                            var tryGetEntry3 =
                                A_Collection.Me.AutoCastOverrides._Overrides.FirstOrDefault(
                                    x =>
                                        x.HeroId == HeroId &&
                                        x.PowerSnoId == A_Collection.Skills.SkillInfos._HotBar3Skill.PowerSNO);
                            if (tryGetEntry3 != null)
                            {
                                A_Collection.Me.AutoCastOverrides.AutoCast3Override = tryGetEntry3.Enabled;
                            }
                            else
                            {
                                A_Collection.Me.AutoCastOverrides.AutoCast3Override = false;
                            }
                        }

                        if (A_Collection.Skills.SkillInfos._HotBar4Skill != null)
                        {
                            var tryGetEntry4 =
                                A_Collection.Me.AutoCastOverrides._Overrides.FirstOrDefault(
                                    x =>
                                        x.HeroId == HeroId &&
                                        x.PowerSnoId == A_Collection.Skills.SkillInfos._HotBar4Skill.PowerSNO);
                            if (tryGetEntry4 != null)
                            {
                                A_Collection.Me.AutoCastOverrides.AutoCast4Override = tryGetEntry4.Enabled;
                            }
                            else
                            {
                                A_Collection.Me.AutoCastOverrides.AutoCast4Override = false;
                            }
                        }

                        if (A_Collection.Skills.SkillInfos._HotBarRightClickSkill != null)
                        {
                            var tryGetEntryRMB =
                                A_Collection.Me.AutoCastOverrides._Overrides.FirstOrDefault(
                                    x =>
                                        x.HeroId == HeroId &&
                                        x.PowerSnoId == A_Collection.Skills.SkillInfos._HotBarRightClickSkill.PowerSNO);
                            if (tryGetEntryRMB != null)
                            {
                                A_Collection.Me.AutoCastOverrides.AutoCastRMBOverride = tryGetEntryRMB.Enabled;
                            }
                            else
                            {
                                A_Collection.Me.AutoCastOverrides.AutoCastRMBOverride = false;
                            }
                        }

                        if (A_Collection.Skills.SkillInfos._HotBarLeftClickSkill != null)
                        {
                            var tryGetEntryLMB =
                                A_Collection.Me.AutoCastOverrides._Overrides.FirstOrDefault(
                                    x =>
                                        x.HeroId == HeroId &&
                                        x.PowerSnoId == A_Collection.Skills.SkillInfos._HotBarLeftClickSkill.PowerSNO);
                            if (tryGetEntryLMB != null)
                            {
                                A_Collection.Me.AutoCastOverrides.AutoCastLMBOverride = tryGetEntryLMB.Enabled;
                            }
                            else
                            {
                                A_Collection.Me.AutoCastOverrides.AutoCastLMBOverride = false;
                            }
                        }

                        List<bool> AutoCastOverrides = new List<bool>()
                        {
                            A_Collection.Me.AutoCastOverrides.AutoCast1Override,
                            A_Collection.Me.AutoCastOverrides.AutoCast2Override,
                            A_Collection.Me.AutoCastOverrides.AutoCast3Override,
                            A_Collection.Me.AutoCastOverrides.AutoCast4Override,
                            A_Collection.Me.AutoCastOverrides.AutoCastRMBOverride,
                            A_Collection.Me.AutoCastOverrides.AutoCastLMBOverride
                        };

                        List<A_Collection.SkillPower> AutoCastSkill = new List<A_Collection.SkillPower>()
                        {
                            A_Collection.Skills.SkillInfos._HotBar1Skill,
                            A_Collection.Skills.SkillInfos._HotBar2Skill,
                            A_Collection.Skills.SkillInfos._HotBar3Skill,
                            A_Collection.Skills.SkillInfos._HotBar4Skill,
                            A_Collection.Skills.SkillInfos._HotBarRightClickSkill,
                            A_Collection.Skills.SkillInfos._HotBarLeftClickSkill
                        };

                        bool CollectionChanged = false;

                        for (int i = 0; i < AutoCastSkill.Count; i++)
                        {
                            if (AutoCastSkill[i] != null)
                            {
                                var tryGetEntry =
                                    A_Collection.Me.AutoCastOverrides._Overrides.FirstOrDefault(
                                        x => x.HeroId == HeroId && x.PowerSnoId == AutoCastSkill[i].PowerSNO);

                                if (tryGetEntry == null)
                                {
                                    A_Collection.Me.AutoCastOverrides._Overrides.Add(new Override(HeroId,
                                        AutoCastSkill[i].PowerSNO, AutoCastOverrides[i]));
                                    CollectionChanged = true;
                                }
                                else
                                {
                                    if (AutoCastOverrides[i] != Buffer[i])
                                    {
                                        A_Collection.Me.AutoCastOverrides._Overrides.Remove(tryGetEntry);

                                        A_Collection.Me.AutoCastOverrides._Overrides.Add(new Override(HeroId,
                                            AutoCastSkill[i].PowerSNO, AutoCastOverrides[i]));
                                        CollectionChanged = true;
                                    }

                                }



                            }
                        }

                        if (CollectionChanged)
                        {
                            Save();
                        }
                    }
                }
            }

            public static void ChangeOverrides(int SkillSlot)
            {
                try
                {
                    lock (A_Collection.Me.AutoCastOverrides._Overrides)
                    {
                        List<A_Collection.SkillPower> AutoCastSkill = new List<A_Collection.SkillPower>()
                        {
                            A_Collection.Skills.SkillInfos._HotBar1Skill,
                            A_Collection.Skills.SkillInfos._HotBar2Skill,
                            A_Collection.Skills.SkillInfos._HotBar3Skill,
                            A_Collection.Skills.SkillInfos._HotBar4Skill,
                            A_Collection.Skills.SkillInfos._HotBarRightClickSkill,
                            A_Collection.Skills.SkillInfos._HotBarLeftClickSkill
                        };

                        if (AutoCastSkill[SkillSlot] != null)
                        {
                            long HeroId = A_Collection.Me.HeroGlobals.HeroID;

                            var tryGetEntry =
                                A_Collection.Me.AutoCastOverrides._Overrides.ToList()
                                    .FirstOrDefault(
                                        x => x.HeroId == HeroId && x.PowerSnoId == AutoCastSkill[SkillSlot].PowerSNO);

                            if (tryGetEntry != null)
                            {
                                A_Collection.Me.AutoCastOverrides._Overrides.Remove(tryGetEntry);

                                if (tryGetEntry.Enabled)
                                {
                                    A_Collection.Me.AutoCastOverrides._Overrides.Add(new Override(HeroId,
                                        AutoCastSkill[SkillSlot].PowerSNO, false));
                                }
                                else
                                {
                                    A_Collection.Me.AutoCastOverrides._Overrides.Add(new Override(HeroId,
                                        AutoCastSkill[SkillSlot].PowerSNO, true));
                                }
                            }
                        }
                    }
                }
                catch
                {
                }
            }

            public static void Save() // Save All Overrides
            {
                if (A_Collection.Me.AutoCastOverrides._Overrides.Count > 0)
                {
                    File.Delete(A_Collection.Me.AutoCastOverrides.FilePath);

                    string Text = "";

                    foreach (var _override in A_Collection.Me.AutoCastOverrides._Overrides)
                    {
                        Text = Text + _override.HeroId + "\t" + _override.PowerSnoId + "\t" + _override.Enabled +
                               System.Environment.NewLine;
                    }

                    File.WriteAllText(A_Collection.Me.AutoCastOverrides.FilePath, Text);
                }
            }

            public static void Load() // Load All Overrides 
            {
                // File Struct:
                // HeroId \t PowerSnoId \t Enabled(True/False)

                if (File.Exists(A_Collection.Me.AutoCastOverrides.FilePath))
                {
                    var Lines = File.ReadAllLines(A_Collection.Me.AutoCastOverrides.FilePath);

                    if (Lines.Count() > 0)
                    {
                        foreach (var line in Lines)
                        {
                            if (line.Length > 5)
                            {
                                var Split = line.Split('\t');

                                long HeroId = long.Parse(Split[0]);
                                int PowerSnoId = int.Parse(Split[1]);
                                bool Enabled = bool.Parse(Split[2]);

                                Override newOverride = new Override(HeroId, PowerSnoId, Enabled);
                                A_Collection.Me.AutoCastOverrides._Overrides.Add(newOverride);
                            }
                        }
                    }
                }
            }
        }

        public class CustomSkillDefinitions
        {
            [ProtoContract]
            private struct _SkillPower
            {
                [ProtoMember(1)]
                public int PowerSNO { get; set; }

                [ProtoMember(2)]
                public string Name { get; set; }

                [ProtoMember(3)]
                public List<_Rune> Runes { get; set; }

                [ProtoMember(4)]
                public int ResourceCost { get; set; }

                [ProtoMember(5)]
                public bool IsPrimaryResource { get; set; }

            }

            [ProtoContract]
            private struct _SkillData
            {
                [ProtoMember(1)]
                public _SkillPower Power { get; set; }

                [ProtoMember(2)]
                public string Name { get; set; }

                [ProtoMember(3)]
                public _Rune SelectedRune { get; set; }

                [ProtoMember(4)]
                public List<_CastCondition> CastConditions { get; set; }
            }

            [ProtoContract]
            private struct _SkillData_int
            {
                [ProtoMember(1)]
                public _SkillPower Power { get; set; }

                [ProtoMember(2)]
                public string Name { get; set; }

                [ProtoMember(3)]
                public _Rune SelectedRune { get; set; }

                [ProtoMember(4)]
                public List<_CastCondition_int> CastConditions { get; set; }
            }

            [ProtoContract]
            private struct _CastCondition
            {
                [ProtoMember(1)]
                public int ConditionGroup { get; set; }

                [ProtoMember(2)]
                public ConditionType Type { get; set; }

                [ProtoMember(3)]
                public double[] Values { get; set; }

                [ProtoMember(4)]
                public ConditionValueName[] ValueNames { get; set; }
            }

            [ProtoContract]
            private struct _CastCondition_int
            {
                [ProtoMember(1)]
                public int ConditionGroup { get; set; }

                [ProtoMember(2)]
                public ConditionType Type { get; set; }

                [ProtoMember(3)]
                public int[] Values { get; set; }

                [ProtoMember(4)]
                public ConditionValueName[] ValueNames { get; set; }
            }

            [ProtoContract]
            private struct _Rune
            {
                [ProtoMember(1)]
                public string Name { get; set; }

                [ProtoMember(2)]
                public int RuneIndex { get; set; }

                [ProtoMember(3)]
                public DamageType _DamageType { get; set; }
            }

            private static string Dir_Definitions = AppDomain.CurrentDomain.BaseDirectory + "\\definitions";

            public static List<SkillData> Load()
            {
                try
                {
                    List<SkillData> CustomDefinitions = new List<SkillData>();

                    if (Directory.Exists(Dir_Definitions))
                    {
                        var files = Directory.GetFiles(Dir_Definitions);

                        foreach (var file in files)
                        {
                            bool failed;

                            _SkillData _skilldata = BinaryFile.ReadFromFile<_SkillData>(file, out failed);

                            if (!failed)
                            {
                                SkillData newSkillData = Convert_ToClass(_skilldata);

                                CustomDefinitions.Add(newSkillData);
                            }
                            else
                            {
                                _SkillData_int _skilldata_int = BinaryFile.ReadFromFile<_SkillData_int>(file, out failed);

                                SkillData newSkillData = Convert_ToClass(_skilldata_int);

                                CustomDefinitions.Add(newSkillData);
                            }
                        }
                    }

                    return CustomDefinitions;
                }
                catch
                {
                    return new List<SkillData>();
                }
            }

            public static void Fix()
            {
                try
                {
                    foreach (var definition in A_Collection.SkillCastConditions.Custom.CustomDefinitions)
                    {
                        foreach (var condition in definition.CastConditions)
                        {
                            var _default =
                                A_Collection.Presets.DefaultCastConditions._Default_CastConditions.FirstOrDefault(
                                    x => x.Type == condition.Type);

                            
                            if (condition.ValueNames != _default.ValueNames ||
                                condition.Values.Count() != _default.Values.Count())
                            {
                                List<double> addBuffer = new List<double>();

                                

                                for (int i = 0; i < _default.ValueNames.Count(); i++)
                                {
                                    ConditionValueName name = _default.ValueNames[i];

                                    if (!condition.ValueNames.Contains(name))
                                    {
                                        switch (name)
                                        {
                                                case ConditionValueName.Bool:
                                                addBuffer.Add(1);
                                                break;

                                                case ConditionValueName.AttribID:
                                                case ConditionValueName.Distance:
                                                case ConditionValueName.PowerSNO:
                                                case ConditionValueName.Value:
                                                addBuffer.Add(0);
                                                break;
                                        }
                                        
                                    }
                                    else
                                    {
                                        var valueIndex = Array.FindIndex(condition.ValueNames, x => x == name);

                                        addBuffer.Add(condition.Values[valueIndex]);
                                    }

                                }

                                condition.Values = addBuffer.ToArray();
                                condition.ValueNames = _default.ValueNames;
                            }
                            
                        }
                    }
                }
                catch (Exception)
                {
                    
                }
            }

            public static void Save(List<SkillData> CustomDefinitions)
            {
                if (!Directory.Exists(Dir_Definitions))
                    Directory.CreateDirectory(Dir_Definitions);

                var files = Directory.GetFiles(Dir_Definitions);
                if (files.Any())
                    foreach (var file in files)
                    {
                        var m = Regex.Match(file, @"definitions\\(.*)", RegexOptions.Singleline);
                        string filename = m.Groups[1].Value;
                        if (CustomDefinitions.FirstOrDefault(x => x.Name == filename.Replace(".bin", "")) == null)
                            File.Delete((file));
                    }

                foreach (var definition in CustomDefinitions)
                {
                    try
                    {

                        _SkillData data = Convert_ToStruct(definition);

                        BinaryFile.WriteToFile<_SkillData>(Dir_Definitions + "\\" + data.Name + ".bin", data);
                    }
                    catch
                    {
                    }
                }
            }

            private static _SkillData Convert_ToStruct(SkillData Data)
            {
                try
                {

                    _SkillData Struct = new _SkillData();

                    _SkillPower Power = new _SkillPower();
                    Power.IsPrimaryResource = Data.Power.IsPrimaryResource;
                    Power.Name = Data.Power.Name;
                    Power.PowerSNO = Data.Power.PowerSNO;
                    Power.ResourceCost = Data.Power.ResourceCost;

                    List<_Rune> Runes = new List<_Rune>();
                    foreach (var rune in Data.Power.Runes)
                    {
                        _Rune newRune = new _Rune();
                        newRune.Name = rune.Name;
                        newRune.RuneIndex = rune.RuneIndex;
                        newRune._DamageType = rune._DamageType;

                        Runes.Add(newRune);
                    }

                    Power.Runes = Runes;

                    Struct.Power = Power;

                    Struct.Name = Data.Name;

                    _Rune SelectedRune = new _Rune();
                    SelectedRune.Name = Data.SelectedRune.Name;
                    SelectedRune.RuneIndex = Data.SelectedRune.RuneIndex;
                    SelectedRune._DamageType = Data.SelectedRune._DamageType;

                    Struct.SelectedRune = SelectedRune;

                    List<_CastCondition> CastConditions = new List<_CastCondition>();
                    foreach (var condition in Data.CastConditions)
                    {
                        _CastCondition newConditions = new _CastCondition();
                        newConditions.ConditionGroup = condition.ConditionGroup;
                        newConditions.Type = condition.Type;
                        newConditions.ValueNames = condition.ValueNames;
                        newConditions.Values = condition.Values;

                        CastConditions.Add(newConditions);
                    }

                    Struct.CastConditions = CastConditions;


                    return Struct;
                }
                catch
                {
                    return default(_SkillData);
                }
            }

            private static SkillData Convert_ToClass(_SkillData_int Data)
            {
                try
                {
                    List<Rune> Runes = new List<Rune>();
                    foreach (var rune in Data.Power.Runes)
                    {
                        Rune newRune = new Rune(rune.Name, rune.RuneIndex, rune._DamageType);

                        Runes.Add(newRune);
                    }

                    SkillPower Power = new SkillPower(Data.Power.PowerSNO, Data.Power.Name, Runes,
                        Data.Power.ResourceCost, Data.Power.IsPrimaryResource, false);

                    Rune SelectedRune = new Rune(Data.SelectedRune.Name, Data.SelectedRune.RuneIndex,
                        Data.SelectedRune._DamageType);

                    List<CastCondition> CastConditions = new List<CastCondition>();

                    if (Data.CastConditions != null)
                    {
                        foreach (var condition in Data.CastConditions)
                        {
                            double[] values = Array.ConvertAll(condition.Values, x => (double) x);

                            CastCondition newCondition = new CastCondition(condition.ConditionGroup, condition.Type,
                                values, condition.ValueNames);

                            CastConditions.Add(newCondition);
                        }
                    }

                    SkillData Class = new SkillData(Power, Data.Name, SelectedRune, CastConditions);

                    return Class;
                }
                catch
                {
                    return default(SkillData);
                }
            }
        
            private static SkillData Convert_ToClass(_SkillData Data)
            {
                try
                {
                    List<Rune> Runes = new List<Rune>();
                    foreach (var rune in Data.Power.Runes)
                    {
                        Rune newRune = new Rune(rune.Name, rune.RuneIndex, rune._DamageType);

                        Runes.Add(newRune);
                    }

                    SkillPower Power = new SkillPower(Data.Power.PowerSNO, Data.Power.Name, Runes,
                        Data.Power.ResourceCost, Data.Power.IsPrimaryResource, false);

                    Rune SelectedRune = new Rune(Data.SelectedRune.Name, Data.SelectedRune.RuneIndex,
                        Data.SelectedRune._DamageType);

                    List<CastCondition> CastConditions = new List<CastCondition>();

                    if (Data.CastConditions != null)
                    {
                        foreach (var condition in Data.CastConditions)
                        {
                            CastCondition newCondition = new CastCondition(condition.ConditionGroup, condition.Type,
                                condition.Values, condition.ValueNames);

                            CastConditions.Add(newCondition);
                        }
                    }

                    SkillData Class = new SkillData(Power, Data.Name, SelectedRune, CastConditions);

                    return Class;
                }
                catch
                {
                    return default(SkillData);
                }
            }
        }



        public class BinaryFile
        {
            public static void WriteToFile<T>(string filePath, T objectToWrite, bool append = false)
            {

                using (Stream stream = File.Open(filePath, append ? FileMode.Append : FileMode.Create))
                {

                    Serializer.Serialize(stream, objectToWrite);

                }


            }


            public static T ReadFromFile<T>(string filePath, out bool failed)
            {
                failed = false;

                try
                {

                    using (Stream stream = File.Open(filePath, FileMode.Open))
                    {

                        return Serializer.Deserialize<T>(stream);
                    }
                }
                catch
                {
                    failed = true;

                    return default(T);

                }


            }
        }

        public class Manual
        {
            public class ConditionType_Tooltips
            {
                public static void Load()
                {
                    try
                    {
                        List<string> AllLines =
                            Properties.Resources.conditiontype_manual.Split(new[] {System.Environment.NewLine},
                                StringSplitOptions.RemoveEmptyEntries).ToList();

                        for (int i = 0; i < AllLines.Count(); i++)
                        {
                            var line = AllLines[i];

                            if (line.StartsWith("*"))
                            {
                                ConditionType type =
                                    (ConditionType) Enum.Parse(typeof (ConditionType), line.TrimStart('*'));

                                string tooltip = AllLines[i + 1].TrimStart('-', ' ');

                                A_Collection.Presets.Manual.Tooltips.ConditionTypes.Add(type, tooltip);
                            }

                        }
                    }
                    catch (Exception)
                    {

                    }
                }
            }
        }

        public class SkillBuilds
        {
            public static void Save()
            {
                try
                {
                    //heroid,id,setupname,(actives)power|rune:power|rune,(passives)
                    

                    if (!File.Exists(A_Collection.Me.SkillBuilds.SkillBuilds_FilePath))
                        File.Create(A_Collection.Me.SkillBuilds.SkillBuilds_FilePath).Close();

                    string output = "";

                    foreach (var build in A_Collection.Me.SkillBuilds.Builds)
                    {

                        output += build.Value + "," + build.Key.Id + "," + build.Key.Name + ",";

                        string activeOutput = "";

                        foreach (var active in build.Key.ActiveSkills)
                        {
                            activeOutput += active.PowerSno + "|" + active.Rune + ":";
                        }

                        activeOutput = activeOutput.TrimEnd(':');

                        output += activeOutput + ",";

                        string passiveOutput = "";

                        foreach (var passive in build.Key.PassiveSkills)
                        {
                            passiveOutput += passive.PowerSno + ":";
                        }

                        passiveOutput = passiveOutput.TrimEnd(':');

                        output += passiveOutput + System.Environment.NewLine;
                    }

                    File.WriteAllText(A_Collection.Me.SkillBuilds.SkillBuilds_FilePath, output);
                }
                catch (Exception)
                {
                    
                }
            }
            public static void Load()
            {
                try
                {
                    if (File.Exists(A_Collection.Me.SkillBuilds.SkillBuilds_FilePath))
                    {
                        Dictionary<SkillBuildSwap.SkillBuild, long> Buffer = new Dictionary<SkillBuildSwap.SkillBuild, long>();

                        var Lines = File.ReadAllLines(A_Collection.Me.SkillBuilds.SkillBuilds_FilePath);

                        foreach (var line in Lines)
                        {
                            var split = line.Split(',');

                            long heroid = long.Parse(split[0]);
                            int id = int.Parse(split[1]);
                            string name = split[2];

                            var AllActives = split[3];
                            var AllPassives = split[4];

                            var splitted_actives = AllActives.Split(':');
                            var splitted_passives = AllPassives.Split(':');

                            List<SkillBuildSwap.ActiveSkill> activeBuffer = new List<SkillBuildSwap.ActiveSkill>();
                            List<SkillBuildSwap.PassiveSkill> passiveBuffer = new List<SkillBuildSwap.PassiveSkill>();

                            foreach (var active in splitted_actives)
                            {
                                int powerSno = int.Parse(active.Split('|')[0]);
                                int runeId = int.Parse(active.Split('|')[1]);

                                activeBuffer.Add(new SkillBuildSwap.ActiveSkill(powerSno, runeId));
                            }

                            foreach (var passive in splitted_passives)
                            {
                                int powerSno = int.Parse(passive);
                                

                                passiveBuffer.Add(new SkillBuildSwap.PassiveSkill(powerSno));
                            }

                            Buffer.Add(new SkillBuildSwap.SkillBuild(id, name, activeBuffer, passiveBuffer), heroid);
                        }

                        A_Collection.Me.SkillBuilds.Builds = Buffer;
                    }
                }
                catch (Exception)
                {
                    
                }
            }
        }
    }
}




