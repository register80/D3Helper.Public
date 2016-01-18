using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

using Enigma.D3;
using Enigma.D3.UI;
using Enigma.D3.Helpers;
using Enigma.D3.UI.Controls;

using D3Helper.A_Enigma_Extenstions;

namespace D3Helper.A_Tools.Authentification
{
    class Validation
    {
		public static bool IsValidated()
        {
			try
            {
                // Get BTag
                // Get ClanTag
                // Check BTag	White&BlackList
                // Check ClanTag	White&BlackList

                while (A_Collection.Me.Account.BTag == null)
                    System.Threading.Thread.Sleep(10);
               

                string BTag = A_Collection.Me.Account.BTag;
                string ClanTag = get_ClanTag();

                //if (BTag.Length < 1)
                //    return false;
                                
                //bool ValidatedBTag = Convert.ToBoolean(A_TCPClient.TCPClient.send_Instruction(A_Enums.TCPInstructions.ValidateBTag, BTag));
                //bool ValidatedClanTag = Convert.ToBoolean(A_TCPClient.TCPClient.send_Instruction(A_Enums.TCPInstructions.ValidateClanTag, ClanTag));

                //if (ValidatedBTag || ValidatedClanTag)
                //{
                    if(Convert.ToBoolean(A_TCPClient.TCPClient.send_Instruction(A_Enums.TCPInstructions.WriteLogEntry, BTag)))
						return true;
                //}

                //if (Convert.ToBoolean(A_TCPClient.TCPClient.send_Instruction(A_Enums.TCPInstructions.WriteLogEntry, "INVALID ACCESS " + BTag)))
                //{
                //    A_SMTPClient.SMTPClient.sendMail(A_Web.Web.GetPublicIP() + "\t" +  get_RealName() + "\t" + get_Country() + "\t" + BTag + "\t" + ClanTag);
                //    return false;
                //}

                return false;
            }
            catch { return false; }
        }
        private static string get_ClanTag()
        {
            try
            {
                string ClanTag = "";

                if (!A_Collection.Me.HeroStates.isInGame)
                    ClanTag = UXHelper.GetControl<UXIcon>(A_Enums.UIElements.menu_portrait_1_text).xA20_Text_StructStart_Min84Bytes;

                else
                    ClanTag = UXHelper.GetControl<UXIcon>(A_Enums.UIElements.portrait_0_text).xA20_Text_StructStart_Min84Bytes;

                ClanTag = Regex.Match(ClanTag, @"\<(.*)>").Groups[0].Value;

                return ClanTag;
            }
            catch { return ""; }
        }
        private static string get_RealName()
        {
            try
            {
                string name = Engine.Current.Memory.Reader.ReadChain<RefString>(0x01BBB55C, 0x10, 0x9C, 0x18).x04_PtrText;
                return name;
            }
            catch { return ""; }
        }
        private static string get_Country()
        {
            try
            {
                string country = Engine.Current.Memory.Reader.ReadChain<RefString>(0x01BBB55C, 0x10, 0x9C, 0x50).x04_PtrText;
                return country;
            }
            catch { return ""; }
        }
    }
}
