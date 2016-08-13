using Enigma.D3.UI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D3Helper
{
	internal static class EnigmaAdapter
	{
		public static int GetPowerSnoId(this UXIcon icon)
		{
			return icon.Read<int>(0x166C);
		}
	}
}
