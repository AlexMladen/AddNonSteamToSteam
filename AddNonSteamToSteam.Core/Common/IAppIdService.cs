using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddNonSteamToSteam.Core.Common
{
    public interface IAppIdService
    {
        /// <summary>Compute Non-Steam shortcut AppID from AppName + Exe (quoted), CRC32 | 0x80000000.</summary>
        uint ComputeNonSteamAppId(string appName, string exeQuoted);
    }
}
