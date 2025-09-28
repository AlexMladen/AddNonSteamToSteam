using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddNonSteamToSteam.Core.Models
{
    public class ShortcutModel
    {
        public string AppName { get; set; } = "";
        public string Exe { get; set; } = "";        // quoted
        public string StartDir { get; set; } = "";   // ends with slash
        public string Icon { get; set; } = "";
        public string ShortcutPath { get; set; } = "";
        public string LaunchOptions { get; set; } = "";
        public bool IsHidden { get; set; }
        public bool AllowDesktopConfig { get; set; } = true;
        public bool AllowOverlay { get; set; } = true;
        public bool OpenVR { get; set; }
        public bool Devkit { get; set; }
        public string DevkitGameID { get; set; } = "";
        public string FlatpakAppID { get; set; } = "";
        public List<string> Tags { get; set; } = new();
        public uint NonSteamAppId { get; set; }       // for artwork filenames
    }
}
