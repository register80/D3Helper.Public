using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsInput;

namespace D3Helper.A_Tools
{
    class T_Chat
    {
        public static void write_PartyChatmessage(string Message)
        {
            WindowsInput.InputSimulator.SimulateKeyPress(VirtualKeyCode.RETURN);
            WindowsInput.InputSimulator.SimulateTextEntry("/p " + Message);
            WindowsInput.InputSimulator.SimulateKeyPress(VirtualKeyCode.RETURN);
        }
    }
}
