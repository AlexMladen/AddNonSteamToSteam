using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddNonSteamToSteam.Core.Models
{
    public class ArtworkRequest
    {
        public uint NonSteamAppId { get; set; }
        public int? SteamAppId { get; set; }
        public string? SteamSearchName { get; set; }
    }
}
