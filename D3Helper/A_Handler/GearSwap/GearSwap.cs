using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Enigma.D3;
using Enigma.D3.UI;
using Enigma.D3.UI.Controls;
using Enigma.D3.Helpers;
using D3Helper.A_Collection;
using System.Drawing;
using System.Windows.Forms;
using WindowsInput;

using D3Helper.A_Enums;
using D3Helper.A_Tools;
using Enigma.D3.Enums;

namespace D3Helper.A_Handler.GearSwap
{
    class GearSwap
    {
        public static bool isSwaping = false;
        public static void tryGearSwap()
        {
            try
            {
                isSwaping = true;

                bool CollectionChanged = false;

                // Save Current Cursor Pos
                Point getCursorPos = new Point((int)Cursor.Position.X, (int)Cursor.Position.Y);

                // Get Current Pressed SwapHotkey Id
                int Current_SwapId = A_Collection.Me.GearSwap.Selected_SwapId;
                long HeroId = A_Collection.Me.HeroGlobals.HeroID;

                // Get Collection of Items to Swap
                lock(A_Collection.Me.GearSwap.GearSwaps)
                {
                    var GearSwap = A_Collection.Me.GearSwap.GearSwaps.ToList().Where(x => x.HeroID == HeroId && x.SwapId == Current_SwapId).ToList();


                    if (GearSwap.Count() > 0)
                    {
                        // Check If Inventory is opened otherwise Open it
                        if (InventoryOpened())
                        {
                            int _Ring_Counter = 0;
                            int _1HWeapon_Counter = 0;

                            for (int i = 0; i < GearSwap.Count(); i++)
                            {
                                // Get ItemType
                                A_Enums.ItemTypes ItemType = GetItemTypeByItemSeed(GearSwap[i].ItemSeed);

                                if (ItemType == A_Enums.ItemTypes.Ring) { _Ring_Counter = _Ring_Counter + 1; }
                                if (ItemType == A_Enums.ItemTypes._1HWeapon) { _1HWeapon_Counter = _1HWeapon_Counter + 1; }

                                // Get UIRect of current Item to Swap
                                UIRect ItemUIRect = A_Collection.D3UI.InventoryItemUIRectMesh.FirstOrDefault(x => x.Key.ItemSlotX == GearSwap[i].ItemSlotX && x.Key.ItemSlotY == GearSwap[i].ItemSlotY).Value;

                                // Buffer Current Equipped Items before Swaping Item
                                var Inventory_Buffer = ActorCommonDataHelper.EnumerateInventoryItems();
                                List<double> Pre_Inventory_Seeds = new List<double>();
                                foreach (var Item in Inventory_Buffer)
                                {
                                    double Seed = Item.GetAttributeValue(Enigma.D3.Enums.AttributeId.Seed);
                                    Pre_Inventory_Seeds.Add(Seed);
                                }

                                // Right Click on Random Point in Items UIRect
                                bool IsAltSwap = false;

                                if (ItemType == A_Enums.ItemTypes._1HWeapon && _1HWeapon_Counter >= 2)
                                {
                                    IsAltSwap = true;

                                    InputSimulator.SimulateKeyDown(VirtualKeyCode.LMENU);
                                }
                                if (ItemType == A_Enums.ItemTypes.Ring && _Ring_Counter >= 2)
                                {
                                    IsAltSwap = true;

                                    InputSimulator.SimulateKeyDown(VirtualKeyCode.LMENU);
                                }

                                while (!IsItemSeedEquipped(GearSwap[i].ItemSeed) && A_Tools.T_D3UI.UIElement.isVisible(A_Enums.UIElements.Inventory))
                                {
                                    A_Tools.InputSimulator.IS_Mouse.RightCLick((int)ItemUIRect.Left, (int)ItemUIRect.Top, (int)ItemUIRect.Right, (int)ItemUIRect.Bottom);
                                    Thread.Sleep(250);
                                }

                                if (IsAltSwap)
                                {
                                    InputSimulator.SimulateKeyUp(VirtualKeyCode.LMENU);
                                }

                                // Equipped Items after Swaping
                                var Inventory = ActorCommonDataHelper.EnumerateInventoryItems();
                                List<double> Cur_Inventory_Seeds = new List<double>();
                                foreach (var Item in Inventory)
                                {
                                    double Seed = Item.GetAttributeValue(Enigma.D3.Enums.AttributeId.Seed);
                                    Cur_Inventory_Seeds.Add(Seed);
                                }

                                // Diff Equipped and Equipped_Buffer and add Swapped Item(s) to GearSwap
                                foreach (var seed in Pre_Inventory_Seeds)
                                {
                                    if (!Cur_Inventory_Seeds.Contains(seed))
                                    {
                                        //Console.WriteLine("Inventory Items changed! Item removed!");

                                        var TryGetEntry = A_Collection.Me.GearSwap.GearSwaps.FirstOrDefault(x => x.HeroID == HeroId && x.SwapId == Current_SwapId && x.ItemSeed == seed);

                                        if (TryGetEntry != null)
                                        {
                                            A_Collection.Me.GearSwap.GearSwaps.Remove(TryGetEntry);

                                            CollectionChanged = true;
                                        }
                                    }

                                }
                                foreach (var seed in Cur_Inventory_Seeds)
                                {
                                    if (!Pre_Inventory_Seeds.Contains(seed))
                                    {
                                        //Console.WriteLine("Inventory Items changed! Item added!");

                                        var TryGetEntry = A_Collection.Me.GearSwap.GearSwaps.FirstOrDefault(x => x.HeroID == HeroId && x.SwapId == Current_SwapId && x.ItemSeed == seed);

                                        if (TryGetEntry == null)
                                        {
                                            var item =
                                                Inventory.FirstOrDefault(
                                                    x => x.GetAttributeValue(AttributeId.Seed) == seed);

                                            A_Collection.Me.GearSwap.GearSwaps.Add(new GearSwapItem(HeroId, Current_SwapId, seed, item.x118_ItemSlotX, item.x11C_ItemSlotY, GearSwap[i].ItemSize));

                                            CollectionChanged = true;
                                        }
                                    }

                                }

                                if (CollectionChanged)
                                {
                                    A_Tools.T_ExternalFile.GearSwaps.Save();
                                }
                            }
                        }
                    }


                    // Close Inventory
                    CloseInventory();

                    // Restore Cursor Pos to previous Pos
                    Cursor.Position = getCursorPos;




                    isSwaping = false;
                }
            }
            catch { isSwaping = false; }
        }
        public static void SelectionMode()
        {

            try
            {
                bool CollectionChanged = false;

                if (A_Collection.Me.GearSwap.editModeEnabled)
                {
                    A_Collection.Me.GearSwap.isEditing = true;

                    if (!A_Collection.D3UI.isOpenInventory)
                    {
                        A_Collection.Me.GearSwap.editModeEnabled = false;

                    }

                    var SelectedAcdId = GetSelectedAcdId();

                    if (SelectedAcdId > -1) // Add/Remove Selected Items to/from Collection
                    {


                        int AcdId = SelectedAcdId;
                        ActorCommonData ItemAcd = GetSelectedItemAcd(AcdId);
                        double ItemSeed = GetItemSeed(ItemAcd);
                        ItemSlotSize ItemSize = GetSlotSize(ItemAcd);
                        long HeroId = A_Collection.Me.HeroGlobals.HeroID;

                        if (A_Collection.Me.GearSwap.GearSwaps.FirstOrDefault(x => x.ItemSeed == ItemSeed) == null)
                        {
                            A_Collection.Me.GearSwap.GearSwaps.Add(new GearSwapItem(HeroId, A_Collection.Me.GearSwap.Selected_SwapId, ItemSeed, ItemAcd.x118_ItemSlotX, ItemAcd.x11C_ItemSlotY, ItemSize));

                            CollectionChanged = true;
                        }
                        else
                        {
                            var tryGetEntry = A_Collection.Me.GearSwap.GearSwaps.FirstOrDefault(x => x.ItemSeed == ItemSeed);

                            A_Collection.Me.GearSwap.GearSwaps.Remove(tryGetEntry);

                            CollectionChanged = true;
                        }

                        while (GetSelectedAcdId() == AcdId)
                        {
                            Thread.Sleep(50);
                        }
                    }




                    //Thread.Sleep(50);

                }

                if (CollectionChanged)
                {
                    A_Tools.T_ExternalFile.GearSwaps.Save();
                }

                A_Collection.Me.GearSwap.isEditing = false;
            }
            catch { A_Collection.Me.GearSwap.isEditing = false; }
        }
        public static List<double> GetInventoryChanges()
        {
            try
            {
                List<double> Changes_Seeds = new List<double>();

                bool InventoryChanged = false;

                List<ActorCommonData> Inventory_Buffer = ActorCommonDataHelper.EnumerateInventoryItems().ToList();

                while (true)
                {
                    var Inventory = ActorCommonDataHelper.EnumerateInventoryItems().ToList();

                    foreach (var item in Inventory)
                    {
                        var tryGetEntry = Inventory_Buffer.FirstOrDefault(x => x.x000_Id == item.x000_Id);

                        if (tryGetEntry == null)
                        {
                            InventoryChanged = true;

                            Changes_Seeds.Add(item.GetAttributeValue(Enigma.D3.Enums.AttributeId.Seed));
                        }
                    }

                    if (InventoryChanged)
                    {
                        break;
                    }

                    Inventory_Buffer = Inventory;

                    Thread.Sleep(50);
                }

                return Changes_Seeds;
            }
            catch { return new List<double>(); }
        }
        public static void UpdateSwapItems()
        {
            try
            {
                if (A_Collection.Me.HeroStates.isInGame && A_Collection.D3UI.isOpenInventory)
                {
                    List<GearSwapItem> GearSwaps;
                    lock (A_Collection.Me.GearSwap.GearSwaps) GearSwaps = A_Collection.Me.GearSwap.GearSwaps.ToList();

                    bool CollectionChanged = false;

                    var HeroId = A_Collection.Me.HeroGlobals.HeroID;
                    var CurrentSwaps = GearSwaps.Where(x => x.HeroID == HeroId).ToList();

                    List<GearSwapItem> Buffer = new List<GearSwapItem>();

                    if (CurrentSwaps.Count() > 0)
                    {
                        var InventoryItems = ActorCommonDataHelper.EnumerateInventoryItems();


                        foreach (var Item in CurrentSwaps)
                        {
                            var tryGetEntry =
                                InventoryItems.FirstOrDefault(
                                    x => x.GetAttributeValue(Enigma.D3.Enums.AttributeId.Seed) == Item.ItemSeed);

                            if (tryGetEntry != null)
                            {

                                Buffer.Add(new GearSwapItem(HeroId, Item.SwapId, Item.ItemSeed,
                                    tryGetEntry.x118_ItemSlotX, tryGetEntry.x11C_ItemSlotY, Item.ItemSize));

                                if (Item.ItemSlotX != tryGetEntry.x118_ItemSlotX ||
                                    Item.ItemSlotY != tryGetEntry.x11C_ItemSlotY)
                                {

                                    CollectionChanged = true;
                                }
                            }
                            else
                            {
                                CollectionChanged = true;
                            }
                        }

                        if (CollectionChanged)
                        {
                            A_Collection.Me.GearSwap.GearSwaps = Buffer;

                            A_Tools.T_ExternalFile.GearSwaps.Save();
                        }
                    }

                }
            }
            catch { }
        }

