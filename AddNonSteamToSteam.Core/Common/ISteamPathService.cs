using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddNonSteamToSteam.Core.Common
{
    public interface ISteamPathService
    {
        string GetSteamRoot();
        string GetActiveUserConfigDir(string steamRoot);
        string GetShortcutsPath(string userConfigDir);
        string GetGridDir(string userConfigDir);
    }
}
