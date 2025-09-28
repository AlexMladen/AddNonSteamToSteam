using AddNonSteamToSteam.Core.Common;
using AddNonSteamToSteam.Core.Models;

namespace AddNonSteamToSteam.Core.Services;

public class ShortcutService : IShortcutService
{
    private readonly IVdfService _vdf;
    private readonly IAppIdService _appid;

    public ShortcutService(IVdfService vdf, IAppIdService appid)
    {
        _vdf = vdf;
        _appid = appid;
    }

    public OperationResult AddOrUpdateShortcut(ShortcutRequest request, string shortcutsVdfPath, out ShortcutModel resultingShortcut)
    {
        resultingShortcut = new ShortcutModel();

        if (string.IsNullOrWhiteSpace(request.DisplayName))
            return OperationResult.Fail("Display name is required.");

        string exeQuoted, startDir, launchOptions;
        var tags = new List<string>();

        if (request.Mode == LaunchMode.Uri)
        {
            if (string.IsNullOrWhiteSpace(request.Uri) || !request.Uri.StartsWith("com.", StringComparison.OrdinalIgnoreCase))
                return OperationResult.Fail("Valid Epic/launcher URI is required in Uri mode.");

            exeQuoted = "\"C:\\\\Windows\\\\System32\\\\cmd.exe\"";
            startDir = "C:\\\\Windows\\\\System32\\\\";
            launchOptions = $"/k start {request.Uri} & exit";
            tags.Add("Uri");
        }
        else
        {
            if (string.IsNullOrWhiteSpace(request.ExePath) || !File.Exists(request.ExePath))
                return OperationResult.Fail("Valid .exe path is required in DirectExe mode.");

            exeQuoted = $"\"{request.ExePath}\"";
            var dir = Path.GetDirectoryName(request.ExePath!) ?? "";
            startDir = string.IsNullOrEmpty(dir) ? "" : (dir.EndsWith('\\') ? dir : (dir + "\\"));
            launchOptions = "";
            tags.Add("Custom");
        }

        // Load existing
        var all = _vdf.ReadShortcuts(shortcutsVdfPath).ToList();

        var existing = all.FirstOrDefault(s => s.AppName.Equals(request.DisplayName, StringComparison.OrdinalIgnoreCase));

        if (existing == null)
        {
            existing = new ShortcutModel
            {
                AppName = request.DisplayName,
                Icon = "",
                ShortcutPath = "",
                DevkitGameID = "",
                FlatpakAppID = "",
            };
            all.Add(existing);
        }
        existing.Icon = request.ExePath ?? "";
        existing.Exe = exeQuoted;
        existing.StartDir = startDir;
        existing.LaunchOptions = launchOptions;
        existing.AllowOverlay = request.AllowOverlay;
        if (!existing.Tags.Contains(tags[0])) existing.Tags.Add(tags[0]);

        // Compute Non-Steam AppID and set it (used for grid filenames)
        existing.NonSteamAppId = _appid.ComputeNonSteamAppId(existing.AppName, existing.Exe);

        var write = _vdf.WriteShortcuts(shortcutsVdfPath, all, backup: true);
        resultingShortcut = existing;
        return write;
    }
}
