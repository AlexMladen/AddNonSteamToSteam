using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;
using AddNonSteamToSteam.Core.Common;

namespace AddNonSteamToSteam.Core.Services;

public class SteamStoreService : ISteamStoreService
{
    private readonly HttpClient _http = new();

    public async Task<int?> FindSteamAppIdByNameAsync(string title)
    {
        if (string.IsNullOrWhiteSpace(title)) return null;
        try
        {
            var url = $"https://store.steampowered.com/search/results/?json=1&term={Uri.EscapeDataString(title)}&category1=998";
            var json = await _http.GetStringAsync(url).ConfigureAwait(false);
            using var doc = JsonDocument.Parse(json);
            if (!doc.RootElement.TryGetProperty("items", out var items) || items.ValueKind != JsonValueKind.Array) return null;

            int? best = null;
            foreach (var it in items.EnumerateArray())
            {
                if (it.TryGetProperty("id", out var idEl) && idEl.TryGetInt32(out var idInt))
                {
                    var name = it.TryGetProperty("name", out var nEl) ? nEl.GetString() ?? "" : "";
                    if (string.Equals(name, title, StringComparison.OrdinalIgnoreCase)) return idInt;
                    best ??= idInt;
                }
                else if (it.TryGetProperty("logo", out var logoEl))
                {
                    var m = Regex.Match(logoEl.GetString() ?? "", @"/steam/(apps|bundles)/(\d+)/", RegexOptions.IgnoreCase);
                    if (m.Success && int.TryParse(m.Groups[2].Value, out var idFromLogo))
                        best ??= idFromLogo;
                }
            }
            return best;
        }
        catch { return null; }
    }

    public Task<byte[]?> DownloadPortraitAsync(int steamAppId)
        => GetBytesOrNull($"https://cdn.cloudflare.steamstatic.com/steam/apps/{steamAppId}/library_600x900_2x.jpg");

    public Task<byte[]?> DownloadHeroAsync(int steamAppId)
        => GetBytesOrNull($"https://cdn.cloudflare.steamstatic.com/steam/apps/{steamAppId}/library_hero.jpg");

    public Task<byte[]?> DownloadLogoAsync(int steamAppId)
        => GetBytesOrNull($"https://cdn.cloudflare.steamstatic.com/steam/apps/{steamAppId}/logo.png");

    public Task<byte[]?> DownloadHeaderAsync(int steamAppId)
    => GetBytesOrNull($"https://cdn.cloudflare.steamstatic.com/steam/apps/{steamAppId}/header.jpg");

    private async Task<byte[]?> GetBytesOrNull(string url)
    {
        try { return await _http.GetByteArrayAsync(url).ConfigureAwait(false); }
        catch { return null; }
    }
    public interface ISteamStoreService
    {
        Task<int?> FindSteamAppIdByNameAsync(string title);
        Task<byte[]?> DownloadPortraitAsync(int steamAppId);
        Task<byte[]?> DownloadHeroAsync(int steamAppId);
        Task<byte[]?> DownloadLogoAsync(int steamAppId);
        Task<byte[]?> DownloadHeaderAsync(int steamAppId);
    }
}
