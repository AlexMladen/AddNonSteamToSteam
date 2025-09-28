using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddNonSteamToSteam.Core.Models
{
    public class ShortcutRequest
    {
        public string DisplayName { get; set; } = "";
        public LaunchMode Mode { get; set; }
        public string? Uri { get; set; }
        public string? ExePath { get; set; }
        public bool AllowOverlay { get; set; } = true;
    }
}
