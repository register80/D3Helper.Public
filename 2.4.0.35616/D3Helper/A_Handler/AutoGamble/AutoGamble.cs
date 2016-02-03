using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

using Enigma.D3;
using Enigma.D3.UI;
using Enigma.D3.UI.Controls;
using Enigma.D3.Helpers;

namespace D3Helper.A_Handler.AutoGamble
{
    class AutoGamble
    {
        public static bool isGambling = false;


        public static void start_Gamble()
        {
            try
            {
                if (A_Tools.T_D3UI.UIElement.isVisible(A_Enums.UIElements.ShopDialogMainPage))
                {
                    if (isGambleVendor())
                    {
                        if (tryGetMouseCaptureUIReference().x008_Name.ToLower().Contains("shop_item_region"))
                        {
                            tryAutoGamble();
                        }
                    }
                }
            }
            catch { }

        }
        private static void tryAutoGamble()
        {
            try
            {
                isGambling = true;

                UIReference lastClickedUIReference = tryGetLastClickedUIReference();
                int gambleCosts = tryGetBloodShardCosts(lastClickedUIReference);
                UIRect shopItemRect = A_Tools.T_D3UI.UIElement.getRect(lastClickedUIReference.x008_Name);

                
                    while (
                        tryGetLastClickedUIReference().x008_Name.ToLower().Contains("shop_item_region") &&
                        A_Tools.T_D3UI.UIElement.isVisible(A_Enums.UIElements.ShopDialogMainPage) &&
                        get_BloodShardAmount() >= gambleCosts &&
                        !tryCheckInventoryFull() &&
                        tryCheckEnoughShards() &&
                        A_Collection.D3Client.Window.isForeground
                        )
                    {
                        A_Tools.InputSimulator.IS_Mouse.RightCLick(((int)shopItemRect.Left + ((int)shopItemRect.Width * 10 / 100)), ((int)shopItemRect.Top + ((int)shopItemRect.Height * 10 / 100)), ((int)shopItemRect.Right - ((int)shopItemRect.Width * 10 / 100)), ((int)shopItemRect.Bottom - ((int)shopItemRect.Height * 10 / 100)));

                        System.Threading.Thread.Sleep(100);
                    }
                

                isGambling = false;

            }
            catch { isGambling = false; }
        }
        private static bool tryCheckInventoryFull()
        {
            try
            {
                var inventoryItems = ActorCommonDataHelper.EnumerateInventoryItems();

                if (inventoryItems.Count() <= 20)
                    return false;

                string error_notification_uielement = "Root.TopLayer.error_notify.error_text";
                //string inventory_full_text = "Not enough Inventory space to complete this operation.";
                string item_canot_be_picked = "That item cannot be picked up.";

                var errortext = UXHelper.GetControl<UXLabel>(error_notification_uielement);

                if (errortext.xA20_Text_StructStart_Min84Bytes == item_canot_be_picked && errortext.IsVisible())
                {
                    return true;
                }

                return false;

            }
            catch { return true; }
        }
        private static bool isGambleVendor()
        {
            try
            {
                string Currency_String = UXHelper.GetControl<UXLabel>("Root.NormalLayer.shop_dialog_mainPage.gold_label").xA20_Text_StructStart_Min84Bytes;

                if (Currency_String == "Your Available Gold:")
                {
                    return false;
                }

                return true;
            }
            catch { return false; }
        }
        private static bool tryCheckEnoughShards()
        {
            try
            {
                string error_notification_uielement = "Root.TopLayer.error_notify.error_text";
                string not_enough_shards = "Not enough Blood Shards.";


                var errortext = UXHelper.GetControl<UXLabel>(error_notification_uielement);

                if (errortext.xA20_Text_StructStart_Min84Bytes == not_enough_shards && errortext.IsVisible())
                {
                    return false;
                }

                return true;
            }
            catch { return false; }
        }
        private static UIReference tryGetLastClickedUIReference()
        {
            try
            {
                // return ObjectManager.Instance.x9A4_UI.x0828_LastClicked;
                return ObjectManager.Instance.x9CC_Ptr_10000Bytes_UI.Dereference().x0828_LastClicked;
            }
            catch { return null; }
        }
        private static UIReference tryGetMouseCaptureUIReference()
        {
            try
            {
                // return ObjectManager.Instance.x9A4_UI.x0008_MouseCapture;
                return ObjectManager.Instance.x9CC_Ptr_10000Bytes_UI.Dereference().x0008_MouseCapture;
            }
            catch { return null; }
        }
        private static int tryGetBloodShardCosts(UIReference lastClickedElement)
        {
            try
            {
                int bloodShardCosts = 10000;




                var text = UXHelper.GetControl<UXLabel>(lastClickedElement.x008_Name + ".text_cost").xA20_Text_StructStart_Min84Bytes;

                var shardstring = Regex.Match(text, @"\d+").Groups[0].Value;

                if (int.TryParse(shardstring, out bloodShardCosts))
                {
                    return bloodShardCosts;
                }


                return bloodShardCosts;
            }
            catch { return 10000; }
        }
        private static long get_BloodShardAmount()
        {
            try
            {
                //uint bloodshardGBId = 2603730171;
                lock(A_Collection.Me.HeroGlobals.LocalPlayerData)
                {
                    return A_Collection.Me.HeroGlobals.LocalPlayerData.GetCurrency(CurrencyType.X1Shard);
                }


            }
            catch { return 0; }
        }
    }
}