        private static ActorCommonData GetItemByHeroLocation(Enigma.D3.Enums.ItemLocation ItemLocation)
        {
            try
            {
                lock(A_Collection.Me.HeroDetails.EquippedItems)
                {
                    var Container = A_Collection.Me.HeroDetails.EquippedItems.ToList();

                    return Container.FirstOrDefault(x => x.x114_ItemLocation == ItemLocation);
                }
            }
            catch { return null; }
        }
        private static Enigma.D3.Enums.ItemLocation GetHeroLocationByItemAcd(ActorCommonData ItemAcd)
        {
            lock (A_Collection.Me.HeroDetails.EquippedItems)
            {
                var Container = A_Collection.Me.HeroDetails.EquippedItems.ToList();

                return Container.FirstOrDefault(x => x.x000_Id == ItemAcd.x000_Id).x114_ItemLocation;
            }
        }
        private static double GetItemSeed(ActorCommonData ItemAcd)
        {
            return ItemAcd.GetAttributeValue(Enigma.D3.Enums.AttributeId.Seed);
        }
        private static int GetSelectedAcdId()
        {
            try
            {
                var SelectedAcdId = ObjectManager.Instance.x9E0_PlayerInput.Dereference().x44_Neg1_SelectedItemAcdId;

                return SelectedAcdId;
            }
            catch { return -1; }
        }
        private static ActorCommonData GetSelectedItemAcd(int AcdId)
        {
            var InventoryItemContainer = ActorCommonDataHelper.EnumerateInventoryItems();

            return InventoryItemContainer.FirstOrDefault(x => x.x000_Id == AcdId);
        }
        public static ItemSlotSize GetSlotSize(ActorCommonData ItemAcd)
        {
            string ItemName = ItemAcd.x004_Name.ToLower();

            if (ItemName.Contains("ring") ||
                ItemName.Contains("amulet") ||
                ItemName.Contains("belt")
                )
            {
                return ItemSlotSize._1x1;
            }

            return ItemSlotSize._2x1;
        }
        private static A_Enums.ItemTypes GetItemTypeByItemSeed(double ItemSeed)
        {
            try
            {
                var InventoryItems = ActorCommonDataHelper.EnumerateInventoryItems();

                string ItemName = InventoryItems.FirstOrDefault(x => x.GetAttributeValue(Enigma.D3.Enums.AttributeId.Seed) == ItemSeed).x004_Name.ToLower();

                if (ItemName.Contains("x1_"))
                {
                    ItemName = ItemName.TrimStart('x', '1', '_');
                }
                if (ItemName.Contains("p2_"))
                {
                    ItemName = ItemName.TrimStart('p', '2', '_');
                }

                // 1HWeapons

                if (ItemName.StartsWith("sword"))
                {
                    return A_Enums.ItemTypes._1HWeapon;
                }
                if (ItemName.StartsWith("axe"))
                {
                    return A_Enums.ItemTypes._1HWeapon;
                }
                if (ItemName.StartsWith("dagger"))
                {
                    return A_Enums.ItemTypes._1HWeapon;
                }
                if (ItemName.StartsWith("wand"))
                {
                    return A_Enums.ItemTypes._1HWeapon;
                }
                if (ItemName.StartsWith("ceremonial"))
                {
                    return A_Enums.ItemTypes._1HWeapon;
                }
                if (ItemName.StartsWith("fistweapon"))
                {
                    return A_Enums.ItemTypes._1HWeapon;
                }
                if (ItemName.StartsWith("handxbow"))
                {
                    return A_Enums.ItemTypes._1HWeapon;
                }
                if (ItemName.StartsWith("mace"))
                {
                    return A_Enums.ItemTypes._1HWeapon;
                }
                if (ItemName.StartsWith("mightyweapon"))
                {
                    return A_Enums.ItemTypes._1HWeapon;
                }

                // Ring

                if (ItemName.StartsWith("ring"))
                {
                    return A_Enums.ItemTypes.Ring;
                }

                // Amulet

                if (ItemName.StartsWith("amulet"))
                {
                    return A_Enums.ItemTypes.Amulet;
                }

                return A_Enums.ItemTypes.Armor;
            }
            catch { return A_Enums.ItemTypes.Other; }
        }
        private static bool IsItemSeedEquipped(double ItemSeed)
        {
            try
            {
                
                    var container = ActorCommonDataHelper.EnumerateEquippedItems();
                    var tryGetEntry = container.FirstOrDefault(x => x.GetAttributeValue(Enigma.D3.Enums.AttributeId.Seed) == ItemSeed);

                    if (tryGetEntry != null)
                    {
                        return true;
                    }
                    return false;
                
            }
            catch { return false; }
        }
        private static bool InventoryOpened()
        {
            if (A_Collection.D3UI.isOpenInventory)
            {
                return true;
            }
            else
            {
                
                while (true)
                {
                    A_Tools.InputSimulator.IS_Keyboard.Inventory();

                    Thread.Sleep(100);

                    bool visible = A_Tools.T_D3UI.UIElement.isVisible(A_Enums.UIElements.Inventory);

                    if (visible)
                        return true;

                    
                    
                }
                
            }
            return false;
        }
        private static void CloseInventory()
        {
            if (A_Tools.T_D3UI.UIElement.isVisible(A_Enums.UIElements.Inventory))
            {

                while (true)
                {
                    A_Tools.InputSimulator.IS_Keyboard.Inventory();

                    Thread.Sleep(100);

                    bool visible = A_Tools.T_D3UI.UIElement.isVisible(A_Enums.UIElements.Inventory);

                    if (!visible)
                        break;



                }
            }
        }
    }
}
