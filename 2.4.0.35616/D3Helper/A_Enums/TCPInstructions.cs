using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D3Helper.A_Enums
{
    public enum TCPInstructions
    {
        ValidateBTag = 1,
        ValidateClanTag = 2,
        WriteLogEntry = 3,
        GetLatestVersion = 4,
        WriteBugReport = 5,
        WriteSuggestionsReport = 6,
        GetTotalUniqueUsers = 7,
        GetActiveUsers = 8,
        GetChangelog = 9,
        GetBugReports = 10,
        GetSuggestions = 11,
        GetPowers = 12,
        StorePower = 13,
    }
}
