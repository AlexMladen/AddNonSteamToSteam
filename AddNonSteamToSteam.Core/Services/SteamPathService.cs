using Microsoft.Win32;
using AddNonSteamToSteam.Core.Common;

namespace AddNonSteamToSteam.Core.Services;

public class SteamPathService : ISteamPathService
{
    public string GetSteamRoot()
    {
        string? TryHive(string hive)
        {
            try { return Registry.GetValue(hive, "SteamPath", null) as string; }
            catch { return null; }
        }

        var candidates = new[]
        {
            @"HKEY_CURRENT_USER\Software\Valve\Steam",
            @"HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Valve\Steam",
            @"HKEY_LOCAL_MACHINE\SOFTWARE\Valve\Steam"
        };

        foreach (var h in candidates)
        {
            var p = TryHive(h);
            if (!string.IsNullOrWhiteSpace(p) && Directory.Exists(p)) return p!;
        }
        throw new DirectoryNotFoundException("SteamPath not found in registry.");
    }

    public string GetActiveUserConfigDir(string steamRoot)
    {
        var userdata = Path.Combine(steamRoot, "userdata");
        if (!Directory.Exists(userdata)) throw new DirectoryNotFoundException(userdata);

        var latest = Directory.GetDirectories(userdata)
            .Where(d => Path.GetFileName(d).All(char.IsDigit))
            .Select(d => new DirectoryInfo(d))
            .OrderByDescending(di => di.LastWriteTimeUtc)
            .FirstOrDefault() ?? throw new InvalidOperationException("No Steam user folders found.");

        var cfg = Path.Combine(latest.FullName, "config");
        Directory.CreateDirectory(cfg);
        return cfg;
    }

    public string GetShortcutsPath(string userConfigDir) => Path.Combine(userConfigDir, "shortcuts.vdf");
    public string GetGridDir(string userConfigDir)
    {
        var grid = Path.Combine(userConfigDir, "grid");
        Directory.CreateDirectory(grid);
        return grid;
    }
}
