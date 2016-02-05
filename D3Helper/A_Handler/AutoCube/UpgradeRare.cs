using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Enigma.D3;
using Enigma.D3.UI;

namespace D3Helper.A_Handler.AutoCube
{
    class UpgradeRare
    {
        public static bool IsUpgrading_Rare = false;

        private const string BTN_Transmute =
            "Root.NormalLayer.vendor_dialog_mainPage.transmute_dialog.LayoutRoot.transmute_button";

        public static void DoUpgrade()
        {
            try
            {
                IsUpgrading_Rare = true;

                ActorCommonData CubeStand;

                bool CubeNearby = Tools.IsCubeNearby(out CubeStand);

                if (CubeNearby)
                {
                    
                    while (true)
                    {
                        float RX_Cube, RY_Cube;

                        A_Tools.T_World.ToScreenCoordinate(CubeStand.x0D0_WorldPosX, CubeStand.x0D4_WorldPosY, CubeStand.x0D8_WorldPosZ, out RX_Cube, out RY_Cube);


                        if (Tools.IsVendorPage_Visible() && Tools.IsKanaisCube_MainPage_Visible())
                            break;

                        A_Tools.InputSimulator.IS_Mouse.MoveCursor((uint)RX_Cube, (uint)RY_Cube);
                        A_Tools.InputSimulator.IS_Mouse.LeftClick();

                        Thread.Sleep(100);

                        
                    }

                    Stopwatch s1 = new Stopwatch(); /////////
                    s1.Start(); ///////////

                    var UpgradableItems = Tools.Get_RareUpgradableItems();
                    List<ActorCommonData> Materials;
                    var Count_AvailableEnchants = Tools.Get_AvailableEnchants_UpgradeRare(out Materials);

                    var Count_Enchants = 0;

                    foreach (var item in UpgradableItems)
                    {
                        if (Count_Enchants == Count_AvailableEnchants)
                            break;

                        while (true)
                        {
                            float RX_Cube, RY_Cube;

                            A_Tools.T_World.ToScreenCoordinate(CubeStand.x0D0_WorldPosX, CubeStand.x0D4_WorldPosY, CubeStand.x0D8_WorldPosZ, out RX_Cube, out RY_Cube);


                            if (Tools.IsVendorPage_Visible() && Tools.IsKanaisCube_MainPage_Visible())
                                break;

                            A_Tools.InputSimulator.IS_Mouse.MoveCursor((uint)RX_Cube, (uint)RY_Cube);
                            A_Tools.InputSimulator.IS_Mouse.LeftClick();

                            Thread.Sleep(100);


                        }

                        UIRect UIRect_item =
                            A_Collection.D3UI.InventoryItemUIRectMesh.FirstOrDefault(
                                x => x.Key.ItemSlotX == item.x118_ItemSlotX && x.Key.ItemSlotY == item.x11C_ItemSlotY)
                                .Value;

                        A_Tools.InputSimulator.IS_Mouse.RightCLick((int)UIRect_item.Left, (int)UIRect_item.Top, (int)UIRect_item.Right, (int)UIRect_item.Bottom);
                        Thread.Sleep(100);

                        foreach (var material in Materials)
                        {
                            UIRect UIRect_material =
                            A_Collection.D3UI.InventoryItemUIRectMesh.FirstOrDefault(
                                x => x.Key.ItemSlotX == material.x118_ItemSlotX && x.Key.ItemSlotY == material.x11C_ItemSlotY)
                                .Value;

                            A_Tools.InputSimulator.IS_Mouse.RightCLick((int)UIRect_material.Left, (int)UIRect_material.Top, (int)UIRect_material.Right, (int)UIRect_material.Bottom);
                            Thread.Sleep(100);
                        }

                        UIRect Transmute = A_Tools.T_D3UI.UIElement.getRect(BTN_Transmute);

                        A_Tools.InputSimulator.IS_Mouse.LeftClick((int)Transmute.Left, (int)Transmute.Top, (int)Transmute.Right, (int)Transmute.Bottom);
                        Thread.Sleep(100);

                        while (Tools.IsVendorPage_Visible())
                            A_Tools.InputSimulator.IS_Keyboard.Close_AllWindows();

                        Count_Enchants++;
                    }

                    s1.Stop(); /////////
                    TimeSpan t1 = s1.Elapsed; //////
                    Console.WriteLine(t1.TotalSeconds); ////////

                }

                IsUpgrading_Rare = false;
            }
            catch (Exception)
            {
                IsUpgrading_Rare = false;
            }
        }
    }
}
