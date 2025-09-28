using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddNonSteamToSteam.Core.Common
{
    public interface ISteamStoreService
    {
        Task<int?> FindSteamAppIdByNameAsync(string title);
        Task<byte[]?> DownloadPortraitAsync(int steamAppId); // 600x900
        Task<byte[]?> DownloadHeroAsync(int steamAppId);     // banner
        Task<byte[]?> DownloadLogoAsync(int steamAppId);
        Task<byte[]?> DownloadHeaderAsync(int steamAppId);
    }
}
