using AddNonSteamToSteam.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddNonSteamToSteam.Core.Common
{
    public interface IShortcutService
    {
        /// <summary>Adds or updates a Non-Steam shortcut in shortcuts.vdf. Returns resulting model with NonSteamAppId.</summary>
        OperationResult AddOrUpdateShortcut(ShortcutRequest request, string shortcutsVdfPath, out ShortcutModel resultingShortcut);
    }
}
