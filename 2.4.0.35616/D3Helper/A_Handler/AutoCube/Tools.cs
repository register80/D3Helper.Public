using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using D3Helper.A_Collector;
using Enigma.D3;
using Enigma.D3.DataTypes;
using Enigma.D3.Enums;
using Enigma.D3.Helpers;
using Enigma.D3.UI.Controls;

namespace D3Helper.A_Handler.AutoCube
{
    class Tools
    {
        private const int KanaiCube_Stand = 439975;

        public static bool IsCubeNearby(out ActorCommonData CubeStand)
        {
            CubeStand = new ActorCommonData();

            try
            {
                List<ACD> AllActors;
                lock (A_Collection.Environment.Actors.AllActors) AllActors = A_Collection.Environment.Actors.AllActors;

                var acd = AllActors.FirstOrDefault(x => x._ACD.x090_ActorSnoId == KanaiCube_Stand)._ACD;

                if (acd != null)
                {
                    CubeStand = acd;

                    return true;
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool IsVendorPage_Visible()
        {
            try
            {
                string vendor_mainpage = "Root.NormalLayer.vendor_dialog_mainPage.text_category";

                return A_Tools.T_D3UI.UIElement.isVisible(vendor_mainpage);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool IsKanaisCube_MainPage_Visible()
        {
            try
            {
                string Text = "KANAI'S CUBE";
                string vendor_mainpage = "Root.NormalLayer.vendor_dialog_mainPage.text_category";

                return UXHelper.GetControl<UXLabel>(vendor_mainpage).xA20_Text_StructStart_Min84Bytes == Text;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static List<ActorCommonData> Get_RareUpgradableItems()
        {
            List<ActorCommonData> Items = new List<ActorCommonData>();

            try
            {
                var inventory = ActorCommonDataHelper.EnumerateInventoryItems();

                foreach (var item in inventory)
                {
                    var quality = item.GetAttributeValue(AttributeId.ItemQualityLevel);

                    if (quality >= 6 || quality <= 8) //Rare
                        Items.Add(item);
                }

                return Items;
            }
            catch (Exception)
            {
                return Items;
            }
        }

        private static int[] Costs_UpgradeRare = new int[] {25,50,50,50}; // Deaths Breath | Reusable Parts | Arcane Dust | Veiled Crystal

        public static double Get_AvailableEnchants_UpgradeRare(out List<ActorCommonData> Materials)
        {
            Materials = new List<ActorCommonData>();
            try
            {
                var inventory = ActorCommonDataHelper.EnumerateInventoryItems().ToList();

                ActorCommonData acd;

                int Count_DB = GetMaterial_DeathBreath(inventory, out acd);
                Materials.Add(acd);

                int Count_RP = GetMaterial_ReusableParts(inventory, out acd);
                Materials.Add(acd);

                int Count_AD = GetMaterial_ArcaneDust(inventory, out acd);
                Materials.Add(acd);

                int Count_VC = GetMaterial_VeiledCrystal(inventory, out acd);
                Materials.Add(acd);

                double Enchants_DB = Count_DB/Costs_UpgradeRare[0];
                double Enchants_RP = Count_RP/Costs_UpgradeRare[1];
                double Enchants_AD = Count_AD/Costs_UpgradeRare[2];
                double Enchants_VC = Count_VC/Costs_UpgradeRare[3];

                double[] x = new[] {Enchants_DB, Enchants_RP, Enchants_AD, Enchants_VC};

                double possibleEnchants = x.OrderBy(y => y).First();

                return possibleEnchants;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        private static int GetMaterial_DeathBreath(List<ActorCommonData> Inventory, out ActorCommonData acd)
        {
            acd = new ActorCommonData();
            try
            {
                int ActorSno = 361989;

                var material = Inventory.FirstOrDefault(x => x.x090_ActorSnoId == ActorSno);

                if (material == null)
                    return 0;

                acd = material;

                return (int)material.GetAttributeValue(AttributeId.ItemStackQuantityLo);
            }
            catch (Exception)
            {
                return 0;
            }
        }
        private static int GetMaterial_ReusableParts(List<ActorCommonData> Inventory, out ActorCommonData acd)
        {
            acd = new ActorCommonData();

            try
            {
                int ActorSno = 361984;

                var material = Inventory.FirstOrDefault(x => x.x090_ActorSnoId == ActorSno);

                if (material == null)
                    return 0;

                acd = material;

                return (int)material.GetAttributeValue(AttributeId.ItemStackQuantityLo);
            }
            catch (Exception)
            {
                return 0;
            }
        }
        private static int GetMaterial_ArcaneDust(List<ActorCommonData> Inventory, out ActorCommonData acd)
        {
            acd = new ActorCommonData();

            try
            {
                int ActorSno = 361985;

                var material = Inventory.FirstOrDefault(x => x.x090_ActorSnoId == ActorSno);

                if (material == null)
                    return 0;

                acd = material;

                return (int)material.GetAttributeValue(AttributeId.ItemStackQuantityLo);
            }
            catch (Exception)
            {
                return 0;
            }
        }
        private static int GetMaterial_VeiledCrystal(List<ActorCommonData> Inventory, out ActorCommonData acd)
        {
            acd = new ActorCommonData();

            try
            {
                int ActorSno = 361986;

                var material = Inventory.FirstOrDefault(x => x.x090_ActorSnoId == ActorSno);

                if (material == null)
                    return 0;

                acd = material;

                return (int)material.GetAttributeValue(AttributeId.ItemStackQuantityLo);
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}
