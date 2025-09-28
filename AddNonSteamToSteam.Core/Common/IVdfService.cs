using AddNonSteamToSteam.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddNonSteamToSteam.Core.Common
{
    public interface IVdfService
    {
        IReadOnlyList<ShortcutModel> ReadShortcuts(string shortcutsVdfPath);
        Models.OperationResult WriteShortcuts(string shortcutsVdfPath, IEnumerable<ShortcutModel> shortcuts, bool backup = true);
    }
}
